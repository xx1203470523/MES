/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquMaintenanceTemplateItemRelation
{
    /// <summary>
    /// 设备点检模板与项目关系仓储
    /// </summary>
    public partial class EquMaintenanceTemplateItemRelationRepository : BaseRepository, IEquMaintenanceTemplateItemRelationRepository
    {

        public EquMaintenanceTemplateItemRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<int> DeleteByMaintenanceTemplateIdsAsync(IEnumerable<long> MaintenanceTemplateIds)
        {
            using var conn = GetMESDbConnection(); 
            return await conn.ExecuteAsync(DeleteByMaintenanceTemplateIdsSql, new { MaintenanceTemplateIds = MaintenanceTemplateIds });
        }


        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>  
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesByTemplateIdAndItemIdsAsync(GetByTemplateIdAndItemIdQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByTemplateIdAndItemIdsSql, param);
        }


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquMaintenanceTemplateItemRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenanceTemplateItemRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateItemRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTemplateItemRelationEntity>(GetByIdsSql, new { Ids = ids });
        }


        /// <summary>
        /// 根据ID获取数据（组合）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateItemRelationEntity>> GetByTemplateIdAndItemIdSqlAsync(GetByTemplateIdAndItemIdQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenanceTemplateItemRelationEntity>(GetByTemplateIdAndItemIdSql, param);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTemplateItemRelationEntity>> GetPagedInfoAsync(EquMaintenanceTemplateItemRelationPagedQuery EquMaintenanceTemplateItemRelationPagedQuery)
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

            var offSet = (EquMaintenanceTemplateItemRelationPagedQuery.PageIndex - 1) * EquMaintenanceTemplateItemRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = EquMaintenanceTemplateItemRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(EquMaintenanceTemplateItemRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var EquMaintenanceTemplateItemRelationEntitiesTask = conn.QueryAsync<EquMaintenanceTemplateItemRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var EquMaintenanceTemplateItemRelationEntities = await EquMaintenanceTemplateItemRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenanceTemplateItemRelationEntity>(EquMaintenanceTemplateItemRelationEntities, EquMaintenanceTemplateItemRelationPagedQuery.PageIndex, EquMaintenanceTemplateItemRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTemplateItemRelationEntity>> GetEquMaintenanceTemplateItemRelationEntitiesAsync(EquMaintenanceTemplateItemRelationQuery EquMaintenanceTemplateItemRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquMaintenanceTemplateItemRelationEntitiesSqlTemplate);

            sqlBuilder.Select("*");

            if (EquMaintenanceTemplateItemRelationQuery.MaintenanceTemplateIds != null && EquMaintenanceTemplateItemRelationQuery.MaintenanceTemplateIds.Any())
            {
                sqlBuilder.Where("MaintenanceTemplateId IN @MaintenanceTemplateIds");
            }

            if (EquMaintenanceTemplateItemRelationQuery.MaintenanceItemIds != null && EquMaintenanceTemplateItemRelationQuery.MaintenanceItemIds.Any())
            {
                sqlBuilder.Where("MaintenanceItemId IN @MaintenanceItemIds");
            }

            using var conn = GetMESDbConnection();
            var EquMaintenanceTemplateItemRelationEntities = await conn.QueryAsync<EquMaintenanceTemplateItemRelationEntity>(template.RawSql, EquMaintenanceTemplateItemRelationQuery);
            return EquMaintenanceTemplateItemRelationEntities;
        }

        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenanceTemplateItemRelationEntity EquMaintenanceTemplateItemRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, EquMaintenanceTemplateItemRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquMaintenanceTemplateItemRelationEntity> EquMaintenanceTemplateItemRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, EquMaintenanceTemplateItemRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenanceTemplateItemRelationEntity EquMaintenanceTemplateItemRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, EquMaintenanceTemplateItemRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="EquMaintenanceTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquMaintenanceTemplateItemRelationEntity> EquMaintenanceTemplateItemRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, EquMaintenanceTemplateItemRelationEntitys);
        }
        #endregion

    }

    public partial class EquMaintenanceTemplateItemRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_Maintenance_template_item_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_Maintenance_template_item_relation` /**where**/ ";
        const string GetEquMaintenanceTemplateItemRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_Maintenance_template_item_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_Maintenance_template_item_relation`(  `MaintenanceTemplateId`, `MaintenanceItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`) VALUES (   @MaintenanceTemplateId, @MaintenanceItemId, @LowerLimit, @Center, @UpperLimit, @CreatedBy, @CreatedOn )  ";
        const string InsertsSql = "INSERT INTO `equ_Maintenance_template_item_relation`(  `MaintenanceTemplateId`, `MaintenanceItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`) VALUES (   @MaintenanceTemplateId, @MaintenanceItemId, @LowerLimit, @Center, @UpperLimit, @CreatedBy, @CreatedOn )  ";

        const string UpdateSql = "UPDATE `equ_Maintenance_template_item_relation` SET   MaintenanceTemplateId = @MaintenanceTemplateId, MaintenanceItemId = @MaintenanceItemId, LowerLimit = @LowerLimit, Center = @Center, UpperLimit = @UpperLimit, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_Maintenance_template_item_relation` SET   MaintenanceTemplateId = @MaintenanceTemplateId, MaintenanceItemId = @MaintenanceItemId, LowerLimit = @LowerLimit, Center = @Center, UpperLimit = @UpperLimit, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE  MaintenanceTemplateId = @MaintenanceTemplateId AND MaintenanceItemId=@MaintenanceItemId ";

        const string DeleteSql = "UPDATE `equ_Maintenance_template_item_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_Maintenance_template_item_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeletesByTemplateIdAndItemIdsSql = "DELETE FROM `equ_Maintenance_template_item_relation`  WHERE MaintenanceTemplateId = @MaintenanceTemplateId AND MaintenanceItemId IN @MaintenanceItemIds";
        const string DeleteByMaintenanceTemplateIdsSql = "DELETE FROM `equ_Maintenance_template_item_relation`  WHERE MaintenanceTemplateId IN @MaintenanceTemplateIds";

        const string GetByIdSql = @"SELECT 
                               `MaintenanceTemplateId`, `MaintenanceItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_item_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `MaintenanceTemplateId`, `MaintenanceItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_item_relation`  WHERE Id IN @Ids ";

        const string GetByTemplateIdAndItemIdSql = @"SELECT  
                                          `MaintenanceTemplateId`, `MaintenanceItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`
                            FROM `equ_Maintenance_template_item_relation`  WHERE MaintenanceTemplateId = @MaintenanceTemplateId AND MaintenanceItemId IN @MaintenanceItemIds ";
        #endregion
    }
}
