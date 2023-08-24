using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 触发条件;1、开班检2、停机检3、换型检4、维修检5、换罐检
    /// </summary>
    public enum TriggerConditionEnum : sbyte
    {
        /// <summary>
        /// 开班检
        /// </summary>
        [Description("开班检")]
        Shift = 1,
        /// <summary>
        /// 停机检
        /// </summary>
        [Description("停机检")]
        Stop = 2,
        /// <summary>
        /// 换型检
        /// </summary>
        [Description("换型检")]
        Remodel = 3,
        /// <summary>
        /// 维修检
        /// </summary>
        [Description("维修检")]
        Repair = 4,
        /// <summary>
        /// 换罐检
        /// </summary>
        [Description("换罐检")]
        ChangeTank = 5
    }
}
