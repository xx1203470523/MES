using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（作业配置）   
    /// inte_job_config
    /// @author zhaoqing
    /// @date 2024-07-02 04:28:15
    /// </summary>
    public class InteJobConfigEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设定值
        /// </summary>
        public string SetValue { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 作业Id inte_jobid
        /// </summary>
        public long JobId { get; set; }

        
    }
}
