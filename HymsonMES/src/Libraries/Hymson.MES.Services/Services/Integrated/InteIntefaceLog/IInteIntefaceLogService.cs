using Hymson.Infrastructure;
using Hymson.Logging;
using Hymson.MES.Services.Dtos.Integrated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Integrated.InteIntefaceLog
{
    public interface IInteIntefaceLogService
    {
        Task<PagedInfo<TraceLogEntry>> GetPagedListAsync(InteIntefaceLogPagedQueryDto pagedQueryDto);

        //Task<InteIntefaceLogDto?> QueryByIdAsync(long id);


    }
}
