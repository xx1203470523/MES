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
    /// 仓储（iqc检验单）
    /// </summary>
    public partial class QualIqcOrderLiteRepository : BaseRepository, IQualIqcOrderLiteRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIqcOrderLiteRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIqcOrderLiteEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIqcOrderLiteEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIqcOrderLiteEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIqcOrderLiteEntity> entities)
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
        public async Task<QualIqcOrderLiteEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcOrderLiteEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderLiteEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderLiteEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<QualIqcOrderLiteEntity> GetEntityAsync(QualIqcOrderLiteQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitySqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.SupplierId.HasValue)
            {
                sqlBuilder.Where("SupplierId = @SupplierId");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (query.StatusArr != null && query.StatusArr.Any())
            {
                sqlBuilder.Where("Status IN @StatusArr");
            }
            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcOrderLiteEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderLiteEntity>> GetEntitiesAsync(QualIqcOrderLiteQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            //if (query.MaterialId.HasValue)
            //{
            //    sqlBuilder.Where("MaterialId = @MaterialId");
            //}
            if (query.MaterialReceiptId.HasValue)
            {
                sqlBuilder.Where("MaterialReceiptId = @MaterialReceiptId");
            }
            if (query.SupplierId.HasValue)
            {
                sqlBuilder.Where("SupplierId = @SupplierId");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (query.StatusArr != null && query.StatusArr.Any())
            {
                sqlBuilder.Where("Status IN @StatusArr");
            }
            //if (query.MaterialReceiptDetailIds != null && query.MaterialReceiptDetailIds.Any())
            //{
            //    sqlBuilder.Where("MaterialReceiptDetailId IN @MaterialReceiptDetailIds");
            //}

            // 查询条数
            sqlBuilder.AddParameters(new { MaxRows = query.MaxRows });

            // 排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderLiteEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcOrderLiteEntity>> GetPagedListAsync(QualIqcOrderLitePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy(string.IsNullOrWhiteSpace(pagedQuery.Sorting) ? "CreatedOn DESC" : pagedQuery.Sorting);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.InspectionOrder))
            {
                pagedQuery.InspectionOrder = $"%{pagedQuery.InspectionOrder}%";
                sqlBuilder.Where(" InspectionOrder LIKE @InspectionOrder ");
            }
            if (pagedQuery.SupplierIds != null) sqlBuilder.Where(" SupplierId IN @SupplierIds ");
            if (pagedQuery.ReceiptIds != null) sqlBuilder.Where(" MaterialReceiptId IN @ReceiptIds ");
            if (pagedQuery.Status.HasValue) sqlBuilder.Where("Status = @Status");
            if (pagedQuery.IsQualified.HasValue) sqlBuilder.Where("IsQualified = @IsQualified");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualIqcOrderLiteEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIqcOrderLiteEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIqcOrderLiteRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_lite /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_iqc_order_lite /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_lite /**where**/ /**orderby**/ LIMIT @MaxRows ";
        const string GetEntitySqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_lite /**where**/ /**orderby**/ LIMIT 1 ";

        const string InsertSql = "INSERT INTO qual_iqc_order_lite(`Id`, `SiteId`, `InspectionOrder`, `MaterialReceiptId`, `SupplierId`, `Status`, `IsQualified`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @MaterialReceiptId, @SupplierId, @Status, @IsQualified, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_iqc_order_lite(`Id`, `SiteId`, `InspectionOrder`, `MaterialReceiptId`, `SupplierId`, `Status`, `IsQualified`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @MaterialReceiptId, @SupplierId, @Status, @IsQualified, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_iqc_order_lite SET Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_iqc_order_lite SET Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_iqc_order_lite SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_iqc_order_lite SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_iqc_order_lite WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_iqc_order_lite WHERE Id IN @Ids ";

    }
}
