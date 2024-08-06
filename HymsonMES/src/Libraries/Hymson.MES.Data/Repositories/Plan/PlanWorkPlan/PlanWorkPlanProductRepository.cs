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
    /// 仓储（生产计划产品表）
    /// </summary>
    public partial class PlanWorkPlanProductRepository : BaseRepository, IPlanWorkPlanProductRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public PlanWorkPlanProductRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<PlanWorkPlanProductEntity> entities)
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
        public async Task<int> UpdateAsync(PlanWorkPlanProductEntity entity)
        {
            if (entity == null) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
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
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteByParentIdsAsync(DeleteByParentIdsCommand command)
        {
            if (command == null || command.ParentIds == null || !command.ParentIds.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByParentIdsSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkPlanProductEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkPlanProductEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanProductEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            if (!ids.Any()) return Array.Empty<PlanWorkPlanProductEntity>();

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkPlanProductEntity>(GetByIdsSql, new { ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPlanProductEntity>> GetPagedInfoAsync(PlanWorkPlanProductPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");

            if (pageQuery.WorkPlanIds != null) sqlBuilder.Where("WorkPlanId IN @WorkPlanIds");

            if (!string.IsNullOrWhiteSpace(pageQuery.ProductCode))
            {
                pageQuery.ProductCode = $"%{pageQuery.ProductCode}%";
                sqlBuilder.Where("ProductCode LIKE @ProductCode");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<PlanWorkPlanProductEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<PlanWorkPlanProductEntity>(entities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkPlanProductEntity>> GetEntitiesAsync(PlanWorkPlanQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkPlanProductEntity>(template.RawSql, query);
        }

        public async Task<PlanWorkPlanProductEntity> GetByPlanIdAndProductIdAsync(long id, long productId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkPlanProductEntity>(GetByIdSql, new { Id = id });
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkPlanProductRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM plan_work_plan_product /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset, @Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM plan_work_plan_product /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM plan_work_plan_product /**where**/  ";

        const string InsertsSql = "INSERT INTO plan_work_plan_product(Id, WorkPlanId, ProductCode, ProductVersion, ProductId, BomCode, BomVersion, BomId, Qty, OverScale, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId) VALUES (@Id, @WorkPlanId, @ProductCode, @ProductVersion, @ProductId, @BomCode, @BomVersion, @BomId, @Qty, @OverScale, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId); ";

        const string UpdateSql = "UPDATE plan_work_plan_product SET IsOpen = @IsOpen, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeletesSql = "UPDATE plan_work_plan_product SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @ids ";
        //const string DeleteByParentIdsSql = "UPDATE plan_work_plan_product SET IsDeleted = Id , UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE WorkPlanId IN @ParentIds";
        const string DeleteByParentIdsSql = "DELETE FROM plan_work_plan_product WHERE WorkPlanId IN @ParentIds";

        const string GetByIdSql = "SELECT * FROM plan_work_plan_product WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM plan_work_plan_product WHERE Id IN @ids ";

    }
}
