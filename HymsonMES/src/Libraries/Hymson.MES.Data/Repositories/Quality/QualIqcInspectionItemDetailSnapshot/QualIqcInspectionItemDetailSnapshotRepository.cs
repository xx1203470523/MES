using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（IQC检验项目详情快照表）
    /// </summary>
    public partial class QualIqcInspectionItemDetailSnapshotRepository : BaseRepository, IQualIqcInspectionItemDetailSnapshotRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIqcInspectionItemDetailSnapshotRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIqcInspectionItemDetailSnapshotEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIqcInspectionItemDetailSnapshotEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIqcInspectionItemDetailSnapshotEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIqcInspectionItemDetailSnapshotEntity> entities)
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
        public async Task<QualIqcInspectionItemDetailSnapshotEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcInspectionItemDetailSnapshotEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcInspectionItemDetailSnapshotEntity>> GetBySnapshotIdAsync(long snapshotId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcInspectionItemDetailSnapshotEntity>(GetBySnapshotIdSql, new { IqcInspectionItemSnapshotId = snapshotId });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcInspectionItemDetailSnapshotEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcInspectionItemDetailSnapshotEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcInspectionItemDetailSnapshotEntity>> GetEntitiesAsync(QualIqcInspectionItemDetailSnapshotQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.IqcInspectionItemSnapshotId.HasValue)
            {
                sqlBuilder.Where("IqcInspectionItemSnapshotId = @IqcInspectionItemSnapshotId");
            }
            if (query.InspectionType.HasValue)
            {
                sqlBuilder.Where("InspectionType = @InspectionType");
            }
            if (!string.IsNullOrWhiteSpace(query.ParameterCode))
            {
                sqlBuilder.Where("ParameterCode = @ParameterCode");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcInspectionItemDetailSnapshotEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcInspectionItemDetailSnapshotEntity>> GetPagedListAsync(QualIqcInspectionItemDetailSnapshotPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualIqcInspectionItemDetailSnapshotEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIqcInspectionItemDetailSnapshotEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIqcInspectionItemDetailSnapshotRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_inspection_item_detail_snapshot /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_iqc_inspection_item_detail_snapshot /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_inspection_item_detail_snapshot /**where**/  ";

        const string InsertSql = "INSERT INTO qual_iqc_inspection_item_detail_snapshot(  `Id`, `IqcInspectionItemSnapshotId`, `ParameterId`, `ParameterCode`, `ParameterName`, `ParameterDataType`, `ParameterUnit`, `Type`, `Utensil`, `Scale`, `LowerLimit`, `Center`, `UpperLimit`, `InspectionType`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @IqcInspectionItemSnapshotId, @ParameterId, @ParameterCode, @ParameterName, @ParameterDataType, @ParameterUnit, @Type, @Utensil, @Scale, @LowerLimit, @Center, @UpperLimit, @InspectionType, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_iqc_inspection_item_detail_snapshot(  `Id`, `IqcInspectionItemSnapshotId`, `ParameterId`, `ParameterCode`, `ParameterName`, `ParameterDataType`, `ParameterUnit`, `Type`, `Utensil`, `Scale`, `LowerLimit`, `Center`, `UpperLimit`, `InspectionType`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @IqcInspectionItemSnapshotId, @ParameterId, @ParameterCode, @ParameterName, @ParameterDataType, @ParameterUnit, @Type, @Utensil, @Scale, @LowerLimit, @Center, @UpperLimit, @InspectionType, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_iqc_inspection_item_detail_snapshot SET   IqcInspectionItemSnapshotId = @IqcInspectionItemSnapshotId, ParameterId = @ParameterId, ParameterCode = @ParameterCode, ParameterName = @ParameterName, ParameterDataType = @ParameterDataType, ParameterUnit = @ParameterUnit, Type = @Type, Utensil = @Utensil, Scale = @Scale, LowerLimit = @LowerLimit, Center = @Center, UpperLimit = @UpperLimit, InspectionType = @InspectionType, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_iqc_inspection_item_detail_snapshot SET   IqcInspectionItemSnapshotId = @IqcInspectionItemSnapshotId, ParameterId = @ParameterId, ParameterCode = @ParameterCode, ParameterName = @ParameterName, ParameterDataType = @ParameterDataType, ParameterUnit = @ParameterUnit, Type = @Type, Utensil = @Utensil, Scale = @Scale, LowerLimit = @LowerLimit, Center = @Center, UpperLimit = @UpperLimit, InspectionType = @InspectionType, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_iqc_inspection_item_detail_snapshot SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_iqc_inspection_item_detail_snapshot SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_iqc_inspection_item_detail_snapshot WHERE Id = @Id ";
        const string GetBySnapshotIdSql = @"SELECT * FROM qual_iqc_inspection_item_detail_snapshot WHERE IqcInspectionItemSnapshotId = @IqcInspectionItemSnapshotId ";
        const string GetByIdsSql = @"SELECT * FROM qual_iqc_inspection_item_detail_snapshot WHERE Id IN @Ids ";

    }
}
