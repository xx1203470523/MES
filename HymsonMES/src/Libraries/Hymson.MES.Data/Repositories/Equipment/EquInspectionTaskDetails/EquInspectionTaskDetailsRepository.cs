using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（点检任务详情）
    /// </summary>
    public partial class EquInspectionTaskDetailsRepository : BaseRepository, IEquInspectionTaskDetailsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquInspectionTaskDetailsRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquInspectionTaskDetailsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquInspectionTaskDetailsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquInspectionTaskDetailsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquInspectionTaskDetailsEntity> entities)
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
        public async Task<EquInspectionTaskDetailsEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquInspectionTaskDetailsEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionTaskDetailsEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquInspectionTaskDetailsEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionTaskDetailsEntity>> GetEntitiesAsync(EquInspectionTaskDetailsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquInspectionTaskDetailsEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquInspectionTaskDetailsEntity>> GetPagedListAsync(EquInspectionTaskDetailsPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<EquInspectionTaskDetailsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquInspectionTaskDetailsEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquInspectionTaskDetailsRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_inspection_task_details /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_inspection_task_details /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_inspection_task_details /**where**/  ";

        const string InsertSql = "INSERT INTO equ_inspection_task_details(  `Id`, `InspectionTaskId`, `InspectionItemId`, `BaseValue`, `MaxValue`, `MinValue`, `Unit`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @InspectionTaskId, @InspectionItemId, @BaseValue, @MaxValue, @MinValue, @Unit, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO equ_inspection_task_details(  `Id`, `InspectionTaskId`, `InspectionItemId`, `BaseValue`, `MaxValue`, `MinValue`, `Unit`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @InspectionTaskId, @InspectionItemId, @BaseValue, @MaxValue, @MinValue, @Unit, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE equ_inspection_task_details SET   InspectionTaskId = @InspectionTaskId, InspectionItemId = @InspectionItemId, BaseValue = @BaseValue, MaxValue = @MaxValue, MinValue = @MinValue, Unit = @Unit, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_inspection_task_details SET   InspectionTaskId = @InspectionTaskId, InspectionItemId = @InspectionItemId, BaseValue = @BaseValue, MaxValue = @MaxValue, MinValue = @MinValue, Unit = @Unit, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_inspection_task_details SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_inspection_task_details SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_inspection_task_details WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_inspection_task_details WHERE Id IN @Ids ";

    }
}
