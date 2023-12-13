using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class TracingSourceSFCService : ITracingSourceSFCService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（条码流转）
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcCirculationRepository"></param>
        public TracingSourceSFCService(IWaterMarkService waterMarkService,
            IManuSfcCirculationRepository manuSfcCirculationRepository)
        {
            _waterMarkService = waterMarkService;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 1000)
        {
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.TracingSourceSFC);

            // 获取流转表数据
            var manuSfcCirculationList = await _manuSfcCirculationRepository.GetListByStartWaterMarkIdAsync(new EntityByWaterMarkQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcCirculationList == null || !manuSfcCirculationList.Any()) return;

            var user = $"{BusinessKey.TracingSourceSFC}作业";

            // 相同条码的数据只记录一条记录
            var manuSfcStepDic = manuSfcCirculationList.ToLookup(x => new SingleSFCBo
            {
                SiteId = x.SiteId,
                SFC = x.SFC
            }).ToDictionary(d => d.Key, d => d);

            using var trans = TransactionHelper.GetTransactionScope();

            // TODO 业务待实现

            // 根据条码递归读取所有父级条码

            // 根据条码递归读取所有子级条码

            // 生成链路JSON数据

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.TracingSourceSFC, manuSfcCirculationList.Max(x => x.Id));
            trans.Complete();

        }

    }
}
