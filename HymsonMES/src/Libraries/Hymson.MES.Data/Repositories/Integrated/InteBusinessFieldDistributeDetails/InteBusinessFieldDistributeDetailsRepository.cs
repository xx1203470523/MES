using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（字段分配管理详情）
    /// </summary>
    public partial class InteBusinessFieldDistributeDetailsRepository : BaseRepository, IInteBusinessFieldDistributeDetailsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteBusinessFieldDistributeDetailsRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteBusinessFieldDistributeDetailsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteBusinessFieldDistributeDetailsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteBusinessFieldDistributeDetailsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteBusinessFieldDistributeDetailsEntity> entities)
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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldDistributeDetailsEntity>> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldDistributeDetailsEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldDistributeDetailsEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldDistributeDetailsEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldDistributeDetailsEntity>> GetEntitiesAsync(InteBusinessFieldDistributeDetailsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldDistributeDetailsEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteBusinessFieldDistributeDetailsEntity>> GetPagedListAsync(InteBusinessFieldDistributeDetailsPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<InteBusinessFieldDistributeDetailsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteBusinessFieldDistributeDetailsEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据BusinessFieldId获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldDistributeDetailsEntity>> GetByBusinessFieldIdsAsync(InteBusinessFieldDistributeDetailBusinessFieldIdIdsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldDistributeDetailsEntity>(GetByBusinessFieldIdsSql, query);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteBusinessFieldDistributeDetailsRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_business_field_distribute_details /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_business_field_distribute_details /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_business_field_distribute_details /**where**/  ";

        const string InsertSql = "INSERT INTO inte_business_field_distribute_details(  `Id`, `SiteId`, `BusinessFieldFistributeid`, `BusinessFieldId`, `Seq`, `IsRequired`) VALUES (  @Id, @SiteId, @BusinessFieldFistributeid, @BusinessFieldId, @Seq, @IsRequired) ";
        const string InsertsSql = "INSERT INTO inte_business_field_distribute_details(  `Id`, `SiteId`, `BusinessFieldFistributeid`, `BusinessFieldId`, `Seq`, `IsRequired`) VALUES (  @Id, @SiteId, @BusinessFieldFistributeid, @BusinessFieldId, @Seq, @IsRequired) ";

        const string UpdateSql = "UPDATE inte_business_field_distribute_details SET   SiteId = @SiteId, BusinessFieldFistributeid = @BusinessFieldFistributeid, BusinessFieldId = @BusinessFieldId, Seq = @Seq, IsRequired = @IsRequired WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_business_field_distribute_details SET   SiteId = @SiteId, BusinessFieldFistributeid = @BusinessFieldFistributeid, BusinessFieldId = @BusinessFieldId, Seq = @Seq, IsRequired = @IsRequired WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_business_field_distribute_details SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_business_field_distribute_details SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_business_field_distribute_details WHERE BusinessFieldFistributeid = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_business_field_distribute_details WHERE Id IN @Ids ";

        const string GetByBusinessFieldIdsSql = @"SELECT * FROM `inte_business_field_distribute_details` WHERE SiteId=@SiteId AND BusinessFieldId in @BusinessFieldIds";

    }
}
