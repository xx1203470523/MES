using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Parameter.Query;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.EquipmentServices.Dtos.OutBound;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using System.Text.Json;

namespace Hymson.MES.EquipmentServices.Services.OutBound
{
    /// <summary>
    /// 出站服务
    /// </summary>
    public class OutBoundService : IOutBoundService
    {
        #region Repository
        private readonly ICurrentEquipment _currentEquipment;
        /// <summary>
        /// 序列号服务
        /// </summary>
        private readonly ISequenceService _sequenceService;
        private readonly AbstractValidator<OutBoundDto> _validationOutBoundDtoRules;
        private readonly AbstractValidator<OutBoundMoreDto> _validationOutBoundMoreDtoRules;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IManuProductParameterRepository _manuProductParameterRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuSfcStepNgRepository _manuSfcStepNgRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteDetailLinkRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcStepMaterialRepository _manuSfcStepMaterialRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;

        public OutBoundService(AbstractValidator<OutBoundDto> validationOutBoundDtoRules,
            ICurrentEquipment currentEquipment,
            AbstractValidator<OutBoundMoreDto> validationOutBoundMoreDtoRules,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository procResourceRepository,
            IProcParameterRepository procParameterRepository,
            IManuProductParameterRepository manuProductParameterRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IManuSfcStepNgRepository manuSfcStepNgRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcedureRepository procProcedureRepository,
            ISequenceService sequenceService,
            IProcProcessRouteDetailLinkRepository procProcessRouteDetailLinkRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IManuSfcStepMaterialRepository manuSfcStepMaterialRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository)
        {
            _validationOutBoundDtoRules = validationOutBoundDtoRules;
            _currentEquipment = currentEquipment;
            _validationOutBoundMoreDtoRules = validationOutBoundMoreDtoRules;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procResourceRepository = procResourceRepository;
            _procParameterRepository = procParameterRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuSfcStepNgRepository = manuSfcStepNgRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcedureRepository = procProcedureRepository;
            _sequenceService = sequenceService;
            _procProcessRouteDetailLinkRepository = procProcessRouteDetailLinkRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _manuSfcStepMaterialRepository = manuSfcStepMaterialRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }
        #endregion

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundDto"></param>
        /// <returns></returns>
        public async Task OutBoundAsync(OutBoundDto outBoundDto)
        {
            await _validationOutBoundDtoRules.ValidateAndThrowAsync(outBoundDto);
            if (outBoundDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            //使用批量出站Dto
            OutBoundMoreDto outBoundMoreDto = new()
            {
                EquipmentCode = _currentEquipment.Code,
                ResourceCode = outBoundDto.ResourceCode,
                LocalTime = outBoundDto.LocalTime,
                SFCs = new OutBoundSFCDto[] {
                    new OutBoundSFCDto{
                        SFC=outBoundDto.SFC,
                        BindFeedingCodes= outBoundDto.BindFeedingCodes,
                        NG=outBoundDto.NG,
                        ParamList=outBoundDto.ParamList,
                        Passed = outBoundDto.Passed,
                        IsPassingStation = outBoundDto.IsPassingStation,
                        IsBindVirtualSFC = outBoundDto.IsBindVirtualSFC
                    }
                }
            };
            await SFCOutBoundAsync(outBoundMoreDto);
        }

        /// <summary>
        /// 出站（多个）
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        public async Task OutBoundMoreAsync(OutBoundMoreDto outBoundMoreDto)
        {
            await _validationOutBoundMoreDtoRules.ValidateAndThrowAsync(outBoundMoreDto);
            if (outBoundMoreDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (outBoundMoreDto.SFCs.Length <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19101));
            }

            await SFCOutBoundAsync(outBoundMoreDto);
        }

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>

        public async Task SFCOutBoundAsync(OutBoundMoreDto outBoundMoreDto)
        {
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = outBoundMoreDto.ResourceCode });
            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = _currentEquipment.SiteId,
                Ids = new long[] { _currentEquipment.Id ?? 0 },
                ResourceId = procResource.Id
            };

            //根据设备资源获取工序
            //根据资源获取工序
            var procedureEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new() { ResourceId = procResource.Id, SiteId = _currentEquipment.SiteId })
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES19913)).WithData("ResCode", procResource.ResCode);

            //出站时IsBindVirtualSFC 为true 不能多条一起出站
            var bindVirtualSfcCount = outBoundMoreDto.SFCs.Where(c => c.IsBindVirtualSFC == true).Count();
            if (bindVirtualSfcCount > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19135));
            }
            //出站传入NG数据时Passed字段应传0
            var ngPassedSfcs = outBoundMoreDto.SFCs.Where(c => c.NG != null && c.NG.Any() && c.Passed == 1);
            if (ngPassedSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19136)).WithData("SFCS", string.Join(',', ngPassedSfcs.Select(c => c.SFC)));
            }

            var resEquipentBind = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(resourceEquipmentBindQuery);
            if (resEquipentBind.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19131)).WithData("ResCode", procResource.ResCode).WithData("EquCode", _currentEquipment.Code);
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

            var sfcs = outBoundMoreDto.SFCs.Select(c => c.SFC).ToArray();
            //条码信息
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(sfcs);
            //查询已经存在条码的生产信息
            var sfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentEquipment.SiteId });
            //如果有不存在的SFC就提示
            var noIncludeSfcs = sfcs.Where(sfc => sfclist.Select(s => s.SFC).Contains(sfc) == false);
            if (noIncludeSfcs.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19125)).WithData("SFCS", string.Join(',', noIncludeSfcs));

            //SFC有条码信息，但已经没有生产信息不允许出站
            var noProduceSfcs = sfclist.Where(w => sfcProduceList.Select(s => s.SFC).Contains(w.SFC) == false).Select(p => p.SFC);
            if (noProduceSfcs.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES19126)).WithData("SFCS", string.Join(',', noProduceSfcs));

            //排队中的条码没进站不允许出站
            if (sfcProduceList.Any())
            {
                //必须进站再出站  添加工单条件,不为本工单允许出站
                var outBoundMoreSfcs = outBoundMoreDto.SFCs.Where(w =>
                                            sfcProduceList.Where(c => c.Status == SfcProduceStatusEnum.lineUp && c.WorkOrderId == planWorkOrder.Id)
                                            .Select(s => s.SFC)
                                            .Contains(w.SFC) && w.IsPassingStation != true);
                if (outBoundMoreSfcs.Any())
                    //非过站,在制条码为排队中，激活的工单与在制工单相同，且标识为进站
                    throw new CustomerValidationException(nameof(ErrorCode.MES19127)).WithData("SFCS", string.Join(',', outBoundMoreSfcs.Select(c => c.SFC)));
            }
            //已经进站条码不允许过站
            if (sfcProduceList.Any())
            {

                var outBoundMoreSfcs = outBoundMoreDto.SFCs.Where(w =>
                                            sfcProduceList.Where(c => c.Status == SfcProduceStatusEnum.Activity)
                                            .Select(s => s.SFC)
                                            .Contains(w.SFC) && w.IsPassingStation == true);
                //标识为过站,同时在制又是加工中，抛出错误提示
                if (outBoundMoreSfcs.Any())
                    throw new CustomerValidationException(nameof(ErrorCode.MES19128)).WithData("SFCS", string.Join(',', outBoundMoreSfcs.Select(c => c.SFC)));
            }
            //保存条码当前所在工序,出站条码去一个即可
            var currentProcedureId = sfcProduceList.OrderByDescending(x => x.CreatedOn).First().ProcedureId;

            //查询已有汇总信息
            ManuSfcSummaryQuery manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = _currentEquipment.SiteId,
                EquipmentId = _currentEquipment.Id,
                //ProcedureIds = new long[] { currentProcedureId },
                SFCS = sfclist.Select(c => c.SFC).ToArray()
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);

            //条码流转信息
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new List<ManuSfcCirculationEntity>();
            List<string> delManuSfcProduces = new List<string>();
            List<ManuSfcStepEntity> manuSfcStepEntities = new List<ManuSfcStepEntity>();
            List<ManuSfcEntity> manuSfcEntities = new List<ManuSfcEntity>();
            List<ManuSfcProduceEntity> manuSfcProduceEntities = new List<ManuSfcProduceEntity>();
            //更新汇总
            List<ManuSfcSummaryEntity> manuSfcSummaryUpdateOrInsertList = new List<ManuSfcSummaryEntity>();
            //SFC信息处理
            foreach (var outBoundSFCDto in outBoundMoreDto.SFCs)
            {
                var sfcEntity = sfclist.Where(c => c.SFC == outBoundSFCDto.SFC).First();
                //var sfcProduceEntity = sfcProduceList.Where(c => c.SFC == outBoundSFCDto.SFC && c.WorkOrderId == planWorkOrderEntity.Id).FirstOrDefault();
                var sfcProduceEntity = sfcProduceList.Where(c => c.SFC == outBoundSFCDto.SFC).FirstOrDefault();
                if (sfcProduceEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18023)).WithData("SFC", outBoundSFCDto.SFC).WithData("OrderCode", planWorkOrderEntity.OrderCode);
                }
                //汇总信息
                var manuSfcSummaryEntity = manuSfcSummaryEntities.Where(c => c.SFC == outBoundSFCDto.SFC).FirstOrDefault();
                // 更新时间
                sfcProduceEntity.UpdatedBy = _currentEquipment.Name;
                sfcProduceEntity.UpdatedOn = HymsonClock.Now();
                //步骤记录
                var manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    ProductId = planWorkOrderEntity.ProductId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    SFC = outBoundSFCDto.SFC,
                    Qty = 1,//出站数量
                    Passed = outBoundSFCDto.Passed,//是否合格
                    EquipmentId = _currentEquipment.Id,
                    ResourceId = procResource.Id,
                    CurrentStatus = SfcProduceStatusEnum.Activity,
                    Operatetype = ManuSfcStepTypeEnum.OutStock,
                    ProcedureId = currentProcedureId,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                    IsPassingStation = outBoundSFCDto.IsPassingStation//是否是过站
                };
                //如果是过站
                if (outBoundSFCDto.IsPassingStation)
                {
                    #region 严格按照工艺路线生产

                    //过站没有进站动作，也需要校验部分进站逻辑
                    // 校验设备资源对应的工序和在制工序是否一致
                    if (procedureEntity.Id != sfcProduceEntity.ProcedureId)
                    {
                        var msgSfc = outBoundMoreDto.SFCs.Select(a => a.SFC);
                        throw new CustomerValidationException(nameof(ErrorCode.MES19161)).WithData("SFC", string.Join(",", msgSfc));
                    }

                    #endregion

                    //复制对象
                    var manuSfcStepPassingEntity = JsonSerializer.Deserialize<ManuSfcStepEntity>(JsonSerializer.Serialize(manuSfcStepEntity));
                    if (manuSfcStepPassingEntity != null)
                    {
                        //插入 manu_sfc_step 状态为 进站
                        manuSfcStepPassingEntity.Id = IdGenProvider.Instance.CreateId();
                        manuSfcStepPassingEntity.Operatetype = ManuSfcStepTypeEnum.InStock;
                        manuSfcStepPassingEntity.CreatedOn = HymsonClock.Now().AddSeconds(-1);//方便区分进站和出站时间
                        manuSfcStepPassingEntity.UpdatedOn = HymsonClock.Now().AddSeconds(-1);
                        manuSfcStepEntities.Add(manuSfcStepPassingEntity);
                    }
                }
                // 获取下一个工序（如果没有了，就表示完工） TODO 这里GetNextProcedureAsync里需要缓存
                var nextProcedure = await GetNextProcedureAsync(sfcProduceEntity.WorkOrderId, sfcProduceEntity.ProcessRouteId, sfcProduceEntity.ProcedureId);
                //完工
                if (nextProcedure == null)
                {
                    //完工删除
                    delManuSfcProduces.Add(manuSfcStepEntity.SFC);
                    //记录流转信息
                    manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = sfcProduceEntity.SiteId,
                        ProcedureId = sfcProduceEntity.ProcedureId,
                        ResourceId = sfcProduceEntity.ResourceId,
                        SFC = sfcProduceEntity.SFC,
                        WorkOrderId = sfcProduceEntity.WorkOrderId,
                        ProductId = sfcProduceEntity.ProductId,
                        EquipmentId = _currentEquipment.Id,
                        CirculationBarCode = "",
                        CirculationProductId = manuSfcStepEntity.ProductId,
                        CirculationMainProductId = manuSfcStepEntity.ProductId,
                        CirculationQty = 1,
                        CirculationType = SfcCirculationTypeEnum.Consume,
                        CreatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentEquipment.Name,
                        UpdatedOn = HymsonClock.Now()
                    });
                    //更新状态
                    manuSfcStepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock; // TODO 这里的状态？？
                    manuSfcStepEntity.CurrentStatus = SfcProduceStatusEnum.Complete; // TODO 这里的状态？？
                    manuSfcStepEntities.Add(manuSfcStepEntity);
                    //更新条码信息
                    sfcEntity.Status = SfcStatusEnum.Complete;
                    sfcEntity.UpdatedBy = _currentEquipment.Name;
                    sfcEntity.UpdatedOn = HymsonClock.Now();
                    manuSfcEntities.Add(sfcEntity);
                    //Sfc生产信息
                    manuSfcProduceEntities.Add(sfcProduceEntity);
                }
                else//未完工
                {
                    // 修改 manu_sfc_produce 为排队, 工序修改为下一工序的id
                    sfcProduceEntity.Status = SfcProduceStatusEnum.lineUp;
                    sfcProduceEntity.ProcedureId = nextProcedure.Id;
                    manuSfcProduceEntities.Add(sfcProduceEntity);
                    //插入 manu_sfc_step 状态为 出站
                    manuSfcStepEntity.Id = IdGenProvider.Instance.CreateId();
                    manuSfcStepEntity.CurrentStatus = SfcProduceStatusEnum.Complete;//出站修改状态为当前工序完成
                    manuSfcStepEntity.Operatetype = ManuSfcStepTypeEnum.OutStock;
                    manuSfcStepEntities.Add(manuSfcStepEntity);
                }

                //汇总表
                if (manuSfcSummaryEntity == null)//如果过站汇总信息为空
                {
                    manuSfcSummaryUpdateOrInsertList.Add(new ManuSfcSummaryEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentEquipment.SiteId,
                        ProcedureId = currentProcedureId,
                        EquipmentId = _currentEquipment.Id ?? 0,
                        ProductId = planWorkOrderEntity.ProductId,
                        WorkOrderId = planWorkOrderEntity.Id,
                        SFC = outBoundSFCDto.SFC,
                        Qty = 1,
                        ResourceId = procResource.Id,
                        BeginTime = HymsonClock.Now().AddSeconds(-1),//方便区分进站和出站时间
                        EndTime = HymsonClock.Now(),
                        NgNum = outBoundSFCDto.Passed == 0 ? 1 : 0,//不合格+1
                        FirstQualityStatus = outBoundSFCDto.Passed == 0 ? 0 : 1,
                        QualityStatus = outBoundSFCDto.Passed == 0 ? 0 : 1,
                        RepeatedCount = 0,
                        CreatedBy = _currentEquipment.Name,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedBy = _currentEquipment.Name,
                        UpdatedOn = HymsonClock.Now(),
                    });
                }
                else
                {
                    manuSfcSummaryEntity.EndTime = HymsonClock.Now();
                    if (manuSfcSummaryEntity.NgNum == 0)
                    {
                        manuSfcSummaryEntity.FirstQualityStatus = outBoundSFCDto.Passed == 0 ? 0 : 1;
                    }
                    //最终合格状态
                    manuSfcSummaryEntity.QualityStatus = outBoundSFCDto.Passed == 0 ? 0 : 1;
                    if (outBoundSFCDto.Passed == 0)
                    {
                        manuSfcSummaryEntity.NgNum += 1;//不合格+1
                    }
                    manuSfcSummaryEntity.UpdatedBy = _currentEquipment.Name;
                    manuSfcSummaryEntity.UpdatedOn = HymsonClock.Now();
                    manuSfcSummaryUpdateOrInsertList.Add(manuSfcSummaryEntity);
                }
            }

            //虚拟条码绑定流转信息处理
            var manuSfcCirculationUpdateEntities = await BindVirtualSFCCirculationAsync(outBoundMoreDto);
            //虚拟条码更新为实际模组条码
            var manuProductParameterUpdateEntities = await BindBirtualProductParameterAsync(outBoundMoreDto);

            //SFC关联绑定流转信息处理
            var manuSfcCirculationBarCodeQuery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Merge,
                ResourceId = procResource.Id,
                IsDisassemble = TrueOrFalseEnum.No,
                SiteId = _currentEquipment.SiteId,
                CirculationBarCodes = outBoundMoreDto.SFCs.Select(c => c.SFC).ToArray()
            };
            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodeQuery);
            if (circulationBarCodeEntities.Any())
            {
                //条码信息
                var circulationSfc = circulationBarCodeEntities.Select(c => c.SFC).ToArray();
                var circulationSfclist = await _manuSfcRepository.GetBySFCsAsync(circulationSfc);
                foreach (var sfc in circulationSfclist)
                {
                    //更新条码信息
                    sfc.Status = SfcStatusEnum.Complete;
                    sfc.UpdatedBy = _currentEquipment.Name;
                    sfc.UpdatedOn = HymsonClock.Now();
                    manuSfcEntities.Add(sfc);
                }
                //查询已经存在条码的生产信息
                var circulationSfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = circulationSfc, SiteId = _currentEquipment.SiteId });
                foreach (var sfc in circulationSfcProduceList)
                {
                    sfc.UpdatedBy = _currentEquipment.Name;
                    sfc.UpdatedOn = HymsonClock.Now();
                    sfc.Status = SfcProduceStatusEnum.Complete;
                    manuSfcProduceEntities.Add(sfc);
                }
            }

            //产品参数&标准参数
            var (manuProductParameterEntities, procParameterEntities) = await PrepareProductParameterEntityAsync(outBoundMoreDto, manuSfcStepEntities, procResource.Id);
            //NG
            List<ManuSfcStepNgEntity> manuProductNGEntities = await PrepareProductNgEntityAsync(outBoundMoreDto, manuSfcStepEntities);
            //批次码信息
            List<ManuSfcStepMaterialEntity> manuProductMaterialEntities = PrepareProductMaterialEntity(outBoundMoreDto, manuSfcStepEntities);

            //数据提交
            using var trans = TransactionHelper.GetTransactionScope();
            int rows = 0;
            // 添加流转记录
            if (manuSfcCirculationEntities.Any())
            {
                rows += await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);
            }
            //修改原有虚拟条码绑定记录
            if (manuSfcCirculationUpdateEntities.Any())
            {
                rows += await _manuSfcCirculationRepository.UpdateRangeAsync(manuSfcCirculationUpdateEntities);
            }
            //删除完工条码信息
            if (delManuSfcProduces.Any())
            {
                rows += await _manuSfcProduceRepository.DeletePhysicalRangeAsync(new DeletePhysicalBySfcsCommand()
                {
                    SiteId = _currentEquipment.SiteId,
                    Sfcs = delManuSfcProduces
                });
            }
            if (manuSfcStepEntities.Any())
            {
                //添加步骤记录
                rows += await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepEntities);
            }
            // 更新状态
            if (manuSfcEntities.Any())
            {
                rows += await _manuSfcRepository.UpdateRangeAsync(manuSfcEntities);
            }
            //更新sfc生成信息
            if (manuSfcProduceEntities.Any())
            {
                rows += await _manuSfcProduceRepository.UpdateRangeAsync(manuSfcProduceEntities);
            }
            //标准参数
            if (procParameterEntities.Any())
            {
                rows += await _procParameterRepository.InsertRangeAsync(procParameterEntities);
            }
            //产品参数信息
            if (manuProductParameterEntities.Any())
            {
                foreach (var entity in manuProductParameterEntities)
                {
                    var sfcProduceEntity = sfcProduceList.Where(c => c.SFC == entity.SFC).First();
                    entity.ProcedureId = currentProcedureId;
                    entity.ProductId = sfcProduceEntity.ProductId;
                    entity.WorkOrderId = sfcProduceEntity.WorkOrderId;
                }
                rows += await _manuProductParameterRepository.InsertsAsync(manuProductParameterEntities);
            }
            //虚拟条码产品参数更新
            if (manuProductParameterUpdateEntities.Any())
            {
                foreach (var entity in manuProductParameterUpdateEntities)
                {
                    var sfcProduceEntity = sfcProduceList.Where(c => c.SFC == entity.SFC).First();
                    entity.ProcedureId = currentProcedureId;
                    entity.ProductId = sfcProduceEntity.ProductId;
                    entity.WorkOrderId = sfcProduceEntity.WorkOrderId;
                }
                rows += await _manuProductParameterRepository.UpdateRangeAsync(manuProductParameterUpdateEntities);
            }
            //Ng信息
            if (manuProductNGEntities.Any())
            {
                rows += await _manuSfcStepNgRepository.InsertsAsync(manuProductNGEntities);
            }
            //批次码信息
            if (manuProductMaterialEntities.Any())
            {
                rows += await _manuSfcStepMaterialRepository.InsertsAsync(manuProductMaterialEntities);
            }
            // 更新工单统计表的 RealEnd
            //rows += await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(new UpdateWorkOrderRealTimeCommand
            //{
            //    UpdatedOn = HymsonClock.Now(),
            //    UpdatedBy = _currentEquipment.Name,
            //    WorkOrderIds = new long[] { planWorkOrder.Id }
            //});

            // 更新完工数量
            rows += await _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdAsync(new UpdateQtyCommand
            {
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now(),
                WorkOrderId = planWorkOrder.Id,
                Qty = outBoundMoreDto.SFCs.Length,
            });

            //新增或更新汇总信息
            if (manuSfcSummaryUpdateOrInsertList.Any())
            {
                //更新NG数量程序中计算好更新
                rows += await _manuSfcSummaryRepository.InsertOrUpdateRangeAsync(manuSfcSummaryUpdateOrInsertList);
            }
            trans.Complete();
        }

        /// <summary>
        /// 绑定前段工序传递的虚拟条码
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcCirculationEntity>> BindVirtualSFCCirculationAsync(OutBoundMoreDto outBoundMoreDto)
        {
            //虚拟码绑定处理
            List<ManuSfcCirculationEntity> manuSfcCirculationUpdateEntities = new List<ManuSfcCirculationEntity>();
            //虚拟条码绑定时只会存在一个SFC同时出站（业务调研时确认过）
            var outBindVirtualSFC = outBoundMoreDto.SFCs.Where(c => c.IsBindVirtualSFC == true)?.FirstOrDefault();
            if (outBindVirtualSFC != null)
            {
                var bindVirtualSFCCirculationBarCodeQuery = new ManuSfcCirculationBarCodeQuery
                {
                    CirculationType = SfcCirculationTypeEnum.Change,//虚拟条码绑定关系为转换
                    IsDisassemble = TrueOrFalseEnum.No,
                    SiteId = _currentEquipment.SiteId,
                    CirculationBarCode = ManuProductParameter.DefaultSFC
                };
                var manuSfcCirculationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(bindVirtualSFCCirculationBarCodeQuery);
                foreach (var item in manuSfcCirculationBarCodeEntities)
                {
                    item.CirculationType = SfcCirculationTypeEnum.Change;
                    item.CirculationBarCode = outBindVirtualSFC.SFC;
                    item.UpdatedBy = _currentEquipment.Name;
                    item.UpdatedOn = HymsonClock.Now();
                    manuSfcCirculationUpdateEntities.Add(item);
                }
            }
            return manuSfcCirculationUpdateEntities;
        }

        /// <summary>
        /// 前段工序传递的虚拟条码参数
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuProductParameterEntity>> BindBirtualProductParameterAsync(OutBoundMoreDto outBoundMoreDto)
        {
            //虚拟码绑定处理
            var manuProductParameterEntities = new List<ManuProductParameterEntity>();
            //虚拟条码绑定时只会存在一个SFC同时出站
            var outBindVirtualSFC = outBoundMoreDto.SFCs.Where(c => c.IsBindVirtualSFC == true)?.FirstOrDefault();
            if (outBindVirtualSFC != null)
            {
                var manuProductParameterQuery = new ManuProductParameterQuery
                {
                    SiteId = _currentEquipment.SiteId,
                    SFC = ManuProductParameter.DefaultSFC
                };
                var manuProductParameters = await _manuProductParameterRepository.GetManuProductParameterAsync(manuProductParameterQuery);
                foreach (var item in manuProductParameters)
                {
                    item.SFC = outBindVirtualSFC.SFC;
                    item.UpdatedBy = _currentEquipment.Name;
                    item.UpdatedOn = HymsonClock.Now();
                    manuProductParameterEntities.Add(item);
                }
            }
            return manuProductParameterEntities;
        }

        /// <summary>
        /// 获当前工序对应的下一工序
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity?> GetNextProcedureAsync(long workOrderId, long processRouteId, long procedureId)
        {
            ProcProcedureEntity result = new();

            // 因为可能有分叉，所以返回的下一步工序是集合
            var netxtProcessRouteDetailLinks = await _procProcessRouteDetailLinkRepository.GetNextProcessRouteDetailLinkAsync(new ProcProcessRouteDetailLinkQuery
            {
                ProcessRouteId = processRouteId,
                ProcedureId = procedureId
            });
            if (netxtProcessRouteDetailLinks == null || netxtProcessRouteDetailLinks.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES10440));

            // 获取当前工序在工艺路线里面的扩展信息（这里存放是Node表的工序ID，而不是主键ID，后期建议改为主键ID）
            var procedureNodes = await _procProcessRouteDetailNodeRepository.GetByProcedureIdsAsync(new ProcProcessRouteDetailNodesQuery
            {
                ProcessRouteId = processRouteId,
                ProcedureIds = netxtProcessRouteDetailLinks.Select(s => s.ProcessRouteDetailId)
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10440));

            // 这个Key太长了
            //var cacheKey = $"{manuSfcProduce.ProcessRouteId}-{manuSfcProduce.ProcedureId}-{manuSfcProduce.ResourceId}-{manuSfcProduce.WorkOrderId}";

            // 默认下一工序
            ProcProcessRouteDetailNodeEntity? defaultNextProcedure = null;

            // 有多工序分叉的情况
            if (procedureNodes.Count() > 1)
            {

                // 检查是否有"空值"类型的工序
                defaultNextProcedure = procedureNodes.FirstOrDefault(f => f.CheckType == ProcessRouteInspectTypeEnum.None)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10441));
                // 随机工序Key
                var cacheKey = $"{procedureId}-{workOrderId}";
                var count = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.None, cacheKey, 9);
                // 如果不是第一次走该工序，count是从1开始，不包括0。
                if (count > 1)
                {
                    // 抽检类型不为空值的下一工序
                    var nextProcedureOfNone = procedureNodes.FirstOrDefault(f => f.CheckType != ProcessRouteInspectTypeEnum.None)
                        ?? throw new CustomerValidationException(nameof(ErrorCode.MES10447));

                    // 判断工序抽检比例
                    if (nextProcedureOfNone.CheckType == ProcessRouteInspectTypeEnum.FixedScale
                        && nextProcedureOfNone.CheckRate == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10446));

                    // 如果满足抽检次数，就取出一个非"空值"的随机工序作为下一工序
                    if (count > 1 && count % nextProcedureOfNone.CheckRate == 0) defaultNextProcedure = nextProcedureOfNone;
                }
            }
            // 没有分叉的情况
            else
            {
                // 抽检类型不为空值的下一工序
                defaultNextProcedure = procedureNodes.FirstOrDefault()
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10440));

                switch (defaultNextProcedure.CheckType)
                {
                    case ProcessRouteInspectTypeEnum.FixedScale:
                        // 判断工序抽检比例
                        if (defaultNextProcedure.CheckRate == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10446));
                        break;
                    case ProcessRouteInspectTypeEnum.None:
                    case ProcessRouteInspectTypeEnum.RandomInspection:
                    case ProcessRouteInspectTypeEnum.SpecialSamplingInspection:
                    default:
                        break;
                }
            }

            // 获取下一工序
            if (defaultNextProcedure == null) throw new CustomerValidationException(nameof(ErrorCode.MES10440));
            result = await _procProcedureRepository.GetByIdAsync(defaultNextProcedure.ProcedureId);

            ////是否报工（该工序设备是否上传），如果该工序不上传则继续获取下一个工序
            ////不上报工序：OP12挤压，OP23模组入箱，OP24模组人工固定，OP32模组固定2，组件安装OP25
            ////BMU是从气密检测进站，涂胶出站，
            ////客户调整工艺路线，去掉不上报工序
            //if (defaultNextProcedure.IsWorkReport == 0)
            //    return await GetNextProcedureAsync(workOrderId, processRouteId, defaultNextProcedure.ProcedureId);
            //else
            return result;
        }

        /// <summary>
        /// 组装参数信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <param name="procResourceId"></param>
        /// <returns></returns>
        private async Task<(List<ManuProductParameterEntity>, List<ProcParameterEntity>)> PrepareProductParameterEntityAsync(OutBoundMoreDto outBoundMoreDto, List<ManuSfcStepEntity> manuSfcStepEntities, long procResourceId)
        {
            List<ManuProductParameterEntity> manuProductParameterEntities = new();
            List<ProcParameterEntity> procParameterEntities = new();
            //所有参数
            List<string> paramCodeList = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                if (item.ParamList != null)
                {
                    //系统中参数编码为大写
                    var paramCodes = item.ParamList.Select(c => c.ParamCode.ToUpper());
                    paramCodeList.AddRange(paramCodes);
                }
            }
            //如果所有参数都为空
            if (paramCodeList.Count <= 0)
            {
                return (manuProductParameterEntities, procParameterEntities);
            }
            var codesQuery = new ProcParametersByCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                Codes = paramCodeList.ToArray()
            };
            var procParameter = await _procParameterRepository.GetByCodesAsync(codesQuery);
            //如果有不存在的参数编码就提示
            var noIncludeCodes = paramCodeList.Where(w => procParameter.Select(s => s.ParameterCode.ToUpper()).Contains(w.ToUpper()) == false);
            //if (noIncludeCodes.Any() == true)
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

            // 找出不在数据库里面的Code，自动新增标准参数，应对设备不合理需求
            if (noIncludeCodes.Any())
            {
                foreach (var item in noIncludeCodes)
                {
                    if (!procParameterEntities.Where(c => c.ParameterCode == item).Any())
                    {
                        procParameterEntities.Add(new ProcParameterEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentEquipment.SiteId,
                            CreatedBy = _currentEquipment.Code,
                            UpdatedBy = _currentEquipment.Code,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now(),
                            ParameterCode = item,
                            ParameterName = item,
                            Remark = "自动创建"
                        });
                    }
                }
                //将要新增的参数也添加到查询列表中
                procParameter = procParameter.Concat(procParameterEntities);
            }

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                if (outBoundDto.ParamList != null)
                {
                    var stepId = manuSfcStepEntities.Where(c => c.SFC == outBoundDto.SFC).First().Id;
                    var paramList = outBoundDto.ParamList.Select(s =>
                         new ManuProductParameterEntity
                         {
                             Id = IdGenProvider.Instance.CreateId(),
                             SiteId = _currentEquipment.SiteId,
                             StepId = stepId,//记录步骤ID
                             CreatedBy = _currentEquipment.Name,
                             UpdatedBy = _currentEquipment.Name,
                             CreatedOn = HymsonClock.Now(),
                             UpdatedOn = HymsonClock.Now(),
                             EquipmentId = _currentEquipment.Id ?? 0,
                             LocalTime = outBoundMoreDto.LocalTime,
                             SFC = outBoundDto.SFC,
                             ResourceId = procResourceId,
                             ParameterId = procParameter.Where(c => c.ParameterCode.ToUpper().Equals(s.ParamCode.ToUpper())).First().Id,
                             ParamValue = s.ParamValue,
                             Timestamp = s.Timestamp,
                             JudgmentResult = s.JudgmentResult,
                             StandardLowerLimit = s.StandardLowerLimit,
                             StandardUpperLimit = s.StandardUpperLimit,
                             TestDuration = s.TestDuration,
                             TestResult = s.TestResult,
                             TestTime = s.TestTime
                         }
                     );
                    manuProductParameterEntities.AddRange(paramList);
                }
            }
            return (manuProductParameterEntities, procParameterEntities);
        }

        /// <summary>
        /// 组装NG信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        private async Task<List<ManuSfcStepNgEntity>> PrepareProductNgEntityAsync(OutBoundMoreDto outBoundMoreDto, List<ManuSfcStepEntity> manuSfcStepEntities)
        {
            List<ManuSfcStepNgEntity> manuSfcStepNgEntities = new();
            //所有NG编码
            List<string> ngCodeList = new();
            foreach (var item in outBoundMoreDto.SFCs)
            {
                if (item.NG != null)
                {
                    var ngCodes = item.NG.Select(c => c.NGCode.ToUpper());
                    ngCodeList.AddRange(ngCodes);
                }
            }
            //如果所有NG都为空
            if (ngCodeList.Count <= 0)
            {
                return manuSfcStepNgEntities;
            }
            var codesQuery = new QualUnqualifiedCodeByCodesQuery
            {
                Site = _currentEquipment.SiteId,
                Codes = ngCodeList.ToArray()
            };
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByCodesAsync(codesQuery);
            //如果有不存在的参数编码就提示
            var noIncludeCodes = ngCodeList.Where(w => qualUnqualifiedCodes.Select(s => s.UnqualifiedCode.ToUpper()).Contains(w.ToUpper()) == false);
            if (noIncludeCodes.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19114)).WithData("Code", string.Join(',', noIncludeCodes));

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                var stepId = manuSfcStepEntities.Where(c => c.SFC == outBoundDto.SFC).First().Id;
                if (outBoundDto.NG != null)
                {
                    var ngList = outBoundDto.NG.Select(s =>
                     new ManuSfcStepNgEntity
                     {
                         Id = IdGenProvider.Instance.CreateId(),
                         SiteId = _currentEquipment.SiteId,
                         BarCodeStepId = stepId,
                         UnqualifiedCode = s.NGCode.ToUpper(),
                         CreatedBy = _currentEquipment.Code,
                         UpdatedBy = _currentEquipment.Code,
                         CreatedOn = HymsonClock.Now(),
                         UpdatedOn = HymsonClock.Now()
                     });
                    manuSfcStepNgEntities.AddRange(ngList);
                }
            }
            return manuSfcStepNgEntities;
        }

        /// <summary>
        /// 组装Material信息
        /// </summary>
        /// <param name="outBoundMoreDto"></param>
        /// <param name="manuSfcStepEntities"></param>
        /// <returns></returns>
        private List<ManuSfcStepMaterialEntity> PrepareProductMaterialEntity(OutBoundMoreDto outBoundMoreDto, List<ManuSfcStepEntity> manuSfcStepEntities)
        {
            List<ManuSfcStepMaterialEntity> manuSfcStepMaterialEntities = new();

            foreach (var outBoundDto in outBoundMoreDto.SFCs)
            {
                var stepId = manuSfcStepEntities.Where(c => c.SFC == outBoundDto.SFC).First().Id;
                if (outBoundDto.BindFeedingCodes != null && outBoundDto.BindFeedingCodes.Any())
                {
                    var materialList = outBoundDto.BindFeedingCodes.Select(materialCode =>
                     new ManuSfcStepMaterialEntity
                     {
                         Id = IdGenProvider.Instance.CreateId(),
                         StepId = stepId,
                         SiteId = _currentEquipment.SiteId,
                         SFC = outBoundDto.SFC,
                         MaterialBarcode = materialCode,
                         CreatedBy = _currentEquipment.Name,
                         UpdatedBy = _currentEquipment.Name,
                         CreatedOn = HymsonClock.Now(),
                         UpdatedOn = HymsonClock.Now()
                     });
                    manuSfcStepMaterialEntities.AddRange(materialList);
                }
            }
            return manuSfcStepMaterialEntities;
        }
    }
}
