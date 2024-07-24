using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrderDetail.Command
{
    public class UpdateManuReturnOrderDetailIsReceivedByIdCommand : UpdateCommand
    {
        /// <summary>
        /// ID（主键）
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ID（物料）
        /// </summary>
        public YesOrNoEnum IsReceived { get; set; }
    }
}
