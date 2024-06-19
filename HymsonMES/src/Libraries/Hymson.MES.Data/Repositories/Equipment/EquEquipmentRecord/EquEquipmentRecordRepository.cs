/*
 *creator: Karl
 *
 *describe: 设备台账信息 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:53:50
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquEquipmentRecord
{
    /// <summary>
    /// 设备台账信息仓储
    /// </summary>
    public partial class EquEquipmentRecordRepository :BaseRepository, IEquEquipmentRecordRepository
    {

        public EquEquipmentRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equEquipmentRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentRecordEntity>> GetPagedInfoAsync(EquEquipmentRecordPagedQuery equEquipmentRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (equEquipmentRecordPagedQuery.PageIndex - 1) * equEquipmentRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equEquipmentRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(equEquipmentRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var equEquipmentRecordEntitiesTask = conn.QueryAsync<EquEquipmentRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equEquipmentRecordEntities = await equEquipmentRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquEquipmentRecordEntity>(equEquipmentRecordEntities, equEquipmentRecordPagedQuery.PageIndex, equEquipmentRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equEquipmentRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentRecordEntity>> GetEquEquipmentRecordEntitiesAsync(EquEquipmentRecordQuery equEquipmentRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquEquipmentRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equEquipmentRecordEntities = await conn.QueryAsync<EquEquipmentRecordEntity>(template.RawSql, equEquipmentRecordQuery);
            return equEquipmentRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentRecordEntity equEquipmentRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equEquipmentRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equEquipmentRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquEquipmentRecordEntity> equEquipmentRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equEquipmentRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentRecordEntity equEquipmentRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equEquipmentRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equEquipmentRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquEquipmentRecordEntity> equEquipmentRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equEquipmentRecordEntitys);
        }
        #endregion

    }

    public partial class EquEquipmentRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_equipment_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_record` /**where**/ ";
        const string GetEquEquipmentRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_equipment_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_equipment_record`(  `Id`, `EquipmentId`, `EquipmentCode`, `EquipmentName`, `EquipmentGroupId`, `EquipmentDesc`, `WorkCenterFactoryId`, `WorkCenterShopId`, `WorkCenterLineId`, `Location`, `EquipmentType`, `UseDepartment`, `UseStatus`, `EntryDate`, `QualTime`, `ExpireDate`, `Manufacturer`, `Supplier`, `Power`, `EnergyLevel`, `Ip`, `TakeTime`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `OperationType`) VALUES (   @Id, @EquipmentId, @EquipmentCode, @EquipmentName, @EquipmentGroupId, @EquipmentDesc, @WorkCenterFactoryId, @WorkCenterShopId, @WorkCenterLineId, @Location, @EquipmentType, @UseDepartment, @UseStatus, @EntryDate, @QualTime, @ExpireDate, @Manufacturer, @Supplier, @Power, @EnergyLevel, @Ip, @TakeTime, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @OperationType )  ";
        const string InsertsSql = "INSERT INTO `equ_equipment_record`(  `Id`, `EquipmentId`, `EquipmentCode`, `EquipmentName`, `EquipmentGroupId`, `EquipmentDesc`, `WorkCenterFactoryId`, `WorkCenterShopId`, `WorkCenterLineId`, `Location`, `EquipmentType`, `UseDepartment`, `UseStatus`, `EntryDate`, `QualTime`, `ExpireDate`, `Manufacturer`, `Supplier`, `Power`, `EnergyLevel`, `Ip`, `TakeTime`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `OperationType`) VALUES (   @Id, @EquipmentId, @EquipmentCode, @EquipmentName, @EquipmentGroupId, @EquipmentDesc, @WorkCenterFactoryId, @WorkCenterShopId, @WorkCenterLineId, @Location, @EquipmentType, @UseDepartment, @UseStatus, @EntryDate, @QualTime, @ExpireDate, @Manufacturer, @Supplier, @Power, @EnergyLevel, @Ip, @TakeTime, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @OperationType )  ";

        const string UpdateSql = "UPDATE `equ_equipment_record` SET   EquipmentId = @EquipmentId, EquipmentCode = @EquipmentCode, EquipmentName = @EquipmentName, EquipmentGroupId = @EquipmentGroupId, EquipmentDesc = @EquipmentDesc, WorkCenterFactoryId = @WorkCenterFactoryId, WorkCenterShopId = @WorkCenterShopId, WorkCenterLineId = @WorkCenterLineId, Location = @Location, EquipmentType = @EquipmentType, UseDepartment = @UseDepartment, UseStatus = @UseStatus, EntryDate = @EntryDate, QualTime = @QualTime, ExpireDate = @ExpireDate, Manufacturer = @Manufacturer, Supplier = @Supplier, Power = @Power, EnergyLevel = @EnergyLevel, Ip = @Ip, TakeTime = @TakeTime, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, OperationType = @OperationType  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_equipment_record` SET   EquipmentId = @EquipmentId, EquipmentCode = @EquipmentCode, EquipmentName = @EquipmentName, EquipmentGroupId = @EquipmentGroupId, EquipmentDesc = @EquipmentDesc, WorkCenterFactoryId = @WorkCenterFactoryId, WorkCenterShopId = @WorkCenterShopId, WorkCenterLineId = @WorkCenterLineId, Location = @Location, EquipmentType = @EquipmentType, UseDepartment = @UseDepartment, UseStatus = @UseStatus, EntryDate = @EntryDate, QualTime = @QualTime, ExpireDate = @ExpireDate, Manufacturer = @Manufacturer, Supplier = @Supplier, Power = @Power, EnergyLevel = @EnergyLevel, Ip = @Ip, TakeTime = @TakeTime, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, OperationType = @OperationType  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_equipment_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_equipment_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `EquipmentId`, `EquipmentCode`, `EquipmentName`, `EquipmentGroupId`, `EquipmentDesc`, `WorkCenterFactoryId`, `WorkCenterShopId`, `WorkCenterLineId`, `Location`, `EquipmentType`, `UseDepartment`, `UseStatus`, `EntryDate`, `QualTime`, `ExpireDate`, `Manufacturer`, `Supplier`, `Power`, `EnergyLevel`, `Ip`, `TakeTime`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `OperationType`
                            FROM `equ_equipment_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `EquipmentId`, `EquipmentCode`, `EquipmentName`, `EquipmentGroupId`, `EquipmentDesc`, `WorkCenterFactoryId`, `WorkCenterShopId`, `WorkCenterLineId`, `Location`, `EquipmentType`, `UseDepartment`, `UseStatus`, `EntryDate`, `QualTime`, `ExpireDate`, `Manufacturer`, `Supplier`, `Power`, `EnergyLevel`, `Ip`, `TakeTime`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `OperationType`
                            FROM `equ_equipment_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
