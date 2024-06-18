using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备心跳仓储
    /// </summary>
    public partial class EquHeartbeatRepository : BaseRepository, IEquHeartbeatRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquHeartbeatRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equHeartbeatQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquHeartbeatEntity>> GetEquHeartbeatEntitiesAsync(EquHeartbeatQuery equHeartbeatQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquHeartbeatEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");
            if (equHeartbeatQuery.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }
            if (equHeartbeatQuery.EquipmentId.HasValue)
            {
                sqlBuilder.Where("EquipmentId = @EquipmentId");
            }
            if (equHeartbeatQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (equHeartbeatQuery.LastOnLineTimeStart.HasValue)
            {
                sqlBuilder.Where(" LastOnLineTime >= @LastOnLineTimeStart");
            }
            if (equHeartbeatQuery.LastOnLineTimeEnd.HasValue)
            {
                sqlBuilder.Where(" LastOnLineTime < @LastOnLineTimeEnd");
            }
            using var conn = GetMESDbConnection();
            var equHeartbeatEntities = await conn.QueryAsync<EquHeartbeatEntity>(template.RawSql, equHeartbeatQuery);
            return equHeartbeatEntities;
        }

        /// <summary>
        /// 根据查询条件获取设备心跳状态报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquHeartbeatReportView>> GetEquHeartbeatReportPageListAsync(EquHeartbeatReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoEquHeartbeatReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoEquHeartbeatReportCountSqlTemplate);

            sqlBuilder.Where(" eh.IsDeleted = 0 ");
            sqlBuilder.Where(" ee.EquipmentCode is not null ");
            sqlBuilder.Where(" eh.SiteId = @SiteId ");

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
                sqlBuilder.Where(" eh.`Status` = @Status ");
            }
            if (pageQuery.AcquisitionTime != null && pageQuery.AcquisitionTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { AcquisitionStart = pageQuery.AcquisitionTime[0], AcquisitionEnd = pageQuery.AcquisitionTime[1].AddDays(1) });
                sqlBuilder.Where(" eh.UpdatedOn >= @AcquisitionStart AND eh.UpdatedOn < @AcquisitionEnd ");
            }
            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var reportDataTask = conn.QueryAsync<EquHeartbeatReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var reportData = await reportDataTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquHeartbeatReportView>(reportData, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquHeartbeatEntity entity)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(InsertSql);
            stringBuilder.Append(" ON DUPLICATE KEY ");
            stringBuilder.Append(" UPDATE ");

            if (entity.Status == true) stringBuilder.Append(" LastOnLineTime = @LastOnLineTime, ");

            stringBuilder.Append(" Status = @Status, ");
            stringBuilder.Append(" UpdatedBy = @UpdatedBy, ");
            stringBuilder.Append(" UpdatedOn = @UpdatedOn ");

            /*
            var sqlBuilder = new SqlBuilder();
            var sqlTemplate = sqlBuilder.AddTemplate(stringBuilder.ToString());
            return await conn.ExecuteAsync(sqlTemplate.RawSql, entity);
            */
            try
            {
                var conn = GetMESDbConnection();
                return await conn.ExecuteAsync(stringBuilder.ToString(), entity);
              
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<EquHeartbeatEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }


        /// <summary>
        /// 新增（记录）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertRecordAsync(EquHeartbeatRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertRecordSql, entity);
        }

        /// <summary>
        /// 批量新增（记录）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRecordsAsync(IEnumerable<EquHeartbeatRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertRecordsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equHeartbeatEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquHeartbeatEntity equHeartbeatEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equHeartbeatEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equHeartbeatEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<EquHeartbeatEntity> equHeartbeatEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equHeartbeatEntitys);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquHeartbeatRepository
    {
        const string GetEquHeartbeatEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_heartbeat` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_heartbeat`(  `Id`, `SiteId`, `EquipmentId`, `LastOnLineTime`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @LastOnLineTime, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_heartbeat`(  `Id`, `SiteId`, `EquipmentId`, `LastOnLineTime`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @LastOnLineTime, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string InsertRecordSql = "INSERT INTO `equ_heartbeat_record`(  `Id`, `SiteId`, `EquipmentId`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertRecordsSql = "INSERT INTO `equ_heartbeat_record`(  `Id`, `SiteId`, `EquipmentId`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_heartbeat` SET   SiteId = @SiteId, EquipmentId = @EquipmentId, LastOnLineTime = @LastOnLineTime, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_heartbeat` SET   SiteId = @SiteId, EquipmentId = @EquipmentId, LastOnLineTime = @LastOnLineTime, Status = @Status,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string GetPagedInfoEquHeartbeatReportDataSqlTemplate = @" SELECT eh.EquipmentId,ee.EquipmentCode,ee.EquipmentName,eh.`Status`,ee.WorkCenterLineId,
									pr.ResCode,pr.ResName,
								    eh.LastOnLineTime,eh.UpdatedOn,eh.UpdatedBy,eh.CreatedOn,eh.CreatedBy FROM equ_heartbeat eh 
								    left join equ_equipment ee on eh.EquipmentId=ee.Id and ee.SiteId=eh.SiteId and ee.IsDeleted=0
								    left join proc_resource_equipment_bind preb on preb.EquipmentId=ee.Id and preb.SiteId=ee.SiteId and preb.IsDeleted=0
								    left join proc_resource  pr on pr.Id = preb.ResourceId and pr.SiteId= preb.SiteId and pr.IsDeleted=0
                                    left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=0								    
                                    /**where**/  ";
        //pp.`Code` as ProcedureCode,pp.`Name` as ProcedureName,
        //left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=0

        const string GetPagedInfoEquHeartbeatReportCountSqlTemplate = @"select COUNT(1) 
                                    FROM equ_heartbeat eh 
									left join equ_equipment ee on eh.EquipmentId=ee.Id and ee.SiteId=eh.SiteId and ee.IsDeleted=0
									left join proc_resource_equipment_bind preb on preb.EquipmentId=ee.Id and preb.SiteId=ee.SiteId and preb.IsDeleted=0
									left join proc_resource  pr on pr.Id = preb.ResourceId and pr.SiteId= preb.SiteId and pr.IsDeleted=0
									left join proc_procedure pp on pp.ResourceTypeId=pr.ResTypeId  and pp.SiteId= pr.SiteId and pp.IsDeleted=0  /**where**/  ";

    }
}
