using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Stator.ManuStatorBarcode.Query
{
    /// <summary>
    /// 定子线成品条码查询
    /// </summary>
    public class StatorSfcQuery
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public List<string> SfcList { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 定子线成品条码查询
    /// </summary>
    public class StatorSfcWaterQuery
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime StartWaterMarkTime { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Rows { get; set; }
    }
}
