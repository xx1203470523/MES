namespace Hymson.MES.CoreServices.Bos.Common
{
    /// <summary>
    /// 设备查询信息
    /// </summary>
    public  class ManufactureEquipmentBo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; } = "";

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; } = "";
    }

    public class ManufactureProcedureBo
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
