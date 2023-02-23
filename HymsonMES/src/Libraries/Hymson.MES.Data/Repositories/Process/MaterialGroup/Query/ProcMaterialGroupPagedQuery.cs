/*
 *creator: Karl
 *
 *describe: 物料组维护表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
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
    /// 物料组维护表 分页参数
    /// </summary>
    public class ProcMaterialGroupPagedQuery : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料组编码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 物料组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }

    /// <summary>
    /// 物料组维护表 分页参数
    /// </summary>
    public class ProcMaterialGroupCustomPagedQuery : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 物料组编码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }

}
