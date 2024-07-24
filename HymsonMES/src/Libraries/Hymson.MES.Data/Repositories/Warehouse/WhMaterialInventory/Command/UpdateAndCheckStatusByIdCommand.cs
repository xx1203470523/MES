using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    /// <summary>
    /// 更新状态带状态校验
    /// </summary>
    public class UpdateAndCheckStatusByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 修改后状态
        /// </summary>
        public WhMaterialInventoryStatusEnum Status { get; set; }

        /// <summary>
        ///当前状态
        /// </summary>
        public WhMaterialInventoryStatusEnum CurrentStatus { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
    }
}
