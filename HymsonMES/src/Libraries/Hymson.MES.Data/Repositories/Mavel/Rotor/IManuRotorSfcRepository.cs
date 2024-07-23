using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Data.Repositories.Common.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Rotor
{
    /// <summary>
    /// 转子线条码
    /// </summary>
    public interface IManuRotorSfcRepository
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuRotorSfcEntity> list);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="eneity"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuRotorSfcEntity> list);

        /// <summary>
        /// 水位查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuRotorSfcEntity>> GetListAsync(EntityByWaterMarkTimeQuery query);
    }
}
