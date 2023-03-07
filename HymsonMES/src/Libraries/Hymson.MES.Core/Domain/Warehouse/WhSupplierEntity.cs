/*
 *creator: Karl
 *
 *describe: 供应商    实体类 | 代码由框架生成  如果数据库字段发生变化,则手动调整
 *builder:  pengxin
 *build datetime: 2023-03-03 01:51:43
 */
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Warehouse
{
    /// <summary>
    /// 供应商，数据实体对象   
    /// wh_supplier
    /// @author pengxin
    /// @date 2023-03-03 01:51:43
    /// </summary>
    public class WhSupplierEntity : BaseEntity
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
