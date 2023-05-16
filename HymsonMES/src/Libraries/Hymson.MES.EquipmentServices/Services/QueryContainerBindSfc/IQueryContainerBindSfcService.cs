using Hymson.MES.EquipmentServices.Request.QueryContainerBindSfc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.QueryContainerBindSfc
{
    /// <summary>
    /// 容器绑定条码查询
    /// </summary>
    public interface IQueryContainerBindSfcService
    {
        /// <summary>
        /// 容器绑定条码查询
        /// </summary>
        /// <param name="queryContainerBindSfcRequest"></param> 
        /// <returns></returns> 
        Task<IEnumerable<QueryContainerBindSfcReaponse>> QueryContainerBindSfcAsync(QueryContainerBindSfcRequest queryContainerBindSfcRequest);
    }
}
