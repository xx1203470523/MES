using Hymson.Infrastructure;
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
        //public long ParameterGroupId { get; set; }

        /// <summary>
        /// 数据收集组编码
        /// </summary>
        public string ParameterGroupCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string ParameterGroupVersion { get; set; }
        //public string ParameterGroupName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

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

        /// <summary>
        /// 中心值
        /// </summary>
        public decimal? CenterValue { get; set; }

        /// <summary>
        /// 上限
        /// </summary>
        public decimal? MaxValue { get; set; }

        /// <summary>
        /// 下限
        /// </summary>
        public decimal? MinValue { get; set; }
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
}
