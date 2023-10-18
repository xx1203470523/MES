using Hymson.MES.Data.Repositories.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command
{
    /// <summary>
    /// 清空库存
    /// </summary>
    public  class UpdateWhMaterialInventoryEmptyByIdCommand : UpdateCommand
    {
       public long Id { get; set; } 
    }
}
