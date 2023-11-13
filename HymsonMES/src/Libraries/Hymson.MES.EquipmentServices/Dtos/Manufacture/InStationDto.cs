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
    /// 进站（多条码）
    /// </summary>
    public record InBoundMoreDto : BaseDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public IEnumerable<InBoundDto> SFCs { get; set; }
    }


}
