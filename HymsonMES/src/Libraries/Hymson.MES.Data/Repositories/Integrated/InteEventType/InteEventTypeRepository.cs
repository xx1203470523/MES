using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（事件类型维护）
    /// </summary>
    public partial class InteEventTypeRepository : BaseRepository, IInteEventTypeRepository
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
        public InteEventTypeRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteEventTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteEventTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteEventTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteEventTypeEntity> entities)
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
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteEventTypeEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteEventTypeEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteEventTypeEntity> GetByIdAsync(long id)
        {
            var key = $"inte_event_type&id-{id}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<InteEventTypeEntity>(GetByIdSql, new { Id = id });
            });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteEventTypeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询事件类型
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeEntity>> GetByWorkShopIdSqlAsync(long workShopId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteEventTypeEntity>(GetByWorkShopIdSql, new { WorkShopId = workShopId });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteEventTypeEntity>> GetEntitiesAsync(InteEventTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteEventTypeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteEventTypeView>> GetPagedListAsync(InteEventTypePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("inte_work_center IWC ON IWC.Id = T.WorkShopId");
            sqlBuilder.Select("T.*, IWC.Name AS WorkShopName");
            sqlBuilder.OrderBy("T.UpdatedOn DESC");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("T.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("T.Name LIKE @Name");
            }

            if (pagedQuery.WorkShopId.HasValue)
            {
                sqlBuilder.Where("T.WorkShopId = @WorkShopId");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.WorkShopName))
            {
                pagedQuery.WorkShopName = $"%{pagedQuery.WorkShopName}%";
                sqlBuilder.Where("IWC.Name LIKE @WorkShopName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<InteEventTypeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteEventTypeView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteEventTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_event_type T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_event_type T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_event_type /**where**/  ";
#if DM
        const string InsertSql = "INSERT  inte_event_type(  `Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT  inte_event_type(  `Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
#else
 const string InsertSql = "INSERT IGNORE inte_event_type(  `Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT IGNORE inte_event_type(  `Id`, `SiteId`, `Code`, `Name`, `WorkShopId`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @WorkShopId, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
#endif
        const string UpdateSql = "UPDATE inte_event_type SET   SiteId = @SiteId, Code = @Code, Name = @Name, WorkShopId = @WorkShopId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_event_type SET   SiteId = @SiteId, Code = @Code, Name = @Name, WorkShopId = @WorkShopId, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_event_type SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_event_type SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByCodeSql = "SELECT * FROM inte_event_type WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetByIdSql = @"SELECT * FROM inte_event_type WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_event_type WHERE Id IN @Ids ";
        const string GetByWorkShopIdSql = @"SELECT * FROM inte_event_type WHERE IsDeleted = 0 AND WorkShopId = @WorkShopId ";

    }
}
