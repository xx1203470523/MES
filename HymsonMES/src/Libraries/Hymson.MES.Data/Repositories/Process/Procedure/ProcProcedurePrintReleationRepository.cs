/*
 *creator: Karl
 *
 *describe: 工序配置打印表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:24:06
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
    /// 工序配置打印表仓储
    /// </summary>
    public partial class ProcProcedurePrintReleationRepository : IProcProcedurePrintReleationRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcProcedurePrintReleationRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedurePrintReleationEntity>> GetPagedInfoAsync(ProcProcedurePrintReleationPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(query.SiteCode))
            {
                sqlBuilder.Where("SiteCode=@SiteCode");
            }
            if (query.ProcedureId > 0)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedurePrintReleationEntitiesTask = conn.QueryAsync<ProcProcedurePrintReleationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedurePrintReleationEntities = await procProcedurePrintReleationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedurePrintReleationEntity>(procProcedurePrintReleationEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcedurePrintReleationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ProcProcedurePrintReleationEntity> procProcedurePrintReleationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procProcedurePrintReleationEntitys);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByProcedureIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByProcedureIdSql, new { ProcedureId = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedurePrintReleationEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcedurePrintReleationEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedurePrintReleationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcedurePrintReleationEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcedurePrintReleationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedurePrintReleationEntity>> GetProcProcedurePrintReleationEntitiesAsync(ProcProcedurePrintReleationQuery procProcedurePrintReleationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcedurePrintReleationEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedurePrintReleationEntities = await conn.QueryAsync<ProcProcedurePrintReleationEntity>(template.RawSql, procProcedurePrintReleationQuery);
            return procProcedurePrintReleationEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedurePrintReleationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcedurePrintReleationEntity procProcedurePrintReleationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcedurePrintReleationEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedurePrintReleationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcedurePrintReleationEntity procProcedurePrintReleationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcedurePrintReleationEntity);
        }
		
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcedurePrintReleationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ProcProcedurePrintReleationEntity> procProcedurePrintReleationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcedurePrintReleationEntitys);
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
    }

    public partial class ProcProcedurePrintReleationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_procedure_print_releation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_procedure_print_releation` /**where**/ ";
        const string GetProcProcedurePrintReleationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_procedure_print_releation` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_procedure_print_releation`(  `Id`, `SiteCode`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ProcedureId, @MaterialId, @Version, @TemplateId, @Copy, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_procedure_print_releation`(  `Id`, `SiteCode`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ProcedureId, @MaterialId, @Version, @TemplateId, @Copy, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_procedure_print_releation` SET   SiteCode = @SiteCode, ProcedureId = @ProcedureId, MaterialId = @MaterialId, Version = @Version, TemplateId = @TemplateId, Copy = @Copy, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteByProcedureIdSql = "UPDATE `proc_procedure_print_releation` SET IsDeleted = '1' WHERE ProcedureId = @ProcedureId ";
        const string DeletesSql = "UPDATE `proc_procedure_print_releation` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_procedure_print_releation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_procedure_print_releation`  WHERE Id IN @ids ";
    }
}
