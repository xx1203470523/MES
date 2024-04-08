using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;

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


    #region 返工
    /// <summary>
    /// 条码信息（返工）
    /// </summary>
    public class ManuReworkBarCodeDto
    {
        /// <summary>
        /// 条码Id
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
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

    }

    /// <summary>
    /// 提交（返工）
    /// </summary>
    public class ManuReworkDto
    {
        /// <summary>
        /// 返工类型
        /// </summary>
        public ManuReworkTypeEnum Type { get; set; }

        /// <summary>
        /// 返工工序
        /// </summary>
        public long? ReworkProcedure { get; set; }

        /// <summary>
        /// 返工工单
        /// </summary>
        public long? ReworkWorkOrder { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long? UnqualifiedCode { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        public long? FoundProcedure { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long? OutProcedure { get; set; }

        /// <summary>
        /// 集合（条码）
        /// </summary>
        public IEnumerable<string>? BarCodes { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 导入/导出模板模型
    /// </summary>
    public record ManuReworkExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [EpplusTableColumn(Header = "产品序列码(必填)", Order = 1)]
        public string BarCode { get; set; }

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
