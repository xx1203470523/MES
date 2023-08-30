using Hymson.MES.EquipmentServices.Dtos.OutBound;

namespace Hymson.MES.EquipmentServices.Dtos.EquipmentCollect
{
    /// <summary>
    /// 设备产品NG
    /// </summary>
    public record EquipmentProductNgDto : BaseDto
    {
        public IEnumerable<EquipmentProductNgSFCDto> SFCParams { get; set; }
    }

    /// <summary>
    /// 请求参数（设备产品NG）
    /// </summary>
    public class EquipmentProductNgSFCDto
    {
        /// <summary>
        ///  产品条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// NG列表
        /// </summary>
        public Ng[]? NgList { get; set; }
    }
}
