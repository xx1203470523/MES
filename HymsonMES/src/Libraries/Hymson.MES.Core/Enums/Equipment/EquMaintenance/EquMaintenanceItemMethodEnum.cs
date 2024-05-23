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
        [Description("目视")]
        SightCheck = 1,

        /// <summary>
        /// 量测
        /// </summary>
        [Description("量测")]
        Quantify = 2,


    }
}
