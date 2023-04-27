using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 包装JOB返回类型
    /// </summary>
    public enum ManuContainerPackagJobReturnTypeEnum : sbyte
    {
        /// <summary>
        /// 装箱
        /// </summary>
        [Description("装箱")]
        JobManuPackageService = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        JobManuPackageCloseService = 2,

        /// <summary>
        /// 打开
        /// </summary>
        [Description("打开")]
        JobManuPackageOpenService = 3
    }
}
