/*
 *creator: Karl
 *
 *describe: 物料组维护表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-10 03:54:07
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
    /// 物料组维护表仓储
    /// </summary>
    public partial class ProcMaterialGroupRepository : IProcMaterialGroupRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcMaterialGroupRepository(IOptions<ConnectionOptions> connectionOptions)
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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialGroupEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialGroupEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialGroupEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcMaterialGroupEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procMaterialGroupPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialGroupEntity>> GetPagedInfoAsync(ProcMaterialGroupPagedQuery procMaterialGroupPagedQuery)
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
           
            var offSet = (procMaterialGroupPagedQuery.PageIndex - 1) * procMaterialGroupPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialGroupPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialGroupPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialGroupEntitiesTask = conn.QueryAsync<ProcMaterialGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procMaterialGroupEntities = await procMaterialGroupEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcMaterialGroupEntity>(procMaterialGroupEntities, procMaterialGroupPagedQuery.PageIndex, procMaterialGroupPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procMaterialGroupQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialGroupEntity>> GetProcMaterialGroupEntitiesAsync(ProcMaterialGroupQuery procMaterialGroupQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcMaterialGroupEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procMaterialGroupEntities = await conn.QueryAsync<ProcMaterialGroupEntity>(template.RawSql, procMaterialGroupQuery);
            return procMaterialGroupEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procMaterialGroupEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcMaterialGroupEntity procMaterialGroupEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procMaterialGroupEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procMaterialGroupEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcMaterialGroupEntity> procMaterialGroupEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procMaterialGroupEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procMaterialGroupEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcMaterialGroupEntity procMaterialGroupEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procMaterialGroupEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procMaterialGroupEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcMaterialGroupEntity> procMaterialGroupEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procMaterialGroupEntitys);
        }

    }

    public partial class ProcMaterialGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_material_group` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_material_group` /**where**/ ";
        const string GetProcMaterialGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_material_group` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_material_group`(  `Id`, `SiteCode`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @GroupCode, @GroupName, @GroupVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_material_group`(  `Id`, `SiteCode`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @GroupCode, @GroupName, @GroupVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_material_group` SET   SiteCode = @SiteCode, GroupCode = @GroupCode, GroupName = @GroupName, GroupVersion = @GroupVersion, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_material_group` SET   SiteCode = @SiteCode, GroupCode = @GroupCode, GroupName = @GroupName, GroupVersion = @GroupVersion, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_material_group` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_material_group` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_material_group`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_material_group`  WHERE Id IN @ids ";
    }
}
