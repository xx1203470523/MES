using Dapper;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture;

public partial class ManuContainerPackRepository
{
    /// <summary>
    /// 获取容器装载信息
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuContainerPackEntity>> GetListAsync(ManuContainerPackQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<ManuContainerPackEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 获取容器装载信息
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ManuContainerPackEntity> GetOneAsync(ManuContainerPackQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<ManuContainerPackEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 获取容器装载数量
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<int> GetCountAsync(ManuContainerPackQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetCountSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<int>(templateData.RawSql, templateData.Parameters);
    }

    ///// <summary>
    ///// 更新最外层包装容器和装载深度
    ///// </summary>
    ///// <param name="command"></param>
    ///// <returns></returns>
    //public async Task<int> UpdateOutermostContainerBarCodeAndDeepAsync(IEnumerable<UpdateOutermostContainerBarCodeAndDeepCommand> commands)
    //{
    //    using var conn = GetMESDbConnection();
    //    return await conn.QueryFirstOrDefaultAsync<int>(UpdateOutermostContainerBarCodeAndDeepSql, commands);
    //}
}

public partial class ManuContainerPackRepository
{
    const string GetOneSqlTemplate = "SELECT * FROM `manu_container_pack` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `manu_container_pack` /**where**/;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `manu_container_pack` /**where**/;";

    //const string UpdateOutermostContainerBarCodeAndDeepSql = "UPDATE manu_container_pack SET OutermostContainerBarCodeId = @OutermostContainerBarCodeId, Deep = @Deep, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id AND UpdatedOn = @CheckUpdatedOn;";
}
