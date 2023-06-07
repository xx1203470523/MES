/*
 *creator: Karl
 *
 *describe: job业务配置配置表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 02:55:48
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
    /// job业务配置配置表仓储
    /// </summary>
    public partial class InteJobBusinessRelationRepository : IInteJobBusinessRelationRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteJobBusinessRelationRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteJobBusinessRelationEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteJobBusinessRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobBusinessRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteJobBusinessRelationEntity>(GetByIdsSql, new { ids = ids });
        }


        /// <summary>
        /// 根据JobIds批量获取数据
        /// </summary>
        /// <param name="jobIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobBusinessRelationEntity>> GetByJobIdsAsync(IEnumerable<long> jobIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteJobBusinessRelationEntity>(GetByJobIdsSql, new { jobIds = jobIds });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteJobBusinessRelationEntity>> GetPagedInfoAsync(InteJobBusinessRelationPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            //if (query.SiteId > 0)
            //{
            sqlBuilder.Where("SiteId = @SiteId");
            //}
            //if (query.BusinessType.HasValue)
            //{
            //    sqlBuilder.Where("BusinessType=@BusinessType");
            //}
            if (query.BusinessId > 0)
            {
                sqlBuilder.Where("BusinessId=@BusinessId");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteJobBusinessRelationEntitiesTask = conn.QueryAsync<InteJobBusinessRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteJobBusinessRelationEntities = await inteJobBusinessRelationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteJobBusinessRelationEntity>(inteJobBusinessRelationEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteJobBusinessRelationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobBusinessRelationEntity>> GetInteJobBusinessRelationEntitiesAsync(InteJobBusinessRelationQuery inteJobBusinessRelationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("OrderNumber DESC");
            sqlBuilder.Select("*");

            sqlBuilder.Where("SiteId = @SiteId");

            if (inteJobBusinessRelationQuery.BusinessType.HasValue)
            {
                sqlBuilder.Where("BusinessType=@BusinessType");
            }
            if (inteJobBusinessRelationQuery.BusinessId > 0)
            {
                sqlBuilder.Where("BusinessId=@BusinessId");
            }

            var template = sqlBuilder.AddTemplate(GetInteJobBusinessRelationEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteJobBusinessRelationEntities = await conn.QueryAsync<InteJobBusinessRelationEntity>(template.RawSql, inteJobBusinessRelationQuery);
            return inteJobBusinessRelationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteJobBusinessRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteJobBusinessRelationEntity inteJobBusinessRelationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, inteJobBusinessRelationEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobBusinessRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteJobBusinessRelationEntity> inteJobBusinessRelationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, inteJobBusinessRelationEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteJobBusinessRelationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteJobBusinessRelationEntity inteJobBusinessRelationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, inteJobBusinessRelationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteJobBusinessRelationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteJobBusinessRelationEntity> inteJobBusinessRelationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, inteJobBusinessRelationEntitys);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByBusinessIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByBusinessIdSql, new { BusinessId = id });
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByBusinessIdRangeAsync(IEnumerable<long> ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByBusinessIdRangeSql, new { BusinessIds = ids });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }
    }

    public partial class InteJobBusinessRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_job_business_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_job_business_relation` /**where**/ ";
        const string GetInteJobBusinessRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_job_business_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_job_business_relation`(  `Id`, `SiteId`, `BusinessType`, `BusinessId`, `LinkPoint`, `OrderNumber`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @BusinessType, @BusinessId, @LinkPoint, @OrderNumber, @JobId, @IsUse, @Parameter, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `inte_job_business_relation` SET   BusinessType = @BusinessType, BusinessId = @BusinessId, OrderNumber = @OrderNumber, JobId = @JobId, IsUse = @IsUse, Parameter = @Parameter, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteByBusinessIdSql = "delete from `inte_job_business_relation` WHERE BusinessId = @BusinessId ";
        const string DeleteByBusinessIdRangeSql = "delete from `inte_job_business_relation` WHERE BusinessId IN @BusinessIds ";
        const string DeletesSql = "UPDATE `inte_job_business_relation` SET IsDeleted = Id WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `BusinessType`, `BusinessId`, `OrderNumber`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_job_business_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `BusinessType`, `BusinessId`, `OrderNumber`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_job_business_relation`  WHERE Id IN @ids ";
        const string GetByJobIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `BusinessType`, `BusinessId`, `OrderNumber`, `JobId`, `IsUse`, `Parameter`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `inte_job_business_relation`  WHERE JobId IN @jobIds ";
    }

}
