using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（字段国际化）   
    /// inte_custom_field_internationalization
    /// @author Karl
    /// @date 2023-12-15 05:24:48
    /// </summary>
    public class InteCustomFieldInternationalizationEntity : BaseEntity
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 自定义字段id
        /// </summary>
        public long CustomFieldId { get; set; }

       /// <summary>
        /// 语言类型
        /// </summary>
        public bool LanguageType { get; set; }

       /// <summary>
        /// 翻译值
        /// </summary>
        public string TranslationValue { get; set; }

       
    }
}
