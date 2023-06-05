using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.EquipmentServices.Bos.Manufacture;
using Hymson.MES.EquipmentServices.Services.Common;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.Job.Implementing
{
    /// <summary>
    /// SFC转换
    /// </summary>
    public class JobManuSfcConvertService : IJobManufactureService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;

        /// <summary>
        /// 仓储接口（掩码规则维护）
        /// </summary>
        private readonly IProcMaskCodeRuleRepository _procMaskCodeRuleRepository;

        /// <summary>
        /// 仓储接口（BBOM）
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;
        /// <summary>
        /// 仓储接口（BOM明细）
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 仓储接口（公共方法）
        /// </summary>
        private readonly ICommonService _manuCommonService;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="procMaskCodeRuleRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procResourceEquipmentBindRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="manuCommonService"></param> 
        public JobManuSfcConvertService(ICurrentEquipment currentEquipment, IProcMaskCodeRuleRepository procMaskCodeRuleRepository,
            IProcResourceRepository procResourceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository, ICommonService manuCommonService)
        {
            _currentEquipment = currentEquipment;
            _procMaskCodeRuleRepository = procMaskCodeRuleRepository;
            _procResourceRepository = procResourceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _manuCommonService = manuCommonService;
        }


        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(Dictionary<string, string>? param)
        {
            if (param == null ||
                param.ContainsKey("SFC") == false
                || param.ContainsKey("EquipmentId") == false
                || param.ContainsKey("ResourceId") == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16312));
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 条码转换（模组形态转换-条码=》工单）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<JobResponseDto> ExecuteAsync(Dictionary<string, string>? param)
        {
            var defaultDto = new JobResponseDto { };

            var bo = new ManufactureDto
            {
                SFC = param["SFC"],
                EquipmentId = param["EquipmentId"].ParseToLong(),
                ResourceId = param["ResourceId"].ParseToLong()
            };

            //获取资源
            var procResource = await _procResourceRepository.GetByIdAsync(bo.ResourceId);
            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = _currentEquipment.SiteId,
                Ids = new long[] { _currentEquipment.Id ?? 0 },
                ResourceId = bo.ResourceId,
            };
            var resEquipentBind = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(resourceEquipmentBindQuery);
            if (resEquipentBind.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19910)).WithData("ResCode", procResource.ResCode).WithData("EquCode", _currentEquipment.Code);
            }

            //查找当前工作中心（产线）
            var workLine = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
            if (workLine == null)
            {
                //通过资源未找到关联产线
                throw new CustomerValidationException(nameof(ErrorCode.MES19911)).WithData("ResourceCode", procResource.ResCode);
            }
            //查找激活工单
            var planWorkOrders = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLine.Id);
            if (planWorkOrders == null || !planWorkOrders.Any())
            {
                //产线未激活工单
                throw new CustomerValidationException(nameof(ErrorCode.MES19912)).WithData("WorkCenterCode", workLine.Code);
            }
            //不考虑混线
            var planWorkOrder = planWorkOrders.First();

            //获取当前资源绑定的工序
            var procedureEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new ProcProdureByResourceIdQuery { ResourceId = bo.ResourceId, SiteId = _currentEquipment.SiteId });
            if (procedureEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", procResource.ResCode);
            }
            //获取当前物料
            var bomEntity = await _procBomRepository.GetByIdAsync(planWorkOrder.ProductBOMId);
            if (bomEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19914)).WithData("OrderCode", planWorkOrder.OrderCode);
            }
            //获取BOM对应当前工序的物料
            var bomDetailList = await _procBomDetailRepository.GetByBomIdAndProcedureIdAsync(new ProcBomDetailByBomIdAndProcedureIdQuery { BomId = bomEntity.Id, ProcedureId = procedureEntity.Id });
            if (bomDetailList == null || !bomDetailList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19915)).WithData("BomCode", bomEntity.BomCode);
            }
            var bomDetail = bomDetailList.FirstOrDefault();
            //验证掩码规则
            var isCodeRule = await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(bo.SFC, bomDetail.MaterialId);
            if (!isCodeRule)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", bo.SFC);
            }
            //获取SFC
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = bo.SFC, SiteId = _currentEquipment.SiteId });
            if (sfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19917)).WithData("SFC", bo.SFC);
            }
            var processRouteFirstProcedure = await _manuCommonService.GetFirstProcedureAsync(planWorkOrder.ProcessRouteId);

            List<ManuSfcEntity> manuSfcList = new();
            List<ManuSfcInfoEntity> manuSfcInfoList = new();
            List<ManuSfcProduceEntity> manuSfcProduceList = new();
            List<ManuSfcStepEntity> manuSfcStepList = new();
            //条码
            var manuSfcEntity = new ManuSfcEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = bo.SFC,
                Qty = 1,
                IsUsed = YesOrNoEnum.Yes,
                Status = SfcStatusEnum.InProcess,
                CreatedBy = _currentEquipment.Name,
                UpdatedBy = _currentEquipment.Name
            };
            manuSfcList.Add(manuSfcEntity);
            //明细
            manuSfcInfoList.Add(new ManuSfcInfoEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SfcId = manuSfcEntity.Id,
                WorkOrderId = planWorkOrder.Id,
                ProductId = planWorkOrder.ProductId,
                IsUsed = true,
                CreatedBy = _currentEquipment.Name,
                UpdatedBy = _currentEquipment.Name
            });
            //在制
            manuSfcProduceList.Add(new ManuSfcProduceEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = bo.SFC,
                ProductId = planWorkOrder.ProductId,
                WorkOrderId = planWorkOrder.Id,
                BarCodeInfoId = manuSfcEntity.Id,
                ProcessRouteId = planWorkOrder.ProcessRouteId,
                WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                ProductBOMId = planWorkOrder.ProductBOMId,
                Qty = 1,
                ProcedureId = processRouteFirstProcedure.ProcedureId,
                Status = SfcProduceStatusEnum.lineUp,
                RepeatedCount = 0,
                IsScrap = TrueOrFalseEnum.No,
                CreatedBy = _currentEquipment.Name,
                UpdatedBy = _currentEquipment.Name
            });
            //步骤
            manuSfcStepList.Add(new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = bo.SFC,
                ProductId = planWorkOrder.ProductId,
                WorkOrderId = planWorkOrder.Id,
                ProductBOMId = planWorkOrder.ProductBOMId,
                WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                Qty = 1,
                ProcedureId = processRouteFirstProcedure.ProcedureId,
                Operatetype = ManuSfcStepTypeEnum.Create,
                CurrentStatus = SfcProduceStatusEnum.lineUp,
                CreatedBy = _currentEquipment.Name,
                UpdatedBy = _currentEquipment.Name
            });
            //事务入库
            using var ts = TransactionHelper.GetTransactionScope();

            await _manuSfcRepository.InsertRangeAsync(manuSfcList);

            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);

            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);

            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);

            ts.Complete();

            return defaultDto;
        }
    }
}
