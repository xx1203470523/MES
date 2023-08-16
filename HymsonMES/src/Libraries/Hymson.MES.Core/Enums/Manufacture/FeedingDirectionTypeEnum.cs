using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 上/卸料方向
    /// </summary>
    public enum FeedingDirectionTypeEnum : sbyte
    {
        /// <summary>
        /// 上料
        /// </summary>
        [Description("上料")]
        Load = 1,
        /// <summary>
        /// 卸料
        /// </summary>
        [Description("卸料")]
        Unload = 2
    }
}
