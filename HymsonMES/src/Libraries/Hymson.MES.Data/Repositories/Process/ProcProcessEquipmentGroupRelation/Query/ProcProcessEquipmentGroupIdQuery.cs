using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process.ProcProcessEquipmentGroupRelation.Query
{
    /// <summary>
    /// 设备组查询
    /// </summary>
    public class ProcProcessEquipmentGroupIdQuery
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 设备组id
        /// </summary>
        public long ProcessEquipmentGroupId { get; set; }
    }
}
