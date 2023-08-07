using Dapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（事件类型推送规则）
    /// </summary>
    public partial class InteEventTypePushRuleRepository : BaseRepository, IInteEventTypePushRuleRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteEventTypePushRuleRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteEventTypePushRuleEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
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
        public async Task<IEnumerable<InteEventTypePushRuleEntity>> GetEntitiesAsync(EntityByParentIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("EventTypeId IN @ParentId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteEventTypePushRuleEntity>(template.RawSql, query);
        }


    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteEventTypePushRuleRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_event_type_push_rule /**where**/  ";

        const string InsertSql = "INSERT INTO inte_event_type_push_rule(`Id`, `SiteId`, `EventTypeId`, `PushScene`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES ( @Id, @SiteId, @EventTypeId, @PushScene, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string DeleteByParentId = "DELETE FROM inte_event_type_push_rule WHERE EventTypeId = @ParentId";

    }
}
