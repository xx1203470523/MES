namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 分选拆盘Dto
    /// </summary>
    public record SortingUnBindDto : QknyBaseDto
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string ContainCode { get; set; } = "";

        /// <summary>
        /// 电芯条码
        /// </summary>
        public List<string> SfcList { get; set; } = new List<string>();
    }
}
