using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>s
    /// 条码信息表仓储
    /// </summary>
    public partial class ManuSfcInfoRepository : BaseRepository, IManuSfcInfoRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSfcInfoRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
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
        public async Task<ManuSfcInfoEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoEntity> GetBySFCAsync(string sfc)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfoEntity>(GetBySFCSql, new { sfc });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetBySFCIdsAsync(IEnumerable<long> sfcIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetBySFCIdsSql, new { sfcIds });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcInfoEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfo1PagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcInfoEntity>> GetPagedInfoAsync(ManuSfcInfo1PagedQuery manuSfcInfo1PagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            var offSet = (manuSfcInfo1PagedQuery.PageIndex - 1) * manuSfcInfo1PagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcInfo1PagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcInfo1PagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcInfo1EntitiesTask = conn.QueryAsync<ManuSfcInfoEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcInfo1Entities = await manuSfcInfo1EntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcInfoEntity>(manuSfcInfo1Entities, manuSfcInfo1PagedQuery.PageIndex, manuSfcInfo1PagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfoEntity>> GetManuSfcInfo1EntitiesAsync(ManuSfcInfo1Query manuSfcInfo1Query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfo1EntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcInfo1Entities = await conn.QueryAsync<ManuSfcInfoEntity>(template.RawSql, manuSfcInfo1Query);
            return manuSfcInfo1Entities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="ManuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcInfoEntity ManuSfcInfoEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, ManuSfcInfoEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="ManuSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcInfoEntity> ManuSfcInfoEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, ManuSfcInfoEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ManuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcInfoEntity ManuSfcInfoEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, ManuSfcInfoEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="ManuSfcInfoEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcInfoEntity> ManuSfcInfoEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, ManuSfcInfoEntitys);
        }

        /// <summary>
        /// 批量更新 是否在用
        /// </summary>
        /// <param name="sfcIds"></param>
        /// <returns></returns>
        public async Task<int> UpdatesIsUsedAsync(ManuSfcInfoUpdateCommand manuSfcInfoUpdate)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesIsUsedSql, manuSfcInfoUpdate);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcInfoRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_info` /**where**/ ";
        const string GetManuSfcInfo1EntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_info` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcId, @WorkOrderId, @ProductId, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_info`(  `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcId, @WorkOrderId, @ProductId, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_info` SET   SiteId = @SiteId, SfcId = @SfcId, WorkOrderId = @WorkOrderId, ProductId = @ProductId, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_info` SET   SiteId = @SiteId, SfcId = @SfcId, WorkOrderId = @WorkOrderId, ProductId = @ProductId, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_info` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_info` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND Id IN @ids ";
        const string GetBySFCSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND SFC = @sfc ";
        const string GetBySFCIdsSql = @"SELECT * FROM manu_sfc_info WHERE IsDeleted = 0 AND SfcId IN @sfcIds ";

        const string UpdatesIsUsedSql = "UPDATE `manu_sfc_info` SET  IsUsed = @IsUsed, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE SfcId IN @SfcIds";

        #endregion
    }
}
