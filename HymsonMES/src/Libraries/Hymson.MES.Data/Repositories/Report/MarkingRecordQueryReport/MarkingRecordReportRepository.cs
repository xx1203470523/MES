using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Marking.Query;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Marking
{
    /// <summary>
    /// 仓储（Marking拦截汇总表）
    /// </summary>
    public partial class MarkingRecordReportRepository : BaseRepository, IMarkingRecordReportRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public MarkingRecordReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<MarkingRecordQueryReportView>> GetPagedInfoAsync(MarkingReportReportPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoReportCountSqlTemplate);
            pagedQuery.PageSize = 10;
            sqlBuilder.Select("msmi.SFC,msmi.FoundBadProcedureId FindProcedureId,msmi.ShouldInterceptProcedureId AppointInterceptProcedureId,msmir.InterceptProcedureId," +
                "msmir.InterceptEquipmentId,quc.UnqualifiedCode,quc.Type,quc.UnqualifiedCodeName," +
                "quc.Status,ms.Qty,msmir.InterceptOn,msi.ProductId,msi.WorkOrderId,msp.ResourceId,msmi.`CreatedBy` MarkingCreatedBy," +
                "msmi.`CreatedOn` MarkingCreatedOn,msmi.`UpdatedBy` MarkingClosedBy,msmi.`UpdatedOn` MarkingClosedOn,msmi.`Status` MarkingStatus");
            sqlBuilder.Where("msmi.IsDeleted = 0");
            sqlBuilder.Where("msmi.SiteId=@SiteId");
            sqlBuilder.LeftJoin("manu_sfc_marking_intercept msmir ON msmir.SfcMarkingId = msmi.id");
            sqlBuilder.LeftJoin("qual_unqualified_code quc on quc.id=msmi.UnqualifiedCodeId");
            sqlBuilder.LeftJoin("manu_sfc ms on ms.SFC=msmi.SFC");
            sqlBuilder.LeftJoin("manu_sfc_info msi on msi.SfcId=ms.id");
            sqlBuilder.LeftJoin("manu_sfc_produce msp on msp.SFC=msmi.SFC");

            //产品
            if (pagedQuery.ProductId.HasValue)
            {
                sqlBuilder.Where("msi.ProductId = @ProductId ");
            }

            //发现工序
            if (pagedQuery.FindProcedureId.HasValue)
            {
                sqlBuilder.Where("msmi.FindProcedureId = @FindProcedureId ");
            }

            //产品序列码
            if (!string.IsNullOrEmpty(pagedQuery.SFC))
            {
                pagedQuery.SFC = $"%{pagedQuery.SFC}%";
                sqlBuilder.Where(" msmi.SFC like @SFC ");
            }

            //指定拦截工序
            if (pagedQuery.AppointInterceptProcedureId.HasValue)
            {
                sqlBuilder.Where("msmi.AppointInterceptProcedureId = @AppointInterceptProcedureId ");
            }

            //实际拦截工序
            if (pagedQuery.InterceptProcedureId.HasValue)
            {
                sqlBuilder.Where("msmir.InterceptProcedureId = @InterceptProcedureId ");
            }

            //拦截设备
            if (pagedQuery.InterceptEquipmentId.HasValue)
            {
                sqlBuilder.Where("msmir.InterceptEquipmentId = @InterceptEquipmentId ");
            }

            //不合格代码
            if (!string.IsNullOrEmpty(pagedQuery.UnqualifiedCode))
            {
                pagedQuery.UnqualifiedCode = $"%{pagedQuery.UnqualifiedCode}%";
                sqlBuilder.Where(" quc.UnqualifiedCode like @UnqualifiedCode ");
            }

            //不合格状态
            if (!string.IsNullOrEmpty(pagedQuery.UnqualifiedStatus))
            {
                sqlBuilder.Where(" quc.Status = @UnqualifiedStatus ");
            }

            //录入人员
            if (!string.IsNullOrEmpty(pagedQuery.MarkingCreatedBy))
            {
                sqlBuilder.Where(" msmir.CreatedBy = @MarkingCreatedBy ");
            }

            //关闭人员
            if (!string.IsNullOrEmpty(pagedQuery.MarkingClosedBy))
            {
                sqlBuilder.Where(" msmir.UpdatedBy = @MarkingClosedBy ");
            }

            //拦截时间
            if (pagedQuery.InterceptOn != null && pagedQuery.InterceptOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.InterceptOn[0], DateEnd = pagedQuery.InterceptOn[1] });
                sqlBuilder.Where(" msmir.InterceptOn >= @DateStart AND msmir.InterceptOn < @DateEnd ");
            }

            //录入时间
            if (pagedQuery.MarkingCreatedOn != null && pagedQuery.MarkingCreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.MarkingCreatedOn[0], DateEnd = pagedQuery.MarkingCreatedOn[1] });
                sqlBuilder.Where(" msmir.CreatedOn >= @DateStart AND msmir.CreatedOn < @DateEnd ");
            }

            //关闭时间
            if (pagedQuery.MarkingCloseOn != null && pagedQuery.MarkingCloseOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DateStart = pagedQuery.MarkingCloseOn[0], DateEnd = pagedQuery.MarkingCloseOn[1] });
                sqlBuilder.Where(" msmir.UpdatedOn >= @DateStart AND msmir.UpdatedOn < @DateEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<MarkingRecordQueryReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<MarkingRecordQueryReportView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class MarkingRecordReportRepository
    {
        const string GetPagedInfoReportDataSqlTemplate = @"
                  SELECT /**select**/ FROM manu_sfc_marking msmi /**join**/ /**leftjoin**/  /**where**/ 	ORDER BY  msmi.UpdatedOn DESC  LIMIT @Offset,@Rows  ";
        const string GetPagedInfoReportCountSqlTemplate = @"select COUNT(*) from (
                       SELECT /**select**/  FROM manu_sfc_marking msmi  /**innerjoin**/ /**leftjoin**/ /**join**/  /**where**/ /**groupby**/  ) AS subquery_table ";
    }
}
