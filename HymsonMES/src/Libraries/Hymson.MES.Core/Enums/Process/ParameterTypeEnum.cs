using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    [Flags]
    public enum ParameterTypeEnum : sbyte
    {
        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        Equipment = 1,
        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product = 2,
        /// <summary>
        /// 环境
        /// </summary>
        [Description("环境")]
        Environment = 4,
        /// <summary>
        /// 质量
        /// </summary>
        [Description("质量")]
        IQC = 8,

        /// <summary>
        /// oqc
        /// </summary>
        [Description("OQC")]
        OQC = 9,

        /// <summary>
        /// fqc
        /// </summary>
        [Description("FQC")]
        FQC = 16,
    }



    /// <summary>
    /// 参数类型展示枚举
    /// </summary>
    public enum ParameterTypeShowEnum : sbyte
    {
        /// <summary>
        /// 未分配
        /// </summary>
        [Description("未分配")]
        None = 0,
        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        Equipment = 1,
        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product = 2,
        /// <summary>
        /// 设备+产品
        /// </summary>
        [Description("设备+产品")]
        Equipment_Product = 3,
        /// <summary>
        /// 环境
        /// </summary>
        [Description("环境")]
        Environment = 4,
        /// <summary>
        /// 设备+环境
        /// </summary>
        [Description("设备+环境")]
        Equipment_Environment = 5,
        /// <summary>
        /// 产品+环境
        /// </summary>
        [Description("产品+环境")]
        Product_Environment = 6,
        /// <summary>
        /// 设备+产品+环境
        /// </summary>
        [Description("设备+产品+环境")]
        Equ_Pro_Env = 7,
        /// <summary>
        /// 质量
        /// </summary>
        [Description("质量")]
        IQC = 8,
        /// <summary>
        /// 设备+质量
        /// </summary>
        [Description("设备+质量")]
        Equ_IQC = 9,
        /// <summary>
        /// 产品+质量
        /// </summary>
        [Description("产品+质量")]
        Pro_IQC = 10,
        /// <summary>
        /// 设备+产品+质量
        /// </summary>
        [Description("设备+产品+质量")]
        Equ_Pro_IQC = 11,
        /// <summary>
        /// 环境+质量
        /// </summary>
        [Description("环境+质量")]
        Env_IQC = 12,
        /// <summary>
        /// 设备+环境+质量
        /// </summary>
        [Description("设备+环境+质量")]
        Equ_Env_IQC = 13,
        /// <summary>
        /// 产品+环境+质量
        /// </summary>
        [Description("产品+环境+质量")]
        Pro_Env_IQC = 14,
        /// <summary>
        /// 设备+产品+环境+质量
        /// </summary>
        [Description("设备+产品+环境+质量")]
        Equ_Pro_Env_IQC = 15,
        /// <summary>
        /// FQC
        /// </summary>
        [Description("FQC")]
        Equ_Pro_Env_FQC = 16,
    }
}
