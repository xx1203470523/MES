using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

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
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<CustomProcMaterialGroupViewDto>> GetPageCustomListAsync(CustomProcMaterialGroupPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialGroupCreateDto"></param>
        /// <returns></returns>
        Task<long> CreateProcMaterialGroupAsync(ProcMaterialGroupCreateDto procMaterialGroupCreateDto);

        /// <summary>   
        /// 修改
        /// </summary>
        /// <param name="procMaterialGroupModifyDto"></param>
        /// <returns></returns>
        Task ModifyProcMaterialGroupAsync(ProcMaterialGroupModifyDto procMaterialGroupModifyDto);

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
