using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.QualFqcOrderSample.View;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（FQC检验单样品）
    /// </summary>
    public partial class QualFqcOrderSampleRepository : BaseRepository, IQualFqcOrderSampleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualFqcOrderSampleRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcOrderSampleEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualFqcOrderSampleEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcOrderSampleEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualFqcOrderSampleEntity> entities)
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
        public async Task<QualFqcOrderSampleEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcOrderSampleEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderSampleEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderSampleEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcOrderSampleEntity>> GetEntitiesAsync(QualFqcOrderSampleQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.FQCOrderId.HasValue)
            {
                sqlBuilder.Where("FQCOrderId = @FQCOrderId");
            }
            if (!string.IsNullOrWhiteSpace(query.Barcode))
            {
                sqlBuilder.Where("Barcode = @Barcode");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcOrderSampleEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 样品表查询检测单
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SampleFqcOrderView>> GetEntitiesByDetailBacodeAsync(IEnumerable<string> Barcodes)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<SampleFqcOrderView>(GetFqcOrder, new { Barcodes = Barcodes });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcOrderSampleEntity>> GetPagedListAsync(QualFqcOrderSamplePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.FQCOrderIds != null && pagedQuery.FQCOrderIds.Any())
            {
                sqlBuilder.Where("FQCOrderId IN @FQCOrderIds");
            }

            if (pagedQuery.Barcode != null)
            {
                sqlBuilder.Where("Barcode = @Barcode");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualFqcOrderSampleEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcOrderSampleEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualFqcOrderSampleRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_order_sample /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_fqc_order_sample /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_order_sample /**where**/  ";

        const string InsertSql = "INSERT INTO qual_fqc_order_sample(  `Id`, `SiteId`, `FQCOrderId`, `Barcode`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @FQCOrderId, @Barcode, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_fqc_order_sample(  `Id`, `SiteId`, `FQCOrderId`, `Barcode`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @FQCOrderId, @Barcode, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_fqc_order_sample SET   SiteId = @SiteId, FQCOrderId = @FQCOrderId, Barcode = @Barcode, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_fqc_order_sample SET   SiteId = @SiteId, FQCOrderId = @FQCOrderId, Barcode = @Barcode, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_fqc_order_sample SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_fqc_order_sample SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_fqc_order_sample WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_fqc_order_sample WHERE Id IN @Ids ";

        const string GetFqcOrder = @"select qfo.*,sa.Barcode from qual_fqc_order_sample sa
                                            right join qual_fqc_order qfo on sa.FQCOrderId =qfo.Id 
                                            where Barcode in @Barcodes ";

    }
}
