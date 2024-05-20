using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Security.Policy;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表仓储
    /// </summary>
    public partial class ProcProcedureRepository : IProcProcedureRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        private readonly IMemoryCache _memoryCache;

        public ProcProcedureRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache)
        {
            _connectionOptions = connectionOptions.Value;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 根据资源类型Id查询工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity> GetByResTypeId(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcedureEntity>(GetByResTypeIdSql, new { Id = id });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureView>> GetPagedInfoAsync(ProcProcedurePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            if (string.IsNullOrEmpty(query.Sorting))
            {
                sqlBuilder.OrderBy("a.CreatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(query.Sorting);
            }

            sqlBuilder.Where("a.SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = $"%{query.Name}%";
                sqlBuilder.Where("Name like @Name");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (query.StatusArr != null && query.StatusArr.Length > 0)
            {
                sqlBuilder.Where("Status in @StatusArr");
            }
            if (query.TypeArr != null && query.TypeArr.Length > 0)
            {
                if (query.Type.HasValue)
                {
                    if (query.TypeArr.Contains(query.Type.Value))
                    {
                        sqlBuilder.Where("Type=@Type");
                    }
                    else
                    {
                        sqlBuilder.Where("Type=-1");
                    }
                }
                else
                {
                    sqlBuilder.Where("Type in @TypeArr");
                }
            }
            else
            {
                if (query.Type.HasValue)
                {
                    sqlBuilder.Where("Type=@Type");
                }
            }
            if (!string.IsNullOrWhiteSpace(query.ResTypeName))
            {
                query.Name = $"%{query.ResTypeName}%";
                sqlBuilder.Where("ResTypeName like @ResTypeName");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedureEntitiesTask = conn.QueryAsync<ProcProcedureView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedureEntities = await procProcedureEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedureView>(procProcedureEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity> GetByCodeAsync(string code, long siteId)
        {
            var key = $"proc_procedure&{siteId}&{code}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                return await conn.QueryFirstOrDefaultAsync<ProcProcedureEntity>(GetByCodeSql, new { Code = code, SiteId = siteId });
            });
        }

        /// <summary>
        /// 根据Codes批量获取数据
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>> GetByCodesAsync(string[] codes, long siteId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcedureEntity>(GetByCodesSql, new { Codes = codes, SiteId = siteId });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity> GetByIdAsync(long id)
        {
            var key = $"proc_procedure&{id}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                return await conn.QueryFirstOrDefaultAsync<ProcProcedureEntity>(GetByIdSql, new { Id = id });
            });
        }

        /// <summary>
        /// 根据资源ID获取工序 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedureEntity> GetProcProdureByResourceIdAsync(ProcProdureByResourceIdQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcedureEntity>(GetProcProdureByResourceIdSql, param);
        }


        /// <summary>
        /// 判断工序是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(ProcProcedureQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entity = await conn.QueryAsync<ProcProcedureEntity>(ExistsSql, new { Code = query.Code, SiteId = query.SiteId });
            return entity != null && entity.Any();
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcedureEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcedureQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureEntity>> GetProcProcedureEntitiesAsync(ProcProcedureQuery procProcedureQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcedureEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedureEntities = await conn.QueryAsync<ProcProcedureEntity>(template.RawSql, procProcedureQuery);
            return procProcedureEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcedureEntity procProcedureEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcedureEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedureEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcedureEntity procProcedureEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcedureEntity);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { UpdatedBy = command.UserId, UpdatedOn = command.DeleteOn, Ids = command.Ids });
        }
    }

    public partial class ProcProcedureRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.ResType ,b.ResTypeName  from proc_procedure a left join proc_resource_type b on a.ResourceTypeId=b.Id and b.IsDeleted=0 /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select COUNT(*) from proc_procedure a left join proc_resource_type b on a.ResourceTypeId=b.Id and b.IsDeleted=0 /**where**/ ";
        const string GetProcProcedureEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_procedure` /**where**/  ";
        const string ExistsSql = "SELECT Id FROM proc_procedure WHERE `IsDeleted`= 0 AND Code=@Code and SiteId=@SiteId LIMIT 1 ";

        const string InsertSql = "INSERT INTO `proc_procedure`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Type`, `PackingLevel`, `ResourceTypeId`, `Cycle`, `IsRepairReturn`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Type, @PackingLevel, @ResourceTypeId, @Cycle, @IsRepairReturn, @Version, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_procedure` SET  Name=@Name,Status = @Status, Type = @Type, PackingLevel = @PackingLevel, ResourceTypeId = @ResourceTypeId, Cycle = @Cycle, IsRepairReturn = @IsRepairReturn, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_procedure` SET IsDeleted =Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id in @Ids";
        const string GetByIdSql = @"SELECT * FROM `proc_procedure`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_procedure`  WHERE Id IN @ids and IsDeleted=0  ";
        const string GetByCodeSql = @"SELECT * FROM `proc_procedure`  WHERE Code = @Code and SiteId=@SiteId LIMIT 1";
        const string GetByCodesSql = @"SELECT * FROM `proc_procedure`  WHERE Code in @Codes and SiteId=@SiteId ";
        const string GetByResTypeIdSql = @"SELECT * FROM `proc_procedure`  WHERE ResourceTypeId = @Id ";

        const string GetProcProdureByResourceIdSql = "SELECT P.* FROM proc_procedure P INNER JOIN  proc_resource R ON R.ResTypeId = P.ResourceTypeId  WHERE R.IsDeleted = 0 AND P.IsDeleted = 0 AND R.SiteId = @SiteId AND P.SiteId = @SiteId AND R.Id = @ResourceId";
    }
}
