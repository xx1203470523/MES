/*
 *creator: Karl
 *
 *describe: 资源配置表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-10 10:21:26
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
    /// 资源配置表仓储
    /// </summary>
    public partial class ProcResourceConfigResRepository : IProcResourceConfigResRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcResourceConfigResRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigResEntity>> GetPagedInfoAsync(ProcResourceConfigResPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("a.ResourceId=@ResourceId");

            //TODO 按UpdateOn倒序排列
            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceConfigResEntitiesTask = conn.QueryAsync<ProcResourceConfigResEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceConfigResEntities = await procResourceConfigResEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceConfigResEntity>(procResourceConfigResEntities, query.PageIndex, query.PageSize, totalCount);
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
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, idsArr);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceConfigResEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceConfigResEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigResEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(ProcResourceConfigResEntity procResourceConfigResEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, procResourceConfigResEntity);
            procResourceConfigResEntity.Id = id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigResEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceConfigResEntity procResourceConfigResEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigResEntity);
        }
    }

    public partial class ProcResourceConfigResRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_resource_config_res` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_resource_config_res` /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_config_res`(  `Id`, `SiteCode`, `ResourceId`, `SetType`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ResourceId, @SetType, @Value, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_res` SET   SiteCode = @SiteCode, ResourceId = @ResourceId, SetType = @SetType, Value = @Value, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_config_res` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ResourceId`, `SetType`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_resource_config_res`  WHERE Id = @Id ";
    }
}
