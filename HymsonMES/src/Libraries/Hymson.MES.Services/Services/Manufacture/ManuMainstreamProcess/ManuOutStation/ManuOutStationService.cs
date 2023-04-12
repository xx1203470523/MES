using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation
{
    /// <summary>
    /// 出站
    /// </summary>
    public class ManuOutStationService : IManuOutStationService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码信息）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public ManuOutStationService(ICurrentUser currentUser, ICurrentSite currentSite,
            IManuCommonService manuCommonService,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
        }


        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> OutStationAsync(ManufactureBo bo)
        {
            var rows = 0;

            // 获取生产条码信息（附带条码合法性校验 + 工序活动状态校验）
            var sfcProduceEntity = await _manuCommonService.GetProduceSFCWithCheckAsync(bo.SFC, bo.ProcedureId);

            // 获取生产工单
            _ = await _manuCommonService.GetProduceWorkOrderByIdAsync(sfcProduceEntity.WorkOrderId);

            // 更新时间
            sfcProduceEntity.UpdatedBy = _currentUser.UserName;
            sfcProduceEntity.UpdatedOn = HymsonClock.Now();

            // 初始化步骤
            var sfcStep = new ManuSfcStepEntity
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfcProduceEntity.SFC,
                ProductId = sfcProduceEntity.ProductId,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                WorkCenterId = sfcProduceEntity.WorkCenterId,
                ProductBOMId = sfcProduceEntity.ProductBOMId,
                Qty = sfcProduceEntity.Qty,
                EquipmentId = sfcProduceEntity.EquipmentId,
                ResourceId = sfcProduceEntity.ResourceId,
                CreatedBy = sfcProduceEntity.UpdatedBy,
                CreatedOn = sfcProduceEntity.UpdatedOn.Value,
                UpdatedBy = sfcProduceEntity.UpdatedBy,
                UpdatedOn = sfcProduceEntity.UpdatedOn.Value,
            };

            // 扣料
            //await func(sfcProduceEntity.ProductBOMId, sfcProduceEntity.ProcedureId);
            await DeductMaterialAsync(sfcProduceEntity.ProductBOMId, sfcProduceEntity.ProcedureId);

            // 合格品出站
            // 获取下一个工序（如果没有了，就表示完工）
            var nextProcedure = await _manuCommonService.GetNextProcedureAsync(sfcProduceEntity);

            // 完工
            if (nextProcedure == null)
            {
                // 删除 manu_sfc_produce
                rows += await _manuSfcProduceRepository.DeletePhysicalAsync(sfcProduceEntity.SFC);

                // TODO 删除 manu_sfc_produce_business

                // 插入 manu_sfc_step 状态为 完成
                sfcStep.Operatetype = ManuSfcStepTypeEnum.Complete;    // TODO 这里的状态？？
                sfcStep.CurrentStatus = SfcProduceStatusEnum.Complete;  // TODO 这里的状态？？
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);

                // TODO manu_sfc_info 修改为 完成或者入库
                // 条码信息
                var sfcInfo = await _manuSfcRepository.GetBySFCAsync(sfcProduceEntity.SFC);

                sfcInfo.Status = SfcStatusEnum.Complete;
                rows += await _manuSfcRepository.UpdateAsync(sfcInfo);
            }
            // 未完工
            else
            {
                // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                sfcProduceEntity.ProcedureId = nextProcedure.Id;
                rows += await _manuSfcProduceRepository.UpdateAsync(sfcProduceEntity);

                // 插入 manu_sfc_step 状态为 进站
                sfcStep.Operatetype = ManuSfcStepTypeEnum.InStock;
                rows += await _manuSfcStepRepository.InsertAsync(sfcStep);
            }

            return rows;
        }

        /// <summary>
        /// 扣料
        /// </summary>
        /// <param name="productBOMId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private async Task DeductMaterialAsync(long productBOMId, long procedureId)
        {
            // 获取条码对应的工序BOM
            var bomMaterials = await _procBomDetailRepository.GetByBomIdAsync(productBOMId);

            // 未设置BOM
            if (bomMaterials == null || bomMaterials.Any() == false) throw new BusinessException(nameof(ErrorCode.MES10612));

            // 取得特定工序的BOM
            var deductList = new List<MaterialDeductBo> { };
            bomMaterials = bomMaterials.Where(w => w.ProcedureId == procedureId);

            // 统计扣料数据
            MaterialDeductBo deduct = new();
            foreach (var item in bomMaterials)
            {
                // 扣减数量
                deduct.MaterialId = item.MaterialId;
                deduct.Qty = item.Usages * item.Loss;

                // TODO 1.确认收集方式是否批次 item.ReferencePoint
                if (item.ReferencePoint == "TODO 收集方式是批次")
                {
                    // 添加到待扣料集合
                    deductList.Add(deduct);
                    continue;
                }

                var materialEntity = await _procMaterialRepository.GetByIdAsync(item.MaterialId);
                if (materialEntity == null) continue;

                // 2.确认主物料的收集方式，不是"批次"就结束
                if (materialEntity.SerialNumber != MaterialSerialNumberEnum.Batch) continue;

                // 如有设置消耗系数
                if (materialEntity.ConsumeRatio.HasValue == true) deduct.Qty *= materialEntity.ConsumeRatio.Value;

                // 添加到待扣料集合
                deductList.Add(deduct);
            }

            // TODO 扣料

            // 判断在线库存物料是否满足要求（物料编码，数量，状态）

            // 扣料并关联主条码

            // 判断BOM物料绑定？？

        }

    }
}
