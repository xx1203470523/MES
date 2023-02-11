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
        /// 根据资源id和打印机Id查询数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceConfigPrintEntity>> GetByResourceIdAsync(ProcResourceConfigPrintQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcResourceConfigPrintEntity>(GetByResourceIdSql, new { ResourceId=query.ResourceId, Ids=query.Ids });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigPrints"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(List<ProcResourceConfigPrintEntity> procResourceConfigPrints)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            await conn.ExecuteScalarAsync<long>(InsertSql, procResourceConfigPrints);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Ids = idsArr });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigPrints"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcResourceConfigPrintEntity> procResourceConfigPrints)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigPrints);
        }
    }

    public partial class ProcResourceConfigPrintRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.PrintName,b.PrintIp from proc_resource_config_print a left join proc_printer b on a.PrintId=b.Id and b.IsDeleted =0   /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_config_print a left join proc_printer b on a.PrintId=b.Id and b.IsDeleted =0  /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_config_print`(  `Id`, `SiteCode`, `ResourceId`, `PrintId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ResourceId, @PrintId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_print` SET  PrintId=@PrintId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id=@Id ";
        const string DeleteSql = "UPDATE `proc_resource_config_print` SET IsDeleted = '1' WHERE Id in @Ids ";
        const string GetByResourceIdSql = "SELECT * FROM proc_resource_config_print where ResourceId=@ResourceId and PrintId  IN @Ids AND IsDeleted =0 ";
    }
}
