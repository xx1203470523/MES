using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 条码信息
    /// </summary>
    public class ManuBarCodeDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 条码类型
        /// </summary>
        public SfcTypeEnum Type { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderCode { get; set; } = "-";

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; } = "-";

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; } = "-";

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

    }

    /// <summary>
    /// 提交（离脱）
    /// </summary>
    public class ManuDetachmentDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string>? BarCodes { get; set; }

    }

}
