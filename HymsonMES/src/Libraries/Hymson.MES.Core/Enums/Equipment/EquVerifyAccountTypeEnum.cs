using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Equipment
{
    /// <summary>
    /// 设备校验账号类型
    /// </summary>
    public enum EquVerifyAccountTypeEnum : sbyte
    {
        /// <summary>
        /// 初级权限
        /// </summary>
        [Description("初级权限")]
        One = 1,
        /// <summary>
        /// 中级权限
        /// </summary>
        [Description("中级权限")]
        Two = 2,
        /// <summary>
        /// 高级权限
        /// </summary>
        [Description("高级权限")]
        Three = 3,
    }
}
