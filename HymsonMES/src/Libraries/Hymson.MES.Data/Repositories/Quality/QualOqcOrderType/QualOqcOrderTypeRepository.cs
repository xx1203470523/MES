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
    /// 仓储（OQC检验类型）
    /// </summary>
    public partial class QualOqcOrderTypeRepository : BaseRepository, IQualOqcOrderTypeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualOqcOrderTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualOqcOrderTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualOqcOrderTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualOqcOrderTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualOqcOrderTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 更新已检数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateCheckedQtyAsync(QualOqcOrderTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesCheckedQtySql, entity);
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
        public async Task<QualOqcOrderTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualOqcOrderTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderTypeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderTypeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据oqcID获取数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderTypeEntity>> GetByOQCOrderIdAsync(long orderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderTypeEntity>(GetByOrderIdSql, new { IQCOrderId = orderId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderTypeEntity>> GetEntitiesAsync(QualOqcOrderTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            //sqlBuilder.Where("SiteId=@SiteId");

            if (query.OQCOrderId.HasValue) {
                sqlBuilder.Where("OQCOrderId = @OQCOrderId");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderTypeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcOrderTypeEntity>> GetPagedListAsync(QualOqcOrderTypePagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualOqcOrderTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualOqcOrderTypeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询Entity
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<QualOqcOrderTypeEntity> GetEntityAsync(QualOqcOrderTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            //sqlBuilder.Where("SiteId=@SiteId");

            if (query.OQCOrderId.HasValue)
            {
                sqlBuilder.Where("OQCOrderId = @OQCOrderId");
            }

            if (query.InspectionType != null) {
                sqlBuilder.Where("InspectionType = @InspectionType");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualOqcOrderTypeEntity>(template.RawSql, query);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualOqcOrderTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_order_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_oqc_order_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_order_type /**where**/  ";

        const string InsertSql = "INSERT INTO qual_oqc_order_type(  `Id`, `SiteId`, `OQCOrderId`, `InspectionType`, `VerificationLevel`, `AcceptanceLevel`, `SampleQty`, `CheckedQty`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @OQCOrderId, @InspectionType, @VerificationLevel, @AcceptanceLevel, @SampleQty, @CheckedQty, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_oqc_order_type(  `Id`, `SiteId`, `OQCOrderId`, `InspectionType`, `VerificationLevel`, `AcceptanceLevel`, `SampleQty`, `CheckedQty`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @OQCOrderId, @InspectionType, @VerificationLevel, @AcceptanceLevel, @SampleQty, @CheckedQty, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_oqc_order_type SET   SiteId = @SiteId, OQCOrderId = @OQCOrderId, InspectionType = @InspectionType, VerificationLevel = @VerificationLevel, AcceptanceLevel = @AcceptanceLevel, SampleQty = @SampleQty, CheckedQty = @CheckedQty, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_oqc_order_type SET   SiteId = @SiteId, OQCOrderId = @OQCOrderId, InspectionType = @InspectionType, VerificationLevel = @VerificationLevel, AcceptanceLevel = @AcceptanceLevel, SampleQty = @SampleQty, CheckedQty = @CheckedQty, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

#if DM
        const string UpdatesCheckedQtySql = "UPDATE qual_oqc_order_type SET CheckedQty = CheckedQty + CAST(@CheckedQty AS DECIMAL), IsQualified = @IsQualified, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
#else
        const string UpdatesCheckedQtySql = "UPDATE qual_oqc_order_type SET CheckedQty = CheckedQty + @CheckedQty, IsQualified = @IsQualified, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
#endif

        const string DeleteSql = "UPDATE qual_oqc_order_type SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_oqc_order_type SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_oqc_order_type WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_oqc_order_type WHERE Id IN @Ids ";
        const string GetByOrderIdSql = @"SELECT * FROM qual_oqc_order_type WHERE OQCOrderId = @OQCOrderId ";

    }
}
