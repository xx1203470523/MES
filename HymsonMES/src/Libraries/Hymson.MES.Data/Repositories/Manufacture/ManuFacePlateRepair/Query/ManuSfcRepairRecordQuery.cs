using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query
{
    /// <summary>
    /// 维修记录
    /// </summary>
    public class ManuSfcRepairRecordQuery
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 步骤id
        /// </summary>
        public long? SfcStepId { get; set; }
    }
}
