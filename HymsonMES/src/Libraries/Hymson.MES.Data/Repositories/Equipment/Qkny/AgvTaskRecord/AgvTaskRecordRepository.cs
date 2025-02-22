using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.AgvTaskRecord;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.AgvTaskRecord.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.AgvTaskRecord
{
    /// <summary>
    /// 仓储（AGV任务记录表）
    /// </summary>
    public partial class AgvTaskRecordRepository : BaseRepository, IAgvTaskRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public AgvTaskRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(AgvTaskRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<AgvTaskRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(AgvTaskRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<AgvTaskRecordEntity> entities)
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
        public async Task<AgvTaskRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<AgvTaskRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AgvTaskRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<AgvTaskRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AgvTaskRecordEntity>> GetEntitiesAsync(AgvTaskRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<AgvTaskRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<AgvTaskRecordEntity>> GetPagedListAsync(AgvTaskRecordPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<AgvTaskRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<AgvTaskRecordEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class AgvTaskRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM agv_task_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM agv_task_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM agv_task_record /**where**/  ";

        const string InsertSql = "INSERT INTO agv_task_record(  `Id`, `EquipmentId`, `TaskType`, `SendContent`, `ReceiveContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @TaskType, @SendContent, @ReceiveContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";
        const string InsertsSql = "INSERT INTO agv_task_record(  `Id`, `EquipmentId`, `TaskType`, `SendContent`, `ReceiveContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (  @Id, @EquipmentId, @TaskType, @SendContent, @ReceiveContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId) ";

        const string UpdateSql = "UPDATE agv_task_record SET   EquipmentId = @EquipmentId, TaskType = @TaskType, SendContent = @SendContent, ReceiveContent = @ReceiveContent, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE agv_task_record SET   EquipmentId = @EquipmentId, TaskType = @TaskType, SendContent = @SendContent, ReceiveContent = @ReceiveContent, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE agv_task_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE agv_task_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM agv_task_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM agv_task_record WHERE Id IN @Ids ";

    }
}
