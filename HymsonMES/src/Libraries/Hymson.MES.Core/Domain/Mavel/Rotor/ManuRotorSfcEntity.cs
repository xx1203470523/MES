using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Mavel.Rotor
{
    /// <summary>
    /// 转子条码实体
    /// </summary>
    public class ManuRotorSfcEntity : BaseEntity
    {
        /// <summary>  
        /// 总成码  
        /// </summary>  
        public string Sfc { get; set; }

        /// <summary>
        /// 条码物料id
        /// </summary>
        public string SfcMaterialCode { get; set; }

        /// <summary>  
        /// 铁芯码  
        /// </summary>  
        public string TxSfc { get; set; }

        /// <summary>
        /// 铁芯码物料ID
        /// </summary>
        public string TxSfcMaterialCode { get; set; }

        /// <summary>  
        /// 轴码  
        /// </summary>  
        public string ZSfc { get; set; }

        /// <summary>
        /// 轴码物料ID
        /// </summary>
        public string ZSfcMaterialCode { get; set; }

        /// <summary>  
        /// 工单id  
        /// </summary>  
        public long? WorkOrderId { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinish { get; set; } = false;
    }
}
