using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 
    /// </summary>
    public enum WorkCenterSourceEnum : short
    {
        /// <summary>
        /// MES
        /// </summary>
        [Description("MES")]
        MES = 1,

        /// <summary>
        /// ERP
        /// </summary>
        [Description("ERP")]
        ERP = 2

    }
}
