using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.BatchBarcode;
using Hymson.MES.CoreServices.Services.Process.PrintTemplate.DataSource.ProductionBarcod;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Print.Abstractions;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.Process.Print
{

    /// <summary>
    /// 打印
    /// </summary>
    public class ExecPrintService : IExecPrintService
    {
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IProductionBarcodeService _productionBarcodeService;
        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IBatchBarcodeService _batchBarcodeService;

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
        /// 打印
        /// </summary>
        private readonly IPrintService _printService;

        /// <summary>
        /// 标签关联表
        /// </summary>
        private readonly IProcLabelTemplateRelationRepository _procLabelTemplateRelationRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productionBarcodeService"></param>
        /// <param name="batchBarcodeService"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="localizationService"></param>
        /// <param name="resourceConfigPrintRepository"></param>
        /// <param name="procedurePrintRelationRepository"></param>
        /// <param name="procLabelTemplateRepository"></param>
        /// <param name="printConfigRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        public ExecPrintService(IProductionBarcodeService productionBarcodeService,
            IBatchBarcodeService batchBarcodeService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository,
            IEquEquipmentRepository equEquipmentRepository,
            ILocalizationService localizationService,
            IProcResourceConfigPrintRepository resourceConfigPrintRepository,
            IProcProcedurePrintRelationRepository procedurePrintRelationRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            IProcPrintConfigRepository printConfigRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcLabelTemplateRelationRepository procLabelTemplateRelationRepository,
            IPrintService printService)
        {
            _productionBarcodeService = productionBarcodeService;
            _batchBarcodeService = batchBarcodeService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _localizationService = localizationService;
            _resourceConfigPrintRepository = resourceConfigPrintRepository;
            _procedurePrintRelationRepository = procedurePrintRelationRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _printConfigRepository = printConfigRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _printService = printService;
            _procLabelTemplateRelationRepository = procLabelTemplateRelationRepository;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task PrintAsync(PrintIntegrationEvent @event)
        {
            long? printId = null;
            //一般情况下是单台打印机
            if (@event.PrintId.HasValue )
            {
                printId = @event.PrintId.Value;
            }
            else
            {
                var procResourceConfigPrintEnties = await _resourceConfigPrintRepository.GetByResourceIdAsync(@event.ResourceId);
                if (procResourceConfigPrintEnties == null || !procResourceConfigPrintEnties.Any())
                {
                    var procResourceEntity = await _procResourceRepository.GetByIdAsync(@event.ResourceId);
                    throw new CustomerValidationException(nameof(ErrorCode.MES10390)).WithData("ResourceCode", procResourceEntity.ResCode);
                }
                printId = procResourceConfigPrintEnties.FirstOrDefault()?.PrintId;
            }
            var printConfigEntiy = await _printConfigRepository.GetByIdAsync(printId ?? 0);
            var procProcedurePrintReleationEnties = await _procedurePrintRelationRepository.GetProcProcedurePrintReleationEntitiesAsync(new ProcProcedurePrintReleationQuery
            {
                SiteId = @event.SiteId,
                ProcedureId = @event.ProcedureId
            });
            var procLabelTemplateEnties = await _procLabelTemplateRepository.GetByIdsAsync(procProcedurePrintReleationEnties.Select(x => x.TemplateId).Distinct());

            var labelTemplateSourceDto = new LabelTemplateSourceDto()
            {
                SiteId = @event.SiteId,
                ProcedureId = @event.ProcedureId,
                ResourceId = @event.ResourceId,
                BarCodes = @event.BarCodes,
                UserName = @event.UserName
            };
            var groups = @event.BarCodes.GroupBy(x => x.MateriaId);

            var procLabelTemplateTypeEnties = await _procLabelTemplateRepository.GetByTemplateTypeAsync(new ProcLabelTemplateByTemplateTypeQuery() { SiteId = @event.SiteId, CurrencyTemplateType = @event.CurrencyTemplateType ?? 0 });
            var batchBarcodeList = new List<PrintStructDto<BatchBarcodeDto>>();
            var productionBarcodeList = new List<PrintStructDto<ProductionBarcodeDto>>();
            foreach (var groupItem in groups)
            {
                var procProcedurePrintReleationByMaterialIdEnties = procProcedurePrintReleationEnties.Where(x => x.MaterialId == groupItem.Key);
                if (procProcedurePrintReleationByMaterialIdEnties == null || !procProcedurePrintReleationByMaterialIdEnties.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10390));
                }
                foreach (var procProcedurePrintReleation in procProcedurePrintReleationByMaterialIdEnties)
                {
                    long labelTemplateId = 0;
                    var rocLabelTemplateEntity = procLabelTemplateEnties.FirstOrDefault(x => x.Id == procProcedurePrintReleation.TemplateId);
                    // rocLabelTemplateEntity为空 就用通用得
                    if (rocLabelTemplateEntity == null)
                    {
                        labelTemplateId = procLabelTemplateTypeEnties.Id;
                    }
                    else
                    {
                        labelTemplateId = rocLabelTemplateEntity.Id;
                    }
                    var procLabelTemplateRelationEntity = await _procLabelTemplateRelationRepository.GetByLabelTemplateIdAsync(labelTemplateId);
                    switch (procLabelTemplateRelationEntity?.PrintDataModel)
                    {
                        case "Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource.BatchBarcodeDto":
                            var batchBarcodeDataList = await _batchBarcodeService.GetLabelTemplateDataAsync(labelTemplateSourceDto);
                            foreach (var batchBarcodeItem in batchBarcodeDataList)
                            {
                                batchBarcodeList.Add(new PrintStructDto<BatchBarcodeDto>
                                {
                                    TemplateName = labelTemplateId.ToString(),
                                    PrintCount = Convert.ToInt16(procProcedurePrintReleation.Copy ?? 0),
                                    PrintName = printConfigEntiy.PrintName,
                                    PrintData = batchBarcodeItem
                                });
                            }
                            break;
                        case "Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.DataSource.ProductionBarcodeDto":
                            var productionBarcodeDataList = await _productionBarcodeService.GetLabelTemplateDataAsync(labelTemplateSourceDto);
                            foreach (var productionBarcodeItem in productionBarcodeDataList)
                            {
                                productionBarcodeList.Add(new PrintStructDto<ProductionBarcodeDto>
                                {
                                    TemplateName = labelTemplateId.ToString(),
                                    PrintCount = Convert.ToInt16(procProcedurePrintReleation.Copy ?? 0),
                                    PrintName = printConfigEntiy.PrintName,
                                    PrintData = productionBarcodeItem
                                });
                            }
                            break;
                        default:
                            throw new CustomerValidationException(nameof(ErrorCode.MES10390));
                    }

                }
            }
            using var trans = TransactionHelper.GetTransactionScope();
            if (batchBarcodeList != null && batchBarcodeList.Any())
            {
                await _printService.AddTaskAsync<BatchBarcodeDto>(batchBarcodeList);
            }
            if (productionBarcodeList != null && productionBarcodeList.Any())
            {
                await _printService.AddTaskAsync<ProductionBarcodeDto>(productionBarcodeList);
            }
            trans.Complete();
        }
    }
};
