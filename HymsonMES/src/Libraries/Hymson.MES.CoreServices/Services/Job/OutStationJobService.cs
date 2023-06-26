using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 出站
    /// </summary>
    [Job("出站", JobTypeEnum.Standard)]
    public class OutStationJobService : IJobService<OutStationJobBo>
    {
        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(OutStationJobBo param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task DataAssemblingAsync(OutStationJobBo param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <returns></returns>
        public async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }

    }
}
