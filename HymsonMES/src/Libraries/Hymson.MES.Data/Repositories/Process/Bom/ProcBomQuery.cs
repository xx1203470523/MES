/*
 *creator: Karl
 *
 *describe: BOM表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM表 查询参数
    /// </summary>
    public class ProcBomQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 描述 :BOM 
        /// 空值 : false  
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// 描述 :版本 
        /// 空值 : false  
        /// </summary>
        public string Version { get; set; }

    }
}
