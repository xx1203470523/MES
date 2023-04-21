/*
 *creator: Karl
 *
 *describe: 产品不良录入 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 不良报告 分页参数
    /// </summary>
    public class ManuProductBadRecordReportPagedQuery : PagerInfo
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
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 创建时间-开始
        /// </summary>
        public DateTime? CreatedOnS { get; set; }

        /// <summary>
        /// 创建时间-结束
        /// </summary>
        public DateTime? CreatedOnE { get; set; }
    }
}
