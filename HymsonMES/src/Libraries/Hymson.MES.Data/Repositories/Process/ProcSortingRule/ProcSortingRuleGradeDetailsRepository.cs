/*
 *creator: Karl
 *
 *describe: 档位详情 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:23
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 档位详情仓储
    /// </summary>
    public partial class ProcSortingRuleGradeDetailsRepository :BaseRepository, IProcSortingRuleGradeDetailsRepository
    {
        public ProcSortingRuleGradeDetailsRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcSortingRuleGradeDetailsEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleGradeDetailsEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleGradeDetailsEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleGradeDetailsEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSortingRuleGradeDetailsEntity>> GetPagedInfoAsync(ProcSortingRuleGradeDetailsPagedQuery procSortingRuleGradeDetailsPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (procSortingRuleGradeDetailsPagedQuery.PageIndex - 1) * procSortingRuleGradeDetailsPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procSortingRuleGradeDetailsPagedQuery.PageSize });
            sqlBuilder.AddParameters(procSortingRuleGradeDetailsPagedQuery);

            using var conn = GetMESDbConnection();
            var procSortingRuleGradeDetailsEntitiesTask = conn.QueryAsync<ProcSortingRuleGradeDetailsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procSortingRuleGradeDetailsEntities = await procSortingRuleGradeDetailsEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcSortingRuleGradeDetailsEntity>(procSortingRuleGradeDetailsEntities, procSortingRuleGradeDetailsPagedQuery.PageIndex, procSortingRuleGradeDetailsPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleGradeDetailsEntity>> GetProcSortingRuleGradeDetailsEntitiesAsync(ProcSortingRuleGradeDetailsQuery procSortingRuleGradeDetailsQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcSortingRuleGradeDetailsEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procSortingRuleGradeDetailsEntities = await conn.QueryAsync<ProcSortingRuleGradeDetailsEntity>(template.RawSql, procSortingRuleGradeDetailsQuery);
            return procSortingRuleGradeDetailsEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcSortingRuleGradeDetailsEntity procSortingRuleGradeDetailsEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procSortingRuleGradeDetailsEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procSortingRuleGradeDetailsEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcSortingRuleGradeDetailsEntity procSortingRuleGradeDetailsEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procSortingRuleGradeDetailsEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procSortingRuleGradeDetailsEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procSortingRuleGradeDetailsEntitys);
        }

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteSortingRuleGradeByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteBySortingRuleGradeByIdAsyncIdSql, new { SortingRuleId = id });
        }

        /// <summary>
        /// 根据分选规则id获取档位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleGradeDetailsEntity>> GetSortingRuleGradeeDetailsByIdAsync(long sortingRuleId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleGradeDetailsEntity>(GetBySortingRuleGradeByIdAsyncIdSql, new { SortingRuleId = sortingRuleId });
        }
        #endregion
    }

    public partial class ProcSortingRuleGradeDetailsRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_sorting_rule_grade_details` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_sorting_rule_grade_details` /**where**/ ";
        const string GetProcSortingRuleGradeDetailsEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_sorting_rule_grade_details` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_sorting_rule_grade_details`(  `Id`, `SiteId`,`SortingRuleId`, `SortingRuleGradeId`, `SortingRuleDetailId`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId,@SortingRuleId, @SortingRuleGradeId, @SortingRuleDetailId, @remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_sorting_rule_grade_details`(  `Id`, `SiteId`,`SortingRuleId`, `SortingRuleGradeId`, `SortingRuleDetailId`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId,@SortingRuleId, @SortingRuleGradeId, @SortingRuleDetailId, @remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_sorting_rule_grade_details` SET   SiteId = @SiteId, SortingRuleGradeId = @SortingRuleGradeId, SortingRuleDetailId = @SortingRuleDetailId, remark = @remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_sorting_rule_grade_details` SET   SiteId = @SiteId, SortingRuleGradeId = @SortingRuleGradeId, SortingRuleDetailId = @SortingRuleDetailId, remark = @remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_sorting_rule_grade_details` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_sorting_rule_grade_details` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SortingRuleGradeId`, `SortingRuleDetailId`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule_grade_details`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SortingRuleGradeId`, `SortingRuleDetailId`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule_grade_details`  WHERE Id IN @Ids ";
        const string GetBySortingRuleGradeByIdAsyncIdSql = @"SELECT 
                                          `Id`, `SiteId`, `SortingRuleGradeId`, `SortingRuleDetailId`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule_grade_details`   WHERE SortingRuleId = @SortingRuleId AND  IsDeleted=0";
        const string DeleteBySortingRuleGradeByIdAsyncIdSql = "DELETE FROM `proc_sorting_rule_grade` WHERE SortingRuleId = @SortingRuleId";
        #endregion
    }
}
