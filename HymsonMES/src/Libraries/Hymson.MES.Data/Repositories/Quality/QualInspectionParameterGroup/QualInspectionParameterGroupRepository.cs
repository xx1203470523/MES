using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Quality.View;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（全检参数表）
    /// </summary>
    public partial class QualInspectionParameterGroupRepository : BaseRepository, IQualInspectionParameterGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualInspectionParameterGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualInspectionParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualInspectionParameterGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualInspectionParameterGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualInspectionParameterGroupEntity> entities)
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
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<QualInspectionParameterGroupEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualInspectionParameterGroupEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualInspectionParameterGroupEntity>> GetByProductProcedureListAsync(EntityByProductProcedureQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualInspectionParameterGroupEntity>(GetByProductProcedureSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualInspectionParameterGroupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualInspectionParameterGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualInspectionParameterGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualInspectionParameterGroupEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualInspectionParameterGroupEntity>> GetEntitiesAsync(QualInspectionParameterGroupQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualInspectionParameterGroupEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualInspectionParameterGroupView>> GetPagedListAsync(QualInspectionParameterGroupPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("proc_material PM ON PM.Id = T.MaterialId");
            sqlBuilder.LeftJoin("proc_procedure PP ON PP.Id = T.ProcedureId");
            sqlBuilder.Select("T.*");
            sqlBuilder.Select("PM.MaterialCode, PM.MaterialName");
            sqlBuilder.Select("PP.Code AS ProcedureCode, PP.Name AS ProcedureName");
            sqlBuilder.OrderBy("T.UpdatedOn DESC");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("T.Status = @Status");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("T.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("T.Name LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where("PM.MaterialCode LIKE @MaterialCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialName))
            {
                pagedQuery.MaterialName = $"%{pagedQuery.MaterialName}%";
                sqlBuilder.Where("PM.MaterialName LIKE @MaterialName");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureCode))
            {
                pagedQuery.ProcedureCode = $"%{pagedQuery.ProcedureCode}%";
                sqlBuilder.Where("PP.Code LIKE @ProcedureCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureName))
            {
                pagedQuery.ProcedureName = $"%{pagedQuery.ProcedureName}%";
                sqlBuilder.Where("PP.Name LIKE @ProcedureName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualInspectionParameterGroupView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualInspectionParameterGroupView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualInspectionParameterGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_inspection_parameter_group` T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `qual_inspection_parameter_group` T /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `qual_inspection_parameter_group` /**where**/  ";

        const string InsertSql = "INSERT IGNORE `qual_inspection_parameter_group`(`Id`, `Code`, `Name`, `Version`, `Status`, `MaterialId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Version, @Status, @MaterialId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT IGNORE `qual_inspection_parameter_group`(`Id`, `Code`, `Name`, `Version`, `Status`, `MaterialId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Version, @Status, @MaterialId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE `qual_inspection_parameter_group` SET Name = @Name, Version = @Version, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `qual_inspection_parameter_group` SET Name = @Name, Version = @Version, Status = @Status, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `qual_inspection_parameter_group` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `qual_inspection_parameter_group` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByCodeSql = "SELECT * FROM qual_inspection_parameter_group WHERE IsDeleted = 0 AND SiteId = @Site AND Code = @Code AND Version = @Version LIMIT 1";
        const string GetByProductProcedureSql = "SELECT * FROM qual_inspection_parameter_group WHERE IsDeleted = 0 AND SiteId = @SiteId AND MaterialId = @ProductId AND ProcedureId = @ProcedureId";
        const string GetByIdSql = @"SELECT * FROM `qual_inspection_parameter_group`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `qual_inspection_parameter_group`  WHERE Id IN @Ids ";

        const string UpdateStatusSql = "UPDATE `qual_inspection_parameter_group` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

    }
}
