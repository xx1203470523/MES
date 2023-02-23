/*
 *creator: Karl
 *
 *describe: 物料维护 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
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
    /// 物料维护 分页参数
    /// </summary>
    public class ProcMaterialPagedQuery : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料组ID
        /// </summary>
        public long? GroupId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Origin { get; set; }
    }
}
