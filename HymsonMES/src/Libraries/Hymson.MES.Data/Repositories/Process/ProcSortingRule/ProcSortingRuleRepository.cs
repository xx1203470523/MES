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
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.Query;
using Hymson.MES.Data.Repositories.Process.ProcSortingRule.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 分选规则仓储
    /// </summary>
    public partial class ProcSortingRuleRepository : BaseRepository, IProcSortingRuleRepository
    {

        public ProcSortingRuleRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortingRuleEntity>(GetByIdsSql, new { Ids = ids });
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
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procSortingRulePagedQuery.Code))
            {
                procSortingRulePagedQuery.Code = $"%{procSortingRulePagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }

            if (!string.IsNullOrWhiteSpace(procSortingRulePagedQuery.Name))
            {
                procSortingRulePagedQuery.Name = $"%{procSortingRulePagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }

            if (!string.IsNullOrWhiteSpace(procSortingRulePagedQuery.Version))
            {
                procSortingRulePagedQuery.Version = $"%{procSortingRulePagedQuery.Version}%";
                sqlBuilder.Where("Version like @Version");
            }

            if (procSortingRulePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            if (procSortingRulePagedQuery.MaterialId.HasValue)
            {
                sqlBuilder.Where("MaterialId = @MaterialId");
            }

            if (procSortingRulePagedQuery.MaterialIds!=null&& procSortingRulePagedQuery.MaterialIds.Any())
            {
                sqlBuilder.Where("MaterialId IN @MaterialIds");
            }
            if (string.IsNullOrEmpty(procSortingRulePagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(procSortingRulePagedQuery.Sorting);
            }


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
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");

            if (procSortingRuleQuery.MaterialId.HasValue)
            {
                sqlBuilder.Where("MaterialId=@MaterialId");
            }
            if (procSortingRuleQuery.MaterialIds!=null&& procSortingRuleQuery.MaterialIds.Any())
            {
                sqlBuilder.Where("MaterialId in @MaterialIds");
            }

            if (procSortingRuleQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }
            if (procSortingRuleQuery.IsDefaultVersion.HasValue)
            {
                sqlBuilder.Where("IsDefaultVersion=@IsDefaultVersion");
            }

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

        /// <summary>
        ///根据编码和版本获取分选规则
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<ProcSortingRuleEntity> GetByCodeAndVersion(ProcSortingRuleByCodeAndVersionQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleEntity>(GetByCodeAndVersionSql, param);
        }

        /// <summary>
        /// 根据编码和物料获取分选规则
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<ProcSortingRuleEntity> GetByCodeAndMaterialId(ProcSortingRuleCodeAndMaterialIdQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleEntity>(GetByCodeAndMaterialIdSql, param);
        }

        /// <summary>
        ///更具编码获取当前版本的分选规则
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<ProcSortingRuleEntity> GetByDefaultVersion(ProcSortingRuleByDefaultVersionQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcSortingRuleEntity>(GetByGetByDefaultVersionSql, param);
        }

        #endregion

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        #region 顷刻

        /// <summary>
        /// 根据产品id获取分选规则详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortRuleDetailEquView>> GetSortRuleDetailAsync(ProcSortRuleDetailEquQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcSortRuleDetailEquView>(GetSortRuleDetailEquSql, param);
        }

        #endregion
    }

    public partial class ProcSortingRuleRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_sorting_rule` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/   LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_sorting_rule` /**where**/  ";
        const string GetProcSortingRuleEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_sorting_rule` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_sorting_rule`(  `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Version, @IsDefaultVersion, @MaterialId, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_sorting_rule`(  `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`,`MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Version,  @IsDefaultVersion,@MaterialId, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_sorting_rule` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Version = @Version,IsDefaultVersion = @IsDefaultVersion, MaterialId = @MaterialId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_sorting_rule` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Version = @Version,IsDefaultVersion = @IsDefaultVersion, MaterialId = @MaterialId, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_sorting_rule` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_sorting_rule` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`  WHERE Id IN @Ids ";
        const string GetByCodeAndVersionSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`  WHERE Code = @Code AND  Version=@Version AND SiteId=@SiteId AND  IsDeleted = 0";
        const string GetByCodeAndMaterialIdSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`   WHERE  MaterialId=@MaterialId AND SiteId=@SiteId  AND  IsDeleted = 0 ";

        const string GetByGetByDefaultVersionSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Version`, `IsDefaultVersion`, `MaterialId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_sorting_rule`   WHERE  IsDefaultVersion=1 AND Code=@Code  AND SiteId=@SiteId  AND  IsDeleted = 0 ";
        #endregion

        const string UpdateStatusSql = "UPDATE `proc_sorting_rule` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        #region 顷刻

        /// <summary>
        /// 获取设备用的分选规则
        /// </summary>
        const string GetSortRuleDetailEquSql = @"
            select t1.Code, t1.Name, t1.MaterialId, t2.* ,t3.ParameterCode ,t3.ParameterName 
            from proc_sorting_rule t1
            inner join proc_sorting_rule_detail t2 on t1.Id = t2.SortingRuleId and t2.IsDeleted = 0
            inner join proc_parameter t3 on t3.Id = t2.ParameterId and t3.IsDeleted = 0
            where t1.IsDeleted = 0
            and t1.Status in ('1','2')
            and t1.MaterialId = @MaterialId
        ";

        #endregion
    }
}
