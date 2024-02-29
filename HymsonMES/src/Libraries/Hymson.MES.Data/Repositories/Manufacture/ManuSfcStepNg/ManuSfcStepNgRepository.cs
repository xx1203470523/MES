/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-18 04:12:10
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture;

/// <summary>
/// 条码步骤ng信息记录表仓储
/// </summary>
public partial class ManuSfcStepNgRepository :BaseRepository, IManuSfcStepNgRepository
{
    public ManuSfcStepNgRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
    {
    }


    #region 方法


    #region 查询

    /// <summary>
    /// 单条数据查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<ManuSfcStepNgEntity> GetOneAsync(ManuSfcStepNgQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryFirstOrDefaultAsync<ManuSfcStepNgEntity>(templateData.RawSql, templateData.Parameters);
    }

    /// <summary>
    /// 数据集查询
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcStepNgEntity>> GetListAsync(ManuSfcStepNgQuery query)
    {
        var sqlBuilder = new SqlBuilder();

        var templateData = sqlBuilder.AddTemplate(GetListSqlTemplate);

        WhereFill(sqlBuilder, query);

        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();

        return await conn.QueryAsync<ManuSfcStepNgEntity>(templateData.RawSql, templateData.Parameters);
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
    public async Task<ManuSfcStepNgEntity> GetByIdAsync(long id)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryFirstOrDefaultAsync<ManuSfcStepNgEntity>(GetByIdSql, new { Id=id});
    }

    /// <summary>
    /// 根据IDs批量获取数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcStepNgEntity>> GetByIdsAsync(long[] ids) 
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ManuSfcStepNgEntity>(GetByIdsSql, new { Ids = ids});
    }

    /// <summary>
    /// 根据BarCodeStepId批量获取数据
    /// </summary>
    /// <param name="manuSfcStepIdsNgQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcStepNgEntity>> GetByBarCodeStepIdsAsync(ManuSfcStepIdsNgQuery manuSfcStepIdsNgQuery)
    {
        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<ManuSfcStepNgEntity>(GetByBarCodeStepIdsSql, manuSfcStepIdsNgQuery);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcStepNgEntity>> GetPagedInfoAsync(ManuSfcStepNgPagedQuery pageQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
        sqlBuilder.Where("IsDeleted=0");
        sqlBuilder.Select("*");

        //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
        //{
        //    sqlBuilder.Where("SiteCode=@SiteCode");
        //}

       
        var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
        sqlBuilder.AddParameters(pageQuery);

        using var conn = GetMESDbConnection();
        var manuSfcStepNgEntities = await conn.QueryAsync<ManuSfcStepNgEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        return new PagedInfo<ManuSfcStepNgEntity>(manuSfcStepNgEntities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
    }

    /// <summary>
    /// 查询List
    /// </summary>
    /// <param name="manuSfcStepNgQuery"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ManuSfcStepNgEntity>> GetManuSfcStepNgEntitiesAsync(ManuSfcStepNgQuery manuSfcStepNgQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var template = sqlBuilder.AddTemplate(GetManuSfcStepNgEntitiesSqlTemplate);
        using var conn = GetMESDbConnection();
        var manuSfcStepNgEntities = await conn.QueryAsync<ManuSfcStepNgEntity>(template.RawSql, manuSfcStepNgQuery);
        return manuSfcStepNgEntities;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="manuSfcStepNgEntity"></param>
    /// <returns></returns>
    public async Task<int> InsertAsync(ManuSfcStepNgEntity manuSfcStepNgEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertSql, manuSfcStepNgEntity);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    /// <param name="manuSfcStepNgEntitys"></param>
    /// <returns></returns>
    public async Task<int> InsertsAsync(List<ManuSfcStepNgEntity> manuSfcStepNgEntitys)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(InsertsSql, manuSfcStepNgEntitys);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="manuSfcStepNgEntity"></param>
    /// <returns></returns>
    public async Task<int> UpdateAsync(ManuSfcStepNgEntity manuSfcStepNgEntity)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdateSql, manuSfcStepNgEntity);
    }

    /// <summary>
    /// 批量更新
    /// </summary>
    /// <param name="manuSfcStepNgEntitys"></param>
    /// <returns></returns>
    public async Task<int> UpdatesAsync(List<ManuSfcStepNgEntity> manuSfcStepNgEntitys)
    {
        using var conn = GetMESDbConnection();
        return await conn.ExecuteAsync(UpdatesSql, manuSfcStepNgEntitys);
    }
    #endregion
}


public partial class ManuSfcStepNgRepository
{
    #region 查询

    const string GetOneSqlTemplate = "SELECT * FROM `manu_sfc_step_ng` /**where**/ LIMIT 1;";

    const string GetListSqlTemplate = "SELECT * FROM `manu_sfc_step_ng` /**where**/;";

    const string GetPagedSqlTemplate = "SELECT * FROM `manu_sfc_step_ng` /**where**/ /**orderby**/ LIMIT @Offset,@Rows;";

    const string GetCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_step_ng` /**where**/;";

    #endregion


    #region 
    const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step_ng` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_step_ng` /**where**/ ";
    const string GetManuSfcStepNgEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_step_ng` /**where**/  ";

    const string InsertSql = "INSERT INTO `manu_sfc_step_ng`(  `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @BarCodeStepId, @UnqualifiedCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
    const string InsertsSql = "INSERT INTO `manu_sfc_step_ng`(  `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @BarCodeStepId, @UnqualifiedCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

    const string UpdateSql = "UPDATE `manu_sfc_step_ng` SET   BarCodeStepId = @BarCodeStepId, UnqualifiedCode = @UnqualifiedCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
    const string UpdatesSql = "UPDATE `manu_sfc_step_ng` SET   BarCodeStepId = @BarCodeStepId, UnqualifiedCode = @UnqualifiedCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

    const string DeleteSql = "UPDATE `manu_sfc_step_ng` SET IsDeleted = Id WHERE Id = @Id ";
    const string DeletesSql = "UPDATE `manu_sfc_step_ng` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

    const string GetByIdSql = @"SELECT 
                               `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_step_ng`  WHERE Id = @Id ";
    const string GetByIdsSql = @"SELECT 
                                          `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_step_ng`  WHERE Id IN @Ids ";

    const string GetByBarCodeStepIdsSql = @"SELECT 
                                          `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_step_ng`  WHERE BarCodeStepId IN @BarCodeStepIds AND SiteId = @SiteId";
    #endregion

}



/// <summary>
/// <para>@层级：仓储层</para>
/// <para>@作用：通用操作</para>
/// <para>@描述：条码步骤ng信息记录表;</para>
/// <para>@作者：Jim</para>
/// <para>@创建时间：2024-2-29</para>
/// </summary>
public partial class ManuSfcStepNgRepository
{

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcStepNgPagedQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (!string.IsNullOrWhiteSpace(query.BarCodeStepId))
        {
            sqlBuilder.Where("BarCodeStepId = @BarCodeStepId");
        }

        if (!string.IsNullOrWhiteSpace(query.BarCodeStepIdLike))
        {
            query.BarCodeStepIdLike = $"{query.BarCodeStepIdLike}%";
            sqlBuilder.Where("BarCodeStepId Like @BarCodeStepIdLike");
        }


        if (!string.IsNullOrWhiteSpace(query.UnqualifiedCode))
        {
            sqlBuilder.Where("UnqualifiedCode = @UnqualifiedCode");
        }

        if (!string.IsNullOrWhiteSpace(query.UnqualifiedCodeLike))
        {
            query.UnqualifiedCodeLike = $"{query.UnqualifiedCodeLike}%";
            sqlBuilder.Where("UnqualifiedCode Like @UnqualifiedCodeLike");
        }


        if (query.IsReplenish.HasValue)
        {
            sqlBuilder.Where("IsReplenish = @IsReplenish");
        }

        if (query.IsReplenishs != null && query.IsReplenishs.Any())
        {
            sqlBuilder.Where("IsReplenish IN @IsReplenishs");
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


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

    /// <summary>
    /// 根据查询对象填充Where条件
    /// </summary>
    /// <param name="query">查询对象</param>
    /// <returns></returns>
    private static SqlBuilder WhereFill(SqlBuilder sqlBuilder, ManuSfcStepNgQuery query)
    {

        if (query.Id.HasValue)
        {
            sqlBuilder.Where("Id = @Id");
        }

        if (query.Ids != null && query.Ids.Any())
        {
            sqlBuilder.Where("Id IN @Ids");
        }


        if (!string.IsNullOrWhiteSpace(query.BarCodeStepId))
        {
            sqlBuilder.Where("BarCodeStepId = @BarCodeStepId");
        }

        if (!string.IsNullOrWhiteSpace(query.BarCodeStepIdLike))
        {
            query.BarCodeStepIdLike = $"{query.BarCodeStepIdLike}%";
            sqlBuilder.Where("BarCodeStepId Like @BarCodeStepIdLike");
        }


        if (!string.IsNullOrWhiteSpace(query.UnqualifiedCode))
        {
            sqlBuilder.Where("UnqualifiedCode = @UnqualifiedCode");
        }

        if (!string.IsNullOrWhiteSpace(query.UnqualifiedCodeLike))
        {
            query.UnqualifiedCodeLike = $"{query.UnqualifiedCodeLike}%";
            sqlBuilder.Where("UnqualifiedCode Like @UnqualifiedCodeLike");
        }


        if (query.IsReplenish.HasValue)
        {
            sqlBuilder.Where("IsReplenish = @IsReplenish");
        }

        if (query.IsReplenishs != null && query.IsReplenishs.Any())
        {
            sqlBuilder.Where("IsReplenish IN @IsReplenishs");
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


        sqlBuilder.Where("IsDeleted = 0");

        return sqlBuilder;
    }

}