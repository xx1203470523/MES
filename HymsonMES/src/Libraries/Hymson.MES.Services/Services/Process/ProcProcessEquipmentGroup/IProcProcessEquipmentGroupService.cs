using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务接口（工艺设备组）
    /// </summary>
    public interface IProcProcessEquipmentGroupService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateProcProcessEquipmentGroupAsync(ProcProcessEquipmentGroupSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyProcProcessEquipmentGroupAsync(ProcProcessEquipmentGroupSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteProcProcessEquipmentGroupAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcProcessEquipmentGroupAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessEquipmentGroupDto?> QueryProcProcessEquipmentGroupByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcessEquipmentGroupListDto>> GetPagedListAsync(ProcProcessEquipmentGroupPagedQueryDto pagedQueryDto);

    }
}