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
    /// 仓储（IPQC检验项目参数）
    /// </summary>
    public partial class QualIpqcInspectionParameterRepository : BaseRepository, IQualIpqcInspectionParameterRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIpqcInspectionParameterRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIpqcInspectionParameterEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIpqcInspectionParameterEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIpqcInspectionParameterEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIpqcInspectionParameterEntity> entities)
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
        /// 软删除（根据主表Id删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteByMainIdAsync(DeleteByParentIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByMainIdSql, command);
        }

        /// <summary>
        /// 物理删除（根据主表Id删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteReallyByMainIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteReallyByMainIdSql, new { MainId = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualIpqcInspectionParameterEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIpqcInspectionParameterEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionParameterEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionParameterEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionParameterEntity>> GetEntitiesAsync(QualIpqcInspectionParameterQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("IpqcInspectionId = @IpqcInspectionId");
            sqlBuilder.Select("*");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIpqcInspectionParameterEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionParameterEntity>> GetPagedListAsync(QualIpqcInspectionParameterPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualIpqcInspectionParameterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualIpqcInspectionParameterEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIpqcInspectionParameterRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_ipqc_inspection_parameter /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_ipqc_inspection_parameter /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM qual_ipqc_inspection_parameter /**where**/  ";

        const string InsertSql = "INSERT INTO qual_ipqc_inspection_parameter(  `Id`, `IpqcInspectionId`, `InspectionParameterGroupDetailId`, `ParameterId`, `UpperLimit`, `LowerLimit`, `CenterValue`, `ReferenceValue`, `EnterNumber`, `IsDeviceCollect`, `Sequence`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @IpqcInspectionId, @InspectionParameterGroupDetailId, @ParameterId, @UpperLimit, @LowerLimit, @CenterValue, @ReferenceValue, @EnterNumber, @IsDeviceCollect, @Sequence, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_ipqc_inspection_parameter(  `Id`, `IpqcInspectionId`, `InspectionParameterGroupDetailId`, `ParameterId`, `UpperLimit`, `LowerLimit`, `CenterValue`, `ReferenceValue`, `EnterNumber`, `IsDeviceCollect`, `Sequence`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @IpqcInspectionId, @InspectionParameterGroupDetailId, @ParameterId, @UpperLimit, @LowerLimit, @CenterValue, @ReferenceValue, @EnterNumber, @IsDeviceCollect, @Sequence, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_ipqc_inspection_parameter SET   IpqcInspectionId = @IpqcInspectionId, InspectionParameterGroupDetailId = @InspectionParameterGroupDetailId, ParameterId = @ParameterId, UpperLimit = @UpperLimit, LowerLimit = @LowerLimit, CenterValue = @CenterValue, ReferenceValue = @ReferenceValue, EnterNumber = @EnterNumber, IsDeviceCollect = @IsDeviceCollect, Sequence = @Sequence, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_ipqc_inspection_parameter SET   IpqcInspectionId = @IpqcInspectionId, InspectionParameterGroupDetailId = @InspectionParameterGroupDetailId, ParameterId = @ParameterId, UpperLimit = @UpperLimit, LowerLimit = @LowerLimit, CenterValue = @CenterValue, ReferenceValue = @ReferenceValue, EnterNumber = @EnterNumber, IsDeviceCollect = @IsDeviceCollect, Sequence = @Sequence, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_ipqc_inspection_parameter SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_ipqc_inspection_parameter SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeleteByMainIdSql = "UPDATE qual_ipqc_inspection_parameter SET IsDeleted = Id, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IpqcInspectionId = @ParentId";
        const string DeleteReallyByMainIdSql = "DELETE FROM qual_ipqc_inspection_parameter WHERE IpqcInspectionId = @MainId";

        const string GetByIdSql = @"SELECT * FROM qual_ipqc_inspection_parameter WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_ipqc_inspection_parameter WHERE Id IN @Ids ";

    }
}
