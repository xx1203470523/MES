/*
 *creator: Karl
 *
 *describe: 条码信息表 仓储类 | 代码由框架生成
 *builder:  wangkeming
 *build datetime: 2023-04-11 02:42:47
 */

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
        private readonly ConnectionOptions _connectionOptions;
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
        public async Task<ManuSfcInfo1Entity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcInfo1Entity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfo1Entity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcInfo1Entity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcInfo1PagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcInfo1Entity>> GetPagedInfoAsync(ManuSfcInfo1PagedQuery manuSfcInfo1PagedQuery)
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
            var manuSfcInfo1EntitiesTask = conn.QueryAsync<ManuSfcInfo1Entity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcInfo1Entities = await manuSfcInfo1EntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcInfo1Entity>(manuSfcInfo1Entities, manuSfcInfo1PagedQuery.PageIndex, manuSfcInfo1PagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcInfo1Query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcInfo1Entity>> GetManuSfcInfo1EntitiesAsync(ManuSfcInfo1Query manuSfcInfo1Query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfo1EntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcInfo1Entities = await conn.QueryAsync<ManuSfcInfo1Entity>(template.RawSql, manuSfcInfo1Query);
            return manuSfcInfo1Entities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcInfo1Entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcInfo1Entity manuSfcInfo1Entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcInfo1Entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcInfo1Entitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcInfo1Entity> manuSfcInfo1Entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcInfo1Entitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcInfo1Entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcInfo1Entity manuSfcInfo1Entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcInfo1Entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcInfo1Entitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcInfo1Entity> manuSfcInfo1Entitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcInfo1Entitys);
        }
        #endregion
    }

    public partial class ManuSfcInfoRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_info1` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_info1` /**where**/ ";
        const string GetManuSfcInfo1EntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_info1` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_info1`(  `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcId, @WorkOrderId, @ProductId, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_info1`(  `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcId, @WorkOrderId, @ProductId, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_info1` SET   SiteId = @SiteId, SfcId = @SfcId, WorkOrderId = @WorkOrderId, ProductId = @ProductId, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_info1` SET   SiteId = @SiteId, SfcId = @SfcId, WorkOrderId = @WorkOrderId, ProductId = @ProductId, IsUsed = @IsUsed, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_info1` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_info1` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_info1`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SfcId`, `WorkOrderId`, `ProductId`, `IsUsed`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_info1`  WHERE Id IN @Ids ";
        #endregion
    }
}
