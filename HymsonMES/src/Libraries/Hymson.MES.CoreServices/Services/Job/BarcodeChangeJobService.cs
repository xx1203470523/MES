using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;

using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymson.Utils;
using Org.BouncyCastle.Asn1.Cmp;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using System.Runtime.Intrinsics.X86;

using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.Data.Repositories.Warehouse;
using FluentValidation.Results;
using FluentValidation;
using Hymson.Localization.Services;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.EventBus.Abstractions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Core.Constants.Process;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 条码转换JOB,同时处理切叠工序，叠芯码的接收工作
    /// </summary>
    [Job("条码转换JOB", JobTypeEnum.Standard)]
    public class BarcodeChangeJobService : IJobService
    {
        private readonly IMasterDataService _masterDataService;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;
        private readonly IManuCommonService _manuCommonService;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        /// <summary>
        /// 仓储接口（生产配置）
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;
        public BarcodeChangeJobService(IManuCommonService manuCommonService
            , IMasterDataService manuService
            , IPlanWorkOrderRepository planWorkOrderRepository
            , ILocalizationService localizationService
            , IWhMaterialInventoryRepository whMaterialInventoryRepository
            , IProcMaterialRepository procSfcMaterialRepository
            , IManuSfcRepository manuSfcRepository
            , IManuSfcStepRepository manuSfcStepRepository
            , IManuSfcInfoRepository manuSfcInfoRepository
            , IManuSfcCirculationRepository manuCirculationRepository
            , IProcResourceRepository procResourceRepository
            , IPlanWorkOrderBindRepository planWorkOrderBindRepository
            , IManuSfcProduceRepository manuSfcProduceRepository,
            IProcProductSetRepository procProductSetRepository)
        {
            _localizationService = localizationService;
            _masterDataService = manuService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _manuSfcCirculationRepository = manuCirculationRepository;
            _manuCommonService = manuCommonService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcRepository = manuSfcRepository;
            _procMaterialRepository = procSfcMaterialRepository;
            _procProductSetRepository = procProductSetRepository;
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _procResourceRepository = procResourceRepository;
        }
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (!commonBo.InStationRequestBos.Any() || commonBo.InStationRequestBos == null) return default;

            var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            {
                SiteId = commonBo.SiteId,
                ResourceId = commonBo.ResourceId
            });

        

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(planWorkOrderBindEntity.WorkOrderId);

            // 获取产出设置的产品ID
            var productIdOfSet = await _masterDataService.GetProductSetIdAsync(new ProductSetBo
            {
                SiteId = commonBo.SiteId,
                ProductId = planWorkOrderEntity.ProductId,
                ProcedureId = commonBo.ProcedureId,
                ResourceId = commonBo.ResourceId
            });
            // 产品ID
            var productId = productIdOfSet ?? planWorkOrderEntity.ProductId;

            BarcodeChangeResponse responseBo = new();
            var manusfcs = new List<ManuSfcEntity>();
            var sfcinfos = new List<ManuSfcInfoEntity>();
            var sfcproduces = new List<ManuSfcProduceEntity>();
            var manuSfcStepEntities = new List<ManuSfcStepEntity>();
            var PhysicalDeleteSFCProduceByIdsCommands = new List<PhysicalDeleteSFCProduceByIdsCommand>();
            var MultiSFCUpdateStatusCommands = new List<MultiSFCUpdateStatusCommand>();
            var manuSfcCirculationEntitys = new List<ManuSfcCirculationEntity>();
            //
            responseBo.IsProductSame = productId == planWorkOrderEntity.ProductId;
            responseBo.manusfcs = manusfcs;
            responseBo.sfcinfos = sfcinfos;
            responseBo.sfcproduces = sfcproduces;
            responseBo.manuSfcStepEntities = manuSfcStepEntities;
            responseBo.PhysicalDeleteSFCProduceByIdsCommands = PhysicalDeleteSFCProduceByIdsCommands;
            responseBo.MultiSFCUpdateStatusCommands = MultiSFCUpdateStatusCommands;
            responseBo.manuSfcCirculationEntitys = manuSfcCirculationEntitys;
            foreach (var bo in commonBo.InStationRequestBos)
            {
                BomMaterial? material = null;
                decimal qty = 0;

                var materialEntity = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
                qty = string.IsNullOrEmpty(materialEntity.Batch) ? 0 : decimal.Parse(materialEntity.Batch);
                var now = HymsonClock.Now();
                //如果在制已经生成 跳过该条码的数据组装
                var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                {
                    Sfc = bo.SFC,
                    SiteId = commonBo.SiteId
                });
                if (sfcProduceEntity != null)
                    continue;

                //生成在制记录

                (ManuSfcEntity manusfc, ManuSfcInfoEntity sfcinfo, ManuSfcProduceEntity sfcproduce, ManuSfcStepEntity? sfcstep) cellsfc = new();
                cellsfc = CreateSFCProduceInfoFromCellSFC(planWorkOrderEntity, bo.SFC, commonBo.ProcedureId, commonBo, qty, SfcStatusEnum.lineUp);
                cellsfc.sfcinfo.ProductId = productId;
                cellsfc.sfcproduce.ProductId = productId;   
                manusfcs.Add(cellsfc.manusfc);
                sfcinfos.Add(cellsfc.sfcinfo);
                sfcproduces.Add(cellsfc.sfcproduce);
                responseBo.WorkCode = planWorkOrderEntity.OrderCode;
                responseBo.WorkOrderId = planWorkOrderEntity.Id;
                responseBo.PlanQuantity = planWorkOrderEntity.Qty * (1 + planWorkOrderEntity.OverScale / 100);
                responseBo.PassDownQuantity = responseBo.sfcproduces.Sum(x => x.Qty);
                responseBo.UserName = commonBo.UserName;
                
              
                //新条码 状态变更为开始
                var manuSfcStepEntity = new ManuSfcStepEntity
                {
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = bo.SFC,
                    ProductId = productId,
                    WorkOrderId = planWorkOrderEntity.Id,
                    WorkCenterId = planWorkOrderEntity.WorkCenterId,
                    ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    ProcedureId = commonBo.ProcedureId,
                    Qty = qty, //TODO:

                    EquipmentId = commonBo.EquipmentId,
                    ResourceId = commonBo.ResourceId,
                    SiteId = commonBo.SiteId,
                    CreatedBy = commonBo.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedBy = commonBo.UserName,
                    UpdatedOn = HymsonClock.Now()
                };
                manuSfcStepEntities.Add(manuSfcStepEntity);
            }

            return responseBo;

        }

        /// <summary>
        /// 通过外部电芯码生成在制记录
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private (ManuSfcEntity manusfc, ManuSfcInfoEntity sfcinfo, ManuSfcProduceEntity sfcproduce, ManuSfcStepEntity sfcstep) CreateSFCProduceInfoFromCellSFC(PlanWorkOrderEntity planWorkOrderEntity, string sfc, long procedureId, JobRequestBo bo, decimal qty, SfcStatusEnum sfcStatus)
        {

            var manuSfcEntity = new ManuSfcEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SFC = sfc,
                Qty = qty,
                IsUsed = YesOrNoEnum.No,
                Status = SfcStatusEnum.lineUp,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };


            var manuSfcInfoEntity = new ManuSfcInfoEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SfcId = manuSfcEntity.Id,
                WorkOrderId = planWorkOrderEntity.Id,
                ProductId = planWorkOrderEntity.ProductId,
                IsUsed = true,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName,
            };

            var manuSfcProduceEntity = new ManuSfcProduceEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SFC = sfc,
                SFCId = manuSfcEntity.Id,
                ProductId = planWorkOrderEntity.ProductId,
                WorkOrderId = planWorkOrderEntity.Id,
                BarCodeInfoId = manuSfcEntity.Id,
                ProcessRouteId = planWorkOrderEntity.ProcessRouteId,
                WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                ProductBOMId = planWorkOrderEntity.ProductBOMId,
                EquipmentId = bo.EquipmentId,
                Qty = qty,
                ProcedureId = procedureId,
                Status = sfcStatus,
                RepeatedCount = 0,
                IsScrap = TrueOrFalseEnum.No,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };

            var manuSfcStepEntity = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SFC = sfc,
                ProductId = planWorkOrderEntity.ProductId,
                WorkOrderId = planWorkOrderEntity.Id,
                ProductBOMId = planWorkOrderEntity.ProductBOMId,
                WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                ProcessRouteId= planWorkOrderEntity.ProcessRouteId,
                Qty = qty,
                ProcedureId = procedureId,
                Operatetype = ManuSfcStepTypeEnum.Create,
                CurrentStatus = SfcStatusEnum.lineUp,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };
            return (manuSfcEntity, manuSfcInfoEntity, manuSfcProduceEntity, manuSfcStepEntity);
        }


        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not BarcodeChangeResponse data)
            {
                return responseBo;
            }
            //条码接收的情况
            if (data.IsProductSame)
            {
                var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderIdAsync(new UpdatePassDownQuantityCommand
                {
                    WorkOrderId = data.WorkOrderId,
                    PlanQuantity = data.PlanQuantity,
                    PassDownQuantity = data.PassDownQuantity,
                    UserName = data.UserName,
                    UpdateDate = HymsonClock.Now()
                });
                if (row == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16503)).WithData("workorder", data.WorkCode);
                }
            }
            

            //生成在制相关信息

            responseBo.Rows += await _manuSfcRepository.InsertRangeAsync(data.manusfcs);

            responseBo.Rows += await _manuSfcInfoRepository.InsertsAsync(data.sfcinfos);
            responseBo.Rows += await _manuSfcProduceRepository.InsertRangeAsync(data.sfcproduces);
            //生成流转记录
            if (data.manuSfcCirculationEntitys != null)
                responseBo.Rows += await _manuSfcCirculationRepository.InsertRangeAsync(data.manuSfcCirculationEntitys);
            responseBo.Rows += await _manuSfcStepRepository.InsertRangeAsync(data.manuSfcStepEntities);

            // 删除 manu_sfc_produce
            foreach (var PhysicalDeleteSFCProduceByIdsCommand in data.PhysicalDeleteSFCProduceByIdsCommands)
            {
                responseBo.Rows += await _manuSfcProduceRepository.DeletePhysicalRangeByIdsAsync(PhysicalDeleteSFCProduceByIdsCommand);
            }
            foreach (var MultiSFCUpdateStatusCommand in data.MultiSFCUpdateStatusCommands)
            {
                // manu_sfc_info 修改为完成 且入库
                responseBo.Rows += await _manuSfcRepository.MultiUpdateSfcStatusAsync(MultiSFCUpdateStatusCommand);
            }

            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;

            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16370));
            }
            var planWorkOrderBindEntity = await _planWorkOrderBindRepository.GetByResourceIDAsync(new PlanWorkOrderBindByResourceIdQuery
            {
                SiteId = commonBo.SiteId,
                ResourceId = commonBo.ResourceId
            });
            if (planWorkOrderBindEntity == null)
            {
                var resourceEntity = await _procResourceRepository.GetResByIdAsync(commonBo.ResourceId);
                throw new CustomerValidationException(nameof(ErrorCode.MES19147)).WithData("ResourceCode", resourceEntity.ResCode);
            }
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(planWorkOrderBindEntity.WorkOrderId);
            var productSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new Data.Repositories.Process.ProductSet.Query.GetByProcedureIdAndProductIdQuery()
            {
                ProductId = planWorkOrderEntity.ProductId,
                SetPointId = commonBo.ResourceId,
                SiteId = param.SiteId,
            });
            if (productSetEntity == null)
            {
                productSetEntity = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new Data.Repositories.Process.ProductSet.Query.GetByProcedureIdAndProductIdQuery()
                {
                    ProductId = planWorkOrderEntity.ProductId,
                    SetPointId = commonBo.ProcedureId,
                    SiteId = commonBo.SiteId,
                });
                //if (psr1 == null)
                //    throw new CustomerValidationException(nameof(ErrorCode.MES19605));
            }
            foreach (var bo in commonBo.InStationRequestBos)
            {
                if (productSetEntity == null)
                {
                    //验证掩码规则
                    var isCodeRule = await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(bo.SFC, planWorkOrderEntity.ProductId);
                    if (!isCodeRule)
                    {

                        throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", bo.SFC);
                    }
                }
                else
                {
                    var isCodeRule = await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(bo.SFC, productSetEntity.SemiProductId);
                    if (!isCodeRule)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", bo.SFC);
                    }
                }


                ////SourceSFC 为空 说明走的条码接收逻辑，没有在制信息
                //if (!string.IsNullOrEmpty(bo.SourceSFC))
                //{
                //    var sfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                //    {
                //        Sfc = bo.TargetSFC,
                //        SiteId = foo.SiteId
                //    });
                //    if (sfcProduceEntity != null)
                //        throw new CustomerValidationException(nameof(ErrorCode.MES14035)).WithData("sfc", bo.TargetSFC);

                //}

            }

        }
    }
}
