/*
 *creator: Karl
 *
 *describe: 分选规则 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则仓储
    /// </summary>
    public partial class ProcSortingRuleRepository :BaseRepository, IProcSortingRuleRepository
    {

        public ProcSortingRuleRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcSortingRuleEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procSortingRulePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSortingRuleEntity>> GetPagedInfoAsync(ProcSortingRulePagedQuery procSortingRulePagedQuery)
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
           
            var offSet = (procSortingRulePagedQuery.PageIndex - 1) * procSortingRulePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procSortingRulePagedQuery.PageSize });
            sqlBuilder.AddParameters(procSortingRulePagedQuery);

            using var conn = GetMESDbConnection();
            var procSortingRuleEntitiesTask = conn.QueryAsync<ProcSortingRuleEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procSortingRuleEntities = await procSortingRuleEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcSortingRuleEntity>(procSortingRuleEntities, procSortingRulePagedQuery.PageIndex, procSortingRulePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procSortingRuleQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleEntity>> GetProcSortingRuleEntitiesAsync(ProcSortingRuleQuery procSortingRuleQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcSortingRuleEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procSortingRuleEntities = await conn.QueryAsync<ProcSortingRuleEntity>(template.RawSql, procSortingRuleQuery);
            return procSortingRuleEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procSortingRuleEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcSortingRuleEntity procSortingRuleEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procSortingRuleEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procSortingRuleEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcSortingRuleEntity> procSortingRuleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procSortingRuleEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procSortingRuleEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcSortingRuleEntity procSortingRuleEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procSortingRuleEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procSortingRuleEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcSortingRuleEntity> procSortingRuleEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procSortingRuleEntitys);
        }
        #endregion

    }

    public partial class ProcSortingRuleRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_sorting_rule` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_sorting_rule` /**where**/ ";
        const string GetProcSortingRuleEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_sorting_rule` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_sorting_rule`(  `Id`, `SiteId`, `Code`, `Name`, `Version`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Version, @MaterialId, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_sorting_rule`(  `Id`, `SiteId`, `Code`, `Name`, `Version`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Version, @MaterialId, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_sorting_rule` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Version = @Version, MaterialId = @MaterialId, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_sorting_rule` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Version = @Version, MaterialId = @MaterialId, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_sorting_rule` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_sorting_rule` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `Version`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Version`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`  WHERE Id IN @Ids ";
        #endregion
    }
}
