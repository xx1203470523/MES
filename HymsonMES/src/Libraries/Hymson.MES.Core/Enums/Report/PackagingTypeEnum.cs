using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums
{
    /// <summary>
    /// 包装报告查询类型
    /// </summary>
    public enum PackagingTypeEnum : sbyte
    {
        /// <summary>
        /// 容器编号
        /// </summary>
        [Description("容器编号")]
        Container = 1,
        /// <summary>
        /// 车间作业控制
        /// </summary>
        [Description("车间作业控制")]
        BarCode = 2,
        /// <summary>
        /// 工单
        /// </summary>
        [Description("工单")]
        Order = 3
    }
}
