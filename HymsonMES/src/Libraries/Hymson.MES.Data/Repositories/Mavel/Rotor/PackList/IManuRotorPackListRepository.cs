using Hymson.MES.Core.Domain.Mavel.Rotor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Rotor.PackList
{
    /// <summary>
    /// 转子包装仓储
    /// </summary>
    public interface IManuRotorPackListRepository
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuRotorPackListEntity> list);
    }
}
