using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcScrap.Command
{
    /// <summary>
    /// 取消报废
    /// </summary>
    public class ManuSfcScrapCancelCommand : UpdateCommand
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        /// 取消步骤表id
        /// </summary>
        public long CancelSfcStepId { get; set; }
    }
}
