using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码工序生产汇总表）
    /// </summary>
    public partial class ManuSfcSummaryRepository : BaseRepository, IManuSfcSummaryRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSfcSummaryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcSummaryEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcSummaryEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 合并存在更新 不存在则新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> MergeRangeAsync(IEnumerable<ManuSfcSummaryEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(MergeSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcSummaryEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcSummaryEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 合格产出更新汇总表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateSummaryOutStationRangeAsync(IEnumerable<UpdateOutputQtySummaryCommand>? multiUpdateSummaryOutStationCommands)
        {
            if (multiUpdateSummaryOutStationCommands == null || !multiUpdateSummaryOutStationCommands.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSummaryOutStationSql, multiUpdateSummaryOutStationCommands);
        }

        /// <summary>
        /// 不合格产出更新汇总表
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateSummaryUnqualifiedRangeAsync(IEnumerable<MultiUpdateSummaryUnqualifiedCommand> multiUpdateSummaryUnqualifiedCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSummaryUnqualifiedSql, multiUpdateSummaryUnqualifiedCommand);
        }

        /// <summary>
        /// 复判不合格
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateSummaryReJudgmentUnqualifiedRangeAsync(IEnumerable<MultiUpdateSummaryReJudgmentUnqualifiedCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSummaryUnqualifiedSql, commands);
        }

        /// <summary>
        /// 复判合格
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> MultiUpdateSummaryReJudgmentQualifiedRangeAsync(IEnumerable<MultiUpdateSummaryReJudgmentQualifiedCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSummaryUnqualifiedSql, commands);
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
        public async Task<ManuSfcSummaryEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcSummaryEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcSummaryEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcSummaryEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 获取条码最后数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcSummaryEntity>> GetyLastListBySfsAsync(LastManuSfcSummaryBySfcsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcSummaryEntity>(GetMaxTimeBySFCsSql, query);
        }

        /// <summary>
        /// 获取条码最后数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcSummaryEntity>> GetyLastListByProcedureIdsAndSfcsAsync(LastManuSfcSummaryByProcedureIdAndSfcsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcSummaryEntity>(GetMaxTimeByProcedureIdsAndSfcsSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcSummaryEntity>> GetEntitiesAsync(ManuSfcSummaryQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Where("IsDeleted=0");

            if (!string.IsNullOrWhiteSpace(query.SFC)) {
                sqlBuilder.Where("SFC=@SFC");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcSummaryEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcSummaryEntity>> GetPagedListAsync(ManuSfcSummaryPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuSfcSummaryEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcSummaryEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcSummaryRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_summary /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_sfc_summary /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM manu_sfc_summary /**where**/  ";

        const string InsertSql = "INSERT INTO manu_sfc_summary(  `Id`, `SiteId`, `SFC`, `WorkOrderId`, `ProductId`, `ProcedureId`, `StartOn`, `EndOn`, `InvestQty`, `OutputQty`, `UnqualifiedQty`, `RepeatedCount`, `IsJudgment`, `JudgmentOn`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SFC, @WorkOrderId, @ProductId, @ProcedureId, @StartOn, @EndOn,@InvestQty, @OutputQty, @UnqualifiedQty, @RepeatedCount, @IsJudgment, @JudgmentOn, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO manu_sfc_summary(  `Id`, `SiteId`, `SFC`, `WorkOrderId`, `ProductId`, `ProcedureId`, `StartOn`, `EndOn`, `InvestQty`, `OutputQty`, `UnqualifiedQty`, `RepeatedCount`, `IsJudgment`, `JudgmentOn`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SFC, @WorkOrderId, @ProductId, @ProcedureId, @StartOn, @EndOn,@InvestQty, @OutputQty, @UnqualifiedQty, @RepeatedCount, @IsJudgment, @JudgmentOn, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string MergeSql = @"INSERT INTO manu_sfc_summary(  `Id`, `SiteId`, `SFC`, `WorkOrderId`, `ProductId`, `ProcedureId`, `StartOn`, `EndOn`, `InvestQty`, `OutputQty`, `UnqualifiedQty`, `RepeatedCount`, `IsJudgment`, `JudgmentOn`, `Remark`,`LastUpdatedOn`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SFC, @WorkOrderId, @ProductId, @ProcedureId, @StartOn, @EndOn,@InvestQty, @OutputQty, @UnqualifiedQty, @RepeatedCount, @IsJudgment, @JudgmentOn, @Remark, @LastUpdatedOn, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) 
                           ON DUPLICATE KEY UPDATE SiteId = @SiteId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, ProcedureId = @ProcedureId, StartOn = @StartOn, EndOn = @EndOn, OutputQty = @OutputQty, UnqualifiedQty = @UnqualifiedQty, RepeatedCount = @RepeatedCount, IsJudgment = @IsJudgment, JudgmentOn = @JudgmentOn, Remark = @Remark,LastUpdatedOn=@LastUpdatedOn, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted;";
#if DM
        const string UpdateSql = "UPDATE manu_sfc_summary SET   SiteId = @SiteId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, ProcedureId = @ProcedureId, StartOn = @StartOn, EndOn = @EndOn, OutputQty = @OutputQty, UnqualifiedQty = @UnqualifiedQty, RepeatedCount = @RepeatedCount, IsJudgment = @IsJudgment, JudgmentOn = @JudgmentOn, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

#else
        const string UpdateSql = "UPDATE manu_sfc_summary SET   SiteId = @SiteId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, ProcedureId = @ProcedureId, StartOn = @StartOn, EndOn = @EndOn, OutputQty = @OutputQty, UnqualifiedQty = @UnqualifiedQty, RepeatedCount = @RepeatedCount, IsJudgment = @IsJudgment, JudgmentOn = @JudgmentOn, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

#endif
        const string UpdatesSql = "UPDATE manu_sfc_summary SET   SiteId = @SiteId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, ProcedureId = @ProcedureId, StartOn = @StartOn, EndOn = @EndOn, OutputQty = @OutputQty, UnqualifiedQty = @UnqualifiedQty, RepeatedCount = @RepeatedCount, IsJudgment = @IsJudgment, JudgmentOn = @JudgmentOn, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateSummaryOutStationSql = "UPDATE manu_sfc_summary SET EndOn = @EndOn, OutputQty = @OutputQty, IsJudgment = @IsJudgment, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateSummaryUnqualifiedSql = "UPDATE manu_sfc_summary SET EndOn = @EndOn, UnqualifiedQty = @UnqualifiedQty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE manu_sfc_summary SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_sfc_summary SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_summary WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_sfc_summary WHERE Id IN @Ids ";
        const string GetMaxTimeBySFCsSql = @"SELECT T1.* FROM manu_sfc_summary T1 LEFT JOIN (
			SELECT SFC, MAX(UpdatedOn) AS MaxUpdatedOn FROM manu_sfc_summary GROUP BY SFC
	        ) T2 ON T1.SFC = T2.SFC AND T1.UpdatedOn = T2.MaxUpdatedOn WHERE T1.SFC IN @Sfcs  AND T1.SiteId=@SiteId AND T1.IsDeleted=0   ";
        const string GetMaxTimeByProcedureIdsAndSfcsSql = @"SELECT T1.* FROM manu_sfc_summary T1 LEFT JOIN (
			SELECT ProductId, SFC, MAX(UpdatedOn) AS MaxUpdatedOn FROM manu_sfc_summary GROUP BY SFC,ProductId
	        ) T2 ON T1.SFC = T2.SFC AND T1.ProductId = T2.ProductId AND T1.UpdatedOn = T2.MaxUpdatedOn WHERE T1.SFC IN @Sfcs  AND T1.IsDeleted=0   ";

        

    }
}
