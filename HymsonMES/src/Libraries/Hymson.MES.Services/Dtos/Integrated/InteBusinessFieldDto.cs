using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Integrated;

namespace Hymson.MES.Services.Dtos.Integrated
{
    /// <summary>
    /// 字段定义新增/更新Dto
    /// </summary>
    public record InteBusinessFieldSaveDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 字段定义Dto
    /// </summary>
    public record InteBusinessFieldDto : BaseEntityDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

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

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

       /// <summary>
        /// 删除标识
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 字段定义分页Dto
    /// </summary>
    public class InteBusinessFieldPagedQueryDto : PagerInfo { }

}
