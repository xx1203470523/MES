using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.OnStock
{
    public interface IWhStockChangeRecordRepository
    {
        Task InsertAsync(WhStockChangeRecordEntity whStockChangeRecordEntity);

        Task<int> UpdateAsync(WhStockChangeRecordEntity whStockChangeRecordEntity);
        Task<int> DeleteAsync(long id);
        Task<WhStockChangeRecordEntity> GetByIdAsync(long id);
        Task<IEnumerable<WhStockChangeRecordEntity>> GetWhStockChangeRecordEntitiesAsync(WhStockChangeRecordQuery whStockChangeRecordQuery);


        Task<PagedInfo<WhStockChangeRecordEntity>> GetPagedInfoAsync(WhStockChangeRecordPagedQuery  whStockChangeRecordPagedQuery);
    }
}
