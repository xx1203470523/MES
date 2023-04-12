/*
 *creator: pengxin
 *
 *describe: 设备故障原因表 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-02-28 15:15:20
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障原因表仓储
    /// </summary>
    public partial class EquFaultReasonRepository : IEquFaultReasonRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public EquFaultReasonRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultReasonEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquFaultReasonEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultReasonEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<EquFaultReasonEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquFaultReasonPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultReasonEntity>> GetPagedInfoAsync(EquFaultReasonPagedQuery EquFaultReasonPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (EquFaultReasonPagedQuery.UseStatus.HasValue)
            {
                sqlBuilder.Where("UseStatus = @UseStatus");
            }

            if (!string.IsNullOrWhiteSpace(EquFaultReasonPagedQuery.FaultReasonCode))
            {
                EquFaultReasonPagedQuery.FaultReasonCode = $"%{EquFaultReasonPagedQuery.FaultReasonCode}%";
                sqlBuilder.Where(" FaultReasonCode like @FaultReasonCode ");
            }
            if (!string.IsNullOrWhiteSpace(EquFaultReasonPagedQuery.FaultReasonName))
            {
                EquFaultReasonPagedQuery.FaultReasonName = $"%{EquFaultReasonPagedQuery.FaultReasonName}%";
                sqlBuilder.Where(" FaultReasonName like @FaultReasonName ");
            }
            if (!string.IsNullOrWhiteSpace(EquFaultReasonPagedQuery.Remark))
            {
                EquFaultReasonPagedQuery.Remark = $"%{EquFaultReasonPagedQuery.Remark}%";
                sqlBuilder.Where(" Remark like @Remark ");
            }

            var offSet = (EquFaultReasonPagedQuery.PageIndex - 1) * EquFaultReasonPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = EquFaultReasonPagedQuery.PageSize });
            sqlBuilder.AddParameters(EquFaultReasonPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var EquFaultReasonEntitiesTask = conn.QueryAsync<EquFaultReasonEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var EquFaultReasonEntities = await EquFaultReasonEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquFaultReasonEntity>(EquFaultReasonEntities, EquFaultReasonPagedQuery.PageIndex, EquFaultReasonPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquFaultReasonQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultReasonEntity>> GetEquFaultReasonEntitiesAsync(EquFaultReasonQuery EquFaultReasonQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquFaultReasonEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (EquFaultReasonQuery.SiteId != 0)
            {
                sqlBuilder.Where(" SiteId=@SiteId ");
            }
            if (!string.IsNullOrWhiteSpace(EquFaultReasonQuery.FaultReasonCode))
            {
                //EquFaultReasonQuery.FaultReasonCode = $"%{EquFaultReasonQuery.FaultReasonCode}%";
                sqlBuilder.Where(" FaultReasonCode = @FaultReasonCode ");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var EquFaultReasonEntities = await conn.QueryAsync<EquFaultReasonEntity>(template.RawSql, EquFaultReasonQuery);
            return EquFaultReasonEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquFaultReasonEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquFaultReasonEntity EquFaultReasonEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, EquFaultReasonEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquFaultReasonEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<EquFaultReasonEntity> EquFaultReasonEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, EquFaultReasonEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquFaultReasonEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquFaultReasonEntity EquFaultReasonEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, EquFaultReasonEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="EquFaultReasonEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<EquFaultReasonEntity> EquFaultReasonEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, EquFaultReasonEntitys);
        }

    }

    public partial class EquFaultReasonRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_fault_reason` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_fault_reason` /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEquFaultReasonEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_fault_reason` /**innerjoin**/ /**leftjoin**/ /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_fault_reason`(  `Id`, `SiteId`, `FaultReasonCode`, `FaultReasonName`, `UseStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FaultReasonCode, @FaultReasonName, @UseStatus, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_fault_reason`(  `Id`, `SiteId`, `FaultReasonCode`, `FaultReasonName`, `UseStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FaultReasonCode, @FaultReasonName, @UseStatus, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `equ_fault_reason` SET  UseStatus = @UseStatus, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_fault_reason` SET SiteId = @SiteId, FaultReasonCode = @FaultReasonCode, FaultReasonName = @FaultReasonName, UseStatus = @UseStatus, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_fault_reason` SET IsDeleted = Id WHERE Id = @Id  ";
        const string DeletesSql = "UPDATE `equ_fault_reason` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `FaultReasonCode`, `FaultReasonName`, `UseStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_fault_reason`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FaultReasonCode`, `FaultReasonName`, `UseStatus`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_fault_reason`  WHERE Id IN @ids ";
    }
}
