using Hymson.MES.Services.Dtos.Manufacture.WhMaterialPicking;
using Hymson.MES.Services.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Warehouse.WhMaterialPicking
{
    public class WhMaterialPickingService : IWhMaterialPickingService
    {
        public WhMaterialPickingService()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> PickMaterialsRequestAsync(PickMaterialDto param)
        {

            return "";
        }
    }
}
