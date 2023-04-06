using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 设备故障原因表 service接口
    /// </summary>
    public interface IEquFaultReasonService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="EquFaultReasonPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultReasonDto>> GetPageListAsync(EquFaultReasonPagedQueryDto EquFaultReasonPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquFaultReasonCreateDto"></param>
        /// <returns></returns>
        Task<int> CreateEquFaultReasonAsync(EquFaultReasonSaveDto EquFaultReasonCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquFaultReasonModifyDto"></param>
        /// <returns></returns>
        Task ModifyEquFaultReasonAsync(EquFaultReasonSaveDto EquFaultReasonModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEquFaultReasonAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        Task<int> DeletesEquFaultReasonAsync(long[] idsArr);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultReasonDto> QueryEquFaultReasonByIdAsync(long id);
    }
}
