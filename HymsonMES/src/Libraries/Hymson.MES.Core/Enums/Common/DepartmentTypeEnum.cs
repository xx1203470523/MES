using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 使用部门（TODO 随便写的部门）
    /// </summary>
    public enum DepartmentTypeEnum : sbyte
    {
        /// <summary>
        /// 财务部
        /// </summary>
        [Description("财务部")]
        Finance = 1,
        /// <summary>
        /// 生产部
        /// </summary>
        [Description("生产部")]
        Production = 2,
        /// <summary>
        /// 市场部
        /// </summary>
        [Description("市场部")]
        Market = 3
    }
}
