using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 仓储（生产计划表）
    /// </summary>
    public partial class PlanWorkPlanRepository : BaseRepository, IPlanWorkPlanRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public PlanWorkPlanRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<PlanWorkPlanEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkPlanEntity entity)
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
        public async Task<int> UpdatesAsync(IEnumerable<PlanWorkPlanEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="param"></param>
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
        public async Task<PlanWorkPlanEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkPlanEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPlanEntity>> GetPagedInfoAsync(PlanWorkPlanPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("wo.IsDeleted = 0");
            sqlBuilder.Where("wo.SiteId = @SiteId");

            // TODO

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<PlanWorkPlanEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<PlanWorkPlanEntity>(entities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanEntity>> GetEntitiesAsync(PlanWorkPlanQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkPlanEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkPlanRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM plan_work_plan /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset, @Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM plan_work_plan /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM plan_work_plan /**where**/  ";

        const string InsertsSql = "INSERT INTO plan_work_plan(Id, PlanCode, ProductCode, ProductVersion, ProductId, BomCode, BomVersion, BomId, Type, Qty, Status, RequirementNumber, OverScale, StartTime, EndTime, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId) VALUES (@Id, @PlanCode, @ProductCode, @ProductVersion, @ProductId, @BomCode, @BomVersion, @BomId, @Type, @Qty, @Status, @RequirementNumber, @OverScale, @StartTime, @EndTime, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE plan_work_plan SET ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE plan_work_plan SET PlanCode = @PlanCode, ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, Status = @Status, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, IsLocked = @IsLocked, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeletesSql = "UPDATE plan_work_plan SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @ids ";

        const string GetByIdSql = "SELECT * FROM plan_work_plan WHERE Id = @Id ";

    }
}