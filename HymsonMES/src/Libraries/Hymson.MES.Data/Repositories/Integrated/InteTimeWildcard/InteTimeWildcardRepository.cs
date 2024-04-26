/*
 *creator: Karl
 *
 *describe: 时间通配（转换） 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-10-13 06:33:21
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 时间通配（转换）仓储
    /// </summary>
    public partial class InteTimeWildcardRepository :BaseRepository, IInteTimeWildcardRepository
    {
        private readonly IMemoryCache _memoryCache;

        public InteTimeWildcardRepository(IOptions<ConnectionOptions> connectionOptions,IMemoryCache memoryCache): base(connectionOptions)
        {
            _memoryCache = memoryCache;
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
        public async Task<InteTimeWildcardEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteTimeWildcardEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteTimeWildcardEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteTimeWildcardEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteTimeWildcardPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteTimeWildcardEntity>> GetPagedInfoAsync(InteTimeWildcardPagedQuery inteTimeWildcardPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (inteTimeWildcardPagedQuery.PageIndex - 1) * inteTimeWildcardPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteTimeWildcardPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteTimeWildcardPagedQuery);

            using var conn = GetMESDbConnection();
            var inteTimeWildcardEntitiesTask = conn.QueryAsync<InteTimeWildcardEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteTimeWildcardEntities = await inteTimeWildcardEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteTimeWildcardEntity>(inteTimeWildcardEntities, inteTimeWildcardPagedQuery.PageIndex, inteTimeWildcardPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteTimeWildcardQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteTimeWildcardEntity>> GetInteTimeWildcardEntitiesAsync(InteTimeWildcardQuery inteTimeWildcardQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteTimeWildcardEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteTimeWildcardEntities = await conn.QueryAsync<InteTimeWildcardEntity>(template.RawSql, inteTimeWildcardQuery);
            return inteTimeWildcardEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteTimeWildcardEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteTimeWildcardEntity inteTimeWildcardEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteTimeWildcardEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteTimeWildcardEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteTimeWildcardEntity> inteTimeWildcardEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteTimeWildcardEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteTimeWildcardEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteTimeWildcardEntity inteTimeWildcardEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteTimeWildcardEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteTimeWildcardEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteTimeWildcardEntity> inteTimeWildcardEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteTimeWildcardEntitys);
        }

        /// <summary>
        /// 根据编码与类型获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteTimeWildcardEntity> GetByCodeAndTypeAsync(InteTimeWildcardCodeAndTypeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteTimeWildcardEntity>(GetByCodeAndTypeSql, query);
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteTimeWildcardEntity>> GetAllAsync(long siteId)
        {
            //var cachedKey = "inte_time_wildcard_All";
            var key = $"{CachedTables.INTE_TIME_WILDCARD}_All";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (ICacheEntry cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<InteTimeWildcardEntity>(GetAllSql, new { SiteId = siteId });
            });
        }
        #endregion

    }

    public partial class InteTimeWildcardRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_time_wildcard` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_time_wildcard` /**where**/ ";
        const string GetInteTimeWildcardEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_time_wildcard` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_time_wildcard`(  `Id`, `Code`, `Type`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Type, @Value, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `inte_time_wildcard`(  `Id`, `Code`, `Type`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Type, @Value, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `inte_time_wildcard` SET   Code = @Code, Type = @Type, Value = @Value, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_time_wildcard` SET   Code = @Code, Type = @Type, Value = @Value, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_time_wildcard` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_time_wildcard` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Type`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_time_wildcard`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Type`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `inte_time_wildcard`  WHERE Id IN @Ids ";

        const string GetByCodeAndTypeSql = @"SELECT * FROM `inte_time_wildcard`  WHERE IsDeleted = 0 AND `SiteId` = @SiteId AND `Code`=@Code AND `Type` = @Type ";
        const string GetAllSql = @"SELECT * FROM `inte_time_wildcard`  WHERE IsDeleted = 0 AND `SiteId` = @SiteId ";
        #endregion
    }
}
