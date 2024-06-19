/*
 *creator: Karl
 *
 *describe: 设备维修记录 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquRepairOrder;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录仓储
    /// </summary>
    public partial class EquRepairOrderRepository :BaseRepository, IEquRepairOrderRepository
    {

        public EquRepairOrderRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<EquRepairOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquRepairOrderEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquRepairOrderEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairOrderEntity>> GetPagedInfoAsync(EquRepairOrderPagedQuery equRepairOrderPagedQuery)
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
           
            var offSet = (equRepairOrderPagedQuery.PageIndex - 1) * equRepairOrderPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equRepairOrderPagedQuery.PageSize });
            sqlBuilder.AddParameters(equRepairOrderPagedQuery);

            using var conn = GetMESDbConnection();
            var equRepairOrderEntitiesTask = conn.QueryAsync<EquRepairOrderEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equRepairOrderEntities = await equRepairOrderEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquRepairOrderEntity>(equRepairOrderEntities, equRepairOrderPagedQuery.PageIndex, equRepairOrderPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equRepairOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquRepairOrderEntity>> GetEquRepairOrderEntitiesAsync(EquRepairOrderQuery equRepairOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquRepairOrderEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equRepairOrderEntities = await conn.QueryAsync<EquRepairOrderEntity>(template.RawSql, equRepairOrderQuery);
            return equRepairOrderEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquRepairOrderEntity equRepairOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equRepairOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquRepairOrderEntity> equRepairOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equRepairOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquRepairOrderEntity equRepairOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equRepairOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equRepairOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquRepairOrderEntity> equRepairOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equRepairOrderEntitys);
        }
        #endregion

    }

    public partial class EquRepairOrderRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_repair_order` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_repair_order` /**where**/ ";
        const string GetEquRepairOrderEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_repair_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_repair_order`(  `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrder, @EquipmentId, @EquipmentRecordId, @FaultTime, @IsShutdown, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_repair_order`(  `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RepairOrder, @EquipmentId, @EquipmentRecordId, @FaultTime, @IsShutdown, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_repair_order` SET   SiteId = @SiteId, RepairOrder = @RepairOrder, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, FaultTime = @FaultTime, IsShutdown = @IsShutdown, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_repair_order` SET   SiteId = @SiteId, RepairOrder = @RepairOrder, EquipmentId = @EquipmentId, EquipmentRecordId = @EquipmentRecordId, FaultTime = @FaultTime, IsShutdown = @IsShutdown, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_repair_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_repair_order` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `RepairOrder`, `EquipmentId`, `EquipmentRecordId`, `FaultTime`, `IsShutdown`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_repair_order`  WHERE Id IN @Ids ";
        #endregion
    }
}
