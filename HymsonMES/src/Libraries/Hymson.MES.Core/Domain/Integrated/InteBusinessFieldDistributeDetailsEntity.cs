using Hymson.Infrastructure;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（字段分配管理详情）   
    /// inte_business_field_distribute_details
    /// @author User
    /// @date 2024-06-13 03:32:11
    /// </summary>
    public class InteBusinessFieldDistributeDetailsEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

       /// <summary>
        /// 字段分配管理id
        /// </summary>
        public long BusinessFieldFistributeid { get; set; }

       /// <summary>
        /// 字段id
        /// </summary>
        public long BusinessFieldId { get; set; }

       /// <summary>
        /// 序号
        /// </summary>
        public int Seq { get; set; }

       /// <summary>
        /// 是否必填 1是 2、否
        /// </summary>
        public bool? IsRequired { get; set; }

       
    }
}
