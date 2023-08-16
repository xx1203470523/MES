using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query
{
    /// <summary>
    /// 工作中心表分页参数
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterByTypeQuery
    {
        /// <summary>
        /// 站点Id 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public WorkCenterTypeEnum? Type { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public long? ParentId { get; set; }

    }
}