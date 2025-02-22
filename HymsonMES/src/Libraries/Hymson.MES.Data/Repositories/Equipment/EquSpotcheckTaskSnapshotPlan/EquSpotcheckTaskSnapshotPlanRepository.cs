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
    /// 仓储（设备点检快照任务计划）
    /// </summary>
    public partial class EquSpotcheckTaskSnapshotPlanRepository : BaseRepository, IEquSpotcheckTaskSnapshotPlanRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSpotcheckTaskSnapshotPlanRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckTaskSnapshotPlanEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquSpotcheckTaskSnapshotPlanEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckTaskSnapshotPlanEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquSpotcheckTaskSnapshotPlanEntity> entities)
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
        public async Task<EquSpotcheckTaskSnapshotPlanEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTaskSnapshotPlanEntity>(GetByIdSql, new { Id = id });
        }

        public async Task<EquSpotcheckTaskSnapshotPlanEntity> GetByTaskIdAsync(long taskId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTaskSnapshotPlanEntity>(GetByTaskIdSql, new { TaskId = taskId });
        }
 


        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTaskSnapshotPlanEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTaskSnapshotPlanEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTaskSnapshotPlanEntity>> GetEntitiesAsync(EquSpotcheckTaskSnapshotPlanQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTaskSnapshotPlanEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTaskSnapshotPlanEntity>> GetPagedListAsync(EquSpotcheckTaskSnapshotPlanPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquSpotcheckTaskSnapshotPlanEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckTaskSnapshotPlanEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquSpotcheckTaskSnapshotPlanRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_spotcheck_task_snapshot_plan /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_spotcheck_task_snapshot_plan /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_spotcheck_task_snapshot_plan /**where**/  ";

        const string InsertSql = "INSERT INTO equ_spotcheck_task_snapshot_plan(  `Id`, `SpotCheckTaskId`, `SpotCheckPlanId`, `Code`, `Name`, `Version`, `EquipmentId`, `SpotCheckTemplateId`, `ExecutorIds`, `LeaderIds`, `Type`, `Status`, `BeginTime`, `EndTime`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @SpotCheckTaskId, @SpotCheckPlanId, @Code, @Name, @Version, @EquipmentId, @SpotCheckTemplateId, @ExecutorIds, @LeaderIds, @Type, @Status, @BeginTime, @EndTime, @IsSkipHoliday, @FirstExecuteTime, @Cycle, @CompletionHour, @CompletionMinute, @PreGeneratedMinute, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO equ_spotcheck_task_snapshot_plan(  `Id`, `SpotCheckTaskId`, `SpotCheckPlanId`, `Code`, `Name`, `Version`, `EquipmentId`, `SpotCheckTemplateId`, `ExecutorIds`, `LeaderIds`, `Type`, `Status`, `BeginTime`, `EndTime`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @SpotCheckTaskId, @SpotCheckPlanId, @Code, @Name, @Version, @EquipmentId, @SpotCheckTemplateId, @ExecutorIds, @LeaderIds, @Type, @Status, @BeginTime, @EndTime, @IsSkipHoliday, @FirstExecuteTime, @Cycle, @CompletionHour, @CompletionMinute, @PreGeneratedMinute, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE equ_spotcheck_task_snapshot_plan SET   SpotCheckTaskId = @SpotCheckTaskId, SpotCheckPlanId = @SpotCheckPlanId, Code = @Code, Name = @Name, Version = @Version, EquipmentId = @EquipmentId, SpotCheckTemplateId = @SpotCheckTemplateId, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds, Type = @Type, Status = @Status, BeginTime = @BeginTime, EndTime = @EndTime, IsSkipHoliday = @IsSkipHoliday, FirstExecuteTime = @FirstExecuteTime, Cycle = @Cycle, CompletionHour = @CompletionHour, CompletionMinute = @CompletionMinute, PreGeneratedMinute = @PreGeneratedMinute, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_spotcheck_task_snapshot_plan SET   SpotCheckTaskId = @SpotCheckTaskId, SpotCheckPlanId = @SpotCheckPlanId, Code = @Code, Name = @Name, Version = @Version, EquipmentId = @EquipmentId, SpotCheckTemplateId = @SpotCheckTemplateId, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds, Type = @Type, Status = @Status, BeginTime = @BeginTime, EndTime = @EndTime, IsSkipHoliday = @IsSkipHoliday, FirstExecuteTime = @FirstExecuteTime, Cycle = @Cycle, CompletionHour = @CompletionHour, CompletionMinute = @CompletionMinute, PreGeneratedMinute = @PreGeneratedMinute, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_spotcheck_task_snapshot_plan SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_spotcheck_task_snapshot_plan SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_spotcheck_task_snapshot_plan WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_spotcheck_task_snapshot_plan WHERE Id IN @Ids ";

        const string GetByTaskIdSql = @"SELECT * FROM equ_spotcheck_task_snapshot_plan WHERE SpotCheckTaskId = @TaskId ";

    }
}
