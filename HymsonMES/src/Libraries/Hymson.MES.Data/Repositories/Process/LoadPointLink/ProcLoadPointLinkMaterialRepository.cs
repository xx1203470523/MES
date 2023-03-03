/*
 *creator: Karl
 *
 *describe: 上料点关联物料表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-18 09:31:10
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
    /// 上料点关联物料表仓储
    /// </summary>
    public partial class ProcLoadPointLinkMaterialRepository : IProcLoadPointLinkMaterialRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcLoadPointLinkMaterialRepository(IOptions<ConnectionOptions> connectionOptions)
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
        /// <param name="ids"></param>
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
        public async Task<ProcLoadPointLinkMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcLoadPointLinkMaterialEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkMaterialEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcLoadPointLinkMaterialEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkMaterialView>> GetLoadPointLinkMaterialAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcLoadPointLinkMaterialView>(GetLoadPointLinkMaterialByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointLinkMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointLinkMaterialEntity>> GetPagedInfoAsync(ProcLoadPointLinkMaterialPagedQuery procLoadPointLinkMaterialPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteId))
            //{
            //    sqlBuilder.Where("SiteId=@SiteId");
            //}
           
            var offSet = (procLoadPointLinkMaterialPagedQuery.PageIndex - 1) * procLoadPointLinkMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLoadPointLinkMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procLoadPointLinkMaterialPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procLoadPointLinkMaterialEntitiesTask = conn.QueryAsync<ProcLoadPointLinkMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLoadPointLinkMaterialEntities = await procLoadPointLinkMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLoadPointLinkMaterialEntity>(procLoadPointLinkMaterialEntities, procLoadPointLinkMaterialPagedQuery.PageIndex, procLoadPointLinkMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLoadPointLinkMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointLinkMaterialEntity>> GetProcLoadPointLinkMaterialEntitiesAsync(ProcLoadPointLinkMaterialQuery procLoadPointLinkMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLoadPointLinkMaterialEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procLoadPointLinkMaterialEntities = await conn.QueryAsync<ProcLoadPointLinkMaterialEntity>(template.RawSql, procLoadPointLinkMaterialQuery);
            return procLoadPointLinkMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLoadPointLinkMaterialEntity procLoadPointLinkMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procLoadPointLinkMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLoadPointLinkMaterialEntity> procLoadPointLinkMaterialEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procLoadPointLinkMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLoadPointLinkMaterialEntity procLoadPointLinkMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procLoadPointLinkMaterialEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointLinkMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLoadPointLinkMaterialEntity> procLoadPointLinkMaterialEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procLoadPointLinkMaterialEntitys);
        }

    }

    public partial class ProcLoadPointLinkMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_load_point_link_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_load_point_link_material` /**where**/ ";
        const string GetProcLoadPointLinkMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_load_point_link_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_load_point_link_material`(  `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `MaterialId`, `Version`, `ReferencePoint`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNo, @LoadPointId, @MaterialId, @Version, @ReferencePoint, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_load_point_link_material`(  `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `MaterialId`, `Version`, `ReferencePoint`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SerialNo, @LoadPointId, @MaterialId, @Version, @ReferencePoint, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_load_point_link_material` SET   SiteId = @SiteId, SerialNo = @SerialNo, LoadPointId = @LoadPointId, MaterialId = @MaterialId, Version = @Version, ReferencePoint = @ReferencePoint, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_load_point_link_material` SET   SiteId = @SiteId, SerialNo = @SerialNo, LoadPointId = @LoadPointId, MaterialId = @MaterialId, Version = @Version, ReferencePoint = @ReferencePoint, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_load_point_link_material` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_load_point_link_material` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string DeletesByLoadPointIdTrueSql = "DELETE  FROM `proc_load_point_link_material` WHERE LoadPointId in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `MaterialId`, `Version`, `ReferencePoint`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point_link_material`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `SerialNo`, `LoadPointId`, `MaterialId`, `Version`, `ReferencePoint`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point_link_material`  WHERE Id IN @ids ";
        const string GetLoadPointLinkMaterialByIdsSql = @"SELECT 
                                         a.Id, a.MaterialId, a.ReferencePoint, b.MaterialCode, b.MaterialName, b.Version
                            FROM `proc_load_point_link_material` a
                            INNER JOIN proc_material b on a.MaterialId = b.Id
                            WHERE a.IsDeleted =0 
                            AND a.LoadPointId IN @ids  
                            order by a.CreatedOn Desc ";

    }
}
