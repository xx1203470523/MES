/*
 *creator: Karl
 *
 *describe: 设备维修记录故障详情 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:30
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairOrderFault;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquRepairOrderFault
{
    /// <summary>
    /// 设备维修记录故障详情仓储
    /// </summary>
    public partial class EquRepairOrderFaultRepository : BaseRepository, IEquRepairOrderFaultRepository
    {

        public EquRepairOrderFaultRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquRepairOrderFaultEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquRepairOrderFaultEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderFaultEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairOrderFaultEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据repairOrderId批量获取数据 
        /// </summary>
        /// <param name="repairOrderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderFaultEntity>> GetByRepairOrderIdAsync(long repairOrderId) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairOrderFaultEntity>(GetByRepairOrderIdSql, new { RepairOrderId = repairOrderId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderFaultPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairOrderFaultEntity>> GetPagedInfoAsync(EquRepairOrderFaultPagedQuery equRepairOrderFaultPagedQuery)
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

            var offSet = (equRepairOrderFaultPagedQuery.PageIndex - 1) * equRepairOrderFaultPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equRepairOrderFaultPagedQuery.PageSize });
            sqlBuilder.AddParameters(equRepairOrderFaultPagedQuery);

            using var conn = GetMESDbConnection();
            var equRepairOrderFaultEntitiesTask = conn.QueryAsync<EquRepairOrderFaultEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equRepairOrderFaultEntities = await equRepairOrderFaultEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquRepairOrderFaultEntity>(equRepairOrderFaultEntities, equRepairOrderFaultPagedQuery.PageIndex, equRepairOrderFaultPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equRepairOrderFaultQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderFaultEntity>> GetEquRepairOrderFaultEntitiesAsync(EquRepairOrderFaultQuery equRepairOrderFaultQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquRepairOrderFaultEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equRepairOrderFaultEntities = await conn.QueryAsync<EquRepairOrderFaultEntity>(template.RawSql, equRepairOrderFaultQuery);
            return equRepairOrderFaultEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderFaultEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquRepairOrderFaultEntity equRepairOrderFaultEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equRepairOrderFaultEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderFaultEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquRepairOrderFaultEntity> equRepairOrderFaultEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equRepairOrderFaultEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderFaultEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquRepairOrderFaultEntity equRepairOrderFaultEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equRepairOrderFaultEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equRepairOrderFaultEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquRepairOrderFaultEntity> equRepairOrderFaultEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equRepairOrderFaultEntitys);
        }

        /// <summary>
        /// 批量更新（故障原因）
        /// </summary>
        /// <param name="equRepairOrderFaultEntitys"></param>
        /// <returns></returns> 
        public async Task<int> UpdateFaultReasonsAsync(List<UpdateFaultReasonsQuery> updatesEquRepairOrderFaultQuery) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateFaultReasonsSql, updatesEquRepairOrderFaultQuery);
        }
        #endregion

    }

    public partial class EquRepairOrderFaultRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_repair_order_fault` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_repair_order_fault` /**where**/ ";
        const string GetEquRepairOrderFaultEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_repair_order_fault` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_repair_order_fault`(  `Id`, `SiteId`, `RepairOrderId`, `FaultPhenomenonId`, `FaultPhenomenon`, `FaultReasonId`, `FaultReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrderId, @FaultPhenomenonId, @FaultPhenomenon, @FaultReasonId, @FaultReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_repair_order_fault`(  `Id`, `SiteId`, `RepairOrderId`, `FaultPhenomenonId`, `FaultPhenomenon`, `FaultReasonId`, `FaultReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrderId, @FaultPhenomenonId, @FaultPhenomenon, @FaultReasonId, @FaultReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_repair_order_fault` SET   SiteId = @SiteId, RepairOrderId = @RepairOrderId, FaultPhenomenonId = @FaultPhenomenonId, FaultPhenomenon = @FaultPhenomenon, FaultReasonId = @FaultReasonId, FaultReason = @FaultReason, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_repair_order_fault` SET   SiteId = @SiteId, RepairOrderId = @RepairOrderId, FaultPhenomenonId = @FaultPhenomenonId, FaultPhenomenon = @FaultPhenomenon, FaultReasonId = @FaultReasonId, FaultReason = @FaultReason, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateFaultReasonsSql = "UPDATE `equ_repair_order_fault` SET   FaultReasonId = @FaultReasonId, FaultReason = @FaultReason,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn   WHERE Id = @Id ";
         
        const string DeleteSql = "UPDATE `equ_repair_order_fault` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_repair_order_fault` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `RepairOrderId`, `FaultPhenomenonId`, `FaultPhenomenon`, `FaultReasonId`, `FaultReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order_fault`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `RepairOrderId`, `FaultPhenomenonId`, `FaultPhenomenon`, `FaultReasonId`, `FaultReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order_fault`  WHERE Id IN @Ids ";

        const string GetByRepairOrderIdSql = @"SELECT  
                               `Id`, `SiteId`, `RepairOrderId`, `FaultPhenomenonId`, `FaultPhenomenon`, `FaultReasonId`, `FaultReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order_fault`  WHERE RepairOrderId = @RepairOrderId ";
        #endregion
    }
}
