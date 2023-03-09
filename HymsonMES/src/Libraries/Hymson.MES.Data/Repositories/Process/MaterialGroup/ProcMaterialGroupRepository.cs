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
using Hymson.MES.Data.Repositories.Common.Command;
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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialGroupEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID和站点获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<ProcMaterialGroupEntity> GetByIdAndSiteIdAsync(long id, long SiteId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcMaterialGroupEntity>(GetByIdAndSiteIdSql, new { Id = id, SiteId = SiteId });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcMaterialGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcMaterialGroupEntity>(GetByIdsSql, new { ids = ids });
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
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialGroupPagedQuery.GroupCode))
            {
                procMaterialGroupPagedQuery.GroupCode = $"%{procMaterialGroupPagedQuery.GroupCode}%";
                sqlBuilder.Where(" GroupCode like @GroupCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialGroupPagedQuery.GroupName))
            {
                procMaterialGroupPagedQuery.GroupName = $"%{procMaterialGroupPagedQuery.GroupName}%";
                sqlBuilder.Where(" GroupName like @GroupName ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialGroupPagedQuery.Remark))
            {
                procMaterialGroupPagedQuery.GroupName = $"%{procMaterialGroupPagedQuery.Remark}%";
                sqlBuilder.Where(" Remark like @Remark ");
            }

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
        /// 分页查询 自定义
        /// </summary>
        /// <param name="procMaterialGroupCustomPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<CustomProcMaterialGroupView>> GetPagedCustomInfoAsync(ProcMaterialGroupCustomPagedQuery procMaterialGroupCustomPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedCustomInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedCustomInfoCountSqlTemplate);
            sqlBuilder.Where("g.IsDeleted=0");
            // sqlBuilder.Select("*");
            sqlBuilder.Where("g.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(procMaterialGroupCustomPagedQuery.GroupCode))
            {
                procMaterialGroupCustomPagedQuery.GroupCode = $"%{procMaterialGroupCustomPagedQuery.GroupCode}%";
                sqlBuilder.Where(" g.GroupCode like @GroupCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialGroupCustomPagedQuery.MaterialCode))
            {
                procMaterialGroupCustomPagedQuery.MaterialCode = $"%{procMaterialGroupCustomPagedQuery.MaterialCode}%";
                sqlBuilder.Where(" o.MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrWhiteSpace(procMaterialGroupCustomPagedQuery.Version))
            {
                procMaterialGroupCustomPagedQuery.Version = $"%{procMaterialGroupCustomPagedQuery.Version}%";
                sqlBuilder.Where(" o.Version like @Version ");
            }

            var offSet = (procMaterialGroupCustomPagedQuery.PageIndex - 1) * procMaterialGroupCustomPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procMaterialGroupCustomPagedQuery.PageSize });
            sqlBuilder.AddParameters(procMaterialGroupCustomPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var customProcMaterialGroupViewTask = conn.QueryAsync<CustomProcMaterialGroupView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var customProcMaterialGroupView = await customProcMaterialGroupViewTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<CustomProcMaterialGroupView>(customProcMaterialGroupView, procMaterialGroupCustomPagedQuery.PageIndex, procMaterialGroupCustomPagedQuery.PageSize, totalCount);

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
            //sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(procMaterialGroupQuery.GroupCode))
            {
                sqlBuilder.Where(" GroupCode = @GroupCode ");
            }

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
        const string GetPagedCustomInfoDataSqlTemplate = @"SELECT 
g.Id,
g.SiteId,
g.GroupCode,
g.GroupName,
g.GroupVersion,
g.Remark,
g.CreatedBy,
g.CreatedOn,
g.UpdatedBy,
g.UpdatedOn,
g.IsDeleted,
o.MaterialCode,
o.MaterialName,
o.Version
                                                            FROM `proc_material_group` g
                                                            LEFT JOIN proc_material o on o.GroupId = g.Id
/**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedCustomInfoCountSqlTemplate = @"SELECT COUNT(1) 
                                                FROM `proc_material_group` g
                                                LEFT JOIN proc_material o on o.GroupId = g.Id  /**where**/ ";
        const string GetProcMaterialGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_material_group` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_material_group`(  `Id`, `SiteId`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @GroupCode, @GroupName, @GroupVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_material_group`(  `Id`, `SiteId`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @GroupCode, @GroupName, @GroupVersion, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_material_group` SET  SiteId = @SiteId, GroupName = @GroupName, GroupVersion = @GroupVersion, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_material_group` SET   SiteId = @SiteId, GroupCode = @GroupCode, GroupName = @GroupName, GroupVersion = @GroupVersion, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_material_group` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_material_group` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_material_group`  WHERE Id = @Id ";
        const string GetByIdAndSiteIdSql = @"SELECT 
                               `Id`, `SiteId`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_material_group`  WHERE Id = @Id And SiteId=@siteId ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `GroupCode`, `GroupName`, `GroupVersion`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_material_group`  WHERE Id IN @ids ";
    }
}
