using Hymson.MES.Services.Dtos.Manufacture.WhMaterialReturn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Warehouse.WhMaterialReturn
{
    public interface IWhMaterialReturnService
    {
        /// <summary>
        /// 退料单申请
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task MaterialReturnAsync(MaterialReturnDto param);
    }
}
