using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 设备使用状态
    /// </summary>
    public enum EquipmentUseStatusEnum : sbyte 
    {
        /// <summary>
        /// 投产
        /// </summary>
        [Description("投产")]
        Put = 1,
        /// <summary>
        /// 停产
        /// </summary>
        [Description("停产")]
        Halt = 2,
        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        Scrap = 3
    }
}
