using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Quality.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（全检参数项目表）
    /// </summary>
    public partial class QualInspectionParameterGroupDetailRepository : BaseRepository, IQualInspectionParameterGroupDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualInspectionParameterGroupDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualInspectionParameterGroupDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByParentId, command);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualInspectionParameterGroupDetailEntity>> GetEntitiesAsync(QualInspectionParameterGroupDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("ParameterGroupId = @ParameterGroupId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualInspectionParameterGroupDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualInspectionParameterGroupDetailView>> GetPagedListAsync(QualInspectionParameterGroupDetailPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("proc_parameter pp ON T.ParameterId = pp.Id");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.ParameterGroupId = @ParameterGroupId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.ParameterCode))
            {
                pagedQuery.ParameterCode = $"%{pagedQuery.ParameterCode}%";
                sqlBuilder.Where("pp.ParameterCode LIKE @ParameterCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ParameterName))
            {
                pagedQuery.ParameterName = $"%{pagedQuery.ParameterName}%";
                sqlBuilder.Where("pp.ParameterName LIKE @ParameterName");
            }

            sqlBuilder.Select("T.*, pp.ParameterCode, pp.ParameterName, pp.ParameterUnit, pp.DataType");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualInspectionParameterGroupDetailView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualInspectionParameterGroupDetailView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualInspectionParameterGroupDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_inspection_parameter_group_detail` T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `qual_inspection_parameter_group_detail` T /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_inspection_parameter_group_detail` /**where**/  ";

        const string InsertsSql = "INSERT INTO `qual_inspection_parameter_group_detail`(`Id`, `ParameterGroupId`, `ParameterId`, `UpperLimit`, `CenterValue`, `LowerLimit`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @ParameterGroupId, @ParameterId, @UpperLimit, @CenterValue, @LowerLimit, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string DeleteByParentId = "DELETE FROM qual_inspection_parameter_group_detail WHERE ParameterGroupId = @ParentId";

    }
}
