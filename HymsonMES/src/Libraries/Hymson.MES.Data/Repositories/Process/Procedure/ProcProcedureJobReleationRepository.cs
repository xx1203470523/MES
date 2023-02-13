/*
 *creator: Karl
 *
 *describe: 工序配置作业表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:23:23
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序配置作业表仓储
    /// </summary>
    public partial class ProcProcedureJobReleationRepository : IProcProcedureJobReleationRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcProcedureJobReleationRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureJobReleationEntity>> GetPagedInfoAsync(ProcProcedureJobReleationPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("ProcedureId=@ProcedureId");

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedureJobReleationEntitiesTask = conn.QueryAsync<ProcProcedureJobReleationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedureJobReleationEntities = await procProcedureJobReleationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedureJobReleationEntity>(procProcedureJobReleationEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询工序关联的job列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureJobReleationEntity>> GetJobListByProcedureIdAsync(long procedureId)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcedureJobReleationEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("ProcedureId=@ProcedureId");
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedureJobReleationEntities = await conn.QueryAsync<ProcProcedureJobReleationEntity>(template.RawSql, new { ProcedureId = procedureId });
            return procProcedureJobReleationEntities;
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureJobReleationEntity>> GetProcProcedureJobReleationEntitiesAsync(ProcProcedureJobReleationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcedureJobReleationEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedureJobReleationEntities = await conn.QueryAsync<ProcProcedureJobReleationEntity>(template.RawSql, query);
            return procProcedureJobReleationEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedureJobReleationEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcedureJobReleationEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureJobReleationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcedureJobReleationEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedureJobReleationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcedureJobReleationEntity procProcedureJobReleationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcedureJobReleationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcedureJobReleationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcProcedureJobReleationEntity> procProcedureJobReleationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procProcedureJobReleationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedureJobReleationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcedureJobReleationEntity procProcedureJobReleationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcedureJobReleationEntity);
        }
		
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcedureJobReleationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcProcedureJobReleationEntity> procProcedureJobReleationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcedureJobReleationEntitys);
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
        public async Task<int> DeleteRangeAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids=ids });
        }
    }

    public partial class ProcProcedureJobReleationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_procedure_job_releation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_procedure_job_releation` /**where**/ ";
        const string GetProcProcedureJobReleationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_procedure_job_releation` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_procedure_job_releation`(  `Id`, `SiteCode`, `ProcedureId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ProcedureId, @LinkPoint, @JobId, @IsUse, @Parameter, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_procedure_job_releation`(  `Id`, `SiteCode`, `ProcedureId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ProcedureId, @LinkPoint, @JobId, @IsUse, @Parameter, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_procedure_job_releation` SET   SiteCode = @SiteCode, ProcedureId = @ProcedureId, LinkPoint = @LinkPoint, JobId = @JobId, IsUse = @IsUse, Parameter = @Parameter, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_procedure_job_releation` SET   SiteCode = @SiteCode, ProcedureId = @ProcedureId, LinkPoint = @LinkPoint, JobId = @JobId, IsUse = @IsUse, Parameter = @Parameter, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_procedure_job_releation` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_procedure_job_releation` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ProcedureId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_procedure_job_releation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `ProcedureId`, `LinkPoint`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_procedure_job_releation`  WHERE Id IN @ids ";
    }
}
