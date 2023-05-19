/*
 *creator: Karl
 *
 *describe: 产出上报绑定物料 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-19 10:46:49
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产出上报绑定物料仓储
    /// </summary>
    public partial class ManuOutputBindMaterialRepository :BaseRepository, IManuOutputBindMaterialRepository
    {

        public ManuOutputBindMaterialRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuOutputBindMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuOutputBindMaterialEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuOutputBindMaterialEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuOutputBindMaterialEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuOutputBindMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuOutputBindMaterialEntity>> GetPagedInfoAsync(ManuOutputBindMaterialPagedQuery manuOutputBindMaterialPagedQuery)
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
           
            var offSet = (manuOutputBindMaterialPagedQuery.PageIndex - 1) * manuOutputBindMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuOutputBindMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuOutputBindMaterialPagedQuery);

            using var conn = GetMESDbConnection();
            var manuOutputBindMaterialEntitiesTask = conn.QueryAsync<ManuOutputBindMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuOutputBindMaterialEntities = await manuOutputBindMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuOutputBindMaterialEntity>(manuOutputBindMaterialEntities, manuOutputBindMaterialPagedQuery.PageIndex, manuOutputBindMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuOutputBindMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuOutputBindMaterialEntity>> GetManuOutputBindMaterialEntitiesAsync(ManuOutputBindMaterialQuery manuOutputBindMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuOutputBindMaterialEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuOutputBindMaterialEntities = await conn.QueryAsync<ManuOutputBindMaterialEntity>(template.RawSql, manuOutputBindMaterialQuery);
            return manuOutputBindMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuOutputBindMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuOutputBindMaterialEntity manuOutputBindMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuOutputBindMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuOutputBindMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuOutputBindMaterialEntity> manuOutputBindMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuOutputBindMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuOutputBindMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuOutputBindMaterialEntity manuOutputBindMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuOutputBindMaterialEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuOutputBindMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuOutputBindMaterialEntity> manuOutputBindMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuOutputBindMaterialEntitys);
        }
        #endregion

    }

    public partial class ManuOutputBindMaterialRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_output_bind_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_output_bind_material` /**where**/ ";
        const string GetManuOutputBindMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_output_bind_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_output_bind_material`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `BindCode`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @EquipmentId, @SFC, @BindCode, @Type, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_output_bind_material`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `BindCode`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @EquipmentId, @SFC, @BindCode, @Type, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_output_bind_material` SET   SiteId = @SiteId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, BindCode = @BindCode, Type = @Type, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_output_bind_material` SET   SiteId = @SiteId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, BindCode = @BindCode, Type = @Type, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_output_bind_material` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_output_bind_material` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `BindCode`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_output_bind_material`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `BindCode`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_output_bind_material`  WHERE Id IN @Ids ";
        #endregion
    }
}
