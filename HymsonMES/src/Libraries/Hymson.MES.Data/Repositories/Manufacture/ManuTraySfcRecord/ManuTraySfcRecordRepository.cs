/*
 *creator: Karl
 *
 *describe: 托盘条码记录表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:02
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
    /// 托盘条码记录表仓储
    /// </summary>
    public partial class ManuTraySfcRecordRepository :BaseRepository, IManuTraySfcRecordRepository
    {

        public ManuTraySfcRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuTraySfcRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuTraySfcRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTraySfcRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuTraySfcRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuTraySfcRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuTraySfcRecordEntity>> GetPagedInfoAsync(ManuTraySfcRecordPagedQuery manuTraySfcRecordPagedQuery)
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
           
            var offSet = (manuTraySfcRecordPagedQuery.PageIndex - 1) * manuTraySfcRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuTraySfcRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuTraySfcRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var manuTraySfcRecordEntitiesTask = conn.QueryAsync<ManuTraySfcRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuTraySfcRecordEntities = await manuTraySfcRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuTraySfcRecordEntity>(manuTraySfcRecordEntities, manuTraySfcRecordPagedQuery.PageIndex, manuTraySfcRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuTraySfcRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuTraySfcRecordEntity>> GetManuTraySfcRecordEntitiesAsync(ManuTraySfcRecordQuery manuTraySfcRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuTraySfcRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuTraySfcRecordEntities = await conn.QueryAsync<ManuTraySfcRecordEntity>(template.RawSql, manuTraySfcRecordQuery);
            return manuTraySfcRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTraySfcRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuTraySfcRecordEntity manuTraySfcRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuTraySfcRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuTraySfcRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuTraySfcRecordEntity> manuTraySfcRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuTraySfcRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuTraySfcRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuTraySfcRecordEntity manuTraySfcRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuTraySfcRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuTraySfcRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuTraySfcRecordEntity> manuTraySfcRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuTraySfcRecordEntitys);
        }
        #endregion

    }

    public partial class ManuTraySfcRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_tray_sfc_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_tray_sfc_record` /**where**/ ";
        const string GetManuTraySfcRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_tray_sfc_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_tray_sfc_record`(  `Id`, `SiteId`, `Tray`, `SFC`, `SFCIinfoId`, `Seq`, `LoadQty`, `OperationType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Tray, @SFC, @SFCIinfoId, @Seq, @LoadQty, @OperationType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_tray_sfc_record`(  `Id`, `SiteId`, `Tray`, `SFC`, `SFCIinfoId`, `Seq`, `LoadQty`, `OperationType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Tray, @SFC, @SFCIinfoId, @Seq, @LoadQty, @OperationType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_tray_sfc_record` SET   SiteId = @SiteId, Tray = @Tray, SFC = @SFC, SFCIinfoId = @SFCIinfoId, Seq = @Seq, LoadQty = @LoadQty, OperationType = @OperationType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_tray_sfc_record` SET   SiteId = @SiteId, Tray = @Tray, SFC = @SFC, SFCIinfoId = @SFCIinfoId, Seq = @Seq, LoadQty = @LoadQty, OperationType = @OperationType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_tray_sfc_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_tray_sfc_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Tray`, `SFC`, `SFCIinfoId`, `Seq`, `LoadQty`, `OperationType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_sfc_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Tray`, `SFC`, `SFCIinfoId`, `Seq`, `LoadQty`, `OperationType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_tray_sfc_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
