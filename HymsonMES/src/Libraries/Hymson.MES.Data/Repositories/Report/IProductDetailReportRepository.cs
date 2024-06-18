
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public interface IProductDetailReportRepository
{
    Task<PagedInfo<ProductDetailReportView>> GetPageInfoAsync(ProductDetailReportPageQuery query);

    Task<decimal> GetOutputSumAsyc(ProductDetailReportQuery query);
    
    /// <summary>
    /// 获取所有工序
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ProcProcedureEntity>> GetProcdureInfoAsync();
}

