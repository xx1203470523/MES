/*
 *creator: Karl
 *
 *describe: 维修结果 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:58:46
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairResult;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquRepairResult
{
    /// <summary>
    /// 维修结果仓储
    /// </summary>
    public partial class EquRepairResultRepository : BaseRepository, IEquRepairResultRepository
    {

        public EquRepairResultRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquRepairResultEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquRepairResultEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据RepairOrderId获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquRepairResultEntity> GetByRepairOrderIdAsync(long repairOrderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquRepairResultEntity>(GetByRepairOrderIdSql, new { RepairOrderId = repairOrderId });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairResultEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairResultEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairResultPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairResultEntity>> GetPagedInfoAsync(EquRepairResultPagedQuery equRepairResultPagedQuery)
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

            var offSet = (equRepairResultPagedQuery.PageIndex - 1) * equRepairResultPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equRepairResultPagedQuery.PageSize });
            sqlBuilder.AddParameters(equRepairResultPagedQuery);

            using var conn = GetMESDbConnection();
            var equRepairResultEntitiesTask = conn.QueryAsync<EquRepairResultEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equRepairResultEntities = await equRepairResultEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquRepairResultEntity>(equRepairResultEntities, equRepairResultPagedQuery.PageIndex, equRepairResultPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equRepairResultQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairResultEntity>> GetEquRepairResultEntitiesAsync(EquRepairResultQuery equRepairResultQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquRepairResultEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equRepairResultEntities = await conn.QueryAsync<EquRepairResultEntity>(template.RawSql, equRepairResultQuery);
            return equRepairResultEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairResultEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquRepairResultEntity equRepairResultEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equRepairResultEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairResultEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquRepairResultEntity> equRepairResultEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equRepairResultEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairResultEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquRepairResultEntity equRepairResultEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equRepairResultEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equRepairResultEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquRepairResultEntity> equRepairResultEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equRepairResultEntitys);
        }
        #endregion

    }

    public partial class EquRepairResultRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_repair_result` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_repair_result` /**where**/ ";
        const string GetEquRepairResultEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_repair_result` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_repair_result`(  `Id`, `SiteId`, `RepairOrderId`, `RepairStartTime`, `RepairEndTime`, `LongTermHandlingMeasures`, `TemporaryTermHandlingMeasures`, `RepairPerson`, `ConfirmResult`, `ConfirmOn`, `ConfirmBy`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrderId, @RepairStartTime, @RepairEndTime, @LongTermHandlingMeasures, @TemporaryTermHandlingMeasures, @RepairPerson, @ConfirmResult, @ConfirmOn, @ConfirmBy, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_repair_result`(  `Id`, `SiteId`, `RepairOrderId`, `RepairStartTime`, `RepairEndTime`, `LongTermHandlingMeasures`, `TemporaryTermHandlingMeasures`, `RepairPerson`, `ConfirmResult`, `ConfirmOn`, `ConfirmBy`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrderId, @RepairStartTime, @RepairEndTime, @LongTermHandlingMeasures, @TemporaryTermHandlingMeasures, @RepairPerson, @ConfirmResult, @ConfirmOn, @ConfirmBy, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_repair_result` SET   SiteId = @SiteId, RepairOrderId = @RepairOrderId, RepairStartTime = @RepairStartTime, RepairEndTime = @RepairEndTime, LongTermHandlingMeasures = @LongTermHandlingMeasures, TemporaryTermHandlingMeasures = @TemporaryTermHandlingMeasures, RepairPerson = @RepairPerson, ConfirmResult = @ConfirmResult, ConfirmOn = @ConfirmOn, ConfirmBy = @ConfirmBy, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_repair_result` SET   SiteId = @SiteId, RepairOrderId = @RepairOrderId, RepairStartTime = @RepairStartTime, RepairEndTime = @RepairEndTime, LongTermHandlingMeasures = @LongTermHandlingMeasures, TemporaryTermHandlingMeasures = @TemporaryTermHandlingMeasures, RepairPerson = @RepairPerson, ConfirmResult = @ConfirmResult, ConfirmOn = @ConfirmOn, ConfirmBy = @ConfirmBy, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_repair_result` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_repair_result` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `RepairOrderId`, `RepairStartTime`, `RepairEndTime`, `LongTermHandlingMeasures`, `TemporaryTermHandlingMeasures`, `RepairPerson`, `ConfirmResult`, `ConfirmOn`, `ConfirmBy`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_result`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `RepairOrderId`, `RepairStartTime`, `RepairEndTime`, `LongTermHandlingMeasures`, `TemporaryTermHandlingMeasures`, `RepairPerson`, `ConfirmResult`, `ConfirmOn`, `ConfirmBy`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_result`  WHERE Id IN @Ids ";


        const string GetByRepairOrderIdSql = @"SELECT 
                               `Id`, `SiteId`, `RepairOrderId`, `RepairStartTime`, `RepairEndTime`, `LongTermHandlingMeasures`, `TemporaryTermHandlingMeasures`, `RepairPerson`, `ConfirmResult`, `ConfirmOn`, `ConfirmBy`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_result`  WHERE RepairOrderId = @RepairOrderId ";
        #endregion
    }
}
