using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.QualIpqcInspection.View;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（IPQC检验项目）
    /// </summary>
    public partial class QualIpqcInspectionRepository : BaseRepository, IQualIpqcInspectionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIpqcInspectionRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIpqcInspectionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIpqcInspectionEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIpqcInspectionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIpqcInspectionEntity> entities)
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
        public async Task<QualIpqcInspectionEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIpqcInspectionEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionEntity>> GetEntitiesAsync(QualIpqcInspectionQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("T.*");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");
            sqlBuilder.Where("T.Type = @Type");
            if (query.MaterialId.HasValue)
            {
                sqlBuilder.Where("T.MaterialId = @MaterialId");
            }
            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("T.ProcedureId = @ProcedureId");
            }
            if (query.GenerateConditionUnit.HasValue)
            {
                sqlBuilder.Where("T.GenerateConditionUnit = @GenerateConditionUnit");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("T.Status = @Status");
            }
            using var conn = GetMESDbConnection();
                return await conn.QueryAsync<QualIpqcInspectionEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionView>> GetPagedListAsync(QualIpqcInspectionPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("qii.IsDeleted = 0");
            sqlBuilder.Where("qii.SiteId = @SiteId");
            sqlBuilder.OrderBy("qii.UpdatedOn DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.ParameterGroupCode))
            {
                pagedQuery.ParameterGroupCode = $"%{pagedQuery.ParameterGroupCode}%";
                sqlBuilder.Where("qipg.Code LIKE @ParameterGroupCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ParameterGroupName))
            {
                pagedQuery.ParameterGroupName = $"%{pagedQuery.ParameterGroupName}%";
                sqlBuilder.Where("qipg.Name LIKE @ParameterGroupName");
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
                sqlBuilder.Where("qii.Status = @Status");
            }
            if (pagedQuery.Type.HasValue)
            {
                sqlBuilder.Where("qii.Type = @Type");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualIpqcInspectionView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIpqcInspectionView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 是否已存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(QualIpqcInspectionQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(IsExistSqlTemplate);
            sqlBuilder.Where("qii.IsDeleted = 0");
            sqlBuilder.Where("qii.SiteId = @SiteId");
            sqlBuilder.Where("qii.Type = @Type");
            sqlBuilder.Where("qii.GenerateConditionUnit = @GenerateConditionUnit");
            sqlBuilder.Where("qii.Version = @Version");
            sqlBuilder.Where("qipg.Code = @ParameterGroupCode");
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var totalCount = await conn.ExecuteScalarAsync<int>(template.RawSql, template.Parameters);
            return totalCount > 0;
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
    public partial class QualIpqcInspectionRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"
            SELECT qii.Id, qii.SiteId, qii.InspectionParameterGroupId, qipg.`Code` AS ParameterGroupCode, qipg.`Name` AS ParameterGroupName, qipg.Version AS ParameterGroupVersion,
	            qipg.`Status` AS ParameterGroupStatus, qipg.MaterialId, pm.MaterialCode, pm.MaterialName, qipg.ProcedureId, pp.`Code` AS ProcedureCode, pp.`Name` AS ProcedureName,
	            qii.GenerateCondition, qii.GenerateConditionUnit, qii.ControlTime, qii.ControlTimeUnit, qii.Type, qii.SampleQty, qii.Version, qii.`Status`, qii.Remark,
	            qii.CreatedBy, qii.CreatedOn, qii.UpdatedBy, qii.UpdatedOn, qii.IsDeleted 
            FROM qual_ipqc_inspection qii
	        INNER JOIN qual_inspection_parameter_group qipg ON qii.InspectionParameterGroupId = qipg.Id
	        LEFT JOIN proc_material pm ON qipg.MaterialId = pm.Id
	        LEFT JOIN proc_procedure pp ON qipg.ProcedureId = pp.Id 
            /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"
            SELECT COUNT(*)
            FROM qual_ipqc_inspection qii
	        INNER JOIN qual_inspection_parameter_group qipg ON qii.InspectionParameterGroupId = qipg.Id
	        LEFT JOIN proc_material pm ON qipg.MaterialId = pm.Id
	        LEFT JOIN proc_procedure pp ON qipg.ProcedureId = pp.Id 
            /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM qual_ipqc_inspection T /**where**/  ";

        const string IsExistSqlTemplate = "SELECT COUNT(1) FROM qual_ipqc_inspection qii LEFT JOIN qual_inspection_parameter_group qipg ON qii.InspectionParameterGroupId = qipg.Id /**where**/ ";

        const string InsertSql = "INSERT INTO qual_ipqc_inspection(  `Id`, `SiteId`, `Type`, `SampleQty`, `InspectionParameterGroupId`, `GenerateCondition`, `GenerateConditionUnit`, `ControlTime`, `ControlTimeUnit`, `MaterialId`, `ProcedureId`, `Version`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Type, @SampleQty, @InspectionParameterGroupId, @GenerateCondition, @GenerateConditionUnit, @ControlTime, @ControlTimeUnit, @MaterialId, @ProcedureId, @Version, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_ipqc_inspection(  `Id`, `SiteId`, `Type`, `SampleQty`, `InspectionParameterGroupId`, `GenerateCondition`, `GenerateConditionUnit`, `ControlTime`, `ControlTimeUnit`, `MaterialId`, `ProcedureId`, `Version`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Type, @SampleQty, @InspectionParameterGroupId, @GenerateCondition, @GenerateConditionUnit, @ControlTime, @ControlTimeUnit, @MaterialId, @ProcedureId, @Version, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_ipqc_inspection SET   SiteId = @SiteId, Type = @Type, SampleQty = @SampleQty, InspectionParameterGroupId = @InspectionParameterGroupId, GenerateCondition = @GenerateCondition, GenerateConditionUnit = @GenerateConditionUnit, ControlTime = @ControlTime, ControlTimeUnit = @ControlTimeUnit, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Version = @Version, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_ipqc_inspection SET   SiteId = @SiteId, Type = @Type, SampleQty = @SampleQty, InspectionParameterGroupId = @InspectionParameterGroupId, GenerateCondition = @GenerateCondition, GenerateConditionUnit = @GenerateConditionUnit, ControlTime = @ControlTime, ControlTimeUnit = @ControlTimeUnit, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Version = @Version, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_ipqc_inspection SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_ipqc_inspection SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_ipqc_inspection WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_ipqc_inspection WHERE Id IN @Ids ";

        const string UpdateStatusSql = "UPDATE `qual_ipqc_inspection` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

    }
}
