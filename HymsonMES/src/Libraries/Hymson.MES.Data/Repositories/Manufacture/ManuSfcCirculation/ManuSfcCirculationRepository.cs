using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 条码流转表仓储
/// </summary>
public partial class ManuSfcCirculationRepository : BaseRepository, IManuSfcCirculationRepository
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    private readonly ConnectionOptions _connectionOptions;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionOptions"></param>
    /// <param name="memoryCache"></param>
    public ManuSfcCirculationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ManuSfcCirculationEntity> GetOneAsync(ManuSfcCirculationQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        return await conn.QueryFirstOrDefaultAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcCirculationEntity>> GetListAsync(ManuSfcCirculationQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        return await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcCirculationEntity>> GetPagedInfoAsync(ManuSfcCirculationPagedQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetPagedSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetCountSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.OrderBy("CreatedOn");

        var offSet = (query.PageIndex - 1) * query.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = query.PageSize });
        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

        var manuSfcCirculationEntities = await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

        return new PagedInfo<ManuSfcCirculationEntity>(manuSfcCirculationEntities, query.PageIndex, query.PageSize, totalCount);
    }

    #endregion

    /// <summary>
    /// 根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ManuSfcCirculationEntity> GetByIdAsync(long id)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryFirstOrDefaultAsync<ManuSfcCirculationEntity>(GetByIdSql, new { Id = id });
    }

    /// <summary>
    /// 根据SFC获取数据
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcCirculationEntity>> GetSfcMoudulesAsync(ManuSfcCirculationQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
        sqlBuilder.Select("*");

        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Where("IsDeleted = 0");

        if (!string.IsNullOrWhiteSpace(query.Sfc))
        {
            sqlBuilder.Where("SFC = @Sfc");
        }

        if (query.CirculationTypes?.Any() == true)
        {
            sqlBuilder.Where("CirculationType IN @CirculationTypes");
        }

        if (query.IsDisassemble.HasValue)
        {
            sqlBuilder.Where("IsDisassemble = @IsDisassemble");
        }

        if (query.ProcedureId.HasValue)
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }
        if (query.CirculationMainProductId.HasValue)
        {
            sqlBuilder.Where("CirculationMainProductId = @CirculationMainProductId");
        }

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 根据SFCs获取数据
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>  
    public async Task<IEnumerable<ManuSfcCirculationEntity>> GetSfcMoudulesAsync(ManuSfcCirculationBySfcsQuery query)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
        sqlBuilder.Select("*");

        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Where("IsDeleted = 0");

        if (query.Sfc != null && query.Sfc.Any())
        {
            sqlBuilder.Where("SFC IN @Sfc");
        }

        if (!string.IsNullOrWhiteSpace(query.CirculationBarCode))
        {
            sqlBuilder.Where("CirculationBarCode = @CirculationBarCode");
        }

        if (query.CirculationTypes?.Any() == true)
        {
            sqlBuilder.Where("CirculationType IN @CirculationTypes");
        }

        if (query.IsDisassemble.HasValue)
        {
            sqlBuilder.Where("IsDisassemble = @IsDisassemble");
        }

        if (query.ProcedureId.HasValue)
        {
            sqlBuilder.Where("ProcedureId = @ProcedureId");
        }
        if (query.CirculationMainProductId.HasValue)
        {
            sqlBuilder.Where("CirculationMainProductId = @CirculationMainProductId");
        }

        sqlBuilder.AddParameters(query);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 根据IDs批量获取数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcCirculationEntity>> GetByIdsAsync(long[] ids)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.QueryAsync<ManuSfcCirculationEntity>(GetByIdsSql, new { ids = ids });
    }

    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="manuSfcCirculationQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationEntitiesAsync(ManuSfcCirculationQuery manuSfcCirculationQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        var manuSfcCirculationEntities = await conn.QueryAsync<ManuSfcCirculationEntity>(template.RawSql, manuSfcCirculationQuery);
        return manuSfcCirculationEntities;
    }

    /// <summary>
    /// 根据流转前和流转后条码获取绑定记录
    /// </summary>
    /// <param name="manuSfcCirculationBarCodeQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationBarCodeEntitiesAsync(ManuSfcCirculationBarCodeQuery manuSfcCirculationBarCodeQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
        sqlBuilder.Where("IsDeleted=0");
        sqlBuilder.Select("*");
        sqlBuilder.Where("SiteId=@SiteId");
        if (manuSfcCirculationBarCodeQuery.CirculationType.HasValue)
        {
            sqlBuilder.Where("CirculationType=@CirculationType");
        }
        if (manuSfcCirculationBarCodeQuery.ResourceId.HasValue)
        {
            sqlBuilder.Where("ResourceId=@ResourceId");
        }
        if (manuSfcCirculationBarCodeQuery.IsDisassemble.HasValue)
        {
            sqlBuilder.Where("IsDisassemble=@IsDisassemble");
        }
        if (manuSfcCirculationBarCodeQuery.Sfcs != null && manuSfcCirculationBarCodeQuery.Sfcs.Length > 0)
        {
            sqlBuilder.Where("SFC IN @Sfcs");
        }
        if (manuSfcCirculationBarCodeQuery.CirculationBarCodes != null && manuSfcCirculationBarCodeQuery.CirculationBarCodes.Length > 0)
        {
            sqlBuilder.Where("CirculationBarCode IN @CirculationBarCodes");
        }
        if (!string.IsNullOrEmpty(manuSfcCirculationBarCodeQuery.CirculationBarCode))
        {
            sqlBuilder.Where("CirculationBarCode = @CirculationBarCode");
        }
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        var manuSfcCirculationEntities = await conn.QueryAsync<ManuSfcCirculationEntity>(template.RawSql, manuSfcCirculationBarCodeQuery);
        return manuSfcCirculationEntities;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="manuSfcCirculationEntity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(ManuSfcCirculationEntity manuSfcCirculationEntity)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertSql, manuSfcCirculationEntity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="manuSfcCirculationEntitys"></param>
    /// <returns></returns>
    public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(InsertSql, manuSfcCirculationEntitys);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="manuSfcCirculationEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ManuSfcCirculationEntity manuSfcCirculationEntity)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(UpdateSql, manuSfcCirculationEntity);
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="manuSfcCirculationEntitys"></param>
    /// <returns></returns>
    public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(UpdateSql, manuSfcCirculationEntitys);
    }

    /// <summary>
    /// 批量删除（软删除）
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteRangeAsync(DeleteCommand command)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(DeleteSql, command);
    }

    /// <summary>
    /// 在制品拆解移除
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<int> DisassemblyUpdateAsync(DisassemblyCommand command)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(DisassemblyUpdateSql, command);
    }

    /// <summary>
    /// 组件使用报告 分页查询
    /// </summary>
    /// <param name="manuSfcCirculationPagedQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcCirculationEntity>> GetReportPagedInfoAsync(ComUsageReportPagedQuery queryParam)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetReportPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetReportPagedInfoCountSqlTemplate);
        sqlBuilder.Where(" IsDeleted=0 ");
        sqlBuilder.Select("*");

        sqlBuilder.Where(" IsDisassemble=0 "); //筛选出未拆解的

        sqlBuilder.Where(" SiteId=@SiteId ");

        //where sc.IsDeleted = 0
        //    and sc.SiteId = ''
        //    and sc.CirculationProductId = ''
        //    and sc.CreatedOn BETWEEN '' and ''-- 查询 时间
        //    and sc.CirculationBarCode like '%%'-- 查询组件车间作业 / 库存批次 
        //    -- and-- 查询 供应商编码
        //    and sc.ProcedureId = ''
        //    and sc.ResourceId-- 查询资源

        if (queryParam.CirculationProductId.HasValue)
        {
            sqlBuilder.Where(" CirculationProductId=@CirculationProductId ");
        }

        if (queryParam.CreatedOn != null && queryParam.CreatedOn.Length >= 2)
        {
            sqlBuilder.AddParameters(new { CreatedOnStart = queryParam.CreatedOn[0], CreatedOnEnd = queryParam.CreatedOn[1].AddDays(1) });
            sqlBuilder.Where(" CreatedOn >= @CreatedOnStart AND CreatedOn < @CreatedOnEnd ");
        }

        if (!string.IsNullOrEmpty(queryParam.CirculationBarCode))
        {
            sqlBuilder.Where(" CirculationBarCode=@CirculationBarCode ");
        }

        if (queryParam.ProcedureId.HasValue)
        {
            sqlBuilder.Where(" ProcedureId=@ProcedureId ");
        }

        if (queryParam.ResourceId.HasValue)
        {
            sqlBuilder.Where(" ResourceId=@ResourceId ");
        }

        if (queryParam.CirculationMainSupplierId.HasValue)
        {
            sqlBuilder.Where(" CirculationMainSupplierId=@CirculationMainSupplierId ");
        }

        var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
        sqlBuilder.AddParameters(queryParam);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<ManuSfcCirculationEntity>(manuSfcCirculationEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
    }

    /// <summary>
    /// 条码追溯 分页查询
    /// </summary>
    /// <param name="manuSfcCirculationPagedQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcCirculationEntity>> GetProductTraceReportPagedInfoAsync(ProductTraceReportPagedQuery queryParam)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetTraceReportPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetTraceReportPagedInfoCountSqlTemplate);

        sqlBuilder.Where("T2.IsDeleted=0 ");
        sqlBuilder.Where("T2.IsDisassemble=0 "); //未拆解的， 是否拆解：0
        if (queryParam.TraceDirection)
        {
            sqlBuilder.Where("T2.SFC=T3.CirculationBarCode "); //正向追溯
        }
        else
        {
            sqlBuilder.Where("T2.CirculationBarCode=T3.SFC ");//反向追溯
        }

        var offSet = (queryParam.PageIndex - 1) * queryParam.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = queryParam.PageSize });
        sqlBuilder.AddParameters(new { queryParam.SiteId });
        sqlBuilder.AddParameters(queryParam);

        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<ManuSfcCirculationEntity>(manuSfcCirculationEntities, queryParam.PageIndex, queryParam.PageSize, totalCount);
    }

    public async Task<int> UpdateSfcAsync(ManuSfcCirculationBind bind)
    {
        using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
        return await conn.ExecuteAsync(UpdateSFCSql, bind);
    }
}

/// <summary>
/// 
/// </summary>
public partial class ManuSfcCirculationRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `manu_sfc_circulation` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `manu_sfc_circulation` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `manu_sfc_circulation` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_circulation` /**where**/;";

    #endregion

    const string UpdateSFCSql = "UPDATE manu_sfc_circulation SET SFC = @SFC WHERE Id = @Id";

    const string GetPageInfoSql = "SELECT /**select**/ FROM `manu_sfc_circulation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";

    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_circulation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_circulation` /**where**/ ";
    const string GetManuSfcCirculationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_circulation` /**where**/  ";

    const string InsertSql = "INSERT INTO `manu_sfc_circulation`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`,CirculationMainProductId, CirculationQty, `CirculationType`,  `IsDisassemble`,`CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Location`,`Name`,`ModelCode`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @FeedingPointId, @SFC, @WorkOrderId, @ProductId, @CirculationBarCode, @CirculationWorkOrderId, @CirculationProductId, @CirculationMainProductId,@CirculationQty, @CirculationType, @IsDisassemble,@CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Location ,@Name, @ModelCode)  ";
    const string UpdateSql = "UPDATE `manu_sfc_circulation` SET ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, FeedingPointId = @FeedingPointId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, CirculationBarCode = @CirculationBarCode, CirculationWorkOrderId = @CirculationWorkOrderId, CirculationProductId = @CirculationProductId, CirculationMainProductId =@CirculationMainProductId,  CirculationQty=@CirculationQty,  CirculationType = @CirculationType, IsDisassemble = @IsDisassemble, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Location = @Location,Name = @Name, ModelCode=@ModelCode  WHERE Id = @Id ";
    const string DeleteSql = "UPDATE `manu_sfc_circulation` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids";
    const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`, CirculationMainProductId, CirculationQty, `CirculationType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Location`, `Name`,`ModelCode`
                            FROM `manu_sfc_circulation`  WHERE Id = @Id ";

    const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`,CirculationMainProductId, CirculationQty, `CirculationType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Location`, `Name`,`ModelCode`
                            FROM `manu_sfc_circulation`  WHERE Id IN @ids ";

    const string DisassemblyUpdateSql = "UPDATE manu_sfc_circulation SET " +
        "CirculationType = @CirculationType, IsDisassemble = @IsDisassemble," +
        "DisassembledBy = @UserId, DisassembledOn = @UpdatedOn, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn WHERE Id = @Id AND IsDisassemble <> @IsDisassemble ";

    const string GetReportPagedInfoDataSqlTemplate = @"
                SELECT 
                    /**select**/ 
                FROM `manu_sfc_circulation` 
                /**innerjoin**/ 
                /**leftjoin**/ 
                /**where**/ 
                LIMIT @Offset,@Rows 
                ";
    const string GetReportPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_circulation` /**where**/ ";

    const string GetTraceReportPagedInfoDataSqlTemplate = @"
            /*追溯*/
            WITH RECURSIVE recursion (id, sfc, CirculationBarCode,IsDeleted,IsDisassemble,CirculationType,CirculationQty,ProcedureId,ResourceId,EquipmentId,WorkOrderId,ProductId,Location,CreatedOn,CreatedBy) AS
            (
             /*初始查询*/
              SELECT T1.id,T1.sfc,T1.CirculationBarCode,T1.IsDeleted,T1.IsDisassemble,T1.CirculationType,T1.CirculationQty,T1.ProcedureId,T1.ResourceId,T1.EquipmentId,T1.WorkOrderId,T1.ProductId,T1.Location,T1.CreatedOn,T1.CreatedBy
		            FROM manu_sfc_circulation T1 WHERE T1.IsDeleted=0 AND T1.sfc = @SFC  and T1.IsDisassemble=0
              UNION /*使用Union 而不是 Union ALL 避免数据存在循环引用导致死循环*/
	            /*递归查询*/
              SELECT T2.id,  T2.SFC, T2.CirculationBarCode,T2.IsDeleted,T2.IsDisassemble,T2.CirculationType,T2.CirculationQty,T2.ProcedureId,T2.ResourceId,T2.EquipmentId,T2.WorkOrderId,T2.ProductId,T2.Location,T2.CreatedOn,T2.CreatedBy
		            FROM manu_sfc_circulation T2, recursion T3  /**where**/
            )
            /*主查询*/
            SELECT T.id, T.SFC, T.CirculationBarCode,IsDeleted,IsDisassemble,CirculationType,CirculationQty,ProcedureId,ResourceId,EquipmentId,WorkOrderId,ProductId,Location,CreatedOn,CreatedBy FROM recursion T  LIMIT @Offset,@Rows  ";

    const string GetTraceReportPagedInfoCountSqlTemplate = @"
            WITH RECURSIVE recursion (id, sfc, CirculationBarCode,IsDeleted,IsDisassemble,CirculationType,CirculationQty,ProcedureId,ResourceId,EquipmentId,WorkOrderId,ProductId,Location,CreatedOn,CreatedBy) AS
            (
             /*初始查询*/
              SELECT T1.id,T1.sfc,T1.CirculationBarCode,T1.IsDeleted,T1.IsDisassemble,T1.CirculationType,T1.CirculationQty,T1.ProcedureId,T1.ResourceId,T1.EquipmentId,T1.WorkOrderId,T1.ProductId,T1.Location,T1.CreatedOn,T1.CreatedBy
		            FROM manu_sfc_circulation T1 WHERE T1.IsDeleted=0 AND T1.sfc = @SFC and T1.IsDisassemble=0
              UNION /*使用Union 而不是 Union ALL 避免数据存在循环引用导致死循环*/
	            /*递归查询*/
              SELECT T2.id,  T2.SFC, T2.CirculationBarCode,T2.IsDeleted,T2.IsDisassemble,T2.CirculationType,T2.CirculationQty,T2.ProcedureId,T2.ResourceId,T2.EquipmentId,T2.WorkOrderId,T2.ProductId,T2.Location,T2.CreatedOn,T2.CreatedBy
		            FROM manu_sfc_circulation T2, recursion T3  /**where**/ 
            )
            /*主查询*/
            SELECT count(1) FROM recursion T";

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：条码流转表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-1-31</para>
/// </summary>
public partial class ManuSfcCirculationRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcCirculationPagedQuery query)
    {
        sqlBuilder.Where("IsDeleted = 0");

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

        if (query.FeedingPointId.HasValue)
        {
            sqlBuilder.Where("FeedingPointId = @FeedingPointId");
        }

        if (query.FeedingPointIds != null && query.FeedingPointIds.Any())
        {
            sqlBuilder.Where("FeedingPointId IN @FeedingPointIds");
        }

        if (!string.IsNullOrWhiteSpace(query.SFC))
        {
            sqlBuilder.Where("SFC = @SFC");
        }

        if (!string.IsNullOrWhiteSpace(query.SFCLike))
        {
            query.SFCLike = $"{query.SFCLike}%";
            sqlBuilder.Where("SFC Like @SFCLike");
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

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            sqlBuilder.Where("Location = @Location");
        }

        if (!string.IsNullOrWhiteSpace(query.LocationLike))
        {
            query.LocationLike = $"{query.LocationLike}%";
            sqlBuilder.Where("Location Like @LocationLike");
        }

        if (!string.IsNullOrWhiteSpace(query.CirculationBarCode))
        {
            sqlBuilder.Where("CirculationBarCode = @CirculationBarCode");
        }

        if (!string.IsNullOrWhiteSpace(query.CirculationBarCodeLike))
        {
            query.CirculationBarCodeLike = $"{query.CirculationBarCodeLike}%";
            sqlBuilder.Where("CirculationBarCode Like @CirculationBarCodeLike");
        }

        if (query.CirculationWorkOrderId.HasValue)
        {
            sqlBuilder.Where("CirculationWorkOrderId = @CirculationWorkOrderId");
        }

        if (query.CirculationWorkOrderIds != null && query.CirculationWorkOrderIds.Any())
        {
            sqlBuilder.Where("CirculationWorkOrderId IN @CirculationWorkOrderIds");
        }

        if (query.CirculationProductId.HasValue)
        {
            sqlBuilder.Where("CirculationProductId = @CirculationProductId");
        }

        if (query.CirculationProductIds != null && query.CirculationProductIds.Any())
        {
            sqlBuilder.Where("CirculationProductId IN @CirculationProductIds");
        }

        if (query.CirculationMainProductId.HasValue)
        {
            sqlBuilder.Where("CirculationMainProductId = @CirculationMainProductId");
        }

        if (query.CirculationMainProductIds != null && query.CirculationMainProductIds.Any())
        {
            sqlBuilder.Where("CirculationMainProductId IN @CirculationMainProductIds");
        }

        if (query.CirculationMainSupplierId.HasValue)
        {
            sqlBuilder.Where("CirculationMainSupplierId = @CirculationMainSupplierId");
        }

        if (query.CirculationMainSupplierIds != null && query.CirculationMainSupplierIds.Any())
        {
            sqlBuilder.Where("CirculationMainSupplierId IN @CirculationMainSupplierIds");
        }

        if (query.CirculationQtyMin.HasValue)
        {
            sqlBuilder.Where("CirculationQty >= @CirculationQtyMin");
        }

        if (query.CirculationQtyMax.HasValue)
        {
            sqlBuilder.Where("CirculationQty <= @CirculationQtyMax");
        }

        if (query.CirculationType.HasValue)
        {
            sqlBuilder.Where("CirculationType = @CirculationType");
        }

        if (query.CirculationTypes?.Any() == true)
        {
            sqlBuilder.Where("CirculationType IN @CirculationTypes");
        }

        if (!string.IsNullOrWhiteSpace(query.CirculationTypeLike))
        {
            query.CirculationTypeLike = $"{query.CirculationTypeLike}%";
            sqlBuilder.Where("CirculationType Like @CirculationTypeLike");
        }

        if (query.IsDisassemble.HasValue)
        {
            sqlBuilder.Where("IsDisassemble = @IsDisassemble");
        }

        if (query.IsDisassembles != null && query.IsDisassembles.Any())
        {
            sqlBuilder.Where("IsDisassemble IN @IsDisassembles");
        }

        if (!string.IsNullOrWhiteSpace(query.DisassembledBy))
        {
            sqlBuilder.Where("DisassembledBy = @DisassembledBy");
        }

        if (!string.IsNullOrWhiteSpace(query.DisassembledByLike))
        {
            query.DisassembledByLike = $"{query.DisassembledByLike}%";
            sqlBuilder.Where("DisassembledBy Like @DisassembledByLike");
        }

        if (query.DisassembledOnStart.HasValue)
        {
            sqlBuilder.Where("DisassembledOn >= @DisassembledOnStart");
        }

        if (query.DisassembledOnEnd.HasValue)
        {
            sqlBuilder.Where("DisassembledOn <= @DisassembledOnEnd");
        }

        if (query.SubstituteId.HasValue)
        {
            sqlBuilder.Where("SubstituteId = @SubstituteId");
        }

        if (query.SubstituteIds != null && query.SubstituteIds.Any())
        {
            sqlBuilder.Where("SubstituteId IN @SubstituteIds");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy = @CreatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedByLike))
        {
            query.CreatedByLike = $"{query.CreatedByLike}%";
            sqlBuilder.Where("CreatedBy Like @CreatedByLike");
        }

        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedBy))
        {
            sqlBuilder.Where("UpdatedBy = @UpdatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedByLike))
        {
            query.UpdatedByLike = $"{query.UpdatedByLike}%";
            sqlBuilder.Where("UpdatedBy Like @UpdatedByLike");
        }

        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sqlBuilder.Where("Name = @Name");
        }

        if (!string.IsNullOrWhiteSpace(query.NameLike))
        {
            query.NameLike = $"{query.NameLike}%";
            sqlBuilder.Where("Name Like @NameLike");
        }

        if (!string.IsNullOrWhiteSpace(query.ModelCode))
        {
            sqlBuilder.Where("ModelCode = @ModelCode");
        }

        if (!string.IsNullOrWhiteSpace(query.ModelCodeLike))
        {
            query.ModelCodeLike = $"{query.ModelCodeLike}%";
            sqlBuilder.Where("ModelCode Like @ModelCodeLike");
        }

        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcCirculationQuery query)
    {
        sqlBuilder.Where("IsDeleted = 0");

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids?.Any() == true)
        {
            sqlBuilder.Where("Id IN @Ids");
        }

        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds?.Any() == true)
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

        if (query.FeedingPointId.HasValue)
        {
            sqlBuilder.Where("FeedingPointId = @FeedingPointId");
        }

        if (query.FeedingPointIds != null && query.FeedingPointIds.Any())
        {
            sqlBuilder.Where("FeedingPointId IN @FeedingPointIds");
        }

        if (!string.IsNullOrWhiteSpace(query.SFC))
        {
            sqlBuilder.Where("SFC = @SFC");
        }

        if (!string.IsNullOrWhiteSpace(query.SFCLike))
        {
            query.SFCLike = $"{query.SFCLike}%";
            sqlBuilder.Where("SFC Like @SFCLike");
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

        if (!string.IsNullOrWhiteSpace(query.Location))
        {
            sqlBuilder.Where("Location = @Location");
        }

        if (!string.IsNullOrWhiteSpace(query.LocationLike))
        {
            query.LocationLike = $"{query.LocationLike}%";
            sqlBuilder.Where("Location Like @LocationLike");
        }

        if (!string.IsNullOrWhiteSpace(query.CirculationBarCode))
        {
            sqlBuilder.Where("CirculationBarCode = @CirculationBarCode");
        }

        if (!string.IsNullOrWhiteSpace(query.CirculationBarCodeLike))
        {
            query.CirculationBarCodeLike = $"{query.CirculationBarCodeLike}%";
            sqlBuilder.Where("CirculationBarCode Like @CirculationBarCodeLike");
        }

        if (query.CirculationWorkOrderId.HasValue)
        {
            sqlBuilder.Where("CirculationWorkOrderId = @CirculationWorkOrderId");
        }

        if (query.CirculationWorkOrderIds != null && query.CirculationWorkOrderIds.Any())
        {
            sqlBuilder.Where("CirculationWorkOrderId IN @CirculationWorkOrderIds");
        }

        if (query.CirculationProductId.HasValue)
        {
            sqlBuilder.Where("CirculationProductId = @CirculationProductId");
        }

        if (query.CirculationProductIds != null && query.CirculationProductIds.Any())
        {
            sqlBuilder.Where("CirculationProductId IN @CirculationProductIds");
        }

        if (query.CirculationMainProductId.HasValue)
        {
            sqlBuilder.Where("CirculationMainProductId = @CirculationMainProductId");
        }

        if (query.CirculationMainProductIds != null && query.CirculationMainProductIds.Any())
        {
            sqlBuilder.Where("CirculationMainProductId IN @CirculationMainProductIds");
        }

        if (query.CirculationMainSupplierId.HasValue)
        {
            sqlBuilder.Where("CirculationMainSupplierId = @CirculationMainSupplierId");
        }

        if (query.CirculationMainSupplierIds != null && query.CirculationMainSupplierIds.Any())
        {
            sqlBuilder.Where("CirculationMainSupplierId IN @CirculationMainSupplierIds");
        }

        if (query.CirculationQtyMin.HasValue)
        {
            sqlBuilder.Where("CirculationQty >= @CirculationQtyMin");
        }

        if (query.CirculationQtyMax.HasValue)
        {
            sqlBuilder.Where("CirculationQty <= @CirculationQtyMax");
        }

        if (query.CirculationType.HasValue)
        {
            sqlBuilder.Where("CirculationType = @CirculationType");
        }

        if (query.CirculationTypes?.Any() == true)
        {
            sqlBuilder.Where("CirculationType IN @CirculationTypes");
        }

        if (query.IsDisassemble.HasValue)
        {
            sqlBuilder.Where("IsDisassemble = @IsDisassemble");
        }

        if (query.IsDisassembles != null && query.IsDisassembles.Any())
        {
            sqlBuilder.Where("IsDisassemble IN @IsDisassembles");
        }

        if (!string.IsNullOrWhiteSpace(query.DisassembledBy))
        {
            sqlBuilder.Where("DisassembledBy = @DisassembledBy");
        }

        if (!string.IsNullOrWhiteSpace(query.DisassembledByLike))
        {
            query.DisassembledByLike = $"{query.DisassembledByLike}%";
            sqlBuilder.Where("DisassembledBy Like @DisassembledByLike");
        }

        if (query.DisassembledOnStart.HasValue)
        {
            sqlBuilder.Where("DisassembledOn >= @DisassembledOnStart");
        }

        if (query.DisassembledOnEnd.HasValue)
        {
            sqlBuilder.Where("DisassembledOn <= @DisassembledOnEnd");
        }

        if (query.SubstituteId.HasValue)
        {
            sqlBuilder.Where("SubstituteId = @SubstituteId");
        }

        if (query.SubstituteIds != null && query.SubstituteIds.Any())
        {
            sqlBuilder.Where("SubstituteId IN @SubstituteIds");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedBy))
        {
            sqlBuilder.Where("CreatedBy = @CreatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.CreatedByLike))
        {
            query.CreatedByLike = $"{query.CreatedByLike}%";
            sqlBuilder.Where("CreatedBy Like @CreatedByLike");
        }

        if (query.CreatedOnStart.HasValue)
        {
            sqlBuilder.Where("CreatedOn >= @CreatedOnStart");
        }

        if (query.CreatedOnEnd.HasValue)
        {
            sqlBuilder.Where("CreatedOn <= @CreatedOnEnd");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedBy))
        {
            sqlBuilder.Where("UpdatedBy = @UpdatedBy");
        }

        if (!string.IsNullOrWhiteSpace(query.UpdatedByLike))
        {
            query.UpdatedByLike = $"{query.UpdatedByLike}%";
            sqlBuilder.Where("UpdatedBy Like @UpdatedByLike");
        }

        if (query.UpdatedOnStart.HasValue)
        {
            sqlBuilder.Where("UpdatedOn >= @UpdatedOnStart");
        }

        if (query.UpdatedOnEnd.HasValue)
        {
            sqlBuilder.Where("UpdatedOn <= @UpdatedOnEnd");
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sqlBuilder.Where("Name = @Name");
        }

        if (!string.IsNullOrWhiteSpace(query.NameLike))
        {
            query.NameLike = $"{query.NameLike}%";
            sqlBuilder.Where("Name Like @NameLike");
        }

        if (!string.IsNullOrWhiteSpace(query.ModelCode))
        {
            sqlBuilder.Where("ModelCode = @ModelCode");
        }

        if (!string.IsNullOrWhiteSpace(query.ModelCodeLike))
        {
            query.ModelCodeLike = $"{query.ModelCodeLike}%";
            sqlBuilder.Where("ModelCode Like @ModelCodeLike");
        }

        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}