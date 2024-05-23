using System.ComponentModel;


namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 设备点检方式
    /// </summary>
    public enum EquSpotcheckItemMethodEnum : sbyte
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

    /// <summary>
    /// 设备点检方式
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
