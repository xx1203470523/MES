/*
 *creator: Karl
 *
 *describe: 设备点检计划与设备关系 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 03:51:20
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSpotcheckPlanEquipmentRelation;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSpotcheckPlanEquipmentRelation
{
    /// <summary>
    /// 设备点检计划与设备关系仓储
    /// </summary>
    public partial class EquSpotcheckPlanEquipmentRelationRepository : BaseRepository, IEquSpotcheckPlanEquipmentRelationRepository
    {

        public EquSpotcheckPlanEquipmentRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<int> PhysicalDeletesAsync(IEnumerable<long> spotCheckPlanIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(PhysicalDeletesSql, new { SpotCheckPlanIds = spotCheckPlanIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckPlanEquipmentRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckPlanEquipmentRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckPlanEquipmentRelationEntity>(GetByIdsSql, new { Ids = ids });
        }



        /// <summary>
        /// 根据SpotCheckPlanId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetBySpotCheckPlanIdsAsync(long spotCheckPlanId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckPlanEquipmentRelationEntity>(GetByspotCheckPlanIdsSql, new { SpotCheckPlanId = spotCheckPlanId });
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckPlanEquipmentRelationEntity>> GetPagedInfoAsync(EquSpotcheckPlanEquipmentRelationPagedQuery equSpotcheckPlanEquipmentRelationPagedQuery)
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

            var offSet = (equSpotcheckPlanEquipmentRelationPagedQuery.PageIndex - 1) * equSpotcheckPlanEquipmentRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSpotcheckPlanEquipmentRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSpotcheckPlanEquipmentRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var equSpotcheckPlanEquipmentRelationEntitiesTask = conn.QueryAsync<EquSpotcheckPlanEquipmentRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSpotcheckPlanEquipmentRelationEntities = await equSpotcheckPlanEquipmentRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckPlanEquipmentRelationEntity>(equSpotcheckPlanEquipmentRelationEntities, equSpotcheckPlanEquipmentRelationPagedQuery.PageIndex, equSpotcheckPlanEquipmentRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckPlanEquipmentRelationEntity>> GetEquSpotcheckPlanEquipmentRelationEntitiesAsync(EquSpotcheckPlanEquipmentRelationQuery equSpotcheckPlanEquipmentRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSpotcheckPlanEquipmentRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equSpotcheckPlanEquipmentRelationEntities = await conn.QueryAsync<EquSpotcheckPlanEquipmentRelationEntity>(template.RawSql, equSpotcheckPlanEquipmentRelationQuery);
            return equSpotcheckPlanEquipmentRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSpotcheckPlanEquipmentRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equSpotcheckPlanEquipmentRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckPlanEquipmentRelationEntity equSpotcheckPlanEquipmentRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSpotcheckPlanEquipmentRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSpotcheckPlanEquipmentRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSpotcheckPlanEquipmentRelationEntity> equSpotcheckPlanEquipmentRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSpotcheckPlanEquipmentRelationEntitys);
        }
        #endregion

    }

    public partial class EquSpotcheckPlanEquipmentRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spotcheck_plan_equipment_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spotcheck_plan_equipment_relation` /**where**/ ";
        const string GetEquSpotcheckPlanEquipmentRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spotcheck_plan_equipment_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spotcheck_plan_equipment_relation`(  `SpotCheckPlanId`, `SpotCheckTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`) VALUES (   @SpotCheckPlanId, @SpotCheckTemplateId, @EquipmentId, @CreatedBy, @CreatedOn, @ExecutorIds, @LeaderIds )  ";
        const string InsertsSql = "INSERT INTO `equ_spotcheck_plan_equipment_relation`(  `SpotCheckPlanId`, `SpotCheckTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`) VALUES (   @SpotCheckPlanId, @SpotCheckTemplateId, @EquipmentId, @CreatedBy, @CreatedOn, @ExecutorIds, @LeaderIds )  ";

        const string UpdateSql = "UPDATE `equ_spotcheck_plan_equipment_relation` SET   SpotCheckPlanId = @SpotCheckPlanId, SpotCheckTemplateId = @SpotCheckTemplateId, EquipmentId = @EquipmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spotcheck_plan_equipment_relation` SET   SpotCheckPlanId = @SpotCheckPlanId, SpotCheckTemplateId = @SpotCheckTemplateId, EquipmentId = @EquipmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_spotcheck_plan_equipment_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spotcheck_plan_equipment_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string PhysicalDeletesSql = "DELETE  FROM `equ_spotcheck_plan_equipment_relation`  WHERE SpotCheckPlanId IN @SpotCheckPlanIds";

        const string GetByIdSql = @"SELECT 
                               `SpotCheckPlanId`, `SpotCheckTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_spotcheck_plan_equipment_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `SpotCheckPlanId`, `SpotCheckTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_spotcheck_plan_equipment_relation`  WHERE Id IN @Ids ";
        const string GetByspotCheckPlanIdsSql = @"SELECT  
                                          `SpotCheckPlanId`, `SpotCheckTemplateId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_spotcheck_plan_equipment_relation`  WHERE SpotCheckPlanId = @SpotCheckPlanId ";
        #endregion
    }
}
