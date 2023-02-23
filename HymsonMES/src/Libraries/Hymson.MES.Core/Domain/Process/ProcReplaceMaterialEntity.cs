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
    /// 物料替代组件表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcReplaceMaterialEntity: BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }
        
        /// <summary>
        /// 描述 :物料ID 
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }
        
        /// <summary>
        /// 描述 :替代组件ID 
        /// 空值 : false  
        /// </summary>
        public long ReplaceMaterialId { get; set; }
        
        /// <summary>
        /// 描述 :是否启用 
        /// 空值 : true  
        /// </summary>
        public bool IsUse { get; set; }
        
        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string Remark { get; set; }

       
        }
}