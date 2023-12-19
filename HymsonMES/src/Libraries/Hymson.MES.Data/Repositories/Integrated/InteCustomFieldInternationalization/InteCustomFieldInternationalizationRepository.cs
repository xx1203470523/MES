using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（字段国际化）
    /// </summary>
    public partial class InteCustomFieldInternationalizationRepository : BaseRepository, IInteCustomFieldInternationalizationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteCustomFieldInternationalizationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCustomFieldInternationalizationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteCustomFieldInternationalizationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCustomFieldInternationalizationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteCustomFieldInternationalizationEntity> entities)
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
        public async Task<InteCustomFieldInternationalizationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteCustomFieldInternationalizationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldInternationalizationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomFieldInternationalizationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldInternationalizationEntity>> GetEntitiesAsync(InteCustomFieldInternationalizationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomFieldInternationalizationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCustomFieldInternationalizationEntity>> GetPagedListAsync(InteCustomFieldInternationalizationPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<InteCustomFieldInternationalizationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCustomFieldInternationalizationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 批量根据字段ID获取字段语言设置信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldInternationalizationEntity>> GetEntitiesByCustomFieldIdsAsync(InteCustomFieldInternationalizationByCustomFieldIdsQuery query) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomFieldInternationalizationEntity>(GetEntitiesByCustomFieldIdsSql, query);
        }

        /// <summary>
        /// 根据自定义字段IDs 批量硬删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletedByCustomFieldIdsAsync(InternationalizationDeleteByCustomFieldIdsCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletedByCustomFieldIdsSql, command);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteCustomFieldInternationalizationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_custom_field_internationalization /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_custom_field_internationalization /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM inte_custom_field_internationalization /**where**/  ";

        const string InsertSql = "INSERT INTO inte_custom_field_internationalization(  `Id`, `SiteId`, `CustomFieldId`, `LanguageType`, `TranslationValue`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @CustomFieldId, @LanguageType, @TranslationValue, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO inte_custom_field_internationalization(  `Id`, `SiteId`, `CustomFieldId`, `LanguageType`, `TranslationValue`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @CustomFieldId, @LanguageType, @TranslationValue, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE inte_custom_field_internationalization SET   SiteId = @SiteId, CustomFieldId = @CustomFieldId, LanguageType = @LanguageType, TranslationValue = @TranslationValue, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_custom_field_internationalization SET   SiteId = @SiteId, CustomFieldId = @CustomFieldId, LanguageType = @LanguageType, TranslationValue = @TranslationValue, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_custom_field_internationalization SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_custom_field_internationalization SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_custom_field_internationalization WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_custom_field_internationalization WHERE Id IN @Ids ";

        const string GetEntitiesByCustomFieldIdsSql = @"SELECT * FROM inte_custom_field_internationalization WHERE SiteId=@SiteId and IsDeleted=0 and CustomFieldId in @CustomFieldIds ";

        const string DeletedByCustomFieldIdsSql = @"DELETE FROM inte_custom_field_internationalization WHERE  SiteId=@SiteId and CustomFieldId in @CustomFieldIds ";
    }
}
