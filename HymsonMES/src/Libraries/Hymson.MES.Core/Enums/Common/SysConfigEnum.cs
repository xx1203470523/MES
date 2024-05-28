using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 枚举（系统配置）
    /// </summary>
    public enum SysConfigEnum : sbyte
    {
        /// <summary>
        /// AQL检验水平
        /// </summary>
        [Description("AQLLevel")]
        AQLLevel = 1,
        /// <summary>
        /// AQL检验计划
        /// </summary>
        [Description("AQLPlan")]
        AQLPlan = 2,
        /// <summary>
        /// 上料点上料条码下发到设备
        /// </summary>
        [Description("UpMaterialSendEqu")]
        UpMaterialSendEqu = 3
    }
}
