using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.OnStock
{
    public interface IWhStockChangeRecordService
    {
        Task<PagedInfo<WhStockChangeRecordDto>> GetListAsync(WhStockChangeRecordPagedQueryDto whStockChangeRecordPagedQueryDto);

        Task CreateWhStockChangeRecordAsync(WhStockChangeRecordDto whStockChangeRecordDto);

        Task ModifyWhStockChangeRecordAsync(WhStockChangeRecordDto whStockChangeRecordDto);

        Task DeleteWhStockChangeRecordAsync(long id);
    }
}
