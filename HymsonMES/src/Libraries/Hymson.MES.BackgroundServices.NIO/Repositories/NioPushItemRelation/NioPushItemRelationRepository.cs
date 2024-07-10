using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.BackgroundServices.NIO
{
    /// <summary>
    /// 仓储（蔚来推送项目关系表）
    /// </summary>
    public partial class NioPushItemRelationRepository : BaseRepository, INioPushItemRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public NioPushItemRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(NioPushItemRelationEntity entity)
        {
            using var conn = GetStatorDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<NioPushItemRelationEntity> entities)
        {
            using var conn = GetStatorDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(NioPushItemRelationEntity entity)
        {
            using var conn = GetStatorDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<NioPushItemRelationEntity> entities)
        {
            using var conn = GetStatorDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetStatorDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetStatorDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NioPushItemRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetStatorDbConnection();
            return await conn.QueryFirstOrDefaultAsync<NioPushItemRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushItemRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetStatorDbConnection();
            return await conn.QueryAsync<NioPushItemRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushItemRelationEntity>> GetEntitiesAsync(NioPushItemRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetStatorDbConnection();
            return await conn.QueryAsync<NioPushItemRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushItemRelationEntity>> GetPagedListAsync(NioPushItemRelationPagedQuery pagedQuery)
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

            using var conn = GetStatorDbConnection();
            var entitiesTask = conn.QueryAsync<NioPushItemRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<NioPushItemRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class NioPushItemRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM nio_push_item_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM nio_push_item_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM nio_push_item_relation /**where**/  ";

        const string InsertSql = "INSERT INTO nio_push_item_relation(  `Id`, `BuzScene`, `BuzType`, `PushId`, `PushItemId`, `CreatedBy`, `CreatedOn`) VALUES (  @Id, @BuzScene, @BuzType, @PushId, @PushItemId, @CreatedBy, @CreatedOn) ";
        const string InsertsSql = "INSERT INTO nio_push_item_relation(  `Id`, `BuzScene`, `BuzType`, `PushId`, `PushItemId`, `CreatedBy`, `CreatedOn`) VALUES (  @Id, @BuzScene, @BuzType, @PushId, @PushItemId, @CreatedBy, @CreatedOn) ";

        const string UpdateSql = "UPDATE nio_push_item_relation SET   BuzScene = @BuzScene, BuzType = @BuzType, PushId = @PushId, PushItemId = @PushItemId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE nio_push_item_relation SET   BuzScene = @BuzScene, BuzType = @BuzType, PushId = @PushId, PushItemId = @PushItemId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE nio_push_item_relation SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE nio_push_item_relation SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM nio_push_item_relation WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM nio_push_item_relation WHERE Id IN @Ids ";

    }
}
