using Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Warehouse.WhMaterialPicking
{
    public interface IWhMaterialPickingService
    {
        /// <summary>
        /// 领料单
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> PickMaterialsRequestAsync(PickMaterialDto param);
    }
}
