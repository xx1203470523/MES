using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.Query;
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
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(IEnumerable<ManuSFCNodeSourceEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, entities);
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
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_node_source /**where**/  ";

        const string InsertsSql = "REPLACE INTO manu_sfc_node_source(`Id`, CirculationId, `NodeId`, `SourceId`, `CreatedBy`, `CreatedOn`, `SiteId`) VALUES (@Id, @CirculationId, @NodeId, @SourceId, @CreatedBy, @CreatedOn, @SiteId) ";

        const string DeleteSql = "DELETE FROM manu_sfc_node_source WHERE NodeId = @NodeId AND SourceId = @SourceId; ";

#if DM
        const string GetTreeEntitiesSql = @"WITH RECURSIVE CTE (CirculationId, NodeId, SourceId) AS (
              SELECT CirculationId, NodeId, SourceId
              FROM manu_sfc_node_source
              WHERE NodeId = @NodeId
              UNION ALL
              SELECT T.CirculationId, T.NodeId, T.SourceId
              FROM manu_sfc_node_source T
              INNER JOIN CTE ON CTE.SourceId = T.NodeId
            )
            SELECT * FROM CTE;";

        const string MergeSql = "MERGE INTO manu_sfc_node_source t " +
            "USING (SELECT @NodeId AS NodeId, @SourceId AS SourceId FROM dual) s " +
            "ON (t.NodeId = s.NodeId AND t.SourceId = s.SourceId) " +
            "WHEN MATCHED THEN " +
              "UPDATE SET " +
                "t.CirculationId = @CirculationId, " +
                "t.CreatedBy = @CreatedBy, " +
                "t.CreatedOn = @CreatedOn, " +
                "t.SiteId = @SiteId " +
            "WHEN NOT MATCHED THEN " +
              "INSERT (Id, CirculationId, NodeId, SourceId, CreatedBy, CreatedOn, SiteId) " +
              "VALUES (@Id, @CirculationId, s.NodeId, s.SourceId, @CreatedBy, @CreatedOn, @SiteId);";
#else
        const string GetTreeEntitiesSql = @"
                            WITH RECURSIVE CTE AS (
                              SELECT CirculationId, NodeId, SourceId
                              FROM manu_sfc_node_source
                              WHERE NodeId = @NodeId 
                              UNION ALL
                              SELECT T.CirculationId, T.NodeId, T.SourceId
                              FROM manu_sfc_node_source T
                              JOIN CTE ON CTE.SourceId = T.NodeId
                            )
                            SELECT * FROM CTE;";

#endif

    }
}
