using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Services.Dtos.Common;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Manufacture
{
    /// <summary>
    /// 含有不良记录的条码信息（公用）
    /// </summary>
    public record ManuProductNGBarCodeDto : BaseDto
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
    /// 条码信息（公用）
    /// </summary>
    public record ManuProductBarCodeDto : BaseDto
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


    #region 让步接收
    /// <summary>
    /// 条码信息（让步接收）
    /// </summary>
    public record ManuCompromiseBarCodeDto : BaseDto
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
    /// 提交（让步接收）
    /// </summary>
    public class ManuCompromiseDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 子项集合（让步接收）
        /// </summary>
        public IEnumerable<ManuCompromiseItemDto> Compromises { get; set; }

    }

    /// <summary>
    /// 提交子项（让步接收）
    /// </summary>
    public class ManuCompromiseItemDto
    {
        /// <summary>
        /// 不合格代码
        /// </summary>
        public long? UnqualifiedCodeId { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        public long? FoundProcedureId { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long? OutProcedureId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

    }

    /// <summary>
    /// 导入/导出模板模型（让步接收）
    /// </summary>
    public record ManuCompromiseExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [EpplusTableColumn(Header = "产品序列码(必填)", Order = 1)]
        public string BarCode { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        [EpplusTableColumn(Header = "发现不良工序", Order = 2)]
        public string FoundProcedure { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        [EpplusTableColumn(Header = "不合格代码(必填)", Order = 3)]
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        [EpplusTableColumn(Header = "流出不良工序", Order = 4)]
        public string OutProcedure { get; set; }

    }

    #endregion


    #region 设备误判
    /// <summary>
    /// 条码信息（设备误判）
    /// </summary>
    public record ManuMisjudgmentBarCodeDto : BaseDto
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
    public record ManuReworkBarCodeDto : BaseDto
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
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 子项集合（返工）
        /// </summary>
        public IEnumerable<ManuReworkIemDto> Reworks { get; set; }

    }

    /// <summary>
    /// 提交子项（返工）
    /// </summary>
    public class ManuReworkIemDto
    {
        /// <summary>
        /// 返工类型
        /// </summary>
        public ManuReworkTypeEnum Type { get; set; }

        /// <summary>
        /// 返工工序
        /// </summary>
        public long? ReworkProcedureId { get; set; }

        /// <summary>
        /// 返工工单
        /// </summary>
        public long? ReworkWorkOrderId { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long? UnqualifiedCodeId { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        public long? FoundProcedureId { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        public long? OutProcedureId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }

    }

    /// <summary>
    /// 导入/导出模板模型（返工）
    /// </summary>
    public record ManuReworkExcelDto : BaseExcelDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [EpplusTableColumn(Header = "产品序列码(必填)", Order = 1)]
        public string BarCode { get; set; }

        /// <summary>
        /// 返工类型
        /// </summary>
        [EpplusTableColumn(Header = "返工类型(必填)", Order = 2)]
        public ManuReworkTypeEnum Type { get; set; }

        /// <summary>
        /// 返工工序
        /// </summary>
        [EpplusTableColumn(Header = "返工工序", Order = 3)]
        public string ReworkProcedure { get; set; }

        /// <summary>
        /// 返工工单
        /// </summary>
        [EpplusTableColumn(Header = "返工工单", Order = 4)]
        public string ReworkWorkOrder { get; set; }

        /// <summary>
        /// 发现不良工序
        /// </summary>
        [EpplusTableColumn(Header = "发现不良工序", Order = 5)]
        public string FoundProcedure { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        [EpplusTableColumn(Header = "不合格代码(必填)", Order = 6)]
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 流出不良工序
        /// </summary>
        [EpplusTableColumn(Header = "流出不良工序", Order = 7)]
        public string OutProcedure { get; set; }

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
