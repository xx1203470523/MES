namespace Hymson.MES.EquipmentServices.Request.Equipment
{
    /// <summary>
    /// 请求参数（设备产品过程参数采集）
    /// </summary>
    public class EquipmentProductProcessParamRequest : BaseRequest
    {
        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProductProcessParamSFCRequest> SFCParams { get; set; }
    }

    /// <summary>
    /// 请求参数（设备过程参数采集）
    /// </summary>
    public class EquipmentProductProcessParamSFCRequest
    {
        /// <summary>
        ///  产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoRequest> ParamList { get; set; }
    }

}
