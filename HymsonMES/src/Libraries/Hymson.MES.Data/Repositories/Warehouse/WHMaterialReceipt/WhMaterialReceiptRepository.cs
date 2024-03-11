using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WHMaterialReceipt.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.WHMaterialReceipt
{
    /// <summary>
    /// 仓储（物料收货表）
    /// </summary>
    public partial class WhMaterialReceiptRepository : BaseRepository, IWhMaterialReceiptRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WhMaterialReceiptRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhMaterialReceiptEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhMaterialReceiptEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhMaterialReceiptEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WhMaterialReceiptEntity> entities)
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

        public async Task<int> DeletesDetailByIdAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesDetailByIdSql, new { Ids = ids });
        }

        public async Task<int> InsertDetailAsync(List<WHMaterialReceiptDetailEntity> entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertDetailSql, entity);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialReceiptEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhMaterialReceiptEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialReceiptEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialReceiptEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhMaterialReceiptEntity>> GetEntitiesAsync(WhMaterialReceiptQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            //sqlBuilder.Where("SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(query.ReceiptNum))
            {
                sqlBuilder.Where(" ReceiptNum = @ReceiptNum ");
            }
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhMaterialReceiptEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialReceiptEntity>> GetPagedListAsync(WhMaterialReceiptPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.ReceiptNum))
            {
                sqlBuilder.Where(" ReceiptNum = @ReceiptNum ");
            }

            if (pagedQuery.SupplierId != null)
            {
                sqlBuilder.Where(" SupplierId = @SupplierId ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhMaterialReceiptEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhMaterialReceiptEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        #region 明细
        /// <summary>
        /// 根据receiptId获取明细数据
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WHMaterialReceiptDetailEntity>> GetDetailsByReceiptIdAsync(long receiptId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WHMaterialReceiptDetailEntity>(GetDetailsByReceiptIdSql, new { MaterialReceiptId = receiptId });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WHMaterialReceiptDetailEntity> GetDetailByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WHMaterialReceiptDetailEntity>(GetDetailByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WHMaterialReceiptDetailEntity>> GetDetailsByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WHMaterialReceiptDetailEntity>(GetDetailsByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据收货主表Id明细
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WHMaterialReceiptDetailEntity>> GetDetailsByReceiptIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WHMaterialReceiptDetailEntity>(GetDetailsByReceiptIdsSql, new { Ids = ids });
        }

        
        #endregion

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class WhMaterialReceiptRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM wh_material_receipt /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM wh_material_receipt /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM wh_material_receipt /**where**/  ";

        const string InsertSql = "INSERT INTO wh_material_receipt(  `Id`, `SiteId`, `ReceiptNum`, `SupplierId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ReceiptNum, @SupplierId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO wh_material_receipt(  `Id`, `SiteId`, `ReceiptNum`, `SupplierId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ReceiptNum, @SupplierId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string InsertDetailSql = "INSERT INTO wh_material_receipt_detail(`Id`, `MaterialReceiptId`, `SiteId`, `MaterialId`, `SupplierBatch`, InternalBatch, `PlanQty`,`Qty`, `PlanTime`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES(@Id, @MaterialReceiptId, @SiteId, @MaterialId, @SupplierBatch,@InternalBatch, @PlanQty,@Qty, @PlanTime, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, 0)";

        const string UpdateSql = "UPDATE wh_material_receipt SET   SiteId = @SiteId, ReceiptNum = @ReceiptNum, SupplierId = @SupplierId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE wh_material_receipt SET   SiteId = @SiteId, ReceiptNum = @ReceiptNum, SupplierId = @SupplierId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE wh_material_receipt SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE wh_material_receipt SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeletesDetailByIdSql = "delete from wh_material_receipt_detail WHERE MaterialReceiptId IN @Ids";

        const string GetByIdSql = @"SELECT * FROM wh_material_receipt WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM wh_material_receipt WHERE Id IN @Ids ";



        const string GetDetailsByReceiptIdSql = @"SELECT * FROM wh_material_receipt_detail WHERE MaterialReceiptId = @MaterialReceiptId ";
        const string GetDetailByIdSql = @"SELECT * FROM wh_material_receipt_detail WHERE Id = @Id ";
        const string GetDetailsByIdsSql = @"SELECT * FROM wh_material_receipt_detail WHERE Id IN @Ids ";
        const string GetDetailsByReceiptIdsSql = @"SELECT * FROM wh_material_receipt_detail WHERE MaterialReceiptId IN @Ids "; 


    }
}
