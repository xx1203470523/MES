using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.View;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;



namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（配方维护）
    /// </summary>
    public partial class ProcFormulaRepository : BaseRepository, IProcFormulaRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcFormulaRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcFormulaEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcFormulaEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcFormulaEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcFormulaEntity> entities)
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
        public async Task<ProcFormulaEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcFormulaEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaEntity>> GetEntitiesAsync(ProcFormulaQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (query.MaterialId.HasValue && query.MaterialId.Value>0)
            {
                sqlBuilder.Where(" MaterialId = @MaterialId ");
            }
            if (query.ProcedureId.HasValue && query.ProcedureId.Value > 0)
            {
                sqlBuilder.Where(" ProcedureId = @ProcedureId ");
            }
            if (query.Status.HasValue )
            {
                sqlBuilder.Where(" Status = @Status ");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaView>> GetPagedListAsync(ProcFormulaPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("f.*, m.MaterialCode, m.MaterialName, p.Code as ProcedureCode ,p.Name as ProcedureName ");
            sqlBuilder.LeftJoin(" proc_material m ON m.Id = f.MaterialId ");
            sqlBuilder.LeftJoin(" proc_procedure p ON p.Id = f.ProcedureId ");
            sqlBuilder.OrderBy("f.UpdatedOn DESC");
            sqlBuilder.Where("f.IsDeleted = 0");
            sqlBuilder.Where("f.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace( pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("f.Code LIKE  @Code");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("f.Name LIKE  @Name");
            }
            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("f.Status =  @Status");
            }

            if (pagedQuery.MaterialId.HasValue)
            {
                sqlBuilder.Where("f.MaterialId =  @MaterialId");
            }
            if (pagedQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where("f.ProcedureId =  @ProcedureId");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode)) 
            {
                sqlBuilder.Where("m.MaterialCode =  @MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureCode))
            {
                sqlBuilder.Where("p.Code =  @ProcedureCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialName))
            {
                pagedQuery.MaterialName = $"%{pagedQuery.MaterialName}%";
                sqlBuilder.Where("m.MaterialName LIKE  @MaterialName");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureName))
            {
                pagedQuery.ProcedureName = $"%{pagedQuery.ProcedureName}%";
                sqlBuilder.Where("p.Name LIKE  @ProcedureName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ProcFormulaView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcFormulaView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 更新某物料 的状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        public async Task<ProcFormulaEntity> GetByCodeAndVersionAsync(ProcFormulaByCodeAndVersion query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcFormulaEntity>(GetByCodeAndVersionSql, query);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcFormulaRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM proc_formula f /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM proc_formula f /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM proc_formula /**where**/  ";

        const string InsertSql = "INSERT INTO proc_formula(  `Id`, `Code`, `Name`, `Status`, `Version`, `MaterialId`, `ProcedureId`, `EquipmentGroupId`, `FormulaOperationGroupId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @Status, @Version, @MaterialId, @ProcedureId, @EquipmentGroupId, @FormulaOperationGroupId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO proc_formula(  `Id`, `Code`, `Name`, `Status`, `Version`, `MaterialId`, `ProcedureId`, `EquipmentGroupId`, `FormulaOperationGroupId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @Code, @Name, @Status, @Version, @MaterialId, @ProcedureId, @EquipmentGroupId, @FormulaOperationGroupId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE proc_formula SET  Name = @Name, MaterialId = @MaterialId, ProcedureId = @ProcedureId, EquipmentGroupId = @EquipmentGroupId, FormulaOperationGroupId = @FormulaOperationGroupId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE proc_formula SET Name = @Name, Status = @Status, MaterialId = @MaterialId, ProcedureId = @ProcedureId, EquipmentGroupId = @EquipmentGroupId, FormulaOperationGroupId = @FormulaOperationGroupId, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE proc_formula SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE proc_formula SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM proc_formula WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM proc_formula WHERE Id IN @Ids ";

        const string UpdateStatusSql = @"UPDATE `proc_formula` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";
        const string GetByCodeAndVersionSql = @"SELECT * FROM proc_formula WHERE SiteId=@SiteId AND Code=@Code AND Version=@Version AND IsDeleted=0 ";
    }
}
