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
    /// 仓储（工具绑定设备操作记录表）
    /// </summary>
    public partial class EquToolsEquipmentBindRecordRepository : BaseRepository, IEquToolsEquipmentBindRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolsEquipmentBindRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolsEquipmentBindRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquToolsEquipmentBindRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquToolsEquipmentBindRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquToolsEquipmentBindRecordEntity> entities)
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
        public async Task<EquToolsEquipmentBindRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolsEquipmentBindRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsEquipmentBindRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsEquipmentBindRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsEquipmentBindRecordEntity>> GetEntitiesAsync(EquToolsEquipmentBindRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsEquipmentBindRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolsEquipmentBindRecordEntity>> GetPagedListAsync(EquToolsEquipmentBindRecordPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<EquToolsEquipmentBindRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolsEquipmentBindRecordEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquToolsEquipmentBindRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_tools_equipment__bind_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_tools_equipment__bind_record /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_tools_equipment__bind_record /**where**/  ";

        const string InsertSql = "INSERT INTO equ_tools_equipment__bind_record(  `Id`, `SiteId`, `ToolId`, `ToolsRecordId`, `EquipmentId`, `EquipmentRecordId`, `Position`, `OperationType`, `UninstallReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `UninstallBy`, `UninstallOn`) VALUES (  @Id, @SiteId, @ToolId, @ToolsRecordId, @EquipmentId, @EquipmentRecordId, @Position, @OperationType, @UninstallReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @UninstallBy, @UninstallOn) ";
        const string InsertsSql = "INSERT INTO equ_tools_equipment__bind_record(  `Id`, `SiteId`, `ToolId`, `ToolsRecordId`, `EquipmentId`, `EquipmentRecordId`, `Position`, `OperationType`, `UninstallReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `UninstallBy`, `UninstallOn`) VALUES (  @Id, @SiteId, @ToolId, @ToolsRecordId, @EquipmentId, @EquipmentRecordId, @Position, @OperationType, @UninstallReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @UninstallBy, @UninstallOn) ";

        const string UpdateSql = "UPDATE equ_tools_equipment__bind_record SET   SiteId = @SiteId, ToolId = @ToolId, ToolsRecordId = @ToolsRecordId, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, Position = @Position, OperationType = @OperationType, UninstallReason = @UninstallReason, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, UninstallBy = @UninstallBy, UninstallOn = @UninstallOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_tools_equipment__bind_record SET   SiteId = @SiteId, ToolId = @ToolId, ToolsRecordId = @ToolsRecordId, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, Position = @Position, OperationType = @OperationType, UninstallReason = @UninstallReason, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, UninstallBy = @UninstallBy, UninstallOn = @UninstallOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_tools_equipment__bind_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_tools_equipment__bind_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_tools_equipment__bind_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_tools_equipment__bind_record WHERE Id IN @Ids ";

    }
}
