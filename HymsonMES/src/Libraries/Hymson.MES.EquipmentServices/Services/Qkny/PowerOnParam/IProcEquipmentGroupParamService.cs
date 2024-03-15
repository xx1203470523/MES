using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.Qkny.PowerOnParam;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.PowerOnParam
{
    /// <summary>
    /// 开机参数
    /// </summary>
    public interface IProcEquipmentGroupParamService
    {
        /// <summary>
        /// 根据设备和产品型号查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcEquipmentGroupParamEquProductView>> QueryByEquProductAsync(ProcEquipmentGroupParamEquProductQuery query);

        /// <summary>
        /// 根据编码查询参数详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<List<ProcEquipmentGroupParamDetailView>> GetDetailByCode(ProcEquipmentGroupParamCodeDetailQuery query);

        /// <summary>
        /// 根据编码版本型号获取激活的数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ProcEquipmentGroupParamEntity> GetEntityByCodeVersion(ProcEquipmentGroupCheckQuery query);
    }
}
