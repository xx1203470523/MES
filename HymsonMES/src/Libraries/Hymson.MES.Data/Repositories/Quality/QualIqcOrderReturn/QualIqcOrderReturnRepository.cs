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
    public partial class QualIqcOrderReturnRepository : BaseRepository, IQualIqcOrderReturnRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIqcOrderReturnRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIqcOrderReturnEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIqcOrderReturnEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIqcOrderReturnEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIqcOrderReturnEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

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
        public async Task<QualIqcOrderReturnEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcOrderReturnEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderReturnEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcOrderReturnEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<QualIqcOrderReturnEntity> GetEntityAsync(QualIqcOrderReturnQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitySqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
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
            return await conn.QueryFirstOrDefaultAsync<QualIqcOrderReturnEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderReturnEntity>> GetEntitiesAsync(QualIqcOrderReturnQuery query)
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
            if (query.ReturnOrderId.HasValue)
            {
                sqlBuilder.Where("ReturnOrderId = @ReturnOrderId");
            }
            if (query.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
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
            return await conn.QueryAsync<QualIqcOrderReturnEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询，IQC退料检验单，列表查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcOrderReturnEntity>> GetPagedListAsync(QualIqcOrderReturnPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select(" t1.InspectionOrder,t2.ReturnOrderCode,t3.OrderCode,t4.MaterialCode,t4.MaterialName, t1.* ");
            sqlBuilder.OrderBy(string.IsNullOrWhiteSpace(pagedQuery.Sorting) ? " t1.CreatedOn DESC" : pagedQuery.Sorting);
            sqlBuilder.Where(" t1.IsDeleted = 0");
            sqlBuilder.Where(" t1.SiteId = @SiteId");

            sqlBuilder.LeftJoin(" manu_return_order t2 on t1.ReturnOrderId = t2.id ");
            sqlBuilder.LeftJoin(" plan_work_order t3 on t1.WorkOrderId = t3.id ");
            sqlBuilder.LeftJoin(" (SELECT t.IQCOrderId, GROUP_CONCAT(t.MaterialCode SEPARATOR ',') AS MaterialCode, GROUP_CONCAT(t.MaterialName SEPARATOR ',') AS MaterialName FROM (select t.IQCOrderId,p.MaterialCode,p.MaterialName from qual_iqc_order_return_detail t inner JOIN proc_material p on t.MaterialId = p.id) t GROUP BY t.IQCOrderId) t4 on t1.id = t4.IQCOrderId ");

            //检验单号
            if (!string.IsNullOrWhiteSpace(pagedQuery.InspectionOrder))
            {
                pagedQuery.InspectionOrder = $"%{pagedQuery.InspectionOrder}%";
                sqlBuilder.Where(" t1.InspectionOrder LIKE @InspectionOrder ");
            }

            //退料单号的ID
            if (pagedQuery.ReturnOrderIds != null) sqlBuilder.Where(" t1.ReturnOrderId IN @ReturnOrderIds ");

            //工单编码的ID
            if (pagedQuery.WorkOrderIds != null) sqlBuilder.Where(" t1.WorkOrderId IN @WorkOrderIds ");

            if (pagedQuery.Status.HasValue) sqlBuilder.Where(" t1.Status = @Status");
            if (pagedQuery.IsQualified.HasValue) sqlBuilder.Where(" t1.IsQualified = @IsQualified");



            //物料编码
            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where(" t4.MaterialCode LIKE @MaterialCode ");
            }

            //物料名称
            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialName))
            {
                pagedQuery.MaterialName = $"%{pagedQuery.MaterialName}%";
                sqlBuilder.Where(" t4.MaterialName LIKE @MaterialName ");
            }

            // 限定时间
            if (pagedQuery.CreatedOn != null && pagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.CreatedOn[0], DateEnd = pagedQuery.CreatedOn[1] });
                sqlBuilder.Where(" t1.CreatedOn >= @DateStart AND t1.CreatedOn < @DateEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualIqcOrderReturnEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIqcOrderReturnEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIqcOrderReturnRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_return t1 /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_iqc_order_return t1 /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_return /**where**/ /**orderby**/ LIMIT @MaxRows ";
        const string GetEntitySqlTemplate = @"SELECT /**select**/ FROM qual_iqc_order_return /**where**/ /**orderby**/ LIMIT 1 ";

        const string InsertSql = "INSERT INTO qual_iqc_order_return(`Id`, `SiteId`, `InspectionOrder`, `ReturnOrderId`, `WorkOrderId`, `Status`, `IsQualified`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @ReturnOrderId, @WorkOrderId, @Status, @IsQualified, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_iqc_order_return(`Id`, `SiteId`, `InspectionOrder`, `ReturnOrderId`, `WorkOrderId`, `Status`, `IsQualified`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @ReturnOrderId, @WorkOrderId, @Status, @IsQualified, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_iqc_order_return SET Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_iqc_order_return SET Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_iqc_order_return SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_iqc_order_return SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_iqc_order_return WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_iqc_order_return WHERE Id IN @Ids ";

    }
}
