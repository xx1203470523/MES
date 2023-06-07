using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.SfcCirculation
{
    /// <summary>
    /// 流转表条码绑定和解绑服务
    /// </summary>
    public class SfcCirculationService : ISfcCirculationService
    {
        #region Repository
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<SfcCirculationBindDto> _validationSfcCirculationBindDtoRules;
        private readonly AbstractValidator<SfcCirculationUnBindDto> _validationSfcCirculationUnBindDtoRules;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 当前使用工单
        /// </summary>
        private PlanWorkOrderEntity planWorkOrder;
        /// <summary>
        /// 当前资源
        /// </summary>
        private ProcResourceEntity procResource;
        /// <summary>
        /// 工作中心（产线）
        /// </summary>
        private InteWorkCenterEntity inteWorkCenter;

        public SfcCirculationService(
            ICurrentEquipment currentEquipment,
            AbstractValidator<SfcCirculationBindDto> validationSfcCirculationBindDtoRules,
            AbstractValidator<SfcCirculationUnBindDto> validationSfcCirculationUnBindDtoRules,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcRepository manuSfcRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _currentEquipment = currentEquipment;
            _validationSfcCirculationBindDtoRules = validationSfcCirculationBindDtoRules;
            _validationSfcCirculationUnBindDtoRules = validationSfcCirculationUnBindDtoRules;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
        }
        #endregion

        /// <summary>
        /// 流转表绑定
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationBindAsync(SfcCirculationBindDto sfcCirculationBindDto)
        {
            await _validationSfcCirculationBindDtoRules.ValidateAndThrowAsync(sfcCirculationBindDto);
            if (sfcCirculationBindDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (sfcCirculationBindDto.IsVirtualSFC == true && !string.IsNullOrEmpty(sfcCirculationBindDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19133));
            }
            //如果为虚拟条码绑定
            if (sfcCirculationBindDto.IsVirtualSFC == true)
            {
                sfcCirculationBindDto.SFC = ManuProductParameter.DefaultSFC;
            }

            await GetPubInfoByResourceCode(sfcCirculationBindDto.ResourceCode);

            var sfcs = sfcCirculationBindDto.BindSFCs.Select(c => c.SFC).ToArray();
            //条码信息
            var sfclist = await _manuSfcRepository.GetBySFCsAsync(sfcs);
            //查询已经存在条码的生产信息
            var sfcProduceList = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentEquipment.SiteId });
            //如果有不存在的SFC就提示
            var noIncludeSfcs = sfcs.Where(sfc => sfclist.Select(s => s.SFC).Contains(sfc) == false);
            if (noIncludeSfcs.Any() == true)
                throw new CustomerValidationException(nameof(ErrorCode.MES19125)).WithData("SFCS", string.Join(',', noIncludeSfcs));

            //SFC有条码信息，但已经没有生产信息不允许出站
            var noProduceSfcs = sfclist.Where(w => sfcProduceList.Select(s => s.SFC).Contains(w.SFC) == false);
            if (noProduceSfcs.Any())
                throw new CustomerValidationException(nameof(ErrorCode.MES19126)).WithData("SFCS", string.Join(',', noProduceSfcs));

            //排队中的条码没进站不允许绑定
            if (sfcProduceList.Any())
            {
                //必须进站再绑定
                var outBoundMoreSfcs = sfcCirculationBindDto.BindSFCs.Where(w =>
                                            sfcProduceList.Where(c => c.Status != SfcProduceStatusEnum.Activity)
                                            .Select(s => s.SFC)
                                            .Contains(w.SFC));
                if (outBoundMoreSfcs.Any())
                    throw new CustomerValidationException(nameof(ErrorCode.MES19132)).WithData("SFCS", string.Join(',', outBoundMoreSfcs.Select(c => c.SFC)));
            }
            //模组/PACK码信息
            var mpManuSfc = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = sfcCirculationBindDto.SFC, SiteId = _currentEquipment.SiteId });

            //需要写入的实体
            var manuSfc = new ManuSfcEntity();
            var manuSfcInfo = new ManuSfcInfoEntity();
            var manuSfcProduce = new ManuSfcProduceEntity();
            var manuSfcStep = new ManuSfcStepEntity();
            //条码流转信息
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new List<ManuSfcCirculationEntity>();
            List<ManuSfcEntity> manuSfcEntities = new List<ManuSfcEntity>();
            foreach (var circulationBindSFC in sfcCirculationBindDto.BindSFCs)
            {
                var sfcEntity = sfclist.Where(c => c.SFC == circulationBindSFC.SFC).First();
                var sfcProduceEntity = sfcProduceList.Where(c => c.SFC == circulationBindSFC.SFC).First();
                //更新为使用
                sfcEntity.IsUsed = YesOrNoEnum.Yes;
                sfcEntity.UpdatedBy = _currentEquipment.Name;
                sfcEntity.UpdatedOn = HymsonClock.Now();
                manuSfcEntities.Add(sfcEntity);
                //记录流转信息
                manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    SFC = circulationBindSFC.SFC,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = sfcProduceEntity.ProductId,
                    EquipmentId = _currentEquipment.Id,
                    CirculationBarCode = sfcCirculationBindDto.SFC,
                    CirculationProductId = sfcProduceEntity.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = sfcProduceEntity.ProductId,
                    Location = circulationBindSFC.Location,
                    CirculationQty = 1,
                    //使用虚拟码记录为转换
                    CirculationType = sfcCirculationBindDto.IsVirtualSFC == true ? SfcCirculationTypeEnum.Change : SfcCirculationTypeEnum.Merge,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            //绑定条码信息
            if (mpManuSfc == null && sfcCirculationBindDto.IsVirtualSFC != true)
            {
                var sfcProduceEntity = sfcProduceList.First();
                manuSfc = new ManuSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = sfcCirculationBindDto.SFC,
                    IsUsed = YesOrNoEnum.No,
                    Qty = 1,
                    SiteId = _currentEquipment.SiteId,
                    Status = SfcStatusEnum.InProcess,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                };
                manuSfcInfo = new ManuSfcInfoEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SfcId = manuSfc.Id,
                    WorkOrderId = planWorkOrder.Id,
                    ProductId = planWorkOrder.ProductId,
                    IsUsed = false,
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name
                };
                manuSfcProduce = new ManuSfcProduceEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfcCirculationBindDto.SFC,
                    ProductId = planWorkOrder.ProductId,
                    WorkOrderId = planWorkOrder.Id,
                    BarCodeInfoId = manuSfc.Id,
                    ProcessRouteId = planWorkOrder.ProcessRouteId,
                    WorkCenterId = planWorkOrder.WorkCenterId ?? 0,
                    ProductBOMId = planWorkOrder.ProductBOMId,
                    EquipmentId = _currentEquipment.Id ?? 0,
                    ResourceId = procResource.Id,
                    Qty = 1,//进站默认都是1个
                    ProcedureId = sfcProduceList.First().ProcedureId,//当前绑定条码所在工序
                    Status = SfcProduceStatusEnum.Activity,//接口进站直接为活动
                    RepeatedCount = 0,
                    IsScrap = TrueOrFalseEnum.No,
                    CreatedBy = _currentEquipment.Name,
                    UpdatedBy = _currentEquipment.Name
                };
                //步骤记录
                manuSfcStep = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    SFC = sfcCirculationBindDto.SFC,
                    ProductId = planWorkOrder.ProductId,
                    WorkOrderId = planWorkOrder.Id,
                    WorkCenterId = planWorkOrder.WorkCenterId,
                    ProductBOMId = planWorkOrder.ProductBOMId,
                    ProcedureId = sfcProduceEntity.ProcedureId,//当前绑定条码所在工序
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
                //记录流转信息
                manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    SFC = sfcCirculationBindDto.SFC,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = sfcProduceEntity.ProductId,
                    EquipmentId = _currentEquipment.Id,
                    CirculationBarCode = string.Empty,
                    CirculationProductId = sfcProduceEntity.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = sfcProduceEntity.ProductId,
                    CirculationQty = 1,
                    CirculationType = SfcCirculationTypeEnum.Merge,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            //数据提交
            using var trans = TransactionHelper.GetTransactionScope();
            int rows = 0;
            if (mpManuSfc == null && sfcCirculationBindDto.IsVirtualSFC != true)
            {
                await _manuSfcRepository.InsertAsync(manuSfc);
                await _manuSfcInfoRepository.InsertAsync(manuSfcInfo);
                await _manuSfcProduceRepository.InsertAsync(manuSfcProduce);
                await _manuSfcStepRepository.InsertAsync(manuSfcStep);
            }
            // 更新状态
            if (manuSfcEntities.Any())
            {
                rows += await _manuSfcRepository.UpdateRangeAsync(manuSfcEntities);
            }
            // 添加流转记录
            if (manuSfcCirculationEntities.Any())
            {
                rows += await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);
            }
            trans.Complete();
        }

        /// <summary>
        /// 流转表解绑
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationUnBindAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto)
        {
            await _validationSfcCirculationUnBindDtoRules.ValidateAndThrowAsync(sfcCirculationUnBindDto);
            if (sfcCirculationUnBindDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (sfcCirculationUnBindDto.IsVirtualSFC == true && !string.IsNullOrEmpty(sfcCirculationUnBindDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19133));
            }
            //如果为虚拟条码
            if (sfcCirculationUnBindDto.IsVirtualSFC == true)
            {
                sfcCirculationUnBindDto.SFC = ManuProductParameter.DefaultSFC;
            }
            var manuSfcCirculationBarCodequery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = SfcCirculationTypeEnum.Merge,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationBarCodes = new string[] { sfcCirculationUnBindDto.SFC },
                SiteId = _currentEquipment.SiteId
            };
            //如果有传递解绑条码列表,否则解绑该SFC绑定的所有条码记录
            if (sfcCirculationUnBindDto.UnBindSFCs != null && sfcCirculationUnBindDto.UnBindSFCs.Length > 0)
            {
                manuSfcCirculationBarCodequery.Sfcs = sfcCirculationUnBindDto.UnBindSFCs;
            }
            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntities(manuSfcCirculationBarCodequery);
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new();
            if (circulationBarCodeEntities.Any())
            {
                foreach (var entity in circulationBarCodeEntities)
                {
                    entity.IsDisassemble = TrueOrFalseEnum.Yes;
                    entity.DisassembledBy = _currentEquipment.Name;
                    entity.DisassembledOn = HymsonClock.Now();
                    entity.UpdatedBy = _currentEquipment.Name;
                    entity.UpdatedOn = HymsonClock.Now();
                    manuSfcCirculationEntities.Add(entity);
                }
                await _manuSfcCirculationRepository.UpdateRangeAsync(manuSfcCirculationEntities);
            }
        }

        /// <summary>
        /// 组件添加
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SfcCirculationModuleAdd(SfcCirculationBindDto sfcCirculationBindDto)
        {
            await GetPubInfoByResourceCode(sfcCirculationBindDto.ResourceCode);
            if (sfcCirculationBindDto.IsVirtualSFC == true && !string.IsNullOrEmpty(sfcCirculationBindDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19133));
            }
            //如果为虚拟条码
            if (sfcCirculationBindDto.IsVirtualSFC == true)
            {
                sfcCirculationBindDto.SFC = ManuProductParameter.DefaultSFC;
            }
            //获取主条码信息
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = sfcCirculationBindDto.SFC, SiteId = _currentEquipment.SiteId });
            //查询已经存在条码的生产信息
            var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = sfcEntity.SFC, SiteId = _currentEquipment.SiteId })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19125)).WithData("SFCS", sfcEntity.SFC);

            //条码流转信息
            List<ManuSfcCirculationEntity> manuSfcCirculationEntities = new List<ManuSfcCirculationEntity>();
            foreach (var circulationBindSFC in sfcCirculationBindDto.BindSFCs)
            {
                //记录流转信息
                manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentEquipment.SiteId,
                    ProcedureId = sfcProduceEntity.ProcedureId,
                    ResourceId = sfcProduceEntity.ResourceId,
                    SFC = sfcProduceEntity.SFC,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = sfcProduceEntity.ProductId,
                    EquipmentId = _currentEquipment.Id,
                    CirculationBarCode = circulationBindSFC.SFC,
                    CirculationProductId = sfcProduceEntity.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = sfcProduceEntity.ProductId,
                    Location = circulationBindSFC.Location,
                    CirculationQty = 1,
                    CirculationType = SfcCirculationTypeEnum.ModuleAdd,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            await _manuSfcCirculationRepository.InsertRangeAsync(manuSfcCirculationEntities);

        }

        /// <summary>
        /// 组件移除
        /// </summary>
        /// <param name="sfcCirculationUnBindDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SfcCirculationModuleRemove(SfcCirculationUnBindDto sfcCirculationUnBindDto)
        {
            await SfcCirculationUnBindAsync(sfcCirculationUnBindDto);
        }

        /// <summary>
        /// 获取公用信息
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        private async Task GetPubInfoByResourceCode(string resourceCode)
        {
            //已经验证过资源是否存在直接使用
            procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = resourceCode });
            //查询资源和设备是否绑定
            var resourceEquipmentBindQuery = new ProcResourceEquipmentBindQuery
            {
                SiteId = _currentEquipment.SiteId,
                Ids = new long[] { _currentEquipment.Id ?? 0 },
                ResourceId = procResource.Id
            };
            var resEquipentBinds = await _procResourceEquipmentBindRepository.GetByResourceIdAsync(resourceEquipmentBindQuery);
            if (resEquipentBinds.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19131)).WithData("ResCode", procResource.ResCode).WithData("EquCode", _currentEquipment.Code);
            }
            //查找当前工作中心（产线）
            inteWorkCenter = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
            if (inteWorkCenter == null)
            {
                //通过资源未找到关联产线
                throw new CustomerValidationException(nameof(ErrorCode.MES19123)).WithData("ResourceCode", procResource.ResCode);
            }
            //查找激活工单
            var planWorkOrders = await _planWorkOrderRepository.GetByWorkLineIdAsync(inteWorkCenter.Id);
            if (!planWorkOrders.Any())
            {
                //产线未激活工单
                throw new CustomerValidationException(nameof(ErrorCode.MES19124)).WithData("WorkCenterCode", inteWorkCenter.Code);
            }
            //不考虑混线
            planWorkOrder = planWorkOrders.First();
        }
    }
}
