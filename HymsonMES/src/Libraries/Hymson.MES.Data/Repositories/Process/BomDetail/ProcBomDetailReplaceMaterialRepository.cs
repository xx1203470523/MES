/*
 *creator: Karl
 *
 *describe: BOM明细替代料表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 05:33:28
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
    /// BOM明细替代料表仓储
    /// </summary>
    public partial class ProcBomDetailReplaceMaterialRepository : IProcBomDetailReplaceMaterialRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcBomDetailReplaceMaterialRepository(IOptions<ConnectionOptions> connectionOptions)
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
        public async Task<int> DeletesAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids=ids });

        }

        /// <summary>
        /// 批量删除关联的BomId的数据
        /// </summary>
        /// <param name="bomIds"></param>
        /// <returns></returns>
        public async Task<int> DeleteBomIDAsync(long[] bomIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteBomIDsSql, new { bomIds = bomIds });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDetailReplaceMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcBomDetailReplaceMaterialEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDetailReplaceMaterialEntity>> GetPagedInfoAsync(ProcBomDetailReplaceMaterialPagedQuery procBomDetailReplaceMaterialPagedQuery)
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
           
            var offSet = (procBomDetailReplaceMaterialPagedQuery.PageIndex - 1) * procBomDetailReplaceMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBomDetailReplaceMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBomDetailReplaceMaterialPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procBomDetailReplaceMaterialEntitiesTask = conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBomDetailReplaceMaterialEntities = await procBomDetailReplaceMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBomDetailReplaceMaterialEntity>(procBomDetailReplaceMaterialEntities, procBomDetailReplaceMaterialPagedQuery.PageIndex, procBomDetailReplaceMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBomDetailReplaceMaterialEntity>> GetProcBomDetailReplaceMaterialEntitiesAsync(ProcBomDetailReplaceMaterialQuery procBomDetailReplaceMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBomDetailReplaceMaterialEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procBomDetailReplaceMaterialEntities = await conn.QueryAsync<ProcBomDetailReplaceMaterialEntity>(template.RawSql, procBomDetailReplaceMaterialQuery);
            return procBomDetailReplaceMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBomDetailReplaceMaterialEntity procBomDetailReplaceMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procBomDetailReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBomDetailReplaceMaterialEntity> procBomDetailReplaceMaterialEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procBomDetailReplaceMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBomDetailReplaceMaterialEntity procBomDetailReplaceMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procBomDetailReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBomDetailReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBomDetailReplaceMaterialEntity> procBomDetailReplaceMaterialEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procBomDetailReplaceMaterialEntitys);
        }

    }

    public partial class ProcBomDetailReplaceMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bom_detail_replace_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_bom_detail_replace_material` /**where**/ ";
        const string GetProcBomDetailReplaceMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_bom_detail_replace_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bom_detail_replace_material`(  `Id`, `SiteCode`, `BomId`, `BomDetailId`, `ReplaceMaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @BomId, @BomDetailId, @ReplaceMaterialId, @ReferencePoint, @Usages, @Loss, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_bom_detail_replace_material`(  `Id`, `SiteCode`, `BomId`, `BomDetailId`, `ReplaceMaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @BomId, @BomDetailId, @ReplaceMaterialId, @ReferencePoint, @Usages, @Loss, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_bom_detail_replace_material` SET   SiteCode = @SiteCode, BomId = @BomId, BomDetailId = @BomDetailId, ReplaceMaterialId = @ReplaceMaterialId, ReferencePoint = @ReferencePoint, Usages = @Usages, Loss = @Loss, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_bom_detail_replace_material` SET   SiteCode = @SiteCode, BomId = @BomId, BomDetailId = @BomDetailId, ReplaceMaterialId = @ReplaceMaterialId, ReferencePoint = @ReferencePoint, Usages = @Usages, Loss = @Loss, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_bom_detail_replace_material` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bom_detail_replace_material` SET IsDeleted = '1' WHERE Id in @ids";
        /// <summary>
        /// 批量删除关联的BomId的数据
        /// </summary>
        const string DeleteBomIDsSql = "UPDATE `proc_bom_detail_replace_material` SET IsDeleted = '1' WHERE BomId in @bomIds";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `BomId`, `BomDetailId`, `ReplaceMaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_bom_detail_replace_material`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `BomId`, `BomDetailId`, `ReplaceMaterialId`, `ReferencePoint`, `Usages`, `Loss`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_bom_detail_replace_material`  WHERE Id IN @ids ";
    }
}