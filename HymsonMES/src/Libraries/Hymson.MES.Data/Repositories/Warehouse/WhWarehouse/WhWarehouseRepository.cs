using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWareHouse.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.WhWareHouse
{
    /// <summary>
    /// 仓储（仓库）
    /// </summary>
    public partial class WhWarehouseRepository : BaseRepository, IWhWarehouseRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WhWarehouseRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhWarehouseEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增忽略重复
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertIgnoreAsync(WhWarehouseEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertIgnoreSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhWarehouseEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        #endregion

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhWarehouseEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WhWarehouseEntity> entities)
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

        #endregion

        #region 查询

        ///// <summary>
        ///// 根据ID获取数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<WhWarehouseEntity> GetByIdAsync(long id)
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryFirstOrDefaultAsync<WhWarehouseEntity>(GetByIdSql, new { Id = id });
        //}

        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<WhWarehouseEntity> GetOneAsync(WhWarehouseQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(query.Code)) {
                sqlBuilder.Where("Code=@Code");
            }

            if (query.Id.HasValue) {
                sqlBuilder.Where("Id=@Id");
            }

            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhWarehouseEntity>(templateData.RawSql, templateData.Parameters);
        }

        ///// <summary>
        ///// 根据IDs获取数据（批量）
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public async Task<IEnumerable<WhWarehouseEntity>> GetByIdsAsync(IEnumerable<long> ids) 
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryAsync<WhWarehouseEntity>(GetByIdsSql, new { Ids = ids });
        //}

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhWarehouseEntity>> GetEntitiesAsync(WhWarehouseQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(query.CodeLike)) {
                query.CodeLike = $"%{query.CodeLike}%";
                sqlBuilder.Where("Code like @CodeLike");
            }

            if (!string.IsNullOrWhiteSpace(query.NameLike))
            {
                query.NameLike = $"%{query.NameLike}%";
                sqlBuilder.Where("Name like @NameLike");
            }

            if (query.Ids != null && query.Ids.Any()) {
                sqlBuilder.Where("Id IN @Ids");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhWarehouseEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseEntity>> GetPagedListAsync(WhWarehousePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code)) {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }

            if (pagedQuery.Status!=null)
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhWarehouseEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhWarehouseEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseEntity>> GetPagedListCopyAsync(WhWarehousePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("Status != 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }

            if (pagedQuery.Status != null)
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhWarehouseEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhWarehouseEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        #endregion

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class WhWarehouseRepository
    {
        #region 查询

        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM wh_warehouse /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM wh_warehouse /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM wh_warehouse /**where**/  ";
        //const string GetByIdSql = @"SELECT * FROM wh_warehouse WHERE Id = @Id ";
        //const string GetByIdsSql = @"SELECT * FROM wh_warehouse WHERE Id IN @Ids ";

        const string GetOneSqlTemplate = "SELECT * FROM `wh_warehouse` /**where**/ LIMIT 1;";

        #endregion

        #region 新增

        const string InsertSql = "INSERT INTO wh_warehouse(  `Id`, `Code`, `Name`, `Status`, `Address`,  `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO wh_warehouse(  `Id`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
#if DM
        const string InsertIgnoreSql = "MERGE INTO wh_warehouse AS targetTable USING((SELECT @Id) AS sourceTable(Id))  ON(targetTable.Id=sourceTable.Id ) WHEN MATCHED THEN UPDATE SET UpdatedOn=@UpdatedOn  WHEN NOT MATCHED THEN INSERT (  `Id`, `Code`, `Name`, `Status`,  `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name,  @Status,  @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId);";
#else
        const string InsertIgnoreSql = "INSERT IGNORE INTO wh_warehouse(  `Id`, `Code`, `Name`, `Status`,  `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name,  @Status,  @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

#endif
        #endregion

        #region 修改

        const string UpdateSql = "UPDATE wh_warehouse SET  Name = @Name, Status = @Status,  Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE wh_warehouse SET  Name = @Name, Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE wh_warehouse SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE wh_warehouse SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        #endregion

    }
}
