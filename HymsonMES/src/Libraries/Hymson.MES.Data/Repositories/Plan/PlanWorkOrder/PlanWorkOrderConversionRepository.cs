using Dapper;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.View;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Plan;

public partial class PlanWorkOrderConversionRepository : BaseRepository, IPlanWorkOrderConversionRepository
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
    /// <param name="memoryCache"></param>
    public PlanWorkOrderConversionRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// 新增工单转换系数
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(PlanWorkOrderConversionCreateCommand command)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertSql, command);
    }

    /// <summary>
    /// 更新工单转换系数
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> InsertOrUpdateAsync(PlanWorkOrderConversionUpdateCommand command)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertOrUpdateSql, command);
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PlanWorkOrderConversionView>> GetListAsync(PlanWorkOrderConversionQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var getListTemplete = sqlBuilder.AddTemplate(GetListSql);

        if (query.PlanWorkOrderId != null)
        {
            sqlBuilder.Where("PlanWorkOrderId = @PlanWorkOrderId");
        }

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryAsync<PlanWorkOrderConversionView>(getListTemplete.RawSql, getListTemplete.Parameters);
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PlanWorkOrderConversionView> GetOneAsync(PlanWorkOrderConversionQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var getListTemplete = sqlBuilder.AddTemplate(GetOneSql);

        if (query.PlanWorkOrderId != null)
        {
            sqlBuilder.Where("PlanWorkOrderId = @PlanWorkOrderId");
        }

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderConversionView>(getListTemplete.RawSql, getListTemplete.Parameters);
    }
}


public partial class PlanWorkOrderConversionRepository
{
    private const string GetListSql = "SELECT * FROM plan_work_order_conversion /**where**/";

    private const string GetOneSql = "SELECT * FROM plan_work_order_conversion /**where**/ LIMIT 1 ";

    private const string InsertSql = $@"INSERT INTO plan_work_order_conversion(Id, PlanWorkOrderId, ModuleConversion, PackConversion, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn) VALUES (@Id, @PlanWorkOrderId, @ModuleConversion, @PackConversion, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn)";

    private const string InsertOrUpdateSql = $@"INSERT INTO plan_work_order_conversion(Id, PlanWorkOrderId, ModuleConversion, PackConversion, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn) VALUES (@Id, @PlanWorkOrderId, @ModuleConversion, @PackConversion, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn)
ON DUPLICATE KEY UPDATE ModuleConversion = @ModuleConversion, PackConversion = @PackConversion,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn
";

}