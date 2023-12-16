using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 自定义字段新增/更新Dto
    /// </summary>
    public record InteCustomFieldSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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

    /// <summary>
    /// 自定义字段Dto
    /// </summary>
    public record InteCustomFieldDto : BaseEntityDto
    {
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

    /// <summary>
    /// 字段国际化Dto
    /// </summary>
    public record InteCustomFieldInternationalizationDto : BaseEntityDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

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
