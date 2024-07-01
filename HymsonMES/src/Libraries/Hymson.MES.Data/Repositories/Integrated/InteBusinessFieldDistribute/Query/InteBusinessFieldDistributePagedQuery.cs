using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段分配管理 分页参数
    /// </summary>
    public class InteBusinessFieldDistributePagedQuery : PagerInfo
    {

        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 开始时间  数组 ：时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 类型 1、装配，2、不合格，3、包装
        /// </summary>
        public FieldAssignmentTypeEnum? Type { get; set; }
    }
}
