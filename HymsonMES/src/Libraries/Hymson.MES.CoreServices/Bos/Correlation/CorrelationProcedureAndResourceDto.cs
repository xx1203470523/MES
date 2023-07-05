using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Correlation
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrelationProcedureAndResourceDto:JobClassBo
    {
        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源id
        /// </summary>
        public long ResourceId { get; set; }
    }
}
