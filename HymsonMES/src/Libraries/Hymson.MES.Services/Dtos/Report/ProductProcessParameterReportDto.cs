using Hymson.Infrastructure;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    /// <summary>
    /// 产品过程参数报表返回Dto
    /// </summary>
    public class ProductProcessParameterReportDto
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sfc { get; set; }

        ///// <summary>
        ///// 产品Id
        ///// </summary>
        //public long ProductId { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>

        public string ProductName { get; set; }

        ///// <summary>
        ///// 工单Id
        ///// </summary>
        //public long WorkOrderId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        ///// <summary>
        ///// 工作中心Id
        ///// </summary>
        //public long WorkCenterId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string WorkCenterCode { get; set; }

        ///// <summary>
        ///// 工序Id
        ///// </summary>
        //public long ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        ///// <summary>
        ///// 标准参数Id
        ///// </summary>
        //public long ParameterId { get; set; }

        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// 参数收集时间
        /// </summary>
        public DateTime CollectionTime { get; set; }
    }

    public class ProductProcessParameterReportPagedQueryDto : PagerInfo
    {
        /// <summary>
        /// 工作中心
        /// </summary>
        public long? WorkCenterId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string? OrderCode { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public long? ParameterId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string? Sfc { get; set; }

        /// <summary>
        ///条码列表
        /// </summary>
        public IEnumerable<string>? Sfcs { get; set; }

        /// <summary>
        /// 上报时间  时间范围  数组
        /// </summary>
        public DateTime[]? CollectionTimeRange { get; set; }
    }

    public class ProductProcessParameterExportResultDto
    {
        public string Path { get; set; }

        public string FileName { get; set; }
    }

    /// <summary>
    /// 产品过程参数导出模板
    /// </summary>
    public record ProductProcessParameterExportDto : BaseExcelDto
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        [EpplusTableColumn(Header = "产品序列码", Order = 1)]
        public string Sfc { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [EpplusTableColumn(Header = "产品编码", Order = 2)]
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EpplusTableColumn(Header = "产品名称", Order = 3)]
        public string ProductName { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        [EpplusTableColumn(Header = "参数代码", Order = 4)]
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        [EpplusTableColumn(Header = "参数名称", Order = 5)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 采集值
        /// </summary>
        [EpplusTableColumn(Header = "采集值", Order = 6)]
        public string ParameterValue { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// </summary>
        [EpplusTableColumn(Header = "单位", Order = 7)]
        public string ParameterUnit { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        [EpplusTableColumn(Header = "上报时间", Order = 8)]
        public DateTime? CollectionTime { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        [EpplusTableColumn(Header = "工单号", Order = 9)]
        public string OrderCode { get; set; }

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
        /// 工作中心
        /// </summary>
        [EpplusTableColumn(Header = "工作中心", Order = 12)]
        public string WorkCenterCode { get; set; }
    }
}
