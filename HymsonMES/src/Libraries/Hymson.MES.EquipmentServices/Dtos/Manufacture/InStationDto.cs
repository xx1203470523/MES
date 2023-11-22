namespace Hymson.MES.EquipmentServices.Dtos
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
    public record InBoundVehicleDto : BaseDto
    {
        /// <summary>
        /// 载具编码
        /// </summary>
        public string VehicleCode { get; set; } = "";
    }

}
