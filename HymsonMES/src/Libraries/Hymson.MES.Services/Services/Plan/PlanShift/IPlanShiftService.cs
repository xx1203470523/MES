using Hymson.Infrastructure;
using Hymson.MES.Core.Enums.Plan;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 服务接口（班制）
    /// </summary>
    public interface IPlanShiftService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(PlanShiftSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task ModifyAsync(PlanShiftSaveDto saveDto, InteShiftModifyTypeEnum type);

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
        Task<PlanShiftDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanShiftDto>> GetPagedListAsync(PlanShiftPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PlanShiftDto>> GetAllAsync();


        Task<IEnumerable<PlanShiftDetailDto>> GetByMainIdAsync(long mainId);
    }
}