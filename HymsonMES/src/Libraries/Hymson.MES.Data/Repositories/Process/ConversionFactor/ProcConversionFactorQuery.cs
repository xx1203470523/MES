/*
 *creator: Karl
 *
 *describe: 上料点表 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */

using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 转换系数表 查询参数
    /// </summary>
    public class ProcConversionFactorQuery
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :工序ID
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }
    }
}
