using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 生产作业程序，数据实体对象   
    /// inte_job_class
    /// @author Czhipu
    /// @date 2023-06-06 08:59:32
    /// </summary>
    public class InteJobClassEntity : BaseEntity
    {
        /// <summary>
        /// 类名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }


    }
}
