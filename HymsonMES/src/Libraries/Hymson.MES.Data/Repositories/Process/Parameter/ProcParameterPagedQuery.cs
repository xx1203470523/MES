/*
 *creator: Karl
 *
 *describe: 标准参数表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
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
    /// 标准参数表 分页参数
    /// </summary>
    public class ProcParameterPagedQuery : PagerInfo
    {
        ///// <summary>
        ///// 所属站点代码
        ///// </summary>
        //public long SiteId { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 编码（标准参数）
        /// </summary>
        public string? ParameterCode { get; set; }

        /// <summary>
        /// 名称（标准参数）
        /// </summary>
        public string? ParameterName { get; set; }

        /// <summary>
        /// 描述（标准参数）
        /// </summary>
        public string? Remark { get; set; }
    }
}
