using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备报警状态
    /// </summary>
    public enum EquipmentAlarmStatusEnum : sbyte
    {
        /// <summary>
        /// 触发
        /// </summary>
        [Description("触发")]
        Trigger = 0,
        /// <summary>
        /// 自动恢复运行
        /// </summary>
        [Description("恢复")]
        Recover = 1
    }
}
