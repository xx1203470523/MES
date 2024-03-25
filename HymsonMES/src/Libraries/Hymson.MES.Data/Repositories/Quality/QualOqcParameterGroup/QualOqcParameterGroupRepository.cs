using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality;

/// <summary>
/// 仓储（OQC检验参数组）
/// </summary>
public partial class QualOqcParameterGroupRepository : BaseRepository, IQualOqcParameterGroupRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionOptions"></param>
    public QualOqcParameterGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(QualOqcParameterGroupEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, entity);
    }

    /// <summary>
    /// 新增（批量）
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public async Task<int> InsertRangeAsync(IEnumerable<QualOqcParameterGroupEntity> entities)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertsSql, entities);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(QualOqcParameterGroupEntity entity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, entity);
    }

    /// <summary>
    /// 更新（批量）
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public async Task<int> UpdateRangeAsync(IEnumerable<QualOqcParameterGroupEntity> entities)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdatesSql, entities);
    }

    /// <summary>
    /// 软删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(long id)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteSql, new { Id = id });
    }

    /// <summary>
    /// 软删除（批量）
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DeletesAsync(DeleteCommand command)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeletesSql, command);
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<QualOqcParameterGroupEntity> GetByIdAsync(long id)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<QualOqcParameterGroupEntity>(GetByIdSql, new { Id = id });
    }

    /// <summary>
    /// 根据IDs获取数据（批量）
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QualOqcParameterGroupEntity>> GetByIdsAsync(long[] ids)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<QualOqcParameterGroupEntity>(GetByIdsSql, new { Ids = ids });
    }

    /// <summary>
    /// 查询单个实体
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<QualOqcParameterGroupEntity> GetEntityAsync(QualOqcParameterGroupQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetEntitySqlTemplate);
        sqlBuilder.Select("*");
        sqlBuilder.Where("IsDeleted = 0");
        sqlBuilder.Where("SiteId = @SiteId");
        if (query.MaterialId.HasValue)
        {
            sqlBuilder.Where("MaterialId = @MaterialId");
        }
        if (query.CustomerId.HasValue)
        {
            sqlBuilder.Where("CustomerId = @CustomerId");
        }
        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }
        //排序
        if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<QualOqcParameterGroupEntity>(template.RawSql, query);
    }

    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<QualOqcParameterGroupEntity>> GetEntitiesAsync(QualOqcParameterGroupQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
        sqlBuilder.Select("*");
        sqlBuilder.Where("IsDeleted = 0");
        //sqlBuilder.Where("SiteId = @SiteId");
        if (query.MaterialId.HasValue)
        {
            sqlBuilder.Where("MaterialId = @MaterialId");
        }
        if (query.CustomerId.HasValue)
        {
            sqlBuilder.Where("CustomerId = @CustomerId");
        }
        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }
        if (query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }
        //排序
        if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
        sqlBuilder.AddParameters(query);
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<QualOqcParameterGroupEntity>(template.RawSql, query);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="pagedQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<QualOqcParameterGroupEntity>> GetPagedListAsync(QualOqcParameterGroupPagedQuery pagedQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
        sqlBuilder.Select("*");
        sqlBuilder.OrderBy(string.IsNullOrWhiteSpace(pagedQuery.Sorting) ? "CreatedOn DESC" : pagedQuery.Sorting);

        WhereFill(sqlBuilder, new QualOqcParameterGroupToQuery
        {
            CodeLike = pagedQuery.CodeLike,
            NameLike = pagedQuery.NameLike,
            MaterialIds = pagedQuery.MaterialIds,
            CustomerIds = pagedQuery.CustomerIds,
            Status = pagedQuery.Status,
            SiteId = pagedQuery.SiteId
        });

        var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
        sqlBuilder.AddParameters(pagedQuery);

        using var conn = GetMESDbConnection();
        var entitiesTask = conn.QueryAsync<QualOqcParameterGroupEntity>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var entities = await entitiesTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<QualOqcParameterGroupEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
    }


    public async Task<QualOqcParameterGroupEntity> GetOneAsync(QualOqcParameterGroupToQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<QualOqcParameterGroupEntity>(templateData.RawSql, templateData.Parameters);
    }

}


/// <summary>
/// 
/// </summary>
public partial class QualOqcParameterGroupRepository
{
    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_parameter_group T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM qual_oqc_parameter_group T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
    const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_parameter_group /**where**/ /**orderby**/  ";
    const string GetEntitySqlTemplate = @"SELECT /**select**/ FROM qual_oqc_parameter_group /**where**/ /**orderby**/ LIMIT 1";

    const string InsertSql = "INSERT INTO qual_oqc_parameter_group(  `Id`, `SiteId`, `Code`, `Name`, `MaterialId`, `CustomerId`, `SampleQty`, `Version`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @MaterialId, @CustomerId, @SampleQty, @Version, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
    const string InsertsSql = "INSERT INTO qual_oqc_parameter_group(  `Id`, `SiteId`, `Code`, `Name`, `MaterialId`, `CustomerId`, `SampleQty`, `Version`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @MaterialId, @CustomerId, @SampleQty, @Version, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

    const string UpdateSql = "UPDATE qual_oqc_parameter_group SET   SiteId = @SiteId, Code = @Code, Name = @Name, MaterialId = @MaterialId, CustomerId = @CustomerId, Version = @Version, Status = @Status, Remark = @Remark,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
    const string UpdatesSql = "UPDATE qual_oqc_parameter_group SET   SiteId = @SiteId, Code = @Code, Name = @Name, MaterialId = @MaterialId, CustomerId = @CustomerId, Version = @Version, Status = @Status, Remark = @Remark,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

    const string DeleteSql = "UPDATE qual_oqc_parameter_group SET IsDeleted = Id WHERE Id = @Id ";
    const string DeletesSql = "UPDATE qual_oqc_parameter_group SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

    const string GetByIdSql = @"SELECT * FROM qual_oqc_parameter_group WHERE Id = @Id ";
    const string GetByIdsSql = @"SELECT * FROM qual_oqc_parameter_group WHERE Id IN @Ids ";

    const string GetOneSqlTemplate = "SELECT * FROM `qual_oqc_parameter_group` /**where**/ LIMIT 1;";

}

public partial class QualOqcParameterGroupRepository
{
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, QualOqcParameterGroupToQuery query)
    {
        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (!string.IsNullOrWhiteSpace(query.Code))
        {
            sqlBuilder.Where("Code = @Code");
        }

        if (!string.IsNullOrWhiteSpace(query.CodeLike))
        {
            query.CodeLike = $"%{query.CodeLike}%";
            sqlBuilder.Where("Code Like @CodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sqlBuilder.Where("Name = @Name");
        }

        if (!string.IsNullOrWhiteSpace(query.NameLike))
        {
            query.NameLike = $"%{query.NameLike}%";
            sqlBuilder.Where("Name Like @NameLike");
        }


        if (query.MaterialId.HasValue)
        {
            sqlBuilder.Where("MaterialId = @MaterialId");
        }

        if (query.MaterialIds != null && query.MaterialIds.Any())
        {
            sqlBuilder.Where("MaterialId IN @MaterialIds");
        }


        if (query.CustomerId.HasValue)
        {
            sqlBuilder.Where("CustomerId = @CustomerId");
        }

        if (query.CustomerIds != null && query.CustomerIds.Any())
        {
            sqlBuilder.Where("CustomerId IN @CustomerIds");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }


        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
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


        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
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


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }

        if (!string.IsNullOrWhiteSpace(query.Version))
        {
            sqlBuilder.Where("Version = @Version");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }
}
