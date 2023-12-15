using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSFCNodeSourceEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSFCNodeSourceEntity>(GetByIdSql, new { Id = id });
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

        const string InsertsSql = "INSERT IGNORE manu_sfc_node_source(  `Id`, `NodeId`, `SourceId`, `CreatedBy`, `CreatedOn`, `SiteId`) VALUES (  @Id, @NodeId, @SourceId, @CreatedBy, @CreatedOn, @SiteId) ";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_node_source WHERE Id = @Id ";

    }
}
