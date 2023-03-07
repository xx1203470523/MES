/*
 *creator: Karl
 *
 *describe: 物料组维护表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
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
    /// 物料组维护表 service接口
    /// </summary>
    public interface IProcMaterialGroupService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="procMaterialGroupPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcMaterialGroupDto>> GetPageListAsync(ProcMaterialGroupPagedQueryDto procMaterialGroupPagedQueryDto);

        /// <summary>
        /// 获取分页自定义List
        /// </summary>
        /// <param name="customProcMaterialGroupPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<CustomProcMaterialGroupViewDto>> GetPageCustomListAsync(CustomProcMaterialGroupPagedQueryDto customProcMaterialGroupPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialGroupDto"></param>
        /// <returns></returns>
        Task CreateProcMaterialGroupAsync(ProcMaterialGroupCreateDto procMaterialGroupCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialGroupDto"></param>
        /// <returns></returns>
        Task ModifyProcMaterialGroupAsync(ProcMaterialGroupModifyDto procMaterialGroupModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteProcMaterialGroupAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesProcMaterialGroupAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcMaterialGroupDto> QueryProcMaterialGroupByIdAsync(long id);
    }
}
