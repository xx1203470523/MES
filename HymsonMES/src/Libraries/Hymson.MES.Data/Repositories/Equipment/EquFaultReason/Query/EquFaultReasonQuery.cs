/*
 *creator: pengxin
 *
 *describe: 设备故障原因表 查询类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment 
{
    /// <summary>
    /// 设备故障原因表 查询故障原因
    /// </summary>
    public class EquFaultReasonQuery: QueryAbstraction
    {
        //
        // 摘要:
        //     站点id
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :故障原因代码 
        /// 空值 : false  
        /// </summary>
        public string FaultReasonCode { get; set; } 

        /// <summary>
        /// Ids
        /// </summary>
        public IEnumerable<long>? Ids { get; set; }
    }
}
