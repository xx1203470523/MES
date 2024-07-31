using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using OfficeOpenXml.Attributes;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 设备过程参数报表Dto
    /// </summary>
    public record EquProcessParameterReportDto : BaseEntityDto
    {
        /// <summary>
        /// 工作中心Name
        /// </summary>
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

       /// <summary>
        /// 采集值
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// 空值 : false  
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime? CollectionTime { get; set; }

       /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }
       
    }

    /// <summary>
    /// 设备过程参数报表分页Dto
    /// </summary>
    public class EquProcessParameterReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 参数Id
        /// </summary>
        public long? ParameterId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }
    }

    public class EquProcessParameterExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 设备过程参数导出模板
    /// </summary>
    public record EquProcessParameterExportDto : BaseExcelDto
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
        /// 参数编码
        /// </summary>
        [EpplusTableColumn(Header = "参数编码", Order = 3)]
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        [EpplusTableColumn(Header = "参数名称", Order = 4)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 采集值
        /// </summary>
        [EpplusTableColumn(Header = "采集值", Order = 5)]
        public string ParameterValue { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// </summary>
        [EpplusTableColumn(Header = "单位", Order = 6)]
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        [EpplusTableColumn(Header = "采集时间", Order = 7)]
        public DateTime? CollectionTime { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        [EpplusTableColumn(Header = "资源编码", Order = 8)]
        public string ResCode { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [EpplusTableColumn(Header = "资源名称", Order = 9)]
        public string ResName { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        [EpplusTableColumn(Header = "工序编码", Order = 10)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [EpplusTableColumn(Header = "工序名称", Order = 11)]
        public string ProcedureName { get; set; }

        /// <summary>
        /// 工作中心名称
        /// </summary>
        [EpplusTableColumn(Header = "工作中心名称", Order = 12)]
        public string WorkCenterName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EpplusTableColumn(Header = "创建时间", Order = 13)]
        public DateTime CreatedOn { get; set; }
    }
}
