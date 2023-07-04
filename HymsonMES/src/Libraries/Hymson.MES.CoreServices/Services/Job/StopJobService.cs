using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 中止
    /// </summary>
    [Job("中止", JobTypeEnum.Standard)]
    public class StopJobService : IJobService
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
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procProcessRouteDetailNodeRepository"></param>
        /// <param name="procProcessRouteDetailLinkRepository"></param>
        /// <param name="localizationService"></param>
        public StopJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
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
        public async Task<int> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return 0;
        }

    }
}
