/*
 *creator: Karl
 *
 *describe: 开机参数采集表 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 开机参数采集表仓储
    /// </summary>
    public partial class ProcBootupparamrecordRepository :BaseRepository, IProcBootupparamrecordRepository
    {

        public ProcBootupparamrecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcBootupparamrecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBootupparamrecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBootupparamrecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcBootupparamrecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBootupparamrecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBootupparamrecordEntity>> GetPagedInfoAsync(ProcBootupparamrecordPagedQuery procBootupparamrecordPagedQuery)
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
           
            var offSet = (procBootupparamrecordPagedQuery.PageIndex - 1) * procBootupparamrecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBootupparamrecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBootupparamrecordPagedQuery);

            using var conn = GetMESDbConnection();
            var procBootupparamrecordEntitiesTask = conn.QueryAsync<ProcBootupparamrecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBootupparamrecordEntities = await procBootupparamrecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBootupparamrecordEntity>(procBootupparamrecordEntities, procBootupparamrecordPagedQuery.PageIndex, procBootupparamrecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBootupparamrecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBootupparamrecordEntity>> GetProcBootupparamrecordEntitiesAsync(ProcBootupparamrecordQuery procBootupparamrecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBootupparamrecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procBootupparamrecordEntities = await conn.QueryAsync<ProcBootupparamrecordEntity>(template.RawSql, procBootupparamrecordQuery);
            return procBootupparamrecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootupparamrecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBootupparamrecordEntity procBootupparamrecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBootupparamrecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBootupparamrecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBootupparamrecordEntity> procBootupparamrecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procBootupparamrecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBootupparamrecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBootupparamrecordEntity procBootupparamrecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procBootupparamrecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBootupparamrecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBootupparamrecordEntity> procBootupparamrecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procBootupparamrecordEntitys);
        }
        #endregion

    }

    public partial class ProcBootupparamrecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bootupparamrecord` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_bootupparamrecord` /**where**/ ";
        const string GetProcBootupparamrecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_bootupparamrecord` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bootupparamrecord`(  `Id`, `EquipmentId`, `RecipeId`, `ProductCode`, `ParamCode`, `ParamValue`, `ParamUpper`, `ParamLower`, `Timestamp`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @EquipmentId, @RecipeId, @ProductCode, @ParamCode, @ParamValue, @ParamUpper, @ParamLower, @Timestamp, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_bootupparamrecord`(  `Id`, `EquipmentId`, `RecipeId`, `ProductCode`, `ParamCode`, `ParamValue`, `ParamUpper`, `ParamLower`, `Timestamp`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @EquipmentId, @RecipeId, @ProductCode, @ParamCode, @ParamValue, @ParamUpper, @ParamLower, @Timestamp, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_bootupparamrecord` SET   EquipmentId = @EquipmentId, RecipeId = @RecipeId, ProductCode = @ProductCode, ParamCode = @ParamCode, ParamValue = @ParamValue, ParamUpper = @ParamUpper, ParamLower = @ParamLower, Timestamp = @Timestamp, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_bootupparamrecord` SET   EquipmentId = @EquipmentId, RecipeId = @RecipeId, ProductCode = @ProductCode, ParamCode = @ParamCode, ParamValue = @ParamValue, ParamUpper = @ParamUpper, ParamLower = @ParamLower, Timestamp = @Timestamp, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_bootupparamrecord` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bootupparamrecord` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `EquipmentId`, `RecipeId`, `ProductCode`, `ParamCode`, `ParamValue`, `ParamUpper`, `ParamLower`, `Timestamp`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_bootupparamrecord`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `EquipmentId`, `RecipeId`, `ProductCode`, `ParamCode`, `ParamValue`, `ParamUpper`, `ParamLower`, `Timestamp`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_bootupparamrecord`  WHERE Id IN @Ids ";
        #endregion
    }
}
