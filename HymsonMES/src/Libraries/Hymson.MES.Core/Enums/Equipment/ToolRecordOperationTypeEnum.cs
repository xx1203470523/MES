using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum ToolRecordOperationTypeEnum : sbyte
    {
        // 1、注册 2、绑定 3、解绑4、寿命扣减5、校准
        /// <summary>
        /// 注册
        /// </summary>
        [Description("注册")]
        Register = 1,

        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        Binding = 2,

        /// <summary>
        /// 解绑
        /// </summary>
        [Description("解绑")]
        UnBind = 3,

        /// <summary>
        /// 寿命扣减
        /// </summary>
        [Description("寿命扣减")]
        LifetimeDeduction = 4,

        /// <summary>
        /// 校准
        /// </summary>
        [Description("校准")]
        Calibration = 5
    }
}
