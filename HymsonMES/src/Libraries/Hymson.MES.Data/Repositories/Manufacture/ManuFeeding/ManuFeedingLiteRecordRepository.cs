using Dapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuFeeding
{
    /// <summary>
    /// 上卸料记录表（轻量）仓储
    /// </summary>
    public partial class ManuFeedingLiteRecordRepository : BaseRepository, IManuFeedingLiteRecordRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuFeedingLiteRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFeedingLiteRecordEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<ManuFeedingLiteRecordEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuFeedingLiteRecordRepository
    {
        const string InsertSql = "INSERT INTO `manu_feeding_lite_record`(  `Id`, `SiteId`, `EquipmentId`, `ResourceId`, `LocalTime`, `BarCode`, `DirectionType`, `Qty`, `UnloadingType`, `IsFeedingPoint`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @ResourceId, @LocalTime, @BarCode, @DirectionType, @Qty, @UnloadingType, @IsFeedingPoint, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_feeding_lite_record`(  `Id`, `SiteId`, `EquipmentId`, `ResourceId`, `LocalTime`, `BarCode`, `DirectionType`, `Qty`, `UnloadingType`, `IsFeedingPoint`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @ResourceId, @LocalTime, @BarCode, @DirectionType, @Qty, @UnloadingType, @IsFeedingPoint, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

    }
}
