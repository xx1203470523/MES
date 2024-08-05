using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.NioPushCollection
{
    /// <summary>
    /// 仓储（NIO推送参数）
    /// </summary>
    public partial class NioPushCollectionRepository : BaseRepository, INioPushCollectionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public NioPushCollectionRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(NioPushCollectionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<NioPushCollectionEntity> entities)
        {
            var (sql, param) = SqlHelper.JoinInsertSql(InsertSqlInsert, InsertSqlValue, entities);

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(sql, param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(NioPushCollectionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<NioPushCollectionEntity> entities)
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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NioPushCollectionEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<NioPushCollectionEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushCollectionEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushCollectionEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushCollectionEntity>> GetEntitiesAsync(NioPushCollectionQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushCollectionEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushCollectionEntity>> GetPagedListAsync(NioPushCollectionPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<NioPushCollectionEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<NioPushCollectionEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class NioPushCollectionRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM nio_push_collection /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM nio_push_collection /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM nio_push_collection /**where**/  ";

        const string InsertSql = "INSERT INTO nio_push_collection(  `Id`, `NioPushId`, `PlantId`, `WorkshopId`, `ProductionLineId`, `StationId`, `VendorFieldCode`, `VendorProductNum`, `VendorProductName`, `VendorProductSn`, `VendorProductTempSn`, `ProcessType`, `DecimalValue`, `StringValue`, `BooleanValue`, `NioProductNum`, `NioProductName`, `OperatorAccount`, `InputTime`, `OutputTime`, `StationStatus`, `VendorStationStatus`, `VendorValueStatus`, `Owner`, `DataType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @NioPushId, @PlantId, @WorkshopId, @ProductionLineId, @StationId, @VendorFieldCode, @VendorProductNum, @VendorProductName, @VendorProductSn, @VendorProductTempSn, @ProcessType, @DecimalValue, @StringValue, @BooleanValue, @NioProductNum, @NioProductName, @OperatorAccount, @InputTime, @OutputTime, @StationStatus, @VendorStationStatus, @VendorValueStatus, @Owner, @DataType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO nio_push_collection(  `Id`, `NioPushId`, `PlantId`, `WorkshopId`, `ProductionLineId`, `StationId`, `VendorFieldCode`, `VendorProductNum`, `VendorProductName`, `VendorProductSn`, `VendorProductTempSn`, `ProcessType`, `DecimalValue`, `StringValue`, `BooleanValue`, `NioProductNum`, `NioProductName`, `OperatorAccount`, `InputTime`, `OutputTime`, `StationStatus`, `VendorStationStatus`, `VendorValueStatus`, `Owner`, `DataType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @NioPushId, @PlantId, @WorkshopId, @ProductionLineId, @StationId, @VendorFieldCode, @VendorProductNum, @VendorProductName, @VendorProductSn, @VendorProductTempSn, @ProcessType, @DecimalValue, @StringValue, @BooleanValue, @NioProductNum, @NioProductName, @OperatorAccount, @InputTime, @OutputTime, @StationStatus, @VendorStationStatus, @VendorValueStatus, @Owner, @DataType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertSqlInsert = "INSERT INTO nio_push_collection(  `Id`, `NioPushId`, `PlantId`, `WorkshopId`, `ProductionLineId`, `StationId`, `VendorFieldCode`, `VendorProductNum`, `VendorProductName`, `VendorProductSn`, `VendorProductTempSn`, `ProcessType`, `DecimalValue`, `StringValue`, `BooleanValue`, `NioProductNum`, `NioProductName`, `OperatorAccount`, `InputTime`, `OutputTime`, `StationStatus`, `VendorStationStatus`, `VendorValueStatus`, `Owner`, `DataType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES ";
        const string InsertSqlValue = "(  @Id, @NioPushId, @PlantId, @WorkshopId, @ProductionLineId, @StationId, @VendorFieldCode, @VendorProductNum, @VendorProductName, @VendorProductSn, @VendorProductTempSn, @ProcessType, @DecimalValue, @StringValue, @BooleanValue, @NioProductNum, @NioProductName, @OperatorAccount, @InputTime, @OutputTime, @StationStatus, @VendorStationStatus, @VendorValueStatus, @Owner, @DataType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE nio_push_collection SET   NioPushId = @NioPushId, PlantId = @PlantId, WorkshopId = @WorkshopId, ProductionLineId = @ProductionLineId, StationId = @StationId, VendorFieldCode = @VendorFieldCode, VendorProductNum = @VendorProductNum, VendorProductName = @VendorProductName, VendorProductSn = @VendorProductSn, VendorProductTempSn = @VendorProductTempSn, ProcessType = @ProcessType, DecimalValue = @DecimalValue, StringValue = @StringValue, BooleanValue = @BooleanValue, NioProductNum = @NioProductNum, NioProductName = @NioProductName, OperatorAccount = @OperatorAccount, InputTime = @InputTime, OutputTime = @OutputTime, StationStatus = @StationStatus, VendorStationStatus = @VendorStationStatus, VendorValueStatus = @VendorValueStatus, Owner = @Owner, DataType = @DataType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE nio_push_collection SET   NioPushId = @NioPushId, PlantId = @PlantId, WorkshopId = @WorkshopId, ProductionLineId = @ProductionLineId, StationId = @StationId, VendorFieldCode = @VendorFieldCode, VendorProductNum = @VendorProductNum, VendorProductName = @VendorProductName, VendorProductSn = @VendorProductSn, VendorProductTempSn = @VendorProductTempSn, ProcessType = @ProcessType, DecimalValue = @DecimalValue, StringValue = @StringValue, BooleanValue = @BooleanValue, NioProductNum = @NioProductNum, NioProductName = @NioProductName, OperatorAccount = @OperatorAccount, InputTime = @InputTime, OutputTime = @OutputTime, StationStatus = @StationStatus, VendorStationStatus = @VendorStationStatus, VendorValueStatus = @VendorValueStatus, Owner = @Owner, DataType = @DataType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE nio_push_collection SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE nio_push_collection SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM nio_push_collection WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM nio_push_collection WHERE Id IN @Ids ";

    }
}
