/*
 *creator: Karl
 *
 *describe: 条码绑定解绑记录表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:25
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
    /// <summary>
    /// 条码绑定解绑记录表仓储
    /// </summary>
    public partial class ManuSfcBindRecordRepository :BaseRepository, IManuSfcBindRecordRepository
    {

        public ManuSfcBindRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuSfcBindRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcBindRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcBindRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcBindRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcBindRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcBindRecordEntity>> GetPagedInfoAsync(ManuSfcBindRecordPagedQuery manuSfcBindRecordPagedQuery)
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
           
            var offSet = (manuSfcBindRecordPagedQuery.PageIndex - 1) * manuSfcBindRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcBindRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcBindRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcBindRecordEntitiesTask = conn.QueryAsync<ManuSfcBindRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcBindRecordEntities = await manuSfcBindRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcBindRecordEntity>(manuSfcBindRecordEntities, manuSfcBindRecordPagedQuery.PageIndex, manuSfcBindRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcBindRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcBindRecordEntity>> GetManuSfcBindRecordEntitiesAsync(ManuSfcBindRecordQuery manuSfcBindRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcBindRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcBindRecordEntities = await conn.QueryAsync<ManuSfcBindRecordEntity>(template.RawSql, manuSfcBindRecordQuery);
            return manuSfcBindRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcBindRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcBindRecordEntity manuSfcBindRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcBindRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcBindRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcBindRecordEntity> manuSfcBindRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcBindRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcBindRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcBindRecordEntity manuSfcBindRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcBindRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcBindRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcBindRecordEntity> manuSfcBindRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcBindRecordEntitys);
        }
        #endregion

    }

    public partial class ManuSfcBindRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_bind_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_bind_record` /**where**/ ";
        const string GetManuSfcBindRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_bind_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_bind_record`(  `Id`, `SiteId`, `SFC`, `BindSFC`, `Type`, `OperationType`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @BindSFC, @Type, @OperationType, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_bind_record`(  `Id`, `SiteId`, `SFC`, `BindSFC`, `Type`, `OperationType`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @BindSFC, @Type, @OperationType, @Location, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_bind_record` SET  SiteId = @SiteId, SFC = @SFC, BindSFC = @BindSFC, Type = @Type, OperationType = @OperationType, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_bind_record` SET  SiteId = @SiteId, SFC = @SFC, BindSFC = @BindSFC, Type = @Type, OperationType = @OperationType, Location = @Location, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_bind_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_bind_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`,`SiteId`, `SFC`, `BindSFC`, `Type`, `OperationType`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_bind_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`,`SiteId`, `SFC`, `BindSFC`, `Type`, `OperationType`, `Location`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_bind_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
