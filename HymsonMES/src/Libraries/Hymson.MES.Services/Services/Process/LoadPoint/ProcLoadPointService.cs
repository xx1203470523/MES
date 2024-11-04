using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Minio.DataModel;
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

        private readonly IProcLoadPointLinkMaterialRepository _procLoadPointLinkMaterialRepository;
        private readonly IProcLoadPointLinkResourceRepository _procLoadPointLinkResourceRepository;

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
        public ProcLoadPointService(ICurrentUser currentUser, IProcLoadPointRepository procLoadPointRepository, AbstractValidator<ProcLoadPointCreateDto> validationCreateRules, AbstractValidator<ProcLoadPointModifyDto> validationModifyRules, IProcLoadPointLinkMaterialRepository procLoadPointLinkMaterialRepository, IProcLoadPointLinkResourceRepository procLoadPointLinkResourceRepository, ICurrentSite currentSite, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procLoadPointRepository = procLoadPointRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procLoadPointLinkMaterialRepository = procLoadPointLinkMaterialRepository;
            _procLoadPointLinkResourceRepository = procLoadPointLinkResourceRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLoadPointCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcLoadPointAsync(ProcLoadPointCreateDto procLoadPointCreateDto)
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
                if (procLoadPointCreateDto.LinkMaterials.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointCreateDto.LinkMaterials.Any(a => a.MaterialId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointCreateDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointCreateDto.LinkMaterials.GroupBy(x => x.MaterialId).Where(g => g.Count() >= 2).Count() >= 1) throw new CustomerValidationException(nameof(ErrorCode.MES10710));
            }

            if (procLoadPointCreateDto.LinkResources != null)
            {
                if (procLoadPointCreateDto.LinkResources.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointCreateDto.LinkResources.Any(a => a.ResourceId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointCreateDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointCreateDto.LinkResources.GroupBy(x => x.ResourceId).Where(g => g.Count() >= 2).Count() >= 1) throw new CustomerValidationException(nameof(ErrorCode.MES10711));
            }

            // DTO转换实体
            var procLoadPointEntity = procLoadPointCreateDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.Id = IdGenProvider.Instance.CreateId();
            procLoadPointEntity.CreatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.CreatedOn = HymsonClock.Now();
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();
            procLoadPointEntity.SiteId = _currentSite.SiteId ?? 123456;

            #region 数据库验证
            var isExists = (await _procLoadPointRepository.GetProcLoadPointEntitiesAsync(new ProcLoadPointQuery()
            {
                SiteId = procLoadPointEntity.SiteId,
                LoadPoint = procLoadPointEntity.LoadPoint
            })).Any();
            if (isExists == true)
            {
                throw new BusinessException(nameof(ErrorCode.MES10701)).WithData("LoadPoint", procLoadPointEntity.LoadPoint);
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
                        Version = material.Version,
                        ReferencePoint = material.ReferencePoint,
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
                foreach (var resource in procLoadPointCreateDto.LinkResources)
                {
                    i++;
                    if (resource.ResourceId <= 0)
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
                        ResourceId = resource.ResourceId.ParseToLong(),
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

            using TransactionScope trans = new TransactionScope();
            int response = 0;
            // 入库
            response = await _procLoadPointRepository.InsertAsync(procLoadPointEntity);
            if (response == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES10704));
            }

            if (linkMaterials.Count > 0)
            {
                response = await _procLoadPointLinkMaterialRepository.InsertsAsync(linkMaterials);
                if (response <= 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10704));
                }
            }
            if (linkResources.Count > 0)
            {
                await _procLoadPointLinkResourceRepository.InsertsAsync(linkResources);
                if (response <= 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10704));
                }
            }

            trans.Complete();
        }

        private static void PrepareProcLoadPointCreateDto(ProcLoadPointCreateDto procLoadPointCreateDto)
        {
            procLoadPointCreateDto.LoadPoint = procLoadPointCreateDto.LoadPoint.ToTrimSpace().ToUpperInvariant();
            procLoadPointCreateDto.LoadPointName = procLoadPointCreateDto.LoadPointName.Trim();
            procLoadPointCreateDto.Remark = procLoadPointCreateDto?.Remark ?? "".Trim();
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

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procLoadPointModifyDto);
            if (procLoadPointModifyDto.LinkMaterials != null)
            {
                if (procLoadPointModifyDto.LinkMaterials.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointModifyDto.LinkMaterials.Any(a => a.MaterialId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointModifyDto.LinkMaterials.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10702));
                if (procLoadPointModifyDto.LinkMaterials.GroupBy(x => x.MaterialId).Where(g => g.Count() >= 2).Count() >= 1) throw new CustomerValidationException(nameof(ErrorCode.MES10710));
            }

            if (procLoadPointModifyDto.LinkResources != null)
            {
                if (procLoadPointModifyDto.LinkResources.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointModifyDto.LinkResources.Any(a => a.ResourceId == 0)) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointModifyDto.LinkResources.Any(a => string.IsNullOrWhiteSpace(a.ResCode))) throw new CustomerValidationException(nameof(ErrorCode.MES10703));
                if (procLoadPointModifyDto.LinkResources.GroupBy(x => x.ResourceId).Where(g => g.Count() >= 2).Count() >= 1) throw new CustomerValidationException(nameof(ErrorCode.MES10711));
            }

            //DTO转换实体
            var procLoadPointEntity = procLoadPointModifyDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();
            procLoadPointEntity.SiteId = _currentSite.SiteId ?? 123456;

            #region 数据库验证
            var modelOrigin = await _procLoadPointRepository.GetByIdAsync(procLoadPointModifyDto.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10705));
            #endregion

            if (modelOrigin.Status != SysDataStatusEnum.Build && procLoadPointModifyDto.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10716));
            }

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
                        Version = material.Version,
                        ReferencePoint = material.ReferencePoint,
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
                foreach (var resource in procLoadPointModifyDto.LinkResources)
                {
                    i++;
                    if (resource.ResourceId <= 0)
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
                        ResourceId = resource.ResourceId,
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

            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                //入库
                response = await _procLoadPointRepository.UpdateAsync(procLoadPointEntity);

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10706));
                }

                await _procLoadPointLinkMaterialRepository.DeletesByLoadPointIdTrueAsync(new long[] { procLoadPointEntity.Id });
                if (linkMaterials.Count > 0)
                {
                    response = await _procLoadPointLinkMaterialRepository.InsertsAsync(linkMaterials);

                    if (response <= 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10706));
                    }
                }
                await _procLoadPointLinkResourceRepository.DeletesByLoadPointIdTrueAsync(new long[] { procLoadPointEntity.Id });
                if (linkResources.Count > 0)
                {
                    await _procLoadPointLinkResourceRepository.InsertsAsync(linkResources);

                    if (response <= 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10706));
                    }
                }
                ts.Complete();
            }
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
            //if (loadPoints.Any(x => (SysDataStatusEnum.Enable == x.Status || SysDataStatusEnum.Retain == x.Status)))
            //{
            //    throw new BusinessException(nameof(ErrorCode.MES10709));
            //}
            if (loadPoints != null && loadPoints.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            int response = 0;
            using (TransactionScope ts = new TransactionScope())
            {

                response = await _procLoadPointRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
                if (response <= 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10708));
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
            procLoadPointPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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

                //return procLoadPointEntity.ToModel<ProcLoadPointDto>();
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
            if (loadPointLinkMaterials != null && loadPointLinkMaterials.Count() > 0)
            {
                loadPointDto.LinkMaterials = PrepareEntityToDto<ProcLoadPointLinkMaterialView, ProcLoadPointLinkMaterialViewDto>(loadPointLinkMaterials);
            }

            //上料点关联资源
            var loadPointLinkResources = await _procLoadPointLinkResourceRepository.GetLoadPointLinkResourceAsync(new long[] { id });
            if (loadPointLinkResources != null && loadPointLinkResources.Count() > 0)
            {
                loadPointDto.LinkResources = PrepareEntityToDto<ProcLoadPointLinkResourceView, ProcLoadPointLinkResourceViewDto>(loadPointLinkResources);
            }

            return loadPointDto;
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
    }
}
