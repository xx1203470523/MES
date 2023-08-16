namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 模组条码绑定位置
    /// </summary>
    public record SfcBindCCSLocationDto : BaseDto
    {
        /// <summary>
        /// 模组条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;
    }
}
