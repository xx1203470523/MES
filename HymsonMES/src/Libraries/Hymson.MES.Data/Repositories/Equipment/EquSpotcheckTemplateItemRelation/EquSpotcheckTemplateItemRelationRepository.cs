/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation
{
    /// <summary>
    /// 设备点检模板与项目关系仓储
    /// </summary>
    public partial class EquSpotcheckTemplateItemRelationRepository : BaseRepository, IEquSpotcheckTemplateItemRelationRepository
    {

        public EquSpotcheckTemplateItemRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<int> DeleteBySpotCheckTemplateIdsAsync(IEnumerable<long> spotCheckTemplateIds)
        {
            using var conn = GetMESDbConnection(); 
            return await conn.ExecuteAsync(DeleteBySpotCheckTemplateIdsSql, new { SpotCheckTemplateIds = spotCheckTemplateIds });
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
        public async Task<EquSpotcheckTemplateItemRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckTemplateItemRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateItemRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTemplateItemRelationEntity>(GetByIdsSql, new { Ids = ids });
        }


        /// <summary>
        /// 根据ID获取数据（组合）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateItemRelationEntity>> GetByTemplateIdAndItemIdSqlAsync(GetByTemplateIdAndItemIdQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckTemplateItemRelationEntity>(GetByTemplateIdAndItemIdSql, param);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTemplateItemRelationEntity>> GetPagedInfoAsync(EquSpotcheckTemplateItemRelationPagedQuery equSpotcheckTemplateItemRelationPagedQuery)
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

            var offSet = (equSpotcheckTemplateItemRelationPagedQuery.PageIndex - 1) * equSpotcheckTemplateItemRelationPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSpotcheckTemplateItemRelationPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSpotcheckTemplateItemRelationPagedQuery);

            using var conn = GetMESDbConnection();
            var equSpotcheckTemplateItemRelationEntitiesTask = conn.QueryAsync<EquSpotcheckTemplateItemRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSpotcheckTemplateItemRelationEntities = await equSpotcheckTemplateItemRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckTemplateItemRelationEntity>(equSpotcheckTemplateItemRelationEntities, equSpotcheckTemplateItemRelationPagedQuery.PageIndex, equSpotcheckTemplateItemRelationPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckTemplateItemRelationEntity>> GetEquSpotcheckTemplateItemRelationEntitiesAsync(EquSpotcheckTemplateItemRelationQuery equSpotcheckTemplateItemRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSpotcheckTemplateItemRelationEntitiesSqlTemplate);

            sqlBuilder.Select("*");

            if (equSpotcheckTemplateItemRelationQuery.SpotCheckTemplateIds != null && equSpotcheckTemplateItemRelationQuery.SpotCheckTemplateIds.Any())
            {
                sqlBuilder.Where("SpotCheckTemplateId IN @SpotCheckTemplateIds");
            }

            using var conn = GetMESDbConnection();
            var equSpotcheckTemplateItemRelationEntities = await conn.QueryAsync<EquSpotcheckTemplateItemRelationEntity>(template.RawSql, equSpotcheckTemplateItemRelationQuery);
            return equSpotcheckTemplateItemRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckTemplateItemRelationEntity equSpotcheckTemplateItemRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSpotcheckTemplateItemRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSpotcheckTemplateItemRelationEntity> equSpotcheckTemplateItemRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equSpotcheckTemplateItemRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckTemplateItemRelationEntity equSpotcheckTemplateItemRelationEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSpotcheckTemplateItemRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSpotcheckTemplateItemRelationEntity> equSpotcheckTemplateItemRelationEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSpotcheckTemplateItemRelationEntitys);
        }
        #endregion

    }

    public partial class EquSpotcheckTemplateItemRelationRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spotcheck_template_item_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spotcheck_template_item_relation` /**where**/ ";
        const string GetEquSpotcheckTemplateItemRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spotcheck_template_item_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spotcheck_template_item_relation`(  `SpotCheckTemplateId`, `SpotCheckItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`) VALUES (   @SpotCheckTemplateId, @SpotCheckItemId, @LowerLimit, @Center, @UpperLimit, @CreatedBy, @CreatedOn )  ";
        const string InsertsSql = "INSERT INTO `equ_spotcheck_template_item_relation`(  `SpotCheckTemplateId`, `SpotCheckItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`) VALUES (   @SpotCheckTemplateId, @SpotCheckItemId, @LowerLimit, @Center, @UpperLimit, @CreatedBy, @CreatedOn )  ";

        const string UpdateSql = "UPDATE `equ_spotcheck_template_item_relation` SET   SpotCheckTemplateId = @SpotCheckTemplateId, SpotCheckItemId = @SpotCheckItemId, LowerLimit = @LowerLimit, Center = @Center, UpperLimit = @UpperLimit, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spotcheck_template_item_relation` SET   SpotCheckTemplateId = @SpotCheckTemplateId, SpotCheckItemId = @SpotCheckItemId, LowerLimit = @LowerLimit, Center = @Center, UpperLimit = @UpperLimit, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE  SpotCheckTemplateId = @SpotCheckTemplateId AND SpotCheckItemId=@SpotCheckItemId ";

        const string DeleteSql = "UPDATE `equ_spotcheck_template_item_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spotcheck_template_item_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeletesByTemplateIdAndItemIdsSql = "DELETE FROM `equ_spotcheck_template_item_relation`  WHERE SpotCheckTemplateId = @SpotCheckTemplateId AND SpotCheckItemId IN @SpotCheckItemIds";
        const string DeleteBySpotCheckTemplateIdsSql = "DELETE FROM `equ_spotcheck_template_item_relation`  WHERE SpotCheckTemplateId IN @SpotCheckTemplateIds";

        const string GetByIdSql = @"SELECT 
                               `SpotCheckTemplateId`, `SpotCheckItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_item_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `SpotCheckTemplateId`, `SpotCheckItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_item_relation`  WHERE Id IN @Ids ";

        const string GetByTemplateIdAndItemIdSql = @"SELECT  
                                          `SpotCheckTemplateId`, `SpotCheckItemId`, `LowerLimit`, `Center`, `UpperLimit`, `CreatedBy`, `CreatedOn`
                            FROM `equ_spotcheck_template_item_relation`  WHERE SpotCheckTemplateId = @SpotCheckTemplateId AND SpotCheckItemId IN @SpotCheckItemIds ";
        #endregion
    }
}
