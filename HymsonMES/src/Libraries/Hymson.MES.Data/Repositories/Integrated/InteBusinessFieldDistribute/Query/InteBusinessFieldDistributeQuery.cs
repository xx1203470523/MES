using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 字段分配管理 查询参数
    /// </summary>
    public class InteBusinessFieldDistributeQuery
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工厂id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 类型 1、装配，2、不合格，3、包装
        /// </summary>
        public FieldAssignmentTypeEnum? Type { get; set; }
    }
}
