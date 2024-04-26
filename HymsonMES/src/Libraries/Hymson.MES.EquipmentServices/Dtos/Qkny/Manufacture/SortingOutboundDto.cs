namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 分选出站
    /// </summary>
    public record SortingOutboundDto : QknyBaseDto
    {
        /// <summary>
        /// 容器
        /// </summary>
        public string ContainerCode { get; set; } = "";

        /// <summary>
        /// 是否合格(0-不合格 1-合格)
        /// </summary>
        public int Passed {  get; set; }

        /// <summary>
        /// 托盘电芯码列表
        /// </summary>
        public List<ContainerSfcDto> ContainerSfcList { get; set; } = new List<ContainerSfcDto>();

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<QknyParamBaseDto> ParamList { get; set; } = new List<QknyParamBaseDto>();
    }
}
