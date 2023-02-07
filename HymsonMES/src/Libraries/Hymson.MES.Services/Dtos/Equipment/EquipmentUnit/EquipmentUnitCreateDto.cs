using Hymson.Infrastructure;

namespace Hymson.MES.Services.Dtos.Equipment.EquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public record EquipmentUnitCreateDto : BaseEntityDto
    {
        /// <summary>
        /// 单位编码 
        /// </summary>
        public string UnitCode { get; set; } = "";

        /// <summary>
        /// 单位名称 
        /// </summary>
        public string UnitName { get; set; } = "";

        /// <summary>
        /// 单位类型
        /// </summary>
        public string Type { get; set; } = "";

        /// <summary>
        /// 单位状态 
        /// </summary>
        public string Status { get; set; } = "";
    }
}
