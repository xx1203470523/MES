namespace Hymson.MES.CoreServices.Dtos.Manufacture
{

    /// <summary>
    /// 条码绑定
    /// </summary>
    public class ManuBindDto
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 模组/Pack条码
        /// </summary>
        public string SFC { get; set; } = string.Empty;

        /// <summary>
        /// 模组绑电芯条码/Pack绑模组条码
        /// </summary>
        public IEnumerable<CirculationBindDto> BindSFCs { get; set; }
    }

    /// <summary>
    /// 绑定条码信息
    /// </summary>
    public class CirculationBindDto
    {
        /// <summary>
        /// 绑定位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 绑定的条码
        /// </summary>
        public string SFC { get; set; }
    }
}
