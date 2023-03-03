/*
 *creator: Karl
 *
 *describe: 上料点表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
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
    /// 上料点表 分页参数
    /// </summary>
    public class ProcLoadPointPagedQuery : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :上料点 
        /// </summary>
        public string? LoadPoint { get; set; }

        /// <summary>
        /// 描述 :上料点名称 
        /// </summary>
        public string? LoadPointName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
