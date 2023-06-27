using Dapper;
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

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(stringBuilder.ToString(), entity);
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

    }
}
