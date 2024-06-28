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
    /// 仓储（字段定义列表数据）
    /// </summary>
    public partial class InteBusinessFieldListRepository : BaseRepository, IInteBusinessFieldListRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteBusinessFieldListRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteBusinessFieldListEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteBusinessFieldListEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteBusinessFieldListEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteBusinessFieldListEntity> entities)
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
        public async Task<IEnumerable<InteBusinessFieldListEntity>> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldListEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldListEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldListEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteBusinessFieldListEntity>> GetEntitiesAsync(InteBusinessFieldListQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteBusinessFieldListEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteBusinessFieldListEntity>> GetPagedListAsync(InteBusinessFieldListPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<InteBusinessFieldListEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteBusinessFieldListEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteBusinessFieldListRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_business_field_list /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_business_field_list /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_business_field_list /**where**/  ";

        const string InsertSql = "INSERT INTO inte_business_field_list(  `Id`, `SiteId`, `Seq`, `iSDefault`, `BusinessFieldId`, `FieldLabel`, `FieldValue`) VALUES (  @Id, @SiteId, @Seq, @iSDefault, @BusinessFieldId, @FieldLabel, @FieldValue) ";
        const string InsertsSql = "INSERT INTO inte_business_field_list(  `Id`, `SiteId`, `Seq`, `iSDefault`, `BusinessFieldId`, `FieldLabel`, `FieldValue`) VALUES (  @Id, @SiteId, @Seq, @iSDefault, @BusinessFieldId, @FieldLabel, @FieldValue) ";

        const string UpdateSql = "UPDATE inte_business_field_list SET   SiteId = @SiteId, Seq = @Seq, iSDefault = @iSDefault, BusinessFieldId = @BusinessFieldId, FieldLabel = @FieldLabel, FieldValue = @FieldValue WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_business_field_list SET   SiteId = @SiteId, Seq = @Seq, iSDefault = @iSDefault, BusinessFieldId = @BusinessFieldId, FieldLabel = @FieldLabel, FieldValue = @FieldValue WHERE Id = @Id ";
       
        const string DeleteSql = "DELETE FROM inte_business_field_list WHERE BusinessFieldId = @Id ";
        const string DeletesSql = "DELETE FROM inte_business_field_list WHERE BusinessFieldId IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_business_field_list WHERE BusinessFieldId = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_business_field_list WHERE BusinessFieldId IN @Ids ";

    }
}
