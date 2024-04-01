using Hymson.MES.Core.Enums.Manufacture;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    #region 设备误判
    /// <summary>
    /// 条码信息（设备误判）
    /// </summary>
    public class ManuMisjudgmentBarCodeDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 发现工序
        /// </summary>
        public string FoundProcedure { get; set; } = "-";

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
    /// 提交（设备误判）
    /// </summary>
    public class ManuMisjudgmentDto
    {
        /*
        /// <summary>
        /// 集合（ID）
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }
        */

        /// <summary>
        /// 集合（条码）
        /// </summary>
        public IEnumerable<string>? BarCodes { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
    #endregion


    #region 离脱
    /// <summary>
    /// 条码信息（离脱）
    /// </summary>
    public class ManuDetachmentBarCodeDto
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
        /// 集合（条码）
        /// </summary>
        public IEnumerable<string>? BarCodes { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
    #endregion


}
