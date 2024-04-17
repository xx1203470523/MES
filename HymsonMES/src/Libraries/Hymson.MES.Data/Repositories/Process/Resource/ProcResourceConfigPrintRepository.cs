/*
 *creator: Karl
 *
 *describe: 资源配置打印机 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-09 04:14:52
 */

using Dapper;
using Hymson.DbConnection.Abstractions;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process.Resource;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using ConnectionOptions = Hymson.MES.Data.Options.ConnectionOptions;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置打印机仓储
    /// </summary>
    public partial class ProcResourceConfigPrintRepository : BaseRepository, IProcResourceConfigPrintRepository
    {
        public ProcResourceConfigPrintRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
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
            sqlBuilder.Where("a.IsDeleted=0");
            sqlBuilder.Where("a.ResourceId=@ResourceId");
            sqlBuilder.OrderBy("a.UpdatedOn DESC");

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var procResourceConfigPrintEntitiesTask = conn.QueryAsync<ProcResourceConfigPrintView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceConfigPrintEntities = await procResourceConfigPrintEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceConfigPrintView>(procResourceConfigPrintEntities, query.PageIndex, query.PageSize, totalCount);
        }


        /// <summary>
        /// 根据资源id查询数据
        /// </summary> 
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceConfigPrintEntity>> GetByResourceIdAsync(long resourceId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceConfigPrintEntity>(GetByResourceIdSql, new { ResourceId = resourceId });
        }

        public async Task<IEnumerable<ProcResourceConfigPrintEntity>> GetByPrintIdAsync(ProcResourceConfigPrintQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceConfigPrintEntity>(GetByPrintIdSql, new { Ids = query.Ids });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigPrints"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<ProcResourceConfigPrintEntity> procResourceConfigPrints)
        {
            using var conn = GetMESDbConnection();
            await conn.ExecuteAsync(InsertSql, procResourceConfigPrints);
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(long[] idsArr)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Ids = idsArr });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByResourceIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByResourceIdSql, new { ResourceId = id });
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigPrints"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcResourceConfigPrintEntity> procResourceConfigPrints)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigPrints);
        }
    }

    public partial class ProcResourceConfigPrintRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"select a.*,b.PrintName,b.PrintIp from proc_resource_config_print a left join proc_printer b on a.PrintId=b.Id and b.IsDeleted =0   /**where**/ ORDER BY a.UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "select count(*) from proc_resource_config_print a left join proc_printer b on a.PrintId=b.Id and b.IsDeleted =0  /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_config_print`(  `Id`, `SiteId`, `ResourceId`, `PrintId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ResourceId, @PrintId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_print` SET  PrintId=@PrintId,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id=@Id ";
        const string DeleteSql = "UPDATE `proc_resource_config_print` SET IsDeleted = '1' WHERE Id in @Ids ";
        const string DeleteByResourceIdSql = "delete from `proc_resource_config_print` WHERE ResourceId = @ResourceId ";
        const string GetByResourceIdSql = "SELECT * FROM proc_resource_config_print where ResourceId=@ResourceId AND IsDeleted =0 ";
        const string GetByPrintIdSql = "SELECT * FROM proc_resource_config_print where  PrintId  IN @Ids AND IsDeleted =0 ";
    }
}
