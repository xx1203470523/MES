using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.NIO;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.NIO
{
    /// <summary>
    /// 仓储（蔚来推送表）
    /// </summary>
    public partial class NioPushRepository : BaseRepository, INioPushRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public NioPushRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(NioPushEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<NioPushEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(NioPushEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新内容
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateContentAsync(NioPushEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateContentSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<NioPushEntity> entities)
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
        /// 根据水位批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushEntity>> GetListByStartWaterMarkIdAsync(EntityByWaterMarkQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushEntity>(GetListByStartWaterMarkIdSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NioPushEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<NioPushEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushEntity>> GetEntitiesAsync(NioPushQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushEntity>> GetPagedListAsync(NioPushPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<NioPushEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<NioPushEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class NioPushRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM nio_push /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM nio_push /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM nio_push /**where**/  ";

        const string InsertSql = "INSERT INTO nio_push(  `Id`, `SchemaCode`, `BuzScene`, `Status`, Content, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SchemaCode, @BuzScene, @Status, @Content, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO nio_push(  `Id`, `SchemaCode`, `BuzScene`, `Status`, Content, Remark, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SchemaCode, @BuzScene, @Status, @Content, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE nio_push SET Status = @Status, Content = @Content, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE nio_push SET Result = @Result, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE nio_push SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE nio_push SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetListByStartWaterMarkIdSql = @"SELECT * FROM nio_push WHERE Status = 1 ORDER BY Id ASC LIMIT @Rows";
        const string GetByIdSql = @"SELECT * FROM nio_push WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM nio_push WHERE Id IN @Ids ";

        const string UpdateContentSql = "UPDATE nio_push SET Content = @Content, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
    }
}
