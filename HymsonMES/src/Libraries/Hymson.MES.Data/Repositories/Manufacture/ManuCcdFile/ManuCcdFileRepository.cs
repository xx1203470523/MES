/*
 *creator: Karl
 *
 *describe: CCD文件 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-05-17 11:09:19
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
    /// CCD文件仓储
    /// </summary>
    public partial class ManuCcdFileRepository : BaseRepository, IManuCcdFileRepository
    {

        public ManuCcdFileRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuCcdFileEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuCcdFileEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuCcdFileEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuCcdFileEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuCcdFilePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuCcdFileEntity>> GetPagedInfoAsync(ManuCcdFilePagedQuery manuCcdFilePagedQuery)
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

            var offSet = (manuCcdFilePagedQuery.PageIndex - 1) * manuCcdFilePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuCcdFilePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuCcdFilePagedQuery);

            using var conn = GetMESDbConnection();
            var manuCcdFileEntitiesTask = conn.QueryAsync<ManuCcdFileEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuCcdFileEntities = await manuCcdFileEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuCcdFileEntity>(manuCcdFileEntities, manuCcdFilePagedQuery.PageIndex, manuCcdFilePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuCcdFileQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuCcdFileEntity>> GetManuCcdFileEntitiesAsync(ManuCcdFileQuery manuCcdFileQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuCcdFileEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where(" SiteId=@SiteId");
            sqlBuilder.Where(" IsDeleted=0");
            sqlBuilder.Where(" SFC IN @Sfcs");
            using var conn = GetMESDbConnection();
            var manuCcdFileEntities = await conn.QueryAsync<ManuCcdFileEntity>(template.RawSql, manuCcdFileQuery);
            return manuCcdFileEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuCcdFileEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuCcdFileEntity manuCcdFileEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuCcdFileEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuCcdFileEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuCcdFileEntity> manuCcdFileEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuCcdFileEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuCcdFileEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuCcdFileEntity manuCcdFileEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuCcdFileEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuCcdFileEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuCcdFileEntity> manuCcdFileEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuCcdFileEntitys);
        }
        #endregion

    }

    public partial class ManuCcdFileRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_ccd_file` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_ccd_file` /**where**/ ";
        const string GetManuCcdFileEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_ccd_file` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_ccd_file`(  `Id`, `SiteId`, `SFC`, `Passed`, `URL`, `Timestamp`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Passed, @URL, @Timestamp, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_ccd_file`(  `Id`, `SiteId`, `SFC`, `Passed`, `URL`, `Timestamp`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Passed, @URL, @Timestamp, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_ccd_file` SET   SiteId = @SiteId, SFC = @SFC, Passed = @Passed, URL = @URL, Timestamp = @Timestamp, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_ccd_file` SET   SiteId = @SiteId, SFC = @SFC, Passed = @Passed, URL = @URL, Timestamp = @Timestamp, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_ccd_file` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_ccd_file` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SFC`, `Passed`, `URL`, `Timestamp`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_ccd_file`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SFC`, `Passed`, `URL`, `Timestamp`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_ccd_file`  WHERE Id IN @Ids ";
        #endregion
    }
}
