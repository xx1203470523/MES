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
    /// 仓储（FQC检验参数组明细快照）
    /// </summary>
    public partial class QualFqcParameterGroupDetailSnapshootRepository : BaseRepository, IQualFqcParameterGroupDetailSnapshootRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualFqcParameterGroupDetailSnapshootRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcParameterGroupDetailSnapshootEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualFqcParameterGroupDetailSnapshootEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcParameterGroupDetailSnapshootEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualFqcParameterGroupDetailSnapshootEntity> entities)
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
        public async Task<QualFqcParameterGroupDetailSnapshootEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcParameterGroupDetailSnapshootEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupDetailSnapshootEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcParameterGroupDetailSnapshootEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupDetailSnapshootEntity>> GetEntitiesAsync(QualFqcParameterGroupDetailSnapshootQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.ParameterGroupId.HasValue)
            {
                sqlBuilder.Where("ParameterGroupId = @ParameterGroupId");
            }
            if (!string.IsNullOrWhiteSpace(query.ParameterCode))
            {
                sqlBuilder.Where("ParameterCode = @ParameterCode");
            }
            sqlBuilder.AddParameters(query);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcParameterGroupDetailSnapshootEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupDetailSnapshootEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcParameterGroupDetailSnapshootEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcParameterGroupDetailSnapshootEntity>> GetPagedListAsync(QualFqcParameterGroupDetailSnapshootPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualFqcParameterGroupDetailSnapshootEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcParameterGroupDetailSnapshootEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualFqcParameterGroupDetailSnapshootRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_parameter_group_detail_snapshoot /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_fqc_parameter_group_detail_snapshoot /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_parameter_group_detail_snapshoot /**where**/  ";

        const string InsertSql = "INSERT INTO qual_fqc_parameter_group_detail_snapshoot(  `Id`, `SiteId`, `ParameterGroupId`, `ParameterId`, `ParameterCode`, `ParameterName`, `ParameterUnit`, `ParameterDataType`, `UpperLimit`, `CenterValue`, `LowerLimit`, `ReferenceValue`, `EnterNumber`, `IsDeviceCollect`, `DisplayOrder`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ParameterGroupId, @ParameterId, @ParameterCode, @ParameterName, @ParameterUnit, @ParameterDataType, @UpperLimit, @CenterValue, @LowerLimit, @ReferenceValue, @EnterNumber, @IsDeviceCollect, @DisplayOrder, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_fqc_parameter_group_detail_snapshoot(  `Id`, `SiteId`, `ParameterGroupId`, `ParameterId`, `ParameterCode`, `ParameterName`, `ParameterUnit`, `ParameterDataType`, `UpperLimit`, `CenterValue`, `LowerLimit`, `ReferenceValue`, `EnterNumber`, `IsDeviceCollect`, `DisplayOrder`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @ParameterGroupId, @ParameterId, @ParameterCode, @ParameterName, @ParameterUnit, @ParameterDataType, @UpperLimit, @CenterValue, @LowerLimit, @ReferenceValue, @EnterNumber, @IsDeviceCollect, @DisplayOrder, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_fqc_parameter_group_detail_snapshoot SET   SiteId = @SiteId, ParameterGroupId = @ParameterGroupId, ParameterId = @ParameterId, ParameterCode = @ParameterCode, ParameterName = @ParameterName, ParameterUnit = @ParameterUnit, ParameterDataType = @ParameterDataType, UpperLimit = @UpperLimit, CenterValue = @CenterValue, LowerLimit = @LowerLimit, ReferenceValue = @ReferenceValue, EnterNumber = @EnterNumber, IsDeviceCollect = @IsDeviceCollect, DisplayOrder = @DisplayOrder, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_fqc_parameter_group_detail_snapshoot SET   SiteId = @SiteId, ParameterGroupId = @ParameterGroupId, ParameterId = @ParameterId, ParameterCode = @ParameterCode, ParameterName = @ParameterName, ParameterUnit = @ParameterUnit, ParameterDataType = @ParameterDataType, UpperLimit = @UpperLimit, CenterValue = @CenterValue, LowerLimit = @LowerLimit, ReferenceValue = @ReferenceValue, EnterNumber = @EnterNumber, IsDeviceCollect = @IsDeviceCollect, DisplayOrder = @DisplayOrder, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_fqc_parameter_group_detail_snapshoot SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_fqc_parameter_group_detail_snapshoot SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_fqc_parameter_group_detail_snapshoot WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_fqc_parameter_group_detail_snapshoot WHERE Id IN @Ids ";

    }
}
