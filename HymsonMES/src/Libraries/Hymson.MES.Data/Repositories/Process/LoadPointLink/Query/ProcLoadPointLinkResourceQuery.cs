/*
 *creator: Karl
 *
 *describe: 上料点关联资源表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:36:09
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.LoadPointLink.Query
{
    /// <summary>
    /// 上料点关联资源表 查询参数
    /// </summary>
    public class ProcLoadPointLinkResourceQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 上料点ID
        /// </summary>
        public long? LoadPointId {  get; set; }
    }
}
