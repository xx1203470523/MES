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
    /// 仓储（FQC检验单附件）
    /// </summary>
    public partial class QualFqcOrderAttachmentRepository : BaseRepository, IQualFqcOrderAttachmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualFqcOrderAttachmentRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcOrderAttachmentEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualFqcOrderAttachmentEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcOrderAttachmentEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualFqcOrderAttachmentEntity> entities)
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
        public async Task<QualFqcOrderAttachmentEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcOrderAttachmentEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderAttachmentEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderAttachmentEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderAttachmentEntity>> GetByOrderIdAsync(long orderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderAttachmentEntity>(GetByOrderIdSql, new { FQCOrderId = orderId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderAttachmentEntity>> GetEntitiesAsync(QualFqcOrderAttachmentQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderAttachmentEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcOrderAttachmentEntity>> GetPagedListAsync(QualFqcOrderAttachmentPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualFqcOrderAttachmentEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcOrderAttachmentEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualFqcOrderAttachmentRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_order_attachment /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_fqc_order_attachment /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_order_attachment /**where**/  ";

        const string InsertSql = "INSERT INTO qual_fqc_order_attachment(  `Id`, `SiteId`, `FQCOrderId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @FQCOrderId, @AttachmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_fqc_order_attachment(  `Id`, `SiteId`, `FQCOrderId`, `AttachmentId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @FQCOrderId, @AttachmentId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_fqc_order_attachment SET   SiteId = @SiteId, FQCOrderId = @FQCOrderId, AttachmentId = @AttachmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_fqc_order_attachment SET   SiteId = @SiteId, FQCOrderId = @FQCOrderId, AttachmentId = @AttachmentId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_fqc_order_attachment SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_fqc_order_attachment SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_fqc_order_attachment WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_fqc_order_attachment WHERE Id IN @Ids ";

        const string GetByOrderIdSql = @"SELECT * FROM qual_fqc_order_attachment WHERE IsDeleted = 0 AND FQCOrderId = @FQCOrderId ";

    }
}
