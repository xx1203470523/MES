using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// FQC检验项目批次单位
    /// </summary>
    public enum FQCLotUnitEnum : sbyte
    {
        /// <summary>
        /// 托盘
        /// </summary>
        [Description("托盘")]
        Tray = 1,
        /// <summary>
        /// 栈板
        /// </summary>
        [Description("栈板")]
        Pallet = 2,
        /// <summary>
        /// 个
        /// </summary>
        [Description("个")]
        EA = 3,
        /// <summary>
        /// 箱
        /// </summary>
        [Description("箱")]
        Box = 4
    }
}
