using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.View
{
    /// <summary>
    /// 步骤表工序数量
    /// </summary>
    public class SfcStepProcedureNumView
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 根据工单工序数量
    /// </summary>
    public class SfcStepOrderProcedureNumView
    {
        /// <summary>
        /// ERP订单
        /// </summary>
        public string WorkPlanCode { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// MES工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }
}
