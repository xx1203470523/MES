/*
 *creator: Karl
 *
 *describe: 载具注册表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 载具注册表仓储
    /// </summary>
    public partial class InteVehicleRepository : BaseRepository, IInteVehicleRepository
    {

        public InteVehicleRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleEntity>> GetByCodesAsync(EntityByCodesQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleEntity>(GetByCodesSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleView>> GetPagedInfoAsync(InteVehiclePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("v.IsDeleted=0");
            sqlBuilder.Where("v.SiteId=@SiteId");

            if (query.Ids!=null&&query.Ids.Length>0)
            {
                sqlBuilder.Where("v.Id IN @Ids");
            }

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("v.Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = $"%{query.Name}%";
                sqlBuilder.Where("v.Name LIKE @Name");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("v.Status=@Status");
            }
            if (!string.IsNullOrWhiteSpace(query.VehicleTypeCode))
            {
                query.VehicleTypeCode = $"%{query.VehicleTypeCode}%";
                sqlBuilder.Where("vt.Code LIKE @VehicleTypeCode");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var inteVehicleEntitiesTask = conn.QueryAsync<InteVehicleView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehicleEntities = await inteVehicleEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleView>(inteVehicleEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehicleQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleEntity>> GetInteVehicleEntitiesAsync(InteVehicleQuery inteVehicleQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehicleEntities = await conn.QueryAsync<InteVehicleEntity>(template.RawSql, inteVehicleQuery);
            return inteVehicleEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleEntity inteVehicleEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehicleEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleEntity> inteVehicleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehicleEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleEntity inteVehicleEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehicleEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehicleEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleEntity> inteVehicleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehicleEntitys);
        }
        #endregion

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteVehicleEntity> GetByCodeAsync(InteVehicleCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据VehicleTypeId获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleEntity>> GetByVehicleTypeIdsAsync(InteVehicleVehicleTypeIdsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleEntity>(GetByVehicleTypeIdsSql, query);
        }

        /// <summary>
        /// 根据Ids查询载具信息包含载具类型
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleAboutVehicleTypeView>> GetAboutVehicleTypeByIdsAsync(InteVehicleIdsQuery query) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleAboutVehicleTypeView>(GetAboutVehicleTypeByIdsSql, query);
        }
    }

    public partial class InteVehicleRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                        v.*,vt.Code AS VehicleTypeCode,vt.Name AS VehicleTypeName  
                     FROM `inte_vehicle` v 
                     LEFT JOIN `inte_vehicle_type` vt ON vt.Id=v.VehicleTypeId
                    /**where**/ ORDER BY v.UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(*) 
                     FROM `inte_vehicle` v 
                     LEFT JOIN `inte_vehicle_type` vt ON vt.Id=v.VehicleTypeId 
                    /**where**/  ";
        const string GetInteVehicleEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehicle` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `VehicleTypeId`, `Position`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @VehicleTypeId, @Position, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `VehicleTypeId`, `Position`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @VehicleTypeId, @Position, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `inte_vehicle` SET  Name = @Name, Status = @Status, VehicleTypeId = @VehicleTypeId, Position = @Position, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle` SET   Name = @Name, Status = @Status, VehicleTypeId = @VehicleTypeId, Position = @Position, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `inte_vehicle` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_vehicle` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `Status`, `VehicleTypeId`, `Position`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Status`, `VehicleTypeId`, `Position`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle`  WHERE Id IN @Ids ";
        #endregion

        const string GetByCodeSql = @"SELECT * FROM `inte_vehicle`  WHERE Code = @Code AND IsDeleted=0 AND SiteId=@SiteId ";
        const string GetByCodesSql = @"SELECT * FROM `inte_vehicle` WHERE IsDeleted = 0 AND SiteId = @SiteId AND Code IN @Codes ";
        const string GetByVehicleTypeIdsSql = @"SELECT * FROM `inte_vehicle` WHERE IsDeleted=0 AND SiteId=@SiteId AND VehicleTypeId in @VehicleTypeIds";

        const string GetAboutVehicleTypeByIdsSql = @"SELECT v.*,
                            vt.Code as VehicleTypeCode,
                            vt.Name as VehicleTypeName,
                            vt.Status as VehicleTypeStatus,
                            vt.Row, vt.Column, vt.CellQty
                    FROM `inte_vehicle` v
                    LEFT JOIN inte_vehicle_type vt ON vt.id= v.VehicleTypeId
                    WHERE v.IsDeleted=0 
                    AND v.SiteId=@SiteId 
                    AND v.Id IN @Ids";
    }
}
