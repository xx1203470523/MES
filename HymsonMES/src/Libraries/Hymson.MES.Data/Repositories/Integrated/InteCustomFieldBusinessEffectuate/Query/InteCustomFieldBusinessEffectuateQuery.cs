using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Data.Repositories.Integrated.Query
{
    /// <summary>
    /// 自定字段业务实现 查询参数
    /// </summary>
    public class InteCustomFieldBusinessEffectuateQuery
    {
        /// <summary>
        /// 业务表id
        /// </summary>
        public long? BusinessId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public InteCustomFieldBusinessTypeEnum? BusinessType { get; set; }

        /// <summary>
        /// 自定毅字段名称
        /// </summary>
        public string? CustomFieldName { get; set; }
    }
}
