using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码追溯表）
    /// </summary>
    public partial class ManuSFCNodeRepository : BaseRepository, IManuSFCNodeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSFCNodeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSFCNodeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSFCNodeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSFCNodeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSFCNodeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSFCNodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSFCNodeEntity> GetBySFCAsync(EntityBySFCQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("SFC = @SFC");
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSFCNodeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSFCNodeEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSFCNodeEntity>(GetByIdsSql, new { Ids = ids });
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSFCNodeRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_node /**where**/  ";
        //const string InsertSql = "INSERT INTO manu_sfc_node (Id, ProductId, SFC, Name, Location, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId) VALUES (@Id, @ProductId, @SFC, @Name, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId);";
        const string DeleteSql = "DELETE FROM manu_sfc_node WHERE Id IN @Ids ";

#if DM
        const string MergeSql = "MERGE INTO manu_sfc_node t " +
            "USING (SELECT @Id AS Id FROM dual) s " +
            "ON (t.Id = s.Id) " +
            "WHEN NOT MATCHED THEN " +
              "INSERT (Id, ProductId, SFC, Name, Location, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId) " +
              "VALUES (s.Id, @ProductId, @SFC, @Name, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId); ";
#else
        const string InsertsSql = "INSERT IGNORE manu_sfc_node(  `Id`, `ProductId`, `SFC`, `Name`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @ProductId, @SFC, @Name, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
#endif

        const string UpdateSql = "UPDATE manu_sfc_node SET   ProductId = @ProductId, SFC = @SFC, Name = @Name, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_sfc_node SET   ProductId = @ProductId, SFC = @SFC, Name = @Name, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId WHERE Id = @Id ";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_node WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_sfc_node WHERE Id IN @Ids ";

    }
}
