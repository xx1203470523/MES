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

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 条码接收仓储
    /// </summary>
    public partial class PlanSfcReceiveRepository : BaseRepository, IPlanSfcReceiveRepository
    {
        public PlanSfcReceiveRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
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

            using var conn = GetMESDbConnection();
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
        public async Task<ManuSfcInfoEntity> GetPlanSfcInfoAsync(PlanSfcReceiveQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanSfcInfoEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where(" IsDeleted=0");
            sqlBuilder.Where(" SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where(" Sfc=@SFC");
            }
            if (query.WorkOrderId > 0)
            {
                sqlBuilder.Where(" WorkOrderId=@WorkOrderId");
            }
            using var conn = GetMESDbConnection();
            var planSfcInfo = await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(template.RawSql, query);
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
    }
}
