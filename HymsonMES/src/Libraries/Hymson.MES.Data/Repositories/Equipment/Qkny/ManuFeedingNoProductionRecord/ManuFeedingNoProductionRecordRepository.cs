using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuFeedingNoProductionRecord;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuFeedingNoProductionRecord.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 仓储（设备投料非生产投料(洗罐子)）
    /// </summary>
    public partial class ManuFeedingNoProductionRecordRepository : BaseRepository, IManuFeedingNoProductionRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuFeedingNoProductionRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFeedingNoProductionRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuFeedingNoProductionRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFeedingNoProductionRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuFeedingNoProductionRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFeedingNoProductionRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFeedingNoProductionRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingNoProductionRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingNoProductionRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingNoProductionRecordEntity>> GetEntitiesAsync(ManuFeedingNoProductionRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingNoProductionRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFeedingNoProductionRecordEntity>> GetPagedListAsync(ManuFeedingNoProductionRecordPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuFeedingNoProductionRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFeedingNoProductionRecordEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFeedingNoProductionRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_feeding_no_production_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_feeding_no_production_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_feeding_no_production_record /**where**/  ";

        const string InsertSql = "INSERT INTO manu_feeding_no_production_record(  `Id`, `EquipmentId`, `ConsumeEquipmentCode`, `ConsumeResourceCodeCode`, `Sfc`, `Qty`, `Category`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @ConsumeEquipmentCode, @ConsumeResourceCodeCode, @Sfc, @Qty, @Category, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";
        const string InsertsSql = "INSERT INTO manu_feeding_no_production_record(  `Id`, `EquipmentId`, `ConsumeEquipmentCode`, `ConsumeResourceCodeCode`, `Sfc`, `Qty`, `Category`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @ConsumeEquipmentCode, @ConsumeResourceCodeCode, @Sfc, @Qty, @Category, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";

        const string UpdateSql = "UPDATE manu_feeding_no_production_record SET   EquipmentId = @EquipmentId, ConsumeEquipmentCode = @ConsumeEquipmentCode, ConsumeResourceCodeCode = @ConsumeResourceCodeCode, Sfc = @Sfc, Qty = @Qty, Category = @Category, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_feeding_no_production_record SET   EquipmentId = @EquipmentId, ConsumeEquipmentCode = @ConsumeEquipmentCode, ConsumeResourceCodeCode = @ConsumeResourceCodeCode, Sfc = @Sfc, Qty = @Qty, Category = @Category, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_feeding_no_production_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_feeding_no_production_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_feeding_no_production_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_feeding_no_production_record WHERE Id IN @Ids ";

    }
}
