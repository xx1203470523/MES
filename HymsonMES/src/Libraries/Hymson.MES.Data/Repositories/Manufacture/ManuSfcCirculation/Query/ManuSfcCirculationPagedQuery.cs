/*
 *creator: Karl
 *
 *describe: 条码流转表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:50:00
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query
{
    /// <summary>
    /// 条码流转表 分页参数
    /// </summary>
    public class ManuSfcCirculationPagedQuery : PagerInfo
    {

    }

    /// <summary>
    /// 组件使用报告 分页参数
    /// </summary>
    public class ComUsageReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 组件物料编码ID
        /// </summary>
        public long? CirculationProductId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime[]? CreatedOn { get; set; }

        ///// <summary>
        ///// 开始时间 
        ///// </summary>
        //public DateTime? CreatedOnS { get; set; }

        ///// <summary>
        ///// 结束时间
        ///// </summary>
        //public DateTime? CreatedOnE { get; set; }

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
    }


    /// <summary>
    /// 追溯报表查询
    /// </summary>
    public class ProductTraceReportPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string? SFC { get; set; }

        /// <summary>
        /// true 正向，false 反向
        /// 默认正向追溯
        /// </summary>
        public bool TraceDirection { get; set; } = true;
    }
}
