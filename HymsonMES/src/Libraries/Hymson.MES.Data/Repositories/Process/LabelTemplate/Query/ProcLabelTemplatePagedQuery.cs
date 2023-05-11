/*
 *creator: Karl
 *
 *describe: 仓库标签模板 分页查询类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
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
    /// 仓库标签模板 分页参数
    /// </summary>
    public class ProcLabelTemplatePagedQuery : PagerInfo
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string? Name { get; set; }
    }
}
