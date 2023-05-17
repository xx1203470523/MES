namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 请求参数（设备过程参数采集）
    /// </summary>
    public record EquipmentProcessParamDto : BaseDto
    {
        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoDto> ParamList { get; set; }
    }


}
