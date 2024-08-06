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
    /// 仓储（iqc检验单（新））
    /// </summary>
    public partial class QualIqcOrderLiteDetailRepository : BaseRepository, IQualIqcOrderLiteDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIqcOrderLiteDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIqcOrderLiteDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIqcOrderLiteDetailEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIqcOrderLiteDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIqcOrderLiteDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
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
        public async Task<QualIqcOrderLiteDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcOrderLiteDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderLiteDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderLiteDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderLiteDetailEntity>> GetEntitiesAsync(QualIqcOrderLiteDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.IQCOrderId.HasValue)
            {
                sqlBuilder.Where("IQCOrderId = @IQCOrderId");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderLiteDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcOrderLiteDetailEntity>> GetPagedListAsync(QualIqcOrderLiteDetailPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualIqcOrderLiteDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIqcOrderLiteDetailEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIqcOrderLiteDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_lite_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_iqc_order_lite_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_lite_detail /**where**/  ";

        const string InsertSql = "INSERT INTO qual_iqc_order_lite_detail(`Id`, `SiteId`, `IQCOrderId`, `MaterialId`, `MaterialReceiptDetailId`, `IsQualified`, `HandMethod`, `ProcessedBy`, `ProcessedOn`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, UnQualifiedQty) VALUES (@Id, @SiteId, @IQCOrderId, @MaterialId, @MaterialReceiptDetailId, @IsQualified, @HandMethod, @ProcessedBy, @ProcessedOn, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @UnQualifiedQty) ";

        const string UpdateSql = "UPDATE qual_iqc_order_lite_detail SET UnQualifiedQty = @UnQualifiedQty, IsQualified = @IsQualified, HandMethod = @HandMethod, ProcessedBy = @ProcessedBy, ProcessedOn = @ProcessedOn, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_iqc_order_lite_detail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_iqc_order_lite_detail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_iqc_order_lite_detail WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_iqc_order_lite_detail WHERE Id IN @Ids ";

    }
}
