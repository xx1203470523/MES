using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Integrated
{
    /// <summary>
    /// 接口日志返回结果
    /// </summary>
    public enum ResponseResultEnum : sbyte
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Failure = 1,

    }

    /// <summary>
    /// 系统日志查询类型
    /// </summary>
    public enum InterfaceLogQueryTyeEnum : sbyte
    {
        /// <summary>
        /// 设备对接
        /// </summary>
        [Description("设备对接")]
        EquipmentLog = 0,
        /// <summary>
        /// 系统对接
        /// </summary>
        [Description("系统对接")]
        SystemAbutmentLog = 1,
        /// <summary>
        /// 系统调用
        /// </summary>
        [Description("用户操作")]
        SystemLog = 2,
    }
}
