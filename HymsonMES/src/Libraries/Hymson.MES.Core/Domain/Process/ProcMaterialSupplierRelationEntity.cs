/*
 *creator: Karl
 *
 *describe: 物料供应商关系    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  Karl
 *build datetime: 2023-03-27 02:30:48
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
    /// 物料供应商关系，数据实体对象   
    /// proc_material_supplier_relation
    /// @author Karl
    /// @date 2023-03-27 02:30:48
    /// </summary>
    public class ProcMaterialSupplierRelationEntity : BaseEntity
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

       /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

       
    }
}
