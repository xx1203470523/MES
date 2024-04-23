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
    /// 条码合并JOB,特殊作业，出站的条码合并成一个隐式条码
    /// </summary>
    [Job("条码合并JOB", JobTypeEnum.Standard)]
    public class BarcodeMergeJobService : IJobService
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
        public BarcodeMergeJobService(IManuCommonService manuCommonService
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
            var barcodeChangeBo = commonBo.OutStationRequestBos;
            if (barcodeChangeBo == null||!barcodeChangeBo.Any())
            {
                return default;
            }
            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }
            
            BarcodeMergeResponse responseBo = new();
            var manusfcs = new List<ManuSfcEntity>();
            var sfcinfos = new List<ManuSfcInfoEntity>() ;
            var sfcproduces = new List<ManuSfcProduceEntity>();
            var manuSfcStepEntities =  new List<ManuSfcStepEntity>();
            
            var manuSfcCirculationEntitys = new List<ManuSfcCirculationEntity>();
            //
            responseBo.manusfcs = manusfcs;
            responseBo.sfcinfos = sfcinfos;
            responseBo.sfcproduces = sfcproduces;
            responseBo.manuSfcStepEntities = manuSfcStepEntities;
          
            responseBo.manuSfcCirculationEntitys = manuSfcCirculationEntitys;
            var key = IdGenProvider.Instance.CreateId().ToString();
            bool IsCreated = false;
            foreach (var bo in barcodeChangeBo)
            {
                var now = HymsonClock.Now();
                
                var sfcProduceEntity  = sfcProduceEntities.FirstOrDefault(s => s.SFC == bo.SFC);
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(sfcProduceEntity.WorkOrderId);
                var sfcCirculationEntities = await _manuSfcCirculationRepository.GetManuSfcCirculationEntitiesAsync(new ManuSfcCirculationQuery
                {
                    Sfc = sfcProduceEntity.SFC,
                    SiteId = commonBo.SiteId,
                    CirculationTypes = new List<SfcCirculationTypeEnum>() { SfcCirculationTypeEnum.Merge }.ToArray(),
                    ProcedureId = commonBo.ProcedureId,
                });
                //如果流转记录已经生成 跳过该条码的数据组装
                if (sfcCirculationEntities!=null&&sfcCirculationEntities.Any()) {
                    continue;
                }
                if(!IsCreated) 
                {
                    (ManuSfcEntity manusfc, ManuSfcInfoEntity sfcinfo,ManuSfcProduceEntity ManuSfcProduceEntity) cellsfc = new();
                    cellsfc = CreateSFCProduceInfoFromCellSFC(workOrderEntity, key, commonBo.ProcedureId, commonBo, sfcProduceEntity.Qty, SfcStatusEnum.Activity);
                    manusfcs.Add(cellsfc.manusfc);
                    sfcinfos.Add(cellsfc.sfcinfo);
                    sfcproduces.Add(cellsfc.ManuSfcProduceEntity);
                    ////新条码 状态变更为开始  , 不写步骤表 by keming
                    //var manuSfcStepEntity = new ManuSfcStepEntity
                    //{
                    //    Operatetype = ManuSfcStepTypeEnum.BarcodeBinding, //多个条码归一为一个条码，还可以拆分，故使用绑定枚举 by keming
                    //    Id = IdGenProvider.Instance.CreateId(),
                    //    SFC = key,
                    //    ProductId = sfcProduceEntity.ProductId,
                    //    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    //    WorkCenterId = sfcProduceEntity.WorkCenterId,
                    //    ProductBOMId = sfcProduceEntity.ProductBOMId,
                    //    ProcedureId = commonBo.ProcedureId,
                    //    Qty = sfcProduceEntity.Qty, 

                    //    EquipmentId = commonBo.EquipmentId,
                    //    ResourceId = commonBo.ResourceId,
                    //    SiteId = commonBo.SiteId,
                    //    CreatedBy = commonBo.UserName,
                    //    CreatedOn = HymsonClock.Now(),
                    //    UpdatedBy = commonBo.UserName,
                    //    UpdatedOn = HymsonClock.Now()
                    //};
                    //manuSfcStepEntities.Add(manuSfcStepEntity);
                    IsCreated = true;
                }
                
                manuSfcCirculationEntitys.Add(new ManuSfcCirculationEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    ProcedureId = commonBo.ProcedureId,
                    ResourceId = commonBo.ResourceId,
                    SFC = sfcProduceEntity.SFC,
                    WorkOrderId = sfcProduceEntity.WorkOrderId,
                    ProductId = 0,//克明说留空
                    CirculationBarCode = key,
                    CirculationProductId = sfcProduceEntity.ProductId,
                    CirculationMainProductId = sfcProduceEntity.ProductId,
                    CirculationQty = sfcProduceEntity.Qty,
                    CirculationType = SfcCirculationTypeEnum.Merge,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });
               
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
        private ( ManuSfcEntity manusfc, ManuSfcInfoEntity sfcinfo,ManuSfcProduceEntity sfcproduce) CreateSFCProduceInfoFromCellSFC(PlanWorkOrderEntity planWorkOrderEntity, string sfc, long procedureId, JobRequestBo bo,decimal qty,SfcStatusEnum sfcStatus)
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

            //var manuSfcStepEntity = new ManuSfcStepEntity
            //{
            //    Id = IdGenProvider.Instance.CreateId(),
            //    SiteId = bo.SiteId,
            //    SFC = sfc,
            //    ProductId = planWorkOrderEntity.ProductId,
            //    WorkOrderId = planWorkOrderEntity.Id,
            //    ProductBOMId = planWorkOrderEntity.ProductBOMId,
            //    WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
            //    Qty = qty,
            //    ProcedureId = procedureId,
            //    Operatetype = ManuSfcStepTypeEnum.,
            //    CurrentStatus = SfcStatusEnum.Activity,
            //    CreatedBy = bo.UserName,
            //    UpdatedBy = bo.UserName
            //};
            return (manuSfcEntity, manuSfcInfoEntity, manuSfcProduceEntity);
        }


        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not BarcodeMergeResponse data)
            {
                return responseBo;
            }
            

            //生成在制相关信息
            
            responseBo.Rows += await _manuSfcRepository.InsertRangeAsync(data.manusfcs);

            responseBo.Rows += await _manuSfcInfoRepository.InsertsAsync(data.sfcinfos);
            responseBo.Rows += await _manuSfcProduceRepository.InsertRangeAsync(data.sfcproduces);
            //生成流转记录
            if(data.manuSfcCirculationEntitys!=null)
                responseBo.Rows += await _manuSfcCirculationRepository.InsertRangeAsync(data.manuSfcCirculationEntitys);
           // responseBo.Rows += await _manuSfcStepRepository.InsertRangeAsync(data.manuSfcStepEntities);

            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {

        }
    }
}
