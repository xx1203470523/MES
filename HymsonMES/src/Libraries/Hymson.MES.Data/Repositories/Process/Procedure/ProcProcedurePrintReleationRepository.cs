/*
 *creator: Karl
 *
 *describe: 工序配置打印表 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-13 02:24:06
 */

using Dapper;
using FluentValidation.Validators;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 工序配置打印表仓储
    /// </summary>
    public partial class ProcProcedurePrintRelationRepository : IProcProcedurePrintRelationRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcProcedurePrintRelationRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedurePrintRelationEntity>> GetPagedInfoAsync(ProcProcedurePrintReleationPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (query.SiteId > 0)
            {
                sqlBuilder.Where("SiteId = @SiteId");
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
            var procProcedurePrintReleationEntitiesTask = conn.QueryAsync<ProcProcedurePrintRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedurePrintRelationEntities = await procProcedurePrintReleationEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedurePrintRelationEntity>(procProcedurePrintRelationEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProcedurePrintReleationEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcProcedurePrintRelationEntity> procProcedurePrintReleationEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcedurePrintReleationEntitys);
        }

        /// <summary>
        /// 删除
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
        public async Task<ProcProcedurePrintRelationEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcProcedurePrintRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedurePrintRelationEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcProcedurePrintRelationEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProcedurePrintReleationQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedurePrintRelationEntity>> GetProcProcedurePrintReleationEntitiesAsync(ProcProcedurePrintReleationQuery procProcedurePrintReleationQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcedurePrintReleationEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            if (procProcedurePrintReleationQuery.ProcedureId != 0)
            {
                sqlBuilder.Where("ProcedureId = @ProcedureId");
            }
            if (string.IsNullOrEmpty(procProcedurePrintReleationQuery.Version) != true)
            {
                sqlBuilder.Where("Version = @Version");
            }
            if (procProcedurePrintReleationQuery.MaterialId != 0)
            {
                sqlBuilder.Where("MaterialId = @MaterialId");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procProcedurePrintReleationEntities = await conn.QueryAsync<ProcProcedurePrintRelationEntity>(template.RawSql, procProcedurePrintReleationQuery);
            return procProcedurePrintReleationEntities;
            
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProcedurePrintReleationEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcedurePrintRelationEntity procProcedurePrintReleationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procProcedurePrintReleationEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProcedurePrintReleationEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcedurePrintRelationEntity procProcedurePrintReleationEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procProcedurePrintReleationEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProcedurePrintReleationEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcProcedurePrintRelationEntity> procProcedurePrintReleationEntitys)
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
            return await conn.ExecuteAsync(DeletesSql, new { ids = ids });
        }
    }

    public partial class ProcProcedurePrintRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_procedure_print_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_procedure_print_relation` /**where**/ ";
        const string GetProcProcedurePrintReleationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_procedure_print_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_procedure_print_relation`(  `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ProcedureId, @MaterialId, @Version, @TemplateId, @Copy, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_procedure_print_relation` SET    ProcedureId = @ProcedureId, MaterialId = @MaterialId, Version = @Version, TemplateId = @TemplateId, Copy = @Copy, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteByProcedureIdSql = "delete from `proc_procedure_print_relation` WHERE ProcedureId = @ProcedureId ";
        const string DeletesSql = "UPDATE `proc_procedure_print_relation` SET IsDeleted = Id WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_procedure_print_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `MaterialId`, `Version`, `TemplateId`, `Copy`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_procedure_print_relation`  WHERE Id IN @ids ";
    }
}
