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
    /// 仓储（FQC检验单）
    /// </summary>
    public partial class QualFqcOrderRepository : BaseRepository, IQualFqcOrderRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualFqcOrderRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualFqcOrderEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualFqcOrderEntity> entities)
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
        public async Task<QualFqcOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcOrderEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 依检验单查询
        /// </summary>
        /// <param name="inspectionOrder"></param>
        /// <returns></returns>
        public async Task<QualFqcOrderEntity> GetByInspectionOrderAsync(string inspectionOrder)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcOrderEntity>(GetByCodeSql, new { InspectionOrder = inspectionOrder });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderEntity>(GetByIdsSql, new { Ids = ids });
        }

        public async Task<IEnumerable<QualFqcOrderEntity>> GetByIdsAsync(IEnumerable<long> Ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderEntity>(GetByIdsSql, new { Ids});
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderEntity>> GetEntitiesAsync(QualFqcOrderQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if(query.WorkOrderId != null)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcOrderEntity>> GetPagedListAsync(QualFqcOrderPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");


            if (!string.IsNullOrWhiteSpace(pagedQuery.InspectionOrder)) sqlBuilder.Where(" InspectionOrder LIKE @InspectionOrder ");
            if (pagedQuery.FQCOrderIds != null) sqlBuilder.Where(" Id IN @FQCOrderIds ");
            if (pagedQuery.MaterialIds != null) sqlBuilder.Where(" MaterialId IN @MaterialIds ");
            if (pagedQuery.WorkOrderId != null) sqlBuilder.Where(" WorkOrderId = @WorkOrderId ");
            //if (pagedQuery.MaterialReceiptDetailIds != null) sqlBuilder.Where(" MaterialReceiptDetailId IN @MaterialReceiptDetailIds ");
            if (pagedQuery.Status.HasValue) sqlBuilder.Where("Status = @Status");
            //if (pagedQuery.IsExemptInspection.HasValue) sqlBuilder.Where("IsExemptInspection = @IsExemptInspection");
            if (pagedQuery.IsQualified.HasValue) sqlBuilder.Where("IsQualified = @IsQualified");


            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualFqcOrderEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcOrderEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualFqcOrderRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_fqc_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_order /**where**/  ";

        const string InsertSql = "INSERT INTO qual_fqc_order(  `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `WorkOrderId`, `MaterialId`, `SampleQty`, `Status`, `IsQualified`, `IsPreGenerated`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @GroupSnapshootId, @WorkOrderId, @MaterialId, @SampleQty, @Status, @IsQualified, @IsPreGenerated, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_fqc_order(  `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `WorkOrderId`, `MaterialId`, `SampleQty`, `Status`, `IsQualified`, `IsPreGenerated`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @GroupSnapshootId, @WorkOrderId, @MaterialId, @SampleQty, @Status, @IsQualified, @IsPreGenerated, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_fqc_order SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, GroupSnapshootId = @GroupSnapshootId, WorkOrderId = @WorkOrderId, MaterialId = @MaterialId, SampleQty = @SampleQty, Status = @Status, IsQualified = @IsQualified, IsPreGenerated = @IsPreGenerated, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_fqc_order SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, GroupSnapshootId = @GroupSnapshootId, WorkOrderId = @WorkOrderId, MaterialId = @MaterialId, SampleQty = @SampleQty, Status = @Status, IsQualified = @IsQualified, IsPreGenerated = @IsPreGenerated, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_fqc_order SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_fqc_order SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_fqc_order WHERE Id = @Id ";
        const string GetByCodeSql = @"SELECT * FROM qual_fqc_order WHERE IsDeleted=0 AND InspectionOrder = @InspectionOrder ";

        const string GetByIdsSql = @"SELECT * FROM qual_fqc_order WHERE Id IN @Ids ";

    }
}
