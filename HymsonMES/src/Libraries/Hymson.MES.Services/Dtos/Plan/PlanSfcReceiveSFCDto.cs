using Hymson.MES.Core.Enums;

namespace Hymson.MES.Services.Dtos.Plan
{
    /// <summary>
    /// 条码接收
    /// </summary>
    public class PlanSfcReceiveSfcDto
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }


        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum Type { get; set; }

        /// <summary>
        /// 工单计划数量
        /// </summary>
        public decimal OrderCodeQty { get; set; }

        /// <summary>
        /// 工单剩余可下达数量
        /// </summary>
        public decimal OrderResidue { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialVersion { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 关联工单号
        /// </summary>
        public string RelevanceOrderCode { get; set; }
    }
}
