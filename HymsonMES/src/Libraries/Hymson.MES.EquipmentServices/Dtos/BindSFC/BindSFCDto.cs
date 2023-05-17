namespace Hymson.MES.EquipmentServices.Dtos.BindSFC
{
    /// <summary>
    /// 条码绑定请求
    /// </summary>
    public record BindSFCDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 绑定的电芯条码列表
        /// </summary>
        public string[] BindSFCs { get; set; } = Array.Empty<string>();
    }
}
