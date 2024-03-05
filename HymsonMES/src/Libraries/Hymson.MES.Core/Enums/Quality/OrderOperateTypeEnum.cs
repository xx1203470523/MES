using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Quality
{
    /// <summary>
    /// 检验单操作类型枚举
    /// </summary>
    public enum OrderOperateTypeEnum : sbyte
    {
        /// <summary>
        /// 开始检验
        /// </summary>
        [Description("开始检验")]
        Start = 1,
        /// <summary>
        /// 完成检验
        /// </summary>
        [Description("完成检验")]
        Complete = 2,
        /// <summary>
        /// 关闭检验
        /// </summary>
        [Description("关闭检验")]
        Close = 3
    }
}
