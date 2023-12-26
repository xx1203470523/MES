using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.QualificationRateReport.Query;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.QualificationRateReport
{
    /// <summary>
    /// 仓储（合格率报表）
    /// </summary>
    public partial class QualificationRateReportRepository : BaseRepository, IQualificationRateReportRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualificationRateReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualificationRateReportEnity>> GetPagedInfoAsync(QualificationRateReportPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            if (pagedQuery.Type == 2)//按年
            {
                sqlBuilder.Select(@"WorkOrderId,ProductId,ProcedureId,
                    YEAR(EndTime) AS StartYear,
                    MONTH(EndTime) AS StartMonth,
                    SUM(CASE WHEN QualityStatus = 1 THEN Qty ELSE 0 END) AS QualifiedQuantity,
                    SUM(CASE WHEN QualityStatus = 1 AND FirstQualityStatus = 1 THEN Qty ELSE 0 END) AS OneQualifiedQuantity,
                    SUM(CASE WHEN QualityStatus = 0 THEN Qty ELSE 0 END) AS UnQualifiedQuantity");

                sqlBuilder.GroupBy("WorkOrderId,ProductId,ProcedureId,YEAR(EndTime),MONTH(EndTime)");
                sqlBuilder.OrderBy("YEAR(EndTime) DESC,MONTH(EndTime) DESC");
            }
            else
            if (pagedQuery.Type==1)//按月
            {
                sqlBuilder.Select(@"WorkOrderId,ProductId,ProcedureId,DATE(EndTime) AS StartOn, 
                    SUM(CASE WHEN QualityStatus = 1 THEN Qty ELSE 0 END) AS QualifiedQuantity,
                    SUM(CASE WHEN QualityStatus = 1 AND FirstQualityStatus = 1 THEN Qty ELSE 0 END) AS OneQualifiedQuantity,
                    SUM(CASE WHEN QualityStatus = 0 THEN Qty ELSE 0 END) AS UnQualifiedQuantity");

                sqlBuilder.GroupBy("WorkOrderId,ProductId,ProcedureId,DATE(EndTime)");
                sqlBuilder.OrderBy("DATE(EndTime) DESC");
            }
            else//按日
            {
                sqlBuilder.Select(@"WorkOrderId,ProductId,ProcedureId,DATE(EndTime) AS StartOn, 
                    HOUR(EndTime) AS StartHour,
                    SUM(CASE WHEN QualityStatus = 1 THEN Qty ELSE 0 END) AS QualifiedQuantity,
                    SUM(CASE WHEN QualityStatus = 1 AND FirstQualityStatus = 1 THEN Qty ELSE 0 END) AS OneQualifiedQuantity,
                    SUM(CASE WHEN QualityStatus = 0 THEN Qty ELSE 0 END) AS UnQualifiedQuantity");

                sqlBuilder.GroupBy("WorkOrderId,ProductId,ProcedureId,DATE(EndTime),HOUR(EndTime)");
                sqlBuilder.OrderBy("DATE(EndTime) DESC,HOUR(EndTime) DESC");
            }

            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("QualityStatus is not null");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.WorkOrderIds != null && pagedQuery.WorkOrderIds.Any())
            {
                sqlBuilder.Where("WorkOrderId in @WorkOrderIds");
            }
            if (pagedQuery.ProcedureIds !=null && pagedQuery.ProcedureIds.Any())
            {
                sqlBuilder.Where("ProcedureId in @ProcedureIds");
            }
            if (pagedQuery.Date != null && pagedQuery.Date.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.Date[0], DateEnd = pagedQuery.Date[1].AddDays(1) });
                sqlBuilder.Where(" EndTime >= @DateStart AND EndTime < @DateEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<QualificationRateReportEnity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<QualificationRateReportEnity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 获取工序列表的信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>> GetProcdureInfoAsync()
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcedureEntity>(GetProcdureSql);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualificationRateReportRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_summary` /**innerjoin**/ /**leftjoin**/ /**where**/ /**groupby**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM (SELECT /**select**/ FROM `manu_sfc_summary` /**innerjoin**/ /**leftjoin**/ /**where**/ /**groupby**/ /**orderby**/ ) AS T";
        const string GetProcdureSql = "SELECT DISTINCT `Id`,`Name`,`CODE` FROM proc_procedure  /**where**/ ";
    }
}
