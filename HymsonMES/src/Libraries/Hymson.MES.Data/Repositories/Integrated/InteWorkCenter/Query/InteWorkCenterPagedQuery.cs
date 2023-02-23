using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query
{
    /// <summary>
    /// 工作中心表分页参数
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public class InteWorkCenterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点Id 
        /// </summary>
        public long? SiteId { get; set; }
        /// <summary>
        /// 工作中心代码 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 工作中心名称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型(工厂/车间/产线) 
        /// </summary>
        public short? Type { get; set; }

        /// <summary>
        /// 数据来源 
        /// </summary>
        public short? Source { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public short? Status { get; set; }
    }
}