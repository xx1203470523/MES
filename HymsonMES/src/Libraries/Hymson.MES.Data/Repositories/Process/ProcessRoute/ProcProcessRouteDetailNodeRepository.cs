/*
 *creator: Karl
 *
 *describe: 工艺路线工序节点明细表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:17:40
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
    /// 工艺路线工序节点明细表仓储
    /// </summary>
    public partial class ProcProcessRouteDetailNodeRepository : IProcProcessRouteDetailNodeRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcProcessRouteDetailNodeRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeView>> GetListAsync(ProcProcessRouteDetailNodeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetListSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            if (query.ProcessRouteId > 0)
            {
                sqlBuilder.Where("ProcessRouteId=@ProcessRouteId");
            }
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcessRouteDetailNodeEntities = await conn.QueryAsync<ProcProcessRouteDetailNodeView>(template.RawSql, template.Parameters);
            return procProcessRouteDetailNodeEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteDetailNodeEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailNodeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcessRouteDetailNodeEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcessRouteDetailNodeEntitys);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcessRouteDetailNodeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcProcessRouteDetailNodeEntity> procProcessRouteDetailNodeEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteDetailNodeEntitys);
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

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByProcessRouteIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByProcessRouteIdSql, new { ProcedureId = id });
        }
    }

    public partial class ProcProcessRouteDetailNodeRepository
    {
        const string GetListSqlTemplate = @"select a.*,b.Code,b.Name from proc_process_route_detail_node a left join proc_procedure b on a.ProcedureId=b.Id  /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_process_route_detail_node`(  `Id`, `SiteCode`, `ProcessRouteId`, `SerialNo`, `ProcedureId`, `CheckType`, `CheckRate`, `IsWorkReport`, `PackingLevel`, `IsFirstProcess`, `Status`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ProcessRouteId, @SerialNo, @ProcedureId, @CheckType, @CheckRate, @IsWorkReport, @PackingLevel, @IsFirstProcess, @Status, @Extra1, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_process_route_detail_node` SET   SiteCode = @SiteCode, ProcessRouteId = @ProcessRouteId, SerialNo = @SerialNo, ProcedureId = @ProcedureId, CheckType = @CheckType, CheckRate = @CheckRate, IsWorkReport = @IsWorkReport, PackingLevel = @PackingLevel, IsFirstProcess = @IsFirstProcess, Status = @Status, Extra1 = @Extra1, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_process_route_detail_node` SET   SiteCode = @SiteCode, ProcessRouteId = @ProcessRouteId, SerialNo = @SerialNo, ProcedureId = @ProcedureId, CheckType = @CheckType, CheckRate = @CheckRate, IsWorkReport = @IsWorkReport, PackingLevel = @PackingLevel, IsFirstProcess = @IsFirstProcess, Status = @Status, Extra1 = @Extra1, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_process_route_detail_node` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_process_route_detail_node` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ProcessRouteId`, `SerialNo`, `ProcedureBomId`, `CheckType`, `CheckRate`, `IsWorkReport`, `PackingLevel`, `IsFirstProcess`, `Status`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route_detail_node`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `ProcessRouteId`, `SerialNo`, `ProcedureBomId`, `CheckType`, `CheckRate`, `IsWorkReport`, `PackingLevel`, `IsFirstProcess`, `Status`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route_detail_node`  WHERE Id IN @ids ";
        const string DeleteByProcessRouteIdSql = "delete from `proc_process_route_detail_node` WHERE ProcessRouteId = @ProcessRouteId ";
    }
}
