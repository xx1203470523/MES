using Dapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（事件类型关联群组）
    /// </summary>
    public partial class InteEventTypeMessageGroupRelationRepository : BaseRepository, IInteEventTypeMessageGroupRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public InteEventTypeMessageGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteEventTypeMessageGroupRelationEntity> entities)
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
        public async Task<IEnumerable<InteEventTypeMessageGroupRelationEntity>> GetEntitiesAsync(EntityByParentIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("EventTypeId = @ParentId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteEventTypeMessageGroupRelationEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteEventTypeMessageGroupRelationRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_event_type_message_group_relation /**where**/  ";

        const string InsertsSql = "INSERT INTO inte_event_type_message_group_relation(  `Id`, `SiteId`, `EventTypeId`, `MessageGroupId`, `PushTypes`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @EventTypeId, @MessageGroupId, @PushTypes, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string DeleteByParentId = "DELETE FROM inte_event_type_message_group_relation WHERE EventTypeId = @ParentId";

    }
}
