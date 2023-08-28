using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspectionHead.View;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（首检检验单）
    /// </summary>
    public partial class QualIpqcInspectionHeadRepository : BaseRepository, IQualIpqcInspectionHeadRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIpqcInspectionHeadRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIpqcInspectionHeadEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIpqcInspectionHeadEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIpqcInspectionHeadEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIpqcInspectionHeadEntity> entities)
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
        public async Task<QualIpqcInspectionHeadEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIpqcInspectionHeadEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionHeadEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionHeadEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionHeadEntity>> GetEntitiesAsync(QualIpqcInspectionHeadQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionHeadEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionHeadView>> GetPagedListAsync(QualIpqcInspectionHeadPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("qual_ipqc_inspection_head_result qiihr ON qiihr.IpqcInspectionHeadId = T.Id");
            sqlBuilder.LeftJoin("qual_ipqc_inspection qii ON T.IpqcInspectionId = qii.Id");
            sqlBuilder.LeftJoin("proc_material pm ON T.MaterialId = pm.Id");
            sqlBuilder.LeftJoin("proc_procedure pp ON T.ProcedureId = pp.Id");
            sqlBuilder.LeftJoin("proc_resource pr ON T.ResourceId = pr.Id");
            sqlBuilder.LeftJoin("equ_equipment ee ON T.EquipmentId = ee.Id");
            sqlBuilder.LeftJoin("plan_work_order pwo ON T.WorkOrderId = pwo.Id");
            sqlBuilder.LeftJoin("inte_work_center iwc ON T.WorkCenterId = iwc.Id");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");
            sqlBuilder.OrderBy("T.CreatedOn DESC");
            sqlBuilder.Select("T.*, qiihr.InspectionBy, qiihr.InspectionOn, qiihr.StartOn, qiihr.CompleteOn, qiihr.CloseOn, qiihr.HandMethod, qiihr.ProcessedBy, qiihr.ProcessedOn, qii.GenerateCondition, qii.GenerateConditionUnit, pm.MaterialCode, pm.MaterialName, pp.Code ProcedureCode, pp.Name ProcedureName, pr.ResourceCode, pr.ResourceName, ee.EquipmentCode, ee.EquipmentName, pwo.OrderCode WorkOrderCode, iwc.Code WorkCenterCode");

            if (!string.IsNullOrWhiteSpace(pagedQuery.InspectionOrder))
            {
                pagedQuery.InspectionOrder = $"%{pagedQuery.InspectionOrder}%";
                sqlBuilder.Where("T.InspectionOrder LIKE @InspectionOrder");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where("pm.MaterialCode LIKE @MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialName))
            {
                pagedQuery.MaterialName = $"%{pagedQuery.MaterialName}%";
                sqlBuilder.Where("pm.MaterialName LIKE @MaterialName");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureCode))
            {
                pagedQuery.ProcedureCode = $"%{pagedQuery.ProcedureCode}%";
                sqlBuilder.Where("pp.Code LIKE @ProcedureCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureName))
            {
                pagedQuery.ProcedureName = $"%{pagedQuery.ProcedureName}%";
                sqlBuilder.Where("pp.Name LIKE @ProcedureName");
            }
            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("T.Status = @Status");
            }
            if (pagedQuery.IsQualified.HasValue)
            {
                sqlBuilder.Where("T.IsQualified = @IsQualified");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualIpqcInspectionHeadView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIpqcInspectionHeadView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIpqcInspectionHeadRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_ipqc_inspection_head T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_ipqc_inspection_head T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM qual_ipqc_inspection_head /**where**/  ";

        const string InsertSql = "INSERT INTO qual_ipqc_inspection_head(  `Id`, `SiteId`, `InspectionOrder`, `IpqcInspectionId`, `WorkOrderId`, `MaterialId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `TriggerCondition`, `IsStop`, `ControlTime`, `ControlTimeUnit`, `SampleQty`, `IsQualified`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @IpqcInspectionId, @WorkOrderId, @MaterialId, @ProcedureId, @ResourceId, @EquipmentId, @TriggerCondition, @IsStop, @ControlTime, @ControlTimeUnit, @SampleQty, @IsQualified, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_ipqc_inspection_head(  `Id`, `SiteId`, `InspectionOrder`, `IpqcInspectionId`, `WorkOrderId`, `MaterialId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `TriggerCondition`, `IsStop`, `ControlTime`, `ControlTimeUnit`, `SampleQty`, `IsQualified`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @IpqcInspectionId, @WorkOrderId, @MaterialId, @ProcedureId, @ResourceId, @EquipmentId, @TriggerCondition, @IsStop, @ControlTime, @ControlTimeUnit, @SampleQty, @IsQualified, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_ipqc_inspection_head SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, IpqcInspectionId = @IpqcInspectionId, WorkOrderId = @WorkOrderId, MaterialId = @MaterialId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, TriggerCondition = @TriggerCondition, IsStop = @IsStop, ControlTime = @ControlTime, ControlTimeUnit = @ControlTimeUnit, SampleQty = @SampleQty, IsQualified = @IsQualified, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_ipqc_inspection_head SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, IpqcInspectionId = @IpqcInspectionId, WorkOrderId = @WorkOrderId, MaterialId = @MaterialId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, TriggerCondition = @TriggerCondition, IsStop = @IsStop, ControlTime = @ControlTime, ControlTimeUnit = @ControlTimeUnit, SampleQty = @SampleQty, IsQualified = @IsQualified, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_ipqc_inspection_head SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_ipqc_inspection_head SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_ipqc_inspection_head WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_ipqc_inspection_head WHERE Id IN @Ids ";

    }
}
