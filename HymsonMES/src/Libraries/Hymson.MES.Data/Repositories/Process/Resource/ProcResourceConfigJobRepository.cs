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
            return await conn.ExecuteAsync(DeletesSql, new { ids=ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceConfigJobEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceConfigJobEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceConfigJobEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceConfigJobEntity>(GetByIdsSql, new { ids = ids});
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
            sqlBuilder.Where("IsDeleted=0");
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
        /// 查询List
        /// </summary>
        /// <param name="procResourceConfigJobQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceConfigJobEntity>> GetProcResourceConfigJobEntitiesAsync(ProcResourceConfigJobQuery procResourceConfigJobQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcResourceConfigJobEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceConfigJobEntities = await conn.QueryAsync<ProcResourceConfigJobEntity>(template.RawSql, procResourceConfigJobQuery);
            return procResourceConfigJobEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigJobEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceConfigJobEntity procResourceConfigJobEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procResourceConfigJobEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procResourceConfigJobEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procResourceConfigJobEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigJobEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceConfigJobEntity procResourceConfigJobEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigJobEntity);
        }

        public Task<int> UpdatesAsync(List<ProcResourceConfigJobEntity> procResourceConfigJobEntitys)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ProcResourceConfigJobRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.JobCode,b.JobName from proc_resource_config_job a left join inte_job  b on a.JobId=b.Id and b.IsDeleted=0 /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_config_job a left join inte_job  b on a.JobId=b.Id and b.IsDeleted=0 /**where**/";

        const string GetProcResourceConfigJobEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_resource_config_job` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_resource_config_job`(  `Id`, `SiteCode`, `ResourceId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ResourceId, @LinkPoint, @JobId, @IsUse, @Parameter, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_resource_config_job`(  `Id`, `SiteCode`, `ResourceId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ResourceId, @LinkPoint, @JobId, @IsUse, @Parameter, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_job` SET   SiteCode = @SiteCode, ResourceId = @ResourceId, LinkPoint = @LinkPoint, JobId = @JobId, IsUse = @IsUse, Parameter = @Parameter, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_resource_config_job` SET   SiteCode = @SiteCode, ResourceId = @ResourceId, LinkPoint = @LinkPoint, JobId = @JobId, IsUse = @IsUse, Parameter = @Parameter, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
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
