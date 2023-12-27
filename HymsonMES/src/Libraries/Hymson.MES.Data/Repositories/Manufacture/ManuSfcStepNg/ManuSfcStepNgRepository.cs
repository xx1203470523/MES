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

    #region 扩展方法

    /// <summary>
    /// 联表分页查询
    /// </summary>
    /// <param name="pageQuery"></param>
    /// <returns></returns>
    public async Task<PagedInfo<ManuSfcStepNgEntity>> GetJoinPagedInfoAsync(ManuSfcStepNgPagedQuery pageQuery)
    {
        var sqlBuilder = new SqlBuilder();
        var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
        var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
        sqlBuilder.Where("mssn.IsDeleted=0");
        sqlBuilder.Select("mss.*");

        sqlBuilder.InnerJoin("manu_sfc_step mss ON mss.Id  = BarCodeStepId");

        #region WhereFill

        if (pageQuery.SFC != null)
        {
            sqlBuilder.Where("mss.SFC = @SFC");
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

        #endregion


        var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
        sqlBuilder.AddParameters(new { OffSet = offSet });
        sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
        sqlBuilder.AddParameters(pageQuery);

        using var conn = GetMESDbConnection();
        var manuSfcStepNgEntities = await conn.QueryAsync<ManuSfcStepNgEntity>(templateData.RawSql, templateData.Parameters);
        var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
        return new PagedInfo<ManuSfcStepNgEntity>(manuSfcStepNgEntities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
    }

    #endregion
}


public partial class ManuSfcStepNgRepository
{
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

    #region 扩展

    const string GetJoinPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step_ng` mssn /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
    const string GetJoinPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_step_ng` mssn /**where**/ ";

    #endregion
}
