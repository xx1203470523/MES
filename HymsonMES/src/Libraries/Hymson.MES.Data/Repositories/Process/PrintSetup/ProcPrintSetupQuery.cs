/*
 *creator: Karl
 *
 *describe: 上料点表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 打印设置数表 查询参数
    /// </summary>
    public class ProcPrintSetupQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }


        /// <summary>
        /// 描述:资源ID
        /// 空值 : true  
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 描述 :打印机ID
        /// 空值 : true
        /// </summary>
        public long? PrintId { get; set; }

        /// <summary>
        /// 描述 :打印模板ID
        /// 空值 : false  
        /// </summary>
        public long LabelTemplateId { get; set; }

        /// <summary>
        /// 描述：业务类型
        /// 空值 : false  
        /// </summary>
        public int BusinessType { get; set; }

    }
}
