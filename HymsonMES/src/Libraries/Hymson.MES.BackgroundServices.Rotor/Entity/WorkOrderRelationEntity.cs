using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Entity
{
    /// <summary>
    /// 铁芯码，轴码绑定记录表
    /// </summary>
    public class WorkOrderRelationEntity
    {
        /// <summary>  
        /// 主键ID。  
        /// </summary>  
        public string ID { get; set; }

        /// <summary>  
        /// 工单号。  
        /// </summary>  
        public string WorkNo { get; set; }

        /// <summary>  
        /// 产品编码(主码)。  
        /// </summary>  
        public string ProductNo { get; set; }

        /// <summary>  
        /// 产品编码(副码)。  
        /// </summary>  
        public string BranchProductNo { get; set; }

        /// <summary>  
        /// 表示该记录是否已被删除的标志。  
        /// 默认为false（未删除）。  
        /// </summary>  
        public bool IsDeleted { get; set; } = false;
    }
}
