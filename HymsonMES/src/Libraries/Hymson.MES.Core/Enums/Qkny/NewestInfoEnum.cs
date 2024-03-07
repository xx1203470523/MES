using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Qkny
{
    /// <summary>
    /// 设备最新类型
    /// </summary>
    public enum NewestInfoEnum : sbyte
    {
        /// <summary>
        /// 心跳
        /// </summary>
        [Description("心跳接口")]
        Heart = 1,

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态接口")]
        Status = 2,

        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录接口")]
        Login = 3,
    }
}
