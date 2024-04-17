using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.CoreServices.Dtos.Qkny;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.InteVehicle
{
    /// <summary>
    /// 载具接口
    /// </summary>
    public interface IInteVehicleService
    {
        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<InteVehicleEntity> GetByCodeAsync(InteVehicleCodeQuery query);

        /// <summary>
        /// 载具绑定
        /// </summary>
        /// <param name="ivo"></param>
        /// <returns></returns>
        Task VehicleBindOperationAsync(InteVehicleBindDto ivo);

        /// <summary>
        /// 载具解绑
        /// </summary>
        /// <param name="ivo"></param>
        /// <returns></returns>
        Task<int> VehicleUnBindOperationAsync(InteVehicleUnBindDto ivo);

        /// <summary>
        /// 托盘NG电芯上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ContainerNgReportAsync(InteVehicleNgSfcDto dto);
    }
}
