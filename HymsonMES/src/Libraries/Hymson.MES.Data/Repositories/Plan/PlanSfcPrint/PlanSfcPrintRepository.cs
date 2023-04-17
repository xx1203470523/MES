using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码打印仓储
    /// </summary>
    public partial class PlanSfcPrintRepository : IPlanSfcPrintRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public PlanSfcPrintRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
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
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class PlanSfcPrintRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info` msi /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_info` msi /**where**/ ";
    }
}
