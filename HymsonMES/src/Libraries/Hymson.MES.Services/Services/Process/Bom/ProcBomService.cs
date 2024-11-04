using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// BOM表 服务
    /// </summary>
    public class ProcBomService : IProcBomService
    {

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// BOM表 仓储
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;
        private readonly AbstractValidator<ProcBomCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBomModifyDto> _validationModifyRules;

        private readonly IProcBomDetailRepository _procBomDetailRepository;
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        public ProcBomService(ICurrentSite currentSite, ICurrentUser currentUser,
            IProcBomRepository procBomRepository,
            AbstractValidator<ProcBomCreateDto> validationCreateRules,
            AbstractValidator<ProcBomModifyDto> validationModifyRules,
            IProcBomDetailRepository procBomDetailRepository,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository)
        {
            _currentSite = currentSite;
            _currentUser = currentUser;
            _procBomRepository = procBomRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procBomCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcBomAsync(ProcBomCreateDto procBomCreateDto)
        {
            procBomCreateDto.BomCode = procBomCreateDto.BomCode.ToTrimSpace().ToUpperInvariant();
            procBomCreateDto.BomName = procBomCreateDto.BomName.Trim();
            procBomCreateDto.Version = procBomCreateDto.Version.Trim();
            procBomCreateDto.Remark = procBomCreateDto.Remark ?? "".Trim();
            procBomCreateDto.Version = procBomCreateDto.Version.Trim();
            if (procBomCreateDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10503));
            }

            var list = procBomCreateDto.MaterialList.ToList();
            list.ForEach(a =>
            {
                if (string.IsNullOrWhiteSpace(a.ProcedureId))
                {
                    a.ProcedureId = "0";
                }
            });

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procBomCreateDto);

            var bomCode = procBomCreateDto.BomCode.ToUpperInvariant();
            //DTO转换实体
            var procBomEntity = procBomCreateDto.ToEntity<ProcBomEntity>();
            procBomEntity.Id = IdGenProvider.Instance.CreateId();
            procBomEntity.SiteId = _currentSite.SiteId ?? 123456;
            procBomEntity.CreatedBy = _currentUser.UserName;
            procBomEntity.UpdatedBy = _currentUser.UserName;
            procBomEntity.BomCode = bomCode;

            //验证是否存在
            var exists = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
            {
                BomCode = bomCode,
                Version = procBomEntity.Version,
                SiteId = _currentSite.SiteId ?? 123456
            });
            if (exists != null && exists.Count() > 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES10601)).WithData("bomCode", procBomEntity.BomCode).WithData("version", procBomEntity.Version);
            }

            var materialList = procBomCreateDto.MaterialList.ToList();
            var bomDetails = new List<ProcBomDetailEntity>();
            var bomReplaceDetails = new List<ProcBomDetailReplaceMaterialEntity>();
            if (materialList.Count > 0)
            {
                if (materialList.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10603));
                }
                if (materialList.Any(a => a.IsMain != 1 && string.IsNullOrWhiteSpace(a.ReplaceMaterialId)))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10604));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料编码不能为空!";
                    //return apiResult;
                }
                var mainList = materialList.Where(a => a.IsMain == 1).ToList();
                if (mainList.Any(a => string.IsNullOrWhiteSpace(a.Code) || a.ProcedureId == "0"))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10605));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"工序不能为空!";
                    //return apiResult;
                }
                if (mainList.GroupBy(m => new { m.MaterialId, m.ProcedureId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(nameof(ErrorCode.MES10606));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料编码+工序不能重复!";
                    //return apiResult;
                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10607));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料不能跟主物料重复!";
                    //return apiResult;
                }
                var replaceList = materialList.Where(a => a.IsMain == 0).ToList();
                if (replaceList.GroupBy(m => new { m.MaterialId, m.ReplaceMaterialId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(nameof(ErrorCode.MES10608));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料关联的替代物料不能重复!";
                    //return apiResult;
                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10607));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料不能跟主物料重复!";
                    //return apiResult;
                }

                long mainId = 0;
                foreach (var item in materialList)
                {
                    if (item.IsMain == 1)
                    {
                        var bomdeail = new ProcBomDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentSite.SiteId ?? 123456,
                            BomId = procBomEntity.Id,
                            MaterialId = item.MaterialId.ParseToLong(),
                            ProcedureId = item.ProcedureId.ParseToLong(),
                            ReferencePoint = item.ReferencePoint,
                            Usages = item.Usages,
                            Loss = item.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            UpdatedBy = procBomEntity.CreatedBy,

                            DataCollectionWay = item.DataCollectionWay,
                            IsEnableReplace = item.IsEnableReplace ?? false,
                            Seq = (int)item.Seq
                        };
                        mainId = bomdeail.Id;
                        bomDetails.Add(bomdeail);
                    }
                    else
                    {
                        var bomReplacedeail = new ProcBomDetailReplaceMaterialEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentSite.SiteId ?? 123456,
                            BomId = procBomEntity.Id,
                            BomDetailId = mainId,
                            ReplaceMaterialId = item.ReplaceMaterialId.ParseToLong(),
                            ReferencePoint = item.ReferencePoint,
                            Usages = item.Usages,
                            Loss = item.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            UpdatedBy = procBomEntity.CreatedBy,

                            DataCollectionWay = item.DataCollectionWay
                        };
                        bomReplaceDetails.Add(bomReplacedeail);
                    }
                }
            }

            #region 操作数据库
            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                if (procBomEntity.IsCurrentVersion)
                {
                    // 先将同编码的其他bom设置为非当前版本
                    var procBoms = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
                    {
                        SiteId = _currentSite.SiteId ?? 123456,
                        BomCode = procBomEntity.BomCode,
                    });

                    var currentVersionProcBoms = procBoms != null && procBoms.Any() ? procBoms.Where(x => x.IsCurrentVersion = true).ToList() : null;

                    if (currentVersionProcBoms != null && currentVersionProcBoms.Any())
                    {
                        await _procBomRepository.UpdateIsCurrentVersionIsFalseAsync(currentVersionProcBoms.Select(x => x.Id).ToArray());
                    }
                }

                response = await _procBomRepository.InsertAsync(procBomEntity);

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10602));
                }

                if (bomDetails.Count > 0)
                {
                    response = await _procBomDetailRepository.InsertsAsync(bomDetails);
                    if (response == 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10602));
                    }
                }
                if (bomReplaceDetails.Count > 0)
                {
                    response = await _procBomDetailReplaceMaterialRepository.InsertsAsync(bomReplaceDetails);
                    if (response == 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10602));
                    }
                }

                ts.Complete();
            }

            #endregion
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcBomAsync(long id)
        {
            await _procBomRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcBomAsync(long[] ids)
        {
            var updateBy = _currentUser.UserName;

            if (ids.Length < 1)
            {
                throw new ValidationException(nameof(ErrorCode.MES10610));
            }

            //判断需要删除的Bom是否是启用状态
            var bomList = await _procBomRepository.GetByIdsAsync(ids);
            //if (bomList.Any(x => x.Status == SysDataStatusEnum.Enable))
            //{
            //    throw new BusinessException(nameof(ErrorCode.MES10611));
            //}
            if (bomList != null && bomList.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _procBomRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = updateBy });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procBomPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDto>> GetPageListAsync(ProcBomPagedQueryDto procBomPagedQueryDto)
        {
            var procBomPagedQuery = procBomPagedQueryDto.ToQuery<ProcBomPagedQuery>();
            procBomPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _procBomRepository.GetPagedInfoAsync(procBomPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcBomDto> procBomDtos = PrepareProcBomDtos(pagedInfo);
            return new PagedInfo<ProcBomDto>(procBomDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcBomDto> PrepareProcBomDtos(PagedInfo<ProcBomEntity> pagedInfo)
        {
            var procBomDtos = new List<ProcBomDto>();
            foreach (var procBomEntity in pagedInfo.Data)
            {
                var procBomDto = procBomEntity.ToModel<ProcBomDto>();
                procBomDtos.Add(procBomDto);
            }

            return procBomDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcBomAsync(ProcBomModifyDto procBomModifyDto)
        {
            if (procBomModifyDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            procBomModifyDto.BomName = procBomModifyDto.BomName.Trim();
            procBomModifyDto.Remark = procBomModifyDto.Remark ?? "".Trim();

            var list = procBomModifyDto.MaterialList.ToList();
            list.ForEach(a =>
            {
                if (string.IsNullOrWhiteSpace(a.ProcedureId))
                {
                    a.ProcedureId = "0";
                }
            });

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procBomModifyDto);

            var siteId = _currentSite.SiteId ?? 123456;
            var user = _currentUser.UserName ?? "";
            //DTO转换实体
            var procBomEntity = procBomModifyDto.ToEntity<ProcBomEntity>();
            procBomEntity.UpdatedBy = _currentUser.UserName;

            //判断该bom是否还存在
            var modelOrigin = await _procBomRepository.GetByIdAsync(procBomEntity.Id);
            if (modelOrigin == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10610));
            }

            if (modelOrigin.Status != SysDataStatusEnum.Build && procBomEntity.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }

            var bomCode = modelOrigin.BomCode.ToUpperInvariant();
            //验证是否存在
            //var exists = (await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
            //{
            //    SiteId = siteId,
            //    BomCode = bomCode,
            //    Version = procBomEntity.Version,
            //})).Where(x => x.Id != procBomEntity.Id && x.IsDeleted == 0);
            //if (exists != null && exists.Count() > 0)
            //{
            //    throw new BusinessException(ErrorCode.MES10601).WithData("bomCode", procBomEntity.BomCode).WithData("version", procBomEntity.Version);
            //}

            var materialList = procBomModifyDto.MaterialList.ToList();
            var bomDetails = new List<ProcBomDetailEntity>();
            var bomReplaceDetails = new List<ProcBomDetailReplaceMaterialEntity>();
            if (materialList.Count > 0)
            {
                if (materialList.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10603));
                }
                if (materialList.Any(a => a.IsMain != 1 && string.IsNullOrWhiteSpace(a.ReplaceMaterialId)))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10604));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料编码不能为空!";
                    //return apiResult;
                }
                var mainList = materialList.Where(a => a.IsMain == 1).ToList();
                if (mainList.Any(a => string.IsNullOrWhiteSpace(a.Code) || a.ProcedureId == "0"))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10605));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"工序不能为空!";
                    //return apiResult;
                }
                if (mainList.GroupBy(m => new { m.MaterialId, m.ProcedureId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(nameof(ErrorCode.MES10606));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料编码+工序不能重复!";
                    //return apiResult;
                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new ValidationException(nameof(ErrorCode.MES10607));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料不能跟主物料重复!";
                    //return apiResult;
                }

                var replaceList = materialList.Where(a => a.IsMain == 0).ToList();
                if (replaceList.GroupBy(m => new { m.MaterialId, m.ReplaceMaterialId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(nameof(ErrorCode.MES10608));
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料关联的替代物料不能重复!";
                    //return apiResult;
                }

                long mainId = 0;
                foreach (var item in materialList)
                {
                    if (item.IsMain == 1)
                    {
                        var bomdeail = new ProcBomDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = siteId,
                            BomId = procBomEntity.Id,
                            MaterialId = item.MaterialId.ParseToLong(),
                            ProcedureId = item.ProcedureId.ParseToLong(),
                            ReferencePoint = item.ReferencePoint,
                            Usages = item.Usages,
                            Loss = item.Loss,
                            CreatedBy = user,
                            UpdatedBy = user,

                            DataCollectionWay = item.DataCollectionWay,
                            IsEnableReplace = item.IsEnableReplace ?? false,
                            Seq = (int)item.Seq
                        };
                        mainId = bomdeail.Id;
                        bomDetails.Add(bomdeail);
                    }
                    else
                    {
                        var bomReplacedeail = new ProcBomDetailReplaceMaterialEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = siteId,
                            BomId = procBomEntity.Id,
                            BomDetailId = mainId,
                            ReplaceMaterialId = item.ReplaceMaterialId.ParseToLong(),
                            ReferencePoint = item.ReferencePoint,
                            Usages = item.Usages,
                            Loss = item.Loss ?? 0,
                            CreatedBy = user,
                            UpdatedBy = user,

                            DataCollectionWay = item.DataCollectionWay
                        };
                        bomReplaceDetails.Add(bomReplacedeail);
                    }
                }
            }

            #region 操作数据库

            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                if (procBomEntity.IsCurrentVersion)
                {
                    // 先将同编码的其他bom设置为非当前版本
                    var procBoms = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
                    {
                        SiteId = siteId,
                        BomCode = bomCode
                    });

                    var currentVersionProcBoms = procBoms != null && procBoms.Any() ? procBoms.Where(x => x.IsCurrentVersion = true).ToList() : null;

                    if (currentVersionProcBoms != null && currentVersionProcBoms.Any())
                    {
                        await _procBomRepository.UpdateIsCurrentVersionIsFalseAsync(currentVersionProcBoms.Select(x => x.Id).ToArray());
                    }
                }

                //编码+版本唯一在修改时两者均不可修改
                response = await _procBomRepository.UpdateAsync(procBomEntity);

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10609));
                }

                DeleteCommand command = new DeleteCommand
                {
                    UserId = user,
                    Ids = new long[] { procBomEntity.Id },
                    DeleteOn = HymsonClock.Now()
                };
                await _procBomDetailRepository.DeleteBomIDAsync(command);
                await _procBomDetailReplaceMaterialRepository.DeleteBomIDAsync(command);

                if (bomDetails.Count > 0)
                {
                    response = await _procBomDetailRepository.InsertsAsync(bomDetails);

                    if (response == 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10609));
                    }
                }
                if (bomReplaceDetails.Count > 0)
                {
                    response = await _procBomDetailReplaceMaterialRepository.InsertsAsync(bomReplaceDetails);
                    if (response == 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10609));
                    }
                }

                ts.Complete();
            }

            #endregion
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDto> QueryProcBomByIdAsync(long id)
        {
            var procBomEntity = await _procBomRepository.GetByIdAsync(id);
            if (procBomEntity != null)
            {
                return procBomEntity.ToModel<ProcBomDto>();
            }
            return null;
        }

        /// <summary>
        /// 根据ID查询Bom 详情
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<List<ProcBomDetailView>> GetProcBomMaterialAsync(long bomId)
        {
            var procBomDetailViews = new List<ProcBomDetailView>();
            var mainBomDetails = await _procBomDetailRepository.GetListMainAsync(bomId);
            var replaceBomDetails = await _procBomDetailRepository.GetListReplaceAsync(bomId);

            if (mainBomDetails.Count() > 0)
            {
                mainBomDetails = mainBomDetails.OrderBy(x => x.Seq).ToList();

                procBomDetailViews.AddRange(mainBomDetails);
            }
            if (replaceBomDetails.Count() > 0)
            {
                procBomDetailViews.AddRange(replaceBomDetails);
            }

            return procBomDetailViews;
        }
    }
}
