/*
 *creator: Karl
 *
 *describe: 物料维护 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料维护 查询参数
    /// </summary>
    public class ProcMaterialQuery
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [Required(ErrorMessage = "物料编码不能为空")]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
    }
}
