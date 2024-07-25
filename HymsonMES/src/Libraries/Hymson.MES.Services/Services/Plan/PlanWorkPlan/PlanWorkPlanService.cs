using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Common;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 服务（生产计划）
    /// </summary>
    public class PlanWorkPlanService : IPlanWorkPlanService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（生产计划）
        /// </summary>
        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        /// <summary>
        /// 仓储接口（生产计划产品）
        /// </summary>
        private readonly IPlanWorkPlanProductRepository _planWorkPlanProductRepository;

        /// <summary>
        /// 仓储接口（生产计划物料）
        /// </summary>
        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（BOM）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;

        /// <summary>
        /// 仓储接口（物料）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（工艺路线）
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sysConfigRepository"></param>
        /// <param name="planWorkPlanRepository"></param>
        /// <param name="planWorkPlanProductRepository"></param>
        /// <param name="planWorkPlanMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        public PlanWorkPlanService(ICurrentUser currentUser, ICurrentSite currentSite,
            ISysConfigRepository sysConfigRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanProductRepository planWorkPlanProductRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcBomRepository procBomRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcessRouteRepository procProcessRouteRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sysConfigRepository = sysConfigRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanProductRepository = planWorkPlanProductRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procBomRepository = procBomRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
        }


        /// <summary>
        /// 根据数量生成拆分预览（生产计划）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanSplitResponseDto>> SplitAsync(PlanWorkPlanSplitRequestDto dto)
        {
            List<PlanWorkPlanSplitResponseDto> list = new();

            // 生产计划产品
            var planProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(dto.WorkPlanProductId);
            if (planProductEntity == null) return list;

            // 生产计划
            var planEntity = await _planWorkPlanRepository.GetByIdAsync(planProductEntity.WorkPlanId);
            if (planEntity == null) return list;

            // 计算每个分段的的数量
            var remainingQty = planProductEntity.Qty;
            var partitionQty = Math.Floor(planProductEntity.Qty / dto.Count);

            // 计算每个分段的的天数
            var remainingDay = (planEntity.PlanEndTime - planEntity.PlanStartTime).TotalDays;
            var partitionDay = Math.Floor(remainingDay / dto.Count);

            for (int i = 1; i <= dto.Count; i++)
            {
                var splitDto = new PlanWorkPlanSplitResponseDto
                {
                    WorkOrderCode = $"{planEntity.WorkPlanCode}-{i}"
                };

                // 如果不是最后一条
                if (i != dto.Count)
                {
                    splitDto.Qty = partitionQty;
                    splitDto.PlanStartDate = planEntity.PlanStartTime.AddDays((i - 1) * partitionDay);
                    splitDto.PlanEndDate = planEntity.PlanStartTime.AddDays(i * partitionDay);
                }
                // 如果是最后一条
                else
                {
                    splitDto.Qty = remainingQty;
                    splitDto.PlanStartDate = planEntity.PlanStartTime.AddDays((i - 1) * partitionDay);
                    splitDto.PlanEndDate = planEntity.PlanEndTime;
                }

                // 添加到待返回
                list.Add(splitDto);

                // 扣减剩余数量
                remainingQty -= partitionQty;
            }

            // 检查间计算的数量是否等于原数量
            if (list.Sum(s => s.Qty) != planProductEntity.Qty) throw new CustomerValidationException(nameof(ErrorCode.MES16054));

            return list;
        }

        /// <summary>
        /// 生成子工单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> SaveAsync(PlanWorkPlanSaveDto dto)
        {
            // 检查生产计划是否存在
            var workPlanProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(dto.WorkPlanProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16018));

            // 检查生产计划是否存在
            var workPlanEntity = await _planWorkPlanRepository.GetByIdAsync(workPlanProductEntity.WorkPlanId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16018));

            // 检查生产计划的状态
            if (workPlanEntity.Status != PlanWorkPlanStatusEnum.NotStarted)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10247)).WithData("Status", PlanWorkPlanStatusEnum.NotStarted.GetDescription());
            }

            if (dto.Details == null || !dto.Details.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES16019));

            // 检查子工单的工单号是否有重复
            var workOrderCodes = dto.Details.Select(s => s.WorkOrderCode);
            if (workOrderCodes.Count() != workOrderCodes.Distinct().Count())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16053));
            }

            // 查看数据库是否存在相同的工单号
            var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
            {
                SiteId = workPlanEntity.SiteId,
                Codes = workOrderCodes,
            });

            // 检查子工单的总数量是否等于计划数量
            var sumQuantity = dto.Details.Sum(s => s.Qty);
            if (sumQuantity != workPlanProductEntity.Qty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16054));
            }

            // 检查子工单的计划时间是否超出生产计划的时间范围
            if (dto.Details.Any(a => a.PlanStartTime < workPlanEntity.PlanStartTime || a.PlanStartTime > workPlanEntity.PlanEndTime))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16055));
            }

            // 当前对象
            var currentBo = new BaseBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                User = "ERP",
                Time = HymsonClock.Now()
            };

            // 修改生产计划的状态
            workPlanEntity.Status = PlanWorkPlanStatusEnum.Distributed;
            workPlanEntity.UpdatedBy = currentBo.User;
            workPlanEntity.UpdatedOn = currentBo.Time;

            // 工作中心编码配置
            var workCenterConfigs = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.WorkCenterCode });
            if (workCenterConfigs == null || !workCenterConfigs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.ProcessRouteCode.GetDescription());
            }

            // 读取工作中心
            var workCenterCode = GetWorkCenterCode(workCenterConfigs, workPlanEntity.PlanType);
            var workCenterEntity = await _inteWorkCenterRepository.GetEntityAsync(new InteWorkCenterOneQuery
            {
                SiteId = workPlanEntity.SiteId,
                Code = workCenterCode
            });

            // 工艺路线编码配置
            var processRouteConfigs = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.ProcessRouteCode });
            if (processRouteConfigs == null || !processRouteConfigs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139)).WithData("name", SysConfigEnum.ProcessRouteCode.GetDescription());
            }

            // 读取工艺路线
            var processRouteCode = GetProcessRouteCode(processRouteConfigs, workPlanEntity.PlanType);
            var processRouteEntity = await _procProcessRouteRepository.GetByCodeAsync(new ProcProcessRoutesByCodeQuery
            {
                SiteId = workPlanEntity.SiteId,
                Code = processRouteCode
            });

            List<PlanWorkOrderEntity> workOrderEntites = new();
            workOrderEntites.AddRange(dto.Details.Select(s => new PlanWorkOrderEntity
            {
                OrderCode = s.WorkOrderCode,
                WorkCenterType = WorkCenterTypeEnum.Line,
                WorkCenterId = workCenterEntity?.Id,
                PlanStartTime = s.PlanStartTime,
                PlanEndTime = s.PlanEndTime,
                Qty = s.Qty,

                ProductId = workPlanProductEntity.ProductId,
                WorkPlanId = workPlanEntity.Id,
                WorkPlanProductId = workPlanProductEntity.Id,
                ProcessRouteId = processRouteEntity?.Id ?? 0,
                ProductBOMId = workPlanProductEntity.BomId,
                Type = workPlanEntity.Type,
                OverScale = workPlanProductEntity.OverScale,
                Status = PlanWorkOrderStatusEnum.NotStarted,

                Id = IdGenProvider.Instance.CreateId(),
                SiteId = currentBo.SiteId,
                CreatedBy = currentBo.User,
                CreatedOn = currentBo.Time,
                UpdatedBy = currentBo.User,
                UpdatedOn = currentBo.Time
            }));

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _planWorkPlanRepository.UpdateAsync(workPlanEntity);
            rows += await _planWorkOrderRepository.InsertsAsync(workOrderEntites);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            // 检查工单状态
            var workPlanEntities = await _planWorkPlanRepository.GetByIdsAsync(idsArr);
            if (workPlanEntities.Any(a => a.Status != PlanWorkPlanStatusEnum.NotStarted))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16017)).WithData("Status", PlanWorkPlanStatusEnum.NotStarted.GetDescription());
            }

            return await _planWorkPlanRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPlanProductDto>> GetPageListAsync(PlanWorkPlanProductPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<PlanWorkPlanProductPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _planWorkPlanProductRepository.GetPagedInfoAsync(pagedQuery);

            // 读取生产计划
            var planEntities = await _planWorkPlanRepository.GetByIdsAsync(pagedInfo.Data.Select(s => s.WorkPlanId));

            // 读取产品
            var productEntities = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(s => s.ProductId));

            // 读取BOM
            var bomEntities = await _procBomRepository.GetByIdsAsync(pagedInfo.Data.Select(s => s.BomId));

            List<PlanWorkPlanProductDto> dtos = new();
            foreach (var dataItem in pagedInfo.Data)
            {
                var dto = dataItem.ToModel<PlanWorkPlanProductDto>();
                if (dto == null) continue;

                // 填充生产计划
                var planEntity = planEntities.FirstOrDefault(f => f.Id == dataItem.WorkPlanId);
                if (planEntity != null)
                {
                    dto.WorkPlanCode = planEntity.WorkPlanCode;
                    dto.PlanStartTime = planEntity.PlanStartTime;
                    dto.PlanEndTime = planEntity.PlanEndTime;
                    dto.Type = planEntity.Type;
                    dto.Status = planEntity.Status;
                }

                // 填充产品
                var productEntity = productEntities.FirstOrDefault(f => f.Id == dataItem.ProductId);
                if (productEntity != null)
                {
                    dto.ProductCode = productEntity.MaterialCode;
                    dto.ProductName = productEntity.MaterialName;
                }

                // 填充BOM
                var bomEntity = bomEntities.FirstOrDefault(f => f.Id == dataItem.BomId);
                if (bomEntity != null)
                {
                    dto.BomCode = bomEntity.BomCode;
                    dto.BomName = bomEntity.BomName;
                }

                dtos.Add(dto);
            }

            return new PagedInfo<PlanWorkPlanProductDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据planProductId查询
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        public async Task<PlanWorkPlanProductDetailDto?> QueryByIdAsync(long planProductId)
        {
            var planProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(planProductId);
            if (planProductEntity == null) return default;

            // 当前对象
            var dto = new PlanWorkPlanProductDetailDto
            {
                Id = planProductEntity.Id,
                ProductId = planProductEntity.ProductId,
                BomId = planProductEntity.BomId,
                Qty = planProductEntity.Qty,
                OverScale = planProductEntity.OverScale,
                Remark = planProductEntity.Remark
            };

            // 填充生产计划
            var planEntity = await _planWorkPlanRepository.GetByIdAsync(planProductEntity.WorkPlanId);
            if (planEntity != null)
            {
                dto.WorkPlanCode = planEntity.WorkPlanCode;
                dto.Type = planEntity.Type.GetDescription();
                dto.Status = planEntity.Status.GetDescription();
                dto.PlanStartTime = planEntity.PlanStartTime.ToString("yyyy-MM-dd");
                dto.PlanEndTime = planEntity.PlanEndTime.ToString("yyyy-MM-dd");

                // 读取工作中心
                var workCenterEntity = await _inteWorkCenterRepository.GetEntityAsync(new InteWorkCenterOneQuery
                {
                    SiteId = planProductEntity.SiteId,
                    Code = planEntity.WorkCenterCode
                });
                if (workCenterEntity != null)
                {
                    dto.WorkCenterCode = workCenterEntity.Code;
                    dto.WorkCenterName = workCenterEntity.Name;
                }
            }

            // 填充产品
            var productEntity = await _procMaterialRepository.GetByIdAsync(planProductEntity.ProductId);
            if (productEntity != null)
            {
                dto.ProductCode = productEntity.MaterialCode;
                dto.ProductName = productEntity.MaterialName;
            }

            // 填充BOM
            var bomEntity = await _procBomRepository.GetByIdAsync(planProductEntity.BomId);
            if (bomEntity != null)
            {
                dto.BomCode = bomEntity.BomCode;
                dto.BomName = bomEntity.BomName;
            }

            return dto;
        }

        /// <summary>
        /// 读取生产计划已经下发的子工单
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanDetailSaveDto>> QueryOrderByPlanIdAsync(long planProductId)
        {
            // 检查生产计划是否存在
            var workPlanProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(planProductId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16018));

            List<PlanWorkPlanDetailSaveDto> dtos = new();

            // 查询生产计划的子工单
            var workOrderEntities = await _planWorkOrderRepository.GetByPlanProductIdAsync(workPlanProductEntity.Id);
            if (workOrderEntities == null || !workOrderEntities.Any()) return dtos;

            foreach (var item in workOrderEntities)
            {
                dtos.Add(new PlanWorkPlanDetailSaveDto
                {
                    WorkOrderCode = item.OrderCode,
                    PlanStartTime = item.PlanStartTime,
                    PlanEndTime = item.PlanEndTime,
                    Qty = item.Qty
                });
            }

            return dtos;
        }

        // TODO: 读取生产计划的物料

        /// <summary>
        /// 根据planProductId查询
        /// </summary>
        /// <param name="planProductId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanMaterialDto>?> QueryMaterialsByMainIdAsync(long planProductId)
        {
            var planProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(planProductId);
            if (planProductEntity == null) return default;

            var workPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesAsync(new PlanWorkPlanQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                WorkPlanProductId = planProductId
            });
            if (workPlanMaterialEntities == null || !workPlanMaterialEntities.Any()) return default;

            return workPlanMaterialEntities.Select(s => s.ToModel<PlanWorkPlanMaterialDto>());
        }



        #region 内部方法
        /// <summary>
        /// 获取工艺路线编码
        /// </summary>
        /// <param name="configEntities"></param>
        /// <param name="planType"></param>
        /// <returns></returns>
        private static string GetProcessRouteCode(IEnumerable<SysConfigEntity> configEntities, PlanWorkPlanTypeEnum planType)
        {
            var configEntity = configEntities.FirstOrDefault();
            if (configEntity == null) return "not configured";

            if (string.IsNullOrWhiteSpace(configEntity?.Value)) return "no configured value";

            var valueArray = configEntity.Value.Split('|');
            return planType == PlanWorkPlanTypeEnum.Rotor ? valueArray[0] : valueArray[1];
        }

        /// <summary>
        /// 获取工作中心编码
        /// </summary>
        /// <param name="configEntities"></param>
        /// <param name="planType"></param>
        /// <returns></returns>
        private static string GetWorkCenterCode(IEnumerable<SysConfigEntity> configEntities, PlanWorkPlanTypeEnum planType)
        {
            var configEntity = configEntities.FirstOrDefault();
            if (configEntity == null) return "not configured";

            if (string.IsNullOrWhiteSpace(configEntity?.Value)) return "no configured value";

            var valueArray = configEntity.Value.Split('|');
            return planType == PlanWorkPlanTypeEnum.Rotor ? valueArray[0] : valueArray[1];
        }
        #endregion
    }
}
