using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPointLink.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Common;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 上料点表 服务
    /// </summary>
    public class ProcLoadPointService : IProcLoadPointService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 上料点表 仓储
        /// </summary>
        private readonly IProcLoadPointRepository _procLoadPointRepository;
        private readonly AbstractValidator<ProcLoadPointCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcLoadPointModifyDto> _validationModifyRules;
        private readonly AbstractValidator<ImportLoadPointDto> _validationImportRules;

        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        private readonly IManuFeedingRepository _manuFeedingRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="procLoadPointRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procLoadPointLinkMaterialRepository"></param>
        /// <param name="procLoadPointLinkResourceRepository"></param>
        /// <param name="currentSite"></param>
        /// <param name="minioService"></param>
        /// <param name="excelService"></param>
        /// <param name="localizationService"></param>
        /// <param name="validationImportRules"></param>
        /// <param name="manuFeedingRepository"></param>
        public ProcLoadPointService(ICurrentUser currentUser, IMinioService minioService, IExcelService excelService, IProcLoadPointRepository procLoadPointRepository, AbstractValidator<ProcLoadPointCreateDto> validationCreateRules, AbstractValidator<ProcLoadPointModifyDto> validationModifyRules, AbstractValidator<ImportLoadPointDto> validationImportRules, IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository, IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository, ICurrentSite currentSite, ILocalizationService localizationService, IManuFeedingRepository manuFeedingRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procLoadPointRepository = procLoadPointRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _validationImportRules = validationImportRules;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;

            _manuFeedingRepository = manuFeedingRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLoadPointCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcLoadPointAsync(ProcLoadPointCreateDto procLoadPointCreateDto)
        {
            if (procLoadPointCreateDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            PrepareProcLoadPointCreateDto(procLoadPointCreateDto);


            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procLoadPointCreateDto);
            if (procLoadPointCreateDto.LinkMaterials != null)
            {
                if (!procLoadPointCreateDto.LinkMaterials.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointCreateDto.LinkMaterials.Any(a => a.MaterialId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointCreateDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointCreateDto.LinkMaterials.GroupBy(x => x.MaterialId).Any(g => g.Count() >= 2)) throw new CustomerValidationException(nameof(ErrorCode.MES10710));
            }

            if (procLoadPointCreateDto.LinkResources != null && procLoadPointCreateDto.LinkResources.Any())
            {
                //if (!procLoadPointCreateDto.LinkResources.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointCreateDto.LinkResources.Any(a => a.ResourceId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointCreateDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointCreateDto.LinkResources.GroupBy(x => x.ResourceId).Any(g => g.Count() >= 2)) throw new CustomerValidationException(nameof(ErrorCode.MES10711));
            }

            // DTO转换实体
            var procLoadPointEntity = procLoadPointCreateDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.Id = IdGenProvider.Instance.CreateId();
            procLoadPointEntity.CreatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.CreatedOn = HymsonClock.Now();
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();
            procLoadPointEntity.SiteId = _currentSite.SiteId ?? 0;

            procLoadPointEntity.Status = SysDataStatusEnum.Build;

            #region 数据库验证
            var isExists = (await _procLoadPointRepository.GetProcLoadPointEntitiesAsync(new ProcLoadPointQuery()
            {
                SiteId = procLoadPointEntity.SiteId,
                LoadPoint = procLoadPointEntity.LoadPoint
            })).Any();
            if (isExists)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10701)).WithData("LoadPoint", procLoadPointEntity.LoadPoint);
            }

            #endregion

            #region 准备数据

            var validationFailures = new List<ValidationFailure>();
            //上料点关联物料列表
            var linkMaterials = new List<ProcLoadPointLinkMaterialEntity>();
            if (procLoadPointCreateDto.LinkMaterials != null && procLoadPointCreateDto.LinkMaterials.Any())
            {
                int i = 0;
                foreach (var material in procLoadPointCreateDto.LinkMaterials)
                {
                    i++;
                    if (material.MaterialId <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10718);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    linkMaterials.Add(new ProcLoadPointLinkMaterialEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        MaterialId = material.MaterialId,
                        Version = material.Version ?? "",
                        ReferencePoint = material.ReferencePoint ?? "",
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }

            //上料点关联资源列表
            var linkResources = new List<ProcLoadPointLinkResourceEntity>();
            if (procLoadPointCreateDto.LinkResources != null && procLoadPointCreateDto.LinkResources.Any())
            {
                int i = 0;
                foreach (var resource in procLoadPointCreateDto.LinkResources.Select(x => x.ResourceId))
                {
                    i++;
                    if (resource <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10719);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    linkResources.Add(new ProcLoadPointLinkResourceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        ResourceId = resource.ParseToLong(),
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            int response = 0;
            // 入库
            response = await _procLoadPointRepository.InsertAsync(procLoadPointEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10704));
            }

            if (linkMaterials.Count > 0)
            {
                response = await _procLoadPointLinkMaterialRepository.InsertsAsync(linkMaterials);
                if (response <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10704));
                }
            }
            if (linkResources.Count > 0)
            {
                await _procLoadPointLinkResourceRepository.InsertsAsync(linkResources);
                if (response <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10704));
                }
            }
            trans.Complete();

            return procLoadPointEntity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcLoadPointAsync(ProcLoadPointModifyDto procLoadPointModifyDto)
        {
            if (procLoadPointModifyDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            procLoadPointModifyDto.LoadPointName = procLoadPointModifyDto.LoadPointName.Trim();
            procLoadPointModifyDto.Remark = procLoadPointModifyDto?.Remark ?? "".Trim();

            // 验证DTO
            await _validationModifyRules!.ValidateAndThrowAsync(procLoadPointModifyDto);
            if (procLoadPointModifyDto!.LinkMaterials != null)
            {
                if (!procLoadPointModifyDto.LinkMaterials.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointModifyDto.LinkMaterials.Any(a => a.MaterialId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointModifyDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointModifyDto.LinkMaterials.GroupBy(x => x.MaterialId).Any(g => g.Count() >= 2)) throw new CustomerValidationException(nameof(ErrorCode.MES10710));
            }

            if (procLoadPointModifyDto.LinkResources != null && procLoadPointModifyDto.LinkResources.Any())
            {
                //if (!procLoadPointModifyDto.LinkResources.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointModifyDto.LinkResources.Any(a => a.ResourceId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointModifyDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointModifyDto.LinkResources.GroupBy(x => x.ResourceId).Any(g => g.Count() >= 2)) throw new CustomerValidationException(nameof(ErrorCode.MES10711));
            }

            // DTO转换实体
            var procLoadPointEntity = procLoadPointModifyDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();
            procLoadPointEntity.SiteId = _currentSite.SiteId ?? 0;

            #region 数据库验证
            var modelOrigin = await _procLoadPointRepository.GetByIdAsync(procLoadPointModifyDto.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10705));

            // 验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == modelOrigin.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES10129));

            #region 解绑上料点
            // 验证物料加载是否有关联该上料点的资源
            var procLoadPointLinkResourceEntities = await _procLoadPointLinkResourceRepository.GetProcLoadPointLinkResourceEntitiesAsync(new ProcLoadPointLinkResourceQuery
            {
                SiteId = _currentSite.SiteId,
                LoadPointId = procLoadPointModifyDto.Id
            });
            var oldLinkResourceIds = procLoadPointLinkResourceEntities.Select(x => x.ResourceId);

            // 因为上料点被多个资源引用时，即使移除了当前资源，也可以通过其他资源查询出关联的上料点，所以这里只验证仅剩一个资源时
            if (oldLinkResourceIds != null && oldLinkResourceIds.Any() && oldLinkResourceIds.Count() == 1)
            {
                // 获取到需要删除的资源
                var deleteLinkResourceIds = new List<long>();
                if (procLoadPointModifyDto.LinkResources != null && procLoadPointModifyDto.LinkResources.Any())
                {
                    var modifyResourceIds = procLoadPointModifyDto.LinkResources.Select(x => x.ResourceId);
                    deleteLinkResourceIds.AddRange(oldLinkResourceIds.Except(modifyResourceIds));
                }
                else
                {
                    deleteLinkResourceIds.AddRange(oldLinkResourceIds);
                }

                if (deleteLinkResourceIds != null && deleteLinkResourceIds.Any())
                {
                    var hasFeedings = await _manuFeedingRepository.GetByFeedingPointIdAndResourceIdsAsync(new GetByFeedingPointIdAndResourceIdsQuery
                    {
                        FeedingPointId = procLoadPointModifyDto.Id,
                        ResourceIds = deleteLinkResourceIds
                    });

                    if (hasFeedings != null && hasFeedings.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10720));
                }
            }
            #endregion

            #region 删除上料点物料
            // 验证物料加载是否有关联该上料点的物料
            var procLoadPointLinkMaterialEntities = await _procLoadPointLinkMaterialRepository.GetProcLoadPointLinkMaterialEntitiesAsync(new ProcLoadPointLinkMaterialQuery
            {
                SiteId = _currentSite.SiteId,
                LoadPointId = procLoadPointModifyDto.Id
            });
            var oldLinkMaterialIds = procLoadPointLinkMaterialEntities.Select(x => x.MaterialId);
            if (oldLinkMaterialIds != null && oldLinkMaterialIds.Any())
            {
                // 获取到需要删除的物料
                var deleteLinkMaterialIds = new List<long>();
                if (procLoadPointModifyDto.LinkMaterials != null && procLoadPointModifyDto.LinkMaterials.Any())
                {
                    var modifyMaterialIds = procLoadPointModifyDto.LinkMaterials.Select(x => x.MaterialId);
                    deleteLinkMaterialIds.AddRange(oldLinkMaterialIds.Except(modifyMaterialIds));
                }
                else
                {
                    deleteLinkMaterialIds.AddRange(oldLinkMaterialIds);
                }

                if (deleteLinkMaterialIds != null && deleteLinkMaterialIds.Any())
                {
                    var hasFeedings = await _manuFeedingRepository.GetByFeedingPointIdAndMaterialIdsAsync(new GetByFeedingPointIdAndMaterialIdsQuery
                    {
                        FeedingPointId = procLoadPointModifyDto.Id,
                        MaterialIds = deleteLinkMaterialIds
                    });

                    if (hasFeedings != null && hasFeedings.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10720));
                }
            }
            #endregion

            #endregion

            #region 组装数据
            var validationFailures = new List<ValidationFailure>();
            //上料点关联物料列表
            var linkMaterials = new List<ProcLoadPointLinkMaterialEntity>();
            if (procLoadPointModifyDto.LinkMaterials != null && procLoadPointModifyDto.LinkMaterials.Any())
            {
                int i = 0;
                foreach (var material in procLoadPointModifyDto.LinkMaterials)
                {
                    i++;
                    if (material.MaterialId <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i
    }
};
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10718);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    linkMaterials.Add(new ProcLoadPointLinkMaterialEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        MaterialId = material.MaterialId,
                        Version = material.Version!,
                        ReferencePoint = material.ReferencePoint!,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }

            //上料点关联资源列表
            var linkResources = new List<ProcLoadPointLinkResourceEntity>();
            if (procLoadPointModifyDto.LinkResources != null && procLoadPointModifyDto.LinkResources.Any())
            {
                int i = 0;
                foreach (var resource in procLoadPointModifyDto.LinkResources.Select(x => x.ResourceId))
                {
                    i++;
                    if (resource <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10719);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    linkResources.Add(new ProcLoadPointLinkResourceEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = procLoadPointEntity.SiteId,
                        LoadPointId = procLoadPointEntity.Id,
                        ResourceId = resource,
                        CreatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now()
                    });
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }
            #endregion

            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 入库
            rows = await _procLoadPointRepository.UpdateAsync(procLoadPointEntity);
            if (rows == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10706));

            await _procLoadPointLinkMaterialRepository.DeletesByLoadPointIdTrueAsync(new long[] { procLoadPointEntity.Id });
            if (linkMaterials.Count > 0)
            {
                rows = await _procLoadPointLinkMaterialRepository.InsertsAsync(linkMaterials);
                if (rows <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES10706));
            }

            await _procLoadPointLinkResourceRepository.DeletesByLoadPointIdTrueAsync(new long[] { procLoadPointEntity.Id });
            if (linkResources.Count > 0)
            {
                await _procLoadPointLinkResourceRepository.InsertsAsync(linkResources);
                if (rows <= 0) throw new CustomerValidationException(nameof(ErrorCode.MES10706));
            }

            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcLoadPointAsync(long id)
        {
            await _procLoadPointRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcLoadPointAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10707));
            }

            var loadPoints = await _procLoadPointRepository.GetByIdsAsync(idsArr);

            if (loadPoints != null && loadPoints.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            int response = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {

                response = await _procLoadPointRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
                if (response <= 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10708));
                }

                await _procLoadPointLinkMaterialRepository.DeletesByLoadPointIdTrueAsync(idsArr);

                await _procLoadPointLinkResourceRepository.DeletesByLoadPointIdTrueAsync(idsArr);

                ts.Complete();
            }
            return response;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procLoadPointPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointDto>> GetPageListAsync(ProcLoadPointPagedQueryDto procLoadPointPagedQueryDto)
        {
            var procLoadPointPagedQuery = procLoadPointPagedQueryDto.ToQuery<ProcLoadPointPagedQuery>();
            procLoadPointPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procLoadPointRepository.GetPagedInfoAsync(procLoadPointPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcLoadPointDto> procLoadPointDtos = PrepareProcLoadPointDtos(pagedInfo);
            return new PagedInfo<ProcLoadPointDto>(procLoadPointDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcLoadPointDto> PrepareProcLoadPointDtos(PagedInfo<ProcLoadPointEntity> pagedInfo)
        {
            var procLoadPointDtos = new List<ProcLoadPointDto>();
            foreach (var procLoadPointEntity in pagedInfo.Data)
            {
                var procLoadPointDto = procLoadPointEntity.ToModel<ProcLoadPointDto>();
                procLoadPointDtos.Add(procLoadPointDto);
            }

            return procLoadPointDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointDetailDto> QueryProcLoadPointByIdAsync(long id)
        {
            var procLoadPointEntity = await _procLoadPointRepository.GetByIdAsync(id);
            if (procLoadPointEntity == null)
            {
                return new ProcLoadPointDetailDto();

            }

            ProcLoadPointDetailDto loadPointDto = new ProcLoadPointDetailDto
            {
                Id = procLoadPointEntity.Id,
                SiteId = procLoadPointEntity.SiteId,
                LoadPoint = procLoadPointEntity.LoadPoint,
                LoadPointName = procLoadPointEntity.LoadPointName,
                Status = procLoadPointEntity.Status,
                Remark = procLoadPointEntity.Remark,
                LinkMaterials = new List<ProcLoadPointLinkMaterialViewDto>(),
                LinkResources = new List<ProcLoadPointLinkResourceViewDto>()
            };

            //上料点关联物料
            var loadPointLinkMaterials = await _procLoadPointLinkMaterialRepository.GetLoadPointLinkMaterialAsync(new long[] { id });
            if (loadPointLinkMaterials != null && loadPointLinkMaterials.Any())
            {
                loadPointDto.LinkMaterials = PrepareEntityToDto<ProcLoadPointLinkMaterialView, ProcLoadPointLinkMaterialViewDto>(loadPointLinkMaterials);
            }

            //上料点关联资源
            var loadPointLinkResources = await _procLoadPointLinkResourceRepository.GetLoadPointLinkResourceAsync(new long[] { id });
            if (loadPointLinkResources != null && loadPointLinkResources.Any())
            {
                loadPointDto.LinkResources = PrepareEntityToDto<ProcLoadPointLinkResourceView, ProcLoadPointLinkResourceViewDto>(loadPointLinkResources);
            }

            return loadPointDto;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="procLoadPointCreateDto"></param>
        private static void PrepareProcLoadPointCreateDto(ProcLoadPointCreateDto procLoadPointCreateDto)
        {
            procLoadPointCreateDto.LoadPoint = procLoadPointCreateDto.LoadPoint.ToTrimSpace().ToUpperInvariant();
            procLoadPointCreateDto.LoadPointName = procLoadPointCreateDto.LoadPointName.Trim();
            procLoadPointCreateDto.Remark = procLoadPointCreateDto?.Remark ?? "".Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static List<ToT> PrepareEntityToDto<SourceT, ToT>(IEnumerable<SourceT> sources) where SourceT : BaseEntity
            where ToT : BaseEntityDto
        {
            var toTs = new List<ToT>();
            foreach (var source in sources)
            {
                var toT = source.ToModel<ToT>();
                toTs.Add(toT);
            }

            return toTs;
        }

        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _procLoadPointRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10705));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procLoadPointRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ImportLoadPointDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "上料点导入模板");
        }

        /// <summary>
        /// 导入上料点录入表格
        /// </summary>
        /// <returns></returns>
        public async Task ImportLoadPointAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportLoadPointDto>(memoryStream);

            /*
            // 备份用户上传的文件，可选
            var stream = formFile.OpenReadStream();
            var uploadResult = await _minioService.PutObjectAsync(formFile.FileName, stream, formFile.ContentType);
            */

            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException("导入数据为空");
            }
            ExcelCheck excelCheck = new ExcelCheck();
            // 读取Excel第一行的值
            var firstRowValues = await excelCheck.ReadFirstRowAsync(formFile);
            // 获取Excel模板的值
            var columnHeaders = excelCheck.GetColumnHeaders<ImportLoadPointDto>();
            // 校验
            if (firstRowValues != columnHeaders)
            {
                throw new CustomerValidationException("批量导入时使用错误模板提示请安模板导入数据");
            }
            #region 验证基础数据
            var validationFailures = new List<ValidationFailure>();
            var rows = 1;
            foreach (var item in excelImportDtos)
            {
                var validationResult = await _validationImportRules!.ValidateAsync(item);
                if (!validationResult.IsValid && validationResult.Errors != null && validationResult.Errors.Any())
                {
                    foreach (var validationFailure in validationResult.Errors)
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", rows);
                        validationFailures.Add(validationFailure);
                    }
                }
                rows++;
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }
            #endregion

            #region 验证导入数据是否存在重复
            var repeats = new List<string>();
            var hasDuplicates = excelImportDtos.GroupBy(x => new { x.LoadPoint });

            foreach (var item in hasDuplicates)
            {
                if (item.Count() > 1)
                {
                    repeats.Add($@"[{item.Key.LoadPoint}]");
                }
            }
            if (repeats.Any())
            {
                throw new CustomerValidationException("上料点{repeats}重复").WithData("repeats", string.Join(",", repeats));
            }

            List<ProcLoadPointEntity> loadPointList = new();
            #endregion

            #region  验证数据库中是否存在数据，且组装数据
            var currentRow = 0;
            foreach (var item in excelImportDtos)
            {
                currentRow++;
                var loadPoints = await _procLoadPointRepository.GetProcLoadPointEntitiesAsync(new ProcLoadPointQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    LoadPoint = item.LoadPoint
                });

                if (loadPoints.Any(x => x.LoadPoint == item.LoadPoint))
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10701), item.LoadPoint, currentRow, "LoadPoint"));
                }

                if (!loadPoints.Any())
                {
                    var loadPointEntity = new ProcLoadPointEntity()
                    {
                        LoadPoint = item.LoadPoint,
                        LoadPointName = item.LoadPointName,
                        Remark = item.Remark,
                        Status = SysDataStatusEnum.Build,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    loadPointList.Add(loadPointEntity);
                }
            }
            #endregion
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("第{0}行"), validationFailures);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {

                //保存记录 
                if (loadPointList.Any())
                    await _procLoadPointRepository.InsertsAsync(loadPointList);
                ts.Complete();
            }
        }

        /// <summary>
        /// 获取验证对象
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="codeFormattedMessage"></param>
        /// <param name="cuurrentRow"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, int cuurrentRow = 1, string key = "code")
        {
            var validationFailure = new ValidationFailure
            {
                ErrorCode = errorCode
            };
            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
            {
                { "CollectionIndex", cuurrentRow },
                { key, codeFormattedMessage }
            };
            return validationFailure;
        }

        /// <summary>
        /// 根据查询条件导出上料点数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<LoadPointExportResultDto> ExprotLoadPointPageListAsync(ProcLoadPointPagedQueryDto param)
        {
            var pagedQuery = param.ToQuery<ProcLoadPointPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _procLoadPointRepository.GetPagedInfoAsync(pagedQuery);


            //实体到DTO转换 装载数据
            List<ExportLoadPointDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("LoadPointInfo"), _localizationService.GetResource("LoadPointInfo"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new LoadPointExportResultDto
                {
                    FileName = _localizationService.GetResource("LoadPointInfo"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new ExportLoadPointDto()
                {
                    LoadPoint = item.LoadPoint ?? "",
                    LoadPointName = item.LoadPointName ?? "",
                    Status = item.Status.GetDescription()
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("LoadPointInfo"), _localizationService.GetResource("LoadPointInfo"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new LoadPointExportResultDto
            {
                FileName = _localizationService.GetResource("LoadPointInfo"),
                Path = uploadResult.AbsoluteUrl,
            };

        }

    }
}
