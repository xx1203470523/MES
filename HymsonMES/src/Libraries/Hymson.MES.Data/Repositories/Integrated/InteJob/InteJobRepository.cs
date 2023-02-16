/*
 *creator: Karl
 *
 *describe: 作业表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 04:32:34
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Integrated;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 作业表仓储
    /// </summary>
    public partial class InteJobRepository : IInteJobRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteJobRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteJobEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteJobEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteJobEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteJobPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteJobEntity>> GetPagedInfoAsync(InteJobPagedQuery inteJobPagedQuery)
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
           
            var offSet = (inteJobPagedQuery.PageIndex - 1) * inteJobPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteJobPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteJobPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteJobEntitiesTask = conn.QueryAsync<InteJobEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteJobEntities = await inteJobEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteJobEntity>(inteJobEntities, inteJobPagedQuery.PageIndex, inteJobPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteJobQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobEntity>> GetInteJobEntitiesAsync(InteJobQuery inteJobQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteJobEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteJobEntities = await conn.QueryAsync<InteJobEntity>(template.RawSql, inteJobQuery);
            return inteJobEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteJobEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteJobEntity inteJobEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, inteJobEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<InteJobEntity> inteJobEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, inteJobEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteJobEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteJobEntity inteJobEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, inteJobEntity);
        }
		
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteJobEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<InteJobEntity> inteJobEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, inteJobEntitys);
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

    public partial class InteJobRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_job` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_job` /**where**/ ";
        const string GetInteJobEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_job` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_job`(  `Id`, `SiteCode`, `JobCode`, `JobName`, `ClassProgram`, `ParameterDescribe`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @JobCode, @JobName, @ClassProgram, @ParameterDescribe, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_job`(  `Id`, `SiteCode`, `JobCode`, `JobName`, `ClassProgram`, `ParameterDescribe`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @JobCode, @JobName, @ClassProgram, @ParameterDescribe, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `inte_job` SET   SiteCode = @SiteCode, JobCode = @JobCode, JobName = @JobName, ClassProgram = @ClassProgram, ParameterDescribe = @ParameterDescribe, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_job` SET   SiteCode = @SiteCode, JobCode = @JobCode, JobName = @JobName, ClassProgram = @ClassProgram, ParameterDescribe = @ParameterDescribe, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_job` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_job` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `JobCode`, `JobName`, `ClassProgram`, `ParameterDescribe`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_job`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `JobCode`, `JobName`, `ClassProgram`, `ParameterDescribe`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_job`  WHERE Id IN @ids ";
    }
}
