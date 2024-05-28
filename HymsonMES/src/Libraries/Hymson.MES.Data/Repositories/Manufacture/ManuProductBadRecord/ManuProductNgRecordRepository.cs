using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（产品NG记录表）
    /// </summary>
    public partial class ManuProductNgRecordRepository : BaseRepository, IManuProductNgRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuProductNgRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductNgRecordEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductNgRecordEntity>> GetEntitiesAsync(ManuProducNGRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("BadRecordId = @BadRecordId");
            sqlBuilder.Where("UnqualifiedId = @UnqualifiedId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuProductNgRecordEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductNgRecordRepository
    {
        const string InsertsSql = "INSERT INTO manu_product_ng_record(`Id`, `SiteId`, `BadRecordId`, `UnqualifiedId`, `NGCode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @BadRecordId, @UnqualifiedId, @NGCode, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM `manu_product_ng_record` /**where**/  ";
    }
}
