/*
 *creator: Karl
 *
 *describe: 条码信息表 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码信息表仓储
    /// </summary>
    public partial class ManuSfcInfoRepository : IManuSfcInfoRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ManuSfcInfoRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            try
            {
                return await conn.ExecuteAsync(DeletesSql, param);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfoPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcInfoEntity>> GetPagedInfoAsync(ManuSfcInfoPagedQuery manuSfcInfoPagedQuery)
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

            var offSet = (manuSfcInfoPagedQuery.PageIndex - 1) * manuSfcInfoPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcInfoPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcInfoPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcInfoEntitiesTask = conn.QueryAsync<ManuSfcInfoEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcInfoEntities = await manuSfcInfoEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcInfoEntity>(manuSfcInfoEntities, manuSfcInfoPagedQuery.PageIndex, manuSfcInfoPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcInfoQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetManuSfcInfoEntitiesAsync(ManuSfcInfoQuery manuSfcInfoQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfoEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcInfoEntities = await conn.QueryAsync<ManuSfcInfoEntity>(template.RawSql, manuSfcInfoQuery);
            return manuSfcInfoEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcInfoEntity manuSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcInfoEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcInfoEntity> manuSfcInfoEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, manuSfcInfoEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcInfoEntity manuSfcInfoEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcInfoEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcInfoEntity> manuSfcInfoEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, manuSfcInfoEntitys);
        }

    }

    public partial class ManuSfcInfoRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_info` /**where**/ ";
        const string GetManuSfcInfoEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_info` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`) VALUES (   @Id, @SFC, @WorkOrderId, @RelevanceWorkOrderId, @ProductId, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`) VALUES (   @Id, @SFC, @WorkOrderId, @RelevanceWorkOrderId, @ProductId, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId )  ";
        const string UpdateSql = "UPDATE `manu_sfc_info` SET   SFC = @SFC, WorkOrderId = @WorkOrderId, RelevanceWorkOrderId = @RelevanceWorkOrderId, ProductId = @ProductId, Qty = @Qty, Status = @Status, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_info` SET   SFC = @SFC, WorkOrderId = @WorkOrderId, RelevanceWorkOrderId = @RelevanceWorkOrderId, ProductId = @ProductId, Qty = @Qty, Status = @Status, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_sfc_info` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_info`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`
                            FROM `manu_sfc_info`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SFC`, `WorkOrderId`, `RelevanceWorkOrderId`, `ProductId`, `Qty`, `Status`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`
                            FROM `manu_sfc_info`  WHERE Id IN @ids ";
    }
}
