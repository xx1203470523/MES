using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    /// <summary>
    /// 上卸料记录表仓储
    /// </summary>
    public partial class ManuFeedingRecordRepository : BaseRepository, IManuFeedingRecordRepository
    {
        public ManuFeedingRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFeedingRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuFeedingRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingRecordEntity> GetEntity(ManuFeedingRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (query.MaterialStandingbookId.HasValue)
            {
                sqlBuilder.Where("MaterialStandingbookId=@MaterialStandingbookId");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFeedingRecordEntity>(templateData.RawSql, query);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFeedingRecordRepository
    {
        const string InsertSql = "INSERT INTO `manu_feeding_record`(`Id`, `ResourceId`, `FeedingPointId`, `ProductId`, `BarCode`, MaterialId, `Qty`, `DirectionType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, MaterialType, WorkOrderId, LoadSource, MaterialStandingbookId) VALUES (@Id, @ResourceId, @FeedingPointId, @ProductId, @BarCode, @MaterialId, @Qty, @DirectionType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @MaterialType, @WorkOrderId, @LoadSource,@MaterialStandingbookId)  ";
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_feeding_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT 1 ";
    }
}
