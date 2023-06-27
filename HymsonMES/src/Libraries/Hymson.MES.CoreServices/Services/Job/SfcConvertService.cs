using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Job;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 条码转换
    /// </summary>
    [Job("条码转换", JobTypeEnum.Standard)]
    public class SfcConvertService : IJobService<SfcConvertRequestBo, SfcConvertResponseBo>
    {
        /// <summary>
        /// 参数校验
        /// </summary> 
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync(SfcConvertRequestBo param)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SfcConvertResponseBo> DataAssemblingAsync(SfcConvertRequestBo param)
        {
            await Task.CompletedTask;
            return new SfcConvertResponseBo { };
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
