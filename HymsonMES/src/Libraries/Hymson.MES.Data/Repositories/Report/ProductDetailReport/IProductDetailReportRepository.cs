using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;

namespace Hymson.MES.Data.Repositories.Report;

public interface IProductDetailReportRepository
{
    Task<PagedInfo<ProductDetailReportView>> GetPageInfoAsync(ProductDetailReportPageQuery query);

    Task<decimal> GetOutputSumAsyc(ProductDetailReportQuery query);

    Task<IEnumerable<ProcProcedureEntity>> GetProcdureInfoAsync();
}

