using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Constants.Common;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（自定字段业务实现）
    /// </summary>
    public partial class InteCustomFieldBusinessEffectuateRepository : BaseRepository, IInteCustomFieldBusinessEffectuateRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteCustomFieldBusinessEffectuateRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCustomFieldBusinessEffectuateEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteCustomFieldBusinessEffectuateEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCustomFieldBusinessEffectuateEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteCustomFieldBusinessEffectuateEntity> entities)
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
        public async Task<InteCustomFieldBusinessEffectuateEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteCustomFieldBusinessEffectuateEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldBusinessEffectuateEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomFieldBusinessEffectuateEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldBusinessEffectuateEntity>> GetEntitiesAsync(InteCustomFieldBusinessEffectuateQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            if (query.BusinessId.HasValue)
            {
                sqlBuilder.Where("BusinessId = @BusinessId");
            }
            if (query.BusinessType.HasValue)
            {
                sqlBuilder.Where("BusinessType = @BusinessType");
            }
            if (!string.IsNullOrWhiteSpace(query.CustomFieldName))
            {
                sqlBuilder.Where("CustomFieldName = @CustomFieldName");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomFieldBusinessEffectuateEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCustomFieldBusinessEffectuateEntity>> GetPagedListAsync(InteCustomFieldBusinessEffectuatePagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<InteCustomFieldBusinessEffectuateEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCustomFieldBusinessEffectuateEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据业务ID硬删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueByBusinessIdAsync(long businessId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueByBusinessIdSql, new { BusinessId = businessId });
        }
      
        /// <summary>
        /// 通过业务ID获取业务数据
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCustomFieldBusinessEffectuateEntity>> GetBusinessEffectuatesByBusinessIdAsync(long businessId) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteCustomFieldBusinessEffectuateEntity>(GetBusinessEffectuatesByBusinessIdSql, new { BusinessId = businessId });
        }

        /// <summary>
        /// 获取自定义字段值
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetCustomeFieldValue(long businessId, InteCustomFieldBusinessTypeEnum businessType, string customFieldName)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteScalarAsync<string>(GetCustomeFieldValueSql, new { BusinessId = businessId, BusinessType = businessType, CustomFieldName = customFieldName });
        }

        /// <summary>
        /// 批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueByBusinessIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueByBusinessIdsSql, new { BusinessIds = ids });
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteCustomFieldBusinessEffectuateRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_custom_field_business_effectuate /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_custom_field_business_effectuate /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM inte_custom_field_business_effectuate /**where**/  ";

        const string InsertSql = "INSERT INTO inte_custom_field_business_effectuate(  `Id`, `SiteId`, `BusinessId`, `BusinessType`, `CustomFieldName`, `SetValue`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @BusinessId, @BusinessType, @CustomFieldName, @SetValue, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO inte_custom_field_business_effectuate(  `Id`, `SiteId`, `BusinessId`, `BusinessType`, `CustomFieldName`, `SetValue`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @BusinessId, @BusinessType, @CustomFieldName, @SetValue, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE inte_custom_field_business_effectuate SET   SiteId = @SiteId, BusinessId = @BusinessId, BusinessType = @BusinessType, CustomFieldName = @CustomFieldName, SetValue = @SetValue, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_custom_field_business_effectuate SET   SiteId = @SiteId, BusinessId = @BusinessId, BusinessType = @BusinessType, CustomFieldName = @CustomFieldName, SetValue = @SetValue, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_custom_field_business_effectuate SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_custom_field_business_effectuate SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_custom_field_business_effectuate WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_custom_field_business_effectuate WHERE Id IN @Ids ";

        const string DeleteTrueByBusinessIdSql = @"DELETE FROM inte_custom_field_business_effectuate WHERE BusinessId=@BusinessId ";

        const string DeleteTrueByBusinessIdsSql = @"DELETE FROM inte_custom_field_business_effectuate WHERE BusinessId in @BusinessIds ";

        const string GetBusinessEffectuatesByBusinessIdSql = @"SELECT * FROM inte_custom_field_business_effectuate WHERE BusinessId=@BusinessId ";

        const string GetCustomeFieldValueSql= @"SELECT SetValue FROM inte_custom_field_business_effectuate WHERE BusinessId = @BusinessId AND BusinessType = @BusinessType AND CustomFieldName = @CustomFieldName LIMIT 1;";
    }
}
