namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 请求参数（设备产品过程参数采集-无在制品条码）
    /// </summary>
    public record EquipmentProductProcessParamInNotCanSFCDto : BaseDto
    {
        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoDto> ParamList { get; set; }
    }
}
