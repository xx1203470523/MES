using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedGroup.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合格代码组仓储
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public partial class QualUnqualifiedGroupRepository : IQualUnqualifiedGroupRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public QualUnqualifiedGroupRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        #region 不合格代码组
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pram"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedGroupEntity>> GetPagedInfoAsync(QualUnqualifiedGroupPagedQuery pram)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(pram.UnqualifiedCode))
            {
                sqlBuilder.Where("UnqualifiedCode like %@UnqualifiedCode%");
            }

            if (!string.IsNullOrWhiteSpace(pram.UnqualifiedCodeName))
            {
                sqlBuilder.Where("UnqualifiedCodeName like %@UnqualifiedCodeName%");
            }

            if (!string.IsNullOrWhiteSpace(pram.Status))
            {
                sqlBuilder.Where("Status=@Status");
            }

            if (pram.Type != null)
            {
                sqlBuilder.Where("Type=@Type");
            }

            var offSet = (pram.PageIndex - 1) * pram.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pram.PageSize });
            sqlBuilder.AddParameters(pram);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedGroupEntitiesTask = conn.QueryAsync<QualUnqualifiedGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualUnqualifiedGroupEntities = await qualUnqualifiedGroupEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualUnqualifiedGroupEntity>(qualUnqualifiedGroupEntities, pram.PageIndex, pram.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="qualUnqualifiedGroupQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupEntity>> GetQualUnqualifiedGroupEntitiesAsync(QualUnqualifiedGroupQuery qualUnqualifiedGroupQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetQualUnqualifiedGroupEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var qualUnqualifiedGroupEntities = await conn.QueryAsync<QualUnqualifiedGroupEntity>(template.RawSql, qualUnqualifiedGroupQuery);
            return qualUnqualifiedGroupEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedGroupEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<QualUnqualifiedGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<QualUnqualifiedGroupEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualUnqualifiedGroupEntity qualUnqualifiedGroupEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, qualUnqualifiedGroupEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<QualUnqualifiedGroupEntity> qualUnqualifiedGroupEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, qualUnqualifiedGroupEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualUnqualifiedGroupEntity qualUnqualifiedGroupEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, qualUnqualifiedGroupEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="qualUnqualifiedGroupEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<QualUnqualifiedGroupEntity> qualUnqualifiedGroupEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, qualUnqualifiedGroupEntitys);
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
        public async Task<int> DeletesAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }
        #endregion

        #region 不合格代码组关联不合格代码
        /// <summary>
        /// 插入不合格代码组关联不合格代码
        /// </summary>
        /// <param name="qualUnqualifiedCodeGroupRelationList"></param>
        /// <returns></returns>
        public async Task<int> AddQualUnqualifiedCodeGroupRelationAsync(List<QualUnqualifiedCodeGroupRelation> qualUnqualifiedCodeGroupRelationList)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertQualUnqualifiedCodeGroupRelationSql, qualUnqualifiedCodeGroupRelationList);
        }

        /// <summary>
        /// 删除不合格代码组关联不合格代码
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long groupId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DelteQualUnqualifiedCodeGroupRelationSql, groupId);
        }
        #endregion

        #region 不合格代码组关联工序
        /// <summary>
        /// 插入不合格代码组关联工序
        /// </summary>
        /// <param name="qualUnqualifiedGroupProcedureRelationList"></param>
        /// <returns></returns>
        public async Task<int> AddQualUnqualifiedGroupProcedureRelationAsync(List<QualUnqualifiedGroupProcedureRelation> qualUnqualifiedGroupProcedureRelationList)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertQualUnqualifiedGroupProcedureRelationSql, qualUnqualifiedGroupProcedureRelationList);
        }

        /// <summary>
        /// 删除不合格代码组关联工序
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long groupId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(RealDelteQualUnqualifiedGroupProcedureRelationSql, groupId);
        }
        #endregion
    }


    public partial class QualUnqualifiedGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_unqualified_group` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `qual_unqualified_group` /**where**/ ";
        const string GetQualUnqualifiedGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_unqualified_group` /**where**/  ";

        const string InsertSql = "INSERT INTO `qual_unqualified_group`(  `Id`, `SiteCode`, `UnqualifiedGroup`, `UnqualifiedGroupName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @UnqualifiedGroup, @UnqualifiedGroupName, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `qual_unqualified_group`(  `Id`, `SiteCode`, `UnqualifiedGroup`, `UnqualifiedGroupName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @UnqualifiedGroup, @UnqualifiedGroupName, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertQualUnqualifiedCodeGroupRelationSql = "INSERT INTO `qual_unqualified_code`(`Id`, `SiteCode`, `UnqualifiedCode`, `UnqualifiedCodeName`, `Status`, `Type`, `Degree`, `ProcessRouteId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`)";
        const string InsertQualUnqualifiedGroupProcedureRelationSql = "";
        const string UpdateSql = "UPDATE `qual_unqualified_group` SET   SiteCode = @SiteCode, UnqualifiedGroup = @UnqualifiedGroup, UnqualifiedGroupName = @UnqualifiedGroupName, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `qual_unqualified_group` SET   SiteCode = @SiteCode, UnqualifiedGroup = @UnqualifiedGroup, UnqualifiedGroupName = @UnqualifiedGroupName, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `qual_unqualified_group` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `qual_unqualified_group` SET IsDeleted = '1' WHERE Id in @ids";
        const string DelteQualUnqualifiedCodeGroupRelationSql = "";
        const string RealDelteQualUnqualifiedGroupProcedureRelationSql = "";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `UnqualifiedGroup`, `UnqualifiedGroupName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_unqualified_group`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `UnqualifiedGroup`, `UnqualifiedGroupName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `qual_unqualified_group`  WHERE Id IN @ids ";
    }
}
