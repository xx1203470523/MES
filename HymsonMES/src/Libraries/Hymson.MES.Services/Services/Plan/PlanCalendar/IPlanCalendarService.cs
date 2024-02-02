
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Plan;

namespace Hymson.MES.Services.Plan;

/// <summary>
/// <para>@描述：生产日历; 服务接口</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-15</para>
/// </summary>
public interface IPlanCalendarService
{
    /// <summary>
    /// <para>@描述：生产日历; 根据条件判断数据是否存在</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<bool> IsExistAsync(PlanCalendarQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历; 根据条件获取一组数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<IEnumerable<PlanCalendarOutputDto>> GetListAsync(PlanCalendarQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历; 根据条件获取一行数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<PlanCalendarOutputDto> GetOneAsync(PlanCalendarQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历; 分页查询数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="queryDto">查询条件</param>
    /// <returns></returns>
    Task<PagedInfo<PlanCalendarOutputDto>> GetPagedAsync(PlanCalendarPagedQueryDto queryDto);

    /// <summary>
    /// <para>@描述：生产日历; 创建数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="createDto">创建数据输入对象</param>
    /// <returns></returns>
    Task CreateAsync(PlanCalendarDto createDto);

    /// <summary>
    /// <para>@描述：生产日历; 更新数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="updateDto">更新数据输入对象</param>
    /// <returns></returns>
    Task UpdateAsync(PlanCalendarUpdateDto updateDto);

    /// <summary>
    /// <para>@描述：生产日历; 删除数据</para>
    /// <para>@作者：Jim</para>
    /// <para>@创建时间：2024-1-15</para>
    /// </summary>
    /// <param name="deleteDto">删除数据输入对象</param>
    /// <returns></returns>
    Task DeleteAsync(PlanCalendarDeleteDto deleteDto);
}