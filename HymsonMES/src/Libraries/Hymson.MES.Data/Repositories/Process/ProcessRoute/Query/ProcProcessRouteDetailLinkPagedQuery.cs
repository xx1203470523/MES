/*
 *creator: Karl
 *
 *describe: 工艺路线工序节点关系明细表(前节点多条就存多条) 分页查询类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:17:52
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
    /// 工艺路线工序节点关系明细表(前节点多条就存多条) 分页参数
    /// </summary>
    public class ProcProcessRouteDetailLinkPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;
    }
}
