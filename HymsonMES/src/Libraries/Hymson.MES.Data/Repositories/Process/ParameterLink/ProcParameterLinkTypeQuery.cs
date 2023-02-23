/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数关联类型表 查询参数
    /// </summary>
    public class ProcParameterLinkTypeQuery
    {
        ///// <summary>
        ///// 描述 :所属站点代码 
        ///// 空值 : false  
        ///// </summary>
        //public long SiteId { get; set; }

        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :标准参数ID 
        /// 空值 : false  
        /// </summary>
        public long ParameterID { get; set; }

        /// <summary>
        /// 描述 :参数类型 
        /// 空值 : false  
        /// </summary>
        public int ParameterType { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }
    }
}
