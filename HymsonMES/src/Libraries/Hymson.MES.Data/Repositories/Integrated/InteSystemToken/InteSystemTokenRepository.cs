/*
 *creator: Karl
 *
 *describe: 系统Token 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 系统Token仓储
    /// </summary>
    public partial class InteSystemTokenRepository :BaseRepository, IInteSystemTokenRepository
    {

        public InteSystemTokenRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<InteSystemTokenEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteSystemTokenEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteSystemTokenEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteSystemTokenEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteSystemTokenPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteSystemTokenEntity>> GetPagedInfoAsync(InteSystemTokenPagedQuery inteSystemTokenPagedQuery)
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
           
            var offSet = (inteSystemTokenPagedQuery.PageIndex - 1) * inteSystemTokenPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteSystemTokenPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteSystemTokenPagedQuery);

            using var conn = GetMESDbConnection();
            var inteSystemTokenEntitiesTask = conn.QueryAsync<InteSystemTokenEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteSystemTokenEntities = await inteSystemTokenEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteSystemTokenEntity>(inteSystemTokenEntities, inteSystemTokenPagedQuery.PageIndex, inteSystemTokenPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteSystemTokenQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteSystemTokenEntity>> GetInteSystemTokenEntitiesAsync(InteSystemTokenQuery inteSystemTokenQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteSystemTokenEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteSystemTokenEntities = await conn.QueryAsync<InteSystemTokenEntity>(template.RawSql, inteSystemTokenQuery);
            return inteSystemTokenEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteSystemTokenEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteSystemTokenEntity inteSystemTokenEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteSystemTokenEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteSystemTokenEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteSystemTokenEntity> inteSystemTokenEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteSystemTokenEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteSystemTokenEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteSystemTokenEntity inteSystemTokenEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteSystemTokenEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteSystemTokenEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteSystemTokenEntity> inteSystemTokenEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteSystemTokenEntitys);
        }
        #endregion

    }

    public partial class InteSystemTokenRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_system_token` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_system_token` /**where**/ ";
        const string GetInteSystemTokenEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_system_token` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_system_token`(  `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @SystemCode, @SystemName, @Token, @ExpirationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `inte_system_token`(  `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @SystemCode, @SystemName, @Token, @ExpirationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `inte_system_token` SET   SystemCode = @SystemCode, SystemName = @SystemName, Token = @Token, ExpirationTime = @ExpirationTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_system_token` SET   SystemCode = @SystemCode, SystemName = @SystemName, Token = @Token, ExpirationTime = @ExpirationTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_system_token` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_system_token` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_system_token`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_system_token`  WHERE Id IN @Ids ";
        #endregion
    }
}
