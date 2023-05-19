/*
 *creator: Karl
 *
 *describe: 产出上报 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-19 10:44:12
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
    /// 产出上报仓储
    /// </summary>
    public partial class ManuOutputRepository :BaseRepository, IManuOutputRepository
    {

        public ManuOutputRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuOutputEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuOutputEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuOutputEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuOutputEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuOutputPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuOutputEntity>> GetPagedInfoAsync(ManuOutputPagedQuery manuOutputPagedQuery)
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
           
            var offSet = (manuOutputPagedQuery.PageIndex - 1) * manuOutputPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuOutputPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuOutputPagedQuery);

            using var conn = GetMESDbConnection();
            var manuOutputEntitiesTask = conn.QueryAsync<ManuOutputEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuOutputEntities = await manuOutputEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuOutputEntity>(manuOutputEntities, manuOutputPagedQuery.PageIndex, manuOutputPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuOutputQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuOutputEntity>> GetManuOutputEntitiesAsync(ManuOutputQuery manuOutputQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuOutputEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuOutputEntities = await conn.QueryAsync<ManuOutputEntity>(template.RawSql, manuOutputQuery);
            return manuOutputEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuOutputEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuOutputEntity manuOutputEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuOutputEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuOutputEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuOutputEntity> manuOutputEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuOutputEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuOutputEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuOutputEntity manuOutputEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuOutputEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuOutputEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuOutputEntity> manuOutputEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuOutputEntitys);
        }
        #endregion

    }

    public partial class ManuOutputRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_output` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_output` /**where**/ ";
        const string GetManuOutputEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_output` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_output`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `OKQty`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @EquipmentId, @SFC, @OKQty, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_output`(  `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `OKQty`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @EquipmentId, @SFC, @OKQty, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_output` SET   SiteId = @SiteId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, OKQty = @OKQty, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_output` SET   SiteId = @SiteId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SFC = @SFC, OKQty = @OKQty, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_output` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_output` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `OKQty`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_output`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ResourceId`, `EquipmentId`, `SFC`, `OKQty`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_output`  WHERE Id IN @Ids ";
        #endregion
    }
}
