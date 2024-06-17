using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Report.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Report;

public partial class ProductionDetailsReportRepository : BaseRepository, IProductionDetailsReportRepository
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    private readonly ConnectionOptions _connectionOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectionOptions"></param>
    /// <param name="memoryCache"></param>
    public ProductionDetailsReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    /// <summary>
    /// 联表分页查询
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProductionDetailsReportView>> GetPagedInfoAsync(ProductionDetailsReportPageQuery pageQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetJoinPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetJoinPagedInfoCountSqlTemplate);
        sqlBuilder.Where("mss.IsDeleted=0");
        sqlBuilder.Select("mss.*");

        WhereFill(sqlBuilder, pageQuery);

        var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
        sqlBuilder.AddParameters(pageQuery);

        using var conn = GetMESDbConnection();
        var manuSfcStepNgEntities = await conn.QueryAsync<ProductionDetailsReportView>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        return new PagedInfo<ProductionDetailsReportView>(manuSfcStepNgEntities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
    }

    /// <summary>
    /// 列表数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProductionDetailsReportView>> GetListAsync(ProductionDetailsReportQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetJoinListSqlTemplete);
        sqlBuilder.Where("mss.IsDeleted=0");
        sqlBuilder.Select("mss.*");

        WhereFill(sqlBuilder, query);
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ProductionDetailsReportView>(templateData.RawSql, templateData.Parameters);
    }
}

#region WhereFill

public partial class ProductionDetailsReportRepository
{
    public void WhereFill(SqlBuilder sqlBuilder, ProductionDetailsReportPageQuery pageQuery)
    {
        if (pageQuery.SFC != null)
        {
            sqlBuilder.Where("SFC = @SFC");
        }
        if (pageQuery.EquipmentId != null)
        {
            sqlBuilder.Where("mss.EquipmentId = @EquipmentId");
        }
        if (pageQuery.EquipmentIds?.Any() == true)
        {
            sqlBuilder.Where("mss.EquipmentId IN @EquipmentIds");
        }
        if (pageQuery.ProcedureId?.Any() == true)
        {
            sqlBuilder.Where("mss.ProcedureId IN @ProcedureId");
        }
        if (pageQuery.ResourceId != null)
        {
            sqlBuilder.Where("mss.ResourceId = @ResourceId");
        }
        if (pageQuery.ResourceIds?.Any() == true)
        {
            sqlBuilder.Where("mss.ResourceId IN @ResourceIds");
        }
        if (pageQuery.BeginTime != null)
        {
            sqlBuilder.Where("mss.EndTime >= @BeginTime");
        }
        if (pageQuery.EndTime != null)
        {
            sqlBuilder.Where("mss.EndTime < @EndTime");
        }
        if (pageQuery.QualityStatus != null)
        {
            sqlBuilder.Where("mss.QualityStatus = @QualityStatus");
        }
    }

    public void WhereFill(SqlBuilder sqlBuilder, ProductionDetailsReportQuery query)
    {
        if (query.SFC != null)
        {
            sqlBuilder.Where("mss.SFC = @SFC");
        }

        if (query.EquipmentId != null)
        {
            sqlBuilder.Where("mss.EquipmentId = @EquipmentId");
        }

        if (query.EquipmentIds?.Any() == true)
        {
            sqlBuilder.Where("mss.EquipmentId IN @EquipmentIds");
        }

        if (query.ProcedureId?.Any() == true)
        {
            sqlBuilder.Where("mss.ProcedureId IN @ProcedureId");
        }

        if (query.ResourceId != null)
        {
            sqlBuilder.Where("mss.ResourceId = @ResourceId");
        }

        if (query.ResourceIds?.Any() == true)
        {
            sqlBuilder.Where("mss.ResourceId IN @ResourceIds");
        }

        if (query.BeginTime != null)
        {
            sqlBuilder.Where($"mss.EndTime >= '{query.BeginTime?.ToString("yyyy-MM-dd")}'");
        }

        if (query.EndTime != null)
        {
            sqlBuilder.Where($"mss.EndTime < '{query.EndTime?.ToString("yyyy-MM-dd")}'");
        }

        if (query.QualityStatus != null)
        {
            sqlBuilder.Where("mss.QualityStatus = @QualityStatus");
        }
    }
}

#endregion

#region SQL

public partial class ProductionDetailsReportRepository
{
    const string GetJoinPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_summary` mss /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetJoinPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_summary` mss /**innerjoin**/ /**where**/ ";

    const string GetJoinListSqlTemplete = "SELECT /**select**/ FROM `manu_sfc_summary` mss /**innerjoin**/ /**leftjoin**/ /**where**/";
}
#endregion
