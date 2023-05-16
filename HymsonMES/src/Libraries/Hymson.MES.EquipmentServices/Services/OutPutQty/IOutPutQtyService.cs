using Hymson.MES.EquipmentServices.Request.OutPutQty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.OutPutQty
{
    /// <summary>
    /// 产出上报数量
    /// </summary>
    public interface IOutPutQtyService
    {
        /// <summary>
        /// 产出上报数量
        /// </summary>
        /// <param name="outPutQtyRequest"></param> 
        /// <returns></returns> 
        Task OutPutQtyAsync(OutPutQtyRequest outPutQtyRequest);
    }
}
