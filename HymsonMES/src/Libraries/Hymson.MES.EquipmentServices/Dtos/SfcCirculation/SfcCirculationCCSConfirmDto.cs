namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// CCS确认Dto
    /// </summary>
    public record SfcCirculationCCSConfirmDto : BaseDto
    {
        /// <summary>
        /// 模组条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// Ng位置号
        /// </summary>
        public string Location { get; set; } = string.Empty;
    }
}
