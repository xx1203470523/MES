using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    /// <summary>
    /// 解除锁定
    /// </summary>
    public  class UnLockedProcedureCommand
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string> Sfcs { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
