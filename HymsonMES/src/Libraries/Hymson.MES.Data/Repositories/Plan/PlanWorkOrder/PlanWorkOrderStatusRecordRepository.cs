/*
 *creator: Karl
 *
 *describe: 工单变更改记录表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-30 03:46:15
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单变更改记录表仓储
    /// </summary>
    public partial class PlanWorkOrderStatusRecordRepository : IPlanWorkOrderStatusRecordRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public PlanWorkOrderStatusRecordRepository(IOptions<ConnectionOptions> connectionOptions)
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
        public async Task<PlanWorkOrderStatusRecordEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderStatusRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderStatusRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderStatusRecordEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderStatusRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderStatusRecordEntity>> GetPagedInfoAsync(PlanWorkOrderStatusRecordPagedQuery planWorkOrderStatusRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (planWorkOrderStatusRecordPagedQuery.PageIndex - 1) * planWorkOrderStatusRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planWorkOrderStatusRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(planWorkOrderStatusRecordPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderStatusRecordEntitiesTask = conn.QueryAsync<PlanWorkOrderStatusRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planWorkOrderStatusRecordEntities = await planWorkOrderStatusRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderStatusRecordEntity>(planWorkOrderStatusRecordEntities, planWorkOrderStatusRecordPagedQuery.PageIndex, planWorkOrderStatusRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderStatusRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderStatusRecordEntity>> GetPlanWorkOrderStatusRecordEntitiesAsync(PlanWorkOrderStatusRecordQuery planWorkOrderStatusRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderStatusRecordEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderStatusRecordEntities = await conn.QueryAsync<PlanWorkOrderStatusRecordEntity>(template.RawSql, planWorkOrderStatusRecordQuery);
            return planWorkOrderStatusRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderStatusRecordEntity planWorkOrderStatusRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, planWorkOrderStatusRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderStatusRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderStatusRecordEntity planWorkOrderStatusRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderStatusRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderStatusRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanWorkOrderStatusRecordEntity>
    planWorkOrderStatusRecordEntitys)
    {
    using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
    return await conn.ExecuteAsync(UpdatesSql, planWorkOrderStatusRecordEntitys);
    }

    }

    public partial class PlanWorkOrderStatusRecordRepository
    {
    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order_status_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `plan_work_order_status_record` /**where**/ ";
    const string GetPlanWorkOrderStatusRecordEntitiesSqlTemplate = @"SELECT
    /**select**/
    FROM `plan_work_order_status_record` /**where**/  ";

    const string InsertSql = "INSERT INTO `plan_work_order_status_record`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Status`, `Qty`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId` ,LockedStatus) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Status, @Qty, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @LockedStatus )  ";
    const string InsertsSql = "INSERT INTO `plan_work_order_status_record`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Status`, `Qty`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, LockedStatus ) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Status, @Qty, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId ,@LockedStatus)  ";
    const string UpdateSql = "UPDATE `plan_work_order_status_record` SET   OrderCode = @OrderCode, ProductId = @ProductId, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Status = @Status, Qty = @Qty, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, IsLocked = @IsLocked, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId , LockedStatus = @LockedStatus  WHERE Id = @Id ";
    const string UpdatesSql = "UPDATE `plan_work_order_status_record` SET   OrderCode = @OrderCode, ProductId = @ProductId, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Status = @Status, Qty = @Qty, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, IsLocked = @IsLocked, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId , LockedStatus = @LockedStatus  WHERE Id = @Id ";
    const string DeleteSql = "UPDATE `plan_work_order_status_record` SET IsDeleted = Id WHERE Id = @Id ";
    const string DeletesSql = "UPDATE `plan_work_order_status_record`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
    const string GetByIdSql = @"SELECT
      `Id`, `OrderCode`, `ProductId`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Status`, `Qty`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, LockedStatus
    FROM `plan_work_order_status_record`  WHERE Id = @Id ";
    const string GetByIdsSql = @"SELECT
      `Id`, `OrderCode`, `ProductId`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Status`, `Qty`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`,LockedStatus
    FROM `plan_work_order_status_record`  WHERE Id IN @ids ";
    }
    }
