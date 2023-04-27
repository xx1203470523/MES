using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFeeding.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    /// <summary>
    /// 仓储（物料加载）
    /// </summary>
    public partial class ManuFeedingRepository : IManuFeedingRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuFeedingRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFeedingEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyByProductIdAsync(IEnumerable<UpdateQtyByProductIdCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateQtyByProductIdSql, commands);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByIds, new { ids });
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuFeedingEntity>(GetByIds, new { ids });
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdAsync(GetByResourceIdAndMaterialIdQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuFeedingEntity>(GetByResourceIdAndMaterialId, query);
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdsAsync(GetByResourceIdAndMaterialIdsQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuFeedingEntity>(GetByResourceIdAndMaterialIds, query);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFeedingRepository
    {
        const string InsertSql = "INSERT INTO `manu_feeding`(  `Id`, `ResourceId`, `FeedingPointId`, `ProductId`, SupplierId, `BarCode`, `InitQty`, `Qty`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @ResourceId, @FeedingPointId, @ProductId, @SupplierId, @BarCode, @InitQty, @Qty, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateQtyByProductIdSql = "UPDATE manu_feeding SET Qty = @Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE ResourceId = @ResourceId AND ProductId = @ProductId ";
        const string DeleteSql = "UPDATE manu_feeding SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string DeleteByIds = "DELETE FROM manu_feeding WHERE Id IN @ids; ";
        const string GetByIds = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND Id IN @ids; ";
        const string GetByResourceIdAndMaterialId = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND ResourceId = @ResourceId AND ProductId = @MaterialId; ";
        const string GetByResourceIdAndMaterialIds = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND ResourceId = @ResourceId AND ProductId IN @MaterialIds; ";
    }
}
