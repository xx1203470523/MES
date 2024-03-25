using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Equipment
{
    /// <summary>
    /// 数据实体（备件注册表）   
    /// equ_spare_parts
    /// @author Kongaomeng
    /// @date 2023-12-18 02:37:31
    /// </summary>
    public class EquSparePartsEntity : BaseEntity
    {
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
        /// 备件编码
        /// </summary>
        public string? SparePartsGroupCode { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public string? SparePartsGroup { get; set; }

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
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = "";
    }

    public class UpdateSparePartsTypeEntity : BaseEntity
    {
     /// <summary>
     /// 关联备件
      /// </summary>
        public IEnumerable<long>? SparePartIds { get; set; }

        /// <summary>
        /// 备件类型
        /// </summary>
        public IEnumerable<long>? SparePartGroupIds { get; set; }
    }
}
