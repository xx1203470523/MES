using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.IProcessService
{
    /// <summary>
    /// 资源接口service
    /// </summary>
    public interface IProcResourceService
    {
        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceViewDto> GetByIdAsync(long id);

        /// <summary>
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceViewDto>> GetPageListAsync(ProcResourcePagedQueryDto query);

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceDto>> GetListAsync(ProcResourcePagedQueryDto query);

        /// <summary>
        /// 查询资源类型下关联的资源(资源类型详情：可用资源，已分配资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceDto>> GetListForGroupAsync(ProcResourcePagedQueryDto query);

        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddProcResourceAsync(ProcResourceDto param);

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateProcResrouceAsync(ProcResourceDto param);

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteProcResourceAsync(string ids);
    }
}
