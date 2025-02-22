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
    /// 仓储（IQC检验类型）
    /// </summary>
    public partial class QualIqcOrderTypeRepository : BaseRepository, IQualIqcOrderTypeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIqcOrderTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIqcOrderTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIqcOrderTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIqcOrderTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIqcOrderTypeEntity> entities)
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
        public async Task<QualIqcOrderTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcOrderTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderTypeEntity>> GetByOrderIdAsync(long orderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderTypeEntity>(GetByOrderIdSql, new { IQCOrderId = orderId });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderTypeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderTypeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderTypeEntity>> GetEntitiesAsync(QualIqcOrderTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderTypeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcOrderTypeEntity>> GetPagedListAsync(QualIqcOrderTypePagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualIqcOrderTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIqcOrderTypeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIqcOrderTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_iqc_order_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_type /**where**/  ";

        const string InsertSql = "INSERT INTO qual_iqc_order_type(  `Id`, `IQCOrderId`, `Type`, `VerificationLevel`, `AcceptanceLevel`, `SampleQty`, `CheckedQty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @IQCOrderId, @Type, @VerificationLevel, @AcceptanceLevel, @SampleQty, @CheckedQty, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_iqc_order_type(  `Id`, `IQCOrderId`, `Type`, `VerificationLevel`, `AcceptanceLevel`, `SampleQty`, `CheckedQty`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @IQCOrderId, @Type, @VerificationLevel, @AcceptanceLevel, @SampleQty, @CheckedQty, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_iqc_order_type SET   IQCOrderId = @IQCOrderId, Type = @Type, VerificationLevel = @VerificationLevel, AcceptanceLevel = @AcceptanceLevel, SampleQty = @SampleQty, CheckedQty = @CheckedQty, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_iqc_order_type SET   IQCOrderId = @IQCOrderId, Type = @Type, VerificationLevel = @VerificationLevel, AcceptanceLevel = @AcceptanceLevel, SampleQty = @SampleQty, CheckedQty = @CheckedQty, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_iqc_order_type SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_iqc_order_type SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_iqc_order_type WHERE Id = @Id ";
        const string GetByOrderIdSql = @"SELECT * FROM qual_iqc_order_type WHERE IQCOrderId = @IQCOrderId ";
        const string GetByIdsSql = @"SELECT * FROM qual_iqc_order_type WHERE Id IN @Ids ";

    }
}
