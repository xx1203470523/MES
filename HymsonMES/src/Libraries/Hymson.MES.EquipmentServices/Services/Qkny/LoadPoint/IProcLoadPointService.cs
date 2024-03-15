using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.LoadPoint
{
    /// <summary>
    /// 上料点
    /// </summary>
    public interface IProcLoadPointService
    {
        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="procLoadPointQuery"></param>
        /// <returns></returns>
        Task<ProcLoadPointEntity> GetProcLoadPointEntitiesAsync(ProcLoadPointQuery procLoadPointQuery);
    }
}
