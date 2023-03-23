using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManuFeedingMaterialQueryDto
    {
        /// <summary>
        /// 查询来源
        /// </summary>
        public FeedingSourceEnum Source { get; set; } = FeedingSourceEnum.Resource;

        /// <summary>
        /// 设备编码/资源编码
        /// </summary>
        public string Code { get; set; } = "";
    }

    /// <summary>
    /// 物料对象
    /// </summary>
    public class ManuFeedingMaterialDto
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料库存集合
        /// </summary>
        public List<ManuFeedingMaterialItemDto> Children { get; set; }
    }

    /// <summary>
    /// 物料库存对象
    /// </summary>
    public class ManuFeedingMaterialItemDto
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 初始数量
        /// </summary>
        public decimal InitQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
