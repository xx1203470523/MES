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
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.InBound;
using Hymson.MES.EquipmentServices.Dtos.SfcCirculation;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;

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
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IManuSfcCcsNgRecordRepository _manuSfcCcsNgRecordRepository;
        private readonly IManuSfcSummaryRepository _manuSfcSummaryRepository;
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<SfcCirculationService> _logger;

        public SfcCirculationService(
            ILogger<SfcCirculationService> logger,
            ICurrentEquipment currentEquipment,
            AbstractValidator<SfcCirculationBindDto> validationSfcCirculationBindDtoRules,
            AbstractValidator<SfcCirculationUnBindDto> validationSfcCirculationUnBindDtoRules,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcRepository manuSfcRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCcsNgRecordRepository manuSfcCcsNgRecordRepository,
            IManuSfcSummaryRepository manuSfcSummaryRepository)
        {
            _logger = logger;
            _currentEquipment = currentEquipment;
            _validationSfcCirculationBindDtoRules = validationSfcCirculationBindDtoRules;
            _validationSfcCirculationUnBindDtoRules = validationSfcCirculationUnBindDtoRules;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCcsNgRecordRepository = manuSfcCcsNgRecordRepository;
            _manuSfcSummaryRepository = manuSfcSummaryRepository;
        }
        /// <summary>
        /// CCS绑定的Location
        /// </summary>
        //private readonly string[] locationArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H" };

        #endregion

        /// <summary>
        /// 获取公用信息
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        private async Task<(ProcResourceEntity, InteWorkCenterEntity, PlanWorkOrderEntity)> GetPubInfoByResourceCodeAsync(string resourceCode)
        {
            //已经验证过资源是否存在直接使用
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Site = _currentEquipment.SiteId, Code = resourceCode });
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
            var inteWorkCenter = await _inteWorkCenterRepository.GetByResourceIdAsync(procResource.Id);
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
            var planWorkOrder = planWorkOrders.First();
            return (procResource, inteWorkCenter, planWorkOrder);
        }

        /// <summary>
        /// 验证绑定是否重复并触发异常提醒
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <returns></returns>
        private async Task VerifyDuplicate(SfcCirculationBindDto sfcCirculationBindDto)
        {
            //查找当前已有的绑定记录
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new ManuSfcCirculationBarCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                Sfcs = sfcCirculationBindDto.BindSFCs.Select(c => c.SFC).ToArray(),
                CirculationBarCode = sfcCirculationBindDto.SFC,
                IsDisassemble = TrueOrFalseEnum.No
            });
            if (manuSfcCirculationEntities.Any())
            {
                //条码：{SFCS}已经存在绑定记录
                throw new CustomerValidationException(nameof(ErrorCode.MES19138)).WithData("SFCS", string.Join(",", manuSfcCirculationEntities.Select(c => c.SFC)));
            }
        }

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
            //验证是否已经存在绑定关系
            await VerifyDuplicate(sfcCirculationBindDto);
            //如果为虚拟条码绑定
            if (sfcCirculationBindDto.IsVirtualSFC == true)
            {
                sfcCirculationBindDto.SFC = ManuProductParameter.DefaultSFC;
            }
            //获取公共信息
            var (procResource, inteWorkCenter, planWorkOrder) = await GetPubInfoByResourceCodeAsync(sfcCirculationBindDto.ResourceCode);

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

            //根据资源获取工序
            var procdureEntity = await _procProcedureRepository.GetProcProdureByResourceIdAsync(new() { SiteId = _currentEquipment.SiteId, ResourceId = procResource.Id });

            //排队中的条码也允许绑定
            //if (sfcProduceList.Any())
            //{
            //    //必须进站再绑定
            //    var outBoundMoreSfcs = sfcCirculationBindDto.BindSFCs.Where(w =>
            //                                sfcProduceList.Where(c => c.Status != SfcProduceStatusEnum.Activity)
            //                                .Select(s => s.SFC)
            //                                .Contains(w.SFC));
            //    if (outBoundMoreSfcs.Any())
            //        throw new CustomerValidationException(nameof(ErrorCode.MES19132)).WithData("SFCS", string.Join(',', outBoundMoreSfcs.Select(c => c.SFC)));
            //}
            //模组/PACK码信息
            var mpManuSfc = await _manuSfcRepository.GetBySFCAsync(new GetBySfcQuery { SFC = sfcCirculationBindDto.SFC, SiteId = _currentEquipment.SiteId });


            //查询已有汇总信息
            ManuSfcSummaryQuery manuSfcSummaryQuery = new ManuSfcSummaryQuery
            {
                SiteId = _currentEquipment.SiteId,
                SFCS = sfcs
            };
            var manuSfcSummaryEntities = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(manuSfcSummaryQuery);

            //绑定时检验不合格
            var includeNoQuality = manuSfcSummaryEntities.Where(c => c.QualityStatus == 0);
            if (includeNoQuality.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19146)).WithData("SFCS", string.Join(',', includeNoQuality.Select(c => c.SFC)));
            }

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
                var sfcProduceInfoEntity = sfcProduceList.Where(c => c.SFC == circulationBindSFC.SFC).First();
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
                    ProcedureId = sfcProduceInfoEntity.ProcedureId,
                    ResourceId = sfcProduceInfoEntity.ResourceId,
                    SFC = circulationBindSFC.SFC,
                    Name = circulationBindSFC.Name ?? string.Empty,
                    WorkOrderId = sfcProduceInfoEntity.WorkOrderId,
                    ProductId = sfcProduceInfoEntity.ProductId,
                    EquipmentId = _currentEquipment.Id,
                    CirculationBarCode = sfcCirculationBindDto.SFC,
                    CirculationProductId = sfcProduceInfoEntity.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = sfcProduceInfoEntity.ProductId,
                    Location = circulationBindSFC.Location.ToString(),
                    CirculationQty = 1,
                    ModelCode = sfcCirculationBindDto.ModelCode ?? string.Empty,
                    //使用虚拟码记录为转换
                    CirculationType = sfcCirculationBindDto.IsVirtualSFC == true ? SfcCirculationTypeEnum.Change : SfcCirculationTypeEnum.Merge,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            var sfcProduceEntity = sfcProduceList.First();

            var manuSfcProceProcedureEntity = await _procProcedureRepository.GetByIdAsync(sfcProduceList.First().ProcedureId);

            bool checkResult = true;
            //清安：模组码箱体预处理绑定时，不卡控在制工序必须和绑定工序一致
            if (manuSfcProceProcedureEntity.Code == "OP230" && sfcProduceEntity.Status == SfcProduceStatusEnum.lineUp )
            {
                checkResult = false;
            }

            //20231226 绑定校验在制工序和绑定工序是否一致
            if (sfcProduceEntity.ProcedureId != procdureEntity.Id && checkResult)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16355));
            }

            //记录流转信息
            manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                ProcedureId = sfcProduceEntity.ProcedureId,
                ResourceId = sfcProduceEntity.ResourceId,
                SFC = sfcCirculationBindDto.SFC,
                Name = string.Empty,
                WorkOrderId = sfcProduceEntity.WorkOrderId,
                ProductId = sfcProduceEntity.ProductId,
                EquipmentId = _currentEquipment.Id,
                CirculationBarCode = string.Empty,
                CirculationProductId = sfcProduceEntity.ProductId,//暂时使用原有产品ID
                CirculationMainProductId = sfcProduceEntity.ProductId,
                CirculationQty = 1,
                ModelCode = sfcCirculationBindDto?.ModelCode ?? string.Empty,
                CirculationType = SfcCirculationTypeEnum.Merge,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now()
            });
            //绑定条码信息
            if (mpManuSfc == null && sfcCirculationBindDto.IsVirtualSFC != true)
            {
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
                //manuSfcCirculationEntities.Add(new ManuSfcCirculationEntity
                //{
                //    Id = IdGenProvider.Instance.CreateId(),
                //    SiteId = _currentEquipment.SiteId,
                //    ProcedureId = sfcProduceEntity.ProcedureId,
                //    ResourceId = sfcProduceEntity.ResourceId,
                //    SFC = sfcCirculationBindDto.SFC,
                //    Name = string.Empty,
                //    WorkOrderId = sfcProduceEntity.WorkOrderId,
                //    ProductId = sfcProduceEntity.ProductId,
                //    EquipmentId = _currentEquipment.Id,
                //    CirculationBarCode = string.Empty,
                //    CirculationProductId = sfcProduceEntity.ProductId,//暂时使用原有产品ID
                //    CirculationMainProductId = sfcProduceEntity.ProductId,
                //    CirculationQty = 1,
                //    ModelCode = sfcCirculationBindDto?.ModelCode ?? string.Empty,
                //    CirculationType = SfcCirculationTypeEnum.Merge,
                //    CreatedBy = _currentEquipment.Name,
                //    CreatedOn = HymsonClock.Now(),
                //    UpdatedBy = _currentEquipment.Name,
                //    UpdatedOn = HymsonClock.Now()
                //});

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
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        public async Task SfcCirculationUnBindAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto, SfcCirculationTypeEnum sfcCirculationTypeEnum)
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
                CirculationType = sfcCirculationTypeEnum,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationBarCode = sfcCirculationUnBindDto.SFC,
                SiteId = _currentEquipment.SiteId
            };
            //如果有传递解绑条码列表,否则解绑该SFC绑定的所有条码记录
            if (sfcCirculationUnBindDto.UnBindSFCs != null && sfcCirculationUnBindDto.UnBindSFCs.Length > 0)
            {
                manuSfcCirculationBarCodequery.Sfcs = sfcCirculationUnBindDto.UnBindSFCs;
            }
            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodequery);
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
        /// <param name="sfcCirculationTypeEnum"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SfcCirculationModuleAddAsync(SfcCirculationBindDto sfcCirculationBindDto, SfcCirculationTypeEnum sfcCirculationTypeEnum)
        {
            if (sfcCirculationBindDto.IsVirtualSFC == true && !string.IsNullOrEmpty(sfcCirculationBindDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19133));
            }
            //验证是否已经存在绑定关系
            await VerifyDuplicate(sfcCirculationBindDto);
            //如果为虚拟条码
            if (sfcCirculationBindDto.IsVirtualSFC == true)
            {
                sfcCirculationBindDto.SFC = ManuProductParameter.DefaultSFC;
            }
            //获取主条码信息
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(new GetBySfcQuery { SFC = sfcCirculationBindDto.SFC, SiteId = _currentEquipment.SiteId })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES19125)).WithData("SFCS", sfcCirculationBindDto.SFC); ;
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
                    SFC = circulationBindSFC.SFC,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = sfcProduceEntity.ProductId,
                    EquipmentId = _currentEquipment.Id,
                    CirculationBarCode = sfcProduceEntity.SFC,
                    CirculationProductId = sfcProduceEntity.ProductId,//暂时使用原有产品ID
                    CirculationMainProductId = sfcProduceEntity.ProductId,
                    Location = circulationBindSFC.Location,
                    CirculationQty = 1,
                    CirculationType = sfcCirculationTypeEnum,
                    CreatedBy = _currentEquipment.Name,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = _currentEquipment.Name,
                    UpdatedOn = HymsonClock.Now(),
                    ModelCode = sfcCirculationBindDto.ModelCode
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
        public async Task SfcCirculationModuleRemoveAsync(SfcCirculationUnBindDto sfcCirculationUnBindDto)
        {
            await SfcCirculationUnBindAsync(sfcCirculationUnBindDto, SfcCirculationTypeEnum.ModuleAdd);
        }

        /// <summary>
        /// 根据条码和SfcCirculationTypeEnum 获取模组/Pack绑定条码列表
        /// </summary>
        /// <param name="sfc">模组/Pack条码</param>
        /// <param name="sfcCirculationTypeEnum">默认为Merge</param>
        /// <returns></returns>
        public async Task<IEnumerable<CirculationBindDto>> GetCirculationBindSfcsAsync(string sfc, SfcCirculationTypeEnum? sfcCirculationTypeEnum = SfcCirculationTypeEnum.Merge)
        {
            if (string.IsNullOrEmpty(sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));
            }
            var manuSfcCirculationBarCodequery = new ManuSfcCirculationBarCodeQuery
            {
                CirculationType = sfcCirculationTypeEnum,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationBarCode = sfc,
                SiteId = _currentEquipment.SiteId
            };
            //查询流转条码绑定记录
            var circulationBarCodeEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(manuSfcCirculationBarCodequery);
            List<CirculationBindDto> circulationBinds = new();
            if (circulationBarCodeEntities.Any())
            {
                foreach (var entity in circulationBarCodeEntities)
                {
                    circulationBinds.Add(new CirculationBindDto
                    {
                        SFC = entity.SFC,
                        Location = entity.Location ?? string.Empty,
                        Name = entity.Name,
                        ModelCode = entity.ModelCode,
                    });
                }
            }
            return circulationBinds;
        }

        /// <summary>
        /// 验证CSS绑定是否重复并触发异常提醒
        /// </summary>
        /// <param name="sfcCirculationBindDto"></param>
        /// <returns></returns>
        private async Task VerifyCSSLocationAsync(SfcCirculationBindDto sfcCirculationBindDto)
        {
            //不校验
            //var sfcLocations = sfcCirculationBindDto.BindSFCs.Select(c => c.Location ?? string.Empty).ToArray();
            //var exceptLocations = sfcLocations.Except(locationArray).ToArray();
            //if (exceptLocations.Any())
            //{
            //    //错误的Location  1,2，只能为：A,B,C,D,E,F,G
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19140)).WithData("SFCLocation", string.Join(",", exceptLocations))
            //        .WithData("Location", string.Join(",", locationArray));
            //}
            //查找当前已有的绑定记录
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new ManuSfcCirculationBarCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                CirculationBarCode = sfcCirculationBindDto.SFC,
                IsDisassemble = TrueOrFalseEnum.No
            });
            //
            var locations = sfcCirculationBindDto.BindSFCs.Select(c => c.Location).ToArray();
            var sfcCirculationEntities = manuSfcCirculationEntities.Where(c => locations.Contains(c.Location));
            if (sfcCirculationEntities.Any())
            {
                //条码：{SFCS}已经存在绑定记录
                throw new CustomerValidationException(nameof(ErrorCode.MES19139)).WithData("SFCS", string.Join(",", sfcCirculationEntities.Select(c => c.SFC)))
                    .WithData("Location", string.Join(",", sfcCirculationEntities.Select(c => c.Location)));
            }
        }
        /// <summary>
        /// 绑定CCS
        /// </summary>
        /// <param name="sfcCirculationCCSBindDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationCCSBindAsync(SfcCirculationCCSBindDto sfcCirculationCCSBindDto)
        {
            if (string.IsNullOrEmpty(sfcCirculationCCSBindDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));//SFC条码不能为空
            }
            if (sfcCirculationCCSBindDto.BindSFCs == null || sfcCirculationCCSBindDto.BindSFCs.Length == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19119));//绑定条码列表不能为空
            }
            //if (string.IsNullOrEmpty(sfcCirculationCCSBindDto.ModelCode))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19145));//模组对应型号编码【ModelCode】不能为空
            //}

            var sfcCirculationBindDto = new SfcCirculationBindDto()
            {
                SFC = sfcCirculationCCSBindDto.SFC,
                LocalTime = sfcCirculationCCSBindDto.LocalTime,
                ResourceCode = sfcCirculationCCSBindDto.ResourceCode,
                EquipmentCode = sfcCirculationCCSBindDto.EquipmentCode,
                ModelCode = sfcCirculationCCSBindDto.ModelCode,
                BindSFCs = sfcCirculationCCSBindDto.BindSFCs.Select(c =>
                {
                    return new CirculationBindDto
                    {
                        SFC = c.SFC,
                        Location = c.Location,
                        Name = c.Name
                    };
                }).ToArray()
            };
            //验证Location是否已经存在绑定数据
            await VerifyCSSLocationAsync(sfcCirculationBindDto);
            await SfcCirculationModuleAddAsync(sfcCirculationBindDto, SfcCirculationTypeEnum.BindCCS);
        }

        /// <summary>
        /// 解绑CSS
        /// </summary>
        /// <param name="sfcCirculationCCSUnBindDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationCCSUnBindAsync(SfcCirculationCCSUnBindDto sfcCirculationCCSUnBindDto)
        {
            if (string.IsNullOrEmpty(sfcCirculationCCSUnBindDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));//SFC条码不能为空
            }
            if (sfcCirculationCCSUnBindDto.UnBindSFCs == null || sfcCirculationCCSUnBindDto.UnBindSFCs.Length == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19120));//解绑条码列表不能为空
            }
            var sfcCirculationUnBindDto = new SfcCirculationUnBindDto
            {
                SFC = sfcCirculationCCSUnBindDto.SFC,
                LocalTime = sfcCirculationCCSUnBindDto.LocalTime,
                ResourceCode = sfcCirculationCCSUnBindDto.ResourceCode,
                EquipmentCode = sfcCirculationCCSUnBindDto.EquipmentCode,
                UnBindSFCs = sfcCirculationCCSUnBindDto.UnBindSFCs
            };
            await SfcCirculationUnBindAsync(sfcCirculationUnBindDto, SfcCirculationTypeEnum.BindCCS);
        }

        /// <summary>
        /// 获取绑定CCS的位置
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<CirculationBindCCSLocationDto> GetBindCCSLocationAsync(string sfc)
        {
            CirculationBindCCSLocationDto circulationBindCCSLocation = new() { CurrentLocation = string.Empty, Locations = Array.Empty<string>() };
            var manuSfcCcsNgRecords = await _manuSfcCcsNgRecordRepository.GetManuSfcCcsNgRecordEntitiesAsync(new ManuSfcCcsNgRecordQuery
            {
                SiteId = _currentEquipment.SiteId,
                Status = ManuSfcCcsNgRecordStatusEnum.NG,
                SFC = sfc
            });
            if (manuSfcCcsNgRecords.Any())
            {
                var manuSfcCcsNgRecord = manuSfcCcsNgRecords.OrderBy(c => c.CreatedOn).First();//优先返回第一个
                var locations = manuSfcCcsNgRecords.Select(c => c.Location).ToArray();
                circulationBindCCSLocation.CurrentLocation = manuSfcCcsNgRecord.Location;
                circulationBindCCSLocation.Locations = locations;
                circulationBindCCSLocation.ModelCode = manuSfcCcsNgRecord.ModelCode;
            }
            return circulationBindCCSLocation;
        }

        /// <summary>
        /// 根据模组条码获取绑定CCS信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<CirculationModuleCCSInfoDto> GetCirculationModuleCCSInfoAsync(string sfc)
        {
            if (string.IsNullOrEmpty(sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));//SFC条码不能为空
            }
            CirculationModuleCCSInfoDto circulationModuleCCSInfo = new();
            //查找当前已有的绑定记录
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new ManuSfcCirculationBarCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                CirculationBarCode = sfc,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationType = SfcCirculationTypeEnum.BindCCS
            });

            //查找模组绑定时的录入的ModelCode，使用绑定时的
            var manuSfcCirculations = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new ManuSfcCirculationBarCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                CirculationBarCode = sfc,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationType = SfcCirculationTypeEnum.Merge
            });
            if (manuSfcCirculationEntities.Any())
            {
                var manuSfcCirculation = manuSfcCirculationEntities.First();
                //查询模组的NG记录
                var manuSfcSummaries = await _manuSfcSummaryRepository.GetManuSfcSummaryEntitiesAsync(new ManuSfcSummaryQuery
                {
                    SFCS = new string[] { sfc },
                    QualityStatus = 0,//0 不合格，代表NG状态
                    SiteId = _currentEquipment.SiteId
                });
                circulationModuleCCSInfo.SFC = manuSfcCirculation.SFC;
                circulationModuleCCSInfo.Location = manuSfcCirculation.Location;
                circulationModuleCCSInfo.IsNg = manuSfcSummaries.Any();
            }
            if (manuSfcCirculations.Any())
            {
                circulationModuleCCSInfo.ModelCode = manuSfcCirculations.Where(x => !string.IsNullOrEmpty(x.ModelCode)).OrderByDescending(x => x.UpdatedOn).FirstOrDefault()?.ModelCode ?? string.Empty;
            }
            return circulationModuleCCSInfo;
        }

        /// <summary>
        /// CCS NG设定
        /// </summary>
        /// <param name="sfcCirculationCCSNgSetDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationCCSNgSetAsync(SfcCirculationCCSNgSetDto sfcCirculationCCSNgSetDto)
        {
            if (string.IsNullOrEmpty(sfcCirculationCCSNgSetDto.SFC))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19003));//SFC条码不能为空
            }
            //按Location NG或按绑定CCS编码NG必须任选其一,允许只传入模组码NG
            //if ((sfcCirculationCCSNgSetDto.Locations == null || sfcCirculationCCSNgSetDto.Locations.Length <= 0)
            //    && (sfcCirculationCCSNgSetDto.BindSFCs == null || sfcCirculationCCSNgSetDto.BindSFCs.Length <= 0))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19141));//CCS设定NG时Location和BindSfc方式必须任选其一
            //}
            //查找当前已有的绑定记录
            var manuSfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationBarCodeEntitiesAsync(new ManuSfcCirculationBarCodeQuery
            {
                SiteId = _currentEquipment.SiteId,
                CirculationBarCode = sfcCirculationCCSNgSetDto.SFC,
                IsDisassemble = TrueOrFalseEnum.No,
                CirculationType = SfcCirculationTypeEnum.BindCCS
            });
            if (!manuSfcCirculationEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19142)).WithData("SFC", sfcCirculationCCSNgSetDto.SFC);//条码：{SFC}没找到关联CCS码绑定关系
            }

            var delEntities = manuSfcCirculationEntities;
            //根据传入条件找出已经NG的CSS绑定记录   
            //BindSFCs 和  Locations 允许为空
            if (sfcCirculationCCSNgSetDto.BindSFCs == null && sfcCirculationCCSNgSetDto.Locations == null)
            {
                delEntities = manuSfcCirculationEntities;
            }
            else
            {
                delEntities = manuSfcCirculationEntities.Where(c =>
                {
                    return sfcCirculationCCSNgSetDto.BindSFCs != null && sfcCirculationCCSNgSetDto.BindSFCs.Contains(c.SFC)
                        || sfcCirculationCCSNgSetDto.Locations != null && sfcCirculationCCSNgSetDto.Locations.Contains(c.Location);
                });
                if (!delEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19143));//条码：{SFC}指定位置未关联CCS码或和指定CSS码不存在绑定关系
                }
            }
            //记录CCS的NG记录
            List<ManuSfcCcsNgRecordEntity> manuSfcCcsNgRecords = new();
            //只存在一个
            var manuSfcCirculation = delEntities.First();
            manuSfcCcsNgRecords.Add(new ManuSfcCcsNgRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                SFC = sfcCirculationCCSNgSetDto.SFC,
                Location = manuSfcCirculation.Location ?? string.Empty,
                ModelCode = manuSfcCirculation.ModelCode,
                Status = ManuSfcCcsNgRecordStatusEnum.NG,
                CreatedBy = _currentEquipment.Name,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentEquipment.Name,
                UpdatedOn = HymsonClock.Now()
            });
            using var ts = TransactionHelper.GetTransactionScope();
            //NG设定的CSS直接软删除
            await _manuSfcCirculationRepository.DeleteRangeAsync(new DeleteCommand
            {
                UserId = _currentEquipment.Name,
                DeleteOn = HymsonClock.Now(),
                Ids = delEntities.Select(c => c.Id)
            });
            await _manuSfcCcsNgRecordRepository.InsertsAsync(manuSfcCcsNgRecords);
            ts.Complete();
        }

        /// <summary>
        /// CCS确认
        /// </summary>
        /// <param name="sfcCirculationCCSConfirmDto"></param>
        /// <returns></returns>
        public async Task SfcCirculationCCSConfirmAsync(SfcCirculationCCSConfirmDto sfcCirculationCCSConfirmDto)
        {
            //Location不能为空,SFC可以为空，Location修改为可以为空
            //if (string.IsNullOrEmpty(sfcCirculationCCSConfirmDto.Location))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES19144));
            //}
            var manuSfcCcsNgRecordEntities = await _manuSfcCcsNgRecordRepository.GetManuSfcCcsNgRecordEntitiesAsync(new ManuSfcCcsNgRecordQuery
            {
                SiteId = _currentEquipment.SiteId,
                Status = ManuSfcCcsNgRecordStatusEnum.NG,
                SFC = sfcCirculationCCSConfirmDto.SFC,
                Location = sfcCirculationCCSConfirmDto.Location
            });
            if (manuSfcCcsNgRecordEntities.Any())
            {
                var manuSfcCcsNgRecord = manuSfcCcsNgRecordEntities.First();//每次取一个
                manuSfcCcsNgRecord.Status = ManuSfcCcsNgRecordStatusEnum.Confirm;
                manuSfcCcsNgRecord.UpdatedBy = _currentEquipment.Name;
                manuSfcCcsNgRecord.UpdatedOn = HymsonClock.Now();
                await _manuSfcCcsNgRecordRepository.UpdateAsync(manuSfcCcsNgRecord);
            }
        }
    }
}
