using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（车间物料不良记录明细）
    /// </summary>
    public partial class QualMaterialUnqualifiedDataDetailRepository : BaseRepository, IQualMaterialUnqualifiedDataDetailRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualMaterialUnqualifiedDataDetailRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualMaterialUnqualifiedDataDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualMaterialUnqualifiedDataDetailEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualMaterialUnqualifiedDataDetailEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualMaterialUnqualifiedDataDetailEntity> entities)
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
        public async Task<QualMaterialUnqualifiedDataDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualMaterialUnqualifiedDataDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualMaterialUnqualifiedDataDetailEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualMaterialUnqualifiedDataDetailEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualMaterialUnqualifiedDataDetailEntity>> GetEntitiesAsync(QualMaterialUnqualifiedDataDetailQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            if (query.MaterialUnqualifiedDataId.HasValue)
            {
                sqlBuilder.Where(" MaterialUnqualifiedDataId=@MaterialUnqualifiedDataId ");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualMaterialUnqualifiedDataDetailEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualMaterialUnqualifiedDataDetailEntity>> GetPagedListAsync(QualMaterialUnqualifiedDataDetailPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<QualMaterialUnqualifiedDataDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualMaterialUnqualifiedDataDetailEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByDataIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByDataIdSql, new { DataId = id });
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualMaterialUnqualifiedDataDetailRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_material_unqualified_data_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_material_unqualified_data_detail /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_material_unqualified_data_detail /**where**/  ";

        const string InsertSql = "INSERT INTO qual_material_unqualified_data_detail(  `MaterialUnqualifiedDataId`, `UnqualifiedGroupId`, `UnqualifiedCodeId`, `CreatedBy`, `CreatedOn`) VALUES (  @MaterialUnqualifiedDataId, @UnqualifiedGroupId, @UnqualifiedCodeId, @CreatedBy, @CreatedOn) ";
        const string InsertsSql = "INSERT INTO qual_material_unqualified_data_detail(  `MaterialUnqualifiedDataId`, `UnqualifiedGroupId`, `UnqualifiedCodeId`, `CreatedBy`, `CreatedOn`) VALUES (  @MaterialUnqualifiedDataId, @UnqualifiedGroupId, @UnqualifiedCodeId, @CreatedBy, @CreatedOn) ";

        const string UpdateSql = "UPDATE qual_material_unqualified_data_detail SET   MaterialUnqualifiedDataId = @MaterialUnqualifiedDataId, UnqualifiedGroupId = @UnqualifiedGroupId, UnqualifiedCodeId = @UnqualifiedCodeId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_material_unqualified_data_detail SET   MaterialUnqualifiedDataId = @MaterialUnqualifiedDataId, UnqualifiedGroupId = @UnqualifiedGroupId, UnqualifiedCodeId = @UnqualifiedCodeId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_material_unqualified_data_detail SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_material_unqualified_data_detail SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_material_unqualified_data_detail WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_material_unqualified_data_detail WHERE Id IN @Ids ";
        const string DeleteByDataIdSql = "delete from `qual_material_unqualified_data_detail` WHERE MaterialUnqualifiedDataId=@DataId ";

    }
}
