/*
 *creator: Karl
 *
 *describe: 工单激活记录 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-30 02:42:18
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
    /// 工单激活记录仓储
    /// </summary>
    public partial class PlanWorkOrderActivationRecordRepository : IPlanWorkOrderActivationRecordRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public PlanWorkOrderActivationRecordRepository(IOptions<ConnectionOptions> connectionOptions)
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
        public async Task<PlanWorkOrderActivationRecordEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderActivationRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderActivationRecordEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderActivationRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderActivationRecordEntity>> GetPagedInfoAsync(PlanWorkOrderActivationRecordPagedQuery planWorkOrderActivationRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (planWorkOrderActivationRecordPagedQuery.PageIndex - 1) * planWorkOrderActivationRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planWorkOrderActivationRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(planWorkOrderActivationRecordPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderActivationRecordEntitiesTask = conn.QueryAsync<PlanWorkOrderActivationRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planWorkOrderActivationRecordEntities = await planWorkOrderActivationRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderActivationRecordEntity>(planWorkOrderActivationRecordEntities, planWorkOrderActivationRecordPagedQuery.PageIndex, planWorkOrderActivationRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderActivationRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderActivationRecordEntity>> GetPlanWorkOrderActivationRecordEntitiesAsync(PlanWorkOrderActivationRecordQuery planWorkOrderActivationRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderActivationRecordEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderActivationRecordEntities = await conn.QueryAsync<PlanWorkOrderActivationRecordEntity>(template.RawSql, planWorkOrderActivationRecordQuery);
            return planWorkOrderActivationRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderActivationRecordEntity planWorkOrderActivationRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, planWorkOrderActivationRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanWorkOrderActivationRecordEntity> planWorkOrderActivationRecordEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderActivationRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderActivationRecordEntity planWorkOrderActivationRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderActivationRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderActivationRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanWorkOrderActivationRecordEntity>
    planWorkOrderActivationRecordEntitys)
    {
    using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
    return await conn.ExecuteAsync(UpdatesSql, planWorkOrderActivationRecordEntitys);
    }

    }

    public partial class PlanWorkOrderActivationRecordRepository
    {
    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order_activation_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `plan_work_order_activation_record` /**where**/ ";
    const string GetPlanWorkOrderActivationRecordEntitiesSqlTemplate = @"SELECT
    /**select**/
    FROM `plan_work_order_activation_record` /**where**/  ";

    const string InsertSql = "INSERT INTO `plan_work_order_activation_record`(  `Id`, `SiteId`, `WorkOrderId`, `LineId`, `ActivateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @WorkOrderId, @LineId, @ActivateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string InsertsSql = "INSERT INTO `plan_work_order_activation_record`(  `Id`, `SiteId`, `WorkOrderId`, `LineId`, `ActivateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @WorkOrderId, @LineId, @ActivateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string UpdateSql = "UPDATE `plan_work_order_activation_record` SET   SiteId = @SiteId, WorkOrderId = @WorkOrderId, LineId = @LineId, ActivateType = @ActivateType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
    const string UpdatesSql = "UPDATE `plan_work_order_activation_record` SET   SiteId = @SiteId, WorkOrderId = @WorkOrderId, LineId = @LineId, ActivateType = @ActivateType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
    const string DeleteSql = "UPDATE `plan_work_order_activation_record` SET IsDeleted = Id WHERE Id = @Id ";
    const string DeletesSql = "UPDATE `plan_work_order_activation_record`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
    const string GetByIdSql = @"SELECT
      `Id`, `SiteId`, `WorkOrderId`, `LineId`, `ActivateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
    FROM `plan_work_order_activation_record`  WHERE Id = @Id ";
    const string GetByIdsSql = @"SELECT
      `Id`, `SiteId`, `WorkOrderId`, `LineId`, `ActivateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
    FROM `plan_work_order_activation_record`  WHERE Id IN @ids ";
    }
    }
