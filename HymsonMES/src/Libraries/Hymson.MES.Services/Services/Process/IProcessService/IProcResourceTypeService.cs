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
    /// 资源类型接口service
    /// </summary>
    public interface IProcResourceTypeService
    {
        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcResourceTypeDto> GetListAsync(long id);

        /// <summary>
        /// 查询资源类型维护表列表
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceTypeViewDto>> GetPageListAsync(ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto);

        /// <summary>
        /// 获取资源类型分页列表(不关联资源、只展示资源类型信息)
        /// </summary>
        /// <param name="procResourceTypePagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcResourceTypeDto>> GetListAsync(ProcResourceTypePagedQueryDto procResourceTypePagedQueryDto);

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddProcResourceTypeAsync(ProcResourceTypeAddCommandDto param);

        /// <summary>
        /// 修改资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> UpdateProcResrouceTypeAsync(ProcResourceTypeUpdateCommandDto param);

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteProcResourceTypeAsync(string ids);
    }
}
