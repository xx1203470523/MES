namespace Hymson.MES.EquipmentServices.Dtos.BindSFC
{
    /// <summary>
    /// 条码解绑请求
    /// </summary>
    public record UnBindSFCDto : BaseDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 解绑的电芯条码列表
        /// </summary>
        public string[] BindSFCs { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// 换绑DTO
    /// </summary>
    public record SwitchBindSFCDto
    {
        /// <summary>
        /// 模组码
        /// </summary>
        public string SFC { get; set; } = string.Empty;
        /// <summary>
        /// 旧绑定的SFC
        /// </summary>
        public string OldBindSFC { get; set; } = string.Empty;
        /// <summary>
        /// 新绑定的SFC
        /// </summary>
        public string NewBindSFC { get; set; } = string.Empty;
    }
}
