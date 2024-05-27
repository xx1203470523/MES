/*
 *creator: Karl
 *
 *describe: 档次 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:34:14
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
    /// 档次仓储
    /// </summary>
    public partial class ProcSortingRuleGradeRepository :BaseRepository, IProcSortingRuleGradeRepository
    {
        public ProcSortingRuleGradeRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcSortingRuleGradeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleGradeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleGradeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleGradeEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRuleGradePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSortingRuleGradeEntity>> GetPagedInfoAsync(ProcSortingRuleGradePagedQuery procSortingRuleGradePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (procSortingRuleGradePagedQuery.PageIndex - 1) * procSortingRuleGradePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procSortingRuleGradePagedQuery.PageSize });
            sqlBuilder.AddParameters(procSortingRuleGradePagedQuery);

            using var conn = GetMESDbConnection();
            var procSortingRuleGradeEntitiesTask = conn.QueryAsync<ProcSortingRuleGradeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procSortingRuleGradeEntities = await procSortingRuleGradeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcSortingRuleGradeEntity>(procSortingRuleGradeEntities, procSortingRuleGradePagedQuery.PageIndex, procSortingRuleGradePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procSortingRuleGradeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleGradeEntity>> GetProcSortingRuleGradeEntitiesAsync(ProcSortingRuleGradeQuery procSortingRuleGradeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcSortingRuleGradeEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procSortingRuleGradeEntities = await conn.QueryAsync<ProcSortingRuleGradeEntity>(template.RawSql, procSortingRuleGradeQuery);
            return procSortingRuleGradeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleGradeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcSortingRuleGradeEntity procSortingRuleGradeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procSortingRuleGradeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleGradeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procSortingRuleGradeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleGradeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcSortingRuleGradeEntity procSortingRuleGradeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procSortingRuleGradeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procSortingRuleGradeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procSortingRuleGradeEntitys);
        }

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="sortingRuleId"></param>
        /// <returns></returns>
        public async Task<int> DeleteSortingRuleGradeByIdAsync(long sortingRuleId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteBySortingRuleGradeByIdAsyncIdSql, new { SortingRuleId = sortingRuleId });
        }

        /// <summary>
        /// 根据分选规则id获取档位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleGradeEntity>> GetSortingRuleGradesByIdAsync(long sortingRuleId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleGradeEntity>(GetBySortingRuleGradeByIdAsyncIdSql, new { SortingRuleId = sortingRuleId });
        }
        #endregion
    }

    public partial class ProcSortingRuleGradeRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_sorting_rule_grade` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_sorting_rule_grade` /**where**/ ";
        const string GetProcSortingRuleGradeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_sorting_rule_grade` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_sorting_rule_grade`(  `Id`, `SiteId`, `SortingRuleId`, `Grade`, `Priority`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SortingRuleId, @Grade, @Priority, @remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_sorting_rule_grade`(  `Id`, `SiteId`, `SortingRuleId`, `Grade`, `Priority`, `remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SortingRuleId, @Grade, @Priority, @remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_sorting_rule_grade` SET   SiteId = @SiteId, SortingRuleId = @SortingRuleId, Grade = @Grade, Priority = @Priority, remark = @remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_sorting_rule_grade` SET   SiteId = @SiteId, SortingRuleId = @SortingRuleId, Grade = @Grade, Priority = @Priority, remark = @remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_sorting_rule_grade` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_sorting_rule_grade` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `proc_sorting_rule_grade`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_sorting_rule_grade`  WHERE Id IN @Ids ";
        const string GetBySortingRuleGradeByIdAsyncIdSql = @"SELECT * FROM `proc_sorting_rule_grade`  WHERE SortingRuleId = @SortingRuleId AND  IsDeleted=0";
        const string DeleteBySortingRuleGradeByIdAsyncIdSql = "DELETE FROM`proc_sorting_rule_grade` WHERE SortingRuleId = @SortingRuleId";
        #endregion
    }
}
