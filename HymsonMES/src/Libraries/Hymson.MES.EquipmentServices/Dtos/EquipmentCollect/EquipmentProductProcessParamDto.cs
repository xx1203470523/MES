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
        /// NG列表
        /// </summary>
        public Ng[]? NgList { get; set; }

        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoDto> ParamList { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string? CreatedBy { get; set; }
    }

    /// <summary>
    /// 请求参数（设备过程参数采集,单条码）
    /// </summary>
    public record EquipmentProductProcessParamSingleDto : BaseDto
    {
        /// <summary>
        ///  产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// NG列表
        /// </summary>
        public Ng[]? NgList { get; set; }

        /// <summary>
        ///  故障详细信息
        /// </summary>
        public IEnumerable<EquipmentProcessParamInfoDto> ParamList { get; set; }
    }


    /// <summary>
    /// NG代码
    /// </summary>
    public class Ng
    {
        /// <summary>
        /// NG代码
        /// </summary>
        public string NGCode { get; set; } = string.Empty;
    }

}
