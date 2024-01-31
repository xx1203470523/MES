using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report;

public interface IPackBindOtherReportService
{
    Task<PagedInfo<PackBindOtherReportViewDto>> GetPagedInfoAsync(PackBindOtherPageQueryPagedDto query);

}
