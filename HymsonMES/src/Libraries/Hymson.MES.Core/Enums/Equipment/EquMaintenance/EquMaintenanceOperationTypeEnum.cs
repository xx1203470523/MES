using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    public enum EquMaintenanceOperationTypeEnum : sbyte
    {
        /// <summary>
        /// 开始检验
        /// </summary>
        [Description("开始保养")]
        Start = 1,
        /// <summary>
        /// 完成检验
        /// </summary>
        [Description("完成保养")]
        Complete = 2,
        /// <summary>
        /// 关闭检验
        /// </summary>
        [Description("关闭保养")]
        Close = 3
    }
}
