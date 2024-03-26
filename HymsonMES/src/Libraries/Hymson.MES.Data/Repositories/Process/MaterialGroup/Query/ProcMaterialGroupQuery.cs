/*
 *creator: Karl
 *
 *describe: 物料组维护表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料组维护表 查询参数
    /// </summary>
    public class ProcMaterialGroupQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :物料组编号 
        /// 空值 : false  
        /// </summary>
        public string? GroupCode { get; set; }

        public IEnumerable<string>? GroupCodes { get; set; }
    }
}
