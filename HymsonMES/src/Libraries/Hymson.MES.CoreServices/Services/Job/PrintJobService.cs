using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Process;

namespace Hymson.MES.CoreServices.Services.Job
{
    [Job("条码打印", JobTypeEnum.Standard)]
    public class PrintJobService : IJobService
    {
        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance1> _eventBus;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcResourceConfigPrintRepository _resourceConfigPrintRepository;
        private readonly IProcProcedurePrintRelationRepository _procProcedurePrintRelationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventBus"></param>
        public PrintJobService(IMasterDataService masterDataService,
          IEventBus<EventBusInstance1> eventBus,
          IProcResourceRepository procResourceRepository,
          IProcProcedureRepository procProcedureRepository,
          IProcMaterialRepository procMaterialRepository,
          IProcResourceConfigPrintRepository resourceConfigPrintRepository,
          IProcProcedurePrintRelationRepository procProcedurePrintRelationRepository)
        {
            _eventBus = eventBus;
            _masterDataService = masterDataService;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procMaterialRepository = procMaterialRepository;
            _resourceConfigPrintRepository = resourceConfigPrintRepository;
            _procProcedurePrintRelationRepository = procProcedurePrintRelationRepository;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }
            var barCodes = new List<LabelTemplateBarCodeDto>();

            foreach (var item in sfcProduceEntities)
            {
                barCodes.Add(new LabelTemplateBarCodeDto
                {
                    BarCode = item.SFC,
                    MateriaId = item.ProductId
                });
            }

            #region 打印配置校验

            var resourceEntity = await _procResourceRepository.GetResByIdAsync(commonBo.ResourceId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16337));
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16352));

            // 对工序资源类型和资源的资源类型校验
            if (procedureEntity.ResourceTypeId.HasValue && resourceEntity.ResTypeId != procedureEntity.ResourceTypeId.Value)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16507));
            }

            // 校验资源是否已关联打印机
            var procResourceConfigPrintEnties = await _resourceConfigPrintRepository.GetByResourceIdAsync(commonBo.ResourceId);
            if (procResourceConfigPrintEnties == null || !procResourceConfigPrintEnties.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10390)).WithData("ResourceCode", resourceEntity.ResCode);
            }

            // 校验工序是否配置当前物料的打印模板
            var procProcedurePrintReleationEnties = await _procProcedurePrintRelationRepository.GetProcProcedurePrintReleationEntitiesAsync(new ProcProcedurePrintReleationQuery
            {
                SiteId = commonBo.SiteId ,
                ProcedureId = commonBo.ProcedureId,
                MaterialId = sfcProduceEntities.First().ProductId
            });
            if (procProcedurePrintReleationEnties == null || !procProcedurePrintReleationEnties.Where(x => x.TemplateId != 0).Any())
            {
                var materialEntity = await _procMaterialRepository.GetByIdAsync(sfcProduceEntities.First().ProductId);
                throw new CustomerValidationException(nameof(ErrorCode.MES10391)).WithData("ProcedureCode", $"{procedureEntity.Code}({procedureEntity.Name})").WithData("MaterialCode", materialEntity?.MaterialCode ?? "");
            }

            #endregion

            return new PrintIntegrationEvent
            {
                CurrencyTemplateType = CurrencyTemplateTypeEnum.Production,
                SiteId = commonBo.SiteId,
                ResourceId = commonBo.ResourceId,
                ProcedureId = commonBo.ProcedureId,
                BarCodes = barCodes,
                UserName = commonBo.UserName
            };
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            if (obj is not PrintIntegrationEvent data) return null;
            _eventBus.PublishDelay(data, 1);
            return null;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
