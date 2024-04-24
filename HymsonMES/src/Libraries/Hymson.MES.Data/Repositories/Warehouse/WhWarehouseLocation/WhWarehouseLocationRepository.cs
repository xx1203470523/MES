using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWarehouseLocation;
using Hymson.MES.Core.Domain.WhWarehouseShelf;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWarehouseLocation.Query;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.WhWarehouseLocation
{
    /// <summary>
    /// 仓储（库位）
    /// </summary>
    public partial class WhWarehouseLocationRepository : BaseRepository, IWhWarehouseLocationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WhWarehouseLocationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhWarehouseLocationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhWarehouseLocationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 新增Ignore（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertIgnoreRangeAsync(IEnumerable<WhWarehouseLocationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsIgnoreSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhWarehouseLocationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WhWarehouseLocationEntity> entities)
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
        /// 物理删除（批量）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeletesPhysicsAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletePhysicsSql, command);
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

        ///// <summary>
        ///// 根据ID获取数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<WhWarehouseLocationEntity> GetByIdAsync(long id)
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryFirstOrDefaultAsync<WhWarehouseLocationEntity>(GetByIdSql, new { Id = id });
        //}

        ///// <summary>
        ///// 根据IDs获取数据（批量）
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public async Task<IEnumerable<WhWarehouseLocationEntity>> GetByIdsAsync(long[] ids) 
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryAsync<WhWarehouseLocationEntity>(GetByIdsSql, new { Ids = ids });
        //}

        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="whWarehouseLocationQuery"></param>
        /// <returns></returns>
        public async Task<WhWarehouseLocationEntity> GetOneAsync(WhWarehouseLocationQuery whWarehouseLocationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(whWarehouseLocationQuery.Code))
            {
                sqlBuilder.Where("Code=@Code");
            }

            if (whWarehouseLocationQuery.Id.HasValue)
            {
                sqlBuilder.Where("Id=@Id");
            }

            sqlBuilder.AddParameters(whWarehouseLocationQuery);

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhWarehouseLocationEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhWarehouseLocationEntity>> GetEntitiesAsync(WhWarehouseLocationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.OrderBy("UpdatedOn,Code DESC");
            //sqlBuilder.Where("SiteId = @SiteId");

            if (query.WarehouseShelfId.HasValue) {
                sqlBuilder.Where("WarehouseShelfId = @WarehouseShelfId");
            }

            if (query.WarehouseShelfIds != null&& query.WarehouseShelfIds.Any())
            {
                sqlBuilder.Where("WarehouseShelfId IN @WarehouseShelfIds");
            }

            if (query.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }

            if (!string.IsNullOrWhiteSpace(query.CodeLike))
            {
                query.CodeLike = $"%{query.CodeLike}%";
                sqlBuilder.Where("Code like @CodeLike");
            }

            if (query.Status.HasValue) {
                sqlBuilder.Where("Status = @Status");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhWarehouseLocationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseLocationEntity>> GetPagedListAsync(WhWarehouseLocationPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn,Code DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.CodeLike)) {
                pagedQuery.CodeLike = $"%{pagedQuery.CodeLike}%";
                sqlBuilder.Where("Code like @CodeLike");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhWarehouseLocationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhWarehouseLocationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class WhWarehouseLocationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM wh_warehouse_location /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM wh_warehouse_location /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM wh_warehouse_location /**where**/  ";

        const string GetByIdSql = @"SELECT * FROM wh_warehouse_location WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM wh_warehouse_location WHERE Id IN @Ids ";

        const string GetOneSqlTemplate = "SELECT * FROM `wh_warehouse_location` /**where**/ LIMIT 1;";

        const string InsertSql = "INSERT INTO wh_warehouse_location(  `Id`, `WarehouseShelfId`, `Code`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @WarehouseShelfId, @Code, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO wh_warehouse_location(  `Id`, `WarehouseShelfId`, `Code`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @WarehouseShelfId, @Code, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
#if DM
        const string InsertsIgnoreSql = "MERGE INTO wh_warehouse_location AS targetTable USING((SELECT @Id) AS sourceTable(Id))  ON(targetTable.Id=sourceTable.Id ) WHEN MATCHED THEN UPDATE SET UpdatedOn=@UpdatedOn  WHEN NOT MATCHED THEN INSERT (  `Id`, `WarehouseShelfId`, `Code`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @WarehouseShelfId, @Code, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId); ";
#else
        const string InsertsIgnoreSql = "INSERT IGNORE INTO wh_warehouse_location(  `Id`, `WarehouseShelfId`, `Code`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @WarehouseShelfId, @Code, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

#endif
        const string UpdateSql = "UPDATE wh_warehouse_location SET   Status = @Status, Remark = @Remark,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE wh_warehouse_location SET   Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE wh_warehouse_location SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE wh_warehouse_location SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string DeletePhysicsSql = "DELETE FROM  wh_warehouse_location WHERE Id IN @Ids ";

    }
}
