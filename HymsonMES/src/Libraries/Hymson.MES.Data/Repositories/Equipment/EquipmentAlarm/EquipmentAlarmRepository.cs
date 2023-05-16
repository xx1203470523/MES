using Dapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备报警信息仓储
    /// </summary>
    public partial class EquipmentAlarmRepository : BaseRepository, IEquipmentAlarmRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquipmentAlarmRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equipmentAlarmEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquipmentAlarmEntity equipmentAlarmEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equipmentAlarmEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equipmentAlarmEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquipmentAlarmEntity> equipmentAlarmEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equipmentAlarmEntitys);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentAlarmRepository
    {
        const string InsertSql = "INSERT INTO `equ_alarm`(  `Id`, `SiteId`, `EquipmentId`, `FaultCode`, `AlarmMsg`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @FaultCode, @AlarmMsg, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_alarm`(  `Id`, `SiteId`, `EquipmentId`, `FaultCode`, `AlarmMsg`, `Status`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @FaultCode, @AlarmMsg, @Status, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    }
}
