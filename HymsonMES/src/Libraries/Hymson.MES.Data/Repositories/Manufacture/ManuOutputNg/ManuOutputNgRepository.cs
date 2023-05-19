/*
 *creator: Karl
 *
 *describe: 产出上报NG 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-19 10:47:15
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
    /// 产出上报NG仓储
    /// </summary>
    public partial class ManuOutputNgRepository :BaseRepository, IManuOutputNgRepository
    {

        public ManuOutputNgRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuOutputNgEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuOutputNgEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuOutputNgEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuOutputNgEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuOutputNgPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuOutputNgEntity>> GetPagedInfoAsync(ManuOutputNgPagedQuery manuOutputNgPagedQuery)
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
           
            var offSet = (manuOutputNgPagedQuery.PageIndex - 1) * manuOutputNgPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuOutputNgPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuOutputNgPagedQuery);

            using var conn = GetMESDbConnection();
            var manuOutputNgEntitiesTask = conn.QueryAsync<ManuOutputNgEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuOutputNgEntities = await manuOutputNgEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuOutputNgEntity>(manuOutputNgEntities, manuOutputNgPagedQuery.PageIndex, manuOutputNgPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuOutputNgQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuOutputNgEntity>> GetManuOutputNgEntitiesAsync(ManuOutputNgQuery manuOutputNgQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuOutputNgEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuOutputNgEntities = await conn.QueryAsync<ManuOutputNgEntity>(template.RawSql, manuOutputNgQuery);
            return manuOutputNgEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuOutputNgEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuOutputNgEntity manuOutputNgEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuOutputNgEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuOutputNgEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuOutputNgEntity> manuOutputNgEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuOutputNgEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuOutputNgEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuOutputNgEntity manuOutputNgEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuOutputNgEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuOutputNgEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuOutputNgEntity> manuOutputNgEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuOutputNgEntitys);
        }
        #endregion

    }

    public partial class ManuOutputNgRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_output_ng` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_output_ng` /**where**/ ";
        const string GetManuOutputNgEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_output_ng` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_output_ng`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `NGId`, `NGCode`, `NGQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @EquipmentId, @SFC, @NGId, @NGCode, @NGQty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_output_ng`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `NGId`, `NGCode`, `NGQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @EquipmentId, @SFC, @NGId, @NGCode, @NGQty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_output_ng` SET   SiteId = @SiteId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, NGId = @NGId, NGCode = @NGCode, NGQty = @NGQty, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_output_ng` SET   SiteId = @SiteId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, NGId = @NGId, NGCode = @NGCode, NGQty = @NGQty, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_output_ng` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_output_ng` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `NGId`, `NGCode`, `NGQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_output_ng`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `NGId`, `NGCode`, `NGQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_output_ng`  WHERE Id IN @Ids ";
        #endregion
    }
}
