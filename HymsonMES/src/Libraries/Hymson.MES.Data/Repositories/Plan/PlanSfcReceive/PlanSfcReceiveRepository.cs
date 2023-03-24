/*
 *creator: Karl
 *
 *describe: 条码接收 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Warehouse;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码接收仓储
    /// </summary>
    public partial class PlanSfcReceiveRepository : IPlanSfcReceiveRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public PlanSfcReceiveRepository(IOptions<ConnectionOptions> connectionOptions)
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanSfcReceiveView> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanSfcReceiveView>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanSfcReceiveView>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanSfcReceiveView>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcReceiveView>> GetPagedInfoAsync(PlanSfcReceivePagedQuery planSfcInfoPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select(" msi.Id,msi.SFC,pwo.OrderCode,pwo.Type,pm.MaterialCode,pm.MaterialName,pwo.Qty,pwoR.OrderCode AS relevanceOrderCode,msi.CreatedBy,msi.CreatedOn");
            sqlBuilder.InnerJoin(" plan_work_order pwo ON pwo.Id=msi.WorkOrderId");
            sqlBuilder.InnerJoin(" proc_material pm ON pm.Id=msi.ProductId");
            sqlBuilder.LeftJoin(" plan_work_order pwoR ON pwoR.Id=msi.RelevanceWorkOrderId");

            sqlBuilder.Where(" msi.IsDeleted=0");

            if (!string.IsNullOrWhiteSpace(planSfcInfoPagedQuery.WorkOrderCode))
            {
                //planSfcInfoPagedQuery.WorkOrderCode = $"%{planSfcInfoPagedQuery.WorkOrderCode}%";
                //sqlBuilder.Where("WorkOrderCode like @WorkOrderCode");
                sqlBuilder.Where(" pwo.WorkOrderCode=@WorkOrderCode");
            }
            if (planSfcInfoPagedQuery.WorkOrderType > 0)
            {
                sqlBuilder.Where(" pwo.WorkOrderType=@WorkOrderType");
            }

            var offSet = (planSfcInfoPagedQuery.PageIndex - 1) * planSfcInfoPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planSfcInfoPagedQuery.PageSize });
            sqlBuilder.AddParameters(planSfcInfoPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planSfcInfoEntitiesTask = conn.QueryAsync<PlanSfcReceiveView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planSfcInfoEntities = await planSfcInfoEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanSfcReceiveView>(planSfcInfoEntities, planSfcInfoPagedQuery.PageIndex, planSfcInfoPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planSfcInfoQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanSfcReceiveView>> GetPlanSfcInfoEntitiesAsync(PlanSfcReceiveQuery planSfcInfoQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanSfcInfoEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planSfcInfoEntities = await conn.QueryAsync<PlanSfcReceiveView>(template.RawSql, planSfcInfoQuery);
            return planSfcInfoEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanSfcReceiveView planSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, planSfcInfoEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanSfcReceiveView> planSfcInfoEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, planSfcInfoEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanSfcReceiveView planSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, planSfcInfoEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanSfcReceiveView> planSfcInfoEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, planSfcInfoEntitys);
        }


        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="SFC"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetBySFCAsync(string SFC)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetBySFCSql, new { SFC = SFC });
        }


        /// <summary>
        /// 获取条码数据
        /// </summary>
        /// <param name="SFC"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetPlanSfcInfoAsync(PlanSfcReceiveQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanSfcInfoEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where(" Sfc=@SFC");
            }
            if (query.WorkOrderId > 0)
            {
                sqlBuilder.Where(" WorkOrderId=@WorkOrderId");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planSfcInfo = await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(template.RawSql, query);
            return planSfcInfo;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcInfoEntity manuSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcInfoEntity);
        }


    }

    public partial class PlanSfcReceiveRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info` msi /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_info` msi /**where**/ ";
        const string GetPlanSfcInfoEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_info` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`) VALUES (   @Id, @SFC, @WorkOrderId, @RelevanceWorkOrderId, @ProductId, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`) VALUES (   @Id, @SFC, @WorkOrderId, @RelevanceWorkOrderId, @ProductId, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId )  ";
        const string UpdateSql = "UPDATE `manu_sfc_info` SET   SFC = @SFC, WorkOrderId = @WorkOrderId, RelevanceWorkOrderId = @RelevanceWorkOrderId, ProductId = @ProductId, Qty = @Qty, Status = @Status, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_info` SET   SFC = @SFC, WorkOrderId = @WorkOrderId, RelevanceWorkOrderId = @RelevanceWorkOrderId, ProductId = @ProductId, Qty = @Qty, Status = @Status, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_info` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_info`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`
                            FROM `manu_sfc_info`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`
                            FROM `manu_sfc_info`  WHERE Id IN @ids ";


        const string GetBySFCSql = @"SELECT  
                               `Id`, `SFC`, `WorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`
                            FROM `manu_sfc_info`  WHERE SFC = @SFC ";
    }
}
