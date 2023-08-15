using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 仓储（消息管理）
    /// </summary>
    public partial class InteMessageManageRepository : BaseRepository, IInteMessageManageRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteMessageManageRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteMessageManageEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<InteMessageManageEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteMessageManageEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<InteMessageManageEntity> entities)
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
        public async Task<InteMessageManageEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<InteMessageManageEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteMessageManageEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteMessageManageEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteMessageManageEntity>> GetEntitiesAsync(InteMessageManageQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteMessageManageEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteMessageManageView>> GetPagedListAsync(InteMessageManagePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("inte_event_type IET ON IET.Id = T.EventTypeId");
            sqlBuilder.LeftJoin("proc_resource PR ON PR.Id = T.ResourceId");
            sqlBuilder.Select("T.*");
            sqlBuilder.Select("IET.Name AS EventTypeName, PR.ResName AS ResourceName");
            sqlBuilder.OrderBy("T.UpdatedOn DESC");
            sqlBuilder.Where("T.IsDeleted = 0");
            sqlBuilder.Where("T.SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<InteMessageManageView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteMessageManageView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class InteMessageManageRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_message_manage T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM inte_message_manage T /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM inte_message_manage /**where**/  ";

        const string InsertSql = "INSERT INTO inte_message_manage(  `Id`, `Code`, `WorkShopId`, `LineId`, `ResourceId`, `EquipmentId`, `EventTypeId`, `EventDescription`, `Status`, `UrgencyLevel`, `DepartmentId`, `ResponsibleBy`, `ReasonAnalysis`, `HandleSolution`, `Remark`, `ReceiveDuration`, `HandleDuration`, `EvaluateOn`, `EvaluateBy`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @Code, @WorkShopId, @LineId, @ResourceId, @EquipmentId, @EventTypeId, @EventDescription, @Status, @UrgencyLevel, @DepartmentId, @ResponsibleBy, @ReasonAnalysis, @HandleSolution, @Remark, @ReceiveDuration, @HandleDuration, @EvaluateOn, @EvaluateBy, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO inte_message_manage(  `Id`, `Code`, `WorkShopId`, `LineId`, `ResourceId`, `EquipmentId`, `EventTypeId`, `EventDescription`, `Status`, `UrgencyLevel`, `DepartmentId`, `ResponsibleBy`, `ReasonAnalysis`, `HandleSolution`, `Remark`, `ReceiveDuration`, `HandleDuration`, `EvaluateOn`, `EvaluateBy`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsDeleted`) VALUES (  @Id, @Code, @WorkShopId, @LineId, @ResourceId, @EquipmentId, @EventTypeId, @EventDescription, @Status, @UrgencyLevel, @DepartmentId, @ResponsibleBy, @ReasonAnalysis, @HandleSolution, @Remark, @ReceiveDuration, @HandleDuration, @EvaluateOn, @EvaluateBy, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @SiteId, @IsDeleted) ";

        const string UpdateSql = "UPDATE inte_message_manage SET   Code = @Code, WorkShopId = @WorkShopId, LineId = @LineId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, EventTypeId = @EventTypeId, EventDescription = @EventDescription, Status = @Status, UrgencyLevel = @UrgencyLevel, DepartmentId = @DepartmentId, ResponsibleBy = @ResponsibleBy, ReasonAnalysis = @ReasonAnalysis, HandleSolution = @HandleSolution, Remark = @Remark, ReceiveDuration = @ReceiveDuration, HandleDuration = @HandleDuration, EvaluateOn = @EvaluateOn, EvaluateBy = @EvaluateBy, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE inte_message_manage SET   Code = @Code, WorkShopId = @WorkShopId, LineId = @LineId, ResourceId = @ResourceId, EquipmentId = @EquipmentId, EventTypeId = @EventTypeId, EventDescription = @EventDescription, Status = @Status, UrgencyLevel = @UrgencyLevel, DepartmentId = @DepartmentId, ResponsibleBy = @ResponsibleBy, ReasonAnalysis = @ReasonAnalysis, HandleSolution = @HandleSolution, Remark = @Remark, ReceiveDuration = @ReceiveDuration, HandleDuration = @HandleDuration, EvaluateOn = @EvaluateOn, EvaluateBy = @EvaluateBy, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, SiteId = @SiteId, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE inte_message_manage SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE inte_message_manage SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM inte_message_manage WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM inte_message_manage WHERE Id IN @Ids ";

    }
}
