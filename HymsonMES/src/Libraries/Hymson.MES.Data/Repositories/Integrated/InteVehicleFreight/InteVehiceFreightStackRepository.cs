using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.InteVehicleFreight.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 二维载具条码明细仓储
    /// </summary>
    public partial class InteVehiceFreightStackRepository : BaseRepository, IInteVehiceFreightStackRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteVehiceFreightStackRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据载具Id批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteByVehicleIdsAsync(long[] vehicleIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByVehicleIdsSql, new { VehicleIds = vehicleIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleFreightStackEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleFreightStackEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightStackEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleFreightStackEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehiceFreightStackPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleFreightStackEntity>> GetPagedInfoAsync(InteVehiceFreightStackPagedQuery inteVehiceFreightStackPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");


            var offSet = (inteVehiceFreightStackPagedQuery.PageIndex - 1) * inteVehiceFreightStackPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehiceFreightStackPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehiceFreightStackPagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehiceFreightStackEntitiesTask = conn.QueryAsync<InteVehicleFreightStackEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehiceFreightStackEntities = await inteVehiceFreightStackEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleFreightStackEntity>(inteVehiceFreightStackEntities, inteVehiceFreightStackPagedQuery.PageIndex, inteVehiceFreightStackPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightStackEntity>> GetEntitiesAsync(EntityByParentIdsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("VehicleId IN @ParentIds");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleFreightStackEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehiceFreightStackQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightStackEntity>> GetInteVehiceFreightStackEntitiesAsync(InteVehiceFreightStackQuery inteVehiceFreightStackQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            if (inteVehiceFreightStackQuery.VehicleId != null)
            {
                sqlBuilder.Where("VehicleId = @VehicleId");
            }
            if (inteVehiceFreightStackQuery.LocationId != null)
            {
                sqlBuilder.Where("LocationId = @LocationId");
            }
            if (inteVehiceFreightStackQuery.Sfcs != null && inteVehiceFreightStackQuery.Sfcs.Any())
            {
                sqlBuilder.Where("BarCode IN @Sfcs");
            }
            using var conn = GetMESDbConnection();
            var inteVehiceFreightStackEntities = await conn.QueryAsync<InteVehicleFreightStackEntity>(template.RawSql, inteVehiceFreightStackQuery);
            return inteVehiceFreightStackEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleFreightStackEntity inteVehiceFreightStackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehiceFreightStackEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleFreightStackEntity> inteVehiceFreightStackEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehiceFreightStackEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehiceFreightStackEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleFreightStackEntity inteVehiceFreightStackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehiceFreightStackEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehiceFreightStackEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleFreightStackEntity> inteVehiceFreightStackEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehiceFreightStackEntitys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inteVehiceFreightStackQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightStackEntity>> GetInteVehiceFreightStackEntitiesAsync(InteVehiceFreightStackQueryByLocation inteVehiceFreightStackQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehiceFreightStackEntities = await conn.QueryAsync<InteVehicleFreightStackEntity>(template.RawSql, inteVehiceFreightStackQuery);
            return inteVehiceFreightStackEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<InteVehicleFreightStackEntity> GetBySFCAsync(InteVehiceFreightStackBySfcQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleFreightStackEntity>(GetBySFCSql,query);
        }
        #endregion

        #region 顷刻

        /// <summary>
        /// 查询已绑定条码列表
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightStackEntity>> GetBySfcListAsync(InteVehiceSfcListQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleFreightStackEntity>(GetBySfcListSql, query);
        }

        /// <summary>
        /// 根据托盘id和条码进行删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteByVehiceBarCode(List<UpdateVehicleFreightStackCommand> commandList)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByVehicleIdBarCodeSql, commandList);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteVehiceFreightStackRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_freight_stack` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehicle_freight_stack` /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_freight_stack` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle_freight_stack`(  `Id`, `SiteId`, `LocationId`,`VehicleId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @LocationId,@VehicleId, @BarCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle_freight_stack`(  `Id`, `SiteId`, `LocationId`,`VehicleId`, `BarCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @LocationId,@VehicleId, @BarCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";

        const string UpdateSql = "UPDATE `inte_vehicle_freight_stack` SET   SiteId = @SiteId, LocationId = @LocationId, BarCode = @BarCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle_freight_stack` SET   SiteId = @SiteId, LocationId = @LocationId, BarCode = @BarCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "DELETE from `inte_vehicle_freight_stack`  WHERE Id = @Id ";
        const string DeletesSql = "DELETE from `inte_vehicle_freight_stack`  WHERE Id IN @Ids";
        const string DeleteByVehicleIdsSql = "DELETE from `inte_vehicle_freight_stack`  WHERE VehicleId IN @VehicleIds";

        const string GetByIdSql = @"SELECT * FROM `inte_vehicle_freight_stack`  WHERE Id = @Id ";
        const string GetBySFCSql = @"SELECT * FROM `inte_vehicle_freight_stack`  WHERE SiteId=@SiteId and BarCode = @BarCode ";
        const string GetByIdsSql = @"SELECT * FROM `inte_vehicle_freight_stack`  WHERE Id IN @Ids ";
        #endregion

        #region 顷刻

        /// <summary>
        /// 获取已绑定条码列表
        /// </summary>
        const string GetBySfcListSql = @"SELECT * FROM `inte_vehicle_freight_stack`  WHERE SiteId=@SiteId and BarCode IN @SfcList ";

        /// <summary>
        /// 根据载具id和电芯条码进行删除
        /// </summary>
        const string DeleteByVehicleIdBarCodeSql = "DELETE from `inte_vehicle_freight_stack`  WHERE VehicleId = @VehicleId AND BarCode = @BarCode";

        #endregion
    }
}
