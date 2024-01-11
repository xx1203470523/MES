using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Query;
using IdGen;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（配方操作组）
    /// </summary>
    public partial class ProcFormulaOperationGroupRepository : BaseRepository, IProcFormulaOperationGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcFormulaOperationGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcFormulaOperationGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcFormulaOperationGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcFormulaOperationGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcFormulaOperationGroupEntity> entities)
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
        public async Task<ProcFormulaOperationGroupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcFormulaOperationGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ProcFormulaOperationGroupEntity> GetByCodeAsync(string code)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcFormulaOperationGroupEntity>(GetByCodeSql, new { Code = code });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaOperationGroupEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaOperationGroupEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaOperationGroupEntity>> GetEntitiesAsync(ProcFormulaOperationGroupQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcFormulaOperationGroupEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaOperationGroupEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaOperationGroupEntity>> GetPagedInfoAsync(ProcFormulaOperationGroupPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name like @Name");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ProcFormulaOperationGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcFormulaOperationGroupEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcFormulaOperationGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_formula_operation_group` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_formula_operation_group` /**where**/ ";
        const string GetProcFormulaOperationGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_formula_operation_group` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_formula_operation_group`(  `Id`, `Code`, `Name`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `proc_formula_operation_group`(  `Id`, `Code`, `Name`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `proc_formula_operation_group` SET   Code = @Code, Name = @Name, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_formula_operation_group` SET   Code = @Code, Name = @Name, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_formula_operation_group` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_formula_operation_group` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `proc_formula_operation_group`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_formula_operation_group`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT * FROM `proc_formula_operation_group`  WHERE Code = @Code AND IsDeleted=0 ";
    }
}
