using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Enums.Manufacture
{
    /// <summary>
    /// 处置结果
    /// </summary>
    public enum ProductBadDisposalResultEnum
    {
        /// <summary>
        /// 复判关闭
        /// </summary>
        [Description("复判关闭")]
        ReJudgmentClosed = 1,

        /// <summary>
        /// 复判关闭
        /// </summary>
        [Description("复判维修")]
        ReJudgmentRepair = 2,

        /// <summary>
        /// 取消标识
        /// </summary>
        [Description("取消标识")]
        RemoveIdentify = 3,

        /// <summary>
        /// 报废
        /// </summary>
        [Description("报废")]
        scrap = 4,

        /// <summary>
        /// 自动处理
        /// </summary>
        [Description("自动处理")]
        AutoHandle = 5
    }
}
    