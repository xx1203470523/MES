namespace Hymson.MES.EquipmentServices.Dtos.Feeding
{
    /// <summary>
    /// 请求参数（上料-原材料上料）
    /// </summary>
    public record FeedingLoadingDto : BaseDto
    {
        /// <summary>
        /// 上料条码
        /// </summary>
        public string SFC { get; set; } = "";

        /// <summary>
        /// 上料数量（上料数量，允许为空，空则取原材料条码解析数据）
        /// </summary>
        public decimal? Qty { get; set; }

        /// <summary>
        /// 是否上料点（空默认不是上料点，叠片公用的为true，独立为空或false）
        /// </summary>
        public bool? IsFeedingPoint { get; set; }
    }
}
