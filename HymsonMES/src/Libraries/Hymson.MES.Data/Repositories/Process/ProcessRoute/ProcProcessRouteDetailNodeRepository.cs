using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Query;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线工序节点明细表仓储
    /// </summary>
    public partial class ProcProcessRouteDetailNodeRepository : BaseRepository, IProcProcessRouteDetailNodeRepository
    {
        private readonly IMemoryCache _memoryCache;
        public ProcProcessRouteDetailNodeRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeView>> GetListAsync(ProcProcessRouteDetailNodeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetListSqlTemplateWithLeftJoin);
            sqlBuilder.Where("a.IsDeleted=0");
            if (query.ProcessRouteId > 0)
            {
                sqlBuilder.Where("ProcessRouteId=@ProcessRouteId");
            }
            if (query.ProcedureId > 0)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var procProcessRouteDetailNodeEntities = await conn.QueryAsync<ProcProcessRouteDetailNodeView>(template.RawSql, template.Parameters);
            return procProcessRouteDetailNodeEntities;
        }

        /// <summary>
        /// 查询List（已缓存）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetListAsync(EntityBySiteIdQuery query)
        {
            var key = $"{CachedTables.PROC_PROCESS_ROUTE_DETAIL_NODE}&SiteId={query.SiteId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                var sqlBuilder = new SqlBuilder();
                var template = sqlBuilder.AddTemplate(GetListSqlTemplate1);
                sqlBuilder.Where("IsDeleted = 0");
                sqlBuilder.Select("*");
                sqlBuilder.Where("SiteId = @SiteId");
                sqlBuilder.AddParameters(query);

                using var conn = GetMESDbConnection();
                var procProcessRouteDetailLinkEntities = await conn.QueryAsync<ProcProcessRouteDetailNodeEntity>(template.RawSql, template.Parameters);
                return procProcessRouteDetailLinkEntities;
            });
        }

        /// <summary>
        /// 获取路线信息
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetProcessRouteDetailNodesByProcessRouteIdAsync(long processRouteId)
        {
            var key = $"{CachedTables.PROC_PROCESS_ROUTE_DETAIL_NODE}&{processRouteId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<ProcProcessRouteDetailNodeEntity>(GetProcedureByProcessRouteIdSql, new { ProcessRouteId = processRouteId });
            });
        }

        /// <summary>
        /// 获取工艺路线工序信息
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureEntity>> GetProcedureListByProcessRouteIdAsync(ProcessRouteProcedureQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Select("PP.*");
            sqlBuilder.LeftJoin("proc_procedure PP ON proc_process_route_detail_node.ProcedureId = PP.Id");
            sqlBuilder.Where("proc_process_route_detail_node.IsDeleted = 0");
            sqlBuilder.Where("proc_process_route_detail_node.ProcessRouteId = @ProcessRouteId");
            sqlBuilder.Where("proc_process_route_detail_node.ProcedureId <> 999999999");
            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("PP.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("PP.Name LIKE @Name");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ProcProcedureEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedureEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteDetailNodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailNodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询节点明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteDetailNodeEntity> GetByProcessRouteIdAsync(ProcProcessRouteDetailNodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailNodeEntity>(GetByProcessRouteIdSql, query);
        }

        /// <summary>
        /// 查询节点明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetByProcedureIdsAsync(ProcProcessRouteDetailNodesQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcessRouteDetailNodeEntity>(GetByProcedureIdsSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteDetailNodeEntity> GetFirstProcedureByProcessRouteIdAsync(long processRouteId)
        {
            var key = $"{CachedTables.PROC_PROCESS_ROUTE_DETAIL_NODE}&FirstProcedure&{processRouteId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailNodeEntity>(GetFirstProcedureByProcessRouteIdSql, new { ProcessRouteId = processRouteId });
            });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>  
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcessRouteDetailNodeEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procProcessRouteDetailNodeEntitys);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteDetailNodeEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByProcessRouteIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByProcessRouteIdSql, new { ProcessRouteId = id });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcProcessRouteDetailNodeRepository
    {
        const string GetListSqlTemplateWithLeftJoin = @"select a.*,b.Code,b.Name,b.Type from proc_process_route_detail_node a left join proc_procedure b on a.ProcedureId=b.Id  /**where**/  ";
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_process_route_detail_node` /**innerjoin**/ /**leftjoin**/ /**where**/  /**orderby**/  LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_process_route_detail_node` /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetListSqlTemplate1 = @"SELECT /**select**/ FROM `proc_process_route_detail_node` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_process_route_detail_node`(  `Id`, `SiteId`, `ProcessRouteId`, `SerialNo`, `ProcedureId`, `CheckType`, `CheckRate`, `IsWorkReport`, `PackingLevel`, `IsFirstProcess`, `Status`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, ManualSortNumber ) VALUES (   @Id, @SiteId, @ProcessRouteId, @SerialNo, @ProcedureId, @CheckType, @CheckRate, @IsWorkReport, @PackingLevel, @IsFirstProcess, @Status, @Extra1, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @ManualSortNumber )  ";
        const string UpdateSql = "UPDATE `proc_process_route_detail_node` SET    ProcessRouteId = @ProcessRouteId, SerialNo = @SerialNo, ProcedureId = @ProcedureId, CheckType = @CheckType, CheckRate = @CheckRate, IsWorkReport = @IsWorkReport, PackingLevel = @PackingLevel, IsFirstProcess = @IsFirstProcess, Status = @Status, Extra1 = @Extra1, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted , ManualSortNumber=@ManualSortNumber WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_process_route_detail_node` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `proc_process_route_detail_node`  WHERE Id = @Id ";
        const string GetByProcessRouteIdSql = "SELECT * FROM proc_process_route_detail_node WHERE ProcessRouteId = @ProcessRouteId AND ProcedureId = @ProcedureId";
        const string GetByProcedureIdsSql = "SELECT * FROM proc_process_route_detail_node WHERE ProcessRouteId = @ProcessRouteId AND ProcedureId IN @ProcedureIds ORDER BY SerialNo; ";
        const string GetFirstProcedureByProcessRouteIdSql = @"SELECT * FROM `proc_process_route_detail_node`  WHERE ProcessRouteId = @ProcessRouteId and  IsFirstProcess=1";
        const string GetProcedureByProcessRouteIdSql = @"SELECT * FROM `proc_process_route_detail_node`  WHERE ProcessRouteId = @ProcessRouteId ORDER BY SerialNo; ";
        const string GetByIdsSql = @"SELECT * FROM `proc_process_route_detail_node`  WHERE Id IN @ids ";
        const string DeleteByProcessRouteIdSql = "delete from `proc_process_route_detail_node` WHERE ProcessRouteId = @ProcessRouteId ";
    }
}
