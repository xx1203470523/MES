using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrder.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrderDetail.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（生产退料单明细）
    /// </summary>
    public partial class ManuReturnOrderDetailRepository : BaseRepository, IManuReturnOrderDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuReturnOrderDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuReturnOrderDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuReturnOrderDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuReturnOrderDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuReturnOrderDetailEntity> entities)
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
        public async Task<ManuReturnOrderDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuReturnOrderDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReturnOrderDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuReturnOrderDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReturnOrderDetailEntity>> GetEntitiesAsync(ManuReturnOrderDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.ReturnOrderId.HasValue)
            {
                sqlBuilder.Where(" ReturnOrderId = @ReturnOrderId ");
            }

            if (!string.IsNullOrWhiteSpace(query.MaterialBarCode))
            {
                sqlBuilder.Where(" MaterialBarCode = @MaterialBarCode ");
            }

            if (query.ReturnOrderIds != null && query.ReturnOrderIds.Count() > 0)
            {
                sqlBuilder.Where(" ReturnOrderId in @ReturnOrderIds ");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuReturnOrderDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuReturnOrderDetailEntity>> GetPagedListAsync(ManuReturnOrderDetailPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuReturnOrderDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuReturnOrderDetailEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据Id批量更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuReturnOrderDetailIsReceivedByIdRangeAsync(IEnumerable<UpdateManuReturnOrderDetailIsReceivedByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateManuReturnOrderDetailIsReceivedByIdSql, commands);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuRequistionOrderDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ReportReturnOrderResultDto>> GetReportPagedInfoAsync(ReportReturnOrderQueryDto param)
        {
            string whereSql = string.Empty;
            if (string.IsNullOrEmpty(param.OrderCode) == false)
            {
                whereSql += $" and t3.OrderCode like '%{param.OrderCode}%' ";
            }
            if (string.IsNullOrEmpty(param.Warehouse) == false)
            {
                whereSql += $" and t2.Warehouse like '%{param.Warehouse}%' ";
            }
            if (string.IsNullOrEmpty(param.MaterialCode) == false)
            {
                whereSql += $" and t5.MaterialCode like '%{param.MaterialCode}%' ";
            }
            if (string.IsNullOrEmpty(param.MaterialName) == false)
            {
                whereSql += $" and t5.MaterialName like '%{param.MaterialName}%' ";
            }
            if (param.ReqDate != null && param.ReqDate.Count() == 2)
            {
                whereSql += $" and t1.CreatedOn > '{((DateTime)param.ReqDate[0]).ToString("yyyy-MM-dd HH:mm:ss")}' and t1.CreatedOn < '{((DateTime)param.ReqDate[1]).ToString("yyyy-MM-dd HH:mm:ss")}' ";
            }
            if (param.Status != null)
            {
                whereSql += $" and t2.Status  = {(int)param.Status} ";
            }
            if (string.IsNullOrEmpty(param.CreatedBy) == false)
            {
                whereSql += $" and t1.CreatedBy like '%{param.CreatedBy}%' ";
            }

            string sql = $@"
                select t1.CreatedOn ReturnDate,t3.OrderCode ,t5.MaterialCode ,t5.MaterialName , t5.Specifications ,t6.Name Unit ,
	                t3.Qty OrderQty,t1.Qty ReqQty, t4.WorkPlanCode , t2.Warehouse ,t2.ReturnOrderCode, t1.CreatedBy ,t2.Status ,  t2.ReturnOrderCode  ,t2.`Type` 
                from manu_return_order_detail t1
                inner join manu_return_order t2 on t1.ReturnOrderId  = t2.Id and t2.IsDeleted = 0
                inner join plan_work_order t3 on t3.Id = t2.WorkOrderId and t3.IsDeleted = 0
                inner join plan_work_plan t4 on t4.Id = t3.WorkPlanId and t4.IsDeleted = 0
                inner join proc_material t5 on t5.Id = t1.MaterialId and t5.IsDeleted  = 0
                left join inte_unit t6 on t6.Code = t5.Unit and t6.IsDeleted = 0
                where t1.IsDeleted = 0
                {whereSql}
                order by t1.CreatedOn desc
                limit {(param.PageIndex - 1) * param.PageSize},{param.PageSize}
            ";

            string countSql = $@"
                select count(*)
                from manu_return_order_detail t1
                inner join manu_return_order t2 on t1.ReturnOrderId  = t2.Id and t2.IsDeleted = 0
                inner join plan_work_order t3 on t3.Id = t2.WorkOrderId and t3.IsDeleted = 0
                inner join plan_work_plan t4 on t4.Id = t3.WorkPlanId and t4.IsDeleted = 0
                inner join proc_material t5 on t5.Id = t1.MaterialId and t5.IsDeleted  = 0
                left join inte_unit t6 on t6.Code = t5.Unit and t6.IsDeleted = 0
                where t1.IsDeleted = 0
                {whereSql}
            ";

            using var conn = GetMESDbConnection();
            var dbListTask = conn.QueryAsync<ReportReturnOrderResultDto>(sql);
            var totalCountTask = conn.ExecuteScalarAsync<int>(countSql);
            var dbList = await dbListTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ReportReturnOrderResultDto>(dbList, param.PageIndex, param.PageSize, totalCount);
        }
    
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuReturnOrderDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_return_order_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_return_order_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_return_order_detail /**where**/  ";

        const string InsertSql = "INSERT INTO manu_return_order_detail(`Id`, `ReturnOrderId`, MaterialId,  `MaterialBarCode`, `Batch`, `Qty`, SupplierId,  `ExpirationDate`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `IsReceived`) VALUES (  @Id, @ReturnOrderId, @MaterialId,  @MaterialBarCode, @Batch, @Qty, @SupplierId, @ExpirationDate, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @IsReceived) ";
        const string InsertsSql = "INSERT INTO manu_return_order_detail(`Id`, `ReturnOrderId`, MaterialId,  `MaterialBarCode`, `Batch`, `Qty`, SupplierId,  `ExpirationDate`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `IsReceived`) VALUES (  @Id, @ReturnOrderId, @MaterialId, @MaterialBarCode, @Batch, @Qty, @SupplierId, @ExpirationDate, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @IsReceived) ";

        const string UpdateSql = "UPDATE manu_return_order_detail SET ReturnOrderId = @ReturnOrderId, MaterialBarCode = @MaterialBarCode, Batch = @Batch, Qty = @Qty,  ExpirationDate = @ExpirationDate, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_return_order_detail SET ReturnOrderId = @ReturnOrderId,  MaterialBarCode = @MaterialBarCode, Batch = @Batch, Qty = @Qty,  ExpirationDate = @ExpirationDate, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdateManuReturnOrderDetailIsReceivedByIdSql = "UPDATE manu_return_order_detail SET IsReceived = @IsReceived , UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE manu_return_order_detail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_return_order_detail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_return_order_detail WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_return_order_detail WHERE Id IN @Ids ";

    }
}
