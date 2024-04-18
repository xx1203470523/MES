/*
 *creator: Karl
 *
 *describe: 烘烤执行表 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:33
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 烘烤执行表仓储
    /// </summary>
    public partial class ManuBakingRecordRepository :BaseRepository, IManuBakingRecordRepository
    {

        public ManuBakingRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuBakingRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuBakingRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBakingRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuBakingRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuBakingRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuBakingRecordEntity>> GetPagedInfoAsync(ManuBakingRecordPagedQuery manuBakingRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

           
            var offSet = (manuBakingRecordPagedQuery.PageIndex - 1) * manuBakingRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuBakingRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuBakingRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var manuBakingRecordEntitiesTask = conn.QueryAsync<ManuBakingRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuBakingRecordEntities = await manuBakingRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuBakingRecordEntity>(manuBakingRecordEntities, manuBakingRecordPagedQuery.PageIndex, manuBakingRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuBakingRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBakingRecordEntity>> GetManuBakingRecordEntitiesAsync(ManuBakingRecordQuery manuBakingRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuBakingRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuBakingRecordEntities = await conn.QueryAsync<ManuBakingRecordEntity>(template.RawSql, manuBakingRecordQuery);
            return manuBakingRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuBakingRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuBakingRecordEntity manuBakingRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuBakingRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuBakingRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuBakingRecordEntity> manuBakingRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuBakingRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuBakingRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuBakingRecordEntity manuBakingRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuBakingRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuBakingRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuBakingRecordEntity> manuBakingRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuBakingRecordEntitys);
        }
        #endregion

    }

    public partial class ManuBakingRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_baking_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_baking_record` /**where**/ ";
        const string GetManuBakingRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_baking_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_baking_record`(  `Id`, `SiteId`, `BakingId`, `Location`, `BakingStart`, `BakingEnd`, `BakingPlan`, `BakingExecution`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @BakingId, @Location, @BakingStart, @BakingEnd, @BakingPlan, @BakingExecution, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_baking_record`(  `Id`, `SiteId`, `BakingId`, `Location`, `BakingStart`, `BakingEnd`, `BakingPlan`, `BakingExecution`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @BakingId, @Location, @BakingStart, @BakingEnd, @BakingPlan, @BakingExecution, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_baking_record` SET   SiteId = @SiteId, BakingId = @BakingId, Location = @Location, BakingStart = @BakingStart, BakingEnd = @BakingEnd, BakingPlan = @BakingPlan, BakingExecution = @BakingExecution, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_baking_record` SET   SiteId = @SiteId, BakingId = @BakingId, Location = @Location, BakingStart = @BakingStart, BakingEnd = @BakingEnd, BakingPlan = @BakingPlan, BakingExecution = @BakingExecution, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_baking_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_baking_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `BakingId`, `Location`, `BakingStart`, `BakingEnd`, `BakingPlan`, `BakingExecution`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_baking_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `BakingId`, `Location`, `BakingStart`, `BakingEnd`, `BakingPlan`, `BakingExecution`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_baking_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
