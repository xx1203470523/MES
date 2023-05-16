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
    public partial class EquipmentHeartbeatRepository : BaseRepository, IEquipmentHeartbeatRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquipmentHeartbeatRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquipmentHeartbeatEntity entity)
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
        public async Task<int> InsertsAsync(IEnumerable<EquipmentHeartbeatEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }


        /// <summary>
        /// 新增（记录）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertRecordAsync(EquipmentHeartbeatRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertRecordSql, entity);
        }

        /// <summary>
        /// 批量新增（记录）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRecordsAsync(IEnumerable<EquipmentHeartbeatRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertRecordsSql, entities);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentHeartbeatRepository
    {
        const string InsertSql = "INSERT INTO `equ_heartbeat`(  `Id`, `SiteId`, `EquipmentId`, `LastOnLineTime`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @LastOnLineTime, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_heartbeat`(  `Id`, `SiteId`, `EquipmentId`, `LastOnLineTime`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @LastOnLineTime, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string InsertRecordSql = "INSERT INTO `equ_heartbeat_record`(  `Id`, `SiteId`, `EquipmentId`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertRecordsSql = "INSERT INTO `equ_heartbeat_record`(  `Id`, `SiteId`, `EquipmentId`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    }
}
