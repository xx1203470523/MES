using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（资源资质认证（物理删除））
    /// </summary>
    public partial class ProcResourceQualificationAuthenticationRelationRepository : BaseRepository, IProcResourceQualificationAuthenticationRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcResourceQualificationAuthenticationRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceQualificationAuthenticationRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcResourceQualificationAuthenticationRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceQualificationAuthenticationRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcResourceQualificationAuthenticationRelationEntity> entities)
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
        public async Task<int> DeleteByResourceIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByResourceIdSql, new { ResourceId = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceQualificationAuthenticationRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcResourceQualificationAuthenticationRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceQualificationAuthenticationRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceQualificationAuthenticationRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceQualificationAuthenticationRelationEntity>> GetEntitiesAsync(ProcResourceQualificationAuthenticationRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");

            if (query.ResourceId.HasValue)
            {
                sqlBuilder.Where("ResourceId=@ResourceId");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcResourceQualificationAuthenticationRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceQualificationAuthenticationRelationEntity>> GetPagedListAsync(ProcResourceQualificationAuthenticationRelationPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ProcResourceQualificationAuthenticationRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceQualificationAuthenticationRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcResourceQualificationAuthenticationRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM proc_resource_qualification_authentication_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource_qualification_authentication_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM proc_resource_qualification_authentication_relation /**where**/  ";

        const string InsertSql = "INSERT INTO proc_resource_qualification_authentication_relation(  `ResourceId`, `QualificationAuthenticationiD`, `IsEnable`) VALUES (  @ResourceId, @QualificationAuthenticationiD, @IsEnable) ";
        const string InsertsSql = "INSERT INTO proc_resource_qualification_authentication_relation(  `ResourceId`, `QualificationAuthenticationiD`, `IsEnable`) VALUES (  @ResourceId, @QualificationAuthenticationiD, @IsEnable) ";

        const string UpdateSql = "UPDATE proc_resource_qualification_authentication_relation SET   ResourceId = @ResourceId, QualificationAuthenticationiD = @QualificationAuthenticationiD, IsEnable = @IsEnable WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE proc_resource_qualification_authentication_relation SET   ResourceId = @ResourceId, QualificationAuthenticationiD = @QualificationAuthenticationiD, IsEnable = @IsEnable WHERE Id = @Id ";

        const string DeleteSql = "UPDATE proc_resource_qualification_authentication_relation SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE proc_resource_qualification_authentication_relation SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM proc_resource_qualification_authentication_relation WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM proc_resource_qualification_authentication_relation WHERE Id IN @Ids ";

        const string DeleteByResourceIdSql = "delete from `proc_resource_qualification_authentication_relation` WHERE ResourceId = @ResourceId ";

    }
}
