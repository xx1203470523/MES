/*
 *creator: Karl
 *
 *describe: 条码打印 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Warehouse;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码打印仓储
    /// </summary>
    public partial class PlanSfcPrintRepository : IPlanSfcPrintRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public PlanSfcPrintRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
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
        public async Task<PlanSfcPrintView> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanSfcPrintView>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcPrintView>> GetPagedInfoAsync(PlanSfcPrintPagedQuery planSfcInfoPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select(" msi.Id,msi.SFC,pwo.OrderCode,pwo.Type,pm.MaterialCode,pm.MaterialName,pwo.Qty,pwoR.OrderCode AS relevanceOrderCode,msi.IsUsed,msi.CreatedBy,msi.CreatedOn");
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
            if (!string.IsNullOrWhiteSpace(planSfcInfoPagedQuery.SFC))
            {
                sqlBuilder.Where(" msi.SFC=@SFC");
            }
            if (planSfcInfoPagedQuery.IsUsed > 0)
            {
                sqlBuilder.Where(" msi.IsUsed=@IsUsed");
            }
            if (planSfcInfoPagedQuery.PrintStatus > 0)
            {
                //此待确认
                sqlBuilder.Where(" msi.PrintStatus=@PrintStatus");
            }

            var offSet = (planSfcInfoPagedQuery.PageIndex - 1) * planSfcInfoPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planSfcInfoPagedQuery.PageSize });
            sqlBuilder.AddParameters(planSfcInfoPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planSfcInfoEntitiesTask = conn.QueryAsync<PlanSfcPrintView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planSfcInfoEntities = await planSfcInfoEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanSfcPrintView>(planSfcInfoEntities, planSfcInfoPagedQuery.PageIndex, planSfcInfoPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcInfoEntity planSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, planSfcInfoEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcInfoEntity planSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, new { planSfcInfoEntity.Id, planSfcInfoEntity.RelevanceWorkOrderId, planSfcInfoEntity.Status, planSfcInfoEntity.UpdatedBy, planSfcInfoEntity.UpdatedOn });
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
        public async Task<ManuSfcInfoEntity> GetPlanSfcInfoAsync(PlanSfcPrintQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanSfcInfoEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where(" IsDeleted=0");

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

    }

    public partial class PlanSfcPrintRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info` msi /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_info` msi /**where**/ ";
        const string GetPlanSfcInfoEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_info` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`) VALUES (   @Id, @SFC, @WorkOrderId, @RelevanceWorkOrderId, @ProductId, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`) VALUES (   @Id, @SFC, @WorkOrderId, @RelevanceWorkOrderId, @ProductId, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId )  ";
        //const string UpdateSql = "UPDATE `manu_sfc_info` SET   SFC = @SFC, WorkOrderId = @WorkOrderId, RelevanceWorkOrderId = @RelevanceWorkOrderId, ProductId = @ProductId, Qty = @Qty, Status = @Status, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdateSql = "UPDATE `manu_sfc_info` SET    RelevanceWorkOrderId = @RelevanceWorkOrderId,  Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_info` SET   SFC = @SFC, WorkOrderId = @WorkOrderId, RelevanceWorkOrderId = @RelevanceWorkOrderId, ProductId = @ProductId, Qty = @Qty, Status = @Status, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_info` SET IsDeleted =Id WHERE Id = @Id ";
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
