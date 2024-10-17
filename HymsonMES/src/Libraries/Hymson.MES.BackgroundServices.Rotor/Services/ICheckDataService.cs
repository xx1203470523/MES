using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 检查数据
    /// </summary>
    public interface ICheckDataService
    {
        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        Task Check(int rows);
    }
}
