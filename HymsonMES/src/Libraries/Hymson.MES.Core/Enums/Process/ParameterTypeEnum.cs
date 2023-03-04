using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
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
        /// IQC
        /// </summary>
        [Description("IQC")]
        IQC = 8,
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
        /// IQC
        /// </summary>
        [Description("IQC")]
        IQC = 8,
        /// <summary>
        /// 设备+IQC
        /// </summary>
        [Description("设备+IQC")]
        Equ_IQC = 9,
        /// <summary>
        /// 产品+IQC
        /// </summary>
        [Description("产品+IQC")]
        Pro_IQC = 10,
        /// <summary>
        /// 设备+产品+IQC
        /// </summary>
        [Description("设备+产品+IQC")]
        Equ_Pro_IQC = 11,
        /// <summary>
        /// 环境+IQC
        /// </summary>
        [Description("环境+IQC")]
        Env_IQC = 12,
        /// <summary>
        /// 设备+环境+IQC
        /// </summary>
        [Description("设备+环境+IQC")]
        Equ_Env_IQC = 13,
        /// <summary>
        /// 产品+环境+IQC
        /// </summary>
        [Description("产品+环境+IQC")]
        Pro_Env_IQC = 14,
        /// <summary>
        /// 设备+产品+环境+IQC
        /// </summary>
        [Description("设备+产品+环境+IQC")]
        Equ_Pro_Env_IQC = 15,
    }
}
