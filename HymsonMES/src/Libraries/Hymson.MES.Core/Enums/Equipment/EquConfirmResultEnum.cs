using System.ComponentModel;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum EquConfirmResultEnum : sbyte  
    {
        /// <summary>
        /// 维修完成
        /// </summary>
        [Description("维修完成")]
        RepairCompleted = 1,
        /// <summary>
        /// 重新维修
        /// </summary>
        [Description("重新维修")]
        RepairAgain = 2
    }
}
