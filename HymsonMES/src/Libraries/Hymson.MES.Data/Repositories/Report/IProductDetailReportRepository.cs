
using Hymson.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public interface IProductDetailReportRepository
{
    Task<PagedInfo<ProductDetailReportView>> GetPageInfoAsync(ProductDetailReportQuery query);
}

