using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（自定字段业务实现）   
    /// inte_custom_field_business_effectuate
    /// @author Karl
    /// @date 2023-12-18 05:12:06
    /// </summary>
    public class InteCustomFieldBusinessEffectuateEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 业务表id
        /// </summary>
        public long BusinessId { get; set; }

       /// <summary>
        /// 业务类型
        /// </summary>
        public InteCustomFieldBusinessTypeEnum BusinessType { get; set; }

       /// <summary>
        /// 自定毅字段名称
        /// </summary>
        public string CustomFieldName { get; set; }

       /// <summary>
        /// 设定值
        /// </summary>
        public string? SetValue { get; set; }
    }
}
