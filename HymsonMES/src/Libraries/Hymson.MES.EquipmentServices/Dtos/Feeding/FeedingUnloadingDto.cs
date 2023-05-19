using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.EquipmentServices.Dtos.Feeding
{
    /// <summary>
    /// 请求参数（卸料）
    /// </summary>
    public record FeedingUnloadingDto : BaseDto
    {
        /// <summary>
        /// 卸料条码
        /// </summary>
        public string SFC { get; set; } = "";

        /// <summary>
        /// 卸料类型（2：代表剩余物料卸料；3：代表剩余物料卸料并报废）
        /// </summary>
        public FeedingUnloadingTypeEnum Type { get; set; }
    }
}
