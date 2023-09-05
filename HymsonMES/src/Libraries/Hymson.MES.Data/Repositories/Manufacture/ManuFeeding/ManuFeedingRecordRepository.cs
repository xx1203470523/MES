using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    /// <summary>
    /// 上卸料记录表仓储
    /// </summary>
    public partial class ManuFeedingRecordRepository : IManuFeedingRecordRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ManuFeedingRecordRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFeedingRecordEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuFeedingRecordEntity> entities)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entities);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFeedingRecordRepository
    {
        const string InsertSql = "INSERT INTO `manu_feeding_record`(  `Id`, `ResourceId`, `FeedingPointId`, `ProductId`, `BarCode`, MaterialId, `Qty`, `DirectionType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @ResourceId, @FeedingPointId, @ProductId, @BarCode, @MaterialId, @Qty, @DirectionType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
    }
}
