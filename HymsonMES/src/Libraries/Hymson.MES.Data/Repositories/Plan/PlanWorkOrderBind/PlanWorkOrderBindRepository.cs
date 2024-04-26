using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单激活（物理删除）仓储
    /// </summary>
    public partial class PlanWorkOrderBindRepository : BaseRepository, IPlanWorkOrderBindRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public PlanWorkOrderBindRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
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
        public async Task<PlanWorkOrderBindEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderBindEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderBindEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderBindEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="planWorkOrderBindPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderBindEntity>> GetPagedInfoAsync(PlanWorkOrderBindPagedQuery planWorkOrderBindPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            var offSet = (planWorkOrderBindPagedQuery.PageIndex - 1) * planWorkOrderBindPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = planWorkOrderBindPagedQuery.PageSize });
            sqlBuilder.AddParameters(planWorkOrderBindPagedQuery);

            using var conn = GetMESDbConnection();
            var planWorkOrderBindEntitiesTask = conn.QueryAsync<PlanWorkOrderBindEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var planWorkOrderBindEntities = await planWorkOrderBindEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderBindEntity>(planWorkOrderBindEntities, planWorkOrderBindPagedQuery.PageIndex, planWorkOrderBindPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderBindQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderBindEntity>> GetPlanWorkOrderBindEntitiesAsync(PlanWorkOrderBindQuery planWorkOrderBindQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderBindEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where(" IsDeleted = 0 ");
            sqlBuilder.Where(" SiteId = @SiteId ");

            if (planWorkOrderBindQuery.ResourceId.HasValue)
            {
                sqlBuilder.Where(" ResourceId = @ResourceId ");
            }

            using var conn = GetMESDbConnection();
            var planWorkOrderBindEntities = await conn.QueryAsync<PlanWorkOrderBindEntity>(template.RawSql, planWorkOrderBindQuery);
            return planWorkOrderBindEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderBindEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderBindEntity planWorkOrderBindEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, planWorkOrderBindEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderBindEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<PlanWorkOrderBindEntity> planWorkOrderBindEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderBindEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderBindEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderBindEntity planWorkOrderBindEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderBindEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderBindEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<PlanWorkOrderBindEntity> planWorkOrderBindEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, planWorkOrderBindEntitys);
        }
        #endregion


        /// <summary>
        /// 根据资源ID批量删除（真删除）
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByResourceIdAsync(long resourceId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByResourceIdSql, new { ResourceId = resourceId });
        }

        /// <summary>
        /// 根据资源ID和工单Ids批量删除（真删除）
        /// </summary>
        /// <param name="comm"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByResourceIdAndWorkOrderIdsAsync(DeleteplanWorkOrderBindCommand comm)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByResourceIdAndWorkOrderIdsSql, comm);
        }


        /// <summary>
        /// 根据ResourceID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderBindEntity> GetByResourceIDAsync(PlanWorkOrderBindByResourceIdQuery query)
        {
            var key = $"{CachedTables.PLAN_WORK_ORDER_BIND}&{query.ResourceId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderBindEntity>(GetByResourceIdSql, query);
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkOrderBindRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order_bind` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `plan_work_order_bind` /**where**/ ";
        const string GetPlanWorkOrderBindEntitiesSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order_bind` /**where**/  ";

        const string InsertSql = "INSERT INTO `plan_work_order_bind`(  `Id`, `EquipmentId`, `ResourceId`, `WorkOrderId`, `CreatedBy`, `CreatedOn`, `SiteId`, UpdatedBy ,UpdatedOn ,IsDeleted ) VALUES (   @Id, @EquipmentId, @ResourceId, @WorkOrderId, @CreatedBy, @CreatedOn, @SiteId , @UpdatedBy ,@UpdatedOn ,@IsDeleted  )  ";
        const string InsertsSql = "INSERT INTO `plan_work_order_bind`(  `Id`, `EquipmentId`, `ResourceId`, `WorkOrderId`, `CreatedBy`, `CreatedOn`, `SiteId` , UpdatedBy ,UpdatedOn ,IsDeleted ) VALUES (   @Id, @EquipmentId, @ResourceId, @WorkOrderId, @CreatedBy, @CreatedOn, @SiteId,@UpdatedBy ,@UpdatedOn ,@IsDeleted )  ";

        const string UpdateSql = "UPDATE `plan_work_order_bind` SET   EquipmentId = @EquipmentId, ResourceId = @ResourceId, WorkOrderId = @WorkOrderId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, SiteId = @SiteId , UpdatedBy=@UpdatedBy ,UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `plan_work_order_bind` SET   EquipmentId = @EquipmentId, ResourceId = @ResourceId, WorkOrderId = @WorkOrderId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, SiteId = @SiteId , UpdatedBy=@UpdatedBy ,UpdatedOn=@UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `plan_work_order_bind` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `plan_work_order_bind` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `plan_work_order_bind`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `plan_work_order_bind`  WHERE Id IN @Ids ";

        const string DeletesTrueByResourceIdSql = "Delete from `plan_work_order_bind` WHERE ResourceId=@ResourceId ";
        const string DeletesTrueByResourceIdAndWorkOrderIdsSql = "Delete from `plan_work_order_bind` WHERE ResourceId=@ResourceId And WorkOrderId in @WorkOrderIds ";

        const string GetByResourceIdSql = @"SELECT * FROM `plan_work_order_bind`  WHERE  IsDeleted=0 AND SiteId=@SiteId AND ResourceId = @ResourceId ";

    }

}
