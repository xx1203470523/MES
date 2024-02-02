using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 参数收集
    /// </summary>
    [Job("参数收集", JobTypeEnum.Standard)]
    public class ParameterCollectJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="localizationService"></param>
        public ParameterCollectJobService(IMasterDataService masterDataService, ILocalizationService localizationService)
        {
            _masterDataService = masterDataService;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.PanelRequestBos == null || !commonBo.PanelRequestBos.Any()) return;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.PanelRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any()) return;

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcStatusEnum.Activity, _localizationService)
                              .VerifyProcedure(commonBo.ProcedureId)
                              .VerifyResource(commonBo.ResourceId);
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult(new EmptyRequestBo { });
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            // 面板需要的数据
            List<PanelModuleEnum> panelModules = new() { PanelModuleEnum.ParameterCollect };
            return await Task.FromResult(new JobResponseBo
            {
                Content = new Dictionary<string, string> { { "PanelModules", panelModules.ToSerialize() } },
                Message = "" //_localizationService.GetResource(nameof(ErrorCode.MES16363))
            });
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

    }
}
