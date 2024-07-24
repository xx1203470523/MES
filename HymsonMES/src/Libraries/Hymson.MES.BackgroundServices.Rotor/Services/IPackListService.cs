using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 装箱列表
    /// </summary>
    public interface IPackListService
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        Task<int> ExecAsync(int rows);
    }
}
