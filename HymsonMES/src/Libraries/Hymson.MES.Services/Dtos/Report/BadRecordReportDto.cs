using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Report
{
    public class BadRecordReportDto : PagerInfo
    {
        ///// <summary>
        ///// 站点id
        ///// </summary>
        //public long SiteId { get; set; }

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

    public class ManuProductBadRecordReportViewDto
    {
        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 汇总数量
        /// </summary>
        public decimal Num { get; set; }

        /// <summary>
        /// 描述 :不合格代码 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCode { get; set; }

        /// <summary>
        /// 描述 :不合格代码名称 
        /// 空值 : false  
        /// </summary>
        public string UnqualifiedCodeName { get; set; }
    }
}
