/*
 *creator: Karl
 *
 *describe: 物料替代组件表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-09 11:28:39
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
    /// 物料替代组件表仓储
    /// </summary>
    public partial class ProcReplaceMaterialRepository : IProcReplaceMaterialRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcReplaceMaterialRepository(IOptions<ConnectionOptions> connectionOptions)
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
            return await conn.ExecuteAsync(DeletesSql, new { Ids=ids });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcReplaceMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcReplaceMaterialEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procReplaceMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcReplaceMaterialEntity>> GetPagedInfoAsync(ProcReplaceMaterialPagedQuery procReplaceMaterialPagedQuery)
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
           
            var offSet = (procReplaceMaterialPagedQuery.PageIndex - 1) * procReplaceMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procReplaceMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(procReplaceMaterialPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procReplaceMaterialEntitiesTask = conn.QueryAsync<ProcReplaceMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procReplaceMaterialEntities = await procReplaceMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcReplaceMaterialEntity>(procReplaceMaterialEntities, procReplaceMaterialPagedQuery.PageIndex, procReplaceMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procReplaceMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcReplaceMaterialEntity>> GetProcReplaceMaterialEntitiesAsync(ProcReplaceMaterialQuery procReplaceMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcReplaceMaterialEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procReplaceMaterialEntities = await conn.QueryAsync<ProcReplaceMaterialEntity>(template.RawSql, procReplaceMaterialQuery);
            return procReplaceMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcReplaceMaterialEntity procReplaceMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcReplaceMaterialEntity> procReplaceMaterialEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procReplaceMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procReplaceMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcReplaceMaterialEntity procReplaceMaterialEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procReplaceMaterialEntity);
        }

        /// <summary>
        /// 批量更新-只更新 UpdateOn,  UpdateBy,  ReplaceMaterialId,  IsUse 
        /// </summary>
        /// <param name="procReplaceMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcReplaceMaterialEntity> procReplaceMaterialEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procReplaceMaterialEntitys);
        }
    }

    public partial class ProcReplaceMaterialRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_replace_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_replace_material` /**where**/ ";
        const string GetProcReplaceMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_replace_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_replace_material`(  `Id`, `SiteCode`, `MaterialId`, `ReplaceMaterialId`, `IsUse`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @MaterialId, @ReplaceMaterialId, @IsUse, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_replace_material` SET   SiteCode = @SiteCode, MaterialId = @MaterialId, ReplaceMaterialId = @ReplaceMaterialId, IsUse = @IsUse, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_replace_material` SET   ReplaceMaterialId = @ReplaceMaterialId, IsUse = @IsUse, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_replace_material` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_replace_material` SET IsDeleted = '1' WHERE Id in @Ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `MaterialId`, `ReplaceMaterialId`, `IsUse`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_replace_material`  WHERE Id = @Id ";
    }
}
