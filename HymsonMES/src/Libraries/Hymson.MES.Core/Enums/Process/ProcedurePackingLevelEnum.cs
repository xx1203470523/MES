using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 工序包装等级
    /// </summary>
    public enum ProcedurePackingLevelEnum : sbyte
    {
        /// <summary>
        /// 一级
        /// </summary>
        [Description("一级")]
        LevelOne= 1,
        /// <summary>
        /// 二级
        /// </summary>
        [Description("二级")]
        LevelTwo = 2,
        /// <summary>
        /// 三级
        /// </summary>
        [Description("三级")]
        LevelThree = 3
    }
}
