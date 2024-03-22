using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（IQC检验水平）
    /// </summary>
    public partial class QualIqcLevelRepository : BaseRepository, IQualIqcLevelRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualIqcLevelRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualIqcLevelEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualIqcLevelEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualIqcLevelEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualIqcLevelEntity> entities)
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
        public async Task<QualIqcLevelEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualIqcLevelEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcLevelEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcLevelEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcLevelEntity>> GetEntitiesAsync(QualIqcLevelQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("Type = @Type");
            if (query.MaterialId.HasValue)
            {
                sqlBuilder.Where("MaterialId = @MaterialId");
            }
            if (query.SupplierId.HasValue)
            {
                sqlBuilder.Where("SupplierId = @SupplierId");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualIqcLevelEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcLevelEntity>> GetPagedListAsync(QualIqcLevelPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.MaterialIds != null) sqlBuilder.Where(" MaterialId IN @MaterialIds ");
            if (pagedQuery.SupplierIds != null) sqlBuilder.Where(" SupplierId IN @SupplierIds ");
            if (pagedQuery.Type.HasValue) sqlBuilder.Where("Type = @Type");
            if (pagedQuery.Status.HasValue) sqlBuilder.Where("Status = @Status");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<QualIqcLevelEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<QualIqcLevelEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualIqcLevelRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_level /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_iqc_level /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_iqc_level /**where**/  ";

        const string InsertSql = "INSERT INTO qual_iqc_level(  `Id`, `MaterialId`, `SupplierId`, `Type`, `Level`, `Status`, AcceptanceLevel, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @MaterialId, @SupplierId, @Type, @Level, @Status, @AcceptanceLevel, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_iqc_level(  `Id`, `MaterialId`, `SupplierId`, `Type`, `Level`, `Status`, AcceptanceLevel, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @MaterialId, @SupplierId, @Type, @Level, @Status, @AcceptanceLevel, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_iqc_level SET MaterialId = @MaterialId, SupplierId = @SupplierId, Type = @Type, Level = @Level, Status = @Status, AcceptanceLevel = @AcceptanceLevel, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_iqc_level SET MaterialId = @MaterialId, SupplierId = @SupplierId, Type = @Type, Level = @Level, Status = @Status, AcceptanceLevel = @AcceptanceLevel, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_iqc_level SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_iqc_level SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_iqc_level WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_iqc_level WHERE Id IN @Ids ";

    }
}
