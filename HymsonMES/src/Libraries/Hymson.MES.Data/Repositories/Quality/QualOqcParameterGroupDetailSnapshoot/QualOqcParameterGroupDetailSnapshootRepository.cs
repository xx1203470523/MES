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
    /// 仓储（OQC检验参数组明细快照）
    /// </summary>
    public partial class QualOqcParameterGroupDetailSnapshootRepository : BaseRepository, IQualOqcParameterGroupDetailSnapshootRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualOqcParameterGroupDetailSnapshootRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualOqcParameterGroupDetailSnapshootEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualOqcParameterGroupDetailSnapshootEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualOqcParameterGroupDetailSnapshootEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualOqcParameterGroupDetailSnapshootEntity> entities)
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
        public async Task<QualOqcParameterGroupDetailSnapshootEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualOqcParameterGroupDetailSnapshootEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcParameterGroupDetailSnapshootEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcParameterGroupDetailSnapshootEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcParameterGroupDetailSnapshootEntity>> GetEntitiesAsync(QualOqcParameterGroupDetailSnapshootQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(query.ParameterCode)) {
                sqlBuilder.Where("ParameterCode=@ParameterCode");
            }

            if (query.ParameterGroupId != null)
            {
                sqlBuilder.Where("ParameterGroupId=@ParameterGroupId");
            }

            if (query.InspectionType != null) {
                sqlBuilder.Where("InspectionType=@InspectionType");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualOqcParameterGroupDetailSnapshootEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcParameterGroupDetailSnapshootEntity>> GetPagedListAsync(QualOqcParameterGroupDetailSnapshootPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualOqcParameterGroupDetailSnapshootEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualOqcParameterGroupDetailSnapshootEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualOqcParameterGroupDetailSnapshootRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_parameter_group_detail_snapshoot /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_oqc_parameter_group_detail_snapshoot /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_oqc_parameter_group_detail_snapshoot /**where**/  ";

        const string InsertSql = "INSERT INTO qual_oqc_parameter_group_detail_snapshoot(  `Id`, `SiteId`, `ParameterGroupId`, `ParameterId`, `ParameterCode`, `ParameterName`, `ParameterUnit`, `ParameterDataType`, `UpperLimit`, `CenterValue`, `LowerLimit`, `ReferenceValue`, `InspectionType`, `EnterNumber`, `DisplayOrder`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ParameterGroupId, @ParameterId, @ParameterCode, @ParameterName, @ParameterUnit, @ParameterDataType, @UpperLimit, @CenterValue, @LowerLimit, @ReferenceValue, @InspectionType, @EnterNumber, @DisplayOrder, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_oqc_parameter_group_detail_snapshoot(  `Id`, `SiteId`, `ParameterGroupId`, `ParameterId`, `ParameterCode`, `ParameterName`, `ParameterUnit`, `ParameterDataType`, `UpperLimit`, `CenterValue`, `LowerLimit`, `ReferenceValue`, `InspectionType`, `EnterNumber`, `DisplayOrder`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ParameterGroupId, @ParameterId, @ParameterCode, @ParameterName, @ParameterUnit, @ParameterDataType, @UpperLimit, @CenterValue, @LowerLimit, @ReferenceValue, @InspectionType, @EnterNumber, @DisplayOrder, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_oqc_parameter_group_detail_snapshoot SET   SiteId = @SiteId, ParameterGroupId = @ParameterGroupId, ParameterId = @ParameterId, ParameterCode = @ParameterCode, ParameterName = @ParameterName, ParameterUnit = @ParameterUnit, ParameterDataType = @ParameterDataType, UpperLimit = @UpperLimit, CenterValue = @CenterValue, LowerLimit = @LowerLimit, ReferenceValue = @ReferenceValue, InspectionType = @InspectionType, EnterNumber = @EnterNumber, DisplayOrder = @DisplayOrder, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_oqc_parameter_group_detail_snapshoot SET   SiteId = @SiteId, ParameterGroupId = @ParameterGroupId, ParameterId = @ParameterId, ParameterCode = @ParameterCode, ParameterName = @ParameterName, ParameterUnit = @ParameterUnit, ParameterDataType = @ParameterDataType, UpperLimit = @UpperLimit, CenterValue = @CenterValue, LowerLimit = @LowerLimit, ReferenceValue = @ReferenceValue, InspectionType = @InspectionType, EnterNumber = @EnterNumber, DisplayOrder = @DisplayOrder, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_oqc_parameter_group_detail_snapshoot SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_oqc_parameter_group_detail_snapshoot SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_oqc_parameter_group_detail_snapshoot WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_oqc_parameter_group_detail_snapshoot WHERE Id IN @Ids ";

    }
}
