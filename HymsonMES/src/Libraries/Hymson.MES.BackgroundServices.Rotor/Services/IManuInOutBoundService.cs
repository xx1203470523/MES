using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 过站接口
    /// </summary>
    public interface IManuInOutBoundService
    {
        /// <summary>
        /// 进出站
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        Task InOutBoundAsync(int rows);
    }
}
