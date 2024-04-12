using System.ComponentModel;

namespace Hymson.MES.Core.Enums.Process
{
    public enum MaterialTypeEnum: sbyte
    {
        /// <summary>
        /// 正极主料
        /// </summary>

        [Description("正极主料")]
        AnodeMain = 1,
        /// <summary>
        /// 负极主料
        /// </summary>
        [Description("负极主料")]
        CathodeMain = 3,
        /// <summary>
        /// 隔膜
        /// </summary>
        [Description("隔膜")]
        Diaphragm = 5,

        /// <summary>
        /// 正极极片
        /// </summary>
        [Description("正极极片")]
        PositivePlate = 7,

    }
}
