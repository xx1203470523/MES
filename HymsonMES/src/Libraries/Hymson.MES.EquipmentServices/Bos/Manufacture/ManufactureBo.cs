using Hymson.MES.Core.Enums;

namespace Hymson.MES.EquipmentServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class ManufactureDto
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; } = "";

    }
}
