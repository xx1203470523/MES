using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 工序条码请求
    /// </summary>
    [Job("工序条码请求", JobTypeEnum.Standard)]
    public class SFCRequestJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="localizationService"></param>
        public SFCRequestJobService(
            IMasterDataService masterDataService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            ILocalizationService localizationService)
        {
            _masterDataService = masterDataService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {

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
            return new { };
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            var responseBo = new JobResponseBo();

            List<PanelModuleEnum> panelModules = new()
            {
                PanelModuleEnum.SFCRequest
            };
            responseBo.Content = new Dictionary<string, string> { { "PanelModules", panelModules.ToSerialize() } };

            return await Task.FromResult(responseBo);
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
