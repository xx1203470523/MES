
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Plan;

/// <summary>
/// <para>@描述：生产日历详情; 服务接口</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// </summary>
public interface IPlanCalendarDetailService
{
    /// <summary>
    /// <para>@描述：生产日历详情; 根据条件判断数据是否存在</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<bool> IsExistAsync(PlanCalendarDetailQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历详情; 根据条件获取一组数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<IEnumerable<PlanCalendarDetailOutputDto>> GetListAsync(PlanCalendarDetailQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历详情; 根据条件获取一行数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<PlanCalendarDetailOutputDto> GetOneAsync(PlanCalendarDetailQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历详情; 分页查询数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<PagedInfo<PlanCalendarDetailOutputDto>> GetPagedAsync(PlanCalendarDetailPagedQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历详情; 创建数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="createDto">创建数据输入对象</param>
    /// <returns></returns>
    Task CreateAsync(PlanCalendarDetailDto createDto);

    /// <summary>
    /// <para>@描述：生产日历详情; 更新数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="updateDto">更新数据输入对象</param>
    /// <returns></returns>
    Task UpdateAsync(PlanCalendarDetailUpdateDto updateDto);

    /// <summary>
    /// <para>@描述：生产日历详情; 删除数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="deleteDto">删除数据输入对象</param>
    /// <returns></returns>
    Task DeleteAsync(PlanCalendarDetailDeleteDto deleteDto);
}