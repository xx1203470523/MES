using FluentValidation;
using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Manufacture.ManuBind;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Services.Common;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.SfcBinding
{
    /// <summary>
    /// 条码绑定 
    /// </summary>
    public class SfcBindingService : ISfcBindingService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储接口（条码流转信息）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 仓储接口（工单信息）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 条码
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码明细
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 工序和资源半成品产品设置表仓储接口
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 仓储接口（公共方法）
        /// </summary>
        private readonly ICommonService _manuCommonOldService;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 条码
        /// </summary>
        private readonly IManuBindService _manuBindService;

        /// <summary>
        /// 条码
        /// </summary>
        private readonly ILocalizationService _localizationService;


        private readonly AbstractValidator<SfcBindingDto> _validationSfcBindingDtoRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="procProductSetRepository"></param>
        /// <param name="manuCommonService"></param>
        /// <param name="manuCommonOldService"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="manuBindService"></param>
        /// <param name="localizationService"></param>
        /// <param name="validationSfcBindingDtoRules"></param>
        public SfcBindingService(ICurrentEquipment currentEquipment,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IProcProductSetRepository procProductSetRepository,
            IManuCommonService manuCommonService,
            ICommonService manuCommonOldService,
            IEquEquipmentRepository equEquipmentRepository,
            IManuBindService manuBindService,
            ILocalizationService localizationService,
            AbstractValidator<SfcBindingDto> validationSfcBindingDtoRules)
        {
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _currentEquipment = currentEquipment;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _procProductSetRepository = procProductSetRepository;
            _manuCommonService = manuCommonService;
            _manuCommonOldService = manuCommonOldService;
            _equEquipmentRepository = equEquipmentRepository;
            _manuBindService = manuBindService;
            _localizationService = localizationService;
            _validationSfcBindingDtoRules = validationSfcBindingDtoRules;
        }

        #region 注释代码
        /// <summary>
        /// 条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param>
        /// <returns></returns>
        //public async Task SfcBindingAsync(SfcBindingDto sfcBindingDto)
        //{
        //    var sfcs = sfcBindingDto.BindSfc.Select(it => it.SFC).ToArray();
        //    //查询子条码
        //    var manuSfcProduceEntitys = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentEquipment.SiteId });
        //    if (manuSfcProduceEntitys == null || !manuSfcProduceEntitys.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19918)).WithData("SFC", string.Join(",", sfcs));
        //    }
        //    var sfcsExistVerify = sfcs.Where(sfc => !manuSfcProduceEntitys.Any(it => it.SFC == sfc)).ToArray();
        //    if (sfcsExistVerify != null && sfcsExistVerify.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19918)).WithData("SFC", sfcsExistVerify);
        //    }
        //    //子条码位置
        //    var sfcSqeVerify = sfcBindingDto.BindSfc.GroupBy(it => it.Seq).Where(it => it.Count() > 1);
        //    if (sfcSqeVerify != null && sfcSqeVerify.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19924));
        //    }
        //    //子条码
        //    var sfcRepeatVerify = sfcBindingDto.BindSfc.GroupBy(it => it.SFC).Where(it => it.Count() > 1);
        //    if (sfcRepeatVerify != null && sfcRepeatVerify.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19925));
        //    }
        //    //获取资源
        //    var resourceEntitys = await _procResourceRepository.GetResourceByResourceCodeAsync(new ProcResourceQuery { Status = (int)SysDataStatusEnum.Enable, ResCode = sfcBindingDto.ResourceCode, SiteId = _currentEquipment.SiteId });
        //    if (resourceEntitys == null)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19919)).WithData("ResCode", sfcBindingDto.ResourceCode);
        //    }
        //    //获取当前资源绑定的工序
        //    var procedureEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new ProcProdureByResourceIdQuery { ResourceId = resourceEntitys.Id, SiteId = _currentEquipment.SiteId });
        //    if (procedureEntity == null)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", sfcBindingDto.ResourceCode);
        //    }
        //    //验证子条码是否在当前工序活动
        //    var sfcThisProcedure = manuSfcProduceEntitys.Where(it => it.ProcedureId != procedureEntity.Id || it.Status != SfcStatusEnum.Activity);
        //    if (sfcThisProcedure != null && sfcThisProcedure.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19920)).WithData("SFC", string.Join(",", sfcThisProcedure.Select(it => it.SFC).ToArray()));
        //    }
        //    //子条码是否绑定
        //    var manuSfcCirculationList = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery { Sfc = sfcs, SiteId = _currentEquipment.SiteId, IsDisassemble = TrueOrFalseEnum.No });
        //    if (manuSfcCirculationList != null && manuSfcCirculationList.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19921)).WithData("SFC", string.Join(",", manuSfcCirculationList.Select(it => it.SFC).ToArray()));
        //    }
        //    //主条码是否使用
        //    manuSfcCirculationList = await _manuSfcCirculationRepository.GetSfcMoudulesAsync(new ManuSfcCirculationBySfcsQuery { CirculationBarCode = sfcBindingDto.SFC, SiteId = _currentEquipment.SiteId, IsDisassemble = TrueOrFalseEnum.No });
        //    if (manuSfcCirculationList != null && manuSfcCirculationList.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19922)).WithData("SFC", sfcBindingDto.SFC);
        //    }
        //    //获取工单
        //    var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcProduceEntitys.First().WorkOrderId);
        //    if (planWorkOrderEntity == null)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19923));
        //    }

        //    var productId = planWorkOrderEntity.ProductId;
        //    //TODO 根据资源或工序查找产品
        //    var procProductSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new GetByProcedureIdAndProductIdQuery { SiteId = _currentEquipment.SiteId, SetPointId = resourceEntitys.Id, ProductId = productId });
        //    if (procProductSetEntity != null)
        //    {
        //        productId = procProductSetEntity.SemiProductId;
        //    }
        //    else
        //    {
        //        procProductSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new GetByProcedureIdAndProductIdQuery { SiteId = _currentEquipment.SiteId, SetPointId = procedureEntity.Id, ProductId = productId });
        //        if (procProductSetEntity != null)
        //        {
        //            productId = procProductSetEntity.SemiProductId;
        //        }
        //    }

        //    //验证掩码规则
        //    var isCodeRule = await _manuCommonOldService.CheckBarCodeByMaskCodeRuleAsync(sfcBindingDto.SFC, productId);
        //    if (!isCodeRule)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", sfcBindingDto.SFC);
        //    }

        //    var createManuSfcCirculationList = new List<ManuSfcCirculationEntity>();
        //    var createManuSfcStepList = new List<ManuSfcStepEntity>();
        //    var updateManuSfcEntity = new ManuSfcUpdateCommand();
        //    var deteleManuSfcProduceList = new DeletePhysicalBySfcsCommand();
        //    var createManuSfcProduceEntity = new ManuSfcProduceEntity();
        //    var createManuMainSfcEntity = new ManuSfcEntity();
        //    var createManuMainSfcInfoEntity = new ManuSfcInfoEntity();
        //    var createManuMainSfcStepEntity = new ManuSfcStepEntity();

        //    foreach (var item in manuSfcProduceEntitys)
        //    {
        //        //条码流转
        //        createManuSfcCirculationList.Add(new ManuSfcCirculationEntity()
        //        {
        //            Id = IdGenProvider.Instance.CreateId(),
        //            SiteId = _currentEquipment.SiteId,
        //            ProcedureId = item.ProcedureId,
        //            ResourceId = resourceEntitys.Id,
        //            SFC = item.SFC,
        //            WorkOrderId = item.WorkOrderId,
        //            ProductId = item.ProductId,
        //            CirculationBarCode = sfcBindingDto.SFC,
        //            CirculationProductId = planWorkOrderEntity.ProductId,
        //            CirculationMainProductId = planWorkOrderEntity.ProductId,
        //            CirculationQty = 1,
        //            CirculationType = SfcCirculationTypeEnum.Change,
        //            CreatedBy = _currentEquipment.Name,
        //            UpdatedBy = _currentEquipment.Name,
        //        });

        //        //步骤
        //        createManuSfcStepList.Add(new ManuSfcStepEntity
        //        {
        //            Id = IdGenProvider.Instance.CreateId(),
        //            SiteId = _currentEquipment.SiteId,
        //            SFC = item.SFC,
        //            EquipmentId = item.EquipmentId,
        //            ResourceId = item.ResourceId,
        //            ProductId = item.ProductId,
        //            WorkOrderId = item.WorkOrderId,
        //            ProductBOMId = item.ProductBOMId,
        //            WorkCenterId = item.WorkCenterId,
        //            Qty = 1,
        //            ProcedureId = item.ProcedureId,
        //            Operatetype = ManuSfcStepTypeEnum.Change,
        //            CurrentStatus = SfcStatusEnum.Complete,
        //            CreatedBy = _currentEquipment.Name,
        //            UpdatedBy = _currentEquipment.Name
        //        });
        //    }

        //    //子条码完成
        //    updateManuSfcEntity = new ManuSfcUpdateCommand
        //    {
        //        Sfcs = sfcs,
        //        Status = SfcStatusEnum.Complete,
        //        UserId = _currentEquipment.Name,
        //        UpdatedOn = HymsonClock.Now()
        //    };

        //    //在制完成
        //    deteleManuSfcProduceList = new DeletePhysicalBySfcsCommand()
        //    {
        //        Sfcs = sfcs,
        //        SiteId = _currentEquipment.SiteId
        //    };

        //    //条码信息
        //    createManuMainSfcEntity = new ManuSfcEntity
        //    {
        //        Id = IdGenProvider.Instance.CreateId(),
        //        SiteId = _currentEquipment.SiteId,
        //        SFC = sfcBindingDto.SFC,
        //        Qty = 1,
        //        IsUsed = YesOrNoEnum.Yes,
        //        Status = SfcStatusEnum.lineUp,
        //        CreatedBy = _currentEquipment.Name,
        //        UpdatedBy = _currentEquipment.Name
        //    };

        //    //条码明细
        //    createManuMainSfcInfoEntity = new ManuSfcInfoEntity
        //    {
        //        Id = IdGenProvider.Instance.CreateId(),
        //        SiteId = _currentEquipment.SiteId,
        //        SfcId = createManuMainSfcEntity.Id,
        //        WorkOrderId = planWorkOrderEntity.Id,
        //        ProductId = productId,
        //        IsUsed = true,
        //        CreatedBy = _currentEquipment.Name,
        //        UpdatedBy = _currentEquipment.Name
        //    };

        //    //在制
        //    createManuSfcProduceEntity = new ManuSfcProduceEntity
        //    {
        //        Id = IdGenProvider.Instance.CreateId(),
        //        SiteId = _currentEquipment.SiteId,
        //        SFC = sfcBindingDto.SFC,
        //        ProductId = productId,
        //        WorkOrderId = planWorkOrderEntity.Id,
        //        BarCodeInfoId = createManuMainSfcInfoEntity.Id,
        //        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
        //        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
        //        ProductBOMId = planWorkOrderEntity.ProductBOMId,
        //        Qty = 1,
        //        ProcedureId = procedureEntity.Id,
        //        Status = SfcStatusEnum.Activity,
        //        RepeatedCount = 0,
        //        IsScrap = TrueOrFalseEnum.No,
        //        CreatedBy = _currentEquipment.Name,
        //        UpdatedBy = _currentEquipment.Name
        //    };

        //    //步骤
        //    createManuMainSfcStepEntity = new ManuSfcStepEntity
        //    {
        //        Id = IdGenProvider.Instance.CreateId(),
        //        SiteId = _currentEquipment.SiteId,
        //        SFC = sfcBindingDto.SFC,
        //        EquipmentId = _currentEquipment.SiteId,
        //        ResourceId = resourceEntitys.Id,
        //        ProductId = productId,
        //        WorkOrderId = planWorkOrderEntity.Id,
        //        ProductBOMId = planWorkOrderEntity.ProductBOMId,
        //        WorkCenterId = planWorkOrderEntity.WorkCenterId,
        //        Qty = 1,
        //        ProcedureId = procedureEntity.Id,
        //        Operatetype = ManuSfcStepTypeEnum.Change,
        //        CurrentStatus = SfcStatusEnum.Complete,
        //        CreatedBy = _currentEquipment.Name,
        //        UpdatedBy = _currentEquipment.Name
        //    };

        //    //事务统一入库
        //    using var trans = TransactionHelper.GetTransactionScope();

        //    await _manuSfcCirculationRepository.InsertRangeAsync(createManuSfcCirculationList);

        //    await _manuSfcStepRepository.InsertRangeAsync(createManuSfcStepList);

        //    await _manuSfcRepository.UpdateStatusAsync(updateManuSfcEntity);

        //    await _manuSfcProduceRepository.DeletePhysicalRangeAsync(deteleManuSfcProduceList);

        //    await _manuSfcRepository.InsertAsync(createManuMainSfcEntity);

        //    await _manuSfcInfoRepository.InsertAsync(createManuMainSfcInfoEntity);

        //    await _manuSfcProduceRepository.InsertAsync(createManuSfcProduceEntity);

        //    await _manuSfcStepRepository.InsertAsync(createManuMainSfcStepEntity);

        //    trans.Complete();
        //}

        #endregion

        /// <summary>
        /// 条码绑定
        /// </summary>
        /// <param name="sfcBindingDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationBindAsync(SfcBindingDto sfcBindingDto)
        {
            await _validationSfcBindingDtoRules.ValidateAndThrowAsync(sfcBindingDto);
            var manuBo = await _manuCommonService.GetManufactureBoAsync(new ManufactureRequestBo
            {
                SiteId = _currentEquipment.SiteId,
                ResourceCode = sfcBindingDto.ResourceCode,
                EquipmentCode = sfcBindingDto.EquipmentCode
            });
            if (manuBo == null) return;

            await _manuBindService.BindByActive(new ManuBindDto
            {
                SiteId = _currentEquipment.SiteId,
                UserName = _currentEquipment.Name,
                ProcedureId = manuBo.ProcedureId,
                ResourceId = manuBo.ResourceId,
                EquipmentId = _currentEquipment.Id,
                SFC = sfcBindingDto.SFC,
                BindSFCs = sfcBindingDto.BindSfcs.Select(x => new CirculationBindDto { Location = x.Location, SFC = x.SFC })
            }
            , _localizationService);
        }

    }
}
