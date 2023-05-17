namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 请求参数（设备产品过程参数采集）
    /// </summary>
    public record EquipmentProductProcessParamDto : BaseDto
    {
        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProductProcessParamSFCDto> SFCParams { get; set; }
    }

    /// <summary>
    /// 请求参数（设备过程参数采集）
    /// </summary>
    public class EquipmentProductProcessParamSFCDto
    {
        /// <summary>
        ///  产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoDto> ParamList { get; set; }
    }

}
