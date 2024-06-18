using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuProductParameter.Command
{

    /// <summary>
    /// 更新条码
    /// </summary>
   public  class UpdateProductParameterByProcedureId
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 更新条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 被更新条码
        /// </summary>
        public IEnumerable<string>  SFCs { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; }
    }
}
