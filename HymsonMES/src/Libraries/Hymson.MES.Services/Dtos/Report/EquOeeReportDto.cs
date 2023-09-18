using Confluent.Kafka;
using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Mysqlx.Crud;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// OEE返回结果
    /// </summary>
    public record EquOeeReportViewDto : BaseEntityDto
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 计划开机时长(分钟)
        /// </summary>
        public decimal? PlanTimeDuration { get; set; }

        /// <summary>
        /// 实际开机时长(分钟)
        /// </summary>
        public decimal? WorkTimeDuration { get; set; }

        /// <summary>
        /// 停机时长(分钟)
        /// </summary>
        public decimal? LostTimeDuration { get; set; }

        /// <summary>
        /// 设备理论产量(个)
        /// </summary>
        public decimal? TheoryOutputQty { get; set; }
        /// <summary>
        /// 实际产量
        /// </summary>
        public decimal? OutputQty { get; set; }

        /// <summary>
        /// 产出总数量 (==实际产量)
        /// </summary>
        public decimal? OutputSumQty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal? QualifiedQty { get; set; }

        /// <summary>
        /// 设备可用率 = (设备运行时间/规定时间)×100%
        /// </summary>
        public decimal AvailableRatio { get; set; }

        /// <summary>
        /// 设备效率 = (实际产量/设备理论产量)×100%
        /// </summary>
        public decimal WorkpieceRatio { get; set; }
        /// <summary>
        /// 质量合格率 = (合格产品数量/总产量)×100%
        /// </summary>
        public decimal QualifiedRatio { get; set; }

        /// <summary>
        /// OEE = 设备可用率×设备效率×质量合格率
        /// </summary>
        public decimal Oee { get; set; }
    }

    /// <summary>
    /// OEE查询对象
    /// </summary>
    public class EquOeeReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string[]? EquipmentCodes { get; set; }

        /// <summary>
        /// 0白班+夜班、1白班、2夜班
        /// </summary>
        public int DayShift { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public DateTime[] QueryTime { get; set; }

    }

    /// <summary>
    /// OEE导出
    /// </summary>
    public record EquOeeReportExportDto : BaseExcelDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        [EpplusTableColumn(Header = "设备编码", Order = 1)]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [EpplusTableColumn(Header = "设备名称", Order = 2)]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 计划开机时长
        /// </summary>
        [EpplusTableColumn(Header = "计划开机时长", Order = 3)]
        public decimal PlanTimeDuration { get; set; }

        /// <summary>
        /// 实际开机时长(分钟)
        /// </summary>
        [EpplusTableColumn(Header = "实际开机时长", Order = 4)] 
        public decimal WorkTimeDuration { get; set; }

        /// <summary>
        /// 停机时长(分钟)
        /// </summary>
        [EpplusTableColumn(Header = "停机时长", Order = 5)] 
        public decimal LostTimeDuration { get; set; }

        /// <summary>
        /// 设备理论产量(个)
        /// </summary>
        [EpplusTableColumn(Header = "设备理论产量", Order = 6)] 
        public decimal TheoryOutputQty { get; set; }
        /// <summary>
        /// 实际产量
        /// </summary>
        [EpplusTableColumn(Header = "实际产量", Order = 7)] 
        public decimal OutputQty { get; set; }

        /// <summary>
        /// 产出总数量 (==实际产量)
        /// </summary>
        [EpplusTableColumn(Header = "产出总数量", Order = 8)] 
        public decimal OutputSumQty { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        [EpplusTableColumn(Header = "合格数量", Order = 9)]
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 设备可用率 = (设备运行时间/规定时间)×100%
        /// </summary>
        [EpplusTableColumn(Header = "设备可用率", Order = 10)] 
        public decimal AvailableRatio { get; set; }

        /// <summary>
        /// 设备效率 = (实际产量/设备理论产量)×100%
        /// </summary>
        [EpplusTableColumn(Header = "设备效率", Order = 11)]
        public decimal WorkpieceRatio { get; set; }
        /// <summary>
        /// 质量合格率 = (合格产品数量/总产量)×100%
        /// </summary>
        [EpplusTableColumn(Header = "质量合格率", Order = 12)]
        public decimal QualifiedRatio { get; set; }

        /// <summary>
        /// OEE = 设备可用率×设备效率×质量合格率
        /// </summary>
        [EpplusTableColumn(Header = "OEE", Order = 13)]
        public decimal Oee { get; set; }

    }

}