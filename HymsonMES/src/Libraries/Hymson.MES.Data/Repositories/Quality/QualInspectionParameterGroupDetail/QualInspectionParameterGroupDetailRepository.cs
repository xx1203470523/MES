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
            var template = sqlBuilder.AddTemplate(GetQualInspectionParameterGroupDetailEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("ParameterGroupId = @ParameterGroupId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualInspectionParameterGroupDetailEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualInspectionParameterGroupDetailRepository
    {
       const string GetQualInspectionParameterGroupDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_inspection_parameter_group_detail` /**where**/  ";

        const string InsertsSql = "INSERT INTO `qual_inspection_parameter_group_detail`(`Id`, `ParameterGroupId`, `ParameterId`, `UpperLimit`, `CenterValue`, `LowerLimit`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @ParameterGroupId, @ParameterId, @UpperLimit, @CenterValue, @LowerLimit, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string DeleteByParentId = "DELETE FROM qual_inspection_parameter_group_detail WHERE ParameterGroupId = @ParentId";

    }
}
