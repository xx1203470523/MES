using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

 
using Hymson.MES.Core.Domain.Plan; 
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Plan.Query;
 

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 仓储（班制）
    /// </summary>
    public partial class PlanShiftRepository : BaseRepository, IPlanShiftRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public PlanShiftRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanShiftEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 插入明细数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertDetailAsync(List<PlanShiftDetailEntity> entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertDetailSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<PlanShiftEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanShiftEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<PlanShiftEntity> entities)
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
        /// 依主表ID删除详细表数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesDetailByIdAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesDetailByIdSql, new { Ids = ids });
        }

        /// <summary>
        /// 硬删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesByIdAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesByIdSql, new { Ids = ids });
        }        

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanShiftEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanShiftEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanShiftEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanShiftEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanShiftEntity>> GetEntitiesAsync(PlanShiftQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                sqlBuilder.Where(" Code = @Code ");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanShiftEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanShiftEntity>> GetAllAsync(PlanShiftQuery query)
        {
            var sqlBuilder = new SqlBuilder(); 
            sqlBuilder.Select("*");             

            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("Status = 1");

            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.OrderBy("Id desc");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanShiftEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 根据主Id获取详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanShiftDetailEntity>> GetByMainIdAsync(long id)
        {
            var sqlBuilder = new SqlBuilder();

            sqlBuilder.Where("IsDeleted = 0");

            var template = sqlBuilder.AddTemplate(GetByMainIdSql);

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanShiftDetailEntity>(template.RawSql, new { Id = id });
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

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanShiftEntity>> GetPagedListAsync(PlanShiftPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (pagedQuery.Code != null && pagedQuery.Code.Any())
            {
                sqlBuilder.Where("Code = @Code");
            }

            if (pagedQuery.Name != null && pagedQuery.Name.Any())
            {
                sqlBuilder.Where("Name = @Name");
            }

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status = @Status ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<PlanShiftEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanShiftEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }



    }


    /// <summary>
    /// 
    /// </summary>
    public partial class PlanShiftRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM plan_shift /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM plan_shift /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM plan_shift /**where**/ /**orderby**/ ";

        const string InsertSql = "INSERT INTO plan_shift(  `Id`, `SiteId`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO plan_shift(  `Id`, `SiteId`, `Code`, `Name`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string InsertDetailSql = @"INSERT INTO plan_shift_detail
                                        (`Id`, `ShfitId`, `ShiftType`, `StartTime`, `EndTime`, `IsDaySpan`, `IsOverTime`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`)
                                        VALUES(@Id, @ShfitId, @ShiftType, @StartTime, @EndTime, @IsDaySpan, @IsOverTime, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted)";

        const string UpdateSql = "UPDATE plan_shift SET   SiteId = @SiteId, Code = @Code, Name = @Name, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE plan_shift SET   SiteId = @SiteId, Code = @Code, Name = @Name, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE plan_shift SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE plan_shift SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM plan_shift WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM plan_shift WHERE Id IN @Ids ";

        const string GetByMainIdSql = @"SELECT * FROM plan_shift_detail WHERE ShfitId = @Id ";

        const string DeletesDetailByIdSql = @"DELETE FROM plan_shift_detail WHERE ShfitId IN @Ids ";

        const string DeletesByIdSql = @"DELETE FROM plan_shift WHERE Id IN @Ids ";

        const string UpdateStatusSql = "UPDATE `plan_shift` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

    }
}
