using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcResourceConfigJobView: ProcResourceConfigJobEntity
    {
        /// <summary>
        /// 描述 :作业编号 
        /// 空值 : false  
        /// </summary>
        public string JobCode { get; set; }

        /// <summary>
        /// 描述 :作业名称 
        /// 空值 : false  
        /// </summary>
        public string JobName { get; set; }
    }
}
