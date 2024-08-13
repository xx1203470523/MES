using Dapper;
using Hymson.MES.BackgroundServices.Stator;

namespace Hymson.MES.Data.Repositories.Stator
{
    /// <summary>
    /// 仓储（定子铜线关系表）
    /// </summary>
    public partial class StatorWireRelationRepository : BaseRepository, IStatorWireRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public StatorWireRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<StatorWireRelationEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StatorWireRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WireBarCodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StatorWireRelationEntity>> GetEntitiesAsync(StatorWireRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.WireIds != null && query.WireIds.Any())
            {
                sqlBuilder.Where("WireId IN @WireIds");
            }

            if (query.WireBarCodes != null && query.WireBarCodes.Any())
            {
                sqlBuilder.Where("WireBarCode IN @WireBarCodes");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WireBarCodeEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 定子铜线关系表
    /// </summary>
    public partial class StatorWireRelationRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_stator_wire_relation /**where**/  ";

        const string InsertSql = "INSERT IGNORE manu_stator_wire_relation (ID, InnerId, WireId, Remark, CreatedOn, SiteId) VALUES (@ID, @InnerId, @WireId, @Remark, @CreatedOn, @SiteId) ";

        const string GetByIdSql = @"SELECT * FROM manu_stator_wire_relation WHERE Id = @Id ";

    }
}
