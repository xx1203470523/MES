using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（点检记录表）
    /// </summary>
    public partial class EquInspectionRecordRepository : BaseRepository, IEquInspectionRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquInspectionRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquInspectionRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquInspectionRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquInspectionRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquInspectionRecordEntity> entities)
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
        public async Task<EquInspectionRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquInspectionRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquInspectionRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionRecordEntity>> GetEntitiesAsync(EquInspectionRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquInspectionRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquInspectionRecordView>> GetPagedListAsync(EquInspectionRecordPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Select(@"eir.Id,eir.StartExecuTime,eir.InspectionTaskSnapshootId,eit.Code as OrderCode,ee.EquipmentCode,ee.EquipmentName,eit.WorkCenterId,eit.InspectionType,eit.Type,eir.UpdatedBy,eir.UpdatedOn,eit.CompleteTime,iwc.Code as WorkCenterCode,eir.Status,eir.IsQualified");

            sqlBuilder.OrderBy("eir.UpdatedOn DESC");
            sqlBuilder.Where("eir.IsDeleted = 0");
            sqlBuilder.Where("eir.SiteId = @SiteId");

            sqlBuilder.InnerJoin("equ_inspection_task_snapshoot eit on eit.Id=eir.InspectionTaskSnapshootId");
            sqlBuilder.LeftJoin("equ_equipment ee on ee.Id=eit.EquipmentId");
            sqlBuilder.LeftJoin("inte_work_center iwc on iwc.Id=eit.WorkCenterId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where(" ee.EquipmentCode like @EquipmentCode ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentName))
            {
                pagedQuery.EquipmentName = $"%{pagedQuery.EquipmentName}%";
                sqlBuilder.Where(" ee.EquipmentName like @EquipmentName ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.WorkCenterCode))
            {
                pagedQuery.WorkCenterCode = $"%{pagedQuery.WorkCenterCode}%";
                sqlBuilder.Where(" iwc.Code like @WorkCenterCode ");
            }
            if (pagedQuery.WorkCenterId.HasValue)
            {
                sqlBuilder.Where(" eit.WorkCenterId=@WorkCenterId ");
            }
            if (pagedQuery.InspectionType.HasValue)
            {
                sqlBuilder.Where(" eit.InspectionType=@InspectionType ");
            }
            if (pagedQuery.Type.HasValue)
            {
                sqlBuilder.Where(" eit.Type=@Type ");
            }
            if (pagedQuery.StartExecuTime.HasValue&& pagedQuery.StartExecuTime!=DateTime.MinValue)
            {
                sqlBuilder.Where("eir.StartExecuTime>=@StartExecuTime");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquInspectionRecordView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquInspectionRecordView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquInspectionRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_inspection_record eir /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_inspection_record  eir /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_inspection_record /**where**/  ";

        const string InsertSql = "INSERT INTO equ_inspection_record(  `Id`, `OrderCode`, `InspectionTaskSnapshootId`, `StartExecuTime`, `Status`, `IsQualified`, `IsNoticeRepair`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @OrderCode, @InspectionTaskSnapshootId, @StartExecuTime, @Status, @IsQualified, @IsNoticeRepair, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO equ_inspection_record(  `Id`, `OrderCode`, `InspectionTaskSnapshootId`, `StartExecuTime`, `Status`, `IsQualified`, `IsNoticeRepair`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @OrderCode, @InspectionTaskSnapshootId, @StartExecuTime, @Status, @IsQualified, @IsNoticeRepair, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE equ_inspection_record SET   OrderCode = @OrderCode, InspectionTaskSnapshootId = @InspectionTaskSnapshootId, StartExecuTime = @StartExecuTime, Status = @Status, IsQualified = @IsQualified, IsNoticeRepair = @IsNoticeRepair, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_inspection_record SET   OrderCode = @OrderCode, InspectionTaskSnapshootId = @InspectionTaskSnapshootId, StartExecuTime = @StartExecuTime, Status = @Status, IsQualified = @IsQualified, IsNoticeRepair = @IsNoticeRepair, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_inspection_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_inspection_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_inspection_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_inspection_record WHERE Id IN @Ids ";

    }
}
