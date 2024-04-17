/*
 *creator: Karl
 *
 *describe: 载具校验 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-17 09:34:37
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具校验仓储
    /// </summary>
    public partial class InteVehicleVerifyRepository :BaseRepository, IInteVehicleVerifyRepository
    {

        public InteVehicleVerifyRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<InteVehicleVerifyEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleVerifyEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleVerifyEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleVerifyEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleVerifyPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleVerifyEntity>> GetPagedInfoAsync(InteVehicleVerifyPagedQuery inteVehicleVerifyPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (inteVehicleVerifyPagedQuery.PageIndex - 1) * inteVehicleVerifyPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehicleVerifyPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehicleVerifyPagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehicleVerifyEntitiesTask = conn.QueryAsync<InteVehicleVerifyEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehicleVerifyEntities = await inteVehicleVerifyEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleVerifyEntity>(inteVehicleVerifyEntities, inteVehicleVerifyPagedQuery.PageIndex, inteVehicleVerifyPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehicleVerifyQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleVerifyEntity>> GetInteVehicleVerifyEntitiesAsync(InteVehicleVerifyQuery inteVehicleVerifyQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleVerifyEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehicleVerifyEntities = await conn.QueryAsync<InteVehicleVerifyEntity>(template.RawSql, inteVehicleVerifyQuery);
            return inteVehicleVerifyEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleVerifyEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleVerifyEntity inteVehicleVerifyEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehicleVerifyEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleVerifyEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleVerifyEntity> inteVehicleVerifyEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehicleVerifyEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleVerifyEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleVerifyEntity inteVehicleVerifyEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehicleVerifyEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehicleVerifyEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleVerifyEntity> inteVehicleVerifyEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehicleVerifyEntitys);
        }
        #endregion

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

        /// <summary>
        ///获取数据 根据vehicleId
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public async Task<InteVehicleVerifyEntity> GetByVehicleIdAsync(long vehicleId) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleVerifyEntity>(GetByVehicleIdSql, new { VehicleId = vehicleId });
        }
    }

    public partial class InteVehicleVerifyRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_verify` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehicle_verify` /**where**/ ";
        const string GetInteVehicleVerifyEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehicle_verify` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle_verify`(  `Id`, `SiteId`, `VehicleId`, `ExpirationDate`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @VehicleId, @ExpirationDate, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle_verify`(  `Id`, `SiteId`, `VehicleId`, `ExpirationDate`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @VehicleId, @ExpirationDate, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `inte_vehicle_verify` SET   SiteId = @SiteId, VehicleId = @VehicleId, ExpirationDate = @ExpirationDate, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle_verify` SET   SiteId = @SiteId, VehicleId = @VehicleId, ExpirationDate = @ExpirationDate, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_vehicle_verify` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_vehicle_verify` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `VehicleId`, `ExpirationDate`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_verify`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `VehicleId`, `ExpirationDate`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_verify`  WHERE Id IN @Ids ";
        #endregion

        const string DeletesTrueByVehicleIdsSql = "DELETE From `inte_vehicle_verify` WHERE  VehicleId in @VehicleIds";
        const string GetByVehicleIdSql = "SELECT * from `inte_vehicle_verify` WHERE VehicleId = @VehicleId and IsDeleted=0 ";
    }
}
