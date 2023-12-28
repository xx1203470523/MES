﻿using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Report;

public partial class NgRecordReportRepository : BaseRepository, INgRecordReportRepository
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
    public NgRecordReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    /// <summary>
    /// 联表分页查询
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<NgRecordReportView>> GetJoinPagedInfoAsync(NgRecordReportPageQuery pageQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetJoinPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetJoinPagedInfoCountSqlTemplate);
        sqlBuilder.InnerJoin("manu_sfc_step mss ON mss.Id  = BarCodeStepId");
        sqlBuilder.Where("mssn.IsDeleted=0");
        sqlBuilder.Select("mss.*");


        WhereFill(sqlBuilder, pageQuery);

        var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
        sqlBuilder.AddParameters(pageQuery);

        using var conn = GetMESDbConnection();
        var manuSfcStepNgEntities = await conn.QueryAsync<NgRecordReportView>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        return new PagedInfo<NgRecordReportView>(manuSfcStepNgEntities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
    }

    /// <summary>
    /// 列表数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<NgRecordReportView>> GetJoinListAsync(NgRecordReportQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetJoinListSqlTemplete);
        sqlBuilder.InnerJoin("manu_sfc_step mss ON mss.Id  = BarCodeStepId");
        sqlBuilder.Where("mssn.IsDeleted=0");
        sqlBuilder.Select("mss.*");

        WhereFill(sqlBuilder, query);

        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<NgRecordReportView>(templateData.RawSql, templateData.Parameters);
    }
}

#region WhereFill

public partial class NgRecordReportRepository
{
    public void WhereFill(SqlBuilder sqlBuilder, NgRecordReportPageQuery pageQuery)
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
        if (pageQuery.ProcedureId != null)
        {
            sqlBuilder.Where("mss.ProcedureId = @ProcedureId");
        }
        if (pageQuery.ProcedureIds?.Any() == true)
        {
            sqlBuilder.Where("mss.ProcedureId IN @ProcedureIds");
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
            sqlBuilder.Where("mssn.CreatedOn >= @BeginTime");
        }
        if (pageQuery.EndTime != null)
        {
            sqlBuilder.Where("mssn.CreatedOn < @EndTime");
        }
    }

    public void WhereFill(SqlBuilder sqlBuilder, NgRecordReportQuery query)
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
        if (query.ProcedureId != null)
        {
            sqlBuilder.Where("mss.ProcedureId = @ProcedureId");
        }
        if (query.ProcedureIds?.Any() == true)
        {
            sqlBuilder.Where("mss.ProcedureId IN @ProcedureIds");
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
            sqlBuilder.Where("mssn.CreatedOn >= @BeginTime");
        }
        if (query.EndTime != null)
        {
            sqlBuilder.Where("mssn.CreatedOn < @EndTime");
        }
    }
}

#endregion

#region SQL

public partial class NgRecordReportRepository
{
    const string GetJoinPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step_ng` mssn /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetJoinPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_step_ng` mssn /**innerjoin**/ /**where**/ ";

    const string GetJoinListSqlTemplete = "SELECT /**select**/ FROM `manu_sfc_step_ng` mssn /**innerjoin**/ /**leftjoin**/ /**where**/";
}
#endregion
