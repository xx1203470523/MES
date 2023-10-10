/*
 *creator: Karl
 *
 *describe: 降级录入 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 降级录入仓储
    /// </summary>
    public partial class ManuDowngradingRepository :BaseRepository, IManuDowngradingRepository
    {

        public ManuDowngradingRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuDowngradingEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuDowngradingEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuDowngradingEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuDowngradingPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingEntity>> GetPagedInfoAsync(ManuDowngradingPagedQuery manuDowngradingPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
          
            var offSet = (manuDowngradingPagedQuery.PageIndex - 1) * manuDowngradingPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuDowngradingPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuDowngradingPagedQuery);

            using var conn = GetMESDbConnection();
            var manuDowngradingEntitiesTask = conn.QueryAsync<ManuDowngradingEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuDowngradingEntities = await manuDowngradingEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuDowngradingEntity>(manuDowngradingEntities, manuDowngradingPagedQuery.PageIndex, manuDowngradingPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuDowngradingQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingEntity>> GetManuDowngradingEntitiesAsync(ManuDowngradingQuery manuDowngradingQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuDowngradingEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuDowngradingEntities = await conn.QueryAsync<ManuDowngradingEntity>(template.RawSql, manuDowngradingQuery);
            return manuDowngradingEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuDowngradingEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuDowngradingEntity manuDowngradingEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuDowngradingEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuDowngradingEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuDowngradingEntity> manuDowngradingEntitys)
        {
            if (manuDowngradingEntitys == null || manuDowngradingEntitys.Any() == false) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuDowngradingEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuDowngradingEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuDowngradingEntity manuDowngradingEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuDowngradingEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuDowngradingEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuDowngradingEntity> manuDowngradingEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuDowngradingEntitys);
        }
        #endregion

        /// <summary>
        /// 根据sfcs获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingEntity>> GetBySfcsAsync(ManuDowngradingBySfcsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuDowngradingEntity>(GetBySfcsSql, query);
        }

        /// <summary>
        /// 根据sfcs获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingEntity>> GetBySFCsAsync(ManuDowngradingBySFCsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuDowngradingEntity>(GetBySFCsSql, query);
        }

        /// <summary>
        /// 真实删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByIdsSql, new { Ids=ids });
        }
    }

    public partial class ManuDowngradingRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_downgrading` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_downgrading` /**where**/ ";
        const string GetManuDowngradingEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_downgrading` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_downgrading`(  `Id`, `SiteId`, `SFC`, `Grade`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Grade, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_downgrading`(  `Id`, `SiteId`, `SFC`, `Grade`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Grade, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_downgrading` SET   SiteId = @SiteId, SFC = @SFC, Grade = @Grade, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_downgrading` SET  Grade = @Grade, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_downgrading` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_downgrading` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SFC`, `Grade`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_downgrading`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SFC`, `Grade`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_downgrading`  WHERE Id IN @Ids ";
        #endregion

        const string GetBySfcsSql = @"SELECT * FROM  `manu_downgrading` WHERE IsDeleted=0 AND SFC in @Sfcs AND SiteId=@SiteId ";
        const string GetBySFCsSql = @"SELECT * FROM  manu_downgrading WHERE IsDeleted = 0 AND SiteId = @SiteId AND SFC IN @SFCs ";
        const string DeletesTrueByIdsSql = @"DELETE FROM `manu_downgrading` WHERE Id in @Ids ";
    }
}
