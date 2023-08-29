using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序表仓储
    /// </summary>
    public partial class ProcProcedureRepository : IProcProcedureRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcProcedureRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache)
        {
            _connectionOptions = connectionOptions.Value;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcedurePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureView>> GetPagedInfoAsync(ProcProcedurePagedQuery procProcedurePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            if (string.IsNullOrEmpty(procProcedurePagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("a.CreatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(procProcedurePagedQuery.Sorting);
            }

            sqlBuilder.Where("a.SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(procProcedurePagedQuery.Code))
            {
                procProcedurePagedQuery.Code = $"%{procProcedurePagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(procProcedurePagedQuery.Name))
            {
                procProcedurePagedQuery.Name = $"%{procProcedurePagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }
            if (procProcedurePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (procProcedurePagedQuery.StatusArr != null && procProcedurePagedQuery.StatusArr.Length > 0)
            {
                sqlBuilder.Where("Status in @StatusArr");
            }
            if (procProcedurePagedQuery.TypeArr != null && procProcedurePagedQuery.TypeArr.Length > 0)
            {
                if (procProcedurePagedQuery.Type.HasValue)
                {
                    if (procProcedurePagedQuery.TypeArr.Contains(procProcedurePagedQuery.Type.Value))
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
                if (procProcedurePagedQuery.Type.HasValue)
                {
                    sqlBuilder.Where("Type=@Type");
                }
            }
            if (!string.IsNullOrWhiteSpace(procProcedurePagedQuery.ResTypeName))
            {
                procProcedurePagedQuery.Name = $"%{procProcedurePagedQuery.ResTypeName}%";
                sqlBuilder.Where("ResTypeName like @ResTypeName");
            }

            var offSet = (procProcedurePagedQuery.PageIndex - 1) * procProcedurePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procProcedurePagedQuery.PageSize });
            sqlBuilder.AddParameters(procProcedurePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedureEntitiesTask = conn.QueryAsync<ProcProcedureView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedureEntities = await procProcedureEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedureView>(procProcedureEntities, procProcedurePagedQuery.PageIndex, procProcedurePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        ///分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureEntity>> GetPagedInfoByProcessRouteIdAsync(ProcProcedurePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedDataCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            sqlBuilder.Where("a.SiteId = @SiteId");
            sqlBuilder.Where("b.ProcessRouteId = @ProcessRouteId");
            sqlBuilder.Select("a.*");
            sqlBuilder.LeftJoin("proc_process_route_detail_node b on a.id=b.ProcedureId and b.IsDeleted =0 ");

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("a.Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = $"%{query.Name}%";
                sqlBuilder.Where("a.Name like @Name");
            }
            if (query.Type.HasValue)
            {
                sqlBuilder.Where("a.Type=@Type");
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
            return new PagedInfo<ProcProcedureEntity>(procProcedureEntities, query.PageIndex, query.PageSize, totalCount);
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
        public async Task<IEnumerable<ProcProcedureEntity>> GetByIdsAsync(IEnumerable<long>  ids)
        {
            if (!ids.Any())
            {
                return new List<ProcProcedureEntity>();
            }

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
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procProcedureQuery.Code))
            {
                sqlBuilder.Where(" Code= @Code ");
            }
            if (procProcedureQuery.Codes != null && procProcedureQuery.Codes.Any())
            {
                sqlBuilder.Where(" Code in @Codes ");
            }
            sqlBuilder.AddParameters(procProcedureQuery);

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

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }
    }

    public partial class ProcProcedureRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.ResType ,b.ResTypeName  from proc_procedure a left join proc_resource_type b on a.ResourceTypeId=b.Id and b.IsDeleted=0 /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select COUNT(*) from proc_procedure a left join proc_resource_type b on a.ResourceTypeId=b.Id and b.IsDeleted=0 /**where**/ ";

        const string GetPagedDataSqlTemplate = @"SELECT /**select**/ FROM `proc_procedure`  a /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedDataCountSqlTemplate = "SELECT COUNT(1) FROM `proc_procedure`  a  /**innerjoin**/ /**leftjoin**/  /**where**/ ";

        const string GetProcProcedureEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_procedure` /**where**/  ";
        const string ExistsSql = "SELECT Id FROM proc_procedure WHERE `IsDeleted`= 0 AND Code=@Code and SiteId=@SiteId LIMIT 1 ";

        const string InsertSql = "INSERT INTO `proc_procedure`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Type`, `PackingLevel`, `ResourceTypeId`, `Cycle`, `IsRepairReturn`, `Version`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Type, @PackingLevel, @ResourceTypeId, @Cycle, @IsRepairReturn, @Version, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_procedure` SET  Name=@Name,Status = @Status, Type = @Type, PackingLevel = @PackingLevel, ResourceTypeId = @ResourceTypeId, Cycle = @Cycle, IsRepairReturn = @IsRepairReturn, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_procedure` SET IsDeleted =Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id in @Ids";
        const string GetByIdSql = @"SELECT * FROM `proc_procedure`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_procedure`  WHERE Id IN @ids and IsDeleted=0  ";

        const string GetProcProdureByResourceIdSql = "SELECT P.* FROM proc_procedure P INNER JOIN  proc_resource R ON R.ResTypeId = P.ResourceTypeId  WHERE R.IsDeleted = 0 AND P.IsDeleted = 0 AND R.SiteId = @SiteId AND P.SiteId = @SiteId AND R.Id = @ResourceId";

        const string UpdateStatusSql = "UPDATE `proc_procedure` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

    }
}
