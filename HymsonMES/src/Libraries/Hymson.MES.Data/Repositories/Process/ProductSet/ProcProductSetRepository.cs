/*
 *creator: Karl
 *
 *describe: 工序和资源半成品产品设置表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-05 11:16:51
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序和资源半成品产品设置表仓储
    /// </summary>
    public partial class ProcProductSetRepository : BaseRepository, IProcProductSetRepository
    {

        public ProcProductSetRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ProcProductSetEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProductSetEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductSetEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProductSetEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据资源/工序ID或者产品ID批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ProcProductSetEntity> GetByProcedureIdAndProductIdAsync(GetByProcedureIdAndProductIdQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProductSetEntity>(GetByProcedureIdAndProductIdSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProductSetPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProductSetEntity>> GetPagedInfoAsync(ProcProductSetPagedQuery procProductSetPagedQuery)
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

            var offSet = (procProductSetPagedQuery.PageIndex - 1) * procProductSetPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procProductSetPagedQuery.PageSize });
            sqlBuilder.AddParameters(procProductSetPagedQuery);

            using var conn = GetMESDbConnection();
            var procProductSetEntitiesTask = conn.QueryAsync<ProcProductSetEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProductSetEntities = await procProductSetEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProductSetEntity>(procProductSetEntities, procProductSetPagedQuery.PageIndex, procProductSetPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProductSetQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductSetEntity>> GetProcProductSetEntitiesAsync(ProcProductSetQuery procProductSetQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProductSetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procProductSetEntities = await conn.QueryAsync<ProcProductSetEntity>(template.RawSql, procProductSetQuery);
            return procProductSetEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProductSetEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProductSetEntity procProductSetEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procProductSetEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProductSetEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcProductSetEntity> procProductSetEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procProductSetEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProductSetEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProductSetEntity procProductSetEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procProductSetEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProductSetEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcProductSetEntity> procProductSetEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procProductSetEntitys);
        }
        #endregion

    }

    public partial class ProcProductSetRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_product_set ` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_product_set ` /**where**/ ";
        const string GetProcProductSetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_product_set ` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_product_set `(  `Id`, `SiteId`, `CreatedBy`, `CreatedOn`, `ProductId`, `SetPointId`, `SemiProductId`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @CreatedBy, @CreatedOn, @ProductId, @SetPointId, @SemiProductId, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_product_set `(  `Id`, `SiteId`, `CreatedBy`, `CreatedOn`, `ProductId`, `SetPointId`, `SemiProductId`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @CreatedBy, @CreatedOn, @ProductId, @SetPointId, @SemiProductId, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_product_set ` SET   SiteId = @SiteId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, ProductId = @ProductId, SetPointId = @SetPointId, SemiProductId = @SemiProductId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_product_set ` SET   SiteId = @SiteId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, ProductId = @ProductId, SetPointId = @SetPointId, SemiProductId = @SemiProductId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_product_set ` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_product_set ` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `CreatedBy`, `CreatedOn`, `ProductId`, `SetPointId`, `SemiProductId`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_product_set `  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `CreatedBy`, `CreatedOn`, `ProductId`, `SetPointId`, `SemiProductId`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_product_set `  WHERE Id IN @Ids ";


        const string GetByProcedureIdAndProductIdSql = @"SELECT  
                               `Id`, `SiteId`, `CreatedBy`, `CreatedOn`, `ProductId`, `SetPointId`, `SemiProductId`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM  proc_product_set   WHERE   SiteId=@SiteId AND IsDeleted=0 AND SetPointId = @SetPointId AND ProductId=@ProductId";
        #endregion
    }
}
