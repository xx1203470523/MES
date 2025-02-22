using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Equipment.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（工具绑定设备操作记录表）
    /// </summary>
    public partial class EquToolsEquipmentBindRecordRepository : BaseRepository, IEquToolsEquipmentBindRecordRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolsEquipmentBindRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolsEquipmentBindRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquToolsEquipmentBindRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquToolsEquipmentBindRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquToolsEquipmentBindRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
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
        public async Task<EquToolsEquipmentBindRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolsEquipmentBindRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsEquipmentBindRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsEquipmentBindRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsEquipmentBindRecordEntity>> GetEntitiesAsync(EquToolsEquipmentBindRecordQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.EquipmentId.HasValue)
            {
                sqlBuilder.Where(" EquipmentId=@EquipmentId ");
            }
            if (query.ToolId.HasValue)
            {
                sqlBuilder.Where(" ToolId=@ToolId ");
            }
            if (query.OperationType.HasValue)
            {
                sqlBuilder.Where(" OperationType=@OperationType ");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsEquipmentBindRecordEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolsEquipmentBindRecordView>> GetPagedListAsync(EquToolsEquipmentBindRecordPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("es.code as ToolCode,es.`Name`as ToolName,est.`Code` as ToolType,est.`Name` as ToolTypeName,es.ToolsId as ToolTypeId,ee.EquipmentCode,ee.EquipmentName,ese.Id, ese.CreatedBy,ese.CreatedOn,ese.Position,ese.OperationType,ese.UninstallReason,ese.UninstallBy,ese.UninstallOn,es.RatedLife,ese.CurrentUsedLife");

            sqlBuilder.LeftJoin(" equ_equipment ee on ee.Id=ese.EquipmentId");
            sqlBuilder.LeftJoin(" equ_tools es on es.Id=ese.ToolId");
            sqlBuilder.LeftJoin(" equ_tools_type est on est.Id=es.ToolsId");

            sqlBuilder.OrderBy("ese.UpdatedOn DESC");
            sqlBuilder.Where("ese.IsDeleted = 0");
            sqlBuilder.Where("ese.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.ToolCode))
            {
                pagedQuery.ToolCode = $"%{pagedQuery.ToolCode}%";
                sqlBuilder.Where(" es.code like @ToolCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ToolName))
            {
                pagedQuery.ToolName = $"%{pagedQuery.ToolName}%";
                sqlBuilder.Where(" es.Name like @ToolName");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ToolType))
            {
                pagedQuery.ToolType = $"%{pagedQuery.ToolType}%";
                sqlBuilder.Where(" est.Code like @ToolType");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where(" ee.EquipmentCode like @EquipmentCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Position))
            {
                pagedQuery.Position = $"%{pagedQuery.Position}%";
                sqlBuilder.Where(" ese.Position like @Position");
            }

            if (pagedQuery.OperationType.HasValue)
            {
                sqlBuilder.Where(" ese.OperationType= @OperationType");
            }

            if (pagedQuery.InstallTimeRange != null && pagedQuery.InstallTimeRange.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = pagedQuery.InstallTimeRange[0], CreatedOnEnd = pagedQuery.InstallTimeRange[1] });
                sqlBuilder.Where(" ese.CreatedOn >= @CreatedOnStart AND ese.CreatedOn < @CreatedOnEnd");
            }

            if (pagedQuery.UninstallTimeRange != null && pagedQuery.UninstallTimeRange.Length >= 2)
            {
                sqlBuilder.AddParameters(new { UninstallTimeStart = pagedQuery.UninstallTimeRange[0], UninstallTimeEnd = pagedQuery.UninstallTimeRange[1]});
                sqlBuilder.Where(" ese.UninstallOn >= @UninstallTimeStart AND ese.UninstallOn < @UninstallTimeEnd");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquToolsEquipmentBindRecordView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolsEquipmentBindRecordView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询指定位置是否已经绑定工具
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquToolsEquipmentBindRecordEntity> GetIsPostionBindAsync(EquToolsEquipmentBindRecordQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolsEquipmentBindRecordEntity>(GetIsPostionBindSql, query);
        }
    }


    /// <summary>
    /// 仓存储sql
    /// </summary>
    public partial class EquToolsEquipmentBindRecordRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_tools_equipment__bind_record ese /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_tools_equipment__bind_record ese /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_tools_equipment__bind_record /**where**/  ";

        const string InsertSql = "INSERT INTO equ_tools_equipment__bind_record(  `Id`, `SiteId`, `ToolId`, `ToolsRecordId`, `EquipmentId`, `EquipmentRecordId`, `Position`, `OperationType`, `UninstallReason`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `UninstallBy`, `UninstallOn`) VALUES (  @Id, @SiteId, @ToolId, @ToolsRecordId, @EquipmentId, @EquipmentRecordId, @Position, @OperationType, @UninstallReason, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @UninstallBy, @UninstallOn) ";

        const string UpdateSql = "UPDATE equ_tools_equipment__bind_record SET Position=@Position,OperationType=@OperationType, UninstallReason=@UninstallReason, Remark=@Remark, UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn,UninstallBy=@UninstallBy,UninstallOn=@UninstallOn,CurrentUsedLife=@CurrentUsedLife WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_tools_equipment__bind_record SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_tools_equipment__bind_record SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_tools_equipment__bind_record WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_tools_equipment__bind_record WHERE Id IN @Ids ";
        const string GetIsPostionBindSql = "select * from equ_tools_equipment__bind_record where SiteId=@SiteId and EquipmentId=@EquipmentId and Position=@Position and OperationType=@OperationType limit 1";

    }
}
