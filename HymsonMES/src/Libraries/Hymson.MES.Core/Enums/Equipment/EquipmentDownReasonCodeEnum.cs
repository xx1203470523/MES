using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备停机原因
    /// </summary>
    public enum EquipmentDownReasonCodeEnum : sbyte
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 维护保养
        /// </summary>
        [Description("维护保养")]
        Maintain = 1,
        /// <summary>
        /// 吃饭/休息
        /// </summary>
        [Description("吃饭/休息")]
        Dining = 2,
        /// <summary>
        /// 换型
        /// </summary>
        [Description("换型")]
        Remodel = 3,
        /// <summary>
        /// 设备改造
        /// </summary>
        [Description("设备改造")]
        Remake = 4,
        /// <summary>
        /// 来料不良
        /// </summary>
        [Description("来料不良")]
        Badness = 5,
        /// <summary>
        /// 设备校验
        /// </summary>
        [Description("设备校验")]
        Verify = 6,
        /// <summary>
        /// 首件/点检
        /// </summary>
        [Description("首件/点检")]
        SpotCheck = 7,
        /// <summary>
        /// 品质异常
        /// </summary>
        [Description("品质异常")]
        AbnormalQuality = 8,
        /// <summary>
        /// 缺备件
        /// </summary>
        [Description("缺备件")]
        SparePartLack = 9,
        /// <summary>
        /// 环境异常
        /// </summary>
        [Description("环境异常")]
        AbnormalEnvironment = 10,
        /// <summary>
        /// 设备信息不完善
        /// </summary>
        [Description("设备信息不完善")]
        Faultiness = 11,
        /// <summary>
        /// 故障停机
        /// </summary>
        [Description("故障停机")]
        DownFault = 12
    }
}
