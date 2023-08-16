using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum ProcessRouteTypeEnum : sbyte
    {
        /// <summary>
        /// 生产主工艺路线
        /// </summary>
        [Description("生产主工艺路线")]
        ProductionRoute = 1,
        /// <summary>
        /// 不合格工艺路线
        /// </summary>
        [Description("不合格工艺路线")]
        UnqualifiedRoute = 2
    }
}
