using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备状态代码
    /// </summary>
    public enum EquipmentStateEnum : sbyte
    {
        /// <summary>
        /// 自动运行
        /// </summary>
        [Description("自动运行")]
        AutoRun = 0,
        /// <summary>
        /// 待机
        /// </summary>
        [Description("待机")]
        Standby = 1,
        /// <summary>
        /// 正常停机
        /// </summary>
        [Description("正常停机")]
        DownNormal = 2,
        /// <summary>
        /// 故障停机
        /// </summary>
        [Description("故障停机")]
        DownFault = 3,
        /// <summary>
        /// 待料
        /// </summary>
        [Description("待料")]
        WaitMaterials = 4,
        /// <summary>
        /// 满料
        /// </summary>
        [Description("满料")]
        FullMaterials = 5
    }
}
