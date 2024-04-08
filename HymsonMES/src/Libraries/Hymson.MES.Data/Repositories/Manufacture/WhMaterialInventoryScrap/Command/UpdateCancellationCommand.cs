using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.WhMaterialInventoryScrap.Command
{
    public class UpdateCancellationCommand : UpdateCommand
    {
        /// <summary>
        /// 物料报废ID
        /// </summary>
        public long InventoryScrapId { get; set; }
        
        /// <summary>
        /// 取消物料报废的台账ID
        /// </summary>
        public long CancelMaterialStandingbookId { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public TrueOrFalseEnum IsCancellation { get; set; }
    }
}