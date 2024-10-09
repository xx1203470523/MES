using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活仓储
    /// </summary>
    public partial class PlanWorkOrderActivationRepository : BaseRepository, IPlanWorkOrderActivationRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public PlanWorkOrderActivationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 删除（硬删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（硬删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据工单ID批量删除（硬删除）
        /// </summary>
        /// <param name="workOrderIds"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByWorkOrderIdsAsync(long[] workOrderIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByWorkOrderIdsSql, new { WorkOrderIds = workOrderIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderActivationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderActivationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderActivationEntity> GetByWorkOrderIdAsync(long workOrderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderActivationEntity>(GetByworkOrderIdSql, new { workOrderId });
        }

        /// <summary>
        /// 根据工单id获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationEntity>> GetByWorkOrderIdsAsync(long[] orderIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderActivationEntity>(GetByworkOrderIdsSql, new { WorkOrderIds = orderIds });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderActivationEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据产线ID批量获取数据
        /// </summary>
        /// <param name="workCenterId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationEntity>> GetByWorkCenterIdAsync(long workCenterId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderActivationEntity>(GetByWorkCenterIdSql, new { workCenterId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderActivationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderActivationListDetailView>> GetPagedInfoAsync(PlanWorkOrderActivationPagedQuery planWorkOrderActivationPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("wo.IsDeleted=0");
            sqlBuilder.Where("wo.SiteId=@SiteId");

            if (planWorkOrderActivationPagedQuery.WorkCenterIds != null && planWorkOrderActivationPagedQuery.WorkCenterIds.Count > 0)
            {
                sqlBuilder.Where(" wo.WorkCenterId in @WorkCenterIds ");
            }
            if (planWorkOrderActivationPagedQuery.IsActivation.HasValue)
            {
                if ((bool)planWorkOrderActivationPagedQuery.IsActivation)
                {
                    sqlBuilder.Where(" woa.Id is not null ");
                }
                else
                {
                    sqlBuilder.Where(" woa.Id is null ");
                }
            }

            if (!string.IsNullOrWhiteSpace(planWorkOrderActivationPagedQuery.OrderCode))
            {
                planWorkOrderActivationPagedQuery.OrderCode = $"%{planWorkOrderActivationPagedQuery.OrderCode}%";
                sqlBuilder.Where(" wo.OrderCode like @OrderCode ");
            }
            if (!string.IsNullOrWhiteSpace(planWorkOrderActivationPagedQuery.MaterialCode))
            {
                planWorkOrderActivationPagedQuery.MaterialCode = $"%{planWorkOrderActivationPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" m.MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(planWorkOrderActivationPagedQuery.WorkCenterCode))
            {
                planWorkOrderActivationPagedQuery.WorkCenterCode = $"%{planWorkOrderActivationPagedQuery.WorkCenterCode}%";
                sqlBuilder.Where(" wc.Code like @WorkCenterCode ");
            }
            if (planWorkOrderActivationPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" wo.Status = @Status ");
            }
            else
            {
                sqlBuilder.AddParameters(new { NotStatus = new PlanWorkOrderStatusEnum[] { PlanWorkOrderStatusEnum.Closed, PlanWorkOrderStatusEnum.NotStarted } });
                sqlBuilder.Where(" wo.Status not in  @NotStatus ");//不要显示状态为已关闭的 和未开始的
            }

            if (planWorkOrderActivationPagedQuery.PlanStartTime != null && planWorkOrderActivationPagedQuery.PlanStartTime.Length > 0 && planWorkOrderActivationPagedQuery.PlanStartTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { PlanStartTimeStart = planWorkOrderActivationPagedQuery.PlanStartTime[0], PlanStartTimeEnd = planWorkOrderActivationPagedQuery.PlanStartTime[1].AddDays(1) });
                sqlBuilder.Where("wo.PlanStartTime >= @PlanStartTimeStart AND wo.PlanStartTime < @PlanStartTimeEnd");
            }
            sqlBuilder.OrderBy(" wo.WorkPlanId desc, wo.Id asc ");

            var offSet = (planWorkOrderActivationPagedQuery.PageIndex - 1) * planWorkOrderActivationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planWorkOrderActivationPagedQuery.PageSize });
            sqlBuilder.AddParameters(planWorkOrderActivationPagedQuery);

            using var conn = GetMESDbConnection();
            var planWorkOrderActivationEntitiesTask = conn.QueryAsync<PlanWorkOrderActivationListDetailView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planWorkOrderActivationEntities = await planWorkOrderActivationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderActivationListDetailView>(planWorkOrderActivationEntities, planWorkOrderActivationPagedQuery.PageIndex, planWorkOrderActivationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderActivationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationEntity>> GetPlanWorkOrderActivationEntitiesAsync(PlanWorkOrderActivationQuery planWorkOrderActivationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderActivationEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (planWorkOrderActivationQuery.WorkOrderId.HasValue)
            {
                sqlBuilder.Where(" WorkOrderId = @WorkOrderId ");
            }
            if (planWorkOrderActivationQuery.WorkOrderIds != null && planWorkOrderActivationQuery.WorkOrderIds.Any())
            {
                sqlBuilder.Where(" WorkOrderId in @WorkOrderIds ");
            }
            if (planWorkOrderActivationQuery.LineId.HasValue)
            {
                sqlBuilder.Where(" LineId = @LineId ");
            }

            if (planWorkOrderActivationQuery.LineIds != null && planWorkOrderActivationQuery.LineIds.Any())
            {
                sqlBuilder.Where(" LineId IN @LineIds ");
            }

            using var conn = GetMESDbConnection();
            var planWorkOrderActivationEntities = await conn.QueryAsync<PlanWorkOrderActivationEntity>(template.RawSql, planWorkOrderActivationQuery);
            return planWorkOrderActivationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderActivationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderActivationEntity planWorkOrderActivationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, planWorkOrderActivationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderActivationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanWorkOrderActivationEntity> planWorkOrderActivationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderActivationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderActivationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderActivationEntity planWorkOrderActivationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderActivationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderActivationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanWorkOrderActivationEntity> planWorkOrderActivationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, planWorkOrderActivationEntitys);
        }

        /// <summary>
        /// 通过bomID查找激活的工单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationEntity>> GetPlanWorkOrderActivationEntitiesByBomIdAsync(PlanWorkOrderActivationByBomIdQuery query) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderActivationEntity>(GetPlanWorkOrderActivationEntitiesByBomIdSql, query);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkOrderActivationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                          wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`, wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,

                          woa.Id as workOrderActivationId,

                          wor.InputQty,wor.FinishProductQuantity,wor.RealStart,wor.RealEnd,
                          m.MaterialCode, m.MaterialName,m.Version as MaterialVersion,
                          b.BomCode,b.Version as BomVersion,
                          pr.`Code` as ProcessRouteCode ,pr.Version as ProcessRouteVersion,
                          wc.`Code`  as WorkCenterCode
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_activation woa ON wo.Id= woa.WorkOrderId
                         LEFT JOIN plan_work_order_record wor on wo.Id=wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId=m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId=b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId=pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId=wc.Id
                         
                        /**where**/  
                        /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(1) 
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_activation woa ON wo.Id= woa.WorkOrderId
                         LEFT JOIN plan_work_order_record wor on wo.Id=wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId=m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId=b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId=pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId=wc.Id
                        /**where**/ ";
        const string GetPlanWorkOrderActivationEntitiesSqlTemplate = @"SELECT
    /**select**/
    FROM `plan_work_order_activation` /**where**/  ";

        const string InsertSql = "INSERT INTO `plan_work_order_activation`(  `Id`, `SiteId`, `WorkOrderId`, `LineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @WorkOrderId, @LineId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `plan_work_order_activation`(  `Id`, `SiteId`, `WorkOrderId`, `LineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @WorkOrderId, @LineId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `plan_work_order_activation` SET   SiteId = @SiteId, WorkOrderId = @WorkOrderId, LineId = @LineId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `plan_work_order_activation` SET   SiteId = @SiteId, WorkOrderId = @WorkOrderId, LineId = @LineId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `plan_work_order_activation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `plan_work_order_activation`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT * FROM `plan_work_order_activation`  WHERE Id = @Id ";
        const string GetByworkOrderIdSql = @"SELECT * FROM `plan_work_order_activation`  WHERE WorkOrderId = @WorkOrderId AND  IsDeleted=0";
        const string GetByworkOrderIdsSql = @"SELECT * FROM `plan_work_order_activation`  WHERE WorkOrderId in @WorkOrderIds AND  IsDeleted=0";
        const string GetByIdsSql = @"SELECT * FROM `plan_work_order_activation`  WHERE Id IN @ids ";
        const string GetByWorkCenterIdSql = "SELECT * FROM plan_work_order_activation WHERE IsDeleted = 0 AND LineId = @workCenterId";
        const string DeleteTrueSql = @"DELETE FROM  plan_work_order_activation where Id=@Id ";
        const string DeletesTrueSql = @"DELETE FROM  plan_work_order_activation where Id in @Ids ";
        const string DeletesTrueByWorkOrderIdsSql = @"DELETE FROM  plan_work_order_activation where WorkOrderId in @WorkOrderIds ";

        const string GetPlanWorkOrderActivationEntitiesByBomIdSql = @"SELECT woa.* 
                        FROM plan_work_order_activation woa 
                        LEFT JOIN plan_work_order wo on wo.Id=woa.WorkOrderId
                        WHERE woa.IsDeleted=0 AND  woa.SiteId=@SiteId AND wo.ProductBOMId=@BomId ";
    }
}
