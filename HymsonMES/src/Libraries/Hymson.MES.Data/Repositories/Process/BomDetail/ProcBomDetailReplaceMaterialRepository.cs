using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// BOM明细替代料表仓储
    /// </summary>
    public partial class ProcBomDetailReplaceMaterialRepository : BaseRepository, IProcBomDetailReplaceMaterialRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcBomDetailReplaceMaterialRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
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
            return await conn.ExecuteAsync(DeleteBomIDsSql, new { command.UserId, command.DeleteOn, bomIds = command.Ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDetailReplaceMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBomDetailReplaceMaterialEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据BomID查询替代物料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetByBomIdAsync(long bomId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(GetByBomIdSql, new { bomId });
        }

        /// <summary>
        /// 根据BomID查询替代物料
        /// </summary>
        /// <param name="bomIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetByBomIdsAsync(IEnumerable<long> bomIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(GetByBomIdsSql, new { bomIds });
        }

        /// <summary>
        /// 根据BomDetailId查询替代物料
        /// </summary>
        /// <param name="bomDetailId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetByBomDetailIdAsync(long bomDetailId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(GetByBomDetailIdSql, new { BomDetailId = bomDetailId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDetailReplaceMaterialEntity>> GetPagedInfoAsync(ProcBomDetailReplaceMaterialPagedQuery procBomDetailReplaceMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (procBomDetailReplaceMaterialPagedQuery.PageIndex - 1) * procBomDetailReplaceMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBomDetailReplaceMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBomDetailReplaceMaterialPagedQuery);

            using var conn = GetMESDbConnection();
            var procBomDetailReplaceMaterialEntitiesTask = conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBomDetailReplaceMaterialEntities = await procBomDetailReplaceMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBomDetailReplaceMaterialEntity>(procBomDetailReplaceMaterialEntities, procBomDetailReplaceMaterialPagedQuery.PageIndex, procBomDetailReplaceMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetProcBomDetailReplaceMaterialEntitiesAsync(ProcBomDetailReplaceMaterialQuery procBomDetailReplaceMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBomDetailReplaceMaterialEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procBomDetailReplaceMaterialEntities = await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(template.RawSql, procBomDetailReplaceMaterialQuery);
            return procBomDetailReplaceMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBomDetailReplaceMaterialEntity procBomDetailReplaceMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBomDetailReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBomDetailReplaceMaterialEntity> procBomDetailReplaceMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBomDetailReplaceMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBomDetailReplaceMaterialEntity procBomDetailReplaceMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procBomDetailReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBomDetailReplaceMaterialEntity> procBomDetailReplaceMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procBomDetailReplaceMaterialEntitys);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcBomDetailReplaceMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bom_detail_replace_material` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_bom_detail_replace_material` /**where**/ ";
        const string GetProcBomDetailReplaceMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_bom_detail_replace_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bom_detail_replace_material`(  `Id`, `SiteId`, `BomId`, `BomDetailId`, `ReplaceMaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,DataCollectionWay) VALUES (   @Id, @SiteId, @BomId, @BomDetailId, @ReplaceMaterialId, @ReferencePoint, @Usages, @Loss, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @DataCollectionWay )  ";
        const string UpdateSql = "UPDATE `proc_bom_detail_replace_material` SET BomDetailId = @BomDetailId, ReplaceMaterialId = @ReplaceMaterialId, ReferencePoint = @ReferencePoint, Usages = @Usages, Loss = @Loss, Remark = @Remark,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn , DataCollectionWay=@DataCollectionWay WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_bom_detail_replace_material` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bom_detail_replace_material` SET IsDeleted = Id,UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";

        /// <summary>
        /// 批量删除关联的BomId的数据
        /// </summary>
        const string DeleteBomIDsSql = "DELETE FROM proc_bom_detail_replace_material WHERE BomId IN @bomIds ";
        const string GetByIdSql = @"SELECT * FROM `proc_bom_detail_replace_material`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_bom_detail_replace_material`  WHERE Id IN @ids ";
        const string GetByBomIdSql = "SELECT * FROM proc_bom_detail_replace_material WHERE IsDeleted = 0 AND BomId = @bomId ";
        const string GetByBomIdsSql = "SELECT * FROM proc_bom_detail_replace_material WHERE IsDeleted = 0 AND BomId IN @bomIds ";
        const string GetByBomDetailIdSql = "SELECT * FROM proc_bom_detail_replace_material WHERE IsDeleted = 0 AND BomDetailId = @BomDetailId ";
    }
}
