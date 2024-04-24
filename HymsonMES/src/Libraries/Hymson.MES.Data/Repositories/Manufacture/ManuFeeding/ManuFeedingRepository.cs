using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;
using System.Text;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    /// <summary>
    /// 仓储（物料加载）
    /// </summary>
    public partial class ManuFeedingRepository : BaseRepository, IManuFeedingRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuFeedingRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFeedingEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyByIdAsync(UpdateFeedingQtyByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQtyByIdSql, command);
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateFeedingQtyByIdAsync(IEnumerable<UpdateFeedingQtyByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQtyByIdSql, commands);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByIds, new { ids });
        }

        /// <summary>
        /// 根据Code和物料ID查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingEntity> GetByBarCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFeedingEntity>(GetByBarCodeSql, query);
        }

        /// <summary>
        /// 根据Code和物料ID查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingEntity> GetByBarCodeAndMaterialIdAsync(GetByBarCodeAndMaterialIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFeedingEntity>(GetByBarCodeAndMaterialIdSql, query);
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(GetByIds, new { ids });
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdAsync(GetByResourceIdAndMaterialIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(GetByResourceIdAndMaterialId, query);
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdsAsync(GetByResourceIdAndMaterialIdsQuery query)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND ResourceId = @ResourceId ");

            if (query.LoadSource.HasValue) sqlBuilder.Append("AND LoadSource = @LoadSource ");
            if (query.FeedingPointId.HasValue) sqlBuilder.Append("AND FeedingPointId = @FeedingPointId ");
            if (query.MaterialIds != null) sqlBuilder.Append("AND ProductId IN @MaterialIds; ");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(sqlBuilder.ToString(), query);
        }

        /// <summary>
        /// 获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByFeedingPointIdAndMaterialIdsAsync(GetByFeedingPointIdAndMaterialIdsQuery query)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND FeedingPointId = @FeedingPointId ");

            if (query.LoadSource.HasValue) sqlBuilder.Append("AND LoadSource = @LoadSource ");
            if (query.MaterialIds != null) sqlBuilder.Append("AND ProductId IN @MaterialIds; ");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(sqlBuilder.ToString(), query);
        }

        /// <summary>
        /// 获取加载数据列表（只读取剩余数量大于0的）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByResourceIdAndMaterialIdsWithOutZeroAsync(GetByResourceIdAndMaterialIdsQuery query)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND ResourceId = @ResourceId ");

            if (query.MaterialIds != null) sqlBuilder.Append("AND ProductId IN @MaterialIds ");

            sqlBuilder.Append("AND Qty > 0 ");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(sqlBuilder.ToString(), query);
        }

        /// <summary>
        /// 获取加载数据列表（只读取剩余数量大于0的）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByFeedingPointIdWithOutZeroAsync(GetByFeedingPointIdsQuery query)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND FeedingPointId IN @FeedingPointIds ");

            sqlBuilder.Append("AND Qty > 0 ");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(sqlBuilder.ToString(), query);
        }

        /// <summary>
        /// 根据上料点Id与资源IDs获取加载数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetByFeedingPointIdAndResourceIdsAsync(GetByFeedingPointIdAndResourceIdsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(GetByFeedingPointIdAndResourceIdsSql, query);
        }

        #region 顷刻

        /// <summary>
        /// 获取最新的上料记录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingEntity> GetFeedingPointNewAsync(GetFeedingPointNewQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFeedingEntity>(GetFeedingPointNewSql, query);
        }

        /// <summary>
        /// 查询条码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuFeedingEntity> GetManuFeedingSfcAsync(GetManuFeedingSfcQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFeedingEntity>(GetManuFeedingSfcSql, query);
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="loadPointId"></param>
        /// <returns></returns>
        public async Task<int> UpdateFeedingQtyAsync(UpdateFeedingQtyCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateFeedingQtySql, command);
        }

        /// <summary>
        /// 更新条码数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuFeedingBarcodeQtyAsync(UpdateFeedingBarcodeQtyCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateFeedingBarCodeQtySql, command);
        }

        /// <summary>
        /// 根据资源获取所有的上料信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetAllByResourceIdAsync(EntityByResourceIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(GetAllByResourceIdSql, query);
        }

        /// <summary>
        /// 根据条码获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFeedingEntity>> GetAllBySfcListAsync(GetManuFeedingSfcListQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFeedingEntity>(GetAllBySfcListSql, query);
        }

        #endregion

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFeedingRepository
    {
#if DM
        const string InsertSql = "MERGE INTO manu_feeding t " +
            "USING (SELECT @Id AS Id FROM dual) s " +
            "ON (t.Id = s.Id) " +
            "WHEN MATCHED THEN " +
              "UPDATE SET " +
                "t.ResourceId = @ResourceId, " +
                "t.FeedingPointId = @FeedingPointId, " +
                "t.ProductId = @ProductId, " +
                "t.SupplierId = @SupplierId, " +
                "t.BarCode = @BarCode, " +
                "t.MaterialId = @MaterialId, " +
                "t.InitQty = @InitQty, " +
                "t.Qty = @Qty, " +
                "t.MaterialType = @MaterialType, " +
                "t.CreatedBy = @CreatedBy, " +
                "t.CreatedOn = @CreatedOn, " +
                "t.UpdatedBy = @UpdatedBy, " +
                "t.UpdatedOn = @UpdatedOn, " +
                "t.IsDeleted = @IsDeleted, " +
                "t.SiteId = @SiteId, " +
                "t.WorkOrderId = @WorkOrderId, " +
                "t.LoadSource = @LoadSource " +
            "WHEN NOT MATCHED THEN " +
              "INSERT (Id, ResourceId, FeedingPointId, ProductId, SupplierId, BarCode, MaterialId, InitQty, Qty, MaterialType, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, SiteId, WorkOrderId, LoadSource) " +
              "VALUES (s.Id, @ResourceId, @FeedingPointId, @ProductId, @SupplierId, @BarCode, @MaterialId, @InitQty, @Qty, @MaterialType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @WorkOrderId, @LoadSource);";
        const string UpdateQtyByIdSql = "UPDATE manu_feeding SET Qty = (CASE WHEN @Qty > Qty THEN 0 ELSE Qty - CAST(@Qty AS DECIMAL) END), UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Qty > 0 AND Id = @Id; ";
#else
        const string InsertSql = "REPLACE INTO `manu_feeding`(`Id`, `ResourceId`, `FeedingPointId`, `ProductId`, SupplierId, `BarCode`, MaterialId, `InitQty`, `Qty`,`MaterialType`,  `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, WorkOrderId, LoadSource) VALUES (@Id, @ResourceId, @FeedingPointId, @ProductId, @SupplierId, @BarCode, @MaterialId, @InitQty, @Qty,@MaterialType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @WorkOrderId, @LoadSource)  ";
        const string UpdateQtyByIdSql = "UPDATE manu_feeding SET Qty = (CASE WHEN @Qty > Qty THEN 0 ELSE Qty - @Qty END), UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Qty > 0 AND Id = @Id; ";
#endif


        const string DeleteSql = "UPDATE manu_feeding SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string DeleteByIds = "DELETE FROM manu_feeding WHERE Id IN @ids; ";
        const string GetByBarCodeSql = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND SiteId = @Site AND BarCode = @Code;";
        const string GetByBarCodeAndMaterialIdSql = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND FeedingPointId = @FeedingPointId AND ProductId = @ProductId AND BarCode = @BarCode;";
        const string GetByIds = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND Id IN @ids; ";
        const string GetByResourceIdAndMaterialId = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND ResourceId = @ResourceId AND ProductId = @MaterialId; ";

        const string GetByFeedingPointIdAndResourceIdsSql = "SELECT * FROM manu_feeding WHERE IsDeleted = 0 AND ResourceId in @ResourceIds AND FeedingPointId = @FeedingPointId ";

        #region 顷刻

        /// <summary>
        /// 获取最新一条上料记录
        /// </summary>
        const string GetFeedingPointNewSql = @"
            select * from manu_feeding mf 
            where FeedingPointId = @FeedingPointId
            order by CreatedOn desc 
            limit 0,1
        ";

        /// <summary>
        /// 获取条码信息
        /// </summary>
        const string GetManuFeedingSfcSql = @"
            select * from manu_feeding mf 
            where LoadSource  = @LoadSource
            and BarCode = @BarCode
            and IsDeleted = 0
            order by CreatedOn desc
        ";

        /// <summary>
        /// 更新数量
        /// </summary>
        const string UpdateFeedingQtySql = @"
            update manu_feeding
            set Qty = Qty - @Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn,IsDeleted = @IsDeleted
            where BarCode = @BarCode
            and ( ResourceId = @ResourceId or FeedingPointId = @FeedingPointId )
        ";

        /// <summary>
        /// 更新数量
        /// </summary>
        const string UpdateFeedingBarCodeQtySql = @"
            update manu_feeding
            set Qty = 0, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn
            where BarCode = @BarCode
            and SiteId = @SiteId
        ";


        /// <summary>
        /// 根据资源获取所有的上料信息
        /// </summary>
        const string GetAllByResourceIdSql = @"
            select * from manu_feeding 
            where ResourceId = @ResourceId
            and IsDeleted  = 0
            and SiteId = @SiteId
            and LoadSource = 1
            and Qty  > 0
        ";

        /// <summary>
        /// 获取条码列表
        /// </summary>
        const string GetAllBySfcListSql = @"
            select * from manu_feeding mf 
            where BarCode in @BarCodeLisst
            and IsDeleted  = 0
            and SiteId =  @SiteId
        ";

        #endregion
    }
}
