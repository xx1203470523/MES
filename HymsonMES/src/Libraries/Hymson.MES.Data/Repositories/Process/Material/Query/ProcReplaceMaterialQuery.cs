/*
 *creator: Karl
 *
 *describe: 物料替代组件表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-09 11:28:39
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料替代组件表 查询参数
    /// </summary>
    public class ProcReplaceMaterialQuery
    {
        public long SiteId { get; set; }

        public long MaterialId { get; set; }

        //public long ReplaceMaterialId { get; set; }
    }
}
