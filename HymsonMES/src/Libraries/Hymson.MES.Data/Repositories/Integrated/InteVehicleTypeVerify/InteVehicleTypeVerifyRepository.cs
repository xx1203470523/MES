/*
 *creator: Karl
 *
 *describe: 载具类型验证 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-13 03:15:22
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
    /// 载具类型验证仓储
    /// </summary>
    public partial class InteVehicleTypeVerifyRepository :BaseRepository, IInteVehicleTypeVerifyRepository
    {

        public InteVehicleTypeVerifyRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<InteVehicleTypeVerifyEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleTypeVerifyEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeVerifyEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleTypeVerifyEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleTypeVerifyPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleTypeVerifyEntity>> GetPagedInfoAsync(InteVehicleTypeVerifyPagedQuery inteVehicleTypeVerifyPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

           
            var offSet = (inteVehicleTypeVerifyPagedQuery.PageIndex - 1) * inteVehicleTypeVerifyPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehicleTypeVerifyPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehicleTypeVerifyPagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehicleTypeVerifyEntitiesTask = conn.QueryAsync<InteVehicleTypeVerifyEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehicleTypeVerifyEntities = await inteVehicleTypeVerifyEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleTypeVerifyEntity>(inteVehicleTypeVerifyEntities, inteVehicleTypeVerifyPagedQuery.PageIndex, inteVehicleTypeVerifyPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehicleTypeVerifyQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeVerifyEntity>> GetInteVehicleTypeVerifyEntitiesAsync(InteVehicleTypeVerifyQuery inteVehicleTypeVerifyQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleTypeVerifyEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehicleTypeVerifyEntities = await conn.QueryAsync<InteVehicleTypeVerifyEntity>(template.RawSql, inteVehicleTypeVerifyQuery);
            return inteVehicleTypeVerifyEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleTypeVerifyEntity inteVehicleTypeVerifyEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehicleTypeVerifyEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleTypeVerifyEntity> inteVehicleTypeVerifyEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehicleTypeVerifyEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleTypeVerifyEntity inteVehicleTypeVerifyEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehicleTypeVerifyEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehicleTypeVerifyEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleTypeVerifyEntity> inteVehicleTypeVerifyEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehicleTypeVerifyEntitys);
        }
        #endregion


        /// <summary>
        /// 批量删除 (硬删除) 根据 VehicleTypeId
        /// </summary>
        /// <param name="vehicleTypeIds"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByVehicleTypeIdAsync(long[] vehicleTypeIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByVehicleTypeIdSql, new { VehicleTypeIds = vehicleTypeIds });
        }

        /// <summary>
        /// 查询List 获取载具类型验证 根据vehicleTypeId查询
        /// </summary>
        /// <param name="vehicleTypeIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeVerifyEntity>> GetInteVehicleTypeVerifyEntitiesByVehicleTyleIdAsync(long[] vehicleTypeIds)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleTypeVerifyEntitiesByVehicleTypeIdSql);
            using var conn = GetMESDbConnection();
            var inteVehicleTypeVerifyEntities = await conn.QueryAsync<InteVehicleTypeVerifyEntity>(template.RawSql, new { VehicleTypeIds= vehicleTypeIds });
            return inteVehicleTypeVerifyEntities;
        }
    }

    public partial class InteVehicleTypeVerifyRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_type_verify` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehicle_type_verify` /**where**/ ";
        const string GetInteVehicleTypeVerifyEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehicle_type_verify` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle_type_verify`(  `Id`, `SiteId`, `VehicleTypeId`, `Type`, `VerifyId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @VehicleTypeId, @Type, @VerifyId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle_type_verify`(  `Id`, `SiteId`, `VehicleTypeId`, `Type`, `VerifyId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @VehicleTypeId, @Type, @VerifyId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `inte_vehicle_type_verify` SET   SiteId = @SiteId, VehicleTypeId = @VehicleTypeId, Type = @Type, VerifyId = @VerifyId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle_type_verify` SET   SiteId = @SiteId, VehicleTypeId = @VehicleTypeId, Type = @Type, VerifyId = @VerifyId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_vehicle_type_verify` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_vehicle_type_verify` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `VehicleTypeId`, `Type`, `VerifyId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_type_verify`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `VehicleTypeId`, `Type`, `VerifyId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_type_verify`  WHERE Id IN @Ids ";
        #endregion

        const string DeletesTrueByVehicleTypeIdSql = "DELETE From `inte_vehicle_type_verify` WHERE  VehicleTypeId in @VehicleTypeIds";
        const string GetInteVehicleTypeVerifyEntitiesByVehicleTypeIdSql = @"SELECT *   FROM `inte_vehicle_type_verify` WHERE  VehicleTypeId in @VehicleTypeIds   ";
    }
}
