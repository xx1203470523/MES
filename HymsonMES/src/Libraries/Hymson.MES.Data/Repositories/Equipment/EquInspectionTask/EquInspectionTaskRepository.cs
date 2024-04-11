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
    /// 仓储（点检任务）
    /// </summary>
    public partial class EquInspectionTaskRepository : BaseRepository, IEquInspectionTaskRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquInspectionTaskRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquInspectionTaskEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquInspectionTaskEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquInspectionTaskEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquInspectionTaskEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
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
        public async Task<EquInspectionTaskEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquInspectionTaskEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionTaskEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquInspectionTaskEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionTaskEntity>> GetEntitiesAsync(EquInspectionTaskQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                sqlBuilder.Where("Code=@Code ");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquInspectionTaskEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquInspectionTaskView>> GetPagedListAsync(EquInspectionTaskPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Select("eit.*,iwc.Code as WorkCenterCode,ee.EquipmentCode,ee.EquipmentName");
            sqlBuilder.LeftJoin("inte_work_center iwc on iwc.Id=eit.WorkCenterId");
            sqlBuilder.LeftJoin("equ_equipment ee on ee.Id=eit.EquipmentId");

            sqlBuilder.Where("eit.IsDeleted = 0");
            sqlBuilder.Where("eit.SiteId = @SiteId");
            sqlBuilder.OrderBy("eit.UpdatedOn DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("eit.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where("ee.EquipmentCode LIKE @EquipmentCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.WorkCenterCode))
            {
                pagedQuery.WorkCenterCode = $"%{pagedQuery.WorkCenterCode}%";
                sqlBuilder.Where("iwc.Code LIKE @WorkCenterCode");
            }

            if (pagedQuery.InspectionType.HasValue)
            {
                sqlBuilder.Where("eit.InspectionType = @InspectionType");
            }

            if (pagedQuery.Type.HasValue)
            {
                sqlBuilder.Where("eit.Type = @Type");
            }

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("eit.Status = @Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquInspectionTaskView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquInspectionTaskView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquInspectionTaskRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_inspection_task eit /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_inspection_task eit /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_inspection_task /**where**/  ";

        const string InsertSql = "INSERT INTO equ_inspection_task(  `Id`, Code,`InspectionType`, `WorkCenterId`, `EquipmentId`, `Month`, `Day`, `Time`, `CompleteTime`, `Version`, `Status`, `Type`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id,@Code, @InspectionType, @WorkCenterId, @EquipmentId, @Month, @Day, @Time, @CompleteTime, @Version, @Status, @Type, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string UpdateSql = "UPDATE equ_inspection_task SET InspectionType = @InspectionType, WorkCenterId = @WorkCenterId, EquipmentId = @EquipmentId, Month = @Month, Day = @Day, Time = @Time, CompleteTime = @CompleteTime, Version = @Version, Status = @Status, Type = @Type, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_inspection_task SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_inspection_task SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string UpdateStatusSql = "UPDATE `equ_inspection_task` SET Status= @Status,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id = @Id ";

        const string GetByIdSql = @"SELECT * FROM equ_inspection_task WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_inspection_task WHERE Id IN @Ids ";

    }
}
