/*
 *creator: Karl
 *
 *describe: 产品不良录入 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 产品不良录入仓储
    /// </summary>
    public partial class ManuProductBadRecordRepository : IManuProductBadRecordRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ManuProductBadRecordRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuProductBadRecordEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuProductBadRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordView>> GetBadRecordsBySfcAsync(ManuProductBadRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
             sqlBuilder.Where("br.SiteId = @SiteId");
            sqlBuilder.Where("br.IsDeleted =0");

            sqlBuilder.Select("uc.UnqualifiedCode,uc.UnqualifiedCodeName,br.UnqualifiedId,pr.ResCode,pr.ResName,uc.ProcessRouteId");
            sqlBuilder.LeftJoin("qual_unqualified_code uc on br.UnqualifiedId =uc.Id");
            sqlBuilder.LeftJoin("proc_resource pr on pr.Id =br.FoundBadResourceId");

            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                sqlBuilder.Where("br.SFC=@SFC");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("br.Status =@Status");
            }
            if (query.Type.HasValue)
            {
                sqlBuilder.Where("uc.Type =@Type");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntities = await conn.QueryAsync<ManuProductBadRecordView>(template.RawSql, query);
            return manuSfcProduceEntities;
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuProductBadRecordEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuProductBadRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordEntity>> GetPagedInfoAsync(ManuProductBadRecordPagedQuery manuProductBadRecordPagedQuery)
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
           
            var offSet = (manuProductBadRecordPagedQuery.PageIndex - 1) * manuProductBadRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuProductBadRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuProductBadRecordPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuProductBadRecordEntitiesTask = conn.QueryAsync<ManuProductBadRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuProductBadRecordEntities = await manuProductBadRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuProductBadRecordEntity>(manuProductBadRecordEntities, manuProductBadRecordPagedQuery.PageIndex, manuProductBadRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuProductBadRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordEntity>> GetManuProductBadRecordEntitiesAsync(ManuProductBadRecordQuery manuProductBadRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuProductBadRecordEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuProductBadRecordEntities = await conn.QueryAsync<ManuProductBadRecordEntity>(template.RawSql, manuProductBadRecordQuery);
            return manuProductBadRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuProductBadRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuProductBadRecordEntity manuProductBadRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuProductBadRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuProductBadRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuProductBadRecordEntity> manuProductBadRecordEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuProductBadRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuProductBadRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuProductBadRecordEntity manuProductBadRecordEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuProductBadRecordEntity);
        }
		
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuProductBadRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuProductBadRecordEntity> manuProductBadRecordEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuProductBadRecordEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql,command);
        }
    }

    public partial class ManuProductBadRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_product_bad_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_product_bad_record` /**where**/ ";
        const string GetManuProductBadRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_product_bad_record` /**where**/  ";

        const string GetEntitiesSqlTemplate = @"SELECT /**select**/  FROM `manu_product_bad_record` br  /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string InsertSql = "INSERT INTO `manu_product_bad_record`(  `Id`, `SiteId`, `FoundBadOperationId`, `FoundBadResourceId`,`OutflowOperationId`, `UnqualifiedId`, `SFC`, `Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FoundBadOperationId,@FoundBadResourceId, @OutflowOperationId, @UnqualifiedId, @SFC, @Qty, @Status, @Source, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_product_bad_record` SET   SiteId = @SiteId, FoundBadOperationId = @FoundBadOperationId, OutflowOperationId = @OutflowOperationId, UnqualifiedId = @UnqualifiedId, SFC = @SFC, Qty = @Qty, Status = @Status, Source = @Source, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `manu_product_bad_record` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `FoundBadOperationId`, `OutflowOperationId`, `UnqualifiedId`, `SFC`, `Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_product_bad_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FoundBadOperationId`, `OutflowOperationId`, `UnqualifiedId`, `SFC`, `Qty`, `Status`, `Source`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_product_bad_record`  WHERE Id IN @ids ";
    }
}
