using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 载具类型验证类型枚举
    /// </summary>
    public enum VehicleTypeVerifyTypeEnum : sbyte
    {
        /// <summary>
        /// 物料
        /// </summary>
        [Description("物料")]
        Material = 1,
        /// <summary>
        /// 物料组
        /// </summary>
        [Description("物料组")]
        MaterialGroup = 2
    }
    
}
