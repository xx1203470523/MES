using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（工具绑定设备操作记录表）
    /// </summary>
    public interface IEquToolsEquipmentBindRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> InstallAsync(EquToolsEquipmentBindRecordCreateDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> UninstallAsync(EquToolsEquipmentBindRecordSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquToolsEquipmentBindRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquToolsEquipmentBindRecordViewDto>> GetPagedListAsync(EquToolsEquipmentBindRecordPagedQueryDto pagedQueryDto);

    }
}