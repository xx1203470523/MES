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
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<string> CirculationBarCodes { get; set; }

        /// <summary>
        /// 条码模糊查询
        /// </summary>
        public string? CirculationBarCodeLike { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? EndTime { get; set; }
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

    /// <summary>
    /// 条码绑定关系扭矩参数查询
    /// </summary>
    public class SfcBindParameterPagedQuery : PagerInfo
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }
    }

    /// <summary>
    /// 条码绑定关系扭矩参数结果
    /// </summary>
    public class SfcBindParameterPageData : BaseEntity
    {

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 流转条码
        /// </summary>
        public string CirculationBarcode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// TORQUE1
        /// </summary>
        public decimal Column1 { get; set; }

        /// <summary>
        /// TORQUE2
        /// </summary>
        public decimal Column2 { get; set; }

        /// <summary>
        /// TORQUE3
        /// </summary>
        public decimal Column3 { get; set; }

        /// <summary>
        /// TORQUE4
        /// </summary>
        public decimal Column4 { get; set; }

        /// <summary>
        /// TORQUE5
        /// </summary>
        public decimal Column5 { get; set; }

        /// <summary>
        /// TORQUE6
        /// </summary>
        public decimal Column6 { get; set; }

        /// <summary>
        /// TORQUE7
        /// </summary>
        public decimal Column7 { get; set; }

        /// <summary>
        /// TORQUE8
        /// </summary>
        public decimal Column8 { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public new string CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public new DateTime CreatedOn { get; set; }
    }
}
