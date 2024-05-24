using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 组件使用报告 分页参数
    /// </summary>
    public class ComUsageReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 组件物料编码ID
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 计划开始时间  时间范围 
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        /// <summary>
        /// 批次条码
        /// </summary>
        public string? CirculationBarCode { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long? CirculationMainSupplierId { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
        /// </summary>
        public ManuBarCodeRelationTypeEnum? CirculationType { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }
    }

    public class ComUsageReportViewDto
    {
        /// <summary>
        /// 车间作业控制
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 物料/版本
        /// </summary>
        public string ProductCodeVersion {get;set;}

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 组件车间作业
        /// </summary>
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 组件/版本
        /// </summary>
        public string CirculationProductCodeVersion { get; set; }
    }

    /// <summary>
    /// 组件使用模板模型
    /// </summary>
    public record ComUsageReportExcelExportDto : BaseExcelDto
    {
        /// <summary>
        /// 车间作业控制
        /// </summary>
        [EpplusTableColumn(Header = "车间作业控制", Order = 1)]
        public string SFC { get; set; }


        /// <summary>
        /// 物料/版本
        /// </summary>
        [EpplusTableColumn(Header = "物料/版本", Order = 2)]
        public string ProductCodeVersion { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        [EpplusTableColumn(Header = "工单", Order = 3)]
        public string OrderCode { get; set; }

        /// <summary>
        /// 组件车间作业
        /// </summary>
        [EpplusTableColumn(Header = "组件车间作业", Order = 4)]
        public string CirculationBarCode { get; set; }

        /// <summary>
        /// 组件/版本
        /// </summary>
        [EpplusTableColumn(Header = "组件/版本", Order = 5)]
        public string CirculationProductCodeVersion { get; set; }
    }

    public class ComUsageExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }
}
