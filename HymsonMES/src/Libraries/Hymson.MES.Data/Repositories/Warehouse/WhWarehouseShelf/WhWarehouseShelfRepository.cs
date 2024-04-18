using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWareHouse;
using Hymson.MES.Core.Domain.WhWarehouseShelf;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.WhWarehouseShelf
{
    /// <summary>
    /// 仓储（货架）
    /// </summary>
    public partial class WhWarehouseShelfRepository : BaseRepository, IWhWarehouseShelfRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WhWarehouseShelfRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhWarehouseShelfEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertIgnoreAsync(WhWarehouseShelfEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertIgnoreSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhWarehouseShelfEntity> entities)
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
        public async Task<int> UpdateAsync(WhWarehouseShelfEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WhWarehouseShelfEntity> entities)
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
        //public async Task<WhWarehouseShelfEntity> GetByIdAsync(long id)
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryFirstOrDefaultAsync<WhWarehouseShelfEntity>(GetByIdSql, new { Id = id });
        //}

        ///// <summary>
        ///// 根据IDs获取数据（批量）
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public async Task<IEnumerable<WhWarehouseShelfEntity>> GetByIdsAsync(IEnumerable<long> ids) 
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryAsync<WhWarehouseShelfEntity>(GetByIdsSql, new { Ids = ids });
        //}

        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="whWarehouseShelfQuery"></param>
        /// <returns></returns>
        public async Task<WhWarehouseShelfEntity> GetOneAsync(WhWarehouseShelfQuery whWarehouseShelfQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(whWarehouseShelfQuery.Code))
            {
                sqlBuilder.Where("Code=@Code");
            }

            if (whWarehouseShelfQuery.Id.HasValue)
            {
                sqlBuilder.Where("Id=@Id");
            }

            sqlBuilder.AddParameters(whWarehouseShelfQuery);

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhWarehouseShelfEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhWarehouseShelfEntity>> GetEntitiesAsync(WhWarehouseShelfQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");

            if (query.Ids != null && query.Ids.Any()) {
                sqlBuilder.Where("Id IN @Ids");
            }

            if (!string.IsNullOrWhiteSpace(query.CodeLike)) {
                query.CodeLike = $"%{query.CodeLike}%";
                sqlBuilder.Where("Code like @CodeLike");
            }

            if (query.WarehouseRegionIds != null && query.WarehouseRegionIds.Any()) {
                sqlBuilder.Where("WarehouseRegionId IN @WarehouseRegionIds");
            }

            if (query.WarehouseIds != null && query.WarehouseIds.Any())
            {
                sqlBuilder.Where("WarehouseId IN @WarehouseIds");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhWarehouseShelfEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseShelfEntity>> GetPagedListAsync(WhWarehouseShelfPagedQuery pagedQuery)
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
                sqlBuilder.Where("Status like @Status");
            }

            if (pagedQuery.WarehouseIds != null&& pagedQuery.WarehouseIds.Any())
            {
                sqlBuilder.Where("WarehouseId IN @WarehouseIds");
            }

            if (pagedQuery.WarehouseRegionIds != null && pagedQuery.WarehouseRegionIds.Any())
            {
                sqlBuilder.Where("WarehouseRegionId IN @WarehouseRegionIds");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhWarehouseShelfEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhWarehouseShelfEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        #endregion

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class WhWarehouseShelfRepository
    {
        
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM wh_warehouse_shelf /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM wh_warehouse_shelf /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM wh_warehouse_shelf /**where**/  ";

        //const string GetByIdSql = @"SELECT * FROM wh_warehouse_shelf WHERE Id = @Id ";
        //const string GetByIdsSql = @"SELECT * FROM wh_warehouse_shelf WHERE Id IN @Ids ";

        const string GetOneSqlTemplate = "SELECT * FROM `wh_warehouse_shelf` /**where**/ LIMIT 1;";

        const string InsertSql = "INSERT INTO wh_warehouse_shelf(  `Id`, `Code`, `Name`, `WarehouseId`, `WarehouseRegionId`, `Column`, `Row`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @WarehouseRegionId, @Column, @Row, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO wh_warehouse_shelf(  `Id`, `Code`, `Name`, `WarehouseId`, `WarehouseRegionId`, `Column`, `Row`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @WarehouseRegionId, @Column, @Row, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
#if DM
        const string InsertIgnoreSql = "MERGE INTO wh_warehouse_shelf AS targetTable USING((SELECT @Id) AS sourceTable(Id))  ON(targetTable.Id=sourceTable.Id ) WHEN MATCHED THEN UPDATE SET UpdatedOn=@UpdatedOn WHEN NOT MATCHED THEN INSERT (  `Id`, `Code`, `Name`, `WarehouseId`, `WarehouseRegionId`, `Column`, `Row`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @WarehouseRegionId, @Column, @Row, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId);";
#else
        const string InsertIgnoreSql = "INSERT IGNORE INTO wh_warehouse_shelf(  `Id`, `Code`, `Name`, `WarehouseId`, `WarehouseRegionId`, `Column`, `Row`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @WarehouseRegionId, @Column, @Row, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

#endif
        const string UpdateSql = "UPDATE wh_warehouse_shelf SET  Name = @Name,`Column` = @Column, `Row` = @Row, Status = @Status, Remark = @Remark,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE wh_warehouse_shelf SET  Name = @Name,`Column` = @Column, `Row` = @Row, Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE wh_warehouse_shelf SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE wh_warehouse_shelf SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        

    }
}
