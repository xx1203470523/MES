using Hymson.MES.SystemServices.Dtos.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Services.Warehouse.WhMaterialReturn
{
    public interface IWhMaterialReturnService
    {
        /// <summary>
        /// 退料确认
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> WhMaterialReturnConfirmAsync(WhMaterialReturnConfirmDto param);
    }
}
