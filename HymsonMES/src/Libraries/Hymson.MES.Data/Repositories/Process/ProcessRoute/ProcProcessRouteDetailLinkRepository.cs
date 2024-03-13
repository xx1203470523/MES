using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process;

/// <summary>
/// 工艺路线工序节点关系明细表(前节点多条就存多条)仓储
/// </summary>
public partial class ProcProcessRouteDetailLinkRepository : IProcProcessRouteDetailLinkRepository
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ConnectionOptions _connectionOptions;
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionOptions"></param>
    public ProcProcessRouteDetailLinkRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache)
    {
        _connectionOptions = connectionOptions.Value;
        _memoryCache = memoryCache;
    }


    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ProcProcessRouteDetailLinkEntity> GetOneAsync(ProcProcessRouteDetailLinkQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailLinkEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetListAsync(ProcProcessRouteDetailLinkQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.Where("IsDeleted=0");
        sqlBuilder.Select("*");

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        return await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(templateData.RawSql, templateData.Parameters);
    }

    #endregion



    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProcProcessRouteDetailLinkEntity> GetByIdAsync(long id)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailLinkEntity>(GetByIdSql, new { Id = id });
    }

    /// <summary>
    /// 获某工艺路线下面的连线
    /// </summary>
    /// <param name="processRouteId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetProcessRouteDetailLinksByProcessRouteIdAsync(long processRouteId)
    {
        var key = $"proc_process_route_detail_link&{processRouteId}";
        return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(GetByProcessRouteIdSql, new { ProcessRouteId = processRouteId });
        });
    }

    /// <summary>
    /// 获某工序对应的前一工序
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetPreProcessRouteDetailLinkAsync(ProcProcessRouteDetailLinkQuery query)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(GetPreProcedureIDsSql, query);
    }

    /// <summary>
    /// 获某工序对应的下一工序
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetNextProcessRouteDetailLinkAsync(ProcProcessRouteDetailLinkQuery query)
    {
        //工艺路线变更后还按原设置生产，去掉缓存排查
        //var key = $"proc_process_route_detail_link&{query.ProcessRouteId}&{query.ProcedureId}";
        //return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
        //{
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(GetNextProcedureIDsSql, query);
        //});
    }

    /// <summary>
    /// 根据IDs批量获取数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetByIdsAsync(long[] ids)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(GetByIdsSql, new { ids = ids });
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProcProcessRouteDetailLinkEntity>> GetPagedInfoAsync(ProcProcessRouteDetailLinkPagedQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
        sqlBuilder.Where("IsDeleted = 0");
        sqlBuilder.OrderBy("UpdatedOn DESC");
        sqlBuilder.Select("*");

        sqlBuilder.Where("SiteId = @SiteId");

        var offSet = (query.PageIndex - 1) * query.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = query.PageSize });
        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        var procProcessRouteDetailLinkEntitiesTask = conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var procProcessRouteDetailLinkEntities = await procProcessRouteDetailLinkEntitiesTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<ProcProcessRouteDetailLinkEntity>(procProcessRouteDetailLinkEntities, query.PageIndex, query.PageSize, totalCount);
    }

    ///// <summary>
    ///// 查询List
    ///// </summary>
    ///// <param name="query"></param>
    ///// <returns></returns>
    //public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetListAsync(ProcProcessRouteDetailLinkQuery query)
    //{
    //    var sqlBuilder = new SqlBuilder();
    //    var template = sqlBuilder.AddTemplate(GetListSqlTemplate);
    //    sqlBuilder.Where("IsDeleted=0");
    //    sqlBuilder.Select("*");
    //    sqlBuilder.Where("ProcessRouteId=@ProcessRouteId");
    //    sqlBuilder.AddParameters(query);

    //    using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
    //    var procProcessRouteDetailLinkEntities = await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(template.RawSql, template.Parameters);
    //    return procProcessRouteDetailLinkEntities;
    //}

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="procProcessRouteDetailLinkEntity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(ProcProcessRouteDetailLinkEntity procProcessRouteDetailLinkEntity)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertSql, procProcessRouteDetailLinkEntity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="procProcessRouteDetailLinkEntitys"></param>
    /// <returns></returns>
    public async Task<int> InsertRangeAsync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntitys)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertSql, procProcessRouteDetailLinkEntitys);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="procProcessRouteDetailLinkEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ProcProcessRouteDetailLinkEntity procProcessRouteDetailLinkEntity)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(UpdateSql, procProcessRouteDetailLinkEntity);
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="procProcessRouteDetailLinkEntitys"></param>
    /// <returns></returns>
    public async Task<int> UpdateRangeAsync(IEnumerable<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntitys)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(UpdateSql, procProcessRouteDetailLinkEntitys);
    }

    /// <summary>
    /// 删除（软删除）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(long id)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(DeleteSql, new { Id = id });
    }

    /// <summary>
    /// 批量删除（软删除）
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteRangeAsync(long[] ids)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
    }

    /// <summary>
    /// 删除（软删除）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteByProcessRouteIdAsync(long id)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(DeleteByProcessRouteIdSql, new { ProcessRouteId = id });
    }
}

/// <summary>
/// 
/// </summary>
public partial class ProcProcessRouteDetailLinkRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `plan_work_order` /**where**/ LIMIT 1;";

    #endregion

    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_process_route_detail_link` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_process_route_detail_link` /**where**/ ";
    const string GetListSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_process_route_detail_link` /**where**/  ";

    const string InsertSql = "INSERT INTO `proc_process_route_detail_link`(  `Id`, `SiteId`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @SerialNo, @ProcessRouteId, @PreProcessRouteDetailId, @ProcessRouteDetailId, @Extra1, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string UpdateSql = "UPDATE `proc_process_route_detail_link` SET  SerialNo = @SerialNo, ProcessRouteId = @ProcessRouteId, PreProcessRouteDetailId = @PreProcessRouteDetailId, ProcessRouteDetailId = @ProcessRouteDetailId, Extra1 = @Extra1, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
    const string DeleteSql = "UPDATE `proc_process_route_detail_link` SET IsDeleted = '1' WHERE Id = @Id ";
    const string DeletesSql = "UPDATE `proc_process_route_detail_link` SET IsDeleted = '1' WHERE Id in @ids";
    const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route_detail_link`  WHERE Id = @Id ";
    const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route_detail_link`  WHERE Id IN @ids ";
    const string DeleteByProcessRouteIdSql = "delete from `proc_process_route_detail_link` WHERE ProcessRouteId = @ProcessRouteId ";
    const string GetByProcessRouteIdSql = "SELECT * FROM proc_process_route_detail_link WHERE ProcessRouteId = @ProcessRouteId; ";
    const string GetPreProcedureIDsSql = "SELECT * FROM proc_process_route_detail_link WHERE ProcessRouteId = @ProcessRouteId AND ProcessRouteDetailId = @ProcedureId; ";
    const string GetNextProcedureIDsSql = "SELECT * FROM proc_process_route_detail_link WHERE ProcessRouteId = @ProcessRouteId AND PreProcessRouteDetailId = @ProcedureId; ";
}




/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：工艺路线工序节点关系明细表(前节点多条就存多条);</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-28</para>
/// </summary>
public partial class ProcProcessRouteDetailLinkRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ProcProcessRouteDetailLinkQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (!string.IsNullOrWhiteSpace(query.SerialNo))
        {
            sqlBuilder.Where("SerialNo = @SerialNo");
        }

        if (!string.IsNullOrWhiteSpace(query.SerialNoLike))
        {
            query.SerialNoLike = $"{query.SerialNoLike}%";
            sqlBuilder.Where("SerialNo Like @SerialNoLike");
        }


        if (query.ProcessRouteId.HasValue)
        {
            sqlBuilder.Where("ProcessRouteId = @ProcessRouteId");
        }

        if (query.ProcessRouteIds?.Any() == true)
        {
            sqlBuilder.Where("ProcessRouteId IN @ProcessRouteIds");
        }


        if (query.PreProcessRouteDetailId.HasValue)
        {
            sqlBuilder.Where("PreProcessRouteDetailId = @PreProcessRouteDetailId");
        }

        if (query.PreProcessRouteDetailIds != null && query.PreProcessRouteDetailIds.Any())
        {
            sqlBuilder.Where("PreProcessRouteDetailId IN @PreProcessRouteDetailIds");
        }


        if (query.ProcessRouteDetailId.HasValue)
        {
            sqlBuilder.Where("ProcessRouteDetailId = @ProcessRouteDetailId");
        }

        if (query.ProcessRouteDetailIds != null && query.ProcessRouteDetailIds.Any())
        {
            sqlBuilder.Where("ProcessRouteDetailId IN @ProcessRouteDetailIds");
        }


        if (!string.IsNullOrWhiteSpace(query.Extra1))
        {
            sqlBuilder.Where("Extra1 = @Extra1");
        }

        if (!string.IsNullOrWhiteSpace(query.Extra1Like))
        {
            query.Extra1Like = $"{query.Extra1Like}%";
            sqlBuilder.Where("Extra1 Like @Extra1Like");
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


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}