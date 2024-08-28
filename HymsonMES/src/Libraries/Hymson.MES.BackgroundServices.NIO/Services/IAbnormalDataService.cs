using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 异常数据处理
    /// </summary>
    public interface IAbnormalDataService
    {
        /// <summary>
        /// 重复参数处理
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        Task<int> RepeatParamAsync(int day);
    }
}
