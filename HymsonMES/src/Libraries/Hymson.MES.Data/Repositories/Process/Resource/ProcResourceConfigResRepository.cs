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

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源配置表仓储
    /// </summary>
    public partial class ProcResourceConfigResRepository : BaseRepository, IProcResourceConfigResRepository
    {
        public ProcResourceConfigResRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
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
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("ResourceId = @ResourceId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
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
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesRangeAsync(long[] idsArr)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, idsArr);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procResourceConfigRess"></param>
        /// <returns></returns>
        public async Task InsertRangeAsync(IEnumerable<ProcResourceConfigResEntity> procResourceConfigRess)
        {
            using var conn = GetMESDbConnection();
            await conn.ExecuteAsync(InsertSql, procResourceConfigRess);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procResourceConfigRes"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcResourceConfigResEntity> procResourceConfigRes)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procResourceConfigRes);
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
    }

    public partial class ProcResourceConfigResRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_resource_config_res` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_resource_config_res` /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource_config_res`(  `Id`, `SiteId`, `ResourceId`, `SetType`, `Value`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @ResourceId, @SetType, @Value, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_resource_config_res` SET  SetType = @SetType, Value = @Value,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_resource_config_res` SET IsDeleted =Id WHERE Id = @Id ";
        const string DeleteByResourceIdSql = "delete from `proc_resource_config_res` WHERE ResourceId = @ResourceId ";
    }
}
