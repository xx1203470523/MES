/*
 *creator: Karl
 *
 *describe: 设备点检模板与设备组关系 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:22
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplateEquipmentGroupRelation
{
    /// <summary>
    /// 设备点检模板与设备组关系仓储
    /// </summary>
    public partial class EquMaintenanceTemplateEquipmentGroupRelationRepository : BaseRepository, IEquMaintenanceTemplateEquipmentGroupRelationRepository
    {

        public EquMaintenanceTemplateEquipmentGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<int> DeletesByMaintenanceTemplateIdsAsync(IEnumerable<long> MaintenanceTemplateIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByMaintenanceTemplateIdSql, new { MaintenanceTemplateIds = MaintenanceTemplateIds });
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public async Task<int> DeletesByTemplateIdAndGroupIdsAsync(GetByTemplateIdAndGroupIdQuery MaintenanceTemplateIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByTemplateIdAndGroupIdsSql, MaintenanceTemplateIds);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintenanceTemplateEquipmentGroupRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTemplateEquipmentGroupRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTemplateEquipmentGroupRelationEntity>(GetByIdsSql, new { Ids = ids });
        }


        /// <summary>
        /// 根据IDs批量获取数据(组合)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetByTemplateIdAndGroupIdAsync(GetByTemplateIdAndGroupIdQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTemplateEquipmentGroupRelationEntity>(GetByTemplateIdAndGroupIdSql, param);
        }

        /// <summary>
        /// 根据GroupId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetByGroupIdAsync(IEnumerable<long> groupIdSql)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTemplateEquipmentGroupRelationEntity>(GetByGroupIdSql, new { EquipmentGroupIds = groupIdSql });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetPagedInfoAsync(EquMaintenanceTemplateEquipmentGroupRelationPagedQuery EquMaintenanceTemplateEquipmentGroupRelationPagedQuery)
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

            var offSet = (EquMaintenanceTemplateEquipmentGroupRelationPagedQuery.PageIndex - 1) * EquMaintenanceTemplateEquipmentGroupRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = EquMaintenanceTemplateEquipmentGroupRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(EquMaintenanceTemplateEquipmentGroupRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var EquMaintenanceTemplateEquipmentGroupRelationEntitiesTask = conn.QueryAsync<EquMaintenanceTemplateEquipmentGroupRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var EquMaintenanceTemplateEquipmentGroupRelationEntities = await EquMaintenanceTemplateEquipmentGroupRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenanceTemplateEquipmentGroupRelationEntity>(EquMaintenanceTemplateEquipmentGroupRelationEntities, EquMaintenanceTemplateEquipmentGroupRelationPagedQuery.PageIndex, EquMaintenanceTemplateEquipmentGroupRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateEquipmentGroupRelationEntity>> GetEquMaintenanceTemplateEquipmentGroupRelationEntitiesAsync(EquMaintenanceTemplateEquipmentGroupRelationQuery EquMaintenanceTemplateEquipmentGroupRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquMaintenanceTemplateEquipmentGroupRelationEntitiesSqlTemplate);

            sqlBuilder.Select("*");

            if (EquMaintenanceTemplateEquipmentGroupRelationQuery.MaintenanceTemplateIds != null && EquMaintenanceTemplateEquipmentGroupRelationQuery.MaintenanceTemplateIds.Any())
            {
                sqlBuilder.Where("MaintenanceTemplateId IN @MaintenanceTemplateIds");
            }

            using var conn = GetMESDbConnection();
            var EquMaintenanceTemplateEquipmentGroupRelationEntities = await conn.QueryAsync<EquMaintenanceTemplateEquipmentGroupRelationEntity>(template.RawSql, EquMaintenanceTemplateEquipmentGroupRelationQuery);
            return EquMaintenanceTemplateEquipmentGroupRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenanceTemplateEquipmentGroupRelationEntity EquMaintenanceTemplateEquipmentGroupRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, EquMaintenanceTemplateEquipmentGroupRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquMaintenanceTemplateEquipmentGroupRelationEntity> EquMaintenanceTemplateEquipmentGroupRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, EquMaintenanceTemplateEquipmentGroupRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenanceTemplateEquipmentGroupRelationEntity EquMaintenanceTemplateEquipmentGroupRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, EquMaintenanceTemplateEquipmentGroupRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquMaintenanceTemplateEquipmentGroupRelationEntity> EquMaintenanceTemplateEquipmentGroupRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, EquMaintenanceTemplateEquipmentGroupRelationEntitys);
        }
        #endregion

    }

    public partial class EquMaintenanceTemplateEquipmentGroupRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_Maintenance_template_equipment_group_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_Maintenance_template_equipment_group_relation` /**where**/ ";
        const string GetEquMaintenanceTemplateEquipmentGroupRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_Maintenance_template_equipment_group_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_Maintenance_template_equipment_group_relation`(  `MaintenanceTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) VALUES (   @MaintenanceTemplateId, @EquipmentGroupId, @CreatedBy, @CreatedOn )  ";
        const string InsertsSql = "INSERT INTO `equ_Maintenance_template_equipment_group_relation`(  `MaintenanceTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) VALUES (   @MaintenanceTemplateId, @EquipmentGroupId, @CreatedBy, @CreatedOn )  ";

        const string UpdateSql = "UPDATE `equ_Maintenance_template_equipment_group_relation` SET   MaintenanceTemplateId = @MaintenanceTemplateId, EquipmentGroupId = @EquipmentGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_Maintenance_template_equipment_group_relation` SET   MaintenanceTemplateId = @MaintenanceTemplateId, EquipmentGroupId = @EquipmentGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE  MaintenanceTemplateId=@MaintenanceTemplateId AND EquipmentGroupId=@EquipmentGroupId ";

        const string DeleteSql = "UPDATE `equ_Maintenance_template_equipment_group_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_Maintenance_template_equipment_group_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeletesByMaintenanceTemplateIdSql = "DELETE FROM `equ_Maintenance_template_equipment_group_relation` WHERE  MaintenanceTemplateId IN @MaintenanceTemplateIds";

        const string DeletesByTemplateIdAndGroupIdsSql = "DELETE FROM `equ_Maintenance_template_equipment_group_relation` WHERE  MaintenanceTemplateId=@MaintenanceTemplateId AND EquipmentGroupId IN @EquipmentGroupIds ";


        const string GetByIdSql = @"SELECT 
                               `MaintenanceTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_equipment_group_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `MaintenanceTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_equipment_group_relation`  WHERE Id IN @Ids ";

        const string GetByTemplateIdAndGroupIdSql = @"SELECT  
                                          `MaintenanceTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_equipment_group_relation`  WHERE  MaintenanceTemplateId=@MaintenanceTemplateId AND EquipmentGroupId IN @EquipmentGroupIds ";

        const string GetByGroupIdSql = @"SELECT   
                                          `MaintenanceTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_equipment_group_relation`  WHERE  EquipmentGroupId IN @EquipmentGroupIds ";
        #endregion
    }
}
