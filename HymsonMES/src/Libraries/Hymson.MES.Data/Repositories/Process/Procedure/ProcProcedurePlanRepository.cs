

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Proc;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Proc;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础实现</para>
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public partial class ProcProcedurePlanRepository : BaseRepository, IProcProcedurePlanRepository
{
    private readonly ConnectionOptions _connectionOptions;

    public ProcProcedurePlanRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ProcProcedurePlanEntity> GetOneAsync(ProcProcedurePlanQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn =  GetMESDbConnection(); 

        return await conn.QueryFirstOrDefaultAsync<ProcProcedurePlanEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcedurePlanEntity>> GetListAsync(ProcProcedurePlanQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn =  GetMESDbConnection(); 

        return await conn.QueryAsync<ProcProcedurePlanEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProcProcedurePlanEntity>> GetPagedInfoAsync(ProcProcedurePlanPagedQuery query)
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

        using var conn =  GetMESDbConnection(); 

        var procProcedurePlanEntities = await conn.QueryAsync<ProcProcedurePlanEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<ProcProcedurePlanEntity>(procProcedurePlanEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 新增

    /// <summary>
    /// 创建数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(ProcProcedurePlanCreateCommand command)
    {
        using var conn =  GetMESDbConnection(); 
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 批量创建数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(IEnumerable<ProcProcedurePlanCreateCommand> commands)
    {
        using var conn =  GetMESDbConnection(); 
        return await conn.ExecuteAsync(InsertSql, commands);
    }

    #endregion

    #region 修改

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ProcProcedurePlanUpdateCommand command)
    {
        using var conn =  GetMESDbConnection(); 
        return await conn.ExecuteAsync(UpdateByIdSql, command);
    }

    /// <summary>
    /// 批量更新数据
    /// </summary>
    /// <param name="commands"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(IEnumerable<ProcProcedurePlanUpdateCommand> commands)
    {
        using var conn =  GetMESDbConnection(); 
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
        using var conn =  GetMESDbConnection(); 
        return await conn.ExecuteAsync(DeleteByIdSql, command);
    }

    /// <summary>
    /// 根据ID删除数据
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeleteByProcedureIdAsync(ProcProcedurePlanDeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteByProcedureIdSql, command);
    }

    ///// <summary>
    ///// 根据ID删除多条数据
    ///// </summary>
    ///// <param name="command"></param>
    ///// <returns></returns>
    //public async Task<int> DeleteMoreAsync(DeleteMoreCommand command)
    //{
    //    using var conn =  GetMESDbConnection(); 
    //    return await conn.ExecuteAsync(DeleteMoreByIdSql, command);
    //}

    #region 扩展

    public async Task<int> InsertOrUpdateAsnyc(ProcProcedurePlanCreateCommand command)
    {
        using var conn = GetMESDbConnection(); 
        return await conn.ExecuteAsync(InsertOrUpdateSql, command);
    }

    #endregion

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础SQL语句</para>
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public partial class ProcProcedurePlanRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `proc_procedure_plan` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `proc_procedure_plan` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `proc_procedure_plan` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `proc_procedure_plan` /**where**/;";

    #endregion

    #region 新增

    const string InsertSql = "INSERT INTO `proc_procedure_plan` (`Id`,`SiteId`,`ProcedureId`,`PlanOutputQty`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`) VALUES (@Id,@SiteId,@ProcedureId,@PlanOutputQty,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted);";

    #endregion

    #region 修改

    const string UpdateByIdSql = "UPDATE `proc_procedure_plan` SET `SiteId` = @SiteId ,`ProcedureId` = @ProcedureId ,`PlanOutputQty` = @PlanOutputQty  ,`UpdatedBy` = @UpdatedBy ,`UpdatedOn` = @UpdatedOn WHERE Id = @id;";

    #endregion

    #region 删除

    const string DeleteByIdSql = "UPDATE `proc_procedure_plan` SET IsDeleted = Id WHERE Id = @Id;";

    const string DeleteMoreByIdSql = "UPDATE `proc_procedure_plan` SET IsDeleted = Id WHERE Id IN @Ids;";

    const string DeleteByProcedureIdSql = "DELETE `proc_procedure_plan` WHERE ProcedureId IN @ProcedureIds;";

    #endregion

    #region 扩展

    const string InsertOrUpdateSql = @"INSERT INTO `proc_procedure_plan` (`Id`,`SiteId`,`ProcedureId`,`PlanOutputQty`,`CreatedBy`,`CreatedOn`,`UpdatedBy`,`UpdatedOn`,`IsDeleted`) VALUES (@Id,@SiteId,@ProcedureId,@PlanOutputQty,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted)
        ON DUPLICATE KEY UPDATE PlanOutputQty = @PlanOutputQty,UpdatedBy = @UpdatedBy,UpdatedOn = @UpdatedOn";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public partial class ProcProcedurePlanRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ProcProcedurePlanPagedQuery query)
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


        if (!string.IsNullOrWhiteSpace(query.ProcedureId))
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }

        if (!string.IsNullOrWhiteSpace(query.ProcedureIdLike))
        {
            query.ProcedureIdLike = $"{query.ProcedureIdLike}%";
            sqlBuilder.Where("ProcedureId Like @ProcedureIdLike");
        }


        if (query.PlanOutputQtyMin.HasValue)
        {
            sqlBuilder.Where("PlanOutputQty >= @PlanOutputQtyMin");
        }

        if (query.PlanOutputQtyMax.HasValue)
        {
            sqlBuilder.Where("PlanOutputQty <= @PlanOutputQtyMax");
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
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ProcProcedurePlanQuery query)
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


        if (query.ProcedureId.HasValue)
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }

        if (!string.IsNullOrWhiteSpace(query.ProcedureIdLike))
        {
            query.ProcedureIdLike = $"{query.ProcedureIdLike}%";
            sqlBuilder.Where("ProcedureId Like @ProcedureIdLike");
        }


        if (query.PlanOutputQtyMin.HasValue)
        {
            sqlBuilder.Where("PlanOutputQty >= @PlanOutputQtyMin");
        }

        if (query.PlanOutputQtyMax.HasValue)
        {
            sqlBuilder.Where("PlanOutputQty <= @PlanOutputQtyMax");
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
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public partial class ProcProcedurePlanRepository
{
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：工序计划产能;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-27</para>
/// </summary>
public partial class ProcProcedurePlanRepository
{
}