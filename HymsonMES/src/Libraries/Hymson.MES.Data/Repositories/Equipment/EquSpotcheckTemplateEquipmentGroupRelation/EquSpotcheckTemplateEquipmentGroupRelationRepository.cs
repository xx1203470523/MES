/*
 *creator: Karl
 *
 *describe: 设备点检模板与设备组关系 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:22
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSpotcheckTemplateEquipmentGroupRelation;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplateEquipmentGroupRelation
{
    /// <summary>
    /// 设备点检模板与设备组关系仓储
    /// </summary>
    public partial class EquSpotcheckTemplateEquipmentGroupRelationRepository : BaseRepository, IEquSpotcheckTemplateEquipmentGroupRelationRepository
    {

        public EquSpotcheckTemplateEquipmentGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<int> DeletesBySpotCheckTemplateIdsAsync(IEnumerable<long> spotCheckTemplateIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesBySpotCheckTemplateIdSql, new { SpotCheckTemplateIds = spotCheckTemplateIds });
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public async Task<int> DeletesByTemplateIdAndGroupIdsAsync(GetByTemplateIdAndGroupIdQuery spotCheckTemplateIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByTemplateIdAndGroupIdsSql, spotCheckTemplateIds);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSpotcheckTemplateEquipmentGroupRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTemplateEquipmentGroupRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTemplateEquipmentGroupRelationEntity>(GetByIdsSql, new { Ids = ids });
        }


        /// <summary>
        /// 根据IDs批量获取数据(组合)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetByTemplateIdAndGroupIdAsync(GetByTemplateIdAndGroupIdQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTemplateEquipmentGroupRelationEntity>(GetByTemplateIdAndGroupIdSql, param);
        }

        /// <summary>
        /// 根据GroupId批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns> 
        public async Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetByGroupIdAsync(IEnumerable<long> groupIdSql)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTemplateEquipmentGroupRelationEntity>(GetByGroupIdSql, new { EquipmentGroupIds = groupIdSql });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetPagedInfoAsync(EquSpotcheckTemplateEquipmentGroupRelationPagedQuery equSpotcheckTemplateEquipmentGroupRelationPagedQuery)
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

            var offSet = (equSpotcheckTemplateEquipmentGroupRelationPagedQuery.PageIndex - 1) * equSpotcheckTemplateEquipmentGroupRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSpotcheckTemplateEquipmentGroupRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSpotcheckTemplateEquipmentGroupRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var equSpotcheckTemplateEquipmentGroupRelationEntitiesTask = conn.QueryAsync<EquSpotcheckTemplateEquipmentGroupRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSpotcheckTemplateEquipmentGroupRelationEntities = await equSpotcheckTemplateEquipmentGroupRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckTemplateEquipmentGroupRelationEntity>(equSpotcheckTemplateEquipmentGroupRelationEntities, equSpotcheckTemplateEquipmentGroupRelationPagedQuery.PageIndex, equSpotcheckTemplateEquipmentGroupRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateEquipmentGroupRelationEntity>> GetEquSpotcheckTemplateEquipmentGroupRelationEntitiesAsync(EquSpotcheckTemplateEquipmentGroupRelationQuery equSpotcheckTemplateEquipmentGroupRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSpotcheckTemplateEquipmentGroupRelationEntitiesSqlTemplate);

            sqlBuilder.Select("*");

            if (equSpotcheckTemplateEquipmentGroupRelationQuery.SpotCheckTemplateIds != null && equSpotcheckTemplateEquipmentGroupRelationQuery.SpotCheckTemplateIds.Any())
            {
                sqlBuilder.Where("SpotCheckTemplateId IN @SpotCheckTemplateIds");
            }

            using var conn = GetMESDbConnection();
            var equSpotcheckTemplateEquipmentGroupRelationEntities = await conn.QueryAsync<EquSpotcheckTemplateEquipmentGroupRelationEntity>(template.RawSql, equSpotcheckTemplateEquipmentGroupRelationQuery);
            return equSpotcheckTemplateEquipmentGroupRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckTemplateEquipmentGroupRelationEntity equSpotcheckTemplateEquipmentGroupRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSpotcheckTemplateEquipmentGroupRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSpotcheckTemplateEquipmentGroupRelationEntity> equSpotcheckTemplateEquipmentGroupRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equSpotcheckTemplateEquipmentGroupRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckTemplateEquipmentGroupRelationEntity equSpotcheckTemplateEquipmentGroupRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSpotcheckTemplateEquipmentGroupRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSpotcheckTemplateEquipmentGroupRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSpotcheckTemplateEquipmentGroupRelationEntity> equSpotcheckTemplateEquipmentGroupRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSpotcheckTemplateEquipmentGroupRelationEntitys);
        }
        #endregion

    }

    public partial class EquSpotcheckTemplateEquipmentGroupRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spotcheck_template_equipment_group_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spotcheck_template_equipment_group_relation` /**where**/ ";
        const string GetEquSpotcheckTemplateEquipmentGroupRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spotcheck_template_equipment_group_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spotcheck_template_equipment_group_relation`(  `SpotCheckTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) VALUES (   @SpotCheckTemplateId, @EquipmentGroupId, @CreatedBy, @CreatedOn )  ";
        const string InsertsSql = "INSERT INTO `equ_spotcheck_template_equipment_group_relation`(  `SpotCheckTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) VALUES (   @SpotCheckTemplateId, @EquipmentGroupId, @CreatedBy, @CreatedOn )  ";

        const string UpdateSql = "UPDATE `equ_spotcheck_template_equipment_group_relation` SET   SpotCheckTemplateId = @SpotCheckTemplateId, EquipmentGroupId = @EquipmentGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spotcheck_template_equipment_group_relation` SET   SpotCheckTemplateId = @SpotCheckTemplateId, EquipmentGroupId = @EquipmentGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE  SpotCheckTemplateId=@SpotCheckTemplateId AND EquipmentGroupId=@EquipmentGroupId ";

        const string DeleteSql = "UPDATE `equ_spotcheck_template_equipment_group_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spotcheck_template_equipment_group_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeletesBySpotCheckTemplateIdSql = "DELETE FROM `equ_spotcheck_template_equipment_group_relation` WHERE  SpotCheckTemplateId IN @SpotCheckTemplateIds";

        const string DeletesByTemplateIdAndGroupIdsSql = "DELETE FROM `equ_spotcheck_template_equipment_group_relation` WHERE  SpotCheckTemplateId=@SpotCheckTemplateId AND EquipmentGroupId IN @EquipmentGroupIds ";


        const string GetByIdSql = @"SELECT 
                               `SpotCheckTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_equipment_group_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `SpotCheckTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_equipment_group_relation`  WHERE Id IN @Ids ";

        const string GetByTemplateIdAndGroupIdSql = @"SELECT  
                                          `SpotCheckTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_equipment_group_relation`  WHERE  SpotCheckTemplateId=@SpotCheckTemplateId AND EquipmentGroupId IN @EquipmentGroupIds ";

        const string GetByGroupIdSql = @"SELECT   
                                          `SpotCheckTemplateId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_equipment_group_relation`  WHERE  EquipmentGroupId IN @EquipmentGroupIds ";
        #endregion
    }
}
