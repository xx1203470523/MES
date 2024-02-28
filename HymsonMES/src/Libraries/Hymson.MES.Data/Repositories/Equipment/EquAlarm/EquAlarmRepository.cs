using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquAlarm.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment;

/// <summary>
/// 设备报警信息仓储
/// </summary>
public partial class EquAlarmRepository : BaseRepository, IEquAlarmRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionOptions"></param>
    public EquAlarmRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }



    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<EquAlarmEntity> GetOneAsync(EquAlarmQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<EquAlarmEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<EquAlarmEntity>> GetListAsync(EquAlarmQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<EquAlarmEntity>(templateData.RawSql, templateData.Parameters);
    }


    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<EquAlarmEntity>> GetPagedInfoAsync(EquAlarmPagedQuery query)
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

        var equAlarmEntities = await conn.QueryAsync<EquAlarmEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<EquAlarmEntity>(equAlarmEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(EquAlarmEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, entity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public async Task<int> InsertsAsync(IEnumerable<EquAlarmEntity> entities)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertsSql, entities);
    }

    /// <summary>
    /// 根据查询条件获取设备告警报表分页数据
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<EquAlarmReportView>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQuery pageQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoEquAlarmReportDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoEquAlarmReportCountSqlTemplate);

        sqlBuilder.Where(" ea.IsDeleted = 0 ");
        sqlBuilder.Where(" ee.EquipmentCode is not null ");//过滤未关联设备信息
        sqlBuilder.Where(" ea.SiteId = @SiteId ");
        sqlBuilder.OrderBy(" ea.CreatedOn desc");

        if (!string.IsNullOrEmpty(pageQuery.EquipmentCode))
        {
            pageQuery.EquipmentCode = $"%{pageQuery.EquipmentCode}%";
            sqlBuilder.Where(" ee.EquipmentCode like @EquipmentCode ");
        }
        if (!string.IsNullOrEmpty(pageQuery.EquipmentName))
        {
            pageQuery.EquipmentName = $"%{pageQuery.EquipmentName}%";
            sqlBuilder.Where(" ee.EquipmentName like @EquipmentName ");
        }
        if (!string.IsNullOrEmpty(pageQuery.ProcedureName))
        {
            pageQuery.ProcedureName = $"%{pageQuery.ProcedureName}%";
            sqlBuilder.Where(" pp.`Name` like @ProcedureName ");
        }
        if (!string.IsNullOrEmpty(pageQuery.ProcedureCode))
        {
            pageQuery.ProcedureCode = $"%{pageQuery.ProcedureCode}%";
            sqlBuilder.Where(" pp.`Code` like @ProcedureCode ");
        }
        if (pageQuery.ProcedureCodes != null && pageQuery.ProcedureCodes.Length > 0)
        {
            sqlBuilder.Where(" pp.`Code` in @ProcedureCodes ");
        }
        if (!string.IsNullOrEmpty(pageQuery.ResCode))
        {
            pageQuery.ResCode = $"%{pageQuery.ResCode}%";
            sqlBuilder.Where(" pr.ResCode like @ResCode ");
        }
        if (!string.IsNullOrEmpty(pageQuery.ResName))
        {
            pageQuery.ResName = $"%{pageQuery.ResName}%";
            sqlBuilder.Where(" pr.ResName like @ResName ");
        }
        if (pageQuery.Status.HasValue)
        {
            sqlBuilder.Where(" ea.`Status` = @Status ");
        }
        if (pageQuery.TriggerTimes != null && pageQuery.TriggerTimes.Length >= 2)
        {
            sqlBuilder.AddParameters(new { TriggerTimeStart = pageQuery.TriggerTimes[0], TriggerTimeEnd = pageQuery.TriggerTimes[1] });
            sqlBuilder.Where(" ea.LocalTime >= @TriggerTimeStart AND ea.LocalTime < @TriggerTimeEnd ");
        }

        var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
        sqlBuilder.AddParameters(pageQuery);

        using var conn = GetMESDbConnection();
        var reportDataTask = conn.QueryAsync<EquAlarmReportView>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var reportData = await reportDataTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<EquAlarmReportView>(reportData, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
    }

}

/// <summary>
/// 
/// </summary>
public partial class EquAlarmRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `equ_alarm` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `equ_alarm` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `equ_alarm` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `equ_alarm` /**where**/;";

    #endregion

    const string InsertSql = "INSERT INTO `equ_alarm`(  `Id`, `SiteId`, `EquipmentId`, `FaultCode`, `AlarmMsg`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @FaultCode, @AlarmMsg, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string InsertsSql = "INSERT INTO `equ_alarm`(  `Id`, `SiteId`, `EquipmentId`, `FaultCode`, `AlarmMsg`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @FaultCode, @AlarmMsg, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    const string GetPagedInfoEquAlarmReportDataSqlTemplate = @" select  ea.EquipmentId,ee.EquipmentCode,ee.EquipmentName,ea.FaultCode,ea.AlarmMsg,ea.`Status`,ea.`LocalTime`
										,ee.WorkCenterLineId,pr.ResCode,pr.ResName,pp.`Code` as ProcedureCode,pp.`Name` as ProcedureName,
										ea.UpdatedOn,ea.UpdatedBy,ea.CreatedOn,ea.CreatedBy 
										FROM equ_alarm  ea 
										left join equ_equipment ee on ee.Id=ea.EquipmentId and ee.IsDeleted=ea.IsDeleted
										left join proc_resource_equipment_bind preb on preb.EquipmentId=ea.EquipmentId  and preb.IsDeleted=ea.IsDeleted
										left join proc_resource  pr on pr.Id = preb.ResourceId and pr.SiteId= preb.SiteId and pr.IsDeleted=ea.IsDeleted
								        left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=ea.IsDeleted
										/**where**/  /**orderby**/ LIMIT @Offset,@Rows ";

    const string GetPagedInfoEquAlarmReportCountSqlTemplate = @"select COUNT(1) 
                                        FROM equ_alarm  ea 
										left join equ_equipment ee on ee.Id=ea.EquipmentId and ee.IsDeleted=ea.IsDeleted
										left join proc_resource_equipment_bind preb on preb.EquipmentId=ea.EquipmentId  and preb.IsDeleted=ea.IsDeleted
										left join proc_resource  pr on pr.Id = preb.ResourceId and pr.SiteId= preb.SiteId and pr.IsDeleted=ea.IsDeleted
								        left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=ea.IsDeleted
										/**where**/  ";

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：设备报警信息;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-28</para>
/// </summary>
public partial class EquAlarmRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, EquAlarmPagedQuery query)
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


        if (!string.IsNullOrWhiteSpace(query.FaultCode))
        {
            sqlBuilder.Where("FaultCode = @FaultCode");
        }

        if (!string.IsNullOrWhiteSpace(query.FaultCodeLike))
        {
            query.FaultCodeLike = $"{query.FaultCodeLike}%";
            sqlBuilder.Where("FaultCode Like @FaultCodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.AlarmMsg))
        {
            sqlBuilder.Where("AlarmMsg = @AlarmMsg");
        }

        if (!string.IsNullOrWhiteSpace(query.AlarmMsgLike))
        {
            query.AlarmMsgLike = $"{query.AlarmMsgLike}%";
            sqlBuilder.Where("AlarmMsg Like @AlarmMsgLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }

        if (query.Statuss != null && query.Statuss.Any())
        {
            sqlBuilder.Where("Status IN @Statuss");
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
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, EquAlarmQuery query)
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


        if (!string.IsNullOrWhiteSpace(query.FaultCode))
        {
            sqlBuilder.Where("FaultCode = @FaultCode");
        }

        if (!string.IsNullOrWhiteSpace(query.FaultCodeLike))
        {
            query.FaultCodeLike = $"{query.FaultCodeLike}%";
            sqlBuilder.Where("FaultCode Like @FaultCodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.AlarmMsg))
        {
            sqlBuilder.Where("AlarmMsg = @AlarmMsg");
        }

        if (!string.IsNullOrWhiteSpace(query.AlarmMsgLike))
        {
            query.AlarmMsgLike = $"{query.AlarmMsgLike}%";
            sqlBuilder.Where("AlarmMsg Like @AlarmMsgLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }

        if (query.Statuss != null && query.Statuss.Any())
        {
            sqlBuilder.Where("Status IN @Statuss");
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
