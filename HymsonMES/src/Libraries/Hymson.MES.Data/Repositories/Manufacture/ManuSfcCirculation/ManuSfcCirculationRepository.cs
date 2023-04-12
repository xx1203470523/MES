/*
 *creator: Karl
 *
 *describe: 条码流转表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:50:00
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码流转表仓储
    /// </summary>
    public partial class ManuSfcCirculationRepository : IManuSfcCirculationRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ManuSfcCirculationRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcCirculationEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcCirculationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetBySfcAsync(ManuSfcCirculationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted =0");

            if (!string.IsNullOrWhiteSpace(query.Sfc))
            {
                sqlBuilder.Where("SFC=@Sfc");
            }

            if (query.CirculationTypes != null && query.CirculationTypes.Length > 0)
            {
                sqlBuilder.Where("CirculationType in @CirculationTypes");
                sqlBuilder.Where("IsDisassemble!=1");        
            }

            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcCirculationEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcCirculationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcCirculationEntity>> GetPagedInfoAsync(ManuSfcCirculationPagedQuery manuSfcCirculationPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}

            var offSet = (manuSfcCirculationPagedQuery.PageIndex - 1) * manuSfcCirculationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcCirculationPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcCirculationPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcCirculationEntitiesTask = conn.QueryAsync<ManuSfcCirculationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcCirculationEntities = await manuSfcCirculationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcCirculationEntity>(manuSfcCirculationEntities, manuSfcCirculationPagedQuery.PageIndex, manuSfcCirculationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcCirculationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCirculationEntity>> GetManuSfcCirculationEntitiesAsync(ManuSfcCirculationQuery manuSfcCirculationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcCirculationEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcCirculationEntities = await conn.QueryAsync<ManuSfcCirculationEntity>(template.RawSql, manuSfcCirculationQuery);
            return manuSfcCirculationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcCirculationEntity manuSfcCirculationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcCirculationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, manuSfcCirculationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCirculationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcCirculationEntity manuSfcCirculationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcCirculationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcCirculationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcCirculationEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);
        }
    }

    public partial class ManuSfcCirculationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_circulation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_circulation` /**where**/ ";
        const string GetManuSfcCirculationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_circulation` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_circulation`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @FeedingPointId, @SFC, @WorkOrderId, @ProductId, @CirculationBarCode, @CirculationWorkOrderId, @CirculationProductId, @Type, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_circulation`(  `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @ResourceId, @EquipmentId, @FeedingPointId, @SFC, @WorkOrderId, @ProductId, @CirculationBarCode, @CirculationWorkOrderId, @CirculationProductId, @Type, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_sfc_circulation` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, FeedingPointId = @FeedingPointId, SFC = @SFC, WorkOrderId = @WorkOrderId, ProductId = @ProductId, CirculationBarCode = @CirculationBarCode, CirculationWorkOrderId = @CirculationWorkOrderId, CirculationProductId = @CirculationProductId, Type = @Type, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_circulation` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_circulation`  WHERE Id = @Id ";
        const string GetBySfcSql = @"SELECT * FROM manu_sfc_circulation WHERE IsDeleted = 0 AND SFC = @sfc ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `ResourceId`, `EquipmentId`, `FeedingPointId`, `SFC`, `WorkOrderId`, `ProductId`, `CirculationBarCode`, `CirculationWorkOrderId`, `CirculationProductId`, `Type`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_circulation`  WHERE Id IN @ids ";
    }
}
