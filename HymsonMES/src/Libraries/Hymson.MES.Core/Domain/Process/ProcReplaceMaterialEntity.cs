/*
 *creator: Karl
 *
 *describe: 物料替代组件表    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-02-09 11:28:38
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 物料替代组件表，数据实体对象   
    /// proc_replace_material
    /// @author Karl
    /// @date 2023-02-09 11:28:38
    /// </summary>
    public class ProcReplaceMaterialEntity : BaseEntity
    {
        /// <summary>
        /// 所属站点代码
        /// </summary>
        public string SiteCode { get; set; }

       /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 替代组件ID
        /// </summary>
        public long ReplaceMaterialId { get; set; }

       /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsUse { get; set; }

       /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

       
    }
}
