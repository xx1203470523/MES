using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载记录仓储
    /// </summary>
    public partial class InteVehicleFreightRecordRepository : BaseRepository, IInteVehicleFreightRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteVehicleFreightRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleFreightRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleFreightRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleFreightRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleFreightRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleFreightRecordEntity>> GetPagedInfoAsync(InteVehicleFreightRecordPagedQuery inteVehicleFreightRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            var offSet = (inteVehicleFreightRecordPagedQuery.PageIndex - 1) * inteVehicleFreightRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehicleFreightRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehicleFreightRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehicleFreightRecordEntitiesTask = conn.QueryAsync<InteVehicleFreightRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehicleFreightRecordEntities = await inteVehicleFreightRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleFreightRecordEntity>(inteVehicleFreightRecordEntities, inteVehicleFreightRecordPagedQuery.PageIndex, inteVehicleFreightRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehicleFreightRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightRecordEntity>> GetInteVehicleFreightRecordEntitiesAsync(InteVehicleFreightRecordQuery inteVehicleFreightRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleFreightRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehicleFreightRecordEntities = await conn.QueryAsync<InteVehicleFreightRecordEntity>(template.RawSql, inteVehicleFreightRecordQuery);
            return inteVehicleFreightRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleFreightRecordEntity inteVehicleFreightRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehicleFreightRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleFreightRecordEntity> inteVehicleFreightRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehicleFreightRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleFreightRecordEntity inteVehicleFreightRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehicleFreightRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehicleFreightRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleFreightRecordEntity> inteVehicleFreightRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehicleFreightRecordEntitys);
        }
        #endregion

    }

    public partial class InteVehicleFreightRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_freight_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehicle_freight_record` /**where**/ ";
        const string GetInteVehicleFreightRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehicle_freight_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle_freight_record`(  `Id`, `SiteId`, `VehicleId`, `LocationId`, `BarCode`, `OperateType`,`CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @VehicleId, @LocationId, @BarCode, @OperateType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle_freight_record`(  `Id`, `SiteId`, `VehicleId`, `LocationId`, `BarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @SiteId, @VehicleId, @LocationId, @BarCode, @OperateType,@CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn )  ";

        const string UpdateSql = "UPDATE `inte_vehicle_freight_record` SET   SiteId = @SiteId, VehicleId = @VehicleId, LocationId = @LocationId, BarCode = @BarCode, OperateType = @OperateType, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle_freight_record` SET   SiteId = @SiteId, VehicleId = @VehicleId, LocationId = @LocationId, BarCode = @BarCode, OperateType = @OperateType, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_vehicle_freight_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_vehicle_freight_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `VehicleId`, `LocationId`, `BarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `inte_vehicle_freight_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `VehicleId`, `LocationId`, `BarCode`, `OperateType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`
                            FROM `inte_vehicle_freight_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
