/*
 *creator: Karl
 *
 *describe: BOM明细表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
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
    /// BOM明细表 分页参数
    /// </summary>
    public class ProcBomDetailPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
