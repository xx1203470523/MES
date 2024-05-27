using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Equipment
{
    /// <summary>
    /// 备件注册表新增/更新Dto
    /// </summary>
    public record EquSparePartsSaveDto : BaseEntityDto
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
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 厂商
        /// </summary>
        public string? Manufacturer { get; set; }

       /// <summary>
        /// 供应商
        /// </summary>
        public string? Supplier { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 备件类型
        /// </summary>
        public long? SparePartsGroupId { get; set; }

       /// <summary>
        /// 图纸编号
        /// </summary>
        public string? DrawCode { get; set; }

       /// <summary>
        /// 规格型号
        /// </summary>
        public string? Model { get; set; }

       /// <summary>
        /// 存放位置
        /// </summary>
        public string? Position { get; set; }

       /// <summary>
        /// 是否关联设备;0、否 1、是
        /// </summary>
        public YesOrNoEnum? IsAssociatedDevice { get; set; }

       /// <summary>
        /// 是否标准件;0、否 1、是
        /// </summary>
        public YesOrNoEnum? IsStandardPart { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public long Qty { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";
       
    }

    /// <summary>
    /// 备件注册表Dto
    /// </summary>
    public record EquSparePartsDto : BaseEntityDto
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
        /// 备件编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 备件名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 厂商
        /// </summary>
        public string Manufacturer { get; set; }

       /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier { get; set; }

       /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum Status { get; set; }

       /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartsGroup { get; set; }

        /// <summary>
        /// 备件类型Id
        /// </summary>
        public long? SparePartsGroupId { get; set; }

        /// <summary>
        /// 图纸编号
        /// </summary>
        public string DrawCode { get; set; }

       /// <summary>
        /// 规格型号
        /// </summary>
        public string Model { get; set; }

       /// <summary>
        /// 存放位置
        /// </summary>
        public string Position { get; set; }

       /// <summary>
        /// 是否关联设备;0、否 1、是
        /// </summary>
        public YesOrNoEnum? IsAssociatedDevice { get; set; }

       /// <summary>
        /// 是否标准件;0、否 1、是
        /// </summary>
        public YesOrNoEnum? IsStandardPart { get; set; }

       /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; } = "";

       /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

       /// <summary>
        /// 最后修改人
        /// </summary>
        public string UpdatedBy { get; set; }

       /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public long Qty { get; set; }

        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public long IsDeleted { get; set; }

       
    }

    /// <summary>
    /// 备件注册表分页Dto
    /// </summary>
    public class EquSparePartsPagedQueryDto : PagerInfo {
        
        /// <summary>
        /// 
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 备件编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 状态;0 禁用 1、启用
        /// </summary>
        public DisableOrEnableEnum? Status { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartsGroup { get; set; }

    }

}
