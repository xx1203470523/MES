using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcMaterialService
    {
        Task<PagedInfo<ProcMaterialDto>> GetListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto);

        Task CreateProcMaterialAsync(ProcMaterialDto procMaterialDto);

        Task ModifyProcMaterialAsync(ProcMaterialDto procMaterialDto);

        Task DeleteProcMaterialAsync(long id);
    }
}
