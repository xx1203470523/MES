using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Core.Domain.Integrated
{
    /// <summary>
    /// 数据实体（字段定义）   
    /// inte_business_field
    /// @author User
    /// @date 2024-06-13 03:04:06
    /// </summary>
    public class InteBusinessFieldEntity : BaseEntity
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型 1、文本2、文本区域，日期，数字，复选框
        /// </summary>
        public FieldDefinitionTypeEnum Type { get; set; }

        /// <summary>
        /// 值来源 物料条码，供应商，客户，产品序列码，
        /// </summary>
        public FieldDefinitionSourceEnum Source { get; set; }

        /// <summary>
        /// 掩码组id proc_maskcode id
        /// </summary>
        public long? MaskCodeId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


    }
}
