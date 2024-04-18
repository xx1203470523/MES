using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Plan;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础实现</para>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial class PlanCalendarRepository : BaseRepository, IPlanCalendarRepository
{
    public PlanCalendarRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {

    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PlanCalendarEntity> GetOneAsync(PlanCalendarQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<PlanCalendarEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PlanCalendarEntity>> GetListAsync(PlanCalendarQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<PlanCalendarEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<PlanCalendarEntity>> GetPagedInfoAsync(PlanCalendarPagedQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetPagedSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetCountSqlTemplate);

        WhereFill(sqlBuilder, query);

        if (!string.IsNullOrWhiteSpace(query.Sorting))
        {
            sqlBuilder.OrderBy(query.Sorting);
        }

        var offSet = (query.PageIndex - 1) * query.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = query.PageSize });
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        var planCalendarEntities = await conn.QueryAsync<PlanCalendarEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<PlanCalendarEntity>(planCalendarEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(PlanCalendarCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<PlanCalendarCreateCommand> commands)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, commands);
    }

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(PlanCalendarUpdateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<PlanCalendarUpdateCommand> commands)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateByIdSql, commands);
    }

    #endregion

    #region 删除

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByIdSql, command);
    }

    /// <summary>
    /// 根据ID删除多条数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteMoreAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteMoreByIdSql, command);
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础SQL语句</para>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial class PlanCalendarRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `plan_calendar` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `plan_calendar` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `plan_calendar` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `plan_calendar` /**where**/;";

    #endregion

    #region 新增

    const string InsertSql = "INSERT INTO `plan_calendar` (`Id`,`SiteId`,`ShiftId`,`Year`,`Month`,`Workday`,`Status`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`,`Remark`) VALUES (@Id,@SiteId,@ShiftId,@Year,@Month,@Workday,@Status,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@Remark);";

    #endregion

    #region 修改

    const string UpdateByIdSql = "UPDATE `plan_calendar` SET `ShiftId` = @ShiftId ,`Year` = @Year ,`Month` = @Month ,`Workday` = @Workday ,`Status` = @Status ,`Remark` = @Remark  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion

    #region 删除

    const string DeleteByIdSql = "UPDATE `plan_calendar` SET IsDeleted = Id WHERE Id = @Id;";

    const string DeleteMoreByIdSql = "UPDATE `plan_calendar` SET IsDeleted = Id WHERE Id IN @Ids;";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial class PlanCalendarRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, PlanCalendarPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        if (query.ShiftId.HasValue)
        {
            sqlBuilder.Where("ShiftId = @ShiftId");
        }

        if (query.ShiftIds != null && query.ShiftIds.Any())
        {
            sqlBuilder.Where("ShiftId IN @ShiftIds");
        }


        if (query.Year.HasValue)
        {
            sqlBuilder.Where("Year = @Year");
        }

        if (query.Years != null && query.Years.Any())
        {
            sqlBuilder.Where("Year IN @Years");
        }


        if (query.Month.HasValue)
        {
            sqlBuilder.Where("Month = @Month");
        }

        if (query.Months != null && query.Months.Any())
        {
            sqlBuilder.Where("Month IN @Months");
        }


        if (!string.IsNullOrWhiteSpace(query.Workday))
        {
            sqlBuilder.Where("Workday = @Workday");
        }

        if (!string.IsNullOrWhiteSpace(query.WorkdayLike))
        {
            query.WorkdayLike = $"{query.WorkdayLike}%";
            sqlBuilder.Where("Workday Like @WorkdayLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }


        if (!string.IsNullOrWhiteSpace(query.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy = @CreatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedByLike))
        {
            query.CreatedByLike = $"{query.CreatedByLike}%";
            sqlBuilder.Where("CreatedBy Like @CreatedByLike");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
        }


        if (!string.IsNullOrWhiteSpace(query.UpdatedBy))
        {
            sqlBuilder.Where("UpdatedBy = @UpdatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedByLike))
        {
            query.UpdatedByLike = $"{query.UpdatedByLike}%";
            sqlBuilder.Where("UpdatedBy Like @UpdatedByLike");
        }


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, PlanCalendarQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        if (query.ShiftId.HasValue)
        {
            sqlBuilder.Where("ShiftId = @ShiftId");
        }

        if (query.ShiftIds != null && query.ShiftIds.Any())
        {
            sqlBuilder.Where("ShiftId IN @ShiftIds");
        }


        if (query.Year.HasValue)
        {
            sqlBuilder.Where("Year = @Year");
        }

        if (query.Years != null && query.Years.Any())
        {
            sqlBuilder.Where("Year IN @Years");
        }


        if (query.Month.HasValue)
        {
            sqlBuilder.Where("Month = @Month");
        }

        if (query.Months != null && query.Months.Any())
        {
            sqlBuilder.Where("Month IN @Months");
        }


        if (!string.IsNullOrWhiteSpace(query.Workday))
        {
            sqlBuilder.Where("Workday = @Workday");
        }

        if (!string.IsNullOrWhiteSpace(query.WorkdayLike))
        {
            query.WorkdayLike = $"{query.WorkdayLike}%";
            sqlBuilder.Where("Workday Like @WorkdayLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }


        if (!string.IsNullOrWhiteSpace(query.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy = @CreatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedByLike))
        {
            query.CreatedByLike = $"{query.CreatedByLike}%";
            sqlBuilder.Where("CreatedBy Like @CreatedByLike");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
        }


        if (!string.IsNullOrWhiteSpace(query.UpdatedBy))
        {
            sqlBuilder.Where("UpdatedBy = @UpdatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedByLike))
        {
            query.UpdatedByLike = $"{query.UpdatedByLike}%";
            sqlBuilder.Where("UpdatedBy Like @UpdatedByLike");
        }


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial class PlanCalendarRepository
{
    #region 新增

    /// <summary>
    /// 创建数据 - 忽略
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertIgnoreAsync(PlanCalendarCreateCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertIgnoreSql, command);
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：生产日历;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial class PlanCalendarRepository
{
    #region 新增
#if DM
    const string InsertIgnoreSql = "INSERT  INTO `plan_calendar` (`Id`,`SiteId`,`ShiftId`,`Year`,`Month`,`Workday`,`Status`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`,`Remark`) VALUES (@Id,@SiteId,@ShiftId,@Year,@Month,@Workday,@Status,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@Remark);";
#else
    const string InsertIgnoreSql = "INSERT IGNORE INTO `plan_calendar` (`Id`,`SiteId`,`ShiftId`,`Year`,`Month`,`Workday`,`Status`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`,`Remark`) VALUES (@Id,@SiteId,@ShiftId,@Year,@Month,@Workday,@Status,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@Remark);";

#endif
    #endregion
}