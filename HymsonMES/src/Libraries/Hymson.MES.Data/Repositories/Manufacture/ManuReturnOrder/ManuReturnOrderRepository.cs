using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuReturnOrder.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（生产退料单）
    /// </summary>
    public partial class ManuReturnOrderRepository : BaseRepository, IManuReturnOrderRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuReturnOrderRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuReturnOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuReturnOrderEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuReturnOrderEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuReturnOrderEntity> entities)
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
        public async Task<ManuReturnOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuReturnOrderEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReturnOrderEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuReturnOrderEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuReturnOrderEntity>> GetEntitiesAsync(ManuReturnOrderQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("WorkOrderId = @WorkOrderId");
            }

            if (query.Type.HasValue)
            {
                sqlBuilder.Where("Type = @Type");
            }

            if (!string.IsNullOrWhiteSpace(query.ReturnOrderCode))
            {
                query.ReturnOrderCode = $"%{query.ReturnOrderCode}%";
                sqlBuilder.Where("ReturnOrderCode LIKE @ReturnOrderCode");
            }

            if (!string.IsNullOrWhiteSpace(query.ReturnOrderCodeValue))
            {
                sqlBuilder.Where("ReturnOrderCode = @ReturnOrderCodeValue");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuReturnOrderEntity>(template.RawSql, query);
        }


        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuReturnOrderEntity> GetSingleEntityAsync(ManuReturnOrderSingleQuery query)
        {
            string whereSql = string.Empty;
            if (string.IsNullOrEmpty(query.ReturnOrderCode) == false)
            {
                whereSql = $" and ReturnOrderCode = '{query.ReturnOrderCode}' ";
            }

            string sql = $@"
                select * 
                from manu_return_order mro 
                where IsDeleted = 0
                {whereSql}
                and SiteId = {query.SiteId}
                order by id desc
            ";

            //var sqlBuilder = new SqlBuilder();
            //var template = sqlBuilder.AddTemplate(GetSingleEntitySqlTemplate);
            //sqlBuilder.Select("*");
            //sqlBuilder.OrderBy("Id DESC");
            //sqlBuilder.Where("IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");
            //if (!string.IsNullOrEmpty(query.ReturnOrderCode))
            //{
            //    sqlBuilder.Where(" ReturnOrderCode = @ReturnOrderCode ");
            //}

            using var conn = GetMESDbConnection();
            var dbModel = await conn.QueryFirstOrDefaultAsync<ManuReturnOrderEntity>(sql);
            //if (dbModel == null)
            //{
            //    dbModel = new ManuReturnOrderEntity();
            //    dbModel.Remark = $"{conn.ConnectionString}-{sql}";
            //}
            //dbModel.CompleteCount = 999;

            return dbModel;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuReturnOrderEntity>> GetPagedListAsync(ManuReturnOrderPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.Type.HasValue) sqlBuilder.Where(" Type = @Type ");

            if (!string.IsNullOrWhiteSpace(pagedQuery.ReturnOrderCode))
            {
                sqlBuilder.Where(" ReturnOrderCode = @ReturnOrderCode ");
            }

            if (pagedQuery.SourceWorkOrderIds != null) sqlBuilder.Where(" WorkOrderId IN @SourceWorkOrderIds ");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuReturnOrderEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuReturnOrderEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据Id批量更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuReturnOrderStatusByIdRangeAsync(IEnumerable<UpdateManuReturnOrderStatusByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateManuReturnOrderStatusByIdSql, commands);
        }

        /// <summary>
        /// 根据Id更新状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuReturnOrderStatusByIdAsync(UpdateManuReturnOrderStatusByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateManuReturnOrderStatusByIdSql, command);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuReturnOrderRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_return_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_return_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_return_order /**where**/  ";

        const string GetSingleEntitySqlTemplate = @"SELECT /**select**/ FROM manu_return_order /**where**/ LIMIT 1 ";
        const string InsertSql = "INSERT INTO manu_return_order(  `Id`, `ReturnOrderCode`, `WorkOrderId`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, ReturnWarehouseId, `ReceiveWarehouseId`, `Warehouse`) VALUES (  @Id, @ReturnOrderCode, @WorkOrderId, @Type, @Status,  @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @ReturnWarehouseId, @ReceiveWarehouseId, @Warehouse) ";
        const string InsertsSql = "INSERT INTO manu_return_order(  `Id`, `ReturnOrderCode`, `WorkOrderId`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, ReturnWarehouseId, `ReceiveWarehouseId`, `Warehouse`) VALUES (  @Id, @ReturnOrderCode, @WorkOrderId, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @ReturnWarehouseId, @ReceiveWarehouseId, @Warehouse) ";

        const string UpdateSql = "UPDATE manu_return_order SET ReturnOrderCode = @ReturnOrderCode, WorkOrderId = @WorkOrderId, Type = @Type, Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, ReturnWarehouseId = @ReturnWarehouseId, ReceiveWarehouseId = @ReceiveWarehouseId, CompleteCount = @CompleteCount WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_return_order SET ReturnOrderCode = @ReturnOrderCode, WorkOrderId = @WorkOrderId, Type = @Type, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, ReturnWarehouseId = @ReturnWarehouseId, ReceiveWarehouseId = @ReceiveWarehouseId , CompleteCount = @CompleteCount WHERE Id = @Id ";
        const string UpdateManuReturnOrderStatusByIdSql = "UPDATE manu_return_order SET Status = @Status, CompleteCount = @CompleteCount, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE manu_return_order SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_return_order SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_return_order WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_return_order WHERE Id IN @Ids ";

    }
}
