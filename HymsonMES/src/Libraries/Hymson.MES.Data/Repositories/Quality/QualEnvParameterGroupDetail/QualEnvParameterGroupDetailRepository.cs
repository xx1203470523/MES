using Dapper;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（环境检验参数项目表）
    /// </summary>
    public partial class QualEnvParameterGroupDetailRepository : BaseRepository, IQualEnvParameterGroupDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualEnvParameterGroupDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualEnvParameterGroupDetailEntity> entities)
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
        public async Task<IEnumerable<QualEnvParameterGroupDetailEntity>> GetEntitiesAsync(QualEnvParameterGroupDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("ParameterGroupId = @ParameterGroupId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualEnvParameterGroupDetailEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualEnvParameterGroupDetailRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_env_parameter_group_detail` /**where**/  ";
        
        const string InsertsSql = "INSERT INTO `qual_env_parameter_group_detail`(`Id`, `ParameterGroupId`, `ParameterId`, `UpperLimit`, `CenterValue`, `LowerLimit`, `Frequency`, `EntryCount`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @ParameterGroupId, @ParameterId, @UpperLimit, @CenterValue, @LowerLimit, @Frequency, @EntryCount, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string DeleteByParentId = "DELETE FROM qual_env_parameter_group_detail WHERE ParameterGroupId = @ParentId";


    }
}
