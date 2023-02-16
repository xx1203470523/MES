/*
 *creator: Karl
 *
 *describe: 工序配置打印表 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:24:06
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序配置打印表 分页参数
    /// </summary>
    public class ProcProcedurePrintReleationPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码
        /// </summary>
        public string? SiteCode { get; set; }
        public long ProcedureId { get; set; }
    }
}
