using Hymson.MES.SystemServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.SystemServices.Services.Manufacture
{
    /// <summary>
    /// 转子线数据上报服务
    /// </summary>
    public interface IManuRotorService
    {
        /// <summary>
        /// 上报过站数据
        /// </summary>
        /// <param name="stationData"></param>
        /// <returns></returns>
        Task UploadCrossingStationData(RotorCrossingStationData stationData);
    }
}
