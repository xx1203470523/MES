using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command
{
    /// <summary>
    /// 部分报废
    /// </summary>
    public  class ManuSFCPartialScrapByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        ///报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }
       
        /// <summary>
        /// 条码Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态: 1：排队中 2：活动中 3：完成-在制 4：完成 5：锁定 6：报废 7：删除
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        ///当前状态
        /// </summary>
        public SfcStatusEnum CurrentStatus { get; set; }

        /// <summary>
        ///当前数量
        /// </summary>
        public decimal CurrentQty { get; set; }
    }
}
