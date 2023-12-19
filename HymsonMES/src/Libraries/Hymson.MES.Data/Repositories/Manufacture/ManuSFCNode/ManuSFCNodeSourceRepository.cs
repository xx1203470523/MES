using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using IdGen;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码追溯表-反向）
    /// </summary>
    public partial class ManuSFCNodeSourceRepository : BaseRepository, IManuSFCNodeSourceRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSFCNodeSourceRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSFCNodeSourceEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 查询树数据的List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeSourceEntity>> GetTreeEntitiesAsync(long nodeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeSourceEntity>(GetTreeEntitiesSql, new { NodeId = nodeId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeSourceEntity>> GetEntitiesAsync(long nodeId)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("NodeId = @NodeId");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeSourceEntity>(template.RawSql, new { NodeId = nodeId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeSourceEntity>> GetEntitiesAsync(IEnumerable<long> nodeIds)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("NodeId IN @NodeIds");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeSourceEntity>(template.RawSql, new { NodeIds = nodeIds });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeSourceEntity>> GetEntitiesAsync(ManuSFCNodeSourceQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeSourceEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSFCNodeSourceRepository
    {
        const string GetTreeEntitiesSql = @"
                            ;WITH RECURSIVE CTE AS (
                              SELECT NodeId, SourceId
                              FROM manu_sfc_node_source
                              WHERE NodeId = @NodeId 
                              UNION ALL
                              SELECT T.NodeId, T.SourceId
                              FROM manu_sfc_node_source T
                              JOIN CTE ON CTE.SourceId = T.NodeId
                            )
                            SELECT * FROM CTE;";

        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_node_source /**where**/  ";

        const string InsertsSql = "REPLACE INTO manu_sfc_node_source(  `Id`, `NodeId`, `SourceId`, `CreatedBy`, `CreatedOn`, `SiteId`) VALUES (  @Id, @NodeId, @SourceId, @CreatedBy, @CreatedOn, @SiteId) ";

    }
}
