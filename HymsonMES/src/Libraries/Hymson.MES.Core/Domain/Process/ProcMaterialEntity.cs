using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料维护表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public partial class ProcMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :所属物料组ID 
        /// 空值 : false  
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 描述 :物料编码 
        /// 空值 : false  
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 描述 :物料名称 
        /// 空值 : false  
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : true  
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 描述 :来源 
        /// 空值 : true  
        /// </summary>
        public MaterialOriginEnum? Origin { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : true  
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// 描述 :是否默认版本 
        /// 空值 : true  
        /// </summary>
        public bool IsDefaultVersion { get; set; }

        /// <summary>
        /// 描述 :物料描述 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 描述 :采购类型 
        /// 空值 : true  
        /// </summary>
        public MaterialBuyTypeEnum? BuyType { get; set; }

        /// <summary>
        /// 描述 :工艺路线ID 
        /// 空值 : true  
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// 描述 :BomID 
        /// 空值 : true  
        /// </summary>
        public long? BomId { get; set; }

        /// <summary>
        /// 描述 :批次大小 
        /// 空值 : true  
        /// </summary>
        public decimal? Batch { get; set; }

        /// <summary>
        /// 描述 :最小包装数量 
        /// 空值 : true  
        /// </summary>
        public decimal? MinPackageQty { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        public int? ValidTime { get; set; }

        /// <summary>
        /// 描述 :数量限制
        /// </summary>
        public MaterialQuantityLimitEnum? QuantityLimit { get; set; }

        /// <summary>
        /// 标包数量
        /// </summary>
        public int? PackageNum { get; set; }

        /// <summary>
        /// 描述 :计量单位(字典定义) 
        /// 空值 : true  
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// 描述 :数据收集方式
        /// 空值 : true  
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }

        /// <summary>
        /// 描述 :验证掩码组 
        /// 空值 : true  
        /// </summary>
        public string? ValidationMaskGroup { get; set; }

        /// <summary>
        /// 验证规则id
        /// </summary>
        public long? MaskCodeId { get; set; }

        /// <summary>
        /// 描述 :基于时间(字典定义) 
        /// 空值 : true  
        /// </summary>
        public MaterialBaseTimeEnum? BaseTime { get; set; }

        /// <summary>
        /// 描述 :消耗公差 
        /// 空值 : true  
        /// </summary>
        public int? ConsumptionTolerance { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        public decimal? ConsumeRatio { get; set; }

        /// <summary>
        /// 保质期（天）
        /// </summary>
        public int? ShelfLife { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string? Specifications { get; set; }


        /// <summary>
        /// 产品型号 条码规则 生成条码使用
        /// </summary>
        public string? ProductModel { get; set; }

        /// <summary>
        /// 原材料类型
        /// </summary>
        public MaterialTypeEnum? MaterialType { get; set; }

        /// <summary>
        /// 是否唯一码
        /// </summary>
        public bool IsOnlyOne { get; set; }
    }
}