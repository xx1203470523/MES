using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment;

/// <summary>
/// 设备状态仓储
/// </summary>
public partial class EquStatusRepository : BaseRepository, IEquStatusRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionOptions"></param>
    public EquStatusRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<EquStatusEntity> GetOneAsync(EquStatusQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<EquStatusEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquStatusEntity>> GetListAsync(EquStatusQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<EquStatusEntity>(templateData.RawSql, templateData.Parameters);
    }
    #endregion

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(EquStatusEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, entity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public async Task<int> InsertsAsync(IEnumerable<EquStatusEntity> entities)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertsSql, entities);
    }

    /// <summary>
    /// 新增（统计）
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> InsertStatisticsAsync(EquStatusStatisticsEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertStatisticsSql, entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(EquStatusEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, entity);
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public async Task<int> UpdatesAsync(IEnumerable<EquStatusEntity> entities)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdatesSql, entities);
    }

    /// <summary>
    /// 根据设备ID获取最新的状态记录
    /// </summary>
    /// <param name="equipmentId"></param>
    /// <returns></returns>
    public async Task<EquStatusEntity> GetLastEntityByEquipmentIdAsync(long equipmentId)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<EquStatusEntity>(GetLastEntityByEquipmentIdSql, new { equipmentId });
    }


    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="equStatusQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquStatusStatisticsEntity>> GetEquStatusStatisticsEntitiesAsync(EquStatusStatisticsQuery equStatusQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetEquStatusStatisticsEntitiesSqlTemplate);
        sqlBuilder.Where("IsDeleted = 0");
        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Select("*");
        if (equStatusQuery.EquipmentIds != null && equStatusQuery.EquipmentIds.Length > 0)
        {
            sqlBuilder.Where("EquipmentId IN @EquipmentIds");
        }
        if (equStatusQuery.StartTime.HasValue)
        {
            sqlBuilder.Where(" CreatedOn >= @StartTime");
        }
        if (equStatusQuery.EndTime.HasValue)
        {
            sqlBuilder.Where(" CreatedOn < @EndTime");
        }
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<EquStatusStatisticsEntity>(template.RawSql, equStatusQuery);
    }

    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="equTheoryQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquEquipmentTheoryEntity>> GetEquipmentTheoryAsync(EquEquipmentTheoryQuery equTheoryQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetEquipmentTheoryEntitiesSqlTemplate);
        sqlBuilder.Where("IsDeleted = 0");
        //sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Select("*");
        if (equTheoryQuery.EquipmentCodes != null && equTheoryQuery.EquipmentCodes.Length > 0)
        {
            sqlBuilder.Where("EquipmentCode IN @EquipmentCodes");
        }
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<EquEquipmentTheoryEntity>(template.RawSql, equTheoryQuery);
    }
}

/// <summary>
/// 
/// </summary>
public partial class EquStatusRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `equ_status` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `equ_status` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `equ_status` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `equ_status` /**where**/;";

    #endregion

    const string GetEquStatusStatisticsEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_status_statistics` /**where**/  ";
    const string InsertSql = "REPLACE INTO `equ_status`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `LossRemark`, `BeginTime`, `EndTime`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @LossRemark, @BeginTime, @EndTime, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string InsertsSql = "INSERT INTO `equ_status`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `LossRemark`, `BeginTime`, `EndTime`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @LossRemark, @BeginTime, @EndTime, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string InsertStatisticsSql = "INSERT INTO `equ_status_statistics`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `SwitchEquipmentStatus`, `BeginTime`, `EndTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @SwitchEquipmentStatus, @BeginTime, @EndTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    const string UpdateSql = "UPDATE `equ_status` SET SiteId = @SiteId, EquipmentId = @EquipmentId, EquipmentStatus = @EquipmentStatus, LossRemark = @LossRemark, BeginTime = @BeginTime, EndTime = @EndTime, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
    const string UpdatesSql = "UPDATE `equ_status` SET SiteId = @SiteId, EquipmentId = @EquipmentId, EquipmentStatus = @EquipmentStatus, LossRemark = @LossRemark, BeginTime = @BeginTime, EndTime = @EndTime, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

    const string GetLastEntityByEquipmentIdSql = "SELECT * FROM equ_status WHERE EquipmentId = @equipmentId ORDER BY LocalTime DESC LIMIT 1";
    
    const string GetEquipmentTheoryEntitiesSqlTemplate = @"SELECT /**select**/ FROM `equ_equipment_theory` /**where**/  ";
}


/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：设备状态;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-3-5</para>
/// </summary>
public partial class EquStatusRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, EquStatusPagedQuery query)
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


        if (query.EquipmentId.HasValue)
        {
            sqlBuilder.Where("EquipmentId = @EquipmentId");
        }

        if (query.EquipmentIds != null && query.EquipmentIds.Any())
        {
            sqlBuilder.Where("EquipmentId IN @EquipmentIds");
        }


        if (query.EquipmentStatus.HasValue)
        {
            sqlBuilder.Where("EquipmentStatus = @EquipmentStatus");
        }

        if (query.EquipmentStatuss != null && query.EquipmentStatuss.Any())
        {
            sqlBuilder.Where("EquipmentStatus IN @EquipmentStatuss");
        }


        if (!string.IsNullOrWhiteSpace(query.LossRemark))
        {
            sqlBuilder.Where("LossRemark = @LossRemark");
        }

        if (!string.IsNullOrWhiteSpace(query.LossRemarkLike))
        {
            query.LossRemarkLike = $"{query.LossRemarkLike}%";
            sqlBuilder.Where("LossRemark Like @LossRemarkLike");
        }


        if (query.BeginTimeStart.HasValue)
        {
            sqlBuilder.Where("BeginTime >= @BeginTimeStart");
        }

        if (query.BeginTimeEnd.HasValue)
        {
            sqlBuilder.Where("BeginTime <= @BeginTimeEnd");
        }


        if (query.EndTimeStart.HasValue)
        {
            sqlBuilder.Where("EndTime >= @EndTimeStart");
        }

        if (query.EndTimeEnd.HasValue)
        {
            sqlBuilder.Where("EndTime <= @EndTimeEnd");
        }


        if (query.LocalTimeStart.HasValue)
        {
            sqlBuilder.Where("LocalTime >= @LocalTimeStart");
        }

        if (query.LocalTimeEnd.HasValue)
        {
            sqlBuilder.Where("LocalTime <= @LocalTimeEnd");
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
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, EquStatusQuery query)
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


        if (query.EquipmentId.HasValue)
        {
            sqlBuilder.Where("EquipmentId = @EquipmentId");
        }

        if (query.EquipmentIds != null && query.EquipmentIds.Any())
        {
            sqlBuilder.Where("EquipmentId IN @EquipmentIds");
        }


        if (query.EquipmentStatus.HasValue)
        {
            sqlBuilder.Where("EquipmentStatus = @EquipmentStatus");
        }

        if (query.EquipmentStatuss != null && query.EquipmentStatuss.Any())
        {
            sqlBuilder.Where("EquipmentStatus IN @EquipmentStatuss");
        }


        if (!string.IsNullOrWhiteSpace(query.LossRemark))
        {
            sqlBuilder.Where("LossRemark = @LossRemark");
        }

        if (!string.IsNullOrWhiteSpace(query.LossRemarkLike))
        {
            query.LossRemarkLike = $"{query.LossRemarkLike}%";
            sqlBuilder.Where("LossRemark Like @LossRemarkLike");
        }


        if (query.BeginTimeStart.HasValue)
        {
            sqlBuilder.Where("BeginTime >= @BeginTimeStart");
        }

        if (query.BeginTimeEnd.HasValue)
        {
            sqlBuilder.Where("BeginTime <= @BeginTimeEnd");
        }


        if (query.EndTimeStart.HasValue)
        {
            sqlBuilder.Where("EndTime >= @EndTimeStart");
        }

        if (query.EndTimeEnd.HasValue)
        {
            sqlBuilder.Where("EndTime <= @EndTimeEnd");
        }


        if (query.LocalTimeStart.HasValue)
        {
            sqlBuilder.Where("LocalTime >= @LocalTimeStart");
        }

        if (query.LocalTimeEnd.HasValue)
        {
            sqlBuilder.Where("LocalTime <= @LocalTimeEnd");
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