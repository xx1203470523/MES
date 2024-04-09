using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Print.Abstractions;
using System.ComponentModel;

namespace Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.BatchBarcode
{
    /// <summary>
    /// 原材料条码
    /// </summary>
    [Description(nameof(BatchBarcodeDto))]
    public class BatchBarcodeService : IBatchBarcodeService
    {
        /// <summary>
        /// 条码仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 资源关联打印机仓储
        /// </summary>
        private readonly IProcResourceConfigPrintRepository _resourceConfigPrintRepository;

        /// <summary>
        /// 工序配置打印表仓储
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procedurePrintRelationRepository;

        /// <summary>
        /// 标签表仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;

        /// <summary>
        /// 打印配置仓储
        /// </summary>
        private readonly IProcPrintConfigRepository _printConfigRepository;

        /// <summary>
        ///步骤表仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 获取生产执行类
        /// </summary>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="procResourceConfigPrintRepository"></param>
        /// <param name="procProcedurePrintRelationRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="localizationService"></param>
        public BatchBarcodeService(IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IProcResourceConfigPrintRepository procResourceConfigPrintRepository,
                IProcProcedurePrintRelationRepository procProcedurePrintRelationRepository,
                IManuSfcStepRepository manuSfcStepRepository,
            ILocalizationService localizationService
            )
        {
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _resourceConfigPrintRepository = procResourceConfigPrintRepository;
            _procedurePrintRelationRepository = procProcedurePrintRelationRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BatchBarcodeDto>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param)
        {
            var manuSfcEntitiesTask = _manuSfcRepository.GetListAsync(new ManuSfcQuery { SiteId = param.SiteId, SFCs = param.BarCodes.Select(x => x.BarCode), Type = SfcTypeEnum.NoProduce });
            var manuSfcEntities = await manuSfcEntitiesTask;

            var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(manuSfcEntities.Select(x => x.Id));

            var ManuSfcStepEntityList = await _manuSfcStepRepository.GetProductParameterBySFCEntitiesAsync(new EntityBySFCsQuery
            {
                SiteId = param.SiteId,
                SFCs = param.BarCodes.Select(x => x.BarCode)
            });
            var procMaterialEntitiesTask = _procMaterialRepository.GetByIdsAsync(manuSfcInfoEntities.Select(x => x.ProductId));
            var procMaterialEntities = await procMaterialEntitiesTask;

            var list = new List<BatchBarcodeDto>();
            foreach (var item in manuSfcEntities)
            {
                var manuSfcInfoEntity = manuSfcInfoEntities.FirstOrDefault(x => x.SfcId == item.Id);
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.ProductId);
                var createManuSfcStepEntity = ManuSfcStepEntityList.FirstOrDefault(x => x.Operatetype == ManuSfcStepTypeEnum.Create || x.Operatetype == ManuSfcStepTypeEnum.Receive);
                var outputManuSfcStepEntity = ManuSfcStepEntityList.FirstOrDefault(x => x.Operatetype == ManuSfcStepTypeEnum.OutStock);
                var printData = new BatchBarcodeDto
                {
                    SFC = item.SFC,
                    MaterialCode = procMaterialEntity?.MaterialCode ?? "",
                    MaterialName = procMaterialEntity?.MaterialName ?? "",
                    Qty = item.Qty
                };

                BarcodeParsing(printData);
                list.Add(printData);
            }
            return list;
        }

        /// <summary>
        /// 条码解析
        /// </summary>
        /// <param name="param"></param>
        private void BarcodeParsing(BatchBarcodeDto param)
        {
            var strArray = param.SFC.Split(',');
            if (strArray.Length > 2) {
                param.ProductionBatch = strArray[1];
            }
            if (strArray.Length > 3)
            {
                param.ProductionShift = strArray[2];
            }
            if (strArray.Length > 5)
            {
                param.SerialNumber = strArray[4];
            }
        }
    }
}
