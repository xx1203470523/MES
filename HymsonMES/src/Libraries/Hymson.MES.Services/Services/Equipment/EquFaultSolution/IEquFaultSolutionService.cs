using Hymson.Infrastructure;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务接口（设备故障解决措施）
    /// </summary>
    public interface IEquFaultSolutionService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(EquFaultSolutionSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(EquFaultSolutionSaveDto saveDto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquFaultSolutionDto>> GetPagedListAsync(EquFaultSolutionPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquFaultSolutionDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取解决措施（可被引用）
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EquFaultSolutionBaseDto>> QuerySolutionsAsync();

        /// <summary>
        /// 根据ID获取关联解决措施
        /// </summary>
        /// <param name="reasonId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquFaultSolutionBaseDto>> QuerySolutionsByMainIdAsync(long reasonId);


        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        Task UpdateStatusAsync(ChangeStatusDto statusDto);

    }
}