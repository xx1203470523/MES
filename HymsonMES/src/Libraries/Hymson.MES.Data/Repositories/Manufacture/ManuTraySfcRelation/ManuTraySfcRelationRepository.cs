/*
 *creator: Karl
 *
 *describe: 托盘条码关系 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;
using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘条码关系仓储
    /// </summary>
    public partial class ManuTraySfcRelationRepository : BaseRepository, IManuTraySfcRelationRepository
    {

        public ManuTraySfcRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// 批量删除（硬删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteTruesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTruesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuTraySfcRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuTraySfcRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTraySfcRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuTraySfcRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据trayLoadId 获取转载记录
        /// </summary>
        /// <param name="trayLoadId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTraySfcRelationEntity>> GetByTrayLoadIdAsync(long trayLoadId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuTraySfcRelationEntity>(GetByTrayLoadIdSql, new { TrayLoadId = trayLoadId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuTraySfcRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuTraySfcRelationEntity>> GetPagedInfoAsync(ManuTraySfcRelationPagedQuery manuTraySfcRelationPagedQuery)
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

            var offSet = (manuTraySfcRelationPagedQuery.PageIndex - 1) * manuTraySfcRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuTraySfcRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuTraySfcRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var manuTraySfcRelationEntitiesTask = conn.QueryAsync<ManuTraySfcRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuTraySfcRelationEntities = await manuTraySfcRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuTraySfcRelationEntity>(manuTraySfcRelationEntities, manuTraySfcRelationPagedQuery.PageIndex, manuTraySfcRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuTraySfcRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTraySfcRelationEntity>> GetManuTraySfcRelationEntitiesAsync(ManuTraySfcRelationQuery manuTraySfcRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuTraySfcRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuTraySfcRelationEntities = await conn.QueryAsync<ManuTraySfcRelationEntity>(template.RawSql, manuTraySfcRelationQuery);
            return manuTraySfcRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTraySfcRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuTraySfcRelationEntity manuTraySfcRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuTraySfcRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuTraySfcRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuTraySfcRelationEntity> manuTraySfcRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuTraySfcRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuTraySfcRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuTraySfcRelationEntity manuTraySfcRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuTraySfcRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuTraySfcRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuTraySfcRelationEntity> manuTraySfcRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuTraySfcRelationEntitys);
        }

        /// <summary>
        /// 根据托盘条码查询装载信息
        /// </summary>
        /// <param name="manuTraySfcRelationQuery"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<ManuTraySfcRelationEntity>> GetManuTraySfcRelationByTrayCodeAsync(ManuTraySfcRelationByTrayCodeQuery manuTraySfcRelationByTrayCode)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            sqlBuilder.Select("mtsr.*");
            sqlBuilder.InnerJoin(" manu_tray_load mtl ON  mtsr.TrayLoadId=mtl.id");
            sqlBuilder.Where(" mtl.TrayCode=@TrayCode");
            sqlBuilder.Where(" mtsr.SiteId=@SiteId");
            sqlBuilder.Where(" mtsr.IsDeleted=0");
            using var conn = GetMESDbConnection();
            var manuTraySfcRelationEntities = await conn.QueryAsync<ManuTraySfcRelationEntity>(template.RawSql, manuTraySfcRelationByTrayCode);
            return manuTraySfcRelationEntities;
        }

        #endregion

    }

    public partial class ManuTraySfcRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_tray_sfc_relation` mtsr /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_tray_sfc_relation` /**where**/ ";
        const string GetManuTraySfcRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_tray_sfc_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_tray_sfc_relation`(  `Id`, `SiteId`, `TrayLoadId`, `Seq`, `SFC`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @TrayLoadId, @Seq, @SFC, @LoadQty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_tray_sfc_relation`(  `Id`, `SiteId`, `TrayLoadId`, `Seq`, `SFC`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @TrayLoadId, @Seq, @SFC, @LoadQty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_tray_sfc_relation` SET   SiteId = @SiteId, TrayLoadId = @TrayLoadId, Seq = @Seq, SFC = @SFC, LoadQty = @LoadQty, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_tray_sfc_relation` SET   SiteId = @SiteId, TrayLoadId = @TrayLoadId, Seq = @Seq, SFC = @SFC, LoadQty = @LoadQty, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_tray_sfc_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_tray_sfc_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `TrayLoadId`, `Seq`, `SFC`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_sfc_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `TrayLoadId`, `Seq`, `SFC`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_sfc_relation`  WHERE Id IN @Ids ";

        const string GetByTrayLoadIdSql = @"SELECT 
                               `Id`, `SiteId`, `TrayLoadId`, `Seq`, `SFC`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_sfc_relation`  WHERE TrayLoadId = @TrayLoadId ";

        const string DeleteTruesSql = @"Delete FROM `manu_tray_sfc_relation`  WHERE Id IN @Ids ";
        #endregion
    }
}
