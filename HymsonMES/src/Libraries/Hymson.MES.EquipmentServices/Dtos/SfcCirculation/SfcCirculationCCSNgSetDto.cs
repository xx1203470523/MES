namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// CCS NG设定Dto
    /// </summary>
    public record SfcCirculationCCSNgSetDto : BaseDto
    {
        /// <summary>
        /// 模组条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// Ng位置号
        /// </summary>
        public string[] Locations { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Ng的CCS条码
        /// </summary>
        public string[] BindSFCs { get; set; } = Array.Empty<string>();
    }
}
