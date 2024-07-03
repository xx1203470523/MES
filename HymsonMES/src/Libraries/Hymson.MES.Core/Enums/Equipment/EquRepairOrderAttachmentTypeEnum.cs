using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 附件操作类型
    /// </summary>
    public enum EquRepairOrderAttachmentTypeEnum : sbyte
    {
        /// <summary>
        /// 报修
        /// </summary> 
        [Description("报修")]
        report = 1,

        /// <summary>
        /// 维修
        /// </summary>
        [Description("维修")]
        maintenance = 2
    }
}
