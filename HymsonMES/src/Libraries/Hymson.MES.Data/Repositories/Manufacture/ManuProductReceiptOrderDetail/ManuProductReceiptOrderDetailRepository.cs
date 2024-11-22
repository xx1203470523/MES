using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（工单完工入库明细）
    /// </summary>
    public partial class ManuProductReceiptOrderDetailRepository : BaseRepository, IManuProductReceiptOrderDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuProductReceiptOrderDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuProductReceiptOrderDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductReceiptOrderDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuProductReceiptOrderDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuProductReceiptOrderDetailEntity> entities)
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
        public async Task<ManuProductReceiptOrderDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuProductReceiptOrderDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductReceiptOrderDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuProductReceiptOrderDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductReceiptOrderDetailEntity>> GetEntitiesAsync(ManuProductReceiptOrderDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuProductReceiptOrderDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductReceiptOrderDetailEntity>> GetPagedListAsync(ManuProductReceiptOrderDetailPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuProductReceiptOrderDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductReceiptOrderDetailEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductReceiptOrderDetailEntity>> GetByProductReceiptIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuProductReceiptOrderDetailEntity>(GetByProductReceiptIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 数据集查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductReceiptOrderDetailEntity>> GetListAsync(QueryManuProductReceiptOrderDetail query)
        {
            var sqlBuilder = new SqlBuilder();

            var templateData = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.SFCs != null && query.SFCs.Any())
            {
                sqlBuilder.Where("Sfc IN @SFCs");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();

            return await conn.QueryAsync<ManuProductReceiptOrderDetailEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 数据集查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductReceiptOrderDetailEntity>> GetEntitiesWithoutCancelAsync(QueryManuProductReceiptOrderDetail query)
        {
            var sqlBuilder = new SqlBuilder();

            var templateData = sqlBuilder.AddTemplate(GetFormulaListSqlTemplate);
            sqlBuilder.Where("t1.IsDeleted = 0");
            sqlBuilder.Where("t1.SiteId = @SiteId");

            // ProductReceiptStatusEnum
            sqlBuilder.Where("t2.ProductReceiptId NOT IN (SELECT Id FROM manu_product_receipt_order WHERE Status IN (2, 4, 5, 7)) ");

            if (query.SFCs != null && query.SFCs.Any())
            {
                sqlBuilder.Where("t2.Sfc IN @SFCs");
            }

            if (query.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("t1.WorkOrderCode= @WorkOrderId");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();

            return await conn.QueryAsync<ManuProductReceiptOrderDetailEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportBoxResultDto>> GetReportPagedInfoAsync(ReportBoxQueryDto param)
        {
            string whereSql = string.Empty;
            if (string.IsNullOrEmpty(param.OrderCode) == false)
            {
                whereSql += $" and t4.OrderCode like '%{param.OrderCode}%' ";
            }
            if (string.IsNullOrEmpty(param.WarehouseCode) == false)
            {
                whereSql += $" and t1.WarehouseCode like '%{param.WarehouseCode}%' ";
            }
            if (string.IsNullOrEmpty(param.MaterialCode) == false)
            {
                whereSql += $" and t3.MaterialCode like '%{param.MaterialCode}%' ";
            }
            if (string.IsNullOrEmpty(param.MaterialName) == false)
            {
                whereSql += $" and t3.MaterialName like '%{param.MaterialName}%' ";
            }
            if (param.InWmsData != null && param.InWmsData.Count() == 2)
            {
                whereSql += $" and t1.CreatedOn > '{((DateTime)param.InWmsData[0]).ToString("yyyy-MM-dd HH:mm:ss")}' and t1.CreatedOn < '{((DateTime)param.InWmsData[1]).ToString("yyyy-MM-dd HH:mm:ss")}' ";
            }
            if (param.Status != null)
            {
                whereSql += $" and t2.Status  = {(int)param.Status} ";
            }
            if (string.IsNullOrEmpty(param.ContaineCode) == false)
            {
                whereSql += $" and t1.ContaineCode like '%{param.ContaineCode}%' ";
            }
            if (string.IsNullOrEmpty(param.CreatedBy) == false)
            {
                whereSql += $" and t1.CreatedBy like '%{param.CreatedBy}%' ";
            }

            string sql = $@"
                select t1.CreatedOn InWmsData, t4.OrderCode, t2.CompletionOrderCode ,t1.Sfc , t1.ContaineCode , t1.MaterialName ,t1.MaterialCode , 
	                t3.Specifications ,t1.Unit ,t4.Qty , t1.Qty InWmsQty, t5.WorkPlanCode , t1.WarehouseCode , t1.CreatedBy ,t2.Status 
                from manu_product_receipt_order_detail t1
                inner join manu_product_receipt_order t2 on t1.ProductReceiptId = t2.Id and t2.IsDeleted = 0
                inner join proc_material t3 on t3.MaterialCode = t1.MaterialCode and t3.IsDeleted = 0 and t3.SiteId = t1.SiteId 
                inner join plan_work_order t4 on t4.Id = t2.WorkOrderCode and t4.IsDeleted = 0
                inner join plan_work_plan t5 on t5.Id = t4.WorkPlanId and t5.IsDeleted = 0
                where t1.IsDeleted = 0
                {whereSql}
                order by t1.CreatedOn desc
                limit {(param.PageIndex - 1) * param.PageSize},{param.PageSize}
            ";

            string countSql = $@"
                select count(*)
                from manu_product_receipt_order_detail t1
                inner join manu_product_receipt_order t2 on t1.ProductReceiptId = t2.Id and t2.IsDeleted = 0
                inner join proc_material t3 on t3.MaterialCode = t1.MaterialCode and t3.IsDeleted = 0 and t3.SiteId = t1.SiteId 
                inner join plan_work_order t4 on t4.Id = t2.WorkOrderCode and t4.IsDeleted = 0
                inner join plan_work_plan t5 on t5.Id = t4.WorkPlanId and t5.IsDeleted = 0
                where t1.IsDeleted = 0
                {whereSql}
            ";

            using var conn = GetMESDbConnection();
            var dbListTask = conn.QueryAsync<ReportBoxResultDto>(sql);
            var totalCountTask = conn.ExecuteScalarAsync<int>(countSql);
            var dbList = await dbListTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ReportBoxResultDto>(dbList, param.PageIndex, param.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuProductReceiptOrderDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_product_receipt_order_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_product_receipt_order_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_product_receipt_order_detail /**where**/  ";

        const string InsertSql = "INSERT INTO manu_product_receipt_order_detail(  `Id`, `ProductReceiptId`, `MaterialCode`, `MaterialName`, `Sfc`, `Unit`, `Qty`, `WarehouseId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ContaineCode`, `Batch`, `WarehouseCode`, `StorageStatus`, `CompletionOrderCode`) VALUES (  @Id, @ProductReceiptId, @MaterialCode, @MaterialName, @Sfc, @Unit, @Qty, @WarehouseId, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @ContaineCode,@Batch,@WarehouseCode,@StorageStatus,@CompletionOrderCode) ";
        const string InsertsSql = "INSERT INTO manu_product_receipt_order_detail(  `Id`, `ProductReceiptId`, `MaterialCode`, `MaterialName`, `Sfc`, `Unit`, `Qty`, `WarehouseId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ContaineCode`, `Batch`, `WarehouseCode`, `StorageStatus`, `CompletionOrderCode`) VALUES (  @Id, @ProductReceiptId, @MaterialCode, @MaterialName, @Sfc, @Unit, @Qty, @WarehouseId, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @ContaineCode,@Batch,@WarehouseCode,@StorageStatus,@CompletionOrderCode) ";

        const string UpdateSql = "UPDATE manu_product_receipt_order_detail SET   ProductReceiptId = @ProductReceiptId, MaterialCode = @MaterialCode, MaterialName = @MaterialName, Sfc = @Sfc, Unit = @Unit, Qty = @Qty, WarehouseId = @WarehouseId, Status = @Status,StorageStatus=@StorageStatus, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_product_receipt_order_detail SET   ProductReceiptId = @ProductReceiptId, MaterialCode = @MaterialCode, MaterialName = @MaterialName, Sfc = @Sfc, Unit = @Unit, Qty = @Qty, WarehouseId = @WarehouseId, Status = @Status,StorageStatus=@StorageStatus, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_product_receipt_order_detail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_product_receipt_order_detail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_product_receipt_order_detail WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_product_receipt_order_detail WHERE Id IN @Ids ";

        const string GetByProductReceiptIdsSql = @"SELECT * FROM manu_product_receipt_order_detail WHERE ProductReceiptId IN @Ids order by CreatedOn desc ";

        /// <summary>
        /// 获取配方列表
        /// </summary>
        const string GetFormulaListSqlTemplate = $@"
            select * from manu_product_receipt_order t1
            inner join manu_product_receipt_order_detail t2 
            on t1.Id =t2.ProductReceiptId and t2.IsDeleted = 0
            /**where**/ 
        ";

    }
}
