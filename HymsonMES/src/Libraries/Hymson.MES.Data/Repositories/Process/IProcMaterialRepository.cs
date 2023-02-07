using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public interface IProcMaterialRepository
    {
        Task InsertAsync(ProcMaterialEntity procMaterialEntity);

        Task<int> UpdateAsync(ProcMaterialEntity procMaterialEntity);
        Task<int> DeleteAsync(long id);
        Task<ProcMaterialEntity> GetByIdAsync(long id);
        Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntitiesAsync(ProcMaterialQuery procMaterialQuery);

        Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoAsync(ProcMaterialPagedQuery procMaterialPagedQuery);
    }
}
