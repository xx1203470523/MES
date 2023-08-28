namespace Hymson.MES.EquipmentServices.Dtos.SfcCirculation
{
    /// <summary>
    /// 模组CCS信息
    /// </summary>
    public class CirculationModuleCCSInfoDto
    {
        /// <summary>
        /// 模组型号
        /// </summary>
        public string? ModelCode { get; set; }

        /// <summary>
        /// CCS条码
        /// </summary>
        public string? SFC { get; set; }
        /// <summary>
        /// CCS位置/型号
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// 模组是否存在NG
        /// </summary>
        public bool IsNg { get; set; } = false;
    }
}
