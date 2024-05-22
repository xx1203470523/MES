using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（点检任务）
    /// </summary>
    public partial class EquSpotcheckTaskRepository : BaseRepository, IEquSpotcheckTaskRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSpotcheckTaskRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckTaskEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquSpotcheckTaskEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckTaskEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquSpotcheckTaskEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckTaskEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTaskEntity>(GetByIdSql, new { Id = id });
        }

        public async Task<EquSpotcheckTaskUnionPlanEntity> GeUnionByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTaskUnionPlanEntity>(GetUnionByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTaskEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTaskEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTaskEntity>> GetEntitiesAsync(EquSpotcheckTaskQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTaskEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTaskUnionPlanEntity>> GetPagedListAsync(EquSpotcheckTaskPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("st.*,stsp.Code as PlanCode,stsp.Name as PlanName,stsp.Version,stsp.EquipmentId,stsp.ExecutorIds,stsp.LeaderIds,stsp.Type as PlanType,stsp.BeginTime as PlanBeginTime,stsp.EndTime PlanEndTime");
            sqlBuilder.OrderBy("st.UpdatedOn DESC");
            sqlBuilder.Where("st.IsDeleted = 0");
            sqlBuilder.Where("st.SiteId = @SiteId");
            sqlBuilder.LeftJoin("equ_spotcheck_task_snapshot_plan stsp on st.Id=stsp.SpotCheckTaskId");

            if (pagedQuery.EquipmentId.HasValue)
            {
                sqlBuilder.Where("stsp.EquipmentId LIKE @EquipmentId");
            }

            if (pagedQuery.PlanStartTime != null && pagedQuery.PlanStartTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { PlanStartTimeStart = pagedQuery.PlanStartTime[0], PlanStartTimeEnd = pagedQuery.PlanStartTime[1].AddDays(1) });
                sqlBuilder.Where("stsp.BeginTime >= @PlanStartTimeStart AND stsp.EndTime < @PlanStartTimeEnd");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquSpotcheckTaskUnionPlanEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckTaskUnionPlanEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquSpotcheckTaskRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_spotcheck_task st /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_spotcheck_task st /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_spotcheck_task /**where**/  ";

        const string InsertSql = "INSERT INTO equ_spotcheck_task(  `Id`, `Code`, `Name`, `BeginTime`, `EndTime`, `Status`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @BeginTime, @EndTime, @Status, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO equ_spotcheck_task(  `Id`, `Code`, `Name`, `BeginTime`, `EndTime`, `Status`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @BeginTime, @EndTime, @Status, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE equ_spotcheck_task SET   Code = @Code, Name = @Name, BeginTime = @BeginTime, EndTime = @EndTime, Status = @Status, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_spotcheck_task SET   Code = @Code, Name = @Name, BeginTime = @BeginTime, EndTime = @EndTime, Status = @Status, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_spotcheck_task SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_spotcheck_task SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_spotcheck_task WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_spotcheck_task WHERE Id IN @Ids ";

        const string GetUnionByIdSql = @"SELECT st.*,stsp.Code as PlanCode,stsp.Name as PlanName,stsp.Version,stsp.EquipmentId,stsp.ExecutorIds,stsp.LeaderIds,stsp.Type as PlanType,stsp.BeginTime PlanBeginTime,stsp.EndTime PlanEndTime
                                            FROM equ_spotcheck_task st
                                            LEFT JOIN equ_spotcheck_task_snapshot_plan stsp on st.Id=stsp.SpotCheckTaskId
                                            WHERE st.IsDeleted = 0 AND st.Id = @Id ";

    }
}
