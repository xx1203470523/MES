using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;
using System.Net.NetworkInformation;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（FQC检验参数组）
    /// </summary>
    public partial class QualFqcParameterGroupRepository : BaseRepository, IQualFqcParameterGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualFqcParameterGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualFqcParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualFqcParameterGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualFqcParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }
        
        /// <summary>
        /// 更新 (仅更新状态)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(UpdateFqcParameterGroupStatusQuery entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualFqcParameterGroupEntity> entities)
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
        public async Task<QualFqcParameterGroupEntity> GetByIdAsync(long? id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcParameterGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcParameterGroupEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<QualFqcParameterGroupEntity> GetEntityAsync(QualFqcParameterGroupQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitySqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.MaterialId.HasValue)
            {
                sqlBuilder.Where("MaterialId = @MaterialId");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (query.Version!=null)
            {
                sqlBuilder.Where("Version = @Version");
            }
            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualFqcParameterGroupEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupEntity>> GetEntitiesAsync(QualFqcParameterGroupQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.MaterialId.HasValue)
            {
                sqlBuilder.Where("MaterialId = @MaterialId");
            }

            if (query.MaterialIds != null && query.MaterialIds.Any())
            {
                sqlBuilder.Where("MaterialId IN @MaterialIds");
            }

            if (query.Status != null)
            {
                sqlBuilder.Where("Status = @Status");
            }
            //排序
            if (!string.IsNullOrWhiteSpace(query.Sorting)) sqlBuilder.OrderBy(query.Sorting);
            sqlBuilder.AddParameters(query);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualFqcParameterGroupEntity>(template.RawSql, template.Parameters);
        }



        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcParameterGroupEntity>> GetPagedListAsync(QualFqcParameterGroupPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("T.*");
            sqlBuilder.OrderBy(string.IsNullOrWhiteSpace(pagedQuery.Sorting) ? "T.CreatedOn DESC" : pagedQuery.Sorting);
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualFqcParameterGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualFqcParameterGroupEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualFqcParameterGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_parameter_group T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_fqc_parameter_group T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_fqc_parameter_group /**where**/ /**orderby**/ ";
        const string GetEntitySqlTemplate = @"SELECT /**select**/ FROM qual_fqc_parameter_group /**where**/ /**orderby**/ LIMIT 1 ";

        const string InsertSql = "INSERT INTO qual_fqc_parameter_group(  `Id`, `SiteId`, `Code`, `Name`, `MaterialId`, `SampleQty`,`SamplingCount`, `LotSize`, `LotUnit`, `IsSameWorkOrder`, `IsSameWorkCenter`, `Version`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @MaterialId, @SampleQty,@SamplingCount, @LotSize, @LotUnit, @IsSameWorkOrder, @IsSameWorkCenter, @Version, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO qual_fqc_parameter_group(  `Id`, `SiteId`, `Code`, `Name`, `MaterialId`, `SampleQty`, `LotSize`, `LotUnit`, `IsSameWorkOrder`, `IsSameWorkCenter`, `Version`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @MaterialId, @SampleQty, @LotSize, @LotUnit, @IsSameWorkOrder, @IsSameWorkCenter, @Version, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE qual_fqc_parameter_group SET   SiteId = @SiteId, Code = @Code, Name = @Name, MaterialId = @MaterialId, SampleQty = @SampleQty,SamplingCount=@SamplingCount, LotSize = @LotSize, LotUnit = @LotUnit, IsSameWorkOrder = @IsSameWorkOrder, IsSameWorkCenter = @IsSameWorkCenter, Version = @Version, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE qual_fqc_parameter_group SET   SiteId = @SiteId, Code = @Code, Name = @Name, MaterialId = @MaterialId, SampleQty = @SampleQty, LotSize = @LotSize, LotUnit = @LotUnit, IsSameWorkOrder = @IsSameWorkOrder, IsSameWorkCenter = @IsSameWorkCenter, Version = @Version, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE qual_fqc_parameter_group SET Status = @Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
         
        const string DeleteSql = "UPDATE qual_fqc_parameter_group SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_fqc_parameter_group SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_fqc_parameter_group WHERE IsDeleted=0 AND Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_fqc_parameter_group WHERE IsDeleted=0 AND Id IN @Ids ";



    }
}
