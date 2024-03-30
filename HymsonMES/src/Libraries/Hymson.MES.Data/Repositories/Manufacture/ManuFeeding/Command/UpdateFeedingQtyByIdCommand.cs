using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateFeedingQtyByIdCommand : UpdateCommand
    {
        /// <summary>
        /// ID（主键）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID（物料）
        /// </summary>
        public decimal Qty { get; set; }

    }

    #region 顷刻

    /// <summary>
    /// 更新数量
    /// </summary>
    public class UpdateFeedingQtyCommand : UpdateCommand
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 上料点
        /// </summary>
        public long FeedingPointId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public long IsDeleted { get; set; } = 0;
    }

    /// <summary>
    /// 上料点条码数量
    /// </summary>
    public class UpdateFeedingBarcodeQtyCommand : UpdateCommand
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    #endregion

}
