using System.ComponentModel;


namespace Hymson.MES.Core.Enums.Equipment.EquMaintenance
{
    /// <summary>
    /// 保养方式
    /// </summary>
    public enum EquMaintenanceItemMethodEnum : sbyte
    {
        /// <summary>
        /// 目视
        /// </summary>
        [Description("定性")]
        SightCheck = 1,

        /// <summary>
        /// 量测
        /// </summary>
        [Description("定量")]
        Quantify = 2,


    }
}
