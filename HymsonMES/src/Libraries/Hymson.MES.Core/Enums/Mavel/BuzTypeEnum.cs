using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Mavel
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BuzTypeEnum : sbyte
    {
        /// <summary>
        /// 转子线
        /// </summary>
        [Description("转子线")]
        Rator = 1,
        /// <summary>
        /// 定子线 
        /// </summary> 
        [Description("定子线")]
        Stator = 2
    }
}
