using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础实现</para>
/// <para>@描述：条码表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-24</para>
/// </summary>
public partial class ManuSfcRepository
{
    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ManuSfcEntity> GetOneAsync(ManuSfcQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcEntity>> GetListAsync(ManuSfcQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<ManuSfcEntity>(templateData.RawSql, templateData.Parameters);
    }

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：基础SQL语句</para>
/// <para>@描述：条码表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-24</para>
/// </summary>
public partial class ManuSfcRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `manu_sfc` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `manu_sfc` /**where**/;";

    #endregion
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：条码表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2023-12-24</para>
/// </summary>
public partial class ManuSfcRepository
{
    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcQuery query)
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

        if (!string.IsNullOrWhiteSpace(query.SFC))
        {
            sqlBuilder.Where("SFC = @SFC");
        }

        if (query.SFCs != null && query.SFCs.Any())
        {
            sqlBuilder.Where("SFC IN @SFCs");
        }

        if (!string.IsNullOrWhiteSpace(query.SFCLike))
        {
            query.SFCLike = $"{query.SFCLike}%";
            sqlBuilder.Where("SFC Like @SFCLike");
        }

        if (query.QtyMin.HasValue)
        {
            sqlBuilder.Where("Qty >= @QtyMin");
        }

        if (query.QtyMax.HasValue)
        {
            sqlBuilder.Where("Qty <= @QtyMax");
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

        if (query.IsUsed.HasValue)
        {
            sqlBuilder.Where("IsUsed = @IsUsed");
        }

        if (query.SfcScrapId.HasValue)
        {
            sqlBuilder.Where("SfcScrapId = @SfcScrapId");
        }

        if (query.SfcScrapIds != null && query.SfcScrapIds.Any())
        {
            sqlBuilder.Where("SfcScrapId IN @SfcScrapIds");
        }

        if (query.StatusBack.HasValue)
        {
            sqlBuilder.Where("StatusBack = @StatusBack");
        }

        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }
}