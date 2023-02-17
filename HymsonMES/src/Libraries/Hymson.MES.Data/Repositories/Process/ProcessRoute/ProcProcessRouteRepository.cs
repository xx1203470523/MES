/*
 *creator: Karl
 *
 *describe: 工艺路线表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Resource;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工艺路线表仓储
    /// </summary>
    public partial class ProcProcessRouteRepository : IProcProcessRouteRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcProcessRouteRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 删除时查询启用和保留状态的不能删除
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> IsIsExistsEnabledAsync(ProcProcessRouteQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteEntity>(IsIsExistsEnabledSql, new { StatusArr= query.StatusArr, Ids =query.Ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteEntity>> GetPagedInfoAsync(ProcProcessRoutePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(query.SiteCode))
            {
                sqlBuilder.Where("SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = $"%{query.Name}%";
                sqlBuilder.Where("Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(query.Version))
            {
                query.Version = $"%{query.Version}%";
                sqlBuilder.Where("Version like @Version");
            }
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcessRouteEntitiesTask = conn.QueryAsync<ProcProcessRouteEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcessRouteEntities = await procProcessRouteEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcessRouteEntity>(procProcessRouteEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcessRouteEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcessRouteQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteEntity>> GetProcProcessRouteEntitiesAsync(ProcProcessRouteQuery procProcessRouteQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcessRouteEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcessRouteEntities = await conn.QueryAsync<ProcProcessRouteEntity>(template.RawSql, procProcessRouteQuery);
            return procProcessRouteEntities;
        }

        /// <summary>
        /// 判断工艺路线是否存在
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(ProcProcessRouteQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var  procProcessRoutes= await conn.QueryAsync<ProcProcessRouteEntity>(ExistsSql, new { Code = query.Code, SiteCode = query.SiteCode }) ;
            return procProcessRoutes != null && procProcessRoutes.Any();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcessRouteEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcessRouteEntity procProcessRouteEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcessRouteEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcProcessRouteEntity> procProcessRouteEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procProcessRouteEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcessRouteEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcessRouteEntity procProcessRouteEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteEntity);
        }
		
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcessRouteEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcProcessRouteEntity> procProcessRouteEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteEntitys);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { UpdatedBy = command.UserId, UpdatedOn = command.DeleteOn, Ids = command.Ids });
        }
    }

    public partial class ProcProcessRouteRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_process_route` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_process_route` /**where**/ ";
        const string GetProcProcessRouteEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_process_route` /**where**/  ";
        const string ExistsSql = "SELECT Id FROM proc_process_route WHERE `IsDeleted`= 0 AND Code=@Code and SiteCode=@SiteCode LIMIT 1";

        const string InsertSql = "INSERT INTO `proc_process_route`(  `Id`, `SiteCode`, `Code`, `Name`, `Status`, `Type`, `Version`, `IsCurrentVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @Code, @Name, @Status, @Type, @Version, @IsCurrentVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_process_route`(  `Id`, `SiteCode`, `Code`, `Name`, `Status`, `Type`, `Version`, `IsCurrentVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @Code, @Name, @Status, @Type, @Version, @IsCurrentVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_process_route` SET Status = @Status, Type = @Type, IsCurrentVersion = @IsCurrentVersion, Remark = @Remark,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_process_route` SET IsDeleted = '1',UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id in @Ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `Code`, `Name`, `Status`, `Type`, `Version`, `IsCurrentVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `Code`, `Name`, `Status`, `Type`, `Version`, `IsCurrentVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route`  WHERE Id IN @ids ";
        const string IsIsExistsEnabledSql = "select Id  from proc_process_route where IsDeleted=0 and Status in @StatusArr and Id  in @Ids  limit 1";
    }
}
