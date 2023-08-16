/*
 *creator: Karl
 *
 *describe: BOM表 分页查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM表 分页参数
    /// </summary>
    public class ProcBomPagedQuery : PagerInfo
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :BomName 
        /// 空值 : false  
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// 描述 :BomName名称 
        /// 空值 : false  
        /// </summary>
        public string BomName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
