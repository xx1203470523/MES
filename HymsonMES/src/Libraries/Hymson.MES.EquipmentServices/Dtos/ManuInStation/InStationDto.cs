using Hymson.Infrastructure;

namespace Hymson.MES.EquipmentServices
{
    /// <summary>
    /// 进站
    /// </summary>
    public record InStationDto : BaseDto
    {
        /// <summary>
        /// SFC
        /// </summary>
        public string SFC { get; set; } = "";
    }

}
