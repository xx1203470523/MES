using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquAlarm.View;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备报警信息仓储
    /// </summary>
    public partial class EquAlarmRepository : BaseRepository, IEquAlarmRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquAlarmRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquAlarmEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<EquAlarmEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 根据查询条件获取设备告警报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquAlarmReportView>> GetEquAlarmReportPageListAsync(EquAlarmReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoEquAlarmReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoEquAlarmReportCountSqlTemplate);

            sqlBuilder.Where(" ea.IsDeleted = 0 ");
            sqlBuilder.Where(" ee.EquipmentCode is not null ");//过滤未关联设备信息
            sqlBuilder.Where(" ea.SiteId = @SiteId ");
            sqlBuilder.OrderBy(" ea.CreatedOn desc");

            if (!string.IsNullOrEmpty(pageQuery.EquipmentCode))
            {
                pageQuery.EquipmentCode = $"%{pageQuery.EquipmentCode}%";
                sqlBuilder.Where(" ee.EquipmentCode like @EquipmentCode ");
            }
            if (!string.IsNullOrEmpty(pageQuery.EquipmentName))
            {
                pageQuery.EquipmentName = $"%{pageQuery.EquipmentName}%";
                sqlBuilder.Where(" ee.EquipmentName like @EquipmentName ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ProcedureName))
            {
                pageQuery.ProcedureName = $"%{pageQuery.ProcedureName}%";
                sqlBuilder.Where(" pp.`Name` like @ProcedureName ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ProcedureCode))
            {
                pageQuery.ProcedureCode = $"%{pageQuery.ProcedureCode}%";
                sqlBuilder.Where(" pp.`Code` like @ProcedureCode ");
            }
            if (pageQuery.ProcedureCodes != null && pageQuery.ProcedureCodes.Length > 0)
            {
                sqlBuilder.Where(" pp.`Code` in @ProcedureCodes ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ResCode))
            {
                pageQuery.ResCode = $"%{pageQuery.ResCode}%";
                sqlBuilder.Where(" pr.ResCode like @ResCode ");
            }
            if (!string.IsNullOrEmpty(pageQuery.ResName))
            {
                pageQuery.ResName = $"%{pageQuery.ResName}%";
                sqlBuilder.Where(" pr.ResName like @ResName ");
            }
            if (pageQuery.Status.HasValue)
            {
                sqlBuilder.Where(" ea.`Status` = @Status ");
            }
            if (pageQuery.TriggerTimes != null && pageQuery.TriggerTimes.Length >= 2)
            {
                sqlBuilder.AddParameters(new { TriggerTimeStart = pageQuery.TriggerTimes[0], TriggerTimeEnd = pageQuery.TriggerTimes[1] });
                sqlBuilder.Where(" ea.LocalTime >= @TriggerTimeStart AND ea.LocalTime < @TriggerTimeEnd ");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var reportDataTask = conn.QueryAsync<EquAlarmReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var reportData = await reportDataTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquAlarmReportView>(reportData, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquAlarmRepository
    {
        const string InsertSql = "INSERT INTO `equ_alarm`(  `Id`, `SiteId`, `EquipmentId`, `FaultCode`, `AlarmMsg`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @FaultCode, @AlarmMsg, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_alarm`(  `Id`, `SiteId`, `EquipmentId`, `FaultCode`, `AlarmMsg`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @FaultCode, @AlarmMsg, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string GetPagedInfoEquAlarmReportDataSqlTemplate = @" select  ea.EquipmentId,ee.EquipmentCode,ee.EquipmentName,ea.FaultCode,ea.AlarmMsg,ea.`Status`,ea.`LocalTime`
										,ee.WorkCenterLineId,pr.ResCode,pr.ResName,pp.`Code` as ProcedureCode,pp.`Name` as ProcedureName,
										ea.UpdatedOn,ea.UpdatedBy,ea.CreatedOn,ea.CreatedBy 
										FROM equ_alarm  ea 
										left join equ_equipment ee on ee.Id=ea.EquipmentId and ee.IsDeleted=ea.IsDeleted
										left join proc_resource_equipment_bind preb on preb.EquipmentId=ea.EquipmentId  and preb.IsDeleted=ea.IsDeleted
										left join proc_resource  pr on pr.Id = preb.ResourceId and pr.SiteId= preb.SiteId and pr.IsDeleted=ea.IsDeleted
								        left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=ea.IsDeleted
										/**where**/  /**orderby**/ LIMIT @Offset,@Rows ";

        const string GetPagedInfoEquAlarmReportCountSqlTemplate = @"select COUNT(1) 
                                        FROM equ_alarm  ea 
										left join equ_equipment ee on ee.Id=ea.EquipmentId and ee.IsDeleted=ea.IsDeleted
										left join proc_resource_equipment_bind preb on preb.EquipmentId=ea.EquipmentId  and preb.IsDeleted=ea.IsDeleted
										left join proc_resource  pr on pr.Id = preb.ResourceId and pr.SiteId= preb.SiteId and pr.IsDeleted=ea.IsDeleted
								        left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=ea.IsDeleted
										/**where**/  ";

    }
}
