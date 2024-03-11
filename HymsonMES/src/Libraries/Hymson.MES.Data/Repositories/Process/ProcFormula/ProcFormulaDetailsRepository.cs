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
    /// 仓储（配方维护详情）
    /// </summary>
    public partial class ProcFormulaDetailsRepository : BaseRepository, IProcFormulaDetailsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcFormulaDetailsRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcFormulaDetailsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcFormulaDetailsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcFormulaDetailsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcFormulaDetailsEntity> entities)
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
        public async Task<ProcFormulaDetailsEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcFormulaDetailsEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaDetailsEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaDetailsEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaDetailsEntity>> GetEntitiesAsync(ProcFormulaDetailsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaDetailsEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaDetailsEntity>> GetPagedListAsync(ProcFormulaDetailsPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ProcFormulaDetailsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcFormulaDetailsEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 获取对应FormulaId的详情
        /// </summary>
        /// <param name="formulaId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaDetailsEntity>> GetFormulaDetailsByFormulaIdAsync(long formulaId) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaDetailsEntity>(GetFormulaDetailsByFormulaIdSql, new { FormulaId= formulaId });
        }

        /// <summary>
        /// 根据FormulaId硬删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueByFormulaIdsAsync(long[] formulaIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesTrueByFormulaIdsSql, new { FormulaIds= formulaIds });
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcFormulaDetailsRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM proc_formula_details /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM proc_formula_details /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM proc_formula_details /**where**/  ";

        const string InsertSql = "INSERT INTO proc_formula_details(  `Id`, `Serial`, `FormulaId`, `FormulaOperationId`, `MaterialId`, `MaterialGroupId`, `ParameterId`, `FunctionCode`, `Setvalue`, `UpperLimit`, `LowLimit`, `Unit`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`,`Sort`) VALUES (  @Id, @Serial, @FormulaId, @FormulaOperationId, @MaterialId, @MaterialGroupId, @ParameterId, @FunctionCode, @Setvalue, @UpperLimit, @LowLimit, @Unit, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId,@Sort) ";
        const string InsertsSql = "INSERT INTO proc_formula_details(  `Id`, `Serial`, `FormulaId`, `FormulaOperationId`, `MaterialId`, `MaterialGroupId`, `ParameterId`, `FunctionCode`, `Setvalue`, `UpperLimit`, `LowLimit`, `Unit`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`,`Sort`) VALUES (  @Id, @Serial, @FormulaId, @FormulaOperationId, @MaterialId, @MaterialGroupId, @ParameterId, @FunctionCode, @Setvalue, @UpperLimit, @LowLimit, @Unit, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId,@Sort) ";

        const string UpdateSql = "UPDATE proc_formula_details SET   Serial = @Serial, FormulaId = @FormulaId, FormulaOperationId = @FormulaOperationId, MaterialId = @MaterialId, MaterialGroupId = @MaterialGroupId, ParameterId = @ParameterId, FunctionCode = @FunctionCode, Setvalue = @Setvalue, UpperLimit = @UpperLimit, LowLimit = @LowLimit, Unit = @Unit, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE proc_formula_details SET   Serial = @Serial, FormulaId = @FormulaId, FormulaOperationId = @FormulaOperationId, MaterialId = @MaterialId, MaterialGroupId = @MaterialGroupId, ParameterId = @ParameterId, FunctionCode = @FunctionCode, Setvalue = @Setvalue, UpperLimit = @UpperLimit, LowLimit = @LowLimit, Unit = @Unit, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE proc_formula_details SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE proc_formula_details SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM proc_formula_details WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM proc_formula_details WHERE Id IN @Ids ";

        const string GetFormulaDetailsByFormulaIdSql = @"SELECT * FROM proc_formula_details WHERE FormulaId = @FormulaId ORDER BY Sort";

        const string DeletesTrueByFormulaIdsSql = @"DELETE FROM proc_formula_details WHERE FormulaId in @FormulaIds ";
    }
}
