using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 进站
    /// </summary>
    [Job("进站", JobTypeEnum.Standard)]
    public class InStationJobService : IJobService<InStationRequestBo, InStationResponseBo>
    {
        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(InStationRequestBo param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<InStationResponseBo> DataAssemblingAsync(InStationRequestBo param)
        {
            await Task.CompletedTask;
            return new InStationResponseBo { };
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
