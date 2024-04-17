using Dapper;


namespace Hymson.MES.Data.Repositories.Plan;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：生产日历详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-30</para>
/// </summary>
public partial class PlanCalendarDetailRepository
{
    #region 删除

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="planCalendarId"></param>
    /// <returns></returns>
    public async Task<int> DeleteByPlanCalendarIdAsync(long planCalendarId)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByPlanCalendarIdSql, new { PlanCalendarId = planCalendarId });
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：生产日历详情;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-30</para>
/// </summary>
public partial class PlanCalendarDetailRepository
{
    #region 删除

    const string DeleteByPlanCalendarIdSql = "UPDATE `plan_calendar_detail` SET IsDeleted = Id WHERE PlanCalendarId = @PlanCalendarId;";

    #endregion
}