using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcMaterialSupplierView : BaseEntity
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 描述 :供应商编码 
        /// 空值 : false  
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述 :供应商名称 
        /// 空值 : false  
        /// </summary>
        public string Name { get; set; }

    }
}
