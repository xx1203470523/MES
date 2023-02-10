/*
 *creator: Karl
 *
 *describe: 资源配置打印机 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-09 04:14:52
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process.Resource;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置打印机仓储
    /// </summary>
    public partial class ProcResourceConfigPrintRepository : IProcResourceConfigPrintRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcResourceConfigPrintRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigPrintView>> GetPagedInfoAsync(ProcResourceConfigPrintPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("a.ResourceId=@ResourceId");
            //TODO 按更新时间倒序排列

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceConfigPrintEntitiesTask = conn.QueryAsync<ProcResourceConfigPrintView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceConfigPrintEntities = await procResourceConfigPrintEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceConfigPrintView>(procResourceConfigPrintEntities, query.PageIndex, query.PageSize, totalCount);
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
        public async Task<ProcResourceConfigPrintEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceConfigPrintEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigPrintEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(ProcResourceConfigPrintEntity procResourceConfigPrintEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, procResourceConfigPrintEntity);
            procResourceConfigPrintEntity.Id = id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigPrintEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceConfigPrintEntity procResourceConfigPrintEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigPrintEntity);
        }
    }

    public partial class ProcResourceConfigPrintRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.PrintName,b.PrintIp from proc_resource_config_print a left join proc_printer b on a.PrintId=b.Id and b.IsDeleted =0   /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_config_print a left join proc_printer b on a.PrintId=b.Id and b.IsDeleted =0  /**where**/";
        const string GetProcResourceConfigPrintEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_resource_config_print` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_resource_config_print`(  `Id`, `SiteCode`, `ResourceId`, `PrintId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ResourceId, @PrintId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_print` SET   SiteCode = @SiteCode, ResourceId = @ResourceId, PrintId = @PrintId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_config_print` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ResourceId`, `PrintId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_resource_config_print`  WHERE Id = @Id ";
    }
}
