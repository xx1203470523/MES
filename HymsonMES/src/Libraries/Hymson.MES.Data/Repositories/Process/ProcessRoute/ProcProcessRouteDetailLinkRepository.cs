/*
 *creator: Karl
 *
 *describe: 工艺路线工序节点关系明细表(前节点多条就存多条) 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:17:52
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
    /// 工艺路线工序节点关系明细表(前节点多条就存多条)仓储
    /// </summary>
    public partial class ProcProcessRouteDetailLinkRepository : IProcProcessRouteDetailLinkRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcProcessRouteDetailLinkRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcessRouteDetailLinkEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcessRouteDetailLinkEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procProcessRouteDetailLinkPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteDetailLinkEntity>> GetPagedInfoAsync(ProcProcessRouteDetailLinkPagedQuery procProcessRouteDetailLinkPagedQuery)
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
           
            var offSet = (procProcessRouteDetailLinkPagedQuery.PageIndex - 1) * procProcessRouteDetailLinkPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procProcessRouteDetailLinkPagedQuery.PageSize });
            sqlBuilder.AddParameters(procProcessRouteDetailLinkPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcessRouteDetailLinkEntitiesTask = conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcessRouteDetailLinkEntities = await procProcessRouteDetailLinkEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcessRouteDetailLinkEntity>(procProcessRouteDetailLinkEntities, procProcessRouteDetailLinkPagedQuery.PageIndex, procProcessRouteDetailLinkPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcessRouteDetailLinkQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailLinkEntity>> GetListAsync(ProcProcessRouteDetailLinkQuery procProcessRouteDetailLinkQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetListSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("ProcessRouteId=@ProcessRouteId");

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcessRouteDetailLinkEntities = await conn.QueryAsync<ProcProcessRouteDetailLinkEntity>(template.RawSql, template.Parameters);
            return procProcessRouteDetailLinkEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcessRouteDetailLinkEntity procProcessRouteDetailLinkEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcessRouteDetailLinkEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procProcessRouteDetailLinkEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcessRouteDetailLinkEntity procProcessRouteDetailLinkEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteDetailLinkEntity);
        }
		
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcessRouteDetailLinkEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcProcessRouteDetailLinkEntity> procProcessRouteDetailLinkEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcessRouteDetailLinkEntitys);
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

    public partial class ProcProcessRouteDetailLinkRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_process_route_detail_link` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_process_route_detail_link` /**where**/ ";
        const string GetListSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_process_route_detail_link` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_process_route_detail_link`(  `Id`, `SiteCode`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @SerialNo, @ProcessRouteId, @PreProcessRouteDetailId, @ProcessRouteDetailId, @Extra1, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_process_route_detail_link`(  `Id`, `SiteCode`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @SerialNo, @ProcessRouteId, @PreProcessRouteDetailId, @ProcessRouteDetailId, @Extra1, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_process_route_detail_link` SET   SiteCode = @SiteCode, SerialNo = @SerialNo, ProcessRouteId = @ProcessRouteId, PreProcessRouteDetailId = @PreProcessRouteDetailId, ProcessRouteDetailId = @ProcessRouteDetailId, Extra1 = @Extra1, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_process_route_detail_link` SET   SiteCode = @SiteCode, SerialNo = @SerialNo, ProcessRouteId = @ProcessRouteId, PreProcessRouteDetailId = @PreProcessRouteDetailId, ProcessRouteDetailId = @ProcessRouteDetailId, Extra1 = @Extra1, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_process_route_detail_link` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_process_route_detail_link` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route_detail_link`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `SerialNo`, `ProcessRouteId`, `PreProcessRouteDetailId`, `ProcessRouteDetailId`, `Extra1`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_process_route_detail_link`  WHERE Id IN @ids ";
        const string DeleteByProcessRouteIdSql = "UPDATE `proc_process_route_detail_link` SET IsDeleted = '1' WHERE ProcessRouteId = @ProcessRouteId ";
    }
}
