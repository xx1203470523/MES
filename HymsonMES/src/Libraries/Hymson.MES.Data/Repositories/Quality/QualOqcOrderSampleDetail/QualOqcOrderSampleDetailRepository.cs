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
    /// 仓储（OQC检验单检验样本明细）
    /// </summary>
    public partial class QualOqcOrderSampleDetailRepository : BaseRepository, IQualOqcOrderSampleDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualOqcOrderSampleDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualOqcOrderSampleDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualOqcOrderSampleDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualOqcOrderSampleDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新样品明细
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateSampleDetailAsync(QualOqcOrderSampleDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSampleDetailSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualOqcOrderSampleDetailEntity> entities)
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
        public async Task<QualOqcOrderSampleDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualOqcOrderSampleDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderSampleDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderSampleDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderSampleDetailEntity>> GetEntitiesAsync(QualOqcOrderSampleDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (query.OQCOrderId != null) {
                sqlBuilder.Where("OQCOrderId=@OQCOrderId");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcOrderSampleDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcOrderSampleDetailEntity>> GetPagedListAsync(QualOqcOrderSampleDetailPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.OQCOrderId != null) {
                sqlBuilder.Where("OQCOrderId = @OQCOrderId");
            }

            if (pagedQuery.OQCOrderSampleIds != null&& pagedQuery.OQCOrderSampleIds.Any())
            {
                sqlBuilder.Where("OQCOrderSampleId IN @OQCOrderSampleIds");
            }

            if (pagedQuery.GroupDetailSnapshootIds != null && pagedQuery.GroupDetailSnapshootIds.Any())
            {
                sqlBuilder.Where("GroupDetailSnapshootId IN @GroupDetailSnapshootIds");
            }

            if (pagedQuery.IsQualified != null) {
                sqlBuilder.Where("IsQualified = @IsQualified");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualOqcOrderSampleDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualOqcOrderSampleDetailEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualOqcOrderSampleDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_order_sample_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_oqc_order_sample_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_order_sample_detail /**where**/  ";

        const string InsertSql = "INSERT INTO qual_oqc_order_sample_detail(  `Id`, `SiteId`, `OQCOrderId`, `OQCOrderSampleId`, `GroupDetailSnapshootId`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @OQCOrderId, @OQCOrderSampleId, @GroupDetailSnapshootId, @InspectionValue, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_oqc_order_sample_detail(  `Id`, `SiteId`, `OQCOrderId`, `OQCOrderSampleId`, `GroupDetailSnapshootId`, `InspectionValue`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @OQCOrderId, @OQCOrderSampleId, @GroupDetailSnapshootId, @InspectionValue, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_oqc_order_sample_detail SET   SiteId = @SiteId, OQCOrderId = @OQCOrderId, OQCOrderSampleId = @OQCOrderSampleId, GroupDetailSnapshootId = @GroupDetailSnapshootId, InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_oqc_order_sample_detail SET   SiteId = @SiteId, OQCOrderId = @OQCOrderId, OQCOrderSampleId = @OQCOrderSampleId, GroupDetailSnapshootId = @GroupDetailSnapshootId, InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateSampleDetailSql = "UPDATE qual_oqc_order_sample_detail SET   InspectionValue = @InspectionValue, IsQualified = @IsQualified, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_oqc_order_sample_detail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_oqc_order_sample_detail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_oqc_order_sample_detail WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_oqc_order_sample_detail WHERE Id IN @Ids ";

    }
}
