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
    /// 仓储（配方操作组维护）
    /// </summary>
    public partial class ProcFormulaOperationGroupRelatiionRepository : BaseRepository, IProcFormulaOperationGroupRelatiionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcFormulaOperationGroupRelatiionRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcFormulaOperationGroupRelatiionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcFormulaOperationGroupRelatiionEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcFormulaOperationGroupRelatiionEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcFormulaOperationGroupRelatiionEntity> entities)
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
        /// 根据操作组Ids软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesByGroupIdsAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByGroupIdsSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcFormulaOperationGroupRelatiionEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcFormulaOperationGroupRelatiionEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaOperationGroupRelatiionEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaOperationGroupRelatiionEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaOperationGroupRelatiionEntity>> GetEntitiesAsync(ProcFormulaOperationGroupRelatiionQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcFormulaOperationGroupRelatiionEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaOperationGroupRelatiionEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 根据配方操作组id查配方操作 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaOperationGroupRelatiionEntity>> GetPagedInfoAsync(ProcFormulaOperationGroupRelatiionPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
           
            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ProcFormulaOperationGroupRelatiionEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcFormulaOperationGroupRelatiionEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据操作组id查询所有所关联的操作id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcFormulaOperationGroupRelatiionEntity>> GetOperationIdsByGroupIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcFormulaOperationGroupRelatiionEntity>(GetByGroupIdSql, new { Id = id });
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcFormulaOperationGroupRelatiionRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_formula_operation_group_relatiion` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_formula_operation_group_relatiion` /**where**/ ";
        const string GetProcFormulaOperationGroupRelatiionEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_formula_operation_group_relatiion` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_formula_operation_group_relatiion`(  `Id`, `FormulaOperationId`, `FormulaOperationGroupId`, `CreatedBy`, `CreatedOn`, `SiteId`, `IsDeleted`) VALUES (   @Id, @FormulaOperationId, @FormulaOperationGroupId, @CreatedBy, @CreatedOn, @SiteId, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_formula_operation_group_relatiion`(  `Id`, `FormulaOperationId`, `FormulaOperationGroupId`, `CreatedBy`, `CreatedOn`, `SiteId`, `IsDeleted`) VALUES (   @Id, @FormulaOperationId, @FormulaOperationGroupId, @CreatedBy, @CreatedOn, @SiteId, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_formula_operation_group_relatiion` SET   FormulaOperationId = @FormulaOperationId, FormulaOperationGroupId = @FormulaOperationGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_formula_operation_group_relatiion` SET   FormulaOperationId = @FormulaOperationId, FormulaOperationGroupId = @FormulaOperationGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_formula_operation_group_relatiion` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_formula_operation_group_relatiion` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `proc_formula_operation_group_relatiion`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_formula_operation_group_relatiion`  WHERE Id IN @Ids ";

        const string GetByGroupIdSql = @"SELECT * FROM `proc_formula_operation_group_relatiion`  WHERE FormulaOperationGroupId = @Id AND IsDeleted = 0";

        const string DeletesByGroupIdsSql = "UPDATE `proc_formula_operation_group_relatiion` SET IsDeleted = Id WHERE FormulaOperationGroupId IN @Ids";
    
    }
}
