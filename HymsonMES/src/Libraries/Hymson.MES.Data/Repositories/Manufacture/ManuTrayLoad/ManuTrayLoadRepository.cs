/*
 *creator: Karl
 *
 *describe: 托盘装载信息表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 托盘装载信息表仓储
    /// </summary>
    public partial class ManuTrayLoadRepository : BaseRepository, IManuTrayLoadRepository
    {

        public ManuTrayLoadRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuTrayLoadEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuTrayLoadEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTrayLoadEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuTrayLoadEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 通过trayCode查询装载信息
        /// </summary>
        /// <param name="trayCode"></param>
        /// <returns></returns>
        public async Task<ManuTrayLoadEntity> GetByTrayCodeAsync(string trayCode)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuTrayLoadEntity>(GetByTrayCodeSql, new { TrayCode = trayCode });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuTrayLoadPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuTrayLoadEntity>> GetPagedInfoAsync(ManuTrayLoadPagedQuery manuTrayLoadPagedQuery)
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

            var offSet = (manuTrayLoadPagedQuery.PageIndex - 1) * manuTrayLoadPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuTrayLoadPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuTrayLoadPagedQuery);

            using var conn = GetMESDbConnection();
            var manuTrayLoadEntitiesTask = conn.QueryAsync<ManuTrayLoadEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuTrayLoadEntities = await manuTrayLoadEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuTrayLoadEntity>(manuTrayLoadEntities, manuTrayLoadPagedQuery.PageIndex, manuTrayLoadPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuTrayLoadQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTrayLoadEntity>> GetManuTrayLoadEntitiesAsync(ManuTrayLoadQuery manuTrayLoadQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuTrayLoadEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuTrayLoadEntities = await conn.QueryAsync<ManuTrayLoadEntity>(template.RawSql, manuTrayLoadQuery);
            return manuTrayLoadEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTrayLoadEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuTrayLoadEntity manuTrayLoadEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuTrayLoadEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuTrayLoadEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuTrayLoadEntity> manuTrayLoadEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuTrayLoadEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuTrayLoadEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuTrayLoadEntity manuTrayLoadEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuTrayLoadEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuTrayLoadEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuTrayLoadEntity> manuTrayLoadEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuTrayLoadEntitys);
        }
        #endregion

    }

    public partial class ManuTrayLoadRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_tray_load` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_tray_load` /**where**/ ";
        const string GetManuTrayLoadEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_tray_load` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_tray_load`(  `Id`, `SiteId`, `TrayCode`, `TrayId`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @TrayCode, @TrayId, @LoadQty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_tray_load`(  `Id`, `SiteId`, `TrayCode`, `TrayId`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @TrayCode, @TrayId, @LoadQty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_tray_load` SET   SiteId = @SiteId, TrayCode = @TrayCode, TrayId = @TrayId, LoadQty = @LoadQty, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_tray_load` SET   SiteId = @SiteId, TrayCode = @TrayCode, TrayId = @TrayId, LoadQty = @LoadQty, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_tray_load` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_tray_load` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `TrayCode`, `TrayId`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_load`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `TrayCode`, `TrayId`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_load`  WHERE Id IN @Ids ";

        const string GetByTrayCodeSql = @"SELECT 
                               `Id`, `SiteId`, `TrayCode`, `TrayId`, `LoadQty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_load`  WHERE TrayCode = @TrayCode ";
        #endregion
    }
}
