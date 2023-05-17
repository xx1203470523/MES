using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    public enum ManuSfcBindStatusEnum : sbyte
    {
        /// <summary>
        /// 解绑
        /// </summary>
        [Description("解绑")]
        Bind = 0,
        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        UnBind = 1,
    }
}
