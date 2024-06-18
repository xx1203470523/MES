using Dapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备状态仓储
    /// </summary>
    public partial class EquStatusRepository : BaseRepository, IEquStatusRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquStatusRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquStatusEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<EquStatusEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 新增（统计）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertStatisticsAsync(EquStatusStatisticsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertStatisticsSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquStatusEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<EquStatusEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 根据设备ID获取最新的状态记录
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<EquStatusEntity> GetLastEntityByEquipmentIdAsync(long equipmentId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquStatusEntity>(GetLastEntityByEquipmentIdSql, new { equipmentId });
        }


        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equStatusQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquStatusStatisticsEntity>> GetEquStatusStatisticsEntitiesAsync(EquStatusStatisticsQuery equStatusQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquStatusStatisticsEntitiesSqlTemplate);
            sqlBuilder.Where("ess.IsDeleted = 0");
            sqlBuilder.Where("ess.SiteId = @SiteId");
            sqlBuilder.Select("ess.*");
            sqlBuilder.LeftJoin("equ_equipment ee ON ess.EquipmentId=ee.Id");
            if (equStatusQuery.EquipmentIds != null && equStatusQuery.EquipmentIds.Length > 0)
            {
                sqlBuilder.Where("ess.EquipmentId IN @EquipmentIds");
            }
            if (equStatusQuery.StartTime.HasValue)
            {
                sqlBuilder.Where(" ess.CreatedOn >= @StartTime");
            }
            if (equStatusQuery.EndTime.HasValue)
            {
                sqlBuilder.Where(" ess.CreatedOn < @EndTime");
            }
            if (equStatusQuery.EndTime.HasValue)
            {
                sqlBuilder.Where(" ee.WorkCenterLineId < @WorkCenterId");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquStatusStatisticsEntity>(template.RawSql, equStatusQuery);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquStatusRepository
    {
        const string GetEquStatusStatisticsEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_status_statistics` ess  /**innerjoin**/ /**leftjoin**/ /**where**/  ";
        const string InsertSql = "REPLACE INTO `equ_status`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `LossRemark`, `BeginTime`, `EndTime`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @LossRemark, @BeginTime, @EndTime, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `equ_status`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `LossRemark`, `BeginTime`, `EndTime`, `LocalTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @LossRemark, @BeginTime, @EndTime, @LocalTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertStatisticsSql = "INSERT INTO `equ_status_statistics`(  `Id`, `SiteId`, `EquipmentId`, `EquipmentStatus`, `SwitchEquipmentStatus`, `BeginTime`, `EndTime`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @EquipmentStatus, @SwitchEquipmentStatus, @BeginTime, @EndTime, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_status` SET SiteId = @SiteId, EquipmentId = @EquipmentId, EquipmentStatus = @EquipmentStatus, LossRemark = @LossRemark, BeginTime = @BeginTime, EndTime = @EndTime, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_status` SET SiteId = @SiteId, EquipmentId = @EquipmentId, EquipmentStatus = @EquipmentStatus, LossRemark = @LossRemark, BeginTime = @BeginTime, EndTime = @EndTime, LocalTime = @LocalTime, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string GetLastEntityByEquipmentIdSql = "SELECT * FROM equ_status WHERE EquipmentId = @equipmentId ORDER BY LocalTime DESC LIMIT 1";
    }
}
