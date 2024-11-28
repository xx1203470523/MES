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
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.HttpClients;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Common;
using Hymson.MES.Services.Services.Plan.PlanWorkOrder;
using Hymson.Minio;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Minio.DataModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        private readonly AbstractValidator<ImportBomDto> _validationImportRules;
        private readonly AbstractValidator<ProcBomModifyDto> _validationModifyRules;

        private readonly IProcBomDetailRepository _procBomDetailRepository;
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        private readonly ILocalizationService _localizationService;
        private readonly IExcelService _excelService;
        private readonly IMinioService _minioService;

        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IManuRequistionOrderRepository _manuRequistionOrderRepository;
        private readonly IManuRequistionOrderDetailRepository _manuRequistionOrderDetailRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;

        private readonly IWMSApiClient _wmsRequest;

        private readonly IPlanWorkOrderService _planWorkOrderService; 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="validationImportRules"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="excelService"></param>
        /// <param name="minioService"></param>
        /// <param name="procBomDetailReplaceMaterialRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="manuRequistionOrderDetailRepository"></param>
        /// <param name="manuRequistionOrderRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        public ProcBomService(ICurrentSite currentSite, ICurrentUser currentUser,
            IProcBomRepository procBomRepository,
            AbstractValidator<ProcBomCreateDto> validationCreateRules,
            AbstractValidator<ProcBomModifyDto> validationModifyRules,
            AbstractValidator<ImportBomDto> validationImportRules,
            IProcBomDetailRepository procBomDetailRepository,
            IExcelService excelService,
            IMinioService minioService,
            IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository,
            ILocalizationService localizationService,
            IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository,
            IManuRequistionOrderRepository manuRequistionOrderRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository,
            IWMSApiClient wmsRequest,
            IPlanWorkOrderService planWorkOrderService)
        {
            _wmsRequest = wmsRequest;
            _currentSite = currentSite;
            _currentUser = currentUser;
            _procBomRepository = procBomRepository;
            _validationCreateRules = validationCreateRules;
            _validationImportRules = validationImportRules;
            _validationModifyRules = validationModifyRules;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
            _localizationService = localizationService;
            _excelService = excelService;
            _minioService = minioService;

            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
            _planWorkOrderService = planWorkOrderService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procBomCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcBomAsync(ProcBomCreateDto procBomCreateDto)
        {
            procBomCreateDto.BomCode = procBomCreateDto.BomCode.ToTrimSpace().ToUpperInvariant();
            procBomCreateDto.BomName = procBomCreateDto.BomName.Trim();
            procBomCreateDto.Version = procBomCreateDto.Version.Trim();
            procBomCreateDto.Remark = procBomCreateDto.Remark ?? "".Trim();
            procBomCreateDto.Version = procBomCreateDto.Version.Trim();

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
            procBomEntity.SiteId = _currentSite.SiteId ?? 0;
            procBomEntity.CreatedBy = _currentUser.UserName;
            procBomEntity.UpdatedBy = _currentUser.UserName;
            procBomEntity.BomCode = bomCode;

            procBomEntity.Status = SysDataStatusEnum.Build;

            //验证是否存在
            var exists = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
            {
                BomCode = bomCode,
                Version = procBomEntity.Version,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (exists != null && exists.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10601)).WithData("bomCode", procBomEntity.BomCode).WithData("version", procBomEntity.Version);
            }

            var materialList = procBomCreateDto.MaterialList.ToList();
            var bomDetails = new List<ProcBomDetailEntity>();
            var bomReplaceDetails = new List<ProcBomDetailReplaceMaterialEntity>();
            if (materialList.Count > 0)
            {
                if (materialList.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10603));
                }
                if (materialList.Any(a => a.IsMain != 1 && string.IsNullOrWhiteSpace(a.ReplaceMaterialId)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10604));
                }
                var mainList = materialList.Where(a => a.IsMain == 1).ToList();
                if (mainList.Any(a => string.IsNullOrWhiteSpace(a.Code) || a.ProcedureId == "0"))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10605));

                }
                if (mainList.GroupBy(m => new { m.MaterialId, m.ProcedureId }).Any(g => g.Count() > 1))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10606));
                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10607));

                }
                var replaceList = materialList.Where(a => a.IsMain == 0).ToList();
                if (replaceList.GroupBy(m => new { m.MaterialId, m.ReplaceMaterialId }).Any(g => g.Count() > 1))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10608));

                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10607));

                }

                long mainId = 0;
                foreach (var item in materialList)
                {
                    if (item.IsMain == 1)
                    {
                        var bomdeail = new ProcBomDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentSite.SiteId ?? 0,
                            BomId = procBomEntity.Id,
                            MaterialId = item.MaterialId.ParseToLong(),
                            ProcedureId = item.ProcedureId.ParseToLong(),
                            ReferencePoint = item.ReferencePoint!,
                            Usages = item.Usages,
                            Loss = item.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            UpdatedBy = procBomEntity.CreatedBy,
                            BomProductType = item.BomProductType,
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
                            SiteId = _currentSite.SiteId ?? 0,
                            BomId = procBomEntity.Id,
                            BomDetailId = mainId,
                            ReplaceMaterialId = item.ReplaceMaterialId!.ParseToLong(),
                            ReferencePoint = item.ReferencePoint!,
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
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                int response = 0;
                if (procBomEntity.IsCurrentVersion)
                {
                    // 先将同编码的其他bom设置为非当前版本
                    var procBoms = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
                    {
                        SiteId = _currentSite.SiteId ?? 0,
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
                    throw new CustomerValidationException(nameof(ErrorCode.MES10602));
                }

                if (bomDetails.Count > 0)
                {
                    response = await _procBomDetailRepository.InsertsAsync(bomDetails);
                    if (response == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10602));
                    }
                }
                if (bomReplaceDetails.Count > 0)
                {
                    response = await _procBomDetailReplaceMaterialRepository.InsertsAsync(bomReplaceDetails);
                    if (response == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10602));
                    }
                }

                ts.Complete();
            }

            return procBomEntity.Id;
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
                throw new CustomerValidationException(nameof(ErrorCode.MES10610));
            }

            //判断需要删除的Bom是否是启用状态
            var bomList = await _procBomRepository.GetByIdsAsync(ids);
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
            procBomPagedQuery.SiteId = _currentSite.SiteId ?? 0;
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

            var siteId = _currentSite.SiteId ?? 0;
            var user = _currentUser.UserName ?? "";
            //DTO转换实体
            var procBomEntity = procBomModifyDto.ToEntity<ProcBomEntity>();
            procBomEntity.UpdatedBy = _currentUser.UserName;

            //判断该bom是否还存在
            var modelOrigin = await _procBomRepository.GetByIdAsync(procBomEntity.Id);
            if (modelOrigin == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10612));
            }

            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == modelOrigin.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            var bomCode = modelOrigin.BomCode.ToUpperInvariant();

            var materialList = procBomModifyDto.MaterialList.ToList();
            var bomDetails = new List<ProcBomDetailEntity>();
            var bomReplaceDetails = new List<ProcBomDetailReplaceMaterialEntity>();
            if (materialList.Count > 0)
            {
                if (materialList.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10603));
                }
                if (materialList.Any(a => a.IsMain != 1 && string.IsNullOrWhiteSpace(a.ReplaceMaterialId)))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10604));

                }
                var mainList = materialList.Where(a => a.IsMain == 1).ToList();
                if (mainList.Any(a => string.IsNullOrWhiteSpace(a.Code) || a.ProcedureId == "0"))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10605));

                }
                if (mainList.GroupBy(m => new { m.MaterialId, m.ProcedureId }).Any(g => g.Count() > 1))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10606));

                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10607));

                }

                var replaceList = materialList.Where(a => a.IsMain == 0).ToList();
                if (replaceList.GroupBy(m => new { m.MaterialId, m.ReplaceMaterialId }).Any(g => g.Count() > 1))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10608));

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
                            ReferencePoint = item.ReferencePoint!,
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
                            ReplaceMaterialId = item.ReplaceMaterialId!.ParseToLong(),
                            ReferencePoint = item.ReferencePoint!,
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

            #region 当BOM被工单引用并激活时 
            //需要做一些判断：不允许增删BOM的行项目数，顺序，不允许修改行项目中的物料编码和工序编码; (替代物料不做顺序考虑)
            if (await JudgeBomIsReferencedByActivatedWorkOrder(procBomEntity.Id))
            {
                //获取到所有的旧bom的物料清单
                var oldMainBomDetails = await _procBomDetailRepository.GetListMainAsync(procBomEntity.Id);
                var oldReplaceBomDetails = await _procBomDetailRepository.GetListReplaceAsync(procBomEntity.Id);

                //比对总行数
                if (oldMainBomDetails.Count() != bomDetails.Count() || oldReplaceBomDetails.Count() != bomReplaceDetails.Count())
                {
                    throw new CustomerValidationException(ErrorCode.MES10621);
                }

                //比对新旧物料清单
                foreach (var item in oldMainBomDetails)
                {
                    //找到新的物料里对应的 物料+工序
                    var newBomDetail = bomDetails.Where(x => x.MaterialId == item.MaterialId && x.ProcedureId == item.ProcedureId).FirstOrDefault();
                    if (newBomDetail == null)
                    {
                        throw new CustomerValidationException(ErrorCode.MES10622);
                    }
                    if (newBomDetail.Seq != item.Seq)
                    {
                        throw new CustomerValidationException(ErrorCode.MES10623);
                    }

                    //当前旧主物料下的旧替代物料
                    var oldReplaceBomDetailsByMain = oldReplaceBomDetails.Where(x => x.BomDetailId == item.Id);

                    //当前新主物料下的新替代物料
                    var newReplaceBomDetailsByMain = bomReplaceDetails.Where(x => x.BomDetailId == newBomDetail.Id);

                    if (oldReplaceBomDetailsByMain.Count() != newReplaceBomDetailsByMain.Count())
                    {
                        throw new CustomerValidationException(ErrorCode.MES10621);
                    }

                    foreach (var replaceItem in oldReplaceBomDetailsByMain)
                    {
                        //找到新的替代物料里对应的 物料
                        var newBomDetailReplace = newReplaceBomDetailsByMain.Where(x => x.ReplaceMaterialId == replaceItem.ReplaceMaterialId).FirstOrDefault();
                        if (newBomDetailReplace == null)
                        {
                            throw new CustomerValidationException(ErrorCode.MES10622);
                        }
                    }
                }
            }
            #endregion

            #region 操作数据库

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
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
                    throw new CustomerValidationException(nameof(ErrorCode.MES10609));
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
                        throw new CustomerValidationException(nameof(ErrorCode.MES10609));
                    }
                }
                if (bomReplaceDetails.Count > 0)
                {
                    response = await _procBomDetailReplaceMaterialRepository.InsertsAsync(bomReplaceDetails);
                    if (response == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES10609));
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
            return new ProcBomDto();
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

            if (mainBomDetails.Any())
            {
                mainBomDetails = mainBomDetails.OrderBy(x => x.Seq).ToList();

                procBomDetailViews.AddRange(mainBomDetails);
            }
            if (replaceBomDetails.Any())
            {
                procBomDetailViews.AddRange(replaceBomDetails);
            }

            return procBomDetailViews;
        }

        public async Task<List<ProcBomDetailView>> GetPickBomMaterialAsync(long orderId)
        {
            var procBomDetailViews = new List<ProcBomDetailView>();

            //工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(orderId);
            //工单Bom主物料信息
            var mainBomDetails = await _procBomDetailRepository.GetListMainAsync(planWorkOrderEntity.ProductBOMId);
            //工单Bom替代物料信息
            var replaceBomDetails = await _procBomDetailRepository.GetListReplaceAsync(planWorkOrderEntity.ProductBOMId);

            if (mainBomDetails.Any())
            {
                mainBomDetails = mainBomDetails.OrderBy(x => x.Seq).ToList();
                procBomDetailViews.AddRange(mainBomDetails);
            }
            if (replaceBomDetails.Any())
            {
                procBomDetailViews.AddRange(replaceBomDetails);
            }

            //查找领料单集合
            var requistionOrderEntities = await _manuRequistionOrderRepository.GetByOrderCodeAsync(planWorkOrderEntity.Id, planWorkOrderEntity.SiteId);
            if (requistionOrderEntities.Any())
            {
                var requistionOrderDetailEntities = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(new ManuRequistionOrderDetailQuery
                {
                    SiteId = planWorkOrderEntity.SiteId,
                    RequistionOrderIds = requistionOrderEntities.Select(r => r.Id).ToArray(),
                });
                var requistionMaterialGroup = requistionOrderDetailEntities.GroupBy(r => r.MaterialId);
                foreach (var item in procBomDetailViews)
                {
                    //已领料
                    var receivedMaterialList = requistionMaterialGroup.FirstOrDefault(g => g.Key == item.MaterialId)?.ToList();
                    if (receivedMaterialList != null && receivedMaterialList.Any())
                    {
                        item.Usages = item.Usages * planWorkOrderEntity.Qty * (1 + item.Loss ?? 0) - receivedMaterialList.Sum(r => r.Qty) * item.Usages * (1 + item.Loss ?? 0);
                    }
                }
            }

            //汇总出领料明细集合
            foreach (var item in procBomDetailViews)
            {
                item.Usages = item.Usages * planWorkOrderEntity.Qty * (1 + item.Loss ?? 0);
            }
            return procBomDetailViews;
        }

        /// <summary>
        /// 工单列表，点击领料按钮，进入工单Bom领料页面，加载物料列表所调的方法
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ProcOrderBomDetailDto>> GetOrderBomMaterialAsync(PickQueryDto dto)
        {
            var procBomDetailViews = new List<ProcOrderBomDetailDto>();

            // 查询工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(dto.WorkId);

            // 查询生产计划：指定生产计划未找到,工单编码为：【{WorkOrder}】。
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);

            // 查询生产计划物料
            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = planWorkOrderEntity.SiteId,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.WorkPlanProductId ?? 0
            });

            // 查询物料
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(planWorkPlanMaterialEntities.Select(x => x.MaterialId));

            var bomIds = planWorkPlanMaterialEntities.Select(x => x.BomId.GetValueOrDefault()).Distinct().ToArray();

            // 查询bom详情
            var procBomDetailEntities = await _procBomDetailRepository.GetByIdsAsync(bomIds);

            IEnumerable<ManuRequistionOrderDetailEntity> requistionOrderDetailEntities = new List<ManuRequistionOrderDetailEntity>();
            // 查找领料单集合
            var requistionOrderEntities = await _manuRequistionOrderRepository.GetByOrderCodeAsync(planWorkOrderEntity.Id, planWorkOrderEntity.SiteId);
            if (requistionOrderEntities.Any())
            {
                requistionOrderDetailEntities = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(new ManuRequistionOrderDetailQuery
                {
                    SiteId = planWorkOrderEntity.SiteId,
                    RequistionOrderIds = requistionOrderEntities.Select(r => r.Id).ToArray(),
                });
            }


            // 根据工单ID，查询生产领料单信息（manu_requistion_order）,获取物料的已领数量
            var materialHavePickings = await _planWorkOrderService.GetPickDetailByOrderIdByScwAsync(planWorkOrderEntity.Id);

            // 遍历生产计划物料
            foreach (var planMaterial in planWorkPlanMaterialEntities)
            {
                //var mainMaterials = mainBomDetails.Where(x => x.MaterialId == material.Id);
                //var replaceMaterials = replaceBomDetails.Where(x => x.ReplaceMaterialId == material.Id);
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(f => f.Id == planMaterial.MaterialId);
                if (procMaterialEntity == null) continue;

                var procBomDetailEntitie = procBomDetailEntities.FirstOrDefault(f => f.MaterialId == planMaterial.MaterialId);
                if (procBomDetailEntitie != null && procBomDetailEntitie.BomProductType == ManuProductTypeEnum.ByProduct) continue;

                var needQty = 0M;
                if (dto.InputQty == 0)
                {
                    needQty = planMaterial.Usages * planWorkOrderEntity.Qty;
                }
                else
                {
                    needQty = planMaterial.Usages * dto.InputQty;
                }

                var receiveQty = requistionOrderDetailEntities.Where(x => x.MaterialId == planMaterial.Id).Sum(x => x.Qty);

                //2024.11.21确认：1.待领料数量 = 需求数量 - 已领料数量。【工单列表，点击领料按钮，进入工单Bom领料页面，加载物料列表所调的方法】
                //2.需求数量 = 子件单件用量 *（1 + 子件损耗 / 100）*生产工单数量。
                //3.已领料数量 = 根据当前工单和当前物料计算已领料数量之和，领料状态需除去申请取消的

                //赋值物料的需求数量【需求数量 = 子件单件用量 *（1 + 子件损耗 / 100）* 生产工单数量。】
                var materialNeedQty = 0M;
                if (procBomDetailEntitie != null)
                {
                    materialNeedQty = (decimal)(procBomDetailEntitie.Usages * (1 + procBomDetailEntitie.Loss / 100) * planWorkOrderEntity.Qty);
                }


                //赋值物料的已领料数量【已领料数量 = 根据当前工单和当前物料计算已领料数量之和，领料状态需除去申请取消的】

                var havePickingQty = 0M;
                if (materialHavePickings != null && materialHavePickings.Any())
                {
                    foreach (var materialHavePicking in materialHavePickings)
                    {
                        if (materialHavePicking.MaterialCode == procMaterialEntity.MaterialCode)
                        {
                            havePickingQty = materialHavePicking.Qty;
                            break;
                        }
                    }
                }

                //赋值物料的待领料数量【待领料数量 = 需求数量 - 已领料数量】
                var waitPickingQty = materialNeedQty - havePickingQty;
                if (waitPickingQty < 0)
                {
                    waitPickingQty = 0;
                }

                procBomDetailViews.Add(new ProcOrderBomDetailDto
                {
                    MaterialId = procMaterialEntity.Id,
                    MaterialCode = procMaterialEntity.MaterialCode,
                    MaterialName = procMaterialEntity.MaterialName,
                    Version = procMaterialEntity.Version ?? "",
                    Unit = procMaterialEntity.Unit ?? "",
                    Usages = needQty - receiveQty,
                    MaterialNeedQty = materialNeedQty,
                    WaitPickingQty = waitPickingQty,
                    HavePickingQty = havePickingQty,
                    BomId = planMaterial?.BomId ?? 0,
                    Batch = procMaterialEntity.Batch ?? 0
                });
            }

            return procBomDetailViews;
        }




        /// <summary>
        /// 读取工单物料清单,工单Bom领料页面，切换选择仓库后，根据工单id和仓库编码获取物料列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<ProcOrderBomDetailDto>> GetOrderBomMaterialAsyncByWarehouse(PickQueryDto dto)
        {
            var procBomDetailViews = new List<ProcOrderBomDetailDto>();

            // 查询工单信息
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(dto.WorkId);

            // 查询生产计划
            var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16052)).WithData("WorkOrder", planWorkOrderEntity.OrderCode);

            // 查询生产计划物料
            var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
            {
                SiteId = planWorkOrderEntity.SiteId,
                PlanId = planWorkPlanEntity.Id,
                PlanProductId = planWorkOrderEntity.WorkPlanProductId ?? 0
            });

            // 查询物料
            var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(planWorkPlanMaterialEntities.Select(x => x.MaterialId));

            var bomIds = planWorkPlanMaterialEntities.Select(x => x.BomId.GetValueOrDefault()).Distinct().ToArray();

            // 查询bom详情
            var procBomDetailEntities = await _procBomDetailRepository.GetByIdsAsync(bomIds);

            IEnumerable<ManuRequistionOrderDetailEntity> requistionOrderDetailEntities = new List<ManuRequistionOrderDetailEntity>();
            // 查找领料单集合
            var requistionOrderEntities = await _manuRequistionOrderRepository.GetByOrderCodeAsync(planWorkOrderEntity.Id, planWorkOrderEntity.SiteId);
            if (requistionOrderEntities.Any())
            {
                requistionOrderDetailEntities = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(new ManuRequistionOrderDetailQuery
                {
                    SiteId = planWorkOrderEntity.SiteId,
                    RequistionOrderIds = requistionOrderEntities.Select(r => r.Id).ToArray(),
                });
            }


            // 根据工单ID，查询生产领料单信息（manu_requistion_order）,获取物料的已领数量
            var materialHavePickings = await _planWorkOrderService.GetPickDetailByOrderIdByScwAsync(planWorkOrderEntity.Id);

            // 遍历生产计划物料
            foreach (var planMaterial in planWorkPlanMaterialEntities)
            {
                //var mainMaterials = mainBomDetails.Where(x => x.MaterialId == material.Id);
                //var replaceMaterials = replaceBomDetails.Where(x => x.ReplaceMaterialId == material.Id);
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(f => f.Id == planMaterial.MaterialId);
                if (procMaterialEntity == null) continue;

                var procBomDetailEntitie = procBomDetailEntities.FirstOrDefault(f => f.MaterialId == planMaterial.MaterialId);
                if (procBomDetailEntitie != null && procBomDetailEntitie.BomProductType == ManuProductTypeEnum.ByProduct) continue;

                var needQty = 0M;
                if (dto.InputQty == 0)
                {
                    needQty = planMaterial.Usages * planWorkOrderEntity.Qty;
                }
                else
                {
                    needQty = planMaterial.Usages * dto.InputQty;
                }

                var receiveQty = requistionOrderDetailEntities.Where(x => x.MaterialId == planMaterial.Id).Sum(x => x.Qty);

                //如果不传库存，则剩余数量是空；如果传库存，则剩余数量取WMS返回过来的值
                string QuantityResidue = null;
                if (!string.IsNullOrEmpty(dto.warehouse))
                {
                    //根据库存和物料编码，调WMS接口，获取库存的剩余数量
                    var response = await _wmsRequest.GetStockQuantityRequestAsync(new HttpClients.Requests.GetStockQuantityDto
                    {
                        MaterialCode = procMaterialEntity.MaterialCode,
                        WarehouseCode = dto.warehouse
                    });
                    if (response != null && response.Code == 0)
                    {
                        var number = response.Data.Quantity;
                        if (number != 0)
                        {
                            //正则表达式，转换库存的可用数，赋值库存的可用数【切换选择仓库】
                            QuantityResidue = Regex.Replace(number.ToString(), @"\.?0+$", "");
                        }
                        else
                        {
                            QuantityResidue = "0";
                        }
                    }
                    else
                    {
                        QuantityResidue = "0";
                    }
                }



                //2024.11.21确认：1.待领料数量 = 需求数量 - 已领料数量。【读取工单物料清单,工单Bom领料页面，切换选择仓库后，根据工单id和仓库编码获取物料列表】
                //2.需求数量 = 子件单件用量 *（1 + 子件损耗 / 100）*生产工单数量。
                //3.已领料数量 = 根据当前工单和当前物料计算已领料数量之和，领料状态需除去申请取消的

                //赋值物料的需求数量【需求数量 = 子件单件用量 *（1 + 子件损耗 / 100）* 生产工单数量。】
                var materialNeedQty = 0M;
                if (procBomDetailEntitie != null)
                {
                    materialNeedQty = (decimal)(procBomDetailEntitie.Usages * (1 + procBomDetailEntitie.Loss / 100) * planWorkOrderEntity.Qty);
                }


                //赋值物料的已领料数量【已领料数量 = 根据当前工单和当前物料计算已领料数量之和，领料状态需除去申请取消的】

                var havePickingQty = 0M;
                if (materialHavePickings != null && materialHavePickings.Any())
                {
                    foreach (var materialHavePicking in materialHavePickings)
                    {
                        if (materialHavePicking.MaterialCode == procMaterialEntity.MaterialCode)
                        {
                            havePickingQty = materialHavePicking.Qty;
                            break;
                        }
                    }
                }

                //赋值物料的待领料数量【待领料数量 = 需求数量 - 已领料数量】
                var waitPickingQty = materialNeedQty - havePickingQty;
                if (waitPickingQty < 0)
                {
                    waitPickingQty = 0;
                }

                procBomDetailViews.Add(new ProcOrderBomDetailDto
                {
                    MaterialId = procMaterialEntity.Id,
                    MaterialCode = procMaterialEntity.MaterialCode,
                    MaterialName = procMaterialEntity.MaterialName,
                    QuantityResidue = QuantityResidue,
                    MaterialNeedQty = materialNeedQty,
                    WaitPickingQty = waitPickingQty,
                    HavePickingQty = havePickingQty,
                    Version = procMaterialEntity.Version ?? "",
                    Unit = procMaterialEntity.Unit ?? "",
                    Usages = needQty - receiveQty,
                    BomId = planMaterial?.BomId ?? 0,
                    Batch = procMaterialEntity.Batch ?? 0
                });
            }

            return procBomDetailViews;
        }

        #region 状态变更
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
            var entity = await _procBomRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10612));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procBomRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

        /// <summary>
        /// 导入Bom录入表格
        /// </summary>
        /// <returns></returns>
        public async Task ImportBomAsync(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ImportBomDto>(memoryStream);

            /*
            //备份用户上传的文件，可选
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
            var columnHeaders = excelCheck.GetColumnHeaders<ImportBomDto>();
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

            #region 检测导入Bon编码是否重复
            var repeats = new List<string>();
            var hasDuplicates = excelImportDtos.GroupBy(x => new { x.BomCode, x.Version });
            foreach (var item in hasDuplicates)
            {
                if (item.Count() > 1)
                {
                    repeats.Add($@"[{item.Key.BomCode},{item.Key.Version}]");
                }
            }
            if (repeats.Any())
            {
                throw new CustomerValidationException("Bom编码{repeats}重复").WithData("repeats", string.Join(",", repeats));
            }

            List<ProcBomEntity> importBomList = new();
            #endregion

            #region  验证数据库中是否存在数据，且组装数据

            var currentRow = 0;
            foreach (var item in excelImportDtos)
            {
                currentRow++;
                var Boms = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    BomCode = item.BomCode,
                    Version = item.Version
                });
                if (Boms.Any())
                {
                    validationFailures.Add(GetValidationFailure(nameof(ErrorCode.MES10601), item.BomCode, currentRow, "bomCode", "version"));
                }

                if (!Boms.Any())
                {
                    var BomInfoEntity = new ProcBomEntity()
                    {
                        BomCode = item.BomCode,
                        BomName = item.BomName,
                        Version = item.Version,
                        IsCurrentVersion = item.IsCurrentVersion == YesOrNoEnum.Yes,
                        Remark = item.Remark,
                        Status = SysDataStatusEnum.Build,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    };
                    importBomList.Add(BomInfoEntity);
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
                if (importBomList.Any())
                    await _procBomRepository.InsertsAsync(importBomList);
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
        /// <param name="keys"></param>
        /// 
        /// <returns></returns>
        private ValidationFailure GetValidationFailure(string errorCode, string codeFormattedMessage, int cuurrentRow = 1, string key = "code", string keys = "codes")
        {
            var validationFailure = new ValidationFailure
            {
                ErrorCode = errorCode
            };
            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object>
            {
                { "CollectionIndex", cuurrentRow },
                { key, codeFormattedMessage },
                {keys,codeFormattedMessage }
            };
            return validationFailure;
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task DownloadImportTemplateAsync(Stream stream)
        {
            var excelTemplateDtos = new List<ImportBomDto>();
            await _excelService.ExportAsync(excelTemplateDtos, stream, "Bom导入模板");
        }

        /// <summary>
        /// 根据查询条件导出Bom数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BomExportResultDto> ExprotBomPageListAsync(ProcBomPagedQuery param)
        {
            var pagedQuery = param.ToQuery<ProcBomPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            pagedQuery.PageSize = 1000;
            var pagedInfo = await _procBomRepository.GetPagedInfoAsync(param);

            //实体到DTO转换 装载数据
            List<ExportBomDto> listDto = new();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                var filePathN = await _excelService.ExportAsync(listDto, _localizationService.GetResource("BomInfo"), _localizationService.GetResource("BomInfo"));
                //上传到文件服务器
                var uploadResultN = await _minioService.PutObjectAsync(filePathN);
                return new BomExportResultDto
                {
                    FileName = _localizationService.GetResource("BomInfo"),
                    Path = uploadResultN.AbsoluteUrl,
                };
            }

            foreach (var item in pagedInfo.Data)
            {
                listDto.Add(new ExportBomDto()
                {
                    BomCode = item.BomCode ?? "",
                    BomName = item.BomName ?? "",
                    Version = item.Version ?? "",
                    Status = item.Status.GetDescription()
                });
            }

            var filePath = await _excelService.ExportAsync(listDto, _localizationService.GetResource("BomInfo"), _localizationService.GetResource("BomInfo"));
            //上传到文件服务器
            var uploadResult = await _minioService.PutObjectAsync(filePath);
            return new BomExportResultDto
            {
                FileName = _localizationService.GetResource("BomInfo"),
                Path = uploadResult.AbsoluteUrl,
            };

        }


        /// <summary>
        /// 判断bom是否被激活工单引用
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<bool> JudgeBomIsReferencedByActivatedWorkOrder(long bomId)
        {

            var planWorkOrderActivations = await _planWorkOrderActivationRepository.GetPlanWorkOrderActivationEntitiesByBomIdAsync(new PlanWorkOrderActivationByBomIdQuery { SiteId = _currentSite.SiteId ?? 0, BomId = bomId });
            if (planWorkOrderActivations.Any())
            {
                //余帅强说保留也可修改20240417
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
