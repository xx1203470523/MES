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
        /// 根据系统编码获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<InteSystemTokenEntity> GetByCodeAsync(InteSystemTokenQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteSystemTokenEntity>(GetByCodeSql, param);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteSystemTokenEntity>> GetPagedInfoAsync(InteSystemTokenPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy(" UpdatedOn DESC ");
            if (!string.IsNullOrWhiteSpace(pagedQuery.SystemCode))
            {
                pagedQuery.SystemCode = $"%{pagedQuery.SystemCode}%";
                sqlBuilder.Where("SystemCode LIKE @SystemCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.SystemName))
            {
                pagedQuery.SystemName = $"%{pagedQuery.SystemName}%";
                sqlBuilder.Where("SystemName LIKE @SystemName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var inteSystemTokenEntitiesTask = conn.QueryAsync<InteSystemTokenEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteSystemTokenEntities = await inteSystemTokenEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteSystemTokenEntity>(inteSystemTokenEntities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
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
            return await conn.ExecuteAsync(InsertSql, inteSystemTokenEntitys);
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
            return await conn.ExecuteAsync(UpdateSql, inteSystemTokenEntitys);
        }

        /// <summary>
        /// 更新token信息
        /// </summary>
        /// <param name="inteSystemTokenEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateTokenAsync(InteSystemTokenEntity inteSystemTokenEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateTokenSql, inteSystemTokenEntity);
        }
        #endregion
    }

    public partial class InteSystemTokenRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_system_token` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_system_token` /**where**/ ";
        const string GetInteSystemTokenEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_system_token` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_system_token`(  `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @SystemCode, @SystemName, @Token, @ExpirationTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateSql = "UPDATE `inte_system_token` SET  SystemName = @SystemName, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateTokenSql = "UPDATE `inte_system_token` SET Token = @Token, ExpirationTime = @ExpirationTime, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_system_token` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_system_token` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_system_token`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SystemCode`, `SystemName`, `Token`, `ExpirationTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_system_token`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT Id,SystemCode,SystemName,Token,ExpirationTime,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted,SiteId
                                                           FROM inte_system_token  WHERE SystemCode=@SystemCode  AND SiteId=@SiteId AND IsDeleted=0 ";
        #endregion
    }
}
