using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线表仓储
    /// </summary>
    public partial class ProcProcessRouteRepository : BaseRepository, IProcProcessRouteRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcProcessRouteRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 删除时查询启用和保留状态的不能删除
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> IsIsExistsEnabledAsync(ProcProcessRouteQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteEntity>(IsIsExistsEnabledSql, new { StatusArr = query.StatusArr, Ids = query.Ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcessRoutePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteEntity>> GetPagedInfoAsync(ProcProcessRoutePagedQuery procProcessRoutePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            if (string.IsNullOrEmpty(procProcessRoutePagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(procProcessRoutePagedQuery.Sorting);
            }
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(procProcessRoutePagedQuery.Code))
            {
                procProcessRoutePagedQuery.Code = $"%{procProcessRoutePagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(procProcessRoutePagedQuery.Name))
            {
                procProcessRoutePagedQuery.Name = $"%{procProcessRoutePagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(procProcessRoutePagedQuery.Version))
            {
                procProcessRoutePagedQuery.Version = $"%{procProcessRoutePagedQuery.Version}%";
                sqlBuilder.Where("Version like @Version");
            }
            if (procProcessRoutePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (procProcessRoutePagedQuery.StatusArr != null && procProcessRoutePagedQuery.StatusArr.Length > 0)
            {
                sqlBuilder.Where("Status in @StatusArr");
            }
            if (procProcessRoutePagedQuery.Type.HasValue)
            {
                sqlBuilder.Where("Type = @Type");
            }

            var offSet = (procProcessRoutePagedQuery.PageIndex - 1) * procProcessRoutePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procProcessRoutePagedQuery.PageSize });
            sqlBuilder.AddParameters(procProcessRoutePagedQuery);

            using var conn = GetMESDbConnection();
            var procProcessRouteEntitiesTask = conn.QueryAsync<ProcProcessRouteEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcessRouteEntities = await procProcessRouteEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcessRouteEntity>(procProcessRouteEntities, procProcessRoutePagedQuery.PageIndex, procProcessRoutePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcessRouteEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcessRouteQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteEntity>> GetProcProcessRouteEntitiesAsync(ProcProcessRouteQuery procProcessRouteQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcessRouteEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procProcessRouteEntities = await conn.QueryAsync<ProcProcessRouteEntity>(template.RawSql, procProcessRouteQuery);
            return procProcessRouteEntities;
        }

        /// <summary>
        /// 判断工艺路线是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(ProcProcessRouteQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(ExistsSql);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("Code = @Code");
            sqlBuilder.Where("Version = @Version");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.Id > 0)
            {
                sqlBuilder.Where("Id!=@Id");
            }
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var procProcessRoutes = await conn.QueryAsync<ProcProcessRouteEntity>(templateData.RawSql, templateData.Parameters);
            return procProcessRoutes != null && procProcessRoutes.Any();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ResetCurrentVersionAsync(ResetCurrentVersionCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ResetCurrentVersionSql, command);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcessRouteEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcessRouteEntity procProcessRouteEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procProcessRouteEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcProcessRouteEntity> procProcessRouteEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procProcessRouteEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcessRouteEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcessRouteEntity procProcessRouteEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcessRouteEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcProcessRouteEntity> procProcessRouteEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new { UpdatedBy = command.UserId, UpdatedOn = command.DeleteOn, Ids = command.Ids });
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 根据编码获取工艺路线信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> GetByCodeAsync(ProcProcessRoutesByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据编码获取工艺路线信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteEntity>> GetByCodesAsync(ProcProcessRoutesByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcessRouteEntity>(GetByCodesSql, param);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcProcessRouteRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_process_route` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_process_route` /**where**/ ";
        const string GetProcProcessRouteEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_process_route` /**where**/  ";
        const string ExistsSql = "SELECT Id FROM proc_process_route  /**where**/ LIMIT 1";

        const string InsertSql = "INSERT INTO `proc_process_route`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Type`, `Version`, `IsCurrentVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Type, @Version, @IsCurrentVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string ResetCurrentVersionSql = "UPDATE proc_process_route SET IsCurrentVersion = 0, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND SiteId = @SiteId AND IsCurrentVersion = 1 AND Code=@Code ";
        const string UpdateSql = "UPDATE `proc_process_route` SET Name=@Name , Type = @Type, IsCurrentVersion = @IsCurrentVersion, Remark = @Remark,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_process_route` SET IsDeleted = Id,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id in @Ids";
        const string GetByIdSql = @"SELECT * FROM `proc_process_route`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_process_route`  WHERE Id IN @ids ";
        const string IsIsExistsEnabledSql = "select Id  from proc_process_route where IsDeleted=0 and Status in @StatusArr and Id  in @Ids  limit 1";

        const string UpdateStatusSql = "UPDATE `proc_process_route` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        const string GetByCodeSql = @"SELECT * FROM `proc_process_route` WHERE SiteId = @SiteId AND IsDeleted = 0 AND Code = @Code; ";
        const string GetByCodesSql = @"SELECT * FROM `proc_process_route` WHERE Code IN @Codes AND SiteId = @SiteId  AND IsDeleted=0 ";
    }
}
