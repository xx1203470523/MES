using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（成品条码产出记录(FQC生成使用)）
    /// </summary>
    public partial class QualFinallyOutputRecordRepository : BaseRepository, IQualFinallyOutputRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualFinallyOutputRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFinallyOutputRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualFinallyOutputRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFinallyOutputRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualFinallyOutputRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 更新IsGenerated（批量）
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<int> UpdateGeneratedRangeAsync(IEnumerable<string> barcode)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesIsGeneratedSql, new { barcode });
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
        public async Task<QualFinallyOutputRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFinallyOutputRecordEntity>(GetByIdSql, new { Id = id });
        }

        public async Task<QualFinallyOutputRecordEntity> GetBySFCAsync(string barcode)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFinallyOutputRecordEntity>(GetByBarcodeSql, new { Barcode = barcode });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFinallyOutputRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFinallyOutputRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<QualFinallyOutputRecordEntity> GetEntityAsync(QualFinallyOutputRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitySqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFinallyOutputRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFinallyOutputRecordEntity>> GetEntitiesAsync(QualFinallyOutputRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.MaterialId.HasValue)
            {
                sqlBuilder.Where("MaterialId = @MaterialId");
            }
            if (query.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            }
            if (query.WorkCenterId.HasValue)
            {
                sqlBuilder.Where("WorkCenterId = @WorkCenterId");
            }
            if (query.IsGenerated.HasValue)
            {
                sqlBuilder.Where("IsGenerated = @IsGenerated");
            }

            if (query.Barcodes != null)
            {
                sqlBuilder.Where("Barcode IN @Barcodes");
            }
            sqlBuilder.AddParameters(query);
            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFinallyOutputRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFinallyOutputRecordEntity>> GetPagedListAsync(QualFinallyOutputRecordPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("T.*");
            sqlBuilder.OrderBy(string.IsNullOrWhiteSpace(pagedQuery.Sorting) ? "T.CreatedOn DESC" : pagedQuery.Sorting);
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Barcode))
            {
                sqlBuilder.Where("T.Barcode = @Barcode");
            }

            if (pagedQuery.MaterialId != 0)
            {
                sqlBuilder.Where("T.MaterialId = @MaterialId");
            }

            if (pagedQuery.WorkOrderId != 0)
            {
                sqlBuilder.Where("T.WorkOrderId = @WorkOrderId");
            }

            if (pagedQuery.WorkCenterId != 0)
            {
                sqlBuilder.Where("T.WorkCenterId = @WorkCenterId");
            }
            if (pagedQuery.IsGenerated.HasValue)
            {
                sqlBuilder.Where("T.IsGenerated = @IsGenerated");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<QualFinallyOutputRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<QualFinallyOutputRecordEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualFinallyOutputRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_finally_output_record T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_finally_output_record T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_finally_output_record /**where**/ /**orderby**/ ";
        const string GetEntitySqlTemplate = @"SELECT /**select**/ FROM qual_finally_output_record /**where**/ /**orderby**/ LIMIT 1 ";

        const string InsertSql = "INSERT INTO qual_finally_output_record(  `Id`, `SiteId`, `Barcode`, `MaterialId`, `WorkOrderId`, `WorkCenterId`, `CodeType`, `IsGenerated`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Barcode, @MaterialId, @WorkOrderId, @WorkCenterId, @CodeType, @IsGenerated, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_finally_output_record(  `Id`, `SiteId`, `Barcode`, `MaterialId`, `WorkOrderId`, `WorkCenterId`, `CodeType`, `IsGenerated`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Barcode, @MaterialId, @WorkOrderId, @WorkCenterId, @CodeType, @IsGenerated, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_finally_output_record SET   SiteId = @SiteId, Barcode = @Barcode, MaterialId = @MaterialId, WorkOrderId = @WorkOrderId, WorkCenterId = @WorkCenterId, CodeType = @CodeType, IsGenerated = @IsGenerated, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_finally_output_record SET   SiteId = @SiteId, Barcode = @Barcode, MaterialId = @MaterialId, WorkOrderId = @WorkOrderId, WorkCenterId = @WorkCenterId, CodeType = @CodeType, IsGenerated = @IsGenerated, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesIsGeneratedSql = "UPDATE qual_finally_output_record SET   IsGenerated = @IsGenerated,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Barcode IN @barcode ";

        const string DeleteSql = "UPDATE qual_finally_output_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_finally_output_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByBarcodeSql = @"SELECT * FROM qual_finally_output_record WHERE Barcode = @Barcode ";
        const string GetByIdSql = @"SELECT * FROM qual_finally_output_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_finally_output_record WHERE Id IN @Ids ";

    }
}
