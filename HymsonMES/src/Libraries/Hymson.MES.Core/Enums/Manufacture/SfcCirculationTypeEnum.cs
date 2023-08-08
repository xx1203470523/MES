using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// ManuSfcCirculation类型枚举
    /// </summary>
    public enum SfcCirculationTypeEnum : sbyte
    {
        //流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
        /// <summary>
        /// 拆分
        /// </summary>
        [Description("拆分")]
        Split = 1,
        /// <summary>
        /// 合并
        /// </summary>
        [Description("合并")]
        Merge = 2,
        /// <summary>
        /// 转换
        /// </summary>
        [Description("转换")]
        Change = 3,
        /// <summary>
        /// 消耗
        /// </summary>
        [Description("消耗")]
        Consume = 4,
        /// <summary>
        /// 拆解
        /// </summary>
        [Description("拆解")]
        Disassembly = 5,
        /// <summary>
        /// 组件添加
        /// </summary>
        [Description("组件添加")]
        ModuleAdd = 6,
        /// <summary>
        /// 换件
        /// </summary>
        [Description("换件")]
        ModuleReplace = 7,
        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        Bind = 8,
        /// <summary>
        /// 绑定CCS
        /// </summary>
        [Description("绑定CCS")]
        BindCCS = 9,
    }
}
