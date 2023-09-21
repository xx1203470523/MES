using Hymson.Localization.Services;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 作业公共类
    /// </summary>
    public class JobBaseBo : MultiSFCBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IJobContextProxy? Proxy { get; set; }

        public ILocalizationService LocalizationService { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JobResultBo { }
}
