using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public record WorkOrderStepControlViewDto : BaseEntityDto
    {
        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 步骤标识
        /// </summary>
        public string Serialno { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string ProcessRout { get; set; }

        /// <summary>
        /// 排队中的数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 锁定数量
        /// </summary>
        public decimal LockQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQuantity { get; set; }
    }

    public record WorkOrderStepControlViewExportDto: BaseExcelDto
    {
        /// <summary>
        /// 物料编码/版本
        /// </summary>
        [EpplusTableColumn(Header = "物料编码", Order = 1)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 2)]
        public string OrderCode { get; set; }


        /// <summary>
        /// 步骤标识
        /// </summary>
        [EpplusTableColumn(Header = "步骤标识", Order = 3)]
        public string Serialno { get; set; }


        /// <summary>
        /// 工序
        /// </summary>
        [EpplusTableColumn(Header = "工序", Order = 4)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        [EpplusTableColumn(Header = "工艺路线", Order = 5)]
        public string ProcessRout { get; set; }

        /// <summary>
        /// 排队中的数量
        /// </summary>
        [EpplusTableColumn(Header = "排队中的数量", Order = 6)]
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        [EpplusTableColumn(Header = "在制数量", Order = 7)]
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        [EpplusTableColumn(Header = "完成数量", Order = 8)]
        public decimal FinishProductQuantity { get; set; }

        /// <summary>
        /// 锁定数量
        /// </summary>
        [EpplusTableColumn(Header = "锁定数量", Order = 9)]
        public decimal LockQuantity { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        [EpplusTableColumn(Header = "报废数量", Order = 10)]
        public decimal ScrapQuantity { get; set; }
    }

    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderStepControlPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 步骤标识
        /// </summary>
        public string Serialno { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public string ProcessRout { get; set; }

        /// <summary>
        /// 排队中的数量
        /// </summary>
        public decimal PassDownQuantity { get; set; }

        /// <summary>
        /// 在制数量
        /// </summary>
        public decimal ProcessDownQuantity { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishProductQuantity { get; set; }
    }

    /// <summary>
    /// 工单报告 分页参数
    /// </summary>
    public class WorkOrderStepControlOptimizePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }
    }
}
