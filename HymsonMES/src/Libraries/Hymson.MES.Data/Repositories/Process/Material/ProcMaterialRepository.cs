using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Hymson.WMS.Data.Repositories.Process;

namespace Hymson.MES.Data.Repositories.Process;

/// <summary>
/// 物料维护仓储
/// </summary>
public partial class ProcMaterialRepository : BaseRepository, IProcMaterialRepository
{
    private readonly ConnectionOptions _connectionOptions;
    private readonly IMemoryCache _memoryCache;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionOptions"></param>
    /// <param name="memoryCache"></param>
    public ProcMaterialRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
        _memoryCache = memoryCache;
    }



    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ProcMaterialEntity> GetOneAsync(ProcMaterialQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcMaterialEntity>> GetListAsync(ProcMaterialQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
    }

    #endregion

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
    /// <param name="param"></param>
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
    public async Task<ProcMaterialView> GetByIdAsync(long id, long SiteId)
    {
        var key = $"proc_material&proc_material_group&proc_process_route&proc_bom&{id}";
        return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialView>(GetViewByIdSql, new { Id = id, SiteId = SiteId });
        });
    }

    /// <summary>
    ///  根据ID获取数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ProcMaterialEntity> GetByIdAsync(long id)
    {
        var key = $"proc_material&{id}";
        return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetMaterialByIdSql, new { Id = id });
        });
    }

    /// <summary>
    /// 根据IDs批量获取数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcMaterialEntity>> GetByIdsAsync(long[] ids)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ProcMaterialEntity>(GetByIdsSql, new { ids = ids });
    }

    /// <summary>
    /// 根据IDs批量获取数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcMaterialEntity>> GetByIdsAsync(IEnumerable<long> ids)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ProcMaterialEntity>(GetByIdsSql, new { ids });
    }

    /// <summary>
    /// 批量获取数据
    /// </summary>
    /// <param name="siteId"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcMaterialEntity>> GetBySiteIdAsync(long siteId)
    {
        var key = $"proc_material&all&{siteId}";
        return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcMaterialEntity>(GetBySiteIdSql, new { SiteId = siteId });
        });

    }

    /// <summary>
    /// 根据物料组ID查询物料
    /// </summary>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcMaterialEntity>> GetByGroupIdsAsync(long[] groupIds)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ProcMaterialEntity>(GetByGroupIdSql, new { groupIds = groupIds });
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="procMaterialPagedQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
        sqlBuilder.Where("IsDeleted = 0");
        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.OrderBy("UpdatedOn DESC, Id DESC");
        sqlBuilder.Select("*");

        if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialCode))
        {
            procMaterialPagedQuery.MaterialCode = $"%{procMaterialPagedQuery.MaterialCode}%";
            sqlBuilder.Where(" MaterialCode like @MaterialCode ");
        }
        if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialName))
        {
            procMaterialPagedQuery.MaterialName = $"%{procMaterialPagedQuery.MaterialName}%";
            sqlBuilder.Where(" MaterialName like @MaterialName ");
        }
        if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.Version))
        {
            procMaterialPagedQuery.Version = $"%{procMaterialPagedQuery.Version}%";
            sqlBuilder.Where(" Version like @Version ");
        }
        if (procMaterialPagedQuery.GroupId != null)
        {
            sqlBuilder.Where(" GroupId = @GroupId ");
        }
        if (procMaterialPagedQuery.Status.HasValue)
        {
            sqlBuilder.Where(" Status = @Status ");
        }
        if (procMaterialPagedQuery.Origin.HasValue)
        {
            sqlBuilder.Where(" Origin = @Origin ");
        }
        if (procMaterialPagedQuery.BuyTypes != null && procMaterialPagedQuery.BuyTypes.Length > 0)
        {
            sqlBuilder.Where(" BuyType IN @BuyTypes ");
        }

        var offSet = (procMaterialPagedQuery.PageIndex - 1) * procMaterialPagedQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = procMaterialPagedQuery.PageSize });
        sqlBuilder.AddParameters(procMaterialPagedQuery);

        using var conn = GetMESDbConnection();
        var procMaterialEntitiesTask = conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var procMaterialEntities = await procMaterialEntitiesTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<ProcMaterialEntity>(procMaterialEntities, procMaterialPagedQuery.PageIndex, procMaterialPagedQuery.PageSize, totalCount);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="procMaterialPagedQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ProcMaterialEntity>> GetPagedInfoForGroupAsync(ProcMaterialPagedQuery procMaterialPagedQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
        sqlBuilder.Where("IsDeleted=0");
        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Select("*");

        if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialCode))
        {
            procMaterialPagedQuery.MaterialCode = $"%{procMaterialPagedQuery.MaterialCode}%";
            sqlBuilder.Where(" MaterialCode like @MaterialCode ");
        }
        if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.MaterialName))
        {
            procMaterialPagedQuery.MaterialName = $"%{procMaterialPagedQuery.MaterialName}%";
            sqlBuilder.Where(" MaterialName like %@MaterialName% ");
        }
        if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.Version))
        {
            procMaterialPagedQuery.Version = $"%{procMaterialPagedQuery.Version}%";
            sqlBuilder.Where(" Version like @Version ");
        }
        if (procMaterialPagedQuery.GroupId != null)
        {
            if (procMaterialPagedQuery.GroupId == 0)
            {
                //predicate = predicate.And(it => it.GroupId == 0);
                sqlBuilder.Where(" GroupId = 0 ");
            }
            else
            {
                sqlBuilder.Where(" ( GroupId = 0 or GroupId =@GroupId ) ");
            }

            //sqlBuilder.Where(" GroupId = @GroupId ");
        }
        if (procMaterialPagedQuery.Status.HasValue)
        {
            sqlBuilder.Where(" Status = @Status ");
        }
        if (procMaterialPagedQuery.Origin.HasValue)
        {
            sqlBuilder.Where(" Origin = @Origin ");
        }

        var offSet = (procMaterialPagedQuery.PageIndex - 1) * procMaterialPagedQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = procMaterialPagedQuery.PageSize });
        sqlBuilder.AddParameters(procMaterialPagedQuery);

        using var conn = GetMESDbConnection();
        var procMaterialEntitiesTask = conn.QueryAsync<ProcMaterialEntity>(templateData.RawSql, templateData.Parameters);
        var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        var procMaterialEntities = await procMaterialEntitiesTask;
        var totalCount = await totalCountTask;
        return new PagedInfo<ProcMaterialEntity>(procMaterialEntities, procMaterialPagedQuery.PageIndex, procMaterialPagedQuery.PageSize, totalCount);
    }

    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="procMaterialQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ProcMaterialEntity>> GetProcMaterialEntitiesAsync(ProcMaterialQuery procMaterialQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetProcMaterialEntitiesSqlTemplate);
        sqlBuilder.Where("IsDeleted=0");
        sqlBuilder.Where("SiteId = @SiteId");
        sqlBuilder.Select("*");

        if (!string.IsNullOrWhiteSpace(procMaterialQuery.MaterialCode))
        {
            procMaterialQuery.MaterialCode = $"%{procMaterialQuery.MaterialCode}%";
            sqlBuilder.Where(" MaterialCode like @MaterialCode ");
        }
        if (!string.IsNullOrWhiteSpace(procMaterialQuery.Version))
        {
            procMaterialQuery.Version = $"%{procMaterialQuery.Version}%";
            sqlBuilder.Where(" Version like @Version ");
        }
        sqlBuilder.AddParameters(procMaterialQuery);

        using var conn = GetMESDbConnection();
        var procMaterialEntities = await conn.QueryAsync<ProcMaterialEntity>(template.RawSql, template.Parameters);
        return procMaterialEntities;
    }

    /// <summary>
    /// 根据Code查询对象
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ProcMaterialEntity> GetByCodeAsync(ProcMaterialQuery query)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<ProcMaterialEntity>(GetByCodeSql, query);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="procMaterialEntity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(ProcMaterialEntity procMaterialEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, procMaterialEntity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="procMaterialEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ProcMaterialEntity procMaterialEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, procMaterialEntity);
    }

    /// <summary>
    /// 更新 同编码的其他物料设置为非当前版本
    /// </summary>
    /// <param name="procMaterialEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateSameMaterialCodeToNoVersionAsync(ProcMaterialEntity procMaterialEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSameMaterialCodeToNoVersionSql, procMaterialEntity);
    }

    /// <summary>
    /// 更新物料的物料组
    /// </summary>
    /// <param name="procMaterialEntitys"></param>
    /// <returns></returns>
    public async Task<int> UpdateProcMaterialGroupAsync(IEnumerable<ProcMaterialEntity> procMaterialEntitys)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateProcMaterialGroupSql, procMaterialEntitys);
    }

    /// <summary>
    /// 更新某物料组下的物料为未绑定的
    /// </summary>
    /// <param name="procMaterialEntitys"></param>
    /// <returns></returns>
    public async Task<int> UpdateProcMaterialUnboundAsync(long groupId)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateProcMaterialUnboundSql, new { GroupId = groupId });
    }
}

public partial class ProcMaterialRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `proc_material` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `proc_material` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `proc_material` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `proc_material` /**where**/;";

    #endregion


    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_material` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_material` /**where**/ ";
    const string GetProcMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_material` /**where**/  ";
    const string GetByCodeSql = "SELECT * FROM proc_material WHERE `IsDeleted` = 0 AND SiteId = @SiteId AND MaterialCode= @MaterialCode AND Version =@Version LIMIT 1";

    const string InsertSql = "INSERT INTO `proc_material`(  `Id`, `SiteId`, `GroupId`, `MaterialCode`, `MaterialName`, `Status`, `Origin`, `Version`, `IsDefaultVersion`, `Remark`, `BuyType`, `ProcessRouteId`, `BomId`, `Batch`, PackageNum, `Unit`, `SerialNumber`, `BaseTime`, `ConsumptionTolerance`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `ConsumeRatio`,`MaskCodeId` ) VALUES (   @Id, @SiteId, @GroupId, @MaterialCode, @MaterialName, @Status, @Origin, @Version, @IsDefaultVersion, @Remark, @BuyType, @ProcessRouteId, @BomId, @Batch, @PackageNum, @Unit, @SerialNumber, @BaseTime, @ConsumptionTolerance, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @ConsumeRatio,@MaskCodeId )  ";
    const string UpdateSql = "UPDATE `proc_material` SET  GroupId = @GroupId, MaterialName = @MaterialName, Status = @Status, Origin = @Origin, Version = @Version, Remark = @Remark, BuyType = @BuyType, ProcessRouteId = @ProcessRouteId, BomId = @BomId, Batch = @Batch, PackageNum = @PackageNum, Unit = @Unit, SerialNumber = @SerialNumber, BaseTime = @BaseTime, ConsumptionTolerance = @ConsumptionTolerance, IsDefaultVersion=@IsDefaultVersion, MaskCodeId=@MaskCodeId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn , ConsumeRatio=@ConsumeRatio  WHERE Id = @Id ";
    const string DeleteSql = "UPDATE `proc_material` SET IsDeleted = Id WHERE Id = @Id ";
    const string DeletesSql = "UPDATE `proc_material` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
    const string GetViewByIdSql = @"SELECT 
                                        g.`Id`,
                                        g.`SiteId`,
                                        o.GroupName,
                                        g.MaterialCode,
                                        g.MaterialName,
                                        g.Status, 
                                        g.Origin, 
                                        g.Version, 
                                        g.Remark, 
                                        g.BuyType, 
                                        p.Id AS ProcessRouteId, 
                                        p.Code As ProcessRouteCode,
                                        p.Name as ProcessRouteName,
                                        p.Version as ProcessRouteVersion, 
                                        q.Id as BomId, 
                                        q.BomCode as BomCode,
                                        q.BomName as BomName, 
                                        q.Version as BomVersion, 
                                        g.Batch as Batch, 
                                        g.PackageNum as PackageNum,
                                        g.Unit as Unit, 
                                        g.SerialNumber,
                                        g.BaseTime,
                                        g.ConsumptionTolerance,
                                        g.IsDefaultVersion,
                                        g.UpdatedBy,
                                        g.UpdatedOn,
                                        g.ConsumeRatio,
                                        g.MaskCodeId
                            FROM `proc_material` g 
                            LEFT JOIN proc_material_group o on o.Id=g.GroupId
                            LEFT JOIN proc_process_route p on g.ProcessRouteId = p.Id
                            LEFT JOIN proc_bom q on g.BomId = q.Id 
                            WHERE g.Id = @Id and g.SiteId=@SiteId ";
    const string GetMaterialByIdSql = @"SELECT * FROM `proc_material`
                            WHERE Id = @Id";
    const string GetByIdsSql = @"SELECT * FROM `proc_material`
                            WHERE Id IN @ids and IsDeleted=0 ";
    const string GetBySiteIdSql = @"SELECT * FROM proc_material WHERE SiteId = @SiteId AND IsDeleted = 0 ";

    /// <summary>
    /// 根据物料组ID查询物料
    /// </summary>
    const string GetByGroupIdSql = @"SELECT * FROM `proc_material`
                            WHERE IsDeleted = 0 and GroupId IN @groupIds ";
    const string UpdateSameMaterialCodeToNoVersionSql = "UPDATE `proc_material` SET IsDefaultVersion= 0 WHERE MaterialCode= @MaterialCode and IsDeleted = 0 ";

    /// <summary>
    /// 更改物料的物料组
    /// </summary>
    const string UpdateProcMaterialGroupSql = "UPDATE `proc_material` SET GroupId= @GroupId, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

    /// <summary>
    /// 更新某物料组下的物料为未绑定物料组
    /// </summary>
    const string UpdateProcMaterialUnboundSql = "UPDATE `proc_material` SET GroupId= 0 WHERE GroupId = @GroupId ";
}



/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：物料维护表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-29</para>
/// </summary>
public partial class ProcMaterialRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ProcMaterialPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.GroupId.HasValue)
        {
            sqlBuilder.Where("GroupId = @GroupId");
        }

        if (query.GroupIds != null && query.GroupIds.Any())
        {
            sqlBuilder.Where("GroupId IN @GroupIds");
        }


        if (!string.IsNullOrWhiteSpace(query.MaterialCode))
        {
            sqlBuilder.Where("MaterialCode = @MaterialCode");
        }

        if (!string.IsNullOrWhiteSpace(query.MaterialCodeLike))
        {
            query.MaterialCodeLike = $"{query.MaterialCodeLike}%";
            sqlBuilder.Where("MaterialCode Like @MaterialCodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.MaterialName))
        {
            sqlBuilder.Where("MaterialName = @MaterialName");
        }

        if (!string.IsNullOrWhiteSpace(query.MaterialNameLike))
        {
            query.MaterialNameLike = $"{query.MaterialNameLike}%";
            sqlBuilder.Where("MaterialName Like @MaterialNameLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }

        if (query.Statuss != null && query.Statuss.Any())
        {
            sqlBuilder.Where("Status IN @Statuss");
        }


        if (query.Origin.HasValue)
        {
            sqlBuilder.Where("Origin = @Origin");
        }

        if (query.Origins != null && query.Origins.Any())
        {
            sqlBuilder.Where("Origin IN @Origins");
        }


        if (!string.IsNullOrWhiteSpace(query.Version))
        {
            sqlBuilder.Where("Version = @Version");
        }

        if (!string.IsNullOrWhiteSpace(query.VersionLike))
        {
            query.VersionLike = $"{query.VersionLike}%";
            sqlBuilder.Where("Version Like @VersionLike");
        }


        if (query.IsDefaultVersion.HasValue)
        {
            sqlBuilder.Where("IsDefaultVersion = @IsDefaultVersion");
        }

        if (query.IsDefaultVersions != null && query.IsDefaultVersions.Any())
        {
            sqlBuilder.Where("IsDefaultVersion IN @IsDefaultVersions");
        }


        if (query.BuyType.HasValue)
        {
            sqlBuilder.Where("BuyType = @BuyType");
        }

        if (query.BuyTypes != null && query.BuyTypes.Any())
        {
            sqlBuilder.Where("BuyType IN @BuyTypes");
        }


        if (query.ProcessRouteId.HasValue)
        {
            sqlBuilder.Where("ProcessRouteId = @ProcessRouteId");
        }

        if (query.ProcessRouteIds != null && query.ProcessRouteIds.Any())
        {
            sqlBuilder.Where("ProcessRouteId IN @ProcessRouteIds");
        }


        if (query.BomId.HasValue)
        {
            sqlBuilder.Where("BomId = @BomId");
        }

        if (query.BomIds != null && query.BomIds.Any())
        {
            sqlBuilder.Where("BomId IN @BomIds");
        }


        if (query.Batch.HasValue)
        {
            sqlBuilder.Where("Batch = @Batch");
        }

        if (query.Batchs != null && query.Batchs.Any())
        {
            sqlBuilder.Where("Batch IN @Batchs");
        }


        if (query.PackageNum.HasValue)
        {
            sqlBuilder.Where("PackageNum = @PackageNum");
        }

        if (query.PackageNums != null && query.PackageNums.Any())
        {
            sqlBuilder.Where("PackageNum IN @PackageNums");
        }


        if (!string.IsNullOrWhiteSpace(query.Unit))
        {
            sqlBuilder.Where("Unit = @Unit");
        }

        if (!string.IsNullOrWhiteSpace(query.UnitLike))
        {
            query.UnitLike = $"{query.UnitLike}%";
            sqlBuilder.Where("Unit Like @UnitLike");
        }


        if (query.SerialNumber.HasValue)
        {
            sqlBuilder.Where("SerialNumber = @SerialNumber");
        }

        if (query.SerialNumbers != null && query.SerialNumbers.Any())
        {
            sqlBuilder.Where("SerialNumber IN @SerialNumbers");
        }


        if (!string.IsNullOrWhiteSpace(query.ValidationMaskGroup))
        {
            sqlBuilder.Where("ValidationMaskGroup = @ValidationMaskGroup");
        }

        if (!string.IsNullOrWhiteSpace(query.ValidationMaskGroupLike))
        {
            query.ValidationMaskGroupLike = $"{query.ValidationMaskGroupLike}%";
            sqlBuilder.Where("ValidationMaskGroup Like @ValidationMaskGroupLike");
        }


        if (query.BaseTime.HasValue)
        {
            sqlBuilder.Where("BaseTime = @BaseTime");
        }

        if (query.BaseTimes != null && query.BaseTimes.Any())
        {
            sqlBuilder.Where("BaseTime IN @BaseTimes");
        }


        if (query.ConsumptionTolerance.HasValue)
        {
            sqlBuilder.Where("ConsumptionTolerance = @ConsumptionTolerance");
        }

        if (query.ConsumptionTolerances != null && query.ConsumptionTolerances.Any())
        {
            sqlBuilder.Where("ConsumptionTolerance IN @ConsumptionTolerances");
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


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        if (query.ConsumeRatioMin.HasValue)
        {
            sqlBuilder.Where("ConsumeRatio >= @ConsumeRatioMin");
        }

        if (query.ConsumeRatioMax.HasValue)
        {
            sqlBuilder.Where("ConsumeRatio <= @ConsumeRatioMax");
        }


        if (query.MaskCodeId.HasValue)
        {
            sqlBuilder.Where("MaskCodeId = @MaskCodeId");
        }

        if (query.MaskCodeIds != null && query.MaskCodeIds.Any())
        {
            sqlBuilder.Where("MaskCodeId IN @MaskCodeIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ProcMaterialQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (query.GroupId.HasValue)
        {
            sqlBuilder.Where("GroupId = @GroupId");
        }

        if (query.GroupIds != null && query.GroupIds.Any())
        {
            sqlBuilder.Where("GroupId IN @GroupIds");
        }


        if (!string.IsNullOrWhiteSpace(query.MaterialCode))
        {
            sqlBuilder.Where("MaterialCode = @MaterialCode");
        }

        if (!string.IsNullOrWhiteSpace(query.MaterialCodeLike))
        {
            query.MaterialCodeLike = $"{query.MaterialCodeLike}%";
            sqlBuilder.Where("MaterialCode Like @MaterialCodeLike");
        }


        if (!string.IsNullOrWhiteSpace(query.MaterialName))
        {
            sqlBuilder.Where("MaterialName = @MaterialName");
        }

        if (!string.IsNullOrWhiteSpace(query.MaterialNameLike))
        {
            query.MaterialNameLike = $"{query.MaterialNameLike}%";
            sqlBuilder.Where("MaterialName Like @MaterialNameLike");
        }


        if (query.Status.HasValue)
        {
            sqlBuilder.Where("Status = @Status");
        }

        if (query.Statuss != null && query.Statuss.Any())
        {
            sqlBuilder.Where("Status IN @Statuss");
        }


        if (query.Origin.HasValue)
        {
            sqlBuilder.Where("Origin = @Origin");
        }

        if (query.Origins != null && query.Origins.Any())
        {
            sqlBuilder.Where("Origin IN @Origins");
        }


        if (!string.IsNullOrWhiteSpace(query.Version))
        {
            sqlBuilder.Where("Version = @Version");
        }

        if (!string.IsNullOrWhiteSpace(query.VersionLike))
        {
            query.VersionLike = $"{query.VersionLike}%";
            sqlBuilder.Where("Version Like @VersionLike");
        }


        if (query.IsDefaultVersion.HasValue)
        {
            sqlBuilder.Where("IsDefaultVersion = @IsDefaultVersion");
        }

        if (query.IsDefaultVersions != null && query.IsDefaultVersions.Any())
        {
            sqlBuilder.Where("IsDefaultVersion IN @IsDefaultVersions");
        }


        if (query.BuyType.HasValue)
        {
            sqlBuilder.Where("BuyType = @BuyType");
        }

        if (query.BuyTypes != null && query.BuyTypes.Any())
        {
            sqlBuilder.Where("BuyType IN @BuyTypes");
        }


        if (query.ProcessRouteId.HasValue)
        {
            sqlBuilder.Where("ProcessRouteId = @ProcessRouteId");
        }

        if (query.ProcessRouteIds != null && query.ProcessRouteIds.Any())
        {
            sqlBuilder.Where("ProcessRouteId IN @ProcessRouteIds");
        }


        if (query.BomId.HasValue)
        {
            sqlBuilder.Where("BomId = @BomId");
        }

        if (query.BomIds != null && query.BomIds.Any())
        {
            sqlBuilder.Where("BomId IN @BomIds");
        }


        if (query.Batch.HasValue)
        {
            sqlBuilder.Where("Batch = @Batch");
        }

        if (query.Batchs != null && query.Batchs.Any())
        {
            sqlBuilder.Where("Batch IN @Batchs");
        }


        if (query.PackageNum.HasValue)
        {
            sqlBuilder.Where("PackageNum = @PackageNum");
        }

        if (query.PackageNums != null && query.PackageNums.Any())
        {
            sqlBuilder.Where("PackageNum IN @PackageNums");
        }


        if (!string.IsNullOrWhiteSpace(query.Unit))
        {
            sqlBuilder.Where("Unit = @Unit");
        }

        if (!string.IsNullOrWhiteSpace(query.UnitLike))
        {
            query.UnitLike = $"{query.UnitLike}%";
            sqlBuilder.Where("Unit Like @UnitLike");
        }


        if (query.SerialNumber.HasValue)
        {
            sqlBuilder.Where("SerialNumber = @SerialNumber");
        }

        if (query.SerialNumbers != null && query.SerialNumbers.Any())
        {
            sqlBuilder.Where("SerialNumber IN @SerialNumbers");
        }


        if (!string.IsNullOrWhiteSpace(query.ValidationMaskGroup))
        {
            sqlBuilder.Where("ValidationMaskGroup = @ValidationMaskGroup");
        }

        if (!string.IsNullOrWhiteSpace(query.ValidationMaskGroupLike))
        {
            query.ValidationMaskGroupLike = $"{query.ValidationMaskGroupLike}%";
            sqlBuilder.Where("ValidationMaskGroup Like @ValidationMaskGroupLike");
        }


        if (query.BaseTime.HasValue)
        {
            sqlBuilder.Where("BaseTime = @BaseTime");
        }

        if (query.BaseTimes != null && query.BaseTimes.Any())
        {
            sqlBuilder.Where("BaseTime IN @BaseTimes");
        }


        if (query.ConsumptionTolerance.HasValue)
        {
            sqlBuilder.Where("ConsumptionTolerance = @ConsumptionTolerance");
        }

        if (query.ConsumptionTolerances != null && query.ConsumptionTolerances.Any())
        {
            sqlBuilder.Where("ConsumptionTolerance IN @ConsumptionTolerances");
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


        if (query.SiteId.HasValue)
        {
            sqlBuilder.Where("SiteId = @SiteId");
        }

        if (query.SiteIds != null && query.SiteIds.Any())
        {
            sqlBuilder.Where("SiteId IN @SiteIds");
        }


        if (query.ConsumeRatioMin.HasValue)
        {
            sqlBuilder.Where("ConsumeRatio >= @ConsumeRatioMin");
        }

        if (query.ConsumeRatioMax.HasValue)
        {
            sqlBuilder.Where("ConsumeRatio <= @ConsumeRatioMax");
        }


        if (query.MaskCodeId.HasValue)
        {
            sqlBuilder.Where("MaskCodeId = @MaskCodeId");
        }

        if (query.MaskCodeIds != null && query.MaskCodeIds.Any())
        {
            sqlBuilder.Where("MaskCodeId IN @MaskCodeIds");
        }


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展实现</para>
/// <para>@描述：物料维护表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-29</para>
/// </summary>
public partial class ProcMaterialRepository
{
}

/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：扩展SQL</para>
/// <para>@描述：物料维护表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-29</para>
/// </summary>
public partial class ProcMaterialRepository
{
}