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
        /// <summary>
        /// 仓储接口（生产配置）
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;
        public BarcodeChangeJobService(IManuCommonService manuCommonService
            ,IMasterDataService manuService
            , IPlanWorkOrderRepository planWorkOrderRepository
            , ILocalizationService localizationService
            , IWhMaterialInventoryRepository whMaterialInventoryRepository
            , IProcMaterialRepository procSfcMaterialRepository
            , IManuSfcRepository manuSfcRepository
            , IManuSfcStepRepository manuSfcStepRepository
            , IManuSfcInfoRepository manuSfcInfoRepository
            , IManuSfcCirculationRepository manuCirculationRepository
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
            var bcb = commonBo.BarcodeChangeBos;
            if (bcb == null||!bcb.Items.Any())
            {
                return default;
            }
            BarcodeChangeResponse responseBo = new();
            var manusfcs = new List<ManuSfcEntity>();
            var sfcinfos = new List<ManuSfcInfoEntity>() ;
            var sfcproduces = new List<ManuSfcProduceEntity>();
            var manuSfcStepEntities =  new List<ManuSfcStepEntity>();
            var PhysicalDeleteSFCProduceByIdsCommands = new List<PhysicalDeleteSFCProduceByIdsCommand>();
            var MultiSFCUpdateStatusCommands = new List<MultiSFCUpdateStatusCommand>();
            var manuSfcCirculationEntitys = new List<ManuSfcCirculationEntity>();
            //
            responseBo.manusfcs = manusfcs;
            responseBo.sfcinfos = sfcinfos;
            responseBo.sfcproduces = sfcproduces;
            responseBo.manuSfcStepEntities = manuSfcStepEntities;
            responseBo.PhysicalDeleteSFCProduceByIdsCommands = PhysicalDeleteSFCProduceByIdsCommands;
            responseBo.MultiSFCUpdateStatusCommands = MultiSFCUpdateStatusCommands;
            responseBo.manuSfcCirculationEntitys = manuSfcCirculationEntitys;
            foreach (var bo in bcb.Items)
            {
                BomMaterial? material = null;
                decimal qty = 0;

                var mr = await _procMaterialRepository.GetByIdAsync(bcb.WO.ProductId);
                qty = mr.Batch;
                var now = HymsonClock.Now();
                //如果在制已经生成 跳过该条码的数据组装
                var sprtarget =  await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                {
                    Sfc = bo.TargetSFC,
                    SiteId = commonBo.SiteId
                });
                if (sprtarget != null)
                    continue;
                 
                //生成在制记录
                
                (ManuSfcEntity manusfc, ManuSfcInfoEntity sfcinfo, ManuSfcProduceEntity sfcproduce, ManuSfcStepEntity? sfcstep) cellsfc = new();
                cellsfc = CreateSFCProduceInfoFromCellSFC(bcb.WO, bo.TargetSFC, commonBo.ProcedureId, commonBo, qty,bo.Status);
                manusfcs.Add(cellsfc.manusfc);
                sfcinfos.Add(cellsfc.sfcinfo);
                sfcproduces.Add(cellsfc.sfcproduce);
                
                if (!string.IsNullOrEmpty(bo.SourceSFC))
                {
                    var spr = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                    {
                        Sfc = bo.SourceSFC,
                        SiteId = commonBo.SiteId
                    }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES16600));
                    //继承旧在制条码的复投次数
                    cellsfc.sfcproduce.RepeatedCount = spr.RepeatedCount;
                    //旧条码 状态变更为 转换
                    var msse = new ManuSfcStepEntity
                    {
                        Operatetype = bo.SourceStepType,
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = bo.SourceSFC,
                        ProductId = spr.ProductId,
                        WorkOrderId = spr.WorkOrderId,
                        WorkCenterId = spr.WorkCenterId,
                        ProductBOMId = spr.ProductBOMId,
                        ProcedureId = spr.ProcedureId,
                        Qty = spr.Qty,

                        EquipmentId = spr.EquipmentId,
                        ResourceId = spr.ResourceId,
                        SiteId = commonBo.SiteId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = now,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = now
                    };
                    manuSfcStepEntities.Add(msse);
                    // 删除 manu_sfc_produce

                    PhysicalDeleteSFCProduceByIdsCommands.Add( new PhysicalDeleteSFCProduceByIdsCommand
                    {
                        SiteId = commonBo.SiteId,
                        Ids = new long[] { spr.Id }
                    });

                    // manu_sfc_info 修改为完成 且入库
                    MultiSFCUpdateStatusCommands.Add(new MultiSFCUpdateStatusCommand
                    {
                        SiteId = commonBo.SiteId,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = now,
                        Status = SfcStatusEnum.Complete,
                        SFCs = new string[] { bo.SourceSFC } //manuSfcEntities
                    });
                    manuSfcCirculationEntitys.Add( new ManuSfcCirculationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        ProcedureId = commonBo.ProcedureId,
                        ResourceId = commonBo.ResourceId,
                        SFC = bo.SourceSFC,
                        WorkOrderId = bcb.WO.Id,
                        ProductId = 0,//克明说留空
                        CirculationBarCode = bo.TargetSFC,
                        CirculationProductId = bcb.WO.ProductId,
                        CirculationMainProductId = bcb.WO.ProductId,
                        CirculationQty = qty, //TODO: 新工单 产品的标包大小
                        CirculationType = bo.CirculationType,
                        CreatedBy = commonBo.UserName,
                        UpdatedBy = commonBo.UserName
                    });
                }
                else
                {
                    responseBo.WorkCode = bcb.WO.OrderCode;
                    responseBo.WorkOrderId = bcb.WO.Id;
                    responseBo.PlanQuantity = bcb.WO.Qty * (1 + bcb.WO.OverScale / 100);
                    responseBo.PassDownQuantity = responseBo.sfcproduces.Sum(x => x.Qty);
                    responseBo.UserName = commonBo.UserName;
                }
                //新条码 状态变更为开始
                var msse1 = new ManuSfcStepEntity
                {
                    Operatetype = ManuSfcStepTypeEnum.Create,
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = bo.TargetSFC,
                    ProductId = bcb.WO.ProductId,
                    WorkOrderId = bcb.WO.Id,
                    WorkCenterId = bcb.WO.WorkCenterId,
                    ProductBOMId = bcb.WO.ProductBOMId,
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
                manuSfcStepEntities.Add(msse1);
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
        private ( ManuSfcEntity manusfc, ManuSfcInfoEntity sfcinfo, ManuSfcProduceEntity sfcproduce, ManuSfcStepEntity sfcstep) CreateSFCProduceInfoFromCellSFC(PlanWorkOrderEntity planWorkOrderEntity, string sfc, long procedureId, JobRequestBo bo,decimal qty,SfcStatusEnum sfcStatus)
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


            var msie = new ManuSfcInfoEntity
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

            var mspe = new ManuSfcProduceEntity
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

            var msse = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                SFC = sfc,
                ProductId = planWorkOrderEntity.ProductId,
                WorkOrderId = planWorkOrderEntity.Id,
                ProductBOMId = planWorkOrderEntity.ProductBOMId,
                WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                Qty = qty,
                ProcedureId = procedureId,
                Operatetype = ManuSfcStepTypeEnum.Create,
                CurrentStatus = SfcStatusEnum.lineUp,
                CreatedBy = bo.UserName,
                UpdatedBy = bo.UserName
            };
            return (manuSfcEntity, msie, mspe, msse);
        }


        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not BarcodeChangeResponse data)
            {
                return responseBo;
            }
            //条码接收的情况
            if(data.PassDownQuantity>0)
            {
                var row = await _planWorkOrderRepository.UpdatePassDownQuantityByWorkOrderId(new UpdatePassDownQuantityCommand
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
            if(data.manuSfcCirculationEntitys!=null)
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
            var foo = param as JobRequestBo;
            if (foo == null)
            {
                return;
            }
            var bcb = foo.BarcodeChangeBos;
            if (bcb == null||!bcb.Items.Any())
            {
                return;
            }
            var psr1 = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new Data.Repositories.Process.ProductSet.Query.GetByProcedureIdAndProductIdQuery()
            {
                ProductId = bcb.WO.ProductId,
                SetPointId = foo.ResourceId,
                SiteId = param.SiteId,
            });
            if (psr1 == null)
            {
                psr1 = await _procProductSetRepository.GetByProcedureIdAndProductIdAsync(new Data.Repositories.Process.ProductSet.Query.GetByProcedureIdAndProductIdQuery()
                {
                    ProductId = bcb.WO.ProductId,
                    SetPointId = foo.ProcedureId,
                    SiteId = param.SiteId,
                });
                //if (psr1 == null)
                //    throw new CustomerValidationException(nameof(ErrorCode.MES19605));
            }
            foreach (var bo in bcb.Items)
            {
                if(psr1 == null)
                {
                    //验证掩码规则
                    var isCodeRule = await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(bo.TargetSFC, bcb.WO.ProductId);
                    if (!isCodeRule)
                    {
                        
                        throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", bo.TargetSFC);
                    }
                }
                else
                {
                    var isCodeRule = await _manuCommonService.CheckBarCodeByMaskCodeRuleAsync(bo.TargetSFC, psr1.SemiProductId);
                    if (!isCodeRule)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19916)).WithData("SFC", bo.TargetSFC);
                    }
                }
               
             
                //SourceSFC 为空 说明走的条码接收逻辑，没有在制信息
                if (!string.IsNullOrEmpty(bo.SourceSFC))
                {
                    var spr = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
                    {
                        Sfc = bo.TargetSFC,
                        SiteId = foo.SiteId
                    });
                    if (spr != null)
                        throw new CustomerValidationException(nameof(ErrorCode.MES14035)).WithData("sfc", bo.TargetSFC);

                }

            }

        }
    }
}
