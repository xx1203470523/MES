/*
 *creator: Karl
 *
 *describe: 生产汇总表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-06-15 10:37:18
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.WMS.Data.Repositories.ManuManufacture;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 生产汇总表仓储
/// </summary>
public partial class ManuSfcSummaryRepository : BaseRepository, IManuSfcSummaryRepository
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ConnectionOptions _connectionOptions;


    public ManuSfcSummaryRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
    }


    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ManuSfcSummaryEntity> GetOneAsync(ManuSfcSummaryQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);
        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        return await conn.QueryFirstOrDefaultAsync<ManuSfcSummaryEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcSummaryEntity>> GetListAsync(ManuSfcSummaryQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<ManuSfcSummaryEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcSummaryEntity>> GetPagedInfoAsync(ManuSfcSummaryPagedQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetPagedSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetCountSqlTemplate);

        WhereFill(sqlBuilder, query);

        if (!string.IsNullOrWhiteSpace(query.Sorting))
        {
            sqlBuilder.OrderBy(query.Sorting);
        }

        var offSet = (query.PageIndex - 1) * query.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = query.PageSize });
        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        var manuSfcSummaryEntities = await conn.QueryAsync<ManuSfcSummaryEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<ManuSfcSummaryEntity>(manuSfcSummaryEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    #region 更新

    /// <summary>
    /// 更新生产合格状态
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateQuanlityStatusAsync(ManuSfcSummaryUpdateCommand command)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var sqlTemplete = sqlBuilder.AddTemplate(UpdateQuanlityStatusSql);

        sqlBuilder.Where(" SFC = @SFC ");
        if (command.ProcedureId != null) sqlBuilder.Where(" ProcedureId = @ProcedureId");

        sqlBuilder.AddParameters(command);

        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(sqlTemplete.RawSql, sqlTemplete.Parameters);
    }

    /// <summary>
    /// 批量更新生产合格状态
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> UpdateQuanlityStatusAsync(List<ManuSfcSummaryUpdateCommand> command)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();
        var sqlTemplete = sqlBuilder.AddTemplate(UpdateQuanlityStatusSql);

        sqlBuilder.Where("SFC = @SFC");
        if (command.Any(a => a.ProcedureId != null)) sqlBuilder.Where("ProcedureId = @ProcedureId");

        sqlBuilder.AddParameters(command);

        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(sqlTemplete.RawSql, command);
    }

    #endregion


    #region 方法
    /// <summary>
    /// 删除（软删除）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(long id)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeleteSql, new { Id = id });
    }

    /// <summary>
    /// 批量删除（软删除）
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeletesAsync(DeleteCommand param)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(DeletesSql, param);
    }

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ManuSfcSummaryEntity> GetByIdAsync(long id)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<ManuSfcSummaryEntity>(GetByIdSql, new { Id = id });
    }

    /// <summary>
    /// 根据IDs批量获取数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcSummaryEntity>> GetByIdsAsync(long[] ids)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ManuSfcSummaryEntity>(GetByIdsSql, new { Ids = ids });
    }

    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="manuSfcSummaryQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcSummaryEntity>> GetManuSfcSummaryEntitiesAsync(ManuSfcSummaryQuery manuSfcSummaryQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetManuSfcSummaryEntitiesSqlTemplate);
        sqlBuilder.Select("*");
        sqlBuilder.Where("IsDeleted=0");
        sqlBuilder.Where("SiteId = @SiteId");
        if (manuSfcSummaryQuery.ProcedureIds != null && manuSfcSummaryQuery.ProcedureIds.Any())
        {
            sqlBuilder.Where("ProcedureId in @ProcedureIds");
        }
        if (manuSfcSummaryQuery.EquipmentId.HasValue)
        {
            sqlBuilder.Where("EquipmentId = @EquipmentId");
        }
        if (manuSfcSummaryQuery.EquipmentIds != null && manuSfcSummaryQuery.EquipmentIds.Length > 0)
        {
            sqlBuilder.Where("EquipmentId IN @EquipmentIds");
        }
        if (manuSfcSummaryQuery.WorkOrderId.HasValue)
        {
            sqlBuilder.Where("WorkOrderId = @WorkOrderId");
        }
        if (manuSfcSummaryQuery.SFCS != null && manuSfcSummaryQuery.SFCS.Any())
        {
            sqlBuilder.Where("SFC IN @SFCS");
        }
        if (manuSfcSummaryQuery.FirstQualityStatus.HasValue)
        {
            sqlBuilder.Where("FirstQualityStatus = @FirstQualityStatus");
        }
        if (manuSfcSummaryQuery.QualityStatus.HasValue)
        {
            sqlBuilder.Where("QualityStatus = @QualityStatus");
        }
        if (manuSfcSummaryQuery.StartTime.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @StartTime");
        }
        if (manuSfcSummaryQuery.EndTime.HasValue)
        {
            sqlBuilder.Where("CreatedOn < @EndTime");
        }
        using var conn = GetMESDbConnection();
        var manuSfcSummaryEntities = await conn.QueryAsync<ManuSfcSummaryEntity>(template.RawSql, manuSfcSummaryQuery);
        return manuSfcSummaryEntities;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="manuSfcSummaryEntity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(ManuSfcSummaryEntity manuSfcSummaryEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, manuSfcSummaryEntity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="manuSfcSummaryEntitys"></param>
    /// <returns></returns>
    public async Task<int> InsertsAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertsSql, manuSfcSummaryEntitys);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="manuSfcSummaryEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ManuSfcSummaryEntity manuSfcSummaryEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, manuSfcSummaryEntity);
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="manuSfcSummaryEntitys"></param>
    /// <returns></returns>
    public async Task<int> UpdatesAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdatesSql, manuSfcSummaryEntitys);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="manuSfcSummaryEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateNGAsync(ManuSfcSummaryQueryDto manuSfcSummaryEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateNGSql, manuSfcSummaryEntity);
    }

    /// <summary>
    /// 存在更新，不存在新增
    /// </summary>
    /// <param name="manuSfcSummaryEntitys"></param>
    /// <returns></returns>
    public async Task<int> InsertOrUpdateRangeAsync(List<ManuSfcSummaryEntity> manuSfcSummaryEntitys)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertOrUpdateSql, manuSfcSummaryEntitys);
    }
    #endregion

}

public partial class ManuSfcSummaryRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `manu_sfc_summary` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `manu_sfc_summary` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `manu_sfc_summary` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_summary` /**where**/;";

    #endregion

    #region 更新

    const string UpdateQuanlityStatusSql = "UPDATE manu_sfc_summary SET QualityStatus = @QualityStatus, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn /**where**/";

    #endregion

    #region 
    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_summary` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_summary` /**where**/ ";
    const string GetManuSfcSummaryEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_summary` /**where**/  ";

    const string InsertSql = "INSERT INTO `manu_sfc_summary`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
    const string InsertsSql = "INSERT INTO `manu_sfc_summary`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    const string InsertOrUpdateSql = "INSERT INTO `manu_sfc_summary`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @SFC, @WorkOrderId, @ProductId, @BeginTime, @EndTime, @RepeatedCount, @Qty, @NgNum, @FirstQualityStatus, @QualityStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted ) ON DUPLICATE KEY UPDATE" +
                                    " BeginTime=@BeginTime,EndTime=@EndTime,RepeatedCount=@RepeatedCount,Qty=@Qty,NgNum=@NgNum,FirstQualityStatus=@FirstQualityStatus,QualityStatus=@QualityStatus,UpdatedBy=@UpdatedBy,UpdatedOn=NOW(),IsDeleted=@IsDeleted ";

    const string UpdateSql = "UPDATE `manu_sfc_summary` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, BeginTime = @BeginTime, EndTime = @EndTime, RepeatedCount = @RepeatedCount, Qty = @Qty, NgNum = @NgNum, FirstQualityStatus = @FirstQualityStatus, QualityStatus = @QualityStatus, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
    const string UpdatesSql = "UPDATE `manu_sfc_summary` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, BeginTime = @BeginTime, EndTime = @EndTime, RepeatedCount = @RepeatedCount, Qty = @Qty, NgNum = @NgNum, FirstQualityStatus = @FirstQualityStatus, QualityStatus = @QualityStatus, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
    const string UpdateNGSql = "UPDATE `manu_sfc_summary` SET QualityStatus = @QualityStatus WHERE SFC = @SFC and  EquipmentId = @EquipmentId and IsDeleted = 0  ";

    const string DeleteSql = "UPDATE `manu_sfc_summary` SET IsDeleted = Id WHERE Id = @Id ";
    const string DeletesSql = "UPDATE `manu_sfc_summary` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

    const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_summary`  WHERE Id = @Id ";
    const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SFC`, `WorkOrderId`, `ProductId`, `BeginTime`, `EndTime`, `RepeatedCount`, `Qty`, `NgNum`, `FirstQualityStatus`, `QualityStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_summary`  WHERE Id IN @Ids ";
    #endregion
}


/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：生产汇总表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-24</para>
/// </summary>
public partial class ManuSfcSummaryRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcSummaryPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        if (query.ProcedureId.HasValue)
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }

        if (query.ProcedureIds != null && query.ProcedureIds.Any())
        {
            sqlBuilder.Where("ProcedureId IN @ProcedureIds");
        }


        if (query.ResourceId.HasValue)
        {
            sqlBuilder.Where("ResourceId = @ResourceId");
        }

        if (query.ResourceIds != null && query.ResourceIds.Any())
        {
            sqlBuilder.Where("ResourceId IN @ResourceIds");
        }


        if (query.EquipmentId.HasValue)
        {
            sqlBuilder.Where("EquipmentId = @EquipmentId");
        }

        if (query.EquipmentIds != null && query.EquipmentIds.Any())
        {
            sqlBuilder.Where("EquipmentId IN @EquipmentIds");
        }


        if (query.WorkOrderId.HasValue)
        {
            sqlBuilder.Where("WorkOrderId = @WorkOrderId");
        }

        if (query.WorkOrderIds != null && query.WorkOrderIds.Any())
        {
            sqlBuilder.Where("WorkOrderId IN @WorkOrderIds");
        }


        if (query.ProductId.HasValue)
        {
            sqlBuilder.Where("ProductId = @ProductId");
        }

        if (query.ProductIds != null && query.ProductIds.Any())
        {
            sqlBuilder.Where("ProductId IN @ProductIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcSummaryQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }

        if (query.SFCS?.Any() == true)
            sqlBuilder.Where(" SFC IN @SFCS ");

        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        if (query.ProcedureId.HasValue)
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }

        if (query.ProcedureIds != null && query.ProcedureIds.Any())
        {
            sqlBuilder.Where("ProcedureId IN @ProcedureIds");
        }


        if (query.ResourceId.HasValue)
        {
            sqlBuilder.Where("ResourceId = @ResourceId");
        }

        if (query.ResourceIds != null && query.ResourceIds.Any())
        {
            sqlBuilder.Where("ResourceId IN @ResourceIds");
        }


        if (query.EquipmentId.HasValue)
        {
            sqlBuilder.Where("EquipmentId = @EquipmentId");
        }

        if (query.EquipmentIds != null && query.EquipmentIds.Any())
        {
            sqlBuilder.Where("EquipmentId IN @EquipmentIds");
        }


        if (query.WorkOrderId.HasValue)
        {
            sqlBuilder.Where("WorkOrderId = @WorkOrderId");
        }

        if (query.WorkOrderIds != null && query.WorkOrderIds.Any())
        {
            sqlBuilder.Where("WorkOrderId IN @WorkOrderIds");
        }


        if (query.ProductId.HasValue)
        {
            sqlBuilder.Where("ProductId = @ProductId");
        }

        if (query.ProductIds != null && query.ProductIds.Any())
        {
            sqlBuilder.Where("ProductId IN @ProductIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：生产汇总表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-24</para>
/// </summary>
public partial class ManuSfcSummaryRepository
{
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：生产汇总表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-24</para>
/// </summary>
public partial class ManuSfcSummaryRepository
{
}