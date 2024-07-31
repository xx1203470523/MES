using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 
    /// </summary>
    public record WorkshopJobControlReportViewDto : BaseEntityDto
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum SFCStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SfcStatusEnum SFCProduceStatus { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        public PlanWorkOrderTypeEnum? OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// Bom编码/版本
        /// </summary>
        public string BomCodeVersion { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 车间作业控制 分页参数
    /// </summary>
    public class WorkshopJobControlReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string? MaterialCode { get; set; }

        /// <summary>
        /// 物料版本
        /// </summary>
        public string? MaterialVersion { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? SFCStatus { get; set; }

        /// <summary>
        /// 条码在制状态
        /// </summary>
        public SfcStatusEnum? SFCProduceStatus { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }
    }

    /// <summary>
    /// 车间作业控制 分页参数
    /// </summary>
    public class WorkshopJobControlReportOptimizePagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public long? MaterialId { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum? SFCStatus { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long? ResourceId { get; set; }
    }

    /// <summary>
    /// 工单步骤报告
    /// </summary>
    public class WorkshopJobControlStepReportDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public string SFC { get; set; }
        /// <summary>
        /// 工单编码
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodrNameVersion { get; set; }
        /// <summary>
        /// 工艺路线编码/版本
        /// </summary>
        public string ProcessRouteCodeNameVersion { get; set; }
        /// <summary>
        /// BOM编码/版本
        /// </summary>
        public string ProcBomCodeNameVersion { get; set; }
        /// <summary>
        /// 集合（步骤）
        /// </summary>
        public IEnumerable<WorkshopJobControlInOutSteptDto> WorkshopJobControlInOutSteptDtos { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkshopJobControlInOutSteptDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcInOutStatusEnum Status { get; set; }

        /// <summary>
        /// 进站时间
        /// </summary>
        public DateTime InDateTime { get; set; }

        /// <summary>
        /// 出站时间
        /// </summary>
        public DateTime? OutDatetTime { get; set; }
    }

    /// <summary>
    /// 条码步骤表 分页参数
    /// </summary>
    public class ManuSfcStepBySfcPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }
    }

    public class ManuSfcStepBySfcViewDto
    {
        /// <summary>
        /// sfc_step Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 作业名称  步骤类型
        /// </summary>
        public ManuSfcStepTypeEnum Operatetype { get; set; }

        /// <summary>
        /// 作业时间  创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }
    }

    public class WorkshopJobControlResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 导出实体
    /// </summary>
    public record WorkshopJobControlExportDto : BaseExcelDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [EpplusTableColumn(Header = "产品序列码", Order = 1)]
        public string SFC { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [EpplusTableColumn(Header = "状态", Order = 2)]
        public string SFCStatus { get; set; }

        /// <summary>
        /// 物料编码/版本
        /// </summary>
        [EpplusTableColumn(Header = "产品编码/版本", Order = 3)]
        public string MaterialCodeVersion { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EpplusTableColumn(Header = "产品名称", Order = 4)]
        public string MaterialName { get; set; }

        /// <summary>
        /// 工单编码
        /// </summary>
        [EpplusTableColumn(Header = "工单", Order = 5)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        [EpplusTableColumn(Header = "工单类型", Order = 6)]
        public string? OrderType { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 7)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 8)]
        public string ProcedureName { get; set; }

        /// <summary>
        /// Bom编码/版本
        /// </summary>
        [EpplusTableColumn(Header = "Bom编码/版本", Order = 9)]
        public string BomCodeVersion { get; set; }

        /// <summary>
        /// bom名称
        /// </summary>
        [EpplusTableColumn(Header = "Bom名称", Order = 10)]
        public string BomName { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        [EpplusTableColumn(Header = "数量", Order = 11)]
        public decimal Qty { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public SfcStatusEnum SFCProduceStatus { get; set; }
    }
}
