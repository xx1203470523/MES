using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（OQC检验单）
    /// </summary>
    public partial class QualOqcOrderRepository : BaseRepository, IQualOqcOrderRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualOqcOrderRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualOqcOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualOqcOrderEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualOqcOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(QualOqcOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, entity);
        }

        /// <summary>
        /// 更新状态和是否合格
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAndIsQualifiedAsync(QualOqcOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusAndIsQualifiedSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualOqcOrderEntity> entities)
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
        public async Task<QualOqcOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualOqcOrderEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderEntity>> GetEntitiesAsync(QualOqcOrderQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.ShipmentMaterialIds != null && query.ShipmentMaterialIds.Any())
            {
                sqlBuilder.Where("ShipmentMaterialId IN @ShipmentMaterialIds");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcOrderEntity>> GetPagedListAsync(QualOqcOrderPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.InspectionOrderLike)) {
                pagedQuery.InspectionOrderLike = $"%{pagedQuery.InspectionOrderLike}%";
                sqlBuilder.Where("InspectionOrder LIKE @InspectionOrderLike");
            }

            if (pagedQuery.Status != null) {
                sqlBuilder.Where("Status = @Status");
            }

            if (pagedQuery.MaterialIds != null && pagedQuery.MaterialIds.Any()) {
                sqlBuilder.Where("MaterialId IN @MaterialIds");
            }

            if (pagedQuery.CustomerIds != null && pagedQuery.CustomerIds.Any())
            {
                sqlBuilder.Where("CustomerId IN @CustomerIds");
            }

            if (pagedQuery.ShipmentMaterialIds != null && pagedQuery.ShipmentMaterialIds.Any()) {
                sqlBuilder.Where("ShipmentMaterialId IN @ShipmentMaterialIds");
            }

            if (pagedQuery.IsQualified != null)
            {
                sqlBuilder.Where("IsQualified = @IsQualified");
            }

            if (pagedQuery.Ids != null && pagedQuery.Ids.Any()) {
                sqlBuilder.Where("Id IN @Ids");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualOqcOrderEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualOqcOrderEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualOqcOrderRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_oqc_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_order /**where**/  ";

        const string InsertSql = "INSERT INTO qual_oqc_order(  `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `MaterialId`, `CustomerId`, `ShipmentMaterialId`, `ShipmentQty`, `AcceptanceLevel`, `Status`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @GroupSnapshootId, @MaterialId, @CustomerId, @ShipmentMaterialId, @ShipmentQty, @AcceptanceLevel, @Status, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_oqc_order(  `Id`, `SiteId`, `InspectionOrder`, `GroupSnapshootId`, `MaterialId`, `CustomerId`, `ShipmentMaterialId`, `ShipmentQty`, `AcceptanceLevel`, `Status`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @GroupSnapshootId, @MaterialId, @CustomerId, @ShipmentMaterialId, @ShipmentQty, @AcceptanceLevel, @Status, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_oqc_order SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, GroupSnapshootId = @GroupSnapshootId, MaterialId = @MaterialId, CustomerId = @CustomerId, ShipmentMaterialId = @ShipmentMaterialId, ShipmentQty = @ShipmentQty, AcceptanceLevel = @AcceptanceLevel, Status = @Status, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_oqc_order SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, GroupSnapshootId = @GroupSnapshootId, MaterialId = @MaterialId, CustomerId = @CustomerId, ShipmentMaterialId = @ShipmentMaterialId, ShipmentQty = @ShipmentQty, AcceptanceLevel = @AcceptanceLevel, Status = @Status, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE qual_oqc_order SET  Status = @Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateStatusAndIsQualifiedSql = "UPDATE qual_oqc_order SET  Status = @Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn,IsQualified = @IsQualified WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_oqc_order SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_oqc_order SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids AND Status=1";

        const string GetByIdSql = @"SELECT * FROM qual_oqc_order WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_oqc_order WHERE Id IN @Ids ";

    }
}
