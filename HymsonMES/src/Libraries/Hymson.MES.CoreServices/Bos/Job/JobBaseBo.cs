using Hymson.MES.CoreServices.Services.Job.JobUtility.Context;
using System.Reflection.Metadata.Ecma335;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 作业公共类
    /// </summary>
    public class JobBaseBo
    {
        /// <summary>
        /// 
        /// </summary>
        public IJobContextProxy Proxy { get; set; }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码集合
        /// </summary>
        public IEnumerable<string> SFCs { get; set; } = new List<string>();
    }
}
