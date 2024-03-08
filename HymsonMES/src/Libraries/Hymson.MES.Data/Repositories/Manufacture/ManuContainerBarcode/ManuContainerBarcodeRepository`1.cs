using Dapper;
using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.Data.Repositories.Manufacture;

public partial class ManuContainerBarcodeRepository
{
    /// <summary>
    /// 获取容器条码列表
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuContainerBarcodeEntity>> GetListAsync(ManuContainerBarcodeQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<ManuContainerBarcodeEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 获取容器条码
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ManuContainerBarcodeEntity> GetOneAsync(ManuContainerBarcodeQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<ManuContainerBarcodeEntity>(templateData.RawSql, templateData.Parameters);
    }
}

/// <summary>
/// SQL
/// </summary>
public partial class ManuContainerBarcodeRepository
{
    const string GetListSqlTemplate = "SELECT * FROM `manu_container_barcode` /**where**/;";
    const string GetOneSqlTemplate = "SELECT * FROM `manu_container_barcode` /**where**/ LIMIT 1;";
}
