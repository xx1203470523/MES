using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Report;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Report;

public partial class ProductDetailReportRepository : BaseRepository, IProductDetailReportRepository
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectionOptions"></param>
    public ProductDetailReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
    }

    /// <summary>
    /// 获取工序列表的信息
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcProcedureEntity>> GetProcdureInfoAsync()
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ProcProcedureEntity>(GetProcdureSql);
    }
    /// <summary>
    /// 产能报表-分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProductDetailReportView>> GetPageInfoAsync(ProductDetailReportPageQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        query.SearchType = "hour";
        if (query.Type == 1) query.SearchType = "day";

        query.Type = query.Type switch
        {
            0 => 13,
            1 => 10,
            _ => 13
        };

        if (query?.Date?.Any() == true)
        {
            if (query?.Date[0] != null) query.StartDate = query.Date[0];
            if (query?.Date[1] != null) query.EndDate = query.Date[1];
        }

        var GetPageInfoSqlReplace = GetPageInfoSql.Replace("@SearchType", query.SearchType);
        var GetPageInfoCountSqlReplace = GetPageInfoCountSql.Replace("@SearchType", query.SearchType);

        if (query.StartDate == null)
        {
            GetPageInfoSqlReplace = GetPageInfoSqlReplace.Replace("@StartDate", "NULL");
            GetPageInfoCountSqlReplace = GetPageInfoCountSqlReplace.Replace("@StartDate", "NULL");
        }
        if (query.EndDate == null)
        {
            GetPageInfoSqlReplace = GetPageInfoSqlReplace.Replace("@EndDate", "NULL");
            GetPageInfoCountSqlReplace = GetPageInfoCountSqlReplace.Replace("@EndDate", "NULL");
        }
        if (!query.OrderId.Any()&& string.IsNullOrEmpty(query.OrderCode))
        {
            GetPageInfoSqlReplace = GetPageInfoSqlReplace.Replace("@OrderId", "is not null");
            GetPageInfoCountSqlReplace = GetPageInfoCountSqlReplace.Replace("@OrderId", "is not null");
        }
        if (!query.OrderId.Any() && query.OrderCode != null)
        {
            GetPageInfoSqlReplace = GetPageInfoSqlReplace.Replace("@OrderId", "is  null");
            GetPageInfoCountSqlReplace = GetPageInfoCountSqlReplace.Replace("@OrderId", "is  null");
        }

        if (query.ProcedureId.Any())
        {
            sqlBuilder.Where("t1.ProcedureId in @ProcedureId");
        }

            if (query.OrderId.Any())
        {
                string str = "";
                foreach (var item in query.OrderId)
                {
                    str = str + item + ",";
                }
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                    str=" in ("+str+")";
                }

                GetPageInfoSqlReplace = GetPageInfoSqlReplace.Replace("@OrderId", str);
                GetPageInfoCountSqlReplace = GetPageInfoCountSqlReplace.Replace("@OrderId", str);
        }
            

            var templateData = sqlBuilder.AddTemplate(GetPageInfoSqlReplace);
            var templateCount = sqlBuilder.AddTemplate(GetPageInfoCountSqlReplace);

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var pageData = await conn.QueryAsync<ProductDetailReportView>(templateData.RawSql, templateData.Parameters);
            var pageCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<ProductDetailReportView>(pageData, query.PageIndex, query.PageSize, pageCount);
        

    }

    /// <summary>
    /// 获取下线工序产出总数
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<decimal> GetOutputSumAsyc(ProductDetailReportQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();

        var getSql = sqlBuilder.AddTemplate(GetSumQtySql);

        if (query.OrderId != null)
        {
            sqlBuilder.Where(" WorkOrderId = @OrderId ");
        }

        if (query.EquipmentId != null)
        {
            sqlBuilder.Where(" EquipmentId = @EquipmentId ");
        }

        if (query.ResourceId != null)
        {
            sqlBuilder.Where(" ResourceId = @ResourceId ");
        }
        else
        {
            sqlBuilder.Where(" ResourceId = 20032901117112320 ");
        }

        if (query.StartDate != null)
        {
            sqlBuilder.Where(" EndTime >= @StartDate ");
        }

        if (query.EndDate != null)
        {
            sqlBuilder.Where(" EndTime < @EndDate ");
        }


        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<decimal>(getSql.RawSql, getSql.Parameters);
    }
}

/// <summary>
/// SQL语句
/// </summary>
public partial class ProductDetailReportRepository
{
    private readonly string GetPageInfoSql = @"WITH T1 AS(
SELECT mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type) AS startDate,LEFT(date_add(EndTime,interval 1 @SearchType),@Type) AS EndDate,NULLIF(SUM(mss.Qty),0) OutputQty FROM manu_sfc_summary mss 
WHERE (EndTime >=@StartDate OR @StartDate IS NULL)
AND (EndTime < @EndDate OR @EndDate IS NULL)
AND (WorkOrderId  @OrderId )
AND mss.QualityStatus = 1
GROUP BY mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type),LEFT(date_add(EndTime,interval 1 @SearchType),@Type)
),T2 AS(
SELECT mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type) AS startDate , LEFT(date_add(EndTime,interval 1 @SearchType),@Type) AS EndDate,NULLIF(SUM(mss.Qty),0) OutputQty FROM manu_sfc_summary mss
WHERE (EndTime >=@StartDate OR @StartDate IS NULL)
AND (EndTime <  @EndDate OR @EndDate IS NULL)
AND (WorkOrderId  @OrderId)
GROUP BY mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type),LEFT(date_add(EndTime,interval 1 @SearchType),@Type)
)
SELECT t1.workOrderId,t1.ProductId,t1.startDate,t1.ProcedureId,t1.EndDate,t1.OutputQty FeedingQty,IFNULL(t2.OutputQty,0) OutputQty FROM T1
LEFT JOIN T2 ON t1.startDate = t2.startDate AND t1.EndDate = t2.EndDate AND t1.workorderid = t2.workorderid AND t1.productId = t2.productId and t1.ProcedureId=t2.ProcedureId
/**where**/
ORDER BY T1.startDate DESC

LIMIT @offSet,@Rows
";

    private readonly string GetPageInfoCountSql = @"WITH T1 AS(
SELECT mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type) AS startDate,LEFT(date_add(EndTime,interval 1 @SearchType),@Type) AS EndDate,NULLIF(SUM(mss.Qty),0) OutputQty FROM manu_sfc_summary mss 
WHERE (EndTime >=@StartDate OR @StartDate IS NULL)
AND (EndTime < @EndDate OR @EndDate IS NULL)
AND (WorkOrderId  @OrderId )
AND mss.QualityStatus = 1
GROUP BY mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type),LEFT(date_add(EndTime,interval 1 @SearchType),@Type)
),T2 AS(
SELECT mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type) AS startDate , LEFT(date_add(EndTime,interval 1 @SearchType),@Type) AS EndDate,NULLIF(SUM(mss.Qty),0) OutputQty FROM manu_sfc_summary mss
WHERE (EndTime >=@StartDate OR @StartDate IS NULL)
AND (EndTime <  @EndDate OR @EndDate IS NULL)
AND (WorkOrderId  @OrderId)
GROUP BY mss.WorkOrderId ,mss.ProductId,mss.ProcedureId,LEFT(EndTime,@Type),LEFT(date_add(EndTime,interval 1 @SearchType),@Type)
)
SELECT COUNT(1) FROM T1
LEFT JOIN T2 ON t1.startDate = t2.startDate AND t1.EndDate = t2.EndDate AND t1.workorderid = t2.workorderid AND t1.productId = t2.productId AND t1.procedureId=t2.procedureId
/**where**/
ORDER BY T1.startDate DESC
";

    private readonly string GetSumQtySql = "SELECT ifnull(SUM(ifnull(Qty,0)),0) FROM manu_sfc_summary mss  /**where**/";

    private readonly string GetProcdureSql = "SELECT DISTINCT `Name`,`CODE`,`Id` FROM proc_procedure  /**where**/ ";

    private readonly string TestSql = "SELECT * from manu_sfc_summary mss where mss.WorkOrderId in @OrderId  /**where**/";
}