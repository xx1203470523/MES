using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码追溯表-正向）
    /// </summary>
    public partial class ManuSFCNodeDestinationRepository : BaseRepository, IManuSFCNodeDestinationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSFCNodeDestinationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSFCNodeDestinationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 查询树数据的List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeDestinationEntity>> GetTreeEntitiesAsync(long nodeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeDestinationEntity>(GetTreeEntitiesSql, new { NodeId = nodeId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeDestinationEntity>> GetEntitiesAsync(long nodeId)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("NodeId = @NodeId");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeDestinationEntity>(template.RawSql, new { NodeId = nodeId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeDestinationEntity>> GetEntitiesAsync(IEnumerable<long> nodeIds)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("NodeId IN @NodeIds");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeDestinationEntity>(template.RawSql, new { NodeIds = nodeIds });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeDestinationEntity>> GetEntitiesAsync(ManuSFCNodeDestinationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeDestinationEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSFCNodeDestinationRepository
    {
        const string GetTreeEntitiesSql = @"
                            ;WITH RECURSIVE CTE AS (
                              SELECT CirculationId, NodeId, DestinationId
                              FROM manu_sfc_node_destination
                              WHERE NodeId = @NodeId 
                              UNION ALL
                              SELECT T.CirculationId, T.NodeId, T.DestinationId
                              FROM manu_sfc_node_destination T
                              JOIN CTE ON CTE.DestinationId = T.NodeId
                            )
                            SELECT * FROM CTE;";

        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_node_destination /**where**/  ";

        const string InsertsSql = "REPLACE INTO manu_sfc_node_destination(`Id`, CirculationId, `NodeId`, `DestinationId`, `CreatedBy`, `CreatedOn`, `SiteId`) VALUES (@Id, @CirculationId, @NodeId, @DestinationId, @CreatedBy, @CreatedOn, @SiteId) ";


    }
}
