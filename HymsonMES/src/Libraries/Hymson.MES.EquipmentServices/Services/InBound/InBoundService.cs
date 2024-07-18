using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox;
using Hymson.MES.Data.Repositories.Integrated.InteSFCBox.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using IdGen;

namespace Hymson.MES.EquipmentServices.Services.InBound
{
    /// <summary>
    /// 进站服务
    /// </summary>
    public class InBoundService : IInBoundService
    {
        #region Repository
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<InBoundDto> _validationInBoundDtoRules;
        private readonly AbstractValidator<InBoundMoreDto> _validationInBoundMoreDtoRules;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcessRouteDetailNodeRepository _processRouteDetailNodeRepository;
        private readonly IProcProcedureRepository _procedureRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        /// <summary>
        /// 工单关联批次
        /// </summary>
        private readonly IInteSFCBoxRepository _inteSFCBoxRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        public InBoundService(AbstractValidator<InBoundDto> validationInBoundDtoRules,
            ICurrentEquipment currentEquipment,
            AbstractValidator<InBoundMoreDto> validationInBoundMoreDtoRules,
            IProcResourceRepository procResourceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcessRouteDetailNodeRepository processRouteDetailNodeRepository,
            IProcProcedureRepository procedureRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository,
            IProcMaterialRepository procMaterialRepository,
            IInteSFCBoxRepository inteSFCBoxRepository)
        {
            _validationInBoundDtoRules = validationInBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationInBoundMoreDtoRules = validationInBoundMoreDtoRules;
            _procResourceRepository = procResourceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _processRouteDetailNodeRepository = processRouteDetailNodeRepository;
            _procedureRepository = procedureRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteSFCBoxRepository = inteSFCBoxRepository;
        }
        #endregion

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="inBoundDto"></param>
        /// <returns></returns>
        public async Task InBoundAsync(InBoundDto inBoundDto)
        {
            var equName = _currentEquipment.Name;
            await _validationInBoundDtoRules.ValidateAndThrowAsync(inBoundDto);
            if (inBoundDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            //已经验证过资源是否存在直接使用
            var inBoundMore = new InBoundMoreDto
            {
                SFCs = new string[] { inBoundDto.SFC },
                ResourceCode = inBoundDto.ResourceCode,
                LocalTime = inBoundDto.LocalTime,
                IsVerifyVirtualSFC = inBoundDto.IsVerifyVirtualSFC
            };
            await SFCInBoundAsync(inBoundMore);
        }

        /// <summary>
        /// 进站（多个）
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        public async Task InBoundMoreAsync(InBoundMoreDto inBoundMoreDto)
        {
            await _validationInBoundMoreDtoRules.ValidateAndThrowAsync(inBoundMoreDto);
            if (inBoundMoreDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (inBoundMoreDto.SFCs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }
            await SFCInBoundAsync(inBoundMoreDto);
        }

        /// <summary>
        /// 初版
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task SFCInBoundAsync(InBoundMoreDto inBoundMoreDto)
        {
            bool validateBatch = false;
            //校验电芯批次, 扫码&OCVR测试机,稍候增加配置管理功能
            if (_currentEquipment.Code.Equals("YTLPACK01AE004"))
            {
                validateBatch = true;
            }
;
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = inBoundMoreDto.ResourceCode });

            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = _currentEquipment.SiteId,
                Ids = new long[] { _currentEquipment.Id ?? 0 },
                ResourceId = procResource.Id,
            };
            //TODO 需要添加资源对应工序和条码对应进站工序检查
            //TODO 需要添加拦截NG条码进站检查

            var resEquipentBind = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(resourceEquipmentBindQuery);
            if (resEquipentBind.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19131)).WithData("ResCode", procResource.ResCode).WithData("EquCode", _currentEquipment.Code);
            }
            //验证虚拟条码
            if (inBoundMoreDto.IsVerifyVirtualSFC == true)
            {
                await VerifyVirtualSFCAsync(inBoundMoreDto);
            }

            //查找当前工作中心（产线）
            var workLine = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
            if (workLine == null)
            {
                //通过资源未找到关联产线
                throw new CustomerValidationException(nameof(ErrorCode.MES19123)).WithData("ResourceCode", procResource.ResCode);
            }
            //查找激活工单
            var planWorkOrders = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLine.Id);
            if (!planWorkOrders.Any())
            {
                //产线未激活工单
                throw new CustomerValidationException(nameof(ErrorCode.MES19124)).WithData("WorkCenterCode", workLine.Code);
            }
            //不考虑混线
            var planWorkOrder = planWorkOrders.First();
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(planWorkOrder.Id);
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(inBoundMoreDto.SFCs);
            //查询已经存在条码的生产信息
            var sfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = inBoundMoreDto.SFCs, SiteId = _currentEquipment.SiteId });
            //SFC有条码信息，但已经没有生产信息不允许进站 修改为兼容多段工序
            //var noIncludeSfcs = sfclist.Where(w => sfcProduceList.Select(s => s.SFC.ToUpper()).Contains(w.SFC.ToUpper()) == false).Select(p => p.SFC);
            //if (noIncludeSfcs.Any())
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19126)).WithData("SFCS", string.Join(',', noIncludeSfcs));

            //查询已存在条码的工序信息
            List<ProcProcedureEntity> procedureEntityList = new List<ProcProcedureEntity>();
            var procProcedures = await _procProcedureRepository.GetByIdsAsync(sfcProduceList.Select(c => c.ProcedureId).ToArray());
            procedureEntityList = procProcedures.ToList();

            //复投次数限制，对于测试类型工序，条码复投数量大于等于工序循环次数限制进站
            //var noNeedRepeat = sfcProduceList.Where(c => procedureEntityList.Where(p => p.Type == ProcedureTypeEnum.Test && c.RepeatedCount > (p.Cycle ?? 1))
            //                                             .Select(p => p.Id).Contains(c.ProcedureId)).Select(p => p.SFC);
            //if (noNeedRepeat.Any())
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19130)).WithData("SFCS", string.Join(',', noNeedRepeat));

            //当前所在工序不是测试工序，条码不是排队状态不允许进站
            var noLinUpSFCs = sfcProduceList.Where(c => c.Status != SfcProduceStatusEnum.lineUp
                                                        && procedureEntityList.Where(p => p.Type == ProcedureTypeEnum.Test)
                                                        .Select(p => p.Id).Contains(c.ProcedureId) == false).Select(p => p.SFC);
            if (noLinUpSFCs.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES19129)).WithData("SFCS", string.Join(',', noLinUpSFCs));
            //查询已有的条码信息
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfclist.Select(c => c.Id));

            //获取工艺路线首工序
            var processRouteFirstProcedure = await GetFirstProcedureAsync(planWorkOrderEntity.ProcessRouteId);

            //20231108改动，直接根据设备资源获取工序
            //根据资源获取工序
            var procedureEntity = await _procedureRepository.GetProcProdureByResourceIdAsync(new() { ResourceId = procResource.Id, SiteId = _currentEquipment.SiteId })
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", procResource.ResCode);

            //查询已有汇总信息
            ManuSfcSummaryQuery manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = _currentEquipment.SiteId,
                SFCS = inBoundMoreDto.SFCs
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);


            //#region 先不发布

            ////尾工序进站不允许Pack段出现不合格记录（出现后需要复测）
            //var LastProcedureEntity = await GetLastProcedureAsync(planWorkOrderEntity.ProcessRouteId);
            //var isLast = !sfcProduceList.Any(a => a.ProcedureId != LastProcedureEntity.ProcedureId);

            var sfcs = inBoundMoreDto.SFCs.ToArray();
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new() { CirculationBarCodes = sfcs, SiteId = _currentEquipment.SiteId });


            var includeNoQuality = manuSfcSummaryEntities.Where(c => c.QualityStatus == 0);
            //if (includeNoQuality?.Any() == true)
            //{
            //    //允许进站不合格产品
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19137))
            //        .WithData("SFCS", string.Join(',', includeNoQuality.Select(c => c.SFC)));
            //}
            //刻码后一道工序（挤压-刻码-CCS安装）
            //虚拟组件安装后一道工序（人工组件安装(2)-线束固定与连接排固定）
            //客户要去需要在绑定工序后校验数量是否满足
            if ((new string[] { "OP16", "OP26", "OP28" }).Contains(procedureEntity.Code))
            {
                if (!(manuSfcCirculationEntities?.Any() == true))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19158))
                        .WithData("SFCS", string.Join(',', includeNoQuality.Select(c => c.SFC)));
                }
                else
                {
                    var result = false;

                    if (procedureEntity.Code == "OP16" && manuSfcCirculationEntities?.Count() < 12) result = true;
                    if (procedureEntity.Code == "OP26" && manuSfcCirculationEntities?.Count() < 4) result = true;
                    if (procedureEntity.Code == "OP28" && manuSfcCirculationEntities?.Count() < 4) result = true;

                    if (result)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19159))
                            .WithData("SFCS", string.Join(',', includeNoQuality.Select(c => c.SFC)))
                            .WithData("Count", includeNoQuality.Count());
                    }
                }
            }

            //#endregion

            List<ManuSfcEntity> manuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> manuSfcInfoList = new List<ManuSfcInfoEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new List<ManuSfcStepEntity>();
            List<ManuSfcSummaryEntity> manuSfcSummaryList = new List<ManuSfcSummaryEntity>();
            //更新的信息
            List<ManuSfcProduceEntity> updateManuSfcProduceList = new List<ManuSfcProduceEntity>();
            List<ManuSfcEntity> updateManuSfcList = new List<ManuSfcEntity>();
            List<ManuSfcSummaryEntity> updateManuSfcSummaryList = new List<ManuSfcSummaryEntity>();

            //工单绑定的批次信息
            var bindSFCbox = await _inteSFCBoxRepository.GetByWorkOrderAsync(planWorkOrder.Id);
            var sfcboxQuery = new InteSFCBoxEntityQuery
            {
                SFCs = inBoundMoreDto.SFCs
            };
            //条码查询的批次信息
            var sfcBoxInfo = await _inteSFCBoxRepository.GetManuSFCBoxAsync(sfcboxQuery);

            decimal firstProcedureQty = 0;//首工序进站数量
            foreach (var sfc in inBoundMoreDto.SFCs)
            {
                if (validateBatch)
                {
                    //校验工单是否绑定过电芯批次
                    var currenWorkBatch = bindSFCbox.FirstOrDefault()?.BatchNo;
                    if (currenWorkBatch != null)
                    {
                        //校验电芯码批次是否导入过系统
                        var sfcBatch = sfcBoxInfo.Where(x => x.SFC == sfc).FirstOrDefault();
                        if (sfcBatch != null)
                        {
                            //校验工单和条码是否同批次
                            if (!currenWorkBatch.Equals(sfcBatch.BatchNo))
                            {
                                //throw new CustomerValidationException(nameof(ErrorCode.MES19149)).WithData("SFC", sfc).WithData("sfcBatchNo", sfcBatch.BatchNo).WithData("workBatchNo", currenWorkBatch);
                            }
                        }
                        else
                        {
                            //未查到条码{sfc}批次信息,无法正常校验电芯批次
                            throw new CustomerValidationException(nameof(ErrorCode.MES19157)).WithData("SFC", sfc);
                        }
                    }
                    else
                    {
                        //未查到当前工单{planWorkOrder.code}批次信息,无法正常校验电芯批次
                        throw new CustomerValidationException(nameof(ErrorCode.MES19157)).WithData("SFC", sfc);
                    }
                }

                //汇总信息
                var manuSfcSummaryEntity = manuSfcSummaryEntities.Where(c => c.SFC == sfc).FirstOrDefault();
                //当前条码
                var sfcEntity = sfclist.FirstOrDefault(x => x.SFC == sfc);
                //当前条码生产信息，兼容多段工序，根据工单和条码确认
                var sfcProduceEntity = sfcProduceList.FirstOrDefault(x => x.SFC == sfc && x.WorkOrderId == planWorkOrder.Id);
                if (sfcProduceEntity != null)
                {
                    //进站修改为激活
                    sfcProduceEntity.Status = SfcProduceStatusEnum.Activity;
                    //当前SFC的工序信息
                    var sfcprocedureEntity = procedureEntityList.Where(c => c.Id == sfcProduceEntity.ProcedureId).First();
                    // 检查是否测试工序
                    if (sfcprocedureEntity.Type == ProcedureTypeEnum.Test)
                    {
                        // 超过复投次数，标识为NG
                        //if (sfcProduce.RepeatedCount > sfcprocedureEntity.Cycle) throw new CustomerValidationException(nameof(ErrorCode.MES16036));
                        sfcProduceEntity.RepeatedCount++;
                        //更新复投次数
                        if (manuSfcSummaryEntity != null)
                        {
                            manuSfcSummaryEntity.RepeatedCount = sfcProduceEntity.RepeatedCount;
                            manuSfcSummaryEntity.UpdatedBy = _currentEquipment.Name;
                            manuSfcSummaryEntity.UpdatedOn = HymsonClock.Now();
                            updateManuSfcSummaryList.Add(manuSfcSummaryEntity);
                        }
                    }
                    //是否首工序
                    var isFirstProcedure = await IsFirstProcedureAsync(sfcProduceEntity.ProcessRouteId, sfcProduceEntity.ProcedureId);
                    if (isFirstProcedure)
                    {
                        firstProcedureQty++;
                    }
                    // 初始化步骤
                    var sfcStep = new ManuSfcStepEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = sfcProduceEntity.SiteId,
                        SFC = sfcProduceEntity.SFC,
                        ProductId = sfcProduceEntity.ProductId,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        WorkCenterId = sfcProduceEntity.WorkCenterId,
                        ProductBOMId = sfcProduceEntity.ProductBOMId,
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        Qty = 1,//数量1
                        Operatetype = ManuSfcStepTypeEnum.InStock,
                        CurrentStatus = SfcProduceStatusEnum.Activity,
                        EquipmentId = _currentEquipment.Id,
                        ResourceId = procResource.Id,
                        CreatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentEquipment.Name,
                        UpdatedOn = HymsonClock.Now(),
                    };
                    manuSfcStepList.Add(sfcStep);
                    updateManuSfcProduceList.Add(sfcProduceEntity);
                    //更新条码为已使用
                    if (sfcEntity != null)
                    {
                        sfcEntity.UpdatedBy = _currentEquipment.Name;
                        sfcEntity.UpdatedOn = HymsonClock.Now();
                        sfcEntity.IsUsed = YesOrNoEnum.Yes;
                        updateManuSfcList.Add(sfcEntity);
                    }
                    //查询已有条码信息
                    var sfcInfoEntity = sfcInfoEntities.Where(c => c.SfcId == sfcEntity.Id && c.WorkOrderId != planWorkOrderEntity.Id).FirstOrDefault();
                    //已有条码信息不是当前工单
                    if (sfcInfoEntity == null)
                    {
                        manuSfcInfoList.Add(new ManuSfcInfoEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            SfcId = sfcEntity.Id,
                            WorkOrderId = planWorkOrderEntity.Id,
                            ProductId = planWorkOrderEntity.ProductId,
                            IsUsed = true,
                            CreatedBy = _currentEquipment.Name,
                            UpdatedBy = _currentEquipment.Name
                        });
                    }
                    //条码生产信息
                    //var manuSfcProduceEntity = sfcProduceList.Where(c => c.SFC == sfcEntity.SFC && c.WorkOrderId != planWorkOrderEntity.Id).FirstOrDefault();
                    //if (manuSfcProduceEntity == null)
                    //{
                    //    manuSfcProduceList.Add(new ManuSfcProduceEntity
                    //    {
                    //        Id = IdGenProvider.Instance.CreateId(),
                    //        SiteId = _currentEquipment.SiteId,
                    //        SFCId = sfcEntity.Id,
                    //        SFC = sfc,
                    //        ProductId = planWorkOrderEntity.ProductId,
                    //        WorkOrderId = planWorkOrderEntity.Id,
                    //        BarCodeInfoId = sfcEntity.Id,
                    //        ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    //        WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    //        ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    //        EquipmentId = _currentEquipment.Id ?? 0,
                    //        ResourceId = procResource.Id,
                    //        Qty = 1,//电芯进站默认都是1个
                    //        ProcedureId = processRouteFirstProcedure.ProcedureId,
                    //        Status = SfcProduceStatusEnum.Activity,//接口进站直接为活动
                    //        RepeatedCount = 0,
                    //        IsScrap = TrueOrFalseEnum.No,
                    //        CreatedBy = _currentEquipment.Name,
                    //        UpdatedBy = _currentEquipment.Name
                    //    });
                    //}
                    //允许任意工单条码进站
                    //if ((manuSfcProduceEntity != null && manuSfcProduceEntity.WorkOrderId != planWorkOrderEntity.Id))
                    //{
                    //    manuSfcProduceEntity.WorkOrderId = planWorkOrderEntity.Id;
                    //    manuSfcProduceEntity.UpdatedOn = HymsonClock.Now();
                    //    manuSfcProduceEntity.UpdatedBy = _currentEquipment.Name;
                    //    updateManuSfcProduceList.Add(manuSfcProduceEntity);
                    //}
                    continue;
                }
                //条码表
                var manuSfcEntity = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfc,
                    Qty = 1,//电芯进站默认都是1个
                    IsUsed = YesOrNoEnum.Yes,
                    Status = SfcStatusEnum.InProcess,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                };
                //如果生产信息为空sfcProduceEntity，条码信息 sfcEntity不为空时一般为前段工序完工条码 拿到其他段工序进站
                var manuSfcId = manuSfcEntity.Id;
                if (sfcEntity != null)
                {
                    manuSfcId = sfcEntity.Id;
                }
                else//条码表为空才添加
                {
                    manuSfcList.Add(manuSfcEntity);
                }
                //条码信息
                manuSfcInfoList.Add(new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SfcId = manuSfcId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductId = planWorkOrderEntity.ProductId,
                    IsUsed = true,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                });
                //生产表
                manuSfcProduceList.Add(new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFCId = manuSfcId,
                    SFC = sfc,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    BarCodeInfoId = manuSfcId,
                    ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    EquipmentId = _currentEquipment.Id ?? 0,
                    ResourceId = procResource.Id,
                    Qty = 1,//电芯进站默认都是1个
                    //ProcedureId = processRouteFirstProcedure.ProcedureId,
                    ProcedureId = procedureEntity.Id,
                    Status = SfcProduceStatusEnum.Activity,//接口进站直接为活动
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                });
                //步骤表
                manuSfcStepList.Add(new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfc,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    Qty = 1,//电芯进站默认都是1个
                    //ProcedureId = processRouteFirstProcedure.ProcedureId,
                    ProcedureId = procedureEntity.Id,
                    Operatetype = ManuSfcStepTypeEnum.InStock,
                    CurrentStatus = SfcProduceStatusEnum.Activity,
                    EquipmentId = _currentEquipment.Id,
                    ResourceId = procResource.Id,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                });
                //汇总表
                if (manuSfcSummaryEntity == null)
                {
                    manuSfcSummaryList.Add(new ManuSfcSummaryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentEquipment.SiteId,
                        //ProcedureId = processRouteFirstProcedure.ProcedureId,
                        ProcedureId = procedureEntity.Id,
                        EquipmentId = _currentEquipment.Id ?? 0,
                        ProductId = planWorkOrderEntity.ProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        SFC = sfc,
                        Qty = 1,//电芯进站默认都是1个
                        ResourceId = procResource.Id,
                        BeginTime = HymsonClock.Now(),
                        NgNum = 0,
                        RepeatedCount = 0,
                        CreatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentEquipment.Name,
                        UpdatedOn = HymsonClock.Now(),
                    });
                }
                else//条码其他段投入，更新工单信息
                {
                    manuSfcSummaryEntity.WorkOrderId = planWorkOrderEntity.Id;
                    manuSfcSummaryEntity.UpdatedBy = _currentEquipment.Name;
                    manuSfcSummaryEntity.UpdatedOn = HymsonClock.Now();
                    updateManuSfcSummaryList.Add(manuSfcSummaryEntity);
                }
            }

            using var ts = TransactionHelper.GetTransactionScope();
            //更新下达数量
            var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
            {
                WorkOrderId = planWorkOrderEntity.Id,
                PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100),
                PassDownQuantity = firstProcedureQty,
                UserName = _currentEquipment.Name,
                UpdateDate = HymsonClock.Now()
            });
            //更新投入数量
            row += await _planWorkOrderRepository.UpdateInputQtyByWorkOrderIdAsync(new UpdateQtyCommand
            {
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                WorkOrderId = planWorkOrderEntity.Id,
                Qty = firstProcedureQty,
            });

            // 更新工单统计表的 RealStart
            row += await _planWorkOrderRepository.UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
            {
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                WorkOrderIds = new long[] { planWorkOrderEntity.Id }
            });

            //更新manuSfcProduce
            if (updateManuSfcProduceList.Any())
            {
                row += await _manuSfcProduceRepository.UpdateRangeAsync(updateManuSfcProduceList);
            }
            //更新updateManuSfcList
            if (updateManuSfcList.Any())
            {
                row += await _manuSfcRepository.UpdateRangeAsync(updateManuSfcList);
            }
            //更新汇总信息
            if (updateManuSfcSummaryList.Any())
            {
                row += await _manuSfcSummaryRepository.InsertOrUpdateRangeAsync(updateManuSfcSummaryList);
            }
            //新增汇总信息
            if (manuSfcSummaryList.Any())
            {
                await _manuSfcSummaryRepository.InsertsAsync(manuSfcSummaryList);
            }
            await _manuSfcRepository.InsertRangeAsync(manuSfcList);
            await _manuSfcInfoRepository.InsertsAsync(manuSfcInfoList);
            await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList);
            //if (await _manuSfcProduceRepository.InsertRangeAsync(manuSfcProduceList) == 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19148)).WithData("SFC", string.Join(',', manuSfcProduceList)).WithData("action", "插入在制信息");
            //}
            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
            ts.Complete();
        }

        /// <summary>
        /// 校验虚拟码
        /// </summary>
        /// <param name="inBoundMoreDto"></param>
        /// <returns></returns>
        private async Task VerifyVirtualSFCAsync(InBoundMoreDto inBoundMoreDto)
        {
            //SFC流转信息处理
            var manuSfcCirculationBarCodeQuery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Change,
                IsDisassemble = TrueOrFalseEnum.No,
                SiteId = _currentEquipment.SiteId,
                Sfcs = inBoundMoreDto.SFCs.Select(sfc => sfc).ToArray()
            };
            var manuSfcCirculationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodeQuery);
            //如果有不存在的SFC就提示
            var noIncludeSfcs = inBoundMoreDto.SFCs.Where(sfc => manuSfcCirculationBarCodeEntities.Select(s => s.SFC).Contains(sfc) == false);
            if (noIncludeSfcs.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19134)).WithData("SFCS", string.Join(',', noIncludeSfcs));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<bool> IsFirstProcedureAsync(long processRouteId, long procedureId)
        {
            var firstProcedureDetailNodeEntity = await _procProcessRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10435));

            return firstProcedureDetailNodeEntity.ProcedureId == procedureId;
        }

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcessRouteProcedureDto> GetFirstProcedureAsync(long processRouteId)
        {
            var procProcessRouteDetailNodeEntity = await _processRouteDetailNodeRepository.GetFirstProcedureByProcessRouteIdAsync(processRouteId);
            if (procProcessRouteDetailNodeEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16304));

            var procProcedureEntity = await _procedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
            if (procProcedureEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10406));

            return new ProcessRouteProcedureDto
            {
                ProcessRouteId = processRouteId,
                SerialNo = procProcessRouteDetailNodeEntity.SerialNo,
                ProcedureId = procProcessRouteDetailNodeEntity.ProcedureId,
                CheckType = procProcessRouteDetailNodeEntity.CheckType,
                CheckRate = procProcessRouteDetailNodeEntity.CheckRate,
                IsWorkReport = procProcessRouteDetailNodeEntity.IsWorkReport,
                ProcedureCode = procProcedureEntity.Code,
                ProcedureName = procProcedureEntity.Name,
                Type = procProcedureEntity.Type,
                PackingLevel = procProcedureEntity.PackingLevel,
                ResourceTypeId = procProcedureEntity.ResourceTypeId,
                Cycle = procProcedureEntity.Cycle,
                IsRepairReturn = procProcedureEntity.IsRepairReturn
            };
        }

        /// <summary>
        /// 获取尾工序
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<ProcessRouteProcedureDto> GetLastProcedureAsync(long processRouteId)
        {
            var procProcessRouteDetailNodeEntity = await _processRouteDetailNodeRepository.GetLastProcedureByProcessRouteIdAsync(processRouteId);
            if (procProcessRouteDetailNodeEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES16356));

            var procProcedureEntity = await _procedureRepository.GetByIdAsync(procProcessRouteDetailNodeEntity.ProcedureId);
            if (procProcedureEntity == null) throw new CustomerValidationException(nameof(ErrorCode.MES10406));

            return new ProcessRouteProcedureDto
            {
                ProcessRouteId = processRouteId,
                SerialNo = procProcessRouteDetailNodeEntity.SerialNo,
                ProcedureId = procProcessRouteDetailNodeEntity.ProcedureId,
                CheckType = procProcessRouteDetailNodeEntity.CheckType,
                CheckRate = procProcessRouteDetailNodeEntity.CheckRate,
                IsWorkReport = procProcessRouteDetailNodeEntity.IsWorkReport,
                ProcedureCode = procProcedureEntity.Code,
                ProcedureName = procProcedureEntity.Name,
                Type = procProcedureEntity.Type,
                PackingLevel = procProcedureEntity.PackingLevel,
                ResourceTypeId = procProcedureEntity.ResourceTypeId,
                Cycle = procProcedureEntity.Cycle,
                IsRepairReturn = procProcedureEntity.IsRepairReturn
            };
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="baseDto"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderDto> GetWorkOrderAsync(BaseDto baseDto)
        {
            PlanWorkOrderDto planWorkOrderDto = new();

            long _site = _currentEquipment.SiteId;
            if (_site == 0)
            {
                _site = (long)123456;
            }
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _site, Code = baseDto.ResourceCode });

            //查找当前工作中心（产线）
            var workLine = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
            if (workLine == null)
            {
                //通过资源未找到关联产线
                throw new CustomerValidationException(nameof(ErrorCode.MES19123)).WithData("ResourceCode", procResource.ResCode);
            }
            //查找激活工单
            var planWorkOrders = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLine.Id);
            if (!planWorkOrders.Any())
            {
                //产线未激活工单
                throw new CustomerValidationException(nameof(ErrorCode.MES19124)).WithData("WorkCenterCode", workLine.Code);
            }
            //不考虑混线
            var planWorkOrder = planWorkOrders.First();
            //var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(planWorkOrder.Id);

            //产品ID
            var procMaterials = await _procMaterialRepository.GetByIdAsync(planWorkOrder.ProductId);

            planWorkOrderDto.OrderCode = planWorkOrder.OrderCode;
            planWorkOrderDto.MaterialCode = procMaterials?.MaterialCode ?? "";
            planWorkOrderDto.MaterialName = procMaterials?.MaterialName ?? "";

            return planWorkOrderDto;
        }
    }
}