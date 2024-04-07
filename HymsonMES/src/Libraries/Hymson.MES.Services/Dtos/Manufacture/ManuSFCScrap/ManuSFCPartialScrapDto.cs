using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap
{
    /// <summary>
    /// 部分报废实体
    /// </summary>
    public class ManuSFCPartialScrapDto
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 流出工序
        /// </summary>
        public IEnumerable<BarcodeScrap> BarcodeScrapList { get; set; }
    }

    /// <summary>
    /// 报废条码信息
    /// </summary>
    public class BarcodeScrap
    {
        /// <summary>
        /// 发现不合格代码
        /// </summary>
        public long FindProcedureId { get; set; }


        /// <summary>
        /// 流出工序
        /// </summary>
        public long OutFlowProcedureId { get; set; }

        /// <summary>
        /// 报废条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 不合格代码
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }
    }
}
