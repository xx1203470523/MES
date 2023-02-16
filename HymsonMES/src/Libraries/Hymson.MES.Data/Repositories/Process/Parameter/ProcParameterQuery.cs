/*
 *creator: Karl
 *
 *describe: 标准参数表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 02:50:20
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数表 查询参数
    /// </summary>
    public class ProcParameterQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public string SiteCode { get; set; }

        /// <summary>
        /// 描述 :参数代码 
        /// 空值 : false  
        /// </summary>
        public string ParameterCode { get; set; }
    }
}
