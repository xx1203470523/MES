/*
 *creator: Karl
 *
 *describe: 资源作业配置表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 05:26:36
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源作业配置表仓储
    /// </summary>
    public partial class ProcResourceConfigJobRepository : IProcResourceConfigJobRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcResourceConfigJobRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigJobView>> GetPagedInfoAsync(ProcResourceConfigJobPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            sqlBuilder.Where("a.ResourceId=@ResourceId");

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceConfigJobEntitiesTask = conn.QueryAsync<ProcResourceConfigJobView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceConfigJobEntities = await procResourceConfigJobEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceConfigJobView>(procResourceConfigJobEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procResourceConfigJobs"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobs)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procResourceConfigJobs);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigJobs"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobs)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigJobs);
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
        public async Task<int> DeletesRangeAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }
    }

    public partial class ProcResourceConfigJobRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.JobCode,b.JobName from proc_resource_config_job a left join inte_job  b on a.JobId=b.Id and b.IsDeleted=0 /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_config_job a left join inte_job  b on a.JobId=b.Id and b.IsDeleted=0 /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_config_job`(  `Id`, `SiteCode`, `ResourceId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteCode, @ResourceId, @LinkPoint, @JobId, @IsUse, @Parameter, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_job` SET  LinkPoint = @LinkPoint, JobId = @JobId, IsUse = @IsUse, Parameter = @Parameter, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_config_job` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_resource_config_job` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ResourceId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_resource_config_job`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `ResourceId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_resource_config_job`  WHERE Id IN @ids ";
    }
}
