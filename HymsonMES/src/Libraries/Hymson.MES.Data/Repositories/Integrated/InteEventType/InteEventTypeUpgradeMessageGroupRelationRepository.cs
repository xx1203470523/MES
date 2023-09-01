using Dapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（事件升级消息组关联表）
    /// </summary>
    public partial class InteEventTypeUpgradeMessageGroupRelationRepository : BaseRepository, IInteEventTypeUpgradeMessageGroupRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public InteEventTypeUpgradeMessageGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteEventTypeUpgradeMessageGroupRelationEntity> entities)
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
        public async Task<IEnumerable<InteEventTypeUpgradeMessageGroupRelationEntity>> GetEntitiesAsync(InteEventTypeUpgradeMessageGroupRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("PushScene = @PushScene");
            sqlBuilder.Where("EventTypeId = @EventTypeId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteEventTypeUpgradeMessageGroupRelationEntity>(template.RawSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteEventTypeUpgradeMessageGroupRelationRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_event_type_upgrade_message_group_relation /**where**/  ";

        const string InsertsSql = "INSERT INTO inte_event_type_upgrade_message_group_relation(  `Id`, `EventTypeId`, `PushScene`, `EventTypeUpgradeId`, `MessageGroupId`, `PushTypes`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @EventTypeId, @PushScene, @EventTypeUpgradeId, @MessageGroupId, @PushTypes, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string DeleteByParentId = "DELETE FROM inte_event_type_upgrade_message_group_relation WHERE EventTypeId = @ParentId";

    }
}
