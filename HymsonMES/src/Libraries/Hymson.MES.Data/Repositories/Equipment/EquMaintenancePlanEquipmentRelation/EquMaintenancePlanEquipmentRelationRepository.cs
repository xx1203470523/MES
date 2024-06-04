/*
 *creator: Karl
 *
 *describe: 设备点检计划与设备关系 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 03:51:20
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquMaintenancePlanEquipmentRelation
{
    /// <summary>
    /// 设备点检计划与设备关系仓储
    /// </summary>
    public partial class EquMaintenancePlanEquipmentRelationRepository : BaseRepository, IEquMaintenancePlanEquipmentRelationRepository
    {

        public EquMaintenancePlanEquipmentRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> PhysicalDeletesAsync(IEnumerable<long> MaintenancePlanIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(PhysicalDeletesSql, new { MaintenancePlanIds = MaintenancePlanIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintenancePlanEquipmentRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenancePlanEquipmentRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenancePlanEquipmentRelationEntity>(GetByIdsSql, new { Ids = ids });
        }



        /// <summary>
        /// 根据MaintenancePlanId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetByMaintenancePlanIdsAsync(long MaintenancePlanId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenancePlanEquipmentRelationEntity>(GetByMaintenancePlanIdsSql, new { MaintenancePlanId = MaintenancePlanId });
        }

        /// <summary>
        /// 根据MaintenanceTemplateId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetByMaintenanceTemplateIdsAsync(IEnumerable<long> maintenancePlanIds)
        {
            using var conn = GetMESDbConnection();  
            return await conn.QueryAsync<EquMaintenancePlanEquipmentRelationEntity>(GetByMaintenanceTemplateIdsSql, new { MaintenancePlanIds = maintenancePlanIds });
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenancePlanEquipmentRelationEntity>> GetPagedInfoAsync(EquMaintenancePlanEquipmentRelationPagedQuery EquMaintenancePlanEquipmentRelationPagedQuery)
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

            var offSet = (EquMaintenancePlanEquipmentRelationPagedQuery.PageIndex - 1) * EquMaintenancePlanEquipmentRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = EquMaintenancePlanEquipmentRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(EquMaintenancePlanEquipmentRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var EquMaintenancePlanEquipmentRelationEntitiesTask = conn.QueryAsync<EquMaintenancePlanEquipmentRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var EquMaintenancePlanEquipmentRelationEntities = await EquMaintenancePlanEquipmentRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenancePlanEquipmentRelationEntity>(EquMaintenancePlanEquipmentRelationEntities, EquMaintenancePlanEquipmentRelationPagedQuery.PageIndex, EquMaintenancePlanEquipmentRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenancePlanEquipmentRelationEntity>> GetEquMaintenancePlanEquipmentRelationEntitiesAsync(EquMaintenancePlanEquipmentRelationQuery EquMaintenancePlanEquipmentRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquMaintenancePlanEquipmentRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var EquMaintenancePlanEquipmentRelationEntities = await conn.QueryAsync<EquMaintenancePlanEquipmentRelationEntity>(template.RawSql, EquMaintenancePlanEquipmentRelationQuery);
            return EquMaintenancePlanEquipmentRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenancePlanEquipmentRelationEntity EquMaintenancePlanEquipmentRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, EquMaintenancePlanEquipmentRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquMaintenancePlanEquipmentRelationEntity> EquMaintenancePlanEquipmentRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, EquMaintenancePlanEquipmentRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenancePlanEquipmentRelationEntity EquMaintenancePlanEquipmentRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, EquMaintenancePlanEquipmentRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="EquMaintenancePlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquMaintenancePlanEquipmentRelationEntity> EquMaintenancePlanEquipmentRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, EquMaintenancePlanEquipmentRelationEntitys);
        }
        #endregion

    }

    public partial class EquMaintenancePlanEquipmentRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_Maintenance_plan_equipment_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_Maintenance_plan_equipment_relation` /**where**/ ";
        const string GetEquMaintenancePlanEquipmentRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_Maintenance_plan_equipment_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_Maintenance_plan_equipment_relation`(  `MaintenancePlanId`, `MaintenanceTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`) VALUES (   @MaintenancePlanId, @MaintenanceTemplateId, @EquipmentId, @CreatedBy, @CreatedOn, @ExecutorIds, @LeaderIds )  ";
        const string InsertsSql = "INSERT INTO `equ_Maintenance_plan_equipment_relation`(  `MaintenancePlanId`, `MaintenanceTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`) VALUES (   @MaintenancePlanId, @MaintenanceTemplateId, @EquipmentId, @CreatedBy, @CreatedOn, @ExecutorIds, @LeaderIds )  ";

        const string UpdateSql = "UPDATE `equ_Maintenance_plan_equipment_relation` SET   MaintenancePlanId = @MaintenancePlanId, MaintenanceTemplateId = @MaintenanceTemplateId, EquipmentId = @EquipmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_Maintenance_plan_equipment_relation` SET   MaintenancePlanId = @MaintenancePlanId, MaintenanceTemplateId = @MaintenanceTemplateId, EquipmentId = @EquipmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_Maintenance_plan_equipment_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_Maintenance_plan_equipment_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string PhysicalDeletesSql = "DELETE  FROM `equ_Maintenance_plan_equipment_relation`  WHERE MaintenancePlanId IN @MaintenancePlanIds";

        const string GetByIdSql = @"SELECT 
                               `MaintenancePlanId`, `MaintenanceTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan_equipment_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `MaintenancePlanId`, `MaintenanceTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan_equipment_relation`  WHERE Id IN @Ids ";
        const string GetByMaintenancePlanIdsSql = @"SELECT  
                                          `MaintenancePlanId`, `MaintenanceTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan_equipment_relation`  WHERE MaintenancePlanId = @MaintenancePlanId ";

        const string GetByMaintenanceTemplateIdsSql = @"SELECT   
                                          `MaintenancePlanId`, `MaintenanceTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan_equipment_relation`  WHERE MaintenanceTemplateId IN @MaintenanceTemplateIds ";
        #endregion
    }
}
