/*
 *creator: Karl
 *
 *describe: 仓库标签模板 查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓库标签模板 查询参数
    /// </summary>
    public class ProcLabelTemplateQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
    }

    public class ProcLabelTemplateByNameQuery 
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }
    }
}
