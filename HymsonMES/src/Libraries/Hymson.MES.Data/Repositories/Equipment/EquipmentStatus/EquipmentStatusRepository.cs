using Dapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备状态仓储
    /// </summary>
    public partial class EquipmentStatusRepository : BaseRepository, IEquipmentStatusRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquipmentStatusRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquipmentStatusEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<EquipmentStatusEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 新增（统计）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertStatisticsAsync(EquipmentStatusStatisticsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertStatisticsSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquipmentStatusEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<EquipmentStatusEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 根据设备ID获取最新的状态记录
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<EquipmentStatusEntity> GetLastEntityByEquipmentIdAsync(long equipmentId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquipmentStatusEntity>(GetLastEntityByEquipmentIdSql, new { equipmentId });
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentStatusRepository
    {
        const string InsertSql = "REPLACE INTO `equ_status`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `LossRemark`, `BeginTime`, `EndTime`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @LossRemark, @BeginTime, @EndTime, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_status`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `LossRemark`, `BeginTime`, `EndTime`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @LossRemark, @BeginTime, @EndTime, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertStatisticsSql = "INSERT INTO `equ_status_statistics`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `SwitchEquipmentStatus`, `BeginTime`, `EndTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @SwitchEquipmentStatus, @BeginTime, @EndTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_status` SET SiteId = @SiteId, EquipmentId = @EquipmentId, EquipmentStatus = @EquipmentStatus, LossRemark = @LossRemark, BeginTime = @BeginTime, EndTime = @EndTime, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_status` SET SiteId = @SiteId, EquipmentId = @EquipmentId, EquipmentStatus = @EquipmentStatus, LossRemark = @LossRemark, BeginTime = @BeginTime, EndTime = @EndTime, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string GetLastEntityByEquipmentIdSql = "SELECT * FROM equ_status WHERE EquipmentId = @equipmentId ORDER BY LocalTime DESC LIMIT 1";
    }
}
