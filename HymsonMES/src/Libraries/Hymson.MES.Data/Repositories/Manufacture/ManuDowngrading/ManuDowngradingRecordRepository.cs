/*
 *creator: Karl
 *
 *describe: 降级品录入记录 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
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
    /// 降级品录入记录仓储
    /// </summary>
    public partial class ManuDowngradingRecordRepository :BaseRepository, IManuDowngradingRecordRepository
    {

        public ManuDowngradingRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuDowngradingRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuDowngradingRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuDowngradingRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// 不是模糊查询
        /// </summary>
        /// <param name="manuDowngradingRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingRecordEntity>> GetPagedInfoAsync(ManuDowngradingRecordPagedQuery manuDowngradingRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(manuDowngradingRecordPagedQuery.SFC))
            {
                sqlBuilder.Where("SFC=@SFC");
            }
            if (!string.IsNullOrWhiteSpace(manuDowngradingRecordPagedQuery.Grade))
            {
                sqlBuilder.Where("Grade=@Grade");
            }

            var offSet = (manuDowngradingRecordPagedQuery.PageIndex - 1) * manuDowngradingRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuDowngradingRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuDowngradingRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var manuDowngradingRecordEntitiesTask = conn.QueryAsync<ManuDowngradingRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuDowngradingRecordEntities = await manuDowngradingRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuDowngradingRecordEntity>(manuDowngradingRecordEntities, manuDowngradingRecordPagedQuery.PageIndex, manuDowngradingRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuDowngradingRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingRecordEntity>> GetManuDowngradingRecordEntitiesAsync(ManuDowngradingRecordQuery manuDowngradingRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuDowngradingRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuDowngradingRecordEntities = await conn.QueryAsync<ManuDowngradingRecordEntity>(template.RawSql, manuDowngradingRecordQuery);
            return manuDowngradingRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuDowngradingRecordEntity manuDowngradingRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuDowngradingRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuDowngradingRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuDowngradingRecordEntity>? manuDowngradingRecordEntitys)
        {
            if (manuDowngradingRecordEntitys == null || !manuDowngradingRecordEntitys.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuDowngradingRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuDowngradingRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuDowngradingRecordEntity manuDowngradingRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuDowngradingRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuDowngradingRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuDowngradingRecordEntity> manuDowngradingRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuDowngradingRecordEntitys);
        }
        #endregion

    }

    public partial class ManuDowngradingRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_downgrading_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_downgrading_record` /**where**/ ";
        const string GetManuDowngradingRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_downgrading_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_downgrading_record`(  `Id`, `SiteId`, `SFC`, `Grade`, `IsCancellation`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, Remark) VALUES (   @Id, @SiteId, @SFC, @Grade, @IsCancellation, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted , @Remark)  ";
        const string InsertsSql = "INSERT INTO `manu_downgrading_record`(  `Id`, `SiteId`, `SFC`, `Grade`, `IsCancellation`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, Remark) VALUES (   @Id, @SiteId, @SFC, @Grade, @IsCancellation, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark )  ";

        const string UpdateSql = "UPDATE `manu_downgrading_record` SET   SiteId = @SiteId, SFC = @SFC, Grade = @Grade, IsCancellation = @IsCancellation, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark=@Remark  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_downgrading_record` SET   SiteId = @SiteId, SFC = @SFC, Grade = @Grade, IsCancellation = @IsCancellation, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted ,Remark=@Remark  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_downgrading_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_downgrading_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SFC`, `Grade`, `IsCancellation`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, Remark
                            FROM `manu_downgrading_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SFC`, `Grade`, `IsCancellation`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, Remark
                            FROM `manu_downgrading_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
