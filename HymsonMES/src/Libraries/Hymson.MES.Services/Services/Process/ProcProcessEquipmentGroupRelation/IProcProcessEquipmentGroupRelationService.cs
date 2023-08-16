using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务接口（设备组关联设备表）
    /// </summary>
    public interface IProcProcessEquipmentGroupRelationService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateProcProcessEquipmentGroupRelationAsync(ProcProcessEquipmentGroupRelationSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyProcProcessEquipmentGroupRelationAsync(ProcProcessEquipmentGroupRelationSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteProcProcessEquipmentGroupRelationAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesProcProcessEquipmentGroupRelationAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcProcessEquipmentGroupRelationDto?> QueryProcProcessEquipmentGroupRelationByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcProcessEquipmentGroupRelationDto>> GetPagedListAsync(ProcProcessEquipmentGroupRelationPagedQueryDto pagedQueryDto);

    }
}