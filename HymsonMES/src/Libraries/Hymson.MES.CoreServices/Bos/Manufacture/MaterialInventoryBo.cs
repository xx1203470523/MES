using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 车间库存接收
    /// </summary>
    public class MaterialInventoryBo : CoreBaseBo
    {
        /// <summary>
        /// 是否校验供应商
        /// </summary>
        public bool IsCheckSupplier { get; set; } = true;

        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<MaterialInventorySfcInfoBo> BarCodeList { get; set; }
    }

    /// <summary>
    /// 车间库存接收条码
    /// </summary>
    public class MaterialInventorySfcInfoBo
    {
        /// <summary>
        /// 来源
        /// </summary>
        public MaterialInventorySourceEnum Source { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string MaterialBarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal QuantityResidue { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string? Batch { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public WhMaterialInventoryTypeEnum Type { get; set; }

        /// <summary>
        /// 有效期/到期日
        /// </summary>
        public DateTime? DueDate { get; set; }

    }
}
