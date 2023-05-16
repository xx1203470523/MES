namespace Hymson.MES.EquipmentServices.Request.Equipment
{
    /// <summary>
    /// 请求参数（设备产品过程参数采集-无在制品条码）
    /// </summary>
    public class EquipmentProductProcessParamInNotCanSFCRequest : BaseRequest
    {
        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoRequest> ParamList { get; set; }
    }
}
