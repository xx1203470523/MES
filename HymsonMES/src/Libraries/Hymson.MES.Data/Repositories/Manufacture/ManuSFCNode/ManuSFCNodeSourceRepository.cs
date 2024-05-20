using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;
using System.Text;

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
            if (entities == null || !entities.Any()) return 0;

            var sqlBuilder = new StringBuilder(InsertSql);
            foreach (var e in entities)
            {
                sqlBuilder.Append($"({e.Id}, {e.CirculationId}, {e.NodeId}, {e.SourceId}, '{e.CreatedBy}', '{e.CreatedOn}', {e.SiteId}),");
            }

            // 移除最后一个逗号
            sqlBuilder.Length--;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(sqlBuilder.ToString());

            /*
            // 使用StringBuilder来构建VALUES后面的括号集合
            var valuesBuilder = new StringBuilder();
            var parameters = new DynamicParameters();

            foreach (var e in entities)
            {
                // 使用参数化查询的占位符
                valuesBuilder.Append($"({e.Id}, @{e.Id}CirculationId, @{e.Id}NodeId, @{e.Id}SourceId, @{e.Id}CreatedBy, @{e.Id}CreatedOn, @{e.Id}SiteId),");

                // 构建每个对象的参数化值
                parameters.Add($"@{e.Id}CirculationId", e.CirculationId);
                parameters.Add($"@{e.Id}NodeId", e.NodeId);
                parameters.Add($"@{e.Id}SourceId", e.SourceId);
                parameters.Add($"@{e.Id}CreatedBy", e.CreatedBy);
                parameters.Add($"@{e.Id}CreatedOn", e.CreatedOn);
                parameters.Add($"@{e.Id}SiteId", e.SiteId);
            }

            // 移除最后一个逗号
            valuesBuilder.Length--;

            // 将构建的VALUES集合添加到SQL语句中
            sqlBuilder.Append(valuesBuilder);

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(sqlBuilder.ToString(), parameters);
            */
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> ReplaceRangeAsync(IEnumerable<ManuSFCNodeSourceEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ReplaceSql, entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(IEnumerable<ManuSFCNodeSourceEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            // 拼接删除SQL
            var stringBuilder = new StringBuilder();
            foreach (var item in entities)
            {
                stringBuilder.AppendFormat(DeleteSql, item.NodeId, item.SourceId);
            }

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(stringBuilder.ToString());
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

        const string InsertSql = "INSERT INTO manu_sfc_node_source (`Id`, `CirculationId`, `NodeId`, `SourceId`, `CreatedBy`, `CreatedOn`, `SiteId`) VALUES ";
        const string ReplaceSql = "REPLACE INTO manu_sfc_node_source (`Id`, CirculationId, `NodeId`, `SourceId`, `CreatedBy`, `CreatedOn`, `SiteId`) VALUES (@Id, @CirculationId, @NodeId, @SourceId, @CreatedBy, @CreatedOn, @SiteId) ";

        const string DeleteSql = "DELETE FROM manu_sfc_node_source WHERE NodeId = {0} AND SourceId = {1}; ";

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
