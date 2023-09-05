using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspectionTail.View;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（尾检检验单）
    /// </summary>
    public partial class QualIpqcInspectionTailRepository : BaseRepository, IQualIpqcInspectionTailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIpqcInspectionTailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIpqcInspectionTailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIpqcInspectionTailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIpqcInspectionTailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIpqcInspectionTailEntity> entities)
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
        public async Task<QualIpqcInspectionTailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIpqcInspectionTailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionTailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionTailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionTailEntity>> GetEntitiesAsync(QualIpqcInspectionTailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionTailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionTailView>> GetPagedListAsync(QualIpqcInspectionTailPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.LeftJoin("qual_ipqc_inspection qii ON T.IpqcInspectionId = qii.Id");
            sqlBuilder.LeftJoin("proc_material pm ON T.MaterialId = pm.Id");
            sqlBuilder.LeftJoin("proc_procedure pp ON T.ProcedureId = pp.Id");
            sqlBuilder.LeftJoin("proc_resource pr ON T.ResourceId = pr.Id");
            sqlBuilder.LeftJoin("equ_equipment ee ON T.EquipmentId = ee.Id");
            sqlBuilder.LeftJoin("plan_work_order pwo ON T.WorkOrderId = pwo.Id");
            sqlBuilder.LeftJoin("inte_work_center iwc ON pwo.WorkCenterId = iwc.Id");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");
            sqlBuilder.OrderBy("T.CreatedOn DESC");
            sqlBuilder.Select("T.*, qii.GenerateCondition, qii.GenerateConditionUnit, qii.ControlTime, qii.ControlTimeUnit, pm.MaterialCode, pm.MaterialName, pp.Code ProcedureCode, pp.Name ProcedureName, pr. ResCode ResourceCode, pr.ResName ResourceName, ee.EquipmentCode, ee.EquipmentName, pwo.OrderCode WorkOrderCode, iwc.Code WorkCenterCode");

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

            if (pagedQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where("T.ProcedureId = @ProcedureId");
            }

            if (pagedQuery.MaterialId.HasValue)
            {
                sqlBuilder.Where("T.MaterialId = @MaterialId");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualIpqcInspectionTailView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIpqcInspectionTailView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIpqcInspectionTailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_ipqc_inspection_tail T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_ipqc_inspection_tail T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM qual_ipqc_inspection_tail /**where**/  ";

        const string InsertSql = "INSERT INTO qual_ipqc_inspection_tail(  `Id`, `SiteId`, `InspectionOrder`, `IpqcInspectionId`, `WorkOrderId`, `MaterialId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SampleQty`, `IsQualified`, `Status`, `InspectionBy`, `InspectionOn`, `StartOn`, `CompleteOn`, `CloseOn`, `HandMethod`, `ProcessedBy`, `ProcessedOn`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @IpqcInspectionId, @WorkOrderId, @MaterialId, @ProcedureId, @ResourceId, @EquipmentId, @SampleQty, @IsQualified, @Status, @InspectionBy, @InspectionOn, @StartOn, @CompleteOn, @CloseOn, @HandMethod, @ProcessedBy, @ProcessedOn, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_ipqc_inspection_tail(  `Id`, `SiteId`, `InspectionOrder`, `IpqcInspectionId`, `WorkOrderId`, `MaterialId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `SampleQty`, `IsQualified`, `Status`, `InspectionBy`, `InspectionOn`, `StartOn`, `CompleteOn`, `CloseOn`, `HandMethod`, `ProcessedBy`, `ProcessedOn`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @InspectionOrder, @IpqcInspectionId, @WorkOrderId, @MaterialId, @ProcedureId, @ResourceId, @EquipmentId, @SampleQty, @IsQualified, @Status, @InspectionBy, @InspectionOn, @StartOn, @CompleteOn, @CloseOn, @HandMethod, @ProcessedBy, @ProcessedOn, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_ipqc_inspection_tail SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, IpqcInspectionId = @IpqcInspectionId, WorkOrderId = @WorkOrderId, MaterialId = @MaterialId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SampleQty = @SampleQty, IsQualified = @IsQualified, Status = @Status, InspectionBy = @InspectionBy, InspectionOn = @InspectionOn, StartOn = @StartOn, CompleteOn = @CompleteOn, CloseOn = @CloseOn, HandMethod = @HandMethod, ProcessedBy = @ProcessedBy, ProcessedOn = @ProcessedOn, ExecuteBy = @ExecuteBy, ExecuteOn = @ExecuteOn, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_ipqc_inspection_tail SET   SiteId = @SiteId, InspectionOrder = @InspectionOrder, IpqcInspectionId = @IpqcInspectionId, WorkOrderId = @WorkOrderId, MaterialId = @MaterialId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, SampleQty = @SampleQty, IsQualified = @IsQualified, Status = @Status, InspectionBy = @InspectionBy, InspectionOn = @InspectionOn, StartOn = @StartOn, CompleteOn = @CompleteOn, CloseOn = @CloseOn, HandMethod = @HandMethod, ProcessedBy = @ProcessedBy, ProcessedOn = @ProcessedOn, ExecuteBy = @ExecuteBy, ExecuteOn = @ExecuteOn, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_ipqc_inspection_tail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_ipqc_inspection_tail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_ipqc_inspection_tail WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_ipqc_inspection_tail WHERE Id IN @Ids ";

    }
}
