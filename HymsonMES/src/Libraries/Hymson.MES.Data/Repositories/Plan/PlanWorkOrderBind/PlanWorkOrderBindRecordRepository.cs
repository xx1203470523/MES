using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活日志（日志中心）仓储
    /// </summary>
    public partial class PlanWorkOrderBindRecordRepository : BaseRepository, IPlanWorkOrderBindRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public PlanWorkOrderBindRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderBindRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderBindRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderBindRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderBindRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderBindRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderBindRecordEntity>> GetPagedInfoAsync(PlanWorkOrderBindRecordPagedQuery planWorkOrderBindRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (planWorkOrderBindRecordPagedQuery.PageIndex - 1) * planWorkOrderBindRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planWorkOrderBindRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(planWorkOrderBindRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var planWorkOrderBindRecordEntitiesTask = conn.QueryAsync<PlanWorkOrderBindRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planWorkOrderBindRecordEntities = await planWorkOrderBindRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderBindRecordEntity>(planWorkOrderBindRecordEntities, planWorkOrderBindRecordPagedQuery.PageIndex, planWorkOrderBindRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderBindRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderBindRecordEntity>> GetPlanWorkOrderBindRecordEntitiesAsync(PlanWorkOrderBindRecordQuery planWorkOrderBindRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderBindRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var planWorkOrderBindRecordEntities = await conn.QueryAsync<PlanWorkOrderBindRecordEntity>(template.RawSql, planWorkOrderBindRecordQuery);
            return planWorkOrderBindRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderBindRecordEntity planWorkOrderBindRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, planWorkOrderBindRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanWorkOrderBindRecordEntity> planWorkOrderBindRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderBindRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderBindRecordEntity planWorkOrderBindRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderBindRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderBindRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanWorkOrderBindRecordEntity> planWorkOrderBindRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, planWorkOrderBindRecordEntitys);
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkOrderBindRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order_bind_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `plan_work_order_bind_record` /**where**/ ";
        const string GetPlanWorkOrderBindRecordEntitiesSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order_bind_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `plan_work_order_bind_record`(  `Id`, `EquipmentId`, `ResourceId`, `WorkOrderId`, `ActivateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @EquipmentId, @ResourceId, @WorkOrderId, @ActivateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `plan_work_order_bind_record`(  `Id`, `EquipmentId`, `ResourceId`, `WorkOrderId`, `ActivateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @EquipmentId, @ResourceId, @WorkOrderId, @ActivateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `plan_work_order_bind_record` SET   EquipmentId = @EquipmentId, ResourceId = @ResourceId, WorkOrderId = @WorkOrderId, ActivateType = @ActivateType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `plan_work_order_bind_record` SET   EquipmentId = @EquipmentId, ResourceId = @ResourceId, WorkOrderId = @WorkOrderId, ActivateType = @ActivateType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `plan_work_order_bind_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `plan_work_order_bind_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `plan_work_order_bind_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `plan_work_order_bind_record`  WHERE Id IN @Ids ";

    }
}
