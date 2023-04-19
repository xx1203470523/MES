﻿using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Common;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuInStation
{
    /// <summary>
    /// 进站
    /// </summary>
    public interface IManuInStationService
    {
        /// <summary>
        /// 执行（进站）
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        Task<int> InStationAsync(ManufactureBo bo);

    }
}
