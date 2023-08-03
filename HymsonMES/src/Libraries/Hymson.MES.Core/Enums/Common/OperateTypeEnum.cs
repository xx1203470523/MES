using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperateTypeEnum : sbyte
    {
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Add = 1,
        /// <summary>
        /// 编辑
        /// </summary>
        [Description("编辑")]
        Edit = 2,
        /// <summary>
        /// 查看
        /// </summary>
        [Description("查看")]
        View = 3
    }
}
