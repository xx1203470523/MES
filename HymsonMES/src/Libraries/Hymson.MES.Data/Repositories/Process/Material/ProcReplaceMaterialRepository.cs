/*
 *creator: Karl
 *
 *describe: 物料替代组件表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-09 11:28:39
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 物料替代组件表仓储
    /// </summary>
    public partial class ProcReplaceMaterialRepository : BaseRepository, IProcReplaceMaterialRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcReplaceMaterialRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
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
        /// 批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueByMaterialIdsAsync(long[] materialIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueByMaterialIdsSql, new { materialIds = materialIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcReplaceMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcReplaceMaterialEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据物料id查询替代料
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialEntity>> GetByMaterialIdAsync(long materialId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcReplaceMaterialEntity>(GetByMaterialIdSql, new { MaterialId = materialId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procReplaceMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcReplaceMaterialEntity>> GetPagedInfoAsync(ProcReplaceMaterialPagedQuery procReplaceMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (procReplaceMaterialPagedQuery.PageIndex - 1) * procReplaceMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procReplaceMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procReplaceMaterialPagedQuery);

            using var conn = GetMESDbConnection();
            var procReplaceMaterialEntitiesTask = conn.QueryAsync<ProcReplaceMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procReplaceMaterialEntities = await procReplaceMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcReplaceMaterialEntity>(procReplaceMaterialEntities, procReplaceMaterialPagedQuery.PageIndex, procReplaceMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialEntity>> GetProcReplaceMaterialEntitiesAsync(ProcReplaceMaterialQuery procReplaceMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcReplaceMaterialEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (procReplaceMaterialQuery.MaterialId != 0)
            {
                sqlBuilder.Where(" MaterialId=@MaterialId ");
            }

            using var conn = GetMESDbConnection();
            var procReplaceMaterialEntities = await conn.QueryAsync<ProcReplaceMaterialEntity>(template.RawSql, procReplaceMaterialQuery);
            return procReplaceMaterialEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewsAsync(ProcReplaceMaterialQuery procReplaceMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcReplaceMaterialViewsSqlTemplate);
            sqlBuilder.Where("r.IsDeleted = 0");
            sqlBuilder.Where("r.SiteId = @SiteId");

            if (procReplaceMaterialQuery.MaterialId != 0)
            {
                sqlBuilder.Where(" r.MaterialId = @MaterialId ");
            }

            using var conn = GetMESDbConnection();
            var ProcReplaceMaterialViews = await conn.QueryAsync<ProcReplaceMaterialView>(template.RawSql, procReplaceMaterialQuery);
            return ProcReplaceMaterialViews;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewsAsync(long siteId)
        {
            //var key = $"proc_replace_material&proc_material";
            var key = $"{CachedTables.PROC_REPLACE_MATERIAL}&{CachedTables.PROC_MATERIAL}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                var sqlBuilder = new SqlBuilder();
                var template = sqlBuilder.AddTemplate(GetProcReplaceMaterialViewsSqlTemplate);
                sqlBuilder.Where("r.IsDeleted = 0");
                sqlBuilder.Where("r.SiteId = @SiteId");

                using var conn = GetMESDbConnection();
                var ProcReplaceMaterialViews = await conn.QueryAsync<ProcReplaceMaterialView>(template.RawSql, new { SiteId = siteId });
                return ProcReplaceMaterialViews;
            });
        }

        /// <summary>
        /// 查询所有主物料的替代料
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewListAsync(long siteId)
        {
            var key = $"{CachedTables.PROC_REPLACE_MATERIAL}&{CachedTables.PROC_MATERIAL}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                var sqlBuilder = new SqlBuilder();
                var template = sqlBuilder.AddTemplate(GetAllProcReplaceMaterialViewSql);
                sqlBuilder.Where("r.IsDeleted = 0");
                sqlBuilder.Where("r.SiteId = @SiteId");

                using var conn = GetMESDbConnection();
                var ProcReplaceMaterialViews = await conn.QueryAsync<ProcReplaceMaterialView>(template.RawSql, new { SiteId = siteId });
                return ProcReplaceMaterialViews;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialView>> GetProcReplaceMaterialViewsAsync(ProcReplaceMaterialsQuery procReplaceMaterialQuery)
        {
            var key = $"{CachedTables.PROC_REPLACE_MATERIAL}&{CachedTables.PROC_MATERIAL}&{procReplaceMaterialQuery.SiteId}&{procReplaceMaterialQuery.MaterialIds}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                var sqlBuilder = new SqlBuilder();
                var template = sqlBuilder.AddTemplate(GetProcReplaceMaterialViewsSqlTemplate);
                sqlBuilder.Where("r.IsDeleted = 0");
                sqlBuilder.Where("r.SiteId = @SiteId");

                if (procReplaceMaterialQuery.MaterialIds.Any())
                {
                    sqlBuilder.Where(" r.MaterialId IN @MaterialIds ");
                }

                using var conn = GetMESDbConnection();
                var ProcReplaceMaterialViews = await conn.QueryAsync<ProcReplaceMaterialView>(template.RawSql, procReplaceMaterialQuery);
                return ProcReplaceMaterialViews;
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcReplaceMaterialEntity procReplaceMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcReplaceMaterialEntity> procReplaceMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procReplaceMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcReplaceMaterialEntity procReplaceMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量更新-只更新 UpdatedOn,  UpdatedBy,  ReplaceMaterialId,  IsUse 
        /// </summary>
        /// <param name="procReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcReplaceMaterialEntity> procReplaceMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procReplaceMaterialEntitys);
        }

        #region 顷刻

        /// <summary>
        /// 多个-根据物料id查询替代料
        /// </summary>
        /// <param name="materialIdList"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialEntity>> GetListByMaterialIdAsync(List<long> materialIdList)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcReplaceMaterialEntity>(GetListByMaterialIdSql, new { MaterialIdList = materialIdList });
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcReplaceMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_replace_material` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_replace_material` /**where**/ ";
        const string GetProcReplaceMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_replace_material` /**where**/  ";
        const string GetProcReplaceMaterialViewsSqlTemplate = @"SELECT 
                            r.ReplaceMaterialId as Id , m.MaterialName, m.MaterialCode, m.Version, m.SerialNumber, r.IsUse as IsEnabled
                        FROM `proc_replace_material` r
                        LEFT JOIN proc_material m on r.ReplaceMaterialId = m.id AND m.IsDeleted = 0
                    /**where**/  
                ";
        const string GetAllProcReplaceMaterialViewSql = @"SELECT r.ReplaceMaterialId, r.MaterialId, m.MaterialName, m.MaterialCode, m.Version, m.SerialNumber, r.IsUse as IsEnabled
                        FROM `proc_replace_material` r
                        LEFT JOIN proc_material m on r.ReplaceMaterialId = m.id AND m.IsDeleted = 0 /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_replace_material`(  `Id`, `SiteId`, `MaterialId`, `ReplaceMaterialId`, `IsUse`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @MaterialId, @ReplaceMaterialId, @IsUse, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_replace_material` SET   SiteId = @SiteId, MaterialId = @MaterialId, ReplaceMaterialId = @ReplaceMaterialId, IsUse = @IsUse, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_replace_material` SET   ReplaceMaterialId = @ReplaceMaterialId, IsUse = @IsUse, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_replace_material` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_replace_material` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @Ids";
        const string DeleteTrueByMaterialIdsSql = "DELETE From `proc_replace_material` WHERE  MaterialId=@materialIds";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `MaterialId`, `ReplaceMaterialId`, `IsUse`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_replace_material`  WHERE Id = @Id ";
        const string GetByMaterialIdSql = @"SELECT * FROM `proc_replace_material`  WHERE MaterialId = @MaterialId and IsUse=1 ";

        #region 顷刻

        /// <summary>
        /// 多个-根据物料id查询替代料
        /// </summary>
        const string GetListByMaterialIdSql = @"SELECT * FROM proc_replace_material  WHERE MaterialId in @MaterialIdList and IsUse=1 ";

        #endregion
    }
}
