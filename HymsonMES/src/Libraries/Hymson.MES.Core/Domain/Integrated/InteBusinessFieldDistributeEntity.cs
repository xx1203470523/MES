using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（字段分配管理）   
    /// inte_business_field_distribute
    /// @author User
    /// @date 2024-06-13 03:31:20
    /// </summary>
    public class InteBusinessFieldDistributeEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 类型 1、装配，2、不合格，3、包装
        /// </summary>
        public FieldAssignmentTypeEnum Type { get; set; }

       /// <summary>
        /// 分配类型编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

       
    }
}
