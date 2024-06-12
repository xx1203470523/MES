using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（设备保养任务）
    /// </summary>
    public partial class EquMaintenanceTaskRepository : BaseRepository, IEquMaintenanceTaskRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquMaintenanceTaskRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenanceTaskEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquMaintenanceTaskEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenanceTaskEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquMaintenanceTaskEntity> entities)
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
        public async Task<EquMaintenanceTaskEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTaskEntity>(GetByIdSql, new { Id = id });
        }

        public async Task<EquMaintenanceTaskUnionPlanEntity> GeUnionByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTaskUnionPlanEntity>(GetUnionByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTaskEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTaskEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTaskEntity>> GetEntitiesAsync(EquMaintenanceTaskQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTaskEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTaskUnionPlanEntity>> GetPagedListAsync(EquMaintenanceTaskPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("st.*,stsp.Code as PlanCode,stsp.Name as PlanName,stsp.Version,stsp.EquipmentId,stsp.ExecutorIds,stsp.LeaderIds,stsp.Type as PlanType,stsp.BeginTime as PlanBeginTime,stsp.EndTime PlanEndTime");
            sqlBuilder.OrderBy("st.UpdatedOn DESC");
            sqlBuilder.Where("st.IsDeleted = 0");
            sqlBuilder.Where("st.SiteId = @SiteId");
            sqlBuilder.LeftJoin("equ_spotcheck_task_snapshot_plan stsp on st.Id=stsp.SpotCheckTaskId");

            if (pagedQuery.TaskIds != null && pagedQuery.TaskIds.Any())
            {
                sqlBuilder.Where("st.Id IN @TaskIds");
            }
            //任务编码
            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                sqlBuilder.Where("st.Code = @Code");
            }
            //任务名
            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                sqlBuilder.Where("st.Name = @Name");
            }
            //负责人
            if (!string.IsNullOrWhiteSpace(pagedQuery.LeaderIds))
            {
                sqlBuilder.Where("stsp.LeaderIds = @LeaderIds");
            }
            //设备
            if (pagedQuery.EquipmentId.HasValue)
            {
                sqlBuilder.Where("stsp.EquipmentId LIKE @EquipmentId");
            }
            //任务状态
            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("st.Status = @Status");
            }
            //是否合格
            if (pagedQuery.IsQualified.HasValue)
            {
                sqlBuilder.Where("st.IsQualified = @IsQualified");
            }
            //计划开始结束时间
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
            var entitiesTask = conn.QueryAsync<EquMaintenanceTaskUnionPlanEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenanceTaskUnionPlanEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquMaintenanceTaskRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_maintenance_task st /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_maintenance_task st /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_maintenance_task /**where**/  ";

        const string InsertSql = "INSERT INTO equ_maintenance_task(  `Id`, `Code`, `Name`, `BeginTime`, `EndTime`, `Status`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @BeginTime, @EndTime, @Status, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO equ_maintenance_task(  `Id`, `Code`, `Name`, `BeginTime`, `EndTime`, `Status`, `IsQualified`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @BeginTime, @EndTime, @Status, @IsQualified, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE equ_maintenance_task SET   Code = @Code, Name = @Name, BeginTime = @BeginTime, EndTime = @EndTime, Status = @Status, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_maintenance_task SET   Code = @Code, Name = @Name, BeginTime = @BeginTime, EndTime = @EndTime, Status = @Status, IsQualified = @IsQualified, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_maintenance_task SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_maintenance_task SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_maintenance_task WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_maintenance_task WHERE Id IN @Ids ";

        const string GetUnionByIdSql = @"SELECT st.*,stsp.Code as PlanCode,stsp.Name as PlanName,stsp.Version,stsp.EquipmentId,stsp.ExecutorIds,stsp.LeaderIds,stsp.Type as PlanType,stsp.BeginTime PlanBeginTime,stsp.EndTime PlanEndTime
                                            FROM equ_maintenance_task st
                                            LEFT JOIN equ_maintenance_task_snapshot_plan stsp on st.Id=stsp.MaintenanceTaskId
                                            WHERE st.IsDeleted = 0 AND st.Id = @Id ";

    }
}