using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.LoadPoint.View;
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

        /// <summary>
        /// 获取上料点或者设备
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<PointOrEquipmentView>> GetPointOrEquipmmentAsync(ProcLoadPointEquipmentQuery query);

        /// <summary>
        /// 获取上料点根据编码
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcLoadPointEntity> GetProcLoadPointAsync(ProcLoadPointQuery query);

        /// <summary>
        /// 获取上料点物料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcLoadPointMaterialView>> GetProcLoadPointMaterialAsync(ProcLoadPointQuery query);
    }
}
