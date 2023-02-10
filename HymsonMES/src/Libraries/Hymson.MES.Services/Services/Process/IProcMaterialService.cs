/*
 *creator: Karl
 *
 *describe: 物料维护    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */
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
    /// 物料维护 service接口
    /// </summary>
    public interface IProcMaterialService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialDto>> GetPageListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto);

        Task<PagedInfo<ProcMaterialDto>> GetPageListForGroupAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
        Task CreateProcMaterialAsync(ProcMaterialCreateDto procMaterialCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
        Task ModifyProcMaterialAsync(ProcMaterialModifyDto procMaterialModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcMaterialAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcMaterialAsync(string ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaterialViewDto> QueryProcMaterialByIdAsync(long id);
    }
}
