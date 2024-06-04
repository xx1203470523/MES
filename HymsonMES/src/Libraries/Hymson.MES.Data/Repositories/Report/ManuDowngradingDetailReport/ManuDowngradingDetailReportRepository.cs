using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Report.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Report
{
    /// <summary>
    /// 仓储（降级品明细报表）
    /// </summary>
    public partial class ManuDowngradingDetailReportRepository : BaseRepository, IManuDowngradingDetailReportRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuDowngradingDetailReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingDetailReportView>> GetPagedInfoAsync(ManuDowngradingDetailReportPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoReportCountSqlTemplate);
            sqlBuilder.Select(@"pwo.WorkCenterId,mdr.SFC,msi.ProductId,mdr.Grade,mdr.Remark,mdr.UpdatedBy EntryPersonnel,mdr.UpdatedOn EntryTime,pwo.OrderCode,pwo.Type OrderType,mdr.ProcedureId");

            sqlBuilder.Where("mdr.IsDeleted = 0");
            sqlBuilder.Where("mdr.SiteId = @SiteId");
            sqlBuilder.OrderBy("mdr.UpdatedOn DESC");

            sqlBuilder.LeftJoin("manu_sfc_info msi ON mdr.SfcInfoId=msi.Id AND msi.IsDeleted=0");
            sqlBuilder.LeftJoin("plan_work_order pwo ON pwo.Id=msi.WorkOrderId AND pwo.IsDeleted=0");

            if (pagedQuery.WorkCenterId.HasValue)
            {
                sqlBuilder.Where(" pwo.WorkCenterId = @WorkCenterId ");
            }
            if (pagedQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where(" mdr.ProcedureId = @ProcedureId ");
            }
            if (pagedQuery.ProductId.HasValue)
            {
                sqlBuilder.Where(" msi.ProductId = @ProductId ");
            }
            if (pagedQuery.OrderId.HasValue)
            {
                sqlBuilder.Where("msi.WorkOrderId = @OrderId");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.SFC))
            {
                pagedQuery.SFC = $"%{pagedQuery.SFC}%";
                sqlBuilder.Where("mdr.SFC like @SFC");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.DowngradingCode))
            {
                pagedQuery.DowngradingCode = $"%{pagedQuery.DowngradingCode}%";
                sqlBuilder.Where("mdr.Grade like @DowngradingCode");
            }
            if (pagedQuery.CreatedOn != null && pagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.CreatedOn[0].AddHours(8), DateEnd = pagedQuery.CreatedOn[1].AddDays(1).AddHours(8) });
                sqlBuilder.Where(" mdr.UpdatedOn >= @DateStart AND mdr.UpdatedOn < @DateEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuDowngradingDetailReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuDowngradingDetailReportView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuDowngradingDetailReportRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_downgrading_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_downgrading_record` /**where**/ ";
        const string GetPagedInfoReportDataSqlTemplate = @"
                  SELECT /**select**/ FROM manu_downgrading_record mdr /**innerjoin**/ /**leftjoin**/  /**where**/  /**orderby**/ LIMIT @Offset,@Rows  ";
        const string GetPagedInfoReportCountSqlTemplate = @" 
                   SELECT COUNT(*) FROM manu_downgrading_record mdr/**innerjoin**/ /**leftjoin**/  /**where**/ ";
    }
}
