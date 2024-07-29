using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Equipment
{
    public enum EquSpotcheckOperationTypeEnum : sbyte
    {
        /// <summary>
        /// 开始检验
        /// </summary>
        [Description("开始点检")]
        Start = 1,
        /// <summary>
        /// 完成检验
        /// </summary>
        [Description("完成点检")]
        Complete = 2,
        /// <summary>
        /// 关闭检验
        /// </summary>
        [Description("关闭点检")]
        Close = 3
    }
}
