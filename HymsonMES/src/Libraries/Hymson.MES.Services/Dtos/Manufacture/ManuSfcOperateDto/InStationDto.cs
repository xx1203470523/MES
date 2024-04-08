namespace Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperate
{
    /// <summary>
    /// 进站（单条码）
    /// </summary>
    public record InBoundDto : BaseDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; } = "";
    }

    /// <summary>
    /// 进站（单条码）
    /// </summary>
    public record InBoundItemDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; } = "";
    }

    /// <summary>
    /// 进站（多条码）
    /// </summary>
    public record InBoundMoreDto : BaseDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public IEnumerable<InBoundItemDto> SFCs { get; set; }

        
    }

    /// <summary>
    /// 进站（单载具）
    /// </summary>
    public record InBoundCarrierDto : BaseDto
    {
        /// <summary>
        /// 载具编码
        /// </summary>
        public string CarrierNo { get; set; } = string.Empty;
    }

}
