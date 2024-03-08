using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.ManuEquipmentStatusTime
{
    /// <summary>
    /// 仓储（设备状态时间）
    /// </summary>
    public partial class ManuEquipmentStatusTimeRepository : BaseRepository, IManuEquipmentStatusTimeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuEquipmentStatusTimeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuEquipmentStatusTimeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuEquipmentStatusTimeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuEquipmentStatusTimeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuEquipmentStatusTimeEntity> entities)
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
        public async Task<ManuEquipmentStatusTimeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuEquipmentStatusTimeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuEquipmentStatusTimeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuEquipmentStatusTimeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuEquipmentStatusTimeEntity>> GetEntitiesAsync(ManuEquipmentStatusTimeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuEquipmentStatusTimeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuEquipmentStatusTimeEntity>> GetPagedListAsync(ManuEquipmentStatusTimePagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ManuEquipmentStatusTimeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuEquipmentStatusTimeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuEquipmentStatusTimeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_equipment_status_time /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_equipment_status_time /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_equipment_status_time /**where**/  ";

        const string InsertSql = "INSERT INTO manu_equipment_status_time(  `Id`, `EquipmentId`, `CurrentStatus`, `NextStatus`, `BeginTime`, `EndTime`, `StatusDuration`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`, `EquipmentDownReason`) VALUES (  @Id, @EquipmentId, @CurrentStatus, @NextStatus, @BeginTime, @EndTime, @StatusDuration, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId, @EquipmentDownReason) ";
        const string InsertsSql = "INSERT INTO manu_equipment_status_time(  `Id`, `EquipmentId`, `CurrentStatus`, `NextStatus`, `BeginTime`, `EndTime`, `StatusDuration`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`, `EquipmentDownReason`) VALUES (  @Id, @EquipmentId, @CurrentStatus, @NextStatus, @BeginTime, @EndTime, @StatusDuration, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId, @EquipmentDownReason) ";

        const string UpdateSql = "UPDATE manu_equipment_status_time SET   EquipmentId = @EquipmentId, CurrentStatus = @CurrentStatus, NextStatus = @NextStatus, BeginTime = @BeginTime, EndTime = @EndTime, StatusDuration = @StatusDuration, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId, EquipmentDownReason = @EquipmentDownReason WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_equipment_status_time SET   EquipmentId = @EquipmentId, CurrentStatus = @CurrentStatus, NextStatus = @NextStatus, BeginTime = @BeginTime, EndTime = @EndTime, StatusDuration = @StatusDuration, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteId = @SiteId, EquipmentDownReason = @EquipmentDownReason WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_equipment_status_time SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_equipment_status_time SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_equipment_status_time WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_equipment_status_time WHERE Id IN @Ids ";

    }
}
