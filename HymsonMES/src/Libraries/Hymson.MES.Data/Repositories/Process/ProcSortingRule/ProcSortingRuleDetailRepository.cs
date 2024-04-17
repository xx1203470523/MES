/*
 *creator: Karl
 *
 *describe: 分选规则详情 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:25:19
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则详情仓储
    /// </summary>
    public partial class ProcSortingRuleDetailRepository : BaseRepository, IProcSortingRuleDetailRepository
    {

        public ProcSortingRuleDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ProcSortingRuleDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRuleDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSortingRuleDetailEntity>> GetPagedInfoAsync(ProcSortingRuleDetailPagedQuery procSortingRuleDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            var offSet = (procSortingRuleDetailPagedQuery.PageIndex - 1) * procSortingRuleDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procSortingRuleDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(procSortingRuleDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var procSortingRuleDetailEntitiesTask = conn.QueryAsync<ProcSortingRuleDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procSortingRuleDetailEntities = await procSortingRuleDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcSortingRuleDetailEntity>(procSortingRuleDetailEntities, procSortingRuleDetailPagedQuery.PageIndex, procSortingRuleDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procSortingRuleDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailEntity>> GetProcSortingRuleDetailEntitiesAsync(ProcSortingRuleDetailQuery procSortingRuleDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (procSortingRuleDetailQuery.SortingRuleId.HasValue)
            {
                sqlBuilder.Where("SortingRuleId=@SortingRuleId");
            }
            if (procSortingRuleDetailQuery.SortingRuleIds!=null&& procSortingRuleDetailQuery.SortingRuleIds!.Any())
            {
                sqlBuilder.Where("SortingRuleId in @SortingRuleIds");
            }

            var template = sqlBuilder.AddTemplate(GetProcSortingRuleDetailEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procSortingRuleDetailEntities = await conn.QueryAsync<ProcSortingRuleDetailEntity>(template.RawSql, procSortingRuleDetailQuery);
            return procSortingRuleDetailEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcSortingRuleDetailEntity procSortingRuleDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procSortingRuleDetailEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ProcSortingRuleDetailEntity> procSortingRuleDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procSortingRuleDetailEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcSortingRuleDetailEntity procSortingRuleDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procSortingRuleDetailEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procSortingRuleDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<ProcSortingRuleDetailEntity> procSortingRuleDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procSortingRuleDetailEntitys);
        }

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteSortingRuleDetailByIdAsync(long sortingRuleId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteBySortingRuleDetailIdSql, new { SortingRuleId = sortingRuleId });
        }

        /// <summary>
        /// 根据分选规则id获取参数信息
        /// </summary>
        /// <param name="sortingRuleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailEntity>> GetSortingRuleDetailByIdAsync(long sortingRuleId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleDetailEntity>(GetBySortingRuleIdSql, new { SortingRuleId = sortingRuleId });
        }
        #endregion
    }

    public partial class ProcSortingRuleDetailRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_sorting_rule_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_sorting_rule_detail` /**where**/ ";
        const string GetProcSortingRuleDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_sorting_rule_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_sorting_rule_detail`(  `Id`, `SiteId`, `SortingRuleId`, `Serial`, `ProcedureId`, `ParameterId`, `MinValue`, `MinContainingType`, `MaxValue`, `MaxContainingType`, `ParameterValue`, `Rating`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SortingRuleId,@Serial, @ProcedureId, @ParameterId, @MinValue, @MinContainingType, @MaxValue, @MaxContainingType, @ParameterValue, @Rating, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_sorting_rule_detail`(  `Id`, `SiteId`, `SortingRuleId`, `Serial`, `ProcedureId`, `ParameterId`, `MinValue`, `MinContainingType`, `MaxValue`, `MaxContainingType`, `ParameterValue`, `Rating`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SortingRuleId,@Serial, @ProcedureId, @ParameterId, @MinValue, @MinContainingType, @MaxValue, @MaxContainingType, @ParameterValue, @Rating, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_sorting_rule_detail` SET   SiteId = @SiteId, SortingRuleId = @SortingRuleId, ProcedureId = @ProcedureId, ParameterId = @ParameterId, MinValue = @MinValue, MinContainingType = @MinContainingType, MaxValue = @MaxValue, MaxContainingType = @MaxContainingType, ParameterValue = @ParameterValue, Rating = @Rating, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_sorting_rule_detail` SET   SiteId = @SiteId, SortingRuleId = @SortingRuleId, ProcedureId = @ProcedureId, ParameterId = @ParameterId, MinValue = @MinValue, MinContainingType = @MinContainingType, MaxValue = @MaxValue, MaxContainingType = @MaxContainingType, ParameterValue = @ParameterValue, Rating = @Rating, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_sorting_rule_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_sorting_rule_detail` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SortingRuleId`, `Serial`, `ProcedureId`, `ParameterId`, `MinValue`, `MinContainingType`, `MaxValue`, `MaxContainingType`, `ParameterValue`, `Rating`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SortingRuleId`, `Serial`, `ProcedureId`, `ParameterId`, `MinValue`, `MinContainingType`, `MaxValue`, `MaxContainingType`, `ParameterValue`, `Rating`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule_detail`  WHERE Id IN @Ids ";
        const string GetBySortingRuleIdSql = @"SELECT 
                                          `Id`, `SiteId`, `SortingRuleId`, `Serial`, `ProcedureId`, `ParameterId`, `MinValue`, `MinContainingType`, `MaxValue`, `MaxContainingType`, `ParameterValue`, `Rating`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule_detail`  WHERE  SortingRuleId = @SortingRuleId  AND IsDeleted=0  ORDER BY Serial ";
        const string DeleteBySortingRuleDetailIdSql = "DELETE FROM `proc_sorting_rule_detail` WHERE SortingRuleId = @SortingRuleId";
        #endregion
    }
}
