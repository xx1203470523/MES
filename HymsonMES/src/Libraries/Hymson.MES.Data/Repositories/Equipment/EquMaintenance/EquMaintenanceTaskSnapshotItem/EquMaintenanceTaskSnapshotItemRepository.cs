using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（设备保养快照任务项目）
    /// </summary>
    public partial class EquMaintenanceTaskSnapshotItemRepository : BaseRepository, IEquMaintenanceTaskSnapshotItemRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquMaintenanceTaskSnapshotItemRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenanceTaskSnapshotItemEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquMaintenanceTaskSnapshotItemEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenanceTaskSnapshotItemEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquMaintenanceTaskSnapshotItemEntity> entities)
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
        public async Task<EquMaintenanceTaskSnapshotItemEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTaskSnapshotItemEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTaskSnapshotItemEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTaskSnapshotItemEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTaskSnapshotItemEntity>> GetEntitiesAsync(EquMaintenanceTaskSnapshotItemQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTaskSnapshotItemEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTaskSnapshotItemEntity>> GetPagedListAsync(EquMaintenanceTaskSnapshotItemPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<EquMaintenanceTaskSnapshotItemEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenanceTaskSnapshotItemEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquMaintenanceTaskSnapshotItemRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_maintenance_task_snapshot_item /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_maintenance_task_snapshot_item /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_maintenance_task_snapshot_item /**where**/  ";

        const string InsertSql = "INSERT INTO equ_maintenance_task_snapshot_item(  `Id`, `MaintenanceTaskId`, `MaintenanceItemId`, `Code`, `Name`, `Status`, `DataType`, `CheckType`, `CheckMethod`, `UnitId`, `OperationContent`, `Components`, `LowerLimit`, `ReferenceValue`, `UpperLimit`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @MaintenanceTaskId, @MaintenanceItemId, @Code, @Name, @Status, @DataType, @CheckType, @CheckMethod, @UnitId, @OperationContent, @Components, @LowerLimit, @ReferenceValue, @UpperLimit, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO equ_maintenance_task_snapshot_item(  `Id`, `MaintenanceTaskId`, `MaintenanceItemId`, `Code`, `Name`, `Status`, `DataType`, `CheckType`, `CheckMethod`, `UnitId`, `OperationContent`, `Components`, `LowerLimit`, `ReferenceValue`, `UpperLimit`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @MaintenanceTaskId, @MaintenanceItemId, @Code, @Name, @Status, @DataType, @CheckType, @CheckMethod, @UnitId, @OperationContent, @Components, @LowerLimit, @ReferenceValue, @UpperLimit, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE equ_maintenance_task_snapshot_item SET   MaintenanceTaskId = @MaintenanceTaskId, MaintenanceItemId = @MaintenanceItemId, Code = @Code, Name = @Name, Status = @Status, DataType = @DataType, CheckType = @CheckType, CheckMethod = @CheckMethod, UnitId = @UnitId, OperationContent = @OperationContent, Components = @Components, LowerLimit = @LowerLimit, ReferenceValue = @ReferenceValue, UpperLimit = @UpperLimit, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_maintenance_task_snapshot_item SET   MaintenanceTaskId = @MaintenanceTaskId, MaintenanceItemId = @MaintenanceItemId, Code = @Code, Name = @Name, Status = @Status, DataType = @DataType, CheckType = @CheckType, CheckMethod = @CheckMethod, UnitId = @UnitId, OperationContent = @OperationContent, Components = @Components, LowerLimit = @LowerLimit, ReferenceValue = @ReferenceValue, UpperLimit = @UpperLimit, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_maintenance_task_snapshot_item SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_maintenance_task_snapshot_item SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_maintenance_task_snapshot_item WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_maintenance_task_snapshot_item WHERE Id IN @Ids ";

    }
}
