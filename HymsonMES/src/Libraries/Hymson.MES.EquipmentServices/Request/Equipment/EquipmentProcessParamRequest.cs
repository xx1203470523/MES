namespace Hymson.MES.EquipmentServices.Request.Equipment
{
    /// <summary>
    /// 请求参数（设备过程参数采集）
    /// </summary>
    public class EquipmentProcessParamRequest : BaseRequest
    {
        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoRequest> ParamList { get; set; }
    }


}
