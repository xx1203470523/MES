using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 组装
    /// </summary>
    [Job("组装", JobTypeEnum.Standard)]
    public class PackageVerifyJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

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
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="localizationService"></param>
        public PackageVerifyJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            ILocalizationService localizationService)
        {
            _manuCommonService = manuCommonService;
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
            var bo = param.ToBo<PackageRequestBo>();
            if (bo == null) return;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcProduceStatusEnum).FullName}.{nameof(SfcProduceStatusEnum.Activity)}"))
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);
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
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            // 面板需要的数据
            return await Task.FromResult(new JobResponseBo
            {
                Content = new Dictionary<string, string> {
                { "PackageCom", "True" },
                { "BadEntryCom", "False" },
            },
                Message = ""
            });
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
