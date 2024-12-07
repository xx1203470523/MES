using Dapper;
using Hymson.MES.BackgroundServices.Stator;

namespace Hymson.MES.Data.Repositories.Stator
{
    /// <summary>
    /// 仓储（铜线条码表）
    /// </summary>
    public partial class WireBarCodeRepository : BaseRepository, IWireBarCodeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WireBarCodeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WireBarCodeEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WireBarCodeEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WireBarCodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WireBarCodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WireBarCodeEntity>> GetEntitiesAsync(WireBarCodeQuery query)
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
    /// 铜线条码表
    /// </summary>
    public partial class WireBarCodeRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_wire_barcode /**where**/  ";

        const string InsertSql = "REPLACE INTO manu_wire_barcode(ID, WireId, WireBarCode, Remark, CreatedOn, UpdatedOn, SiteId) VALUES (@ID, @WireId, @WireBarCode, @Remark, @CreatedOn, @UpdatedOn, @SiteId) ";

        const string UpdateSql = "UPDATE manu_wire_barcode SET WireBarCode = @WireBarCode, Remark = @Remark, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string GetByIdSql = @"SELECT * FROM manu_wire_barcode WHERE Id = @Id ";

    }
}
