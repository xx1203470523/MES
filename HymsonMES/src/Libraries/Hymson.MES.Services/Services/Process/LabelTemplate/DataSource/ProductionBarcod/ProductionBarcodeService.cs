using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using Hymson.Print.Abstractions;
using Hymson.Utils;
using System.ComponentModel;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.DataSource.ProductionBarcod
{
    /// <summary>
    /// 生产条码
    /// </summary>
    [Description(nameof(ProductionBarcodeDto))]
    public class ProductionBarcodeService: IProductionBarcodeService
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
        public ProductionBarcodeService(IManuSfcRepository manuSfcRepository,
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
        public async Task<IEnumerable<PrintStructDto<ProductionBarcodeDto>>> GetLabelTemplateDataAsync(LabelTemplateSourceDto param)
        {
            long? printId = null;
            //一般情况下是单台打印机
            if (param.PrintId.HasValue)
            {
                printId = param.PrintId.Value;
            }
            else
            {
                var procResourceConfigPrintEnties = await _resourceConfigPrintRepository.GetByResourceIdAsync(param.ResourceId);
                if (procResourceConfigPrintEnties == null || !procResourceConfigPrintEnties.Any())
                {
                    var procResourceEntity= await _procResourceRepository.GetByIdAsync(param.ResourceId);
                    throw new CustomerValidationException(nameof(ErrorCode.MES10390)).WithData("ResourceCode", procResourceEntity.ResCode);
                }
                printId = procResourceConfigPrintEnties.FirstOrDefault()?.PrintId;
            }
            var printConfigEntity = await _printConfigRepository.GetByIdAsync(printId ?? 0);
            var procProcedurePrintReleationEnties = await _procedurePrintRelationRepository.GetProcProcedurePrintReleationEntitiesAsync(new ProcProcedurePrintReleationQuery
            {
                SiteId = param.SiteId,
                ProcedureId = param.ProcedureId,
            });
            var procLabelTemplateEnties = await _procLabelTemplateRepository.GetByIdsAsync(procProcedurePrintReleationEnties.Select(x => x.TemplateId).Distinct());

            var manuSfcEntitiesTask = _manuSfcRepository.GetListAsync(new ManuSfcQuery { SiteId = param.SiteId, SFCs = param.BarCodes.Select(x => x.BarCode), Type = SfcTypeEnum.Produce });
            var manuSfcProduceEntitiesTask = _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery { SiteId = param.SiteId, Sfcs = param.BarCodes.Select(x => x.BarCode) });
            var manuSfcEntities = await manuSfcEntitiesTask;
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;

            var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(manuSfcEntities.Select(x => x.Id));

            var ManuSfcStepEntityList = await _manuSfcStepRepository.GetProductParameterBySFCEntitiesAsync(new EntityBySFCsQuery
            {
                SiteId = param.SiteId,
                SFCs = param.BarCodes.Select(x => x.BarCode)
            });
            var planWorkOrderEntitiesTask = _planWorkOrderRepository.GetByIdsAsync(manuSfcInfoEntities.Select(x => x.WorkOrderId ?? 0));
            var procMaterialEntitiesTask = _procMaterialRepository.GetByIdsAsync(manuSfcInfoEntities.Select(x => x.ProductId));


            var planWorkOrderEntities = await planWorkOrderEntitiesTask;
            var procMaterialEntities = await procMaterialEntitiesTask;


            var printDataSurceList = new List<PrintStructDto<ProductionBarcodeDto>>();
            foreach (var item in manuSfcEntities)
            {
                var manuSfcInfoEntity = manuSfcInfoEntities.FirstOrDefault(x => x.SfcId == item.Id);
                var procMaterialEntity = procMaterialEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.ProductId);
                var planWorkOrderEntity = planWorkOrderEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.WorkOrderId);
                var manuSfcProduceEntity = manuSfcProduceEntities.FirstOrDefault(x => x.SFC == item.SFC);

                var procProcedurePrintReleationByMaterialIdEnties = procProcedurePrintReleationEnties.Where(x => x.MaterialId == manuSfcInfoEntity?.ProductId);
                foreach (var procProcedurePrintReleationByMaterialIditem in procProcedurePrintReleationByMaterialIdEnties)
                {
                    var rocLabelTemplateEntity = procLabelTemplateEnties.FirstOrDefault(x => x.Id == procProcedurePrintReleationByMaterialIditem.TemplateId);
                    var createManuSfcStepEntity = ManuSfcStepEntityList.FirstOrDefault(x => x.Operatetype == ManuSfcStepTypeEnum.Create || x.Operatetype == ManuSfcStepTypeEnum.Receive);
                    var outputManuSfcStepEntity = ManuSfcStepEntityList.FirstOrDefault(x => x.Operatetype == ManuSfcStepTypeEnum.OutStock);
                    var productionBarcodeEntity = new PrintStructDto<ProductionBarcodeDto>
                    {
                        TemplateName = rocLabelTemplateEntity?.Name,
                        PrintName = printConfigEntity.PrintName,
                        PrintCount = Convert.ToInt16(procProcedurePrintReleationByMaterialIditem.Copy),
                        PrintData = new ProductionBarcodeDto
                        {
                            SFC = item.SFC,
                            WorkOrderCode = planWorkOrderEntity?.OrderCode ?? "",
                            ProductCode = procMaterialEntity?.MaterialCode ?? "",
                            ProductName = procMaterialEntity?.MaterialName ?? "",
                            Qty = (item.ScrapQty ?? 0) + item.Qty,
                            QualifiedQty = item.Qty,
                            UnQualifiedQty = item.ScrapQty ?? 0,
                            Status = "",
                            PrintTime = HymsonClock.Now().ToString("yyyy-MM-dd HH:mm:ss"),
                        }
                    };
                    if (createManuSfcStepEntity != null)
                    {
                        productionBarcodeEntity.PrintData.ProductionBatch = createManuSfcStepEntity.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (outputManuSfcStepEntity != null)
                    {
                        productionBarcodeEntity.PrintData.LasttOutputTime = outputManuSfcStepEntity.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
                        var procProcedureEntitiesTask = _procProcedureRepository.GetByIdAsync(outputManuSfcStepEntity.ProcedureId ?? 0);
                        var equEquipmentEntitiesTask = _equEquipmentRepository.GetByIdAsync(outputManuSfcStepEntity.EquipmentId ?? 0);
                        var procResourceEntitiesTask = _procResourceRepository.GetByIdAsync(outputManuSfcStepEntity.ResourceId ?? 0);
                        var procProcedureEntity = await procProcedureEntitiesTask;
                        var equEquipmentEntity = await equEquipmentEntitiesTask;
                        var procResourceEntity = await procResourceEntitiesTask;
                        productionBarcodeEntity.PrintData.ProcedureCode = procProcedureEntity?.Code;
                        productionBarcodeEntity.PrintData.ProcedureName = procProcedureEntity?.Name;
                        productionBarcodeEntity.PrintData.ResourceCode = procResourceEntity?.ResCode;
                        productionBarcodeEntity.PrintData.ResourceName = procResourceEntity?.ResName;
                        productionBarcodeEntity.PrintData.EquipmentCode = equEquipmentEntity?.EquipmentCode;
                        productionBarcodeEntity.PrintData.EquipmentName = equEquipmentEntity?.EquipmentName;
                    }

                    printDataSurceList.Add(productionBarcodeEntity);
                }
            }

            return printDataSurceList;
        }
    }
}
