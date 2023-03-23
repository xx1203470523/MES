/*
 *creator: Karl
 *
 *describe: 编码规则 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 编码规则仓储
    /// </summary>
    public partial class InteCodeRulesRepository : IInteCodeRulesRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteCodeRulesRepository(IOptions<ConnectionOptions> connectionOptions)
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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCodeRulesEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteCodeRulesEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteCodeRulesEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCodeRulesPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCodeRulesEntity>> GetPagedInfoAsync(InteCodeRulesPagedQuery inteCodeRulesPagedQuery)
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
           
            var offSet = (inteCodeRulesPagedQuery.PageIndex - 1) * inteCodeRulesPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteCodeRulesPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteCodeRulesPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteCodeRulesEntitiesTask = conn.QueryAsync<InteCodeRulesEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteCodeRulesEntities = await inteCodeRulesEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCodeRulesEntity>(inteCodeRulesEntities, inteCodeRulesPagedQuery.PageIndex, inteCodeRulesPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="inteCodeRulesQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCodeRulesEntity>> GetInteCodeRulesEntitiesAsync(InteCodeRulesQuery inteCodeRulesQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCodeRulesEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteCodeRulesEntities = await conn.QueryAsync<InteCodeRulesEntity>(template.RawSql, inteCodeRulesQuery);
            return inteCodeRulesEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteCodeRulesEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCodeRulesEntity inteCodeRulesEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, inteCodeRulesEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteCodeRulesEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<InteCodeRulesEntity> inteCodeRulesEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, inteCodeRulesEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteCodeRulesEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCodeRulesEntity inteCodeRulesEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, inteCodeRulesEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="inteCodeRulesEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<InteCodeRulesEntity> inteCodeRulesEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, inteCodeRulesEntitys);
        }

    }

    public partial class InteCodeRulesRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_code_rules` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_code_rules` /**where**/ ";
        const string GetInteCodeRulesEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_code_rules` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_code_rules`(  `Id`, `ProductId`, `CodeType`, `PackType`, `Base`, `IgnoreChar`, `Increment`, `OrderLength`, `ResetType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (   @Id, @ProductId, @CodeType, @PackType, @Base, @IgnoreChar, @Increment, @OrderLength, @ResetType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `inte_code_rules`(  `Id`, `ProductId`, `CodeType`, `PackType`, `Base`, `IgnoreChar`, `Increment`, `OrderLength`, `ResetType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (   @Id, @ProductId, @CodeType, @PackType, @Base, @IgnoreChar, @Increment, @OrderLength, @ResetType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `inte_code_rules` SET   ProductId = @ProductId, CodeType = @CodeType, PackType = @PackType, Base = @Base, IgnoreChar = @IgnoreChar, Increment = @Increment, OrderLength = @OrderLength, ResetType = @ResetType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `inte_code_rules` SET   ProductId = @ProductId, CodeType = @CodeType, PackType = @PackType, Base = @Base, IgnoreChar = @IgnoreChar, Increment = @Increment, OrderLength = @OrderLength, ResetType = @ResetType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_code_rules` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_code_rules` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `ProductId`, `CodeType`, `PackType`, `Base`, `IgnoreChar`, `Increment`, `OrderLength`, `ResetType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`
                            FROM `inte_code_rules`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `ProductId`, `CodeType`, `PackType`, `Base`, `IgnoreChar`, `Increment`, `OrderLength`, `ResetType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`
                            FROM `inte_code_rules`  WHERE Id IN @ids ";
    }
}
