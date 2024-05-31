/*
 *creator: Karl
 *
 *describe: 载具类型维护 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
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
    /// 载具类型维护仓储
    /// </summary>
    public partial class InteVehicleTypeRepository : BaseRepository, IInteVehicleTypeRepository
    {

        public InteVehicleTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<InteVehicleTypeEntity> GetByCodeAsync(InteVehicleTypeCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleTypeEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteVehicleTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleTypeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteVehicleTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleTypeEntity>> GetPagedInfoAsync(InteVehicleTypePagedQuery inteVehicleTypePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(inteVehicleTypePagedQuery.Code))
            {
                inteVehicleTypePagedQuery.Code = $"%{inteVehicleTypePagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(inteVehicleTypePagedQuery.Name))
            {
                inteVehicleTypePagedQuery.Name = $"%{inteVehicleTypePagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }
            if (inteVehicleTypePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }

            var offSet = (inteVehicleTypePagedQuery.PageIndex - 1) * inteVehicleTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteVehicleTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(inteVehicleTypePagedQuery);

            using var conn = GetMESDbConnection();
            var inteVehicleTypeEntitiesTask = conn.QueryAsync<InteVehicleTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteVehicleTypeEntities = await inteVehicleTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteVehicleTypeEntity>(inteVehicleTypeEntities, inteVehicleTypePagedQuery.PageIndex, inteVehicleTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteVehicleTypeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeEntity>> GetInteVehicleTypeEntitiesAsync(InteVehicleTypeQuery inteVehicleTypeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteVehicleTypeEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var inteVehicleTypeEntities = await conn.QueryAsync<InteVehicleTypeEntity>(template.RawSql, inteVehicleTypeQuery);
            return inteVehicleTypeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteVehicleTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteVehicleTypeEntity inteVehicleTypeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, inteVehicleTypeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteVehicleTypeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteVehicleTypeEntity> inteVehicleTypeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteVehicleTypeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteVehicleTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteVehicleTypeEntity inteVehicleTypeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, inteVehicleTypeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteVehicleTypeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<InteVehicleTypeEntity> inteVehicleTypeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, inteVehicleTypeEntitys);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleTypeEntity>> GetByCodesAsync(InteVehicleTypeNameQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteVehicleTypeEntity>(GetByCodesSql, query);
        }
        #endregion

    }

    public partial class InteVehicleTypeRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_vehicle_type` /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_vehicle_type` /**where**/ ";
        const string GetInteVehicleTypeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_vehicle_type` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_vehicle_type`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Row`, `Column`, CellQty, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Row, @Column, @CellQty, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_vehicle_type`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Row`, `Column`, CellQty, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Row, @Column, @CellQty, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `inte_vehicle_type` SET   `Name` = @Name, `Status` = @Status, `Row` = @Row, `Column` = @Column,`CellQty`=@CellQty, `Remark` = @Remark, `UpdatedBy` = @UpdatedBy, `UpdatedOn` = @UpdatedOn  WHERE `Id` = @Id ";
        const string UpdatesSql = "UPDATE `inte_vehicle_type` SET  `Name` = @Name, `Status` = @Status, `Row` = @Row, `Column` = @Column,`CellQty`=@CellQty, `Remark` = @Remark, `UpdatedBy` = @UpdatedBy, `UpdatedOn` = @UpdatedOn  WHERE `Id` = @Id ";

        const string DeleteSql = "UPDATE `inte_vehicle_type` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_vehicle_type` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `Status`, `Row`, `Column`,CellQty, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_type`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Status`, `Row`, `Column`,CellQty, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_vehicle_type`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT * 
                            FROM `inte_vehicle_type`  WHERE Code = @Code AND IsDeleted=0 AND SiteId=@SiteId ";

        const string GetByCodesSql = @"SELECT * 
                            FROM `inte_vehicle_type`  WHERE Code IN @Codes AND IsDeleted=0 AND SiteId=@SiteId ";
        #endregion
    }
}
