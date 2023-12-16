using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（自定义字段）   
    /// inte_custom_field
    /// @author Karl
    /// @date 2023-12-15 04:30:52
    /// </summary>
    public class InteCustomFieldEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 业务类型
        /// </summary>
        public bool BusinessType { get; set; }

       /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

       
    }
}
