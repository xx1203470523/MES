using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 请求源枚举
    /// </summary>
    public enum RequestSourceEnum : sbyte
    {
        /// <summary>
        /// 面板
        /// </summary>
        [Description("面板")]
        Panel = 1,
        /// <summary>
        /// 设备API
        /// </summary>
        [Description("设备API")]
        EquipmentApi = 2,
        /// <summary>
        /// PDA
        /// </summary>
        [Description("PDA")]
        PDA = 3
    }
}
