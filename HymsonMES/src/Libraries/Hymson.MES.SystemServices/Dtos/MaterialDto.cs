using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.SystemServices.Dtos
{
    /// <summary>
    /// 物料维护Dto
    /// </summary>
    public record MaterialDto : BaseEntityDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = "";

        // MES系统中的字段
        /// <summary>
        /// 版本 
        /// </summary>
        public string? Version { get; set; } = "";

        /// <summary>
        /// 批次大小 
        /// </summary>
        public string? Batch { get; set; }

        /// <summary>
        /// 保质期（天）
        /// </summary>
        public int? ShelfLife { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }

        /// <summary>
        /// 采购类型 
        /// </summary>
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 数据收集方式
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 物料单位代码
        /// </summary>
        public string? UnitCode { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// 物料状态
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public string? Specification { get; set; }

    }
}
