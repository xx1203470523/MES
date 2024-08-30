using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.NioPushCollection;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO.NioPushCollection.View;
using Hymson.MES.Data.Repositories.NioPushCollection.Query;
using Hymson.Utils.Tools;
using IdGen;
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
            if(entities == null || entities.Count() == 0)
            {
                return 0;
            }

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
        /// 根据niopushID获取数据
        /// </summary>
        /// <param name="nioPushId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushCollectionEntity>> GetByPushIdAsync(long nioPushId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushCollectionEntity>(GetByNioPushIdSql, new { NioPushId = nioPushId });
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
        public async Task<PagedInfo<NioPushCollectionStatusView>> GetPagedListAsync(NioPushCollectionPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("t1.*,t2.Status");
            sqlBuilder.OrderBy("t1.UpdatedOn DESC");
            sqlBuilder.Where("t1.IsDeleted = 0");
            sqlBuilder.InnerJoin("nio_push t2 on t1.NioPushId = t2.Id and t2.IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");
            if (string.IsNullOrEmpty(pagedQuery.VendorProductSn) == false)
            {
                sqlBuilder.Where("VendorProductSn = @VendorProductSn");
            }
            if (string.IsNullOrEmpty(pagedQuery.VendorProductTempSn) == false)
            {
                sqlBuilder.Where("VendorProductTempSn = @VendorProductTempSn");
            }
            if (string.IsNullOrEmpty(pagedQuery.StationId) == false)
            {
                sqlBuilder.Where("StationId = @StationId");
            }
            if(string.IsNullOrEmpty(pagedQuery.VendorFieldCode) == false)
            {
                sqlBuilder.Where("VendorFieldCode = @VendorFieldCode");
            }
            if(pagedQuery.Status != null)
            {
                sqlBuilder.Where("t2.Status = @Status");
            }
            if(pagedQuery.IsOk != null)
            {
                sqlBuilder.Where("IsOk = @IsOk");
            }
            if(pagedQuery.CreatedOn != null && pagedQuery.CreatedOn.Count() == 2)
            {
                sqlBuilder.Where($"t1.CreatedOn  >= '{pagedQuery.CreatedOn[0].ToString("yyyy-MM-dd HH:mm:ss")}' ");
                sqlBuilder.Where($"t1.CreatedOn  < '{pagedQuery.CreatedOn[1].ToString("yyyy-MM-dd HH:mm:ss")}' ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<NioPushCollectionStatusView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<NioPushCollectionStatusView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 获取重复List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushCollectionRepeatView>> GetRepeatEntitiesAsync(NioPushCollectionRepeatQuery query)
        {
            string stationSql = string.Empty;
            if(query.ProcedureList != null && query.ProcedureList.Count > 0)
            {
                stationSql = " and StationId in @ProcedureList ";
            }

            string sql = $@"
                select StationId, VendorProductTempSn,VendorFieldCode , count(*) Num  
                from nio_push_collection t1
                where t1.CreatedOn >= '{query.BeginDate.ToString("yyyy-MM-dd HH:mm:ss")}'
                and t1.CreatedOn < '{query.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}'
                and t1.IsDeleted = 0
                {stationSql}
                group by StationId, VendorProductTempSn, VendorFieldCode
                having count(*) > 1
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushCollectionRepeatView>(sql, query);
        }

        /// <summary>
        /// 获取指定条码+工序List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushCollectionSfcView>> GetEntitiesBySfcAsync(NioPushCollectionSfcQuery query)
        {
            if(query.SfcList == null || query.SfcList.Count == 0)
            {
                return new List<NioPushCollectionSfcView>();
            }

            string sql = $@"
                select Id, StationId, VendorProductTempSn ,VendorFieldCode 
                from nio_push_collection npc 
                where VendorProductTempSn in @SfcList
                and StationId in @ProcedureList
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushCollectionSfcView>(sql, query);
        }

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NioPushCollectionEntity>> GetNgEntitiesAsync(NioPushCollectionQuery query)
        {
            string sql = $@"
                select * from nio_push_collection npc 
                where IsOk  = 0
                and id > {query.WaterId}
                and IsDeleted = 0
                order by id asc
                limit 0,{query.Num}
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<NioPushCollectionEntity>(sql, query);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class NioPushCollectionRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM nio_push_collection t1 /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM nio_push_collection t1 /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM nio_push_collection /**where**/  ";

        const string InsertSql = "INSERT INTO nio_push_collection(  `Id`, `NioPushId`, `PlantId`, `WorkshopId`, `ProductionLineId`, `StationId`, `VendorFieldCode`, `VendorProductNum`, `VendorProductName`, `VendorProductSn`, `VendorProductTempSn`, `ProcessType`, `DecimalValue`, `StringValue`, `BooleanValue`, `NioProductNum`, `NioProductName`, `OperatorAccount`, `InputTime`, `OutputTime`, `StationStatus`, `VendorStationStatus`, `VendorValueStatus`, `Owner`, `DataType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsOk`) VALUES (  @Id, @NioPushId, @PlantId, @WorkshopId, @ProductionLineId, @StationId, @VendorFieldCode, @VendorProductNum, @VendorProductName, @VendorProductSn, @VendorProductTempSn, @ProcessType, @DecimalValue, @StringValue, @BooleanValue, @NioProductNum, @NioProductName, @OperatorAccount, @InputTime, @OutputTime, @StationStatus, @VendorStationStatus, @VendorValueStatus, @Owner, @DataType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsOk) ";
        const string InsertsSql = "INSERT INTO nio_push_collection(  `Id`, `NioPushId`, `PlantId`, `WorkshopId`, `ProductionLineId`, `StationId`, `VendorFieldCode`, `VendorProductNum`, `VendorProductName`, `VendorProductSn`, `VendorProductTempSn`, `ProcessType`, `DecimalValue`, `StringValue`, `BooleanValue`, `NioProductNum`, `NioProductName`, `OperatorAccount`, `InputTime`, `OutputTime`, `StationStatus`, `VendorStationStatus`, `VendorValueStatus`, `Owner`, `DataType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsOk`) VALUES (  @Id, @NioPushId, @PlantId, @WorkshopId, @ProductionLineId, @StationId, @VendorFieldCode, @VendorProductNum, @VendorProductName, @VendorProductSn, @VendorProductTempSn, @ProcessType, @DecimalValue, @StringValue, @BooleanValue, @NioProductNum, @NioProductName, @OperatorAccount, @InputTime, @OutputTime, @StationStatus, @VendorStationStatus, @VendorValueStatus, @Owner, @DataType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsOk) ";
        const string InsertSqlInsert = "INSERT INTO nio_push_collection(  `Id`, `NioPushId`, `PlantId`, `WorkshopId`, `ProductionLineId`, `StationId`, `VendorFieldCode`, `VendorProductNum`, `VendorProductName`, `VendorProductSn`, `VendorProductTempSn`, `ProcessType`, `DecimalValue`, `StringValue`, `BooleanValue`, `NioProductNum`, `NioProductName`, `OperatorAccount`, `InputTime`, `OutputTime`, `StationStatus`, `VendorStationStatus`, `VendorValueStatus`, `Owner`, `DataType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsOk`) VALUES ";
        const string InsertSqlValue = "(  @Id, @NioPushId, @PlantId, @WorkshopId, @ProductionLineId, @StationId, @VendorFieldCode, @VendorProductNum, @VendorProductName, @VendorProductSn, @VendorProductTempSn, @ProcessType, @DecimalValue, @StringValue, @BooleanValue, @NioProductNum, @NioProductName, @OperatorAccount, @InputTime, @OutputTime, @StationStatus, @VendorStationStatus, @VendorValueStatus, @Owner, @DataType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsOk) ";

        const string UpdateSql = "UPDATE nio_push_collection SET   NioPushId = @NioPushId, PlantId = @PlantId, WorkshopId = @WorkshopId, ProductionLineId = @ProductionLineId, StationId = @StationId, VendorFieldCode = @VendorFieldCode, VendorProductNum = @VendorProductNum, VendorProductName = @VendorProductName, VendorProductSn = @VendorProductSn, VendorProductTempSn = @VendorProductTempSn, ProcessType = @ProcessType, DecimalValue = @DecimalValue, StringValue = @StringValue, BooleanValue = @BooleanValue, NioProductNum = @NioProductNum, NioProductName = @NioProductName, OperatorAccount = @OperatorAccount, InputTime = @InputTime, OutputTime = @OutputTime, StationStatus = @StationStatus, VendorStationStatus = @VendorStationStatus, VendorValueStatus = @VendorValueStatus, Owner = @Owner, IsOk=@IsOk, DataType = @DataType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE nio_push_collection SET   NioPushId = @NioPushId, PlantId = @PlantId, WorkshopId = @WorkshopId, ProductionLineId = @ProductionLineId, StationId = @StationId, VendorFieldCode = @VendorFieldCode, VendorProductNum = @VendorProductNum, VendorProductName = @VendorProductName, VendorProductSn = @VendorProductSn, VendorProductTempSn = @VendorProductTempSn, ProcessType = @ProcessType, DecimalValue = @DecimalValue, StringValue = @StringValue, BooleanValue = @BooleanValue, NioProductNum = @NioProductNum, NioProductName = @NioProductName, OperatorAccount = @OperatorAccount, InputTime = @InputTime, OutputTime = @OutputTime, StationStatus = @StationStatus, VendorStationStatus = @VendorStationStatus, VendorValueStatus = @VendorValueStatus, Owner = @Owner,IsOk=@IsOk, DataType = @DataType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE nio_push_collection SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE nio_push_collection SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM nio_push_collection WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM nio_push_collection WHERE Id IN @Ids ";

        const string GetByNioPushIdSql = @"SELECT * FROM nio_push_collection WHERE NioPushId = @NioPushId and IsDeleted = 0";
    }
}
