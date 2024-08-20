using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（工序资质认证（物理删除））
    /// </summary>
    public partial class ProcProcedureQualificationAuthenticationRelationRepository : BaseRepository, IProcProcedureQualificationAuthenticationRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcProcedureQualificationAuthenticationRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcedureQualificationAuthenticationRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcProcedureQualificationAuthenticationRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcedureQualificationAuthenticationRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcProcedureQualificationAuthenticationRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByProcedureIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByProcedureIdSql, new { ProcedureId = id });
        }

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByProcedureIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByProcedureIdsSql, new { ProcedureIds = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedureQualificationAuthenticationRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcedureQualificationAuthenticationRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureQualificationAuthenticationRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcedureQualificationAuthenticationRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureQualificationAuthenticationRelationEntity>> GetEntitiesAsync(ProcProcedureQualificationAuthenticationRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");

            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }
            if (query.QualificationAuthenticationIds != null && query.QualificationAuthenticationIds.Any())
            {
                sqlBuilder.Where("QualificationAuthenticationId in @QualificationAuthenticationIds");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcedureQualificationAuthenticationRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureQualificationAuthenticationRelationEntity>> GetPagedListAsync(ProcProcedureQualificationAuthenticationRelationPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ProcProcedureQualificationAuthenticationRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcedureQualificationAuthenticationRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcProcedureQualificationAuthenticationRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM proc_procedure_qualification_authentication_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM proc_procedure_qualification_authentication_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM proc_procedure_qualification_authentication_relation /**where**/  ";

        const string InsertSql = "INSERT INTO proc_procedure_qualification_authentication_relation(  `ProcedureId`, `QualificationAuthenticationiD`, `IsEnable`) VALUES (  @ProcedureId, @QualificationAuthenticationiD, @IsEnable) ";
        const string InsertsSql = "INSERT INTO proc_procedure_qualification_authentication_relation(  `ProcedureId`, `QualificationAuthenticationiD`, `IsEnable`) VALUES (  @ProcedureId, @QualificationAuthenticationiD, @IsEnable) ";

        const string UpdateSql = "UPDATE proc_procedure_qualification_authentication_relation SET   ProcedureId = @ProcedureId, QualificationAuthenticationiD = @QualificationAuthenticationiD, IsEnable = @IsEnable WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE proc_procedure_qualification_authentication_relation SET   ProcedureId = @ProcedureId, QualificationAuthenticationiD = @QualificationAuthenticationiD, IsEnable = @IsEnable WHERE Id = @Id ";

        const string DeleteSql = "UPDATE proc_procedure_qualification_authentication_relation SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE proc_procedure_qualification_authentication_relation SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeleteByProcedureIdSql = "delete from `proc_procedure_qualification_authentication_relation` WHERE ProcedureId = @ProcedureId ";
        const string DeleteByProcedureIdsSql = "delete from `proc_procedure_qualification_authentication_relation` WHERE ProcedureId in @ProcedureIds ";

        const string GetByIdSql = @"SELECT * FROM proc_procedure_qualification_authentication_relation WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM proc_procedure_qualification_authentication_relation WHERE Id IN @Ids ";

    }
}
