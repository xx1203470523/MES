using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.InteJob.Query
{
    /// <summary>
    /// 作业表分页参数
    /// @author admin
    /// @date 2023-02-21
    /// </summary>
    public class InteJobPagedQuery : PagerInfo
    {
        /// <summary>
        /// 所属站点代码 
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 作业编号 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 作业名称 
        /// </summary>
        public string Name { get; set; }
    }
}