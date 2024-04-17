/*
 *creator: Karl
 *
 *describe: 载具装载 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-18 09:52:16
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具装载仓储
    /// </summary>
    public partial class InteVehicleFreightRepository :BaseRepository, IInteVehicleFreightRepository
    {

        public InteVehicleFreightRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<InteVehicleFreightEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleFreightEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleFreightEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleFreightPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleFreightEntity>> GetPagedInfoAsync(InteVehicleFreightPagedQuery inteVehicleFreightPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (inteVehicleFreightPagedQuery.PageIndex - 1) * inteVehicleFreightPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehicleFreightPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehicleFreightPagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehicleFreightEntitiesTask = conn.QueryAsync<InteVehicleFreightEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehicleFreightEntities = await inteVehicleFreightEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleFreightEntity>(inteVehicleFreightEntities, inteVehicleFreightPagedQuery.PageIndex, inteVehicleFreightPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehicleFreightQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightEntity>> GetInteVehicleFreightEntitiesAsync(InteVehicleFreightQuery inteVehicleFreightQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleFreightEntitiesSqlTemplate);
            sqlBuilder.Where("SiteId =@SiteId");

            using var conn = GetMESDbConnection();
            var inteVehicleFreightEntities = await conn.QueryAsync<InteVehicleFreightEntity>(template.RawSql, inteVehicleFreightQuery);
            return inteVehicleFreightEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleFreightEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleFreightEntity inteVehicleFreightEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehicleFreightEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleFreightEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleFreightEntity> inteVehicleFreightEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehicleFreightEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleFreightEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleFreightEntity inteVehicleFreightEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehicleFreightEntity);
        }
        public async Task<int> UpdateQtyAsync(InteVehicleFreightEntity inteVehicleFreightEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQtySql, inteVehicleFreightEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehicleFreightEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleFreightEntity> inteVehicleFreightEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehicleFreightEntitys);
        }
        #endregion

        /// <summary>
        ///获取数据 根据vehicleId
        /// </summary>
        /// <param name="vehicleIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightEntity>> GetByVehicleIdsAsync(long[] vehicleIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleFreightEntity>(GetByVehicleIdsSql, new { VehicleIds = vehicleIds });
        }

        /// <summary>
        /// 批量删除 (硬删除) 根据 VehicleId
        /// </summary>
        /// <param name="vehicleIds"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByVehicleIdsAsync(long[] vehicleIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByVehicleIdsSql, new { VehicleIds = vehicleIds });
        }

    }

    public partial class InteVehicleFreightRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_freight` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehicle_freight` /**where**/ ";
        const string GetInteVehicleFreightEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehicle_freight` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle_freight`(  `Id`, `SiteId`, `VehicleId`, `Qty`, `Row`, `Column`, `Location`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @VehicleId, @Qty, @Row, @Column, @Location,  @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle_freight`(  `Id`, `SiteId`, `VehicleId`, `Qty`, `Row`, `Column`, `Location`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @VehicleId, @Qty, @Row, @Column, @Location, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateQtySql = "UPDATE `inte_vehicle_freight` SET   Qty =@Qty+1, VehicleId = @VehicleId, `Row`=@Row, `Column`=@Column, Location = @Location, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateSql = "UPDATE `inte_vehicle_freight` SET   Qty =@Qty+1, VehicleId = @VehicleId, `Row`=@Row, `Column`=@Column, Location = @Location, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle_freight` SET   Qty =@Qty, VehicleId = @VehicleId, `Row`=@Row, `Column`=@Column, Location = @Location, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_vehicle_freight` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_vehicle_freight` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `VehicleId`, `Qty`, `Row`, `Column`, `Location`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_freight`  WHERE Id = @Id ";

        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `VehicleId`, `Qty`, `Row`, `Column`, `Location`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_freight`  WHERE Id IN @Ids ";
        #endregion

        const string GetByVehicleIdsSql = "SELECT * from `inte_vehicle_freight` WHERE VehicleId in @VehicleIds and IsDeleted=0 ";
        const string DeletesTrueByVehicleIdsSql = "DELETE From `inte_vehicle_freight` WHERE  VehicleId in @VehicleIds";
    }
}
