using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.CoreServices.Services.Job.JobUtility;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using MySqlX.XDevAPI.Common;
using System.Threading.Tasks.Dataflow;
using System.Linq;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.Localization.Services;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 条码转换
    /// </summary>
    [Job("条码转换", JobTypeEnum.Standard)]
    public class SfcConvertJobService : IJobService
    {

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
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
        /// 工单激活（物理删除）仓储接口
        /// </summary>
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;


        /// <summary>
        ///  仓储（物料库存）
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;



        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<SfcConvertRequestBo> _validationRepairJob;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="manuCommonService"></param>
        public SfcConvertJobService(IManuCommonService manuCommonService,
            AbstractValidator<SfcConvertRequestBo> validationRepairJob,
            IMasterDataService masterDataService,
             IProcMaskCodeRuleRepository procMaskCodeRuleRepository,
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
            IProcBomDetailRepository procBomDetailRepository,
            IPlanWorkOrderBindRepository planWorkOrderBindRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
            IEquEquipmentRepository equEquipmentRepository)
        {
            _manuCommonService = manuCommonService;
            _validationRepairJob = validationRepairJob;
            _masterDataService = masterDataService;
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
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _equEquipmentRepository = equEquipmentRepository;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param> 
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not SfcConvertRequestBo bo)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            }
            // 验证DTO
            await _validationRepairJob.ValidateAndThrowAsync(bo);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not SfcConvertRequestBo bo)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10103));
            }

            //获取资源与设备
            var procResource = await param.Proxy.GetValueAsync(_procResourceRepository.GetByIdAsync, bo.ResourceId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16337));
            var equEquipment = await param.Proxy.GetValueAsync(_equEquipmentRepository.GetByIdAsync, bo.EquipmentId)
               ?? throw new CustomerValidationException(nameof(ErrorCode.MES16338));
            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = bo.SiteId,
                Ids = new long[] { bo.EquipmentId },
                ResourceId = bo.ResourceId,
            };
            var resEquipentBind = await param.Proxy.GetValueAsync(_procResourceEquipmentBindRepository.GetByResourceIdAsync, resourceEquipmentBindQuery);
            if (resEquipentBind == null || resEquipentBind.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19910)).WithData("ResCode", procResource.ResCode).WithData("EquCode", equEquipment.EquipmentCode);
            }


            //TODO 这里应该查资源绑定的工单
            var planWorkOrderBindEntity = await param.Proxy.GetValueAsync(_planWorkOrderBindRepository.GetByResourceIDAsync, new PlanWorkOrderBindByResourceIdQuery { SiteId = bo.SiteId, EquipmentId = bo.EquipmentId, ResourceId = bo.ResourceId });
              ?? throw new CustomerValidationException(nameof(ErrorCode.MES19928)).WithData("ResCode", procResource.ResCode);
            //获取工单  带验证
            var planWorkOrder = await param.Proxy.GetValueAsync(async paramters =>
            {
                var workOrderId = (long)paramters[0];
                return await _masterDataService.GetProduceWorkOrderByIdAsync(workOrderId, true);
            }, new object[] { planWorkOrderBindEntity.WorkOrderId, true });
            if (planWorkOrder == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19929));
            }

            //获取当前资源绑定的工序
            var procedureEntity = await param.Proxy.GetValueAsync(_procProcedureRepository.GetProcProdureByResourceIdAsync, new ProcProdureByResourceIdQuery { ResourceId = bo.ResourceId, SiteId = bo.SiteId });
            if (procedureEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", procResource.ResCode);
            }
            //获取当前物料
            var bomEntity = await param.Proxy.GetValueAsync(_procBomRepository.GetByIdAsync, planWorkOrder.ProductBOMId);
            if (bomEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19914)).WithData("OrderCode", planWorkOrder.OrderCode);
            }
            //获取BOM对应当前工序的物料
            var bomDetailList = await param.Proxy.GetValueAsync(_procBomDetailRepository.GetByBomIdAndProcedureIdAsync, new ProcBomDetailByBomIdAndProcedureIdQuery { BomId = bomEntity.Id, ProcedureId = procedureEntity.Id });
            if (bomDetailList == null || !bomDetailList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19915)).WithData("BomCode", bomEntity.BomCode);
            }
            var bomDetail = bomDetailList.FirstOrDefault();

            //处理多个SFC


            //返回
            return new SfcConvertResponseBo() { };
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return 0;
        }

    }
}
