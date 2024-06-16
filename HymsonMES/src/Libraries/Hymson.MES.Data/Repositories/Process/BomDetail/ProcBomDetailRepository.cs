using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM明细表仓储
    /// </summary>
    public partial class ProcBomDetailRepository : BaseRepository, IProcBomDetailRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcBomDetailRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
        }

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
        /// 批量删除关联的BomId的数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteBomIDAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteBomIDsSql, new { command.UserId, command.DeleteOn, bomIds = command.Ids, command });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBomDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据BomID查询物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetByBomIdAsync(long bomId)
        {
            var key = $"{CachedTables.PROC_BOM_DETAIL}&{bomId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<ProcBomDetailEntity>(GetByBomIdSql, new { bomId });
            });
        }

        /// <summary>
        /// 根据BomID和工序ID查询物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetByBomIdAndProcedureIdAsync(ProcBomDetailByBomIdAndProcedureIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailEntity>(GetByBomIdAndProcedureIdSql, query);
        }

        /// <summary>
        /// 根据BomID查询物料
        /// </summary>
        /// <param name="bomIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetByBomIdsAsync(IEnumerable<long> bomIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailEntity>(GetByBomIdsSql, new { bomIds });
        }

        /// <summary>
        /// 查询主物料表列表
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailView>> GetListMainAsync(long bomId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailView>(GetListMainSql, new { bomId = bomId });
        }

        /// <summary>
        /// 查询替代物料列表
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailView>> GetListReplaceAsync(long bomId)
        {
            using var conn = GetMESDbConnection();

            return await conn.QueryAsync<ProcBomDetailView>(GetListReplaceSql, new { bomId = bomId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDetailEntity>> GetPagedInfoAsync(ProcBomDetailPagedQuery procBomDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (procBomDetailPagedQuery.SiteId > 0)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }

            var offSet = (procBomDetailPagedQuery.PageIndex - 1) * procBomDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBomDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBomDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var procBomDetailEntitiesTask = conn.QueryAsync<ProcBomDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBomDetailEntities = await procBomDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBomDetailEntity>(procBomDetailEntities, procBomDetailPagedQuery.PageIndex, procBomDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailEntity>> GetProcBomDetailEntitiesAsync(ProcBomDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBomDetailEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");
            if (query.ProcedureIds != null && query.ProcedureIds.Any())
            {
                sqlBuilder.Where(" ProcedureId IN @ProcedureIds");
            }
            if (query.MaterialIds != null && query.MaterialIds.Any())
            {
                sqlBuilder.Where(" MaterialId IN @MaterialIds");
            }
            if (query.BomIds != null && query.BomIds.Any())
            {
                sqlBuilder.Where(" BomId IN @BomIds");
            }
            sqlBuilder.AddParameters(query);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailEntity>(template.RawSql, template.Parameters);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBomDetailEntity entity)
        {
            if (entity == null) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ProcBomDetailEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBomDetailEntity entity)
        {
            if (entity == null) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBomDetailEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcBomDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bom_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_bom_detail` /**where**/ ";
        const string GetProcBomDetailEntitiesSqlTemplate = @"SELECT  /**select**/  FROM `proc_bom_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bom_detail`(`Id`, `SiteId`, `BomId`, `ProcedureId`, `MaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, DataCollectionWay,IsEnableReplace,Seq) VALUES (@Id, @SiteId, @BomId, @ProcedureId, @MaterialId, @ReferencePoint, @Usages, @Loss, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted , @DataCollectionWay,@IsEnableReplace,@Seq )  ";
        const string UpdateSql = "UPDATE `proc_bom_detail` SET  ProcedureId = @ProcedureId, MaterialId = @MaterialId, ReferencePoint = @ReferencePoint, Usages = @Usages, Loss = @Loss, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn , DataCollectionWay=@DataCollectionWay,IsEnableReplace=@IsEnableReplace,Seq=@Seq WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_bom_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bom_detail` SET IsDeleted = Id,UpdatedBy = @UserId,UpdatedOn = @DeleteOn  WHERE Id in @ids";
        /// <summary>
        /// 批量删除关联的BomId的数据
        /// </summary>
        const string DeleteBomIDsSql = "UPDATE `proc_bom_detail` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE BomId IN @bomIds";
        const string GetByIdSql = @"SELECT * FROM `proc_bom_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_bom_detail`  WHERE Id IN @ids ";

        /// <summary>
        /// 查询主物料表列表
        /// </summary>
        const string GetListMainSql = @"SELECT 
                                          a.`Id`,a.MaterialId, 0 as BomDetailId,  a.`Usages`, a.`Loss`,a.`ReferencePoint`, a.`ProcedureId`, a.DataCollectionWay,a.IsEnableReplace,a.Seq,
                     1 as IsMain, b.MaterialCode, b.MaterialName,
                     b.Version, c.Name as ProcedureName, c.Code 
                            FROM `proc_bom_detail` a
                            INNER JOIN proc_material b on a.MaterialId = b.Id 
                            LEFT JOIN proc_procedure c on a.ProcedureId = c.Id
                            WHERE a.IsDeleted =0
                            AND a.BomId=@bomId
                            ORDER by a.UpdatedOn DESC ";
        /// <summary>
        /// 查询替代物料列表
        /// </summary>
        const string GetListReplaceSql = @"SELECT 
                                          a.`Id`,b.MaterialId, a.BomDetailId,
                                          a.`ReplaceMaterialId`, a.`Usages`, a.`Loss`,  a.`ReferencePoint`, a.DataCollectionWay,
                                          b.ProcedureId, 0 as IsMain,
                     c.MaterialCode, c.MaterialName,c.Version,  '' as ProcedureName, '' as Code
                            FROM `proc_bom_detail_replace_material` a
                            INNER JOIN proc_bom_detail b on a.BomDetailId = b.Id 
                            LEFT JOIN proc_material c on a.ReplaceMaterialId = c.Id
                            WHERE a.IsDeleted =0
                            AND a.BomId=@bomId
                            ORDER by a.UpdatedOn DESC ";
        const string GetByBomIdSql = @"SELECT * FROM proc_bom_detail WHERE IsDeleted = 0 AND BomId = @bomId order by Seq ";
        const string GetByBomIdsSql = @"SELECT * FROM proc_bom_detail WHERE IsDeleted = 0 AND BomId IN @bomIds ";

        const string GetByBomIdAndProcedureIdSql = @"SELECT * FROM proc_bom_detail WHERE IsDeleted = 0 AND BomId = @bomId AND ProcedureId=@ProcedureId order by Seq";

    }
}
