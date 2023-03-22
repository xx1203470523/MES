/*
 *creator: Karl
 *
 *describe: 工单信息表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单信息表仓储
    /// </summary>
    public partial class PlanWorkOrderRepository : IPlanWorkOrderRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public PlanWorkOrderRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoAsync(PlanWorkOrderPagedQuery planWorkOrderPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("wo.IsDeleted=0");

            if (!string.IsNullOrWhiteSpace(planWorkOrderPagedQuery.OrderCode))
            {
                planWorkOrderPagedQuery.OrderCode= $"%{planWorkOrderPagedQuery.OrderCode}%";
                sqlBuilder.Where(" wo.OrderCode like @OrderCode ");
            }
            if (!string.IsNullOrWhiteSpace(planWorkOrderPagedQuery.MaterialCode))
            {
                planWorkOrderPagedQuery.MaterialCode = $"%{planWorkOrderPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" m.MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(planWorkOrderPagedQuery.WorkCenterCode))
            {
                planWorkOrderPagedQuery.WorkCenterCode = $"%{planWorkOrderPagedQuery.WorkCenterCode}%";
                sqlBuilder.Where(" wc.Code like @WorkCenterCode ");
            }
            if (planWorkOrderPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" wo.Status = @Status ");
            }
            if (planWorkOrderPagedQuery.IsLocked.HasValue)
            {
                sqlBuilder.Where(" wo.IsLocked = @IsLocked ");
            }
            if (planWorkOrderPagedQuery.PlanStartTimeS.HasValue)
            {
                sqlBuilder.Where(" wo.PlanStartTime>= @PlanStartTimeS ");
            }
            if (planWorkOrderPagedQuery.PlanStartTimeE.HasValue)
            {
                sqlBuilder.Where(" wo.PlanStartTime< @PlanStartTimeE ");
            }

            var offSet = (planWorkOrderPagedQuery.PageIndex - 1) * planWorkOrderPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planWorkOrderPagedQuery.PageSize });
            sqlBuilder.AddParameters(planWorkOrderPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderEntitiesTask = conn.QueryAsync<PlanWorkOrderListDetailView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planWorkOrderViews = await planWorkOrderEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderListDetailView>(planWorkOrderViews, planWorkOrderPagedQuery.PageIndex, planWorkOrderPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetPlanWorkOrderEntitiesAsync(PlanWorkOrderQuery planWorkOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderEntities = await conn.QueryAsync<PlanWorkOrderEntity>(template.RawSql, planWorkOrderQuery);
            return planWorkOrderEntities;
        }

        /// <summary>
        /// 获取List   LIKE
        /// </summary>
        /// <param name="planWorkOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetEqualPlanWorkOrderEntitiesAsync(PlanWorkOrderQuery planWorkOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(planWorkOrderQuery.OrderCode))
            {
                sqlBuilder.Where(" OrderCode = @OrderCode ");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderEntities = await conn.QueryAsync<PlanWorkOrderEntity>(template.RawSql, planWorkOrderQuery);
            return planWorkOrderEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderEntity planWorkOrderEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, planWorkOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanWorkOrderEntity> planWorkOrderEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderEntity planWorkOrderEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanWorkOrderEntity>
    planWorkOrderEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, planWorkOrderEntitys);
        }

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyWorkOrderStatusAsync(List<PlanWorkOrderEntity> parms) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateWorkOrderStatussSql, parms);
        }

    }

    public partial class PlanWorkOrderRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                          wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`, wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,
                          
                          wor.InputQty,wor.FinishProductQuantity,wor.RealStart,wor.RealEnd,
                          m.MaterialCode, m.MaterialName,m.Version as MaterialVersion,
                          b.BomCode,b.Version as BomVersion,
                          pr.`Code` as ProcessRouteCode ,pr.Version as ProcessRouteVersion,
                          wc.`Code`  as WorkCenterCode
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_record wor on wo.Id=wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId=m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId=b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId=pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId=wc.Id
                            
                        /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(1) 
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_record wor on wo.Id=wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId=m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId=b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId=pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId=wc.Id
                        /**where**/ ";
        const string GetPlanWorkOrderEntitiesSqlTemplate = @"SELECT
    /**select**/
    FROM `plan_work_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `plan_work_order`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterType, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Qty, @Status, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `plan_work_order`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterType, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Qty, @Status, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateSql = "UPDATE `plan_work_order` SET  ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `plan_work_order` SET   OrderCode = @OrderCode, ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, Status = @Status, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, IsLocked = @IsLocked, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `plan_work_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `plan_work_order`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT
      `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
    FROM `plan_work_order`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT
      `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
    FROM `plan_work_order`  WHERE Id IN @ids ";

        const string UpdateWorkOrderStatussSql = @"UPDATE `plan_work_order` SET Status = @Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
    }
}
