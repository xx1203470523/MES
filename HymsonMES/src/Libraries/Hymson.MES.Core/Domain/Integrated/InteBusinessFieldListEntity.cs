using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（字段定义列表数据）   
    /// inte_business_field_list
    /// @author User
    /// @date 2024-06-13 03:23:14
    /// </summary>
    public class InteBusinessFieldListEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 是否默认
        /// </summary>
        public bool iSDefault { get; set; }

       /// <summary>
        /// 字段idinte_business_field 的id
        /// </summary>
        public long BusinessFieldId { get; set; }

       /// <summary>
        /// 标签
        /// </summary>
        public string FieldLabel { get; set; }

       /// <summary>
        /// 值
        /// </summary>
        public string FieldValue { get; set; }

       
    }
}
