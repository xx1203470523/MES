using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（废成品入库明细）
    /// </summary>
    public partial class ManuWasteProductsReceiptRecordDetailRepository : BaseRepository, IManuWasteProductsReceiptRecordDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuWasteProductsReceiptRecordDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuWasteProductsReceiptRecordDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuWasteProductsReceiptRecordDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuWasteProductsReceiptRecordDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuWasteProductsReceiptRecordDetailEntity> entities)
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
        public async Task<ManuWasteProductsReceiptRecordDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuWasteProductsReceiptRecordDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuWasteProductsReceiptRecordDetailEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuWasteProductsReceiptRecordDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuWasteProductsReceiptRecordDetailEntity>> GetEntitiesAsync(ManuWasteProductsReceiptRecordDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuWasteProductsReceiptRecordDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuWasteProductsReceiptRecordDetailEntity>> GetPagedListAsync(ManuWasteProductsReceiptRecordDetailPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuWasteProductsReceiptRecordDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuWasteProductsReceiptRecordDetailEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuWasteProductsReceiptRecordDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_waste_products_receipt_record_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_waste_products_receipt_record_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_waste_products_receipt_record_detail /**where**/  ";

        const string InsertSql = "INSERT INTO manu_waste_products_receipt_record_detail(  `Id`, `ProductReceiptId`, `MaterialCode`, `MaterialName`, `Unit`, `Qty`, `Type`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @ProductReceiptId, @MaterialCode, @MaterialName, @Unit, @Qty, @Type, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO manu_waste_products_receipt_record_detail(  `Id`, `ProductReceiptId`, `MaterialCode`, `MaterialName`, `Unit`, `Qty`, `Type`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @ProductReceiptId, @MaterialCode, @MaterialName, @Unit, @Qty, @Type, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE manu_waste_products_receipt_record_detail SET   ProductReceiptId = @ProductReceiptId, MaterialCode = @MaterialCode, MaterialName = @MaterialName, Unit = @Unit, Qty = @Qty, Type = @Type, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_waste_products_receipt_record_detail SET   ProductReceiptId = @ProductReceiptId, MaterialCode = @MaterialCode, MaterialName = @MaterialName, Unit = @Unit, Qty = @Qty, Type = @Type, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_waste_products_receipt_record_detail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_waste_products_receipt_record_detail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_waste_products_receipt_record_detail WHERE ProductReceiptId = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_waste_products_receipt_record_detail WHERE ProductReceiptId IN @Ids  order by CreatedOn desc";

    }
}
