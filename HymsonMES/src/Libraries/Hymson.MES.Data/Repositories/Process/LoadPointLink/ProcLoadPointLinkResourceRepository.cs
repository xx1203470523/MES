/*
 *creator: Karl
 *
 *describe: 上料点关联资源表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:36:09
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 上料点关联资源表仓储
    /// </summary>
    public partial class ProcLoadPointLinkResourceRepository : IProcLoadPointLinkResourceRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcLoadPointLinkResourceRepository(IOptions<ConnectionOptions> connectionOptions)
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
        /// 根据LoadPointId批量真删除 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesByLoadPointIdTrueAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesByLoadPointIdTrueSql, new { ids = ids });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointLinkResourceEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcLoadPointLinkResourceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcLoadPointLinkResourceEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceView>> GetLoadPointLinkResourceAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcLoadPointLinkResourceView>(GetLoadPointLinkResourceByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointLinkResourcePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointLinkResourceEntity>> GetPagedInfoAsync(ProcLoadPointLinkResourcePagedQuery procLoadPointLinkResourcePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteId))
            //{
            //    sqlBuilder.Where("SiteId=@SiteId");
            //}

            var offSet = (procLoadPointLinkResourcePagedQuery.PageIndex - 1) * procLoadPointLinkResourcePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLoadPointLinkResourcePagedQuery.PageSize });
            sqlBuilder.AddParameters(procLoadPointLinkResourcePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procLoadPointLinkResourceEntitiesTask = conn.QueryAsync<ProcLoadPointLinkResourceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLoadPointLinkResourceEntities = await procLoadPointLinkResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLoadPointLinkResourceEntity>(procLoadPointLinkResourceEntities, procLoadPointLinkResourcePagedQuery.PageIndex, procLoadPointLinkResourcePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLoadPointLinkResourceQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkResourceEntity>> GetProcLoadPointLinkResourceEntitiesAsync(ProcLoadPointLinkResourceQuery procLoadPointLinkResourceQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLoadPointLinkResourceEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procLoadPointLinkResourceEntities = await conn.QueryAsync<ProcLoadPointLinkResourceEntity>(template.RawSql, procLoadPointLinkResourceQuery);
            return procLoadPointLinkResourceEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLoadPointLinkResourceEntity procLoadPointLinkResourceEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procLoadPointLinkResourceEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLoadPointLinkResourceEntity> procLoadPointLinkResourceEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procLoadPointLinkResourceEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLoadPointLinkResourceEntity procLoadPointLinkResourceEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procLoadPointLinkResourceEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointLinkResourceEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLoadPointLinkResourceEntity> procLoadPointLinkResourceEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procLoadPointLinkResourceEntitys);
        }

    }

    public partial class ProcLoadPointLinkResourceRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_load_point_link_resource` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_load_point_link_resource` /**where**/ ";
        const string GetProcLoadPointLinkResourceEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_load_point_link_resource` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_load_point_link_resource`(  `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNo, @LoadPointId, @ResourceId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_load_point_link_resource`(  `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNo, @LoadPointId, @ResourceId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_load_point_link_resource` SET   SiteId = @SiteId, SerialNo = @SerialNo, LoadPointId = @LoadPointId, ResourceId = @ResourceId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_load_point_link_resource` SET   SiteId = @SiteId, SerialNo = @SerialNo, LoadPointId = @LoadPointId, ResourceId = @ResourceId, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_load_point_link_resource` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_load_point_link_resource` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string DeletesByLoadPointIdTrueSql = "DELETE  FROM `proc_load_point_link_resource` WHERE LoadPointId in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point_link_resource`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `ResourceId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point_link_resource`  WHERE Id IN @ids ";
        const string GetLoadPointLinkResourceByIdsSql = @"SELECT 
                                           a.Id,  a.ResourceId, b.ResCode, b.ResName 
                            FROM `proc_load_point_link_resource` a
                            Inner JOIN proc_resource b on a.ResourceId = b.Id
                            WHERE a.LoadPointId IN @ids 
                            Order by a.CreatedOn ";
    }
}
