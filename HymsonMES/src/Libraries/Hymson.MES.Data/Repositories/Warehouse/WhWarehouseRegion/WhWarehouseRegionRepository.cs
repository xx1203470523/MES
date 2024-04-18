using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.WhWarehouseRegion;
using Hymson.MES.Core.Domain.WhWarehouseShelf;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.WhWarehouseRegion.Query;
using Hymson.MES.Data.Repositories.WhWarehouseShelf.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.WhWarehouseRegion
{
    /// <summary>
    /// 仓储（库区）
    /// </summary>
    public partial class WhWarehouseRegionRepository : BaseRepository, IWhWarehouseRegionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public WhWarehouseRegionRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(WhWarehouseRegionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增忽略重复
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertIgnoreAsync(WhWarehouseRegionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertIgnoreSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<WhWarehouseRegionEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(WhWarehouseRegionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<WhWarehouseRegionEntity> entities)
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

        ///// <summary>
        ///// 根据ID获取数据
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<WhWarehouseRegionEntity> GetByIdAsync(long id)
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryFirstOrDefaultAsync<WhWarehouseRegionEntity>(GetByIdSql, new { Id = id });
        //}

        ///// <summary>
        ///// 根据IDs获取数据（批量）
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //public async Task<IEnumerable<WhWarehouseRegionEntity>> GetByIdsAsync(IEnumerable<long> ids) 
        //{
        //    using var conn = GetMESDbConnection();
        //    return await conn.QueryAsync<WhWarehouseRegionEntity>(GetByIdsSql, new { Ids = ids });
        //}


        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="whWarehouseRegionQuery"></param>
        /// <returns></returns>
        public async Task<WhWarehouseRegionEntity> GetOneAsync(WhWarehouseRegionQuery whWarehouseRegionQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetOneSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(whWarehouseRegionQuery.Code))
            {
                sqlBuilder.Where("Code=@Code");
            }

            if (whWarehouseRegionQuery.Id.HasValue)
            {
                sqlBuilder.Where("Id=@Id");
            }

            sqlBuilder.AddParameters(whWarehouseRegionQuery);

            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<WhWarehouseRegionEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WhWarehouseRegionEntity>> GetEntitiesAsync(WhWarehouseRegionQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted=0");

            if (!string.IsNullOrWhiteSpace(query.CodeLike)) {
                query.CodeLike = $"%{query.CodeLike}%";
                sqlBuilder.Where("Code like @CodeLike");
            }

            if (query.WarehouseIds != null && query.WarehouseIds.Any()) {
                sqlBuilder.Where("WarehouseId IN @WarehouseIds");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<WhWarehouseRegionEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhWarehouseRegionEntity>> GetPagedListAsync(WhWarehouseRegionPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name)) {
                pagedQuery.Name= $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }

            if (pagedQuery.Status!=null)
            {
                sqlBuilder.Where("Status = @Status");
            }

            if (pagedQuery.WareHouseIds != null && pagedQuery.WareHouseIds.Any()) {
                sqlBuilder.Where("WarehouseId IN @WareHouseIds");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<WhWarehouseRegionEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<WhWarehouseRegionEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class WhWarehouseRegionRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM wh_warehouse_region /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM wh_warehouse_region /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM wh_warehouse_region /**where**/  ";

        //const string GetByIdSql = @"SELECT * FROM wh_warehouse_region WHERE Id = @Id ";
        //const string GetByIdsSql = @"SELECT * FROM wh_warehouse_region WHERE Id IN @Ids ";

        const string GetOneSqlTemplate = "SELECT * FROM `wh_warehouse_region` /**where**/ LIMIT 1;";

        const string InsertSql = "INSERT INTO wh_warehouse_region(  `Id`, `Code`, `Name`, `WarehouseId`, `Remark`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @Remark, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO wh_warehouse_region(  `Id`, `Code`, `Name`, `WarehouseId`, `Remark`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @Remark, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
#if DM
        const string InsertIgnoreSql = "MERGE INTO wh_warehouse_region AS targetTable USING((SELECT @Id) AS sourceTable(Id))  ON(targetTable.Id=sourceTable.Id ) WHEN MATCHED THEN UPDATE SET UpdatedOn=@UpdatedOn  WHEN NOT MATCHED THEN INSERT (  `Id`, `Code`, `Name`, `WarehouseId`, `Remark`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @Remark, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId);";
#else
        const string InsertIgnoreSql = "INSERT IGNORE INTO wh_warehouse_region(  `Id`, `Code`, `Name`, `WarehouseId`, `Remark`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @WarehouseId, @Remark, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

#endif
        const string UpdateSql = "UPDATE wh_warehouse_region SET Name = @Name, Remark = @Remark, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE wh_warehouse_region SET Name = @Name,  Remark = @Remark, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE wh_warehouse_region SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE wh_warehouse_region SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

    }
}
