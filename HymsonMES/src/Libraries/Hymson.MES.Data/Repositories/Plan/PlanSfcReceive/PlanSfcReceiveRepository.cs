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
        /// 分页查询
        /// </summary>
        /// <param name="planSfcInfoPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcReceiveView>> GetPagedInfoAsync(PlanSfcReceivePagedQuery planSfcInfoPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select(" ms.Id,ms.SFC,pwo.OrderCode,pwo.Type,pm.MaterialCode,pm.MaterialName,pwo.Qty,'' AS relevanceOrderCode,msi.IsUsed,ms.CreatedBy,ms.CreatedOn");
            sqlBuilder.InnerJoin(" manu_sfc_info msi ON ms.Id=msi.SfcId");
            sqlBuilder.InnerJoin(" plan_work_order pwo ON pwo.Id=msi.WorkOrderId");
            sqlBuilder.InnerJoin(" proc_material pm ON pm.Id=msi.ProductId");

            sqlBuilder.Where(" msi.IsDeleted=0");

            if (!string.IsNullOrWhiteSpace(planSfcInfoPagedQuery.OrderCode))
            {
                //planSfcInfoPagedQuery.OrderCode = $"%{planSfcInfoPagedQuery.OrderCode}%";
                //sqlBuilder.Where("OrderCode like @OrderCode");
                sqlBuilder.Where(" pwo.OrderCode=@OrderCode");
            }
            if (planSfcInfoPagedQuery.Type > 0)
            {
                sqlBuilder.Where(" pwo.Type=@Type");
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
        /// 获取条码数据
        /// </summary>
        /// <param name="SFC"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetPlanSfcInfoAsync(PlanSfcReceiveQuery query)
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
            var planSfcInfo = await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(template.RawSql, query);
            return planSfcInfo;
        }

    }

    public partial class PlanSfcReceiveRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc` ms /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc` ms /**innerjoin**/ /**leftjoin**/ /**where**/";
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
