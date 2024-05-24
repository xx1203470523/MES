using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    public enum EquMaintenanceTaskProcessedEnum : sbyte
    {
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Pass = 1,

        /// <summary>
        /// 不通过
        /// </summary>
        [Description("不通过")]
        UnPass = 2,
    }
}
