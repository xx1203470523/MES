using Dapper;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（产品检验参数项目表）
    /// </summary>
    public partial class ProcProductParameterGroupDetailRepository : BaseRepository, IProcProductParameterGroupDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcProductParameterGroupDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcProductParameterGroupDetailEntity> entities)
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
        public async Task<IEnumerable<ProcProductParameterGroupDetailEntity>> GetEntitiesAsync(ProcProductParameterGroupDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");

            if (query.ParameterGroupId.HasValue)
            {
                sqlBuilder.Where("ParameterGroupId = @ParameterGroupId");
            }

            if (query.ParameterGroupIds != null && query.ParameterGroupIds.Any())
            {
                sqlBuilder.Where("ParameterGroupId in @ParameterGroupIds");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProductParameterGroupDetailEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcProductParameterGroupDetailRepository
    {
       const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_product_parameter_group_detail` /**where**/  ORDER BY Sort";

        const string InsertsSql = "INSERT INTO `proc_product_parameter_group_detail`(`Id`, `ParameterGroupId`, `ParameterId`, `UpperLimit`, `CenterValue`, `LowerLimit`, IsRequired, Sort, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @ParameterGroupId, @ParameterId, @UpperLimit, @CenterValue, @LowerLimit, @IsRequired, @Sort, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string DeleteByParentId = "DELETE FROM proc_product_parameter_group_detail WHERE ParameterGroupId = @ParentId";

    }
}
