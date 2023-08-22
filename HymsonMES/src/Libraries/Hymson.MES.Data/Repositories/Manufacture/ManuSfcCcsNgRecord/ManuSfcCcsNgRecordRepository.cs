/*
 *creator: Karl
 *
 *describe: CCS盖板NG记录 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-08-15 05:15:40
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
    /// CCS盖板NG记录仓储
    /// </summary>
    public partial class ManuSfcCcsNgRecordRepository : BaseRepository, IManuSfcCcsNgRecordRepository
    {

        public ManuSfcCcsNgRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuSfcCcsNgRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcCcsNgRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCcsNgRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcCcsNgRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcCcsNgRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcCcsNgRecordEntity>> GetPagedInfoAsync(ManuSfcCcsNgRecordPagedQuery manuSfcCcsNgRecordPagedQuery)
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

            var offSet = (manuSfcCcsNgRecordPagedQuery.PageIndex - 1) * manuSfcCcsNgRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcCcsNgRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcCcsNgRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcCcsNgRecordEntitiesTask = conn.QueryAsync<ManuSfcCcsNgRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcCcsNgRecordEntities = await manuSfcCcsNgRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcCcsNgRecordEntity>(manuSfcCcsNgRecordEntities, manuSfcCcsNgRecordPagedQuery.PageIndex, manuSfcCcsNgRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcCcsNgRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcCcsNgRecordEntity>> GetManuSfcCcsNgRecordEntitiesAsync(ManuSfcCcsNgRecordQuery manuSfcCcsNgRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcCcsNgRecordEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("CreatedOn");
            if (!string.IsNullOrWhiteSpace(manuSfcCcsNgRecordQuery.SFC))
            {
                sqlBuilder.Where("SFC = @SFC");
            }
            if (manuSfcCcsNgRecordQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (!string.IsNullOrEmpty(manuSfcCcsNgRecordQuery.Location))
            {
                sqlBuilder.Where("Location = @Location");
            }
            using var conn = GetMESDbConnection();
            var manuSfcCcsNgRecordEntities = await conn.QueryAsync<ManuSfcCcsNgRecordEntity>(template.RawSql, manuSfcCcsNgRecordQuery);
            return manuSfcCcsNgRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcCcsNgRecordEntity manuSfcCcsNgRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcCcsNgRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcCcsNgRecordEntity> manuSfcCcsNgRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcCcsNgRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcCcsNgRecordEntity manuSfcCcsNgRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcCcsNgRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcCcsNgRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcCcsNgRecordEntity> manuSfcCcsNgRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcCcsNgRecordEntitys);
        }
        #endregion

    }

    public partial class ManuSfcCcsNgRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_ccsNg_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_ccsNg_record` /**where**/ ";
        const string GetManuSfcCcsNgRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_ccsNg_record` /**where**/ /**orderby**/ ";

        const string InsertSql = "INSERT INTO `manu_sfc_ccsNg_record`(  `Id`, `SiteId`, `SFC`, `Location`, `Status`, `NgCode`, `NgName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,`ModelCode`) VALUES (   @Id, @SiteId, @SFC, @Location, @Status, @NgCode, @NgName, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted,@ModelCode )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_ccsNg_record`(  `Id`, `SiteId`, `SFC`, `Location`, `Status`, `NgCode`, `NgName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,`ModelCode`) VALUES (   @Id, @SiteId, @SFC, @Location, @Status, @NgCode, @NgName, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted,@ModelCode )  ";

        const string UpdateSql = "UPDATE `manu_sfc_ccsNg_record` SET   SiteId = @SiteId, SFC = @SFC, Location = @Location, Status = @Status, NgCode = @NgCode, NgName = @NgName,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, ModelCode = @ModelCode  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_ccsNg_record` SET   SiteId = @SiteId, SFC = @SFC, Location = @Location, Status = @Status, NgCode = @NgCode, NgName = @NgName, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, ModelCode = @ModelCode  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_ccsNg_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_ccsNg_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SFC`, `Location`, `Status`, `NgCode`, `NgName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `ModelCode`
                            FROM `manu_sfc_ccsNg_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SFC`, `Location`, `Status`, `NgCode`, `NgName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `ModelCode`
                            FROM `manu_sfc_ccsNg_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
