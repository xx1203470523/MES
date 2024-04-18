using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuFillingDataRecord;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuFillingDataRecord.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.ManuFillingDataRecord
{
    /// <summary>
    /// 仓储（补液数据上传记录）
    /// </summary>
    public partial class ManuFillingDataRecordRepository : BaseRepository, IManuFillingDataRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuFillingDataRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFillingDataRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuFillingDataRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFillingDataRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuFillingDataRecordEntity> entities)
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
        public async Task<ManuFillingDataRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFillingDataRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFillingDataRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFillingDataRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFillingDataRecordEntity>> GetEntitiesAsync(ManuFillingDataRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFillingDataRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFillingDataRecordEntity>> GetPagedListAsync(ManuFillingDataRecordPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuFillingDataRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFillingDataRecordEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFillingDataRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_filling_data_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_filling_data_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_filling_data_record /**where**/  ";

        const string InsertSql = "INSERT INTO manu_filling_data_record(  `Id`, `EquipmentId`, `InTime`, `OutTime`, `BeforeWeight`, `AfterWeight`, `ElWeight`, `AddEl`, `TotalEl`, `ManualEl`, `FinalEl`, `Sfc`, `IsOk`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @InTime, @OutTime, @BeforeWeight, @AfterWeight, @ElWeight, @AddEl, @TotalEl, @ManualEl, @FinalEl, @Sfc, @IsOk, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";
        const string InsertsSql = "INSERT INTO manu_filling_data_record(  `Id`, `EquipmentId`, `InTime`, `OutTime`, `BeforeWeight`, `AfterWeight`, `ElWeight`, `AddEl`, `TotalEl`, `ManualEl`, `FinalEl`, `Sfc`, `IsOk`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @InTime, @OutTime, @BeforeWeight, @AfterWeight, @ElWeight, @AddEl, @TotalEl, @ManualEl, @FinalEl, @Sfc, @IsOk, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";

        const string UpdateSql = "UPDATE manu_filling_data_record SET   EquipmentId = @EquipmentId, InTime = @InTime, OutTime = @OutTime, BeforeWeight = @BeforeWeight, AfterWeight = @AfterWeight, ElWeight = @ElWeight, AddEl = @AddEl, TotalEl = @TotalEl, ManualEl = @ManualEl, FinalEl = @FinalEl, Sfc = @Sfc, IsOk = @IsOk, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_filling_data_record SET   EquipmentId = @EquipmentId, InTime = @InTime, OutTime = @OutTime, BeforeWeight = @BeforeWeight, AfterWeight = @AfterWeight, ElWeight = @ElWeight, AddEl = @AddEl, TotalEl = @TotalEl, ManualEl = @ManualEl, FinalEl = @FinalEl, Sfc = @Sfc, IsOk = @IsOk, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_filling_data_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_filling_data_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_filling_data_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_filling_data_record WHERE Id IN @Ids ";

    }
}
