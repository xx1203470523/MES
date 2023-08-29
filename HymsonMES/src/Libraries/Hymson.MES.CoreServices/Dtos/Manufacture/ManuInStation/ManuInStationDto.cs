using Hymson.MES.CoreServices.Dtos.Common;

namespace Hymson.MES.CoreServices.Dtos.Manufacture.ManuInStation
{
    /// <summary>
    /// 设备进站实体类
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public sealed class EquipmentInStationDto : CoreBaseDto
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; } = "";
    }

    /// <summary>
    /// 设备批量进站实体类
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public sealed class EquipmentInStationsDto : CoreBaseDto
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }

    /// <summary>
    /// 资源进站实体类
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public sealed class ResourceInStationDto : CoreBaseDto
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }
    }

    /// <summary>
    /// 资源批量进站实体类
    /// @author wangkeming
    /// @date 2023-05-25
    /// </summary>
    public sealed class ResourceInStationsDto : CoreBaseDto
    {
        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }
}
