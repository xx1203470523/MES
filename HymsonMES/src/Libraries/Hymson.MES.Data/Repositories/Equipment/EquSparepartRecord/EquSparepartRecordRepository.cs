/*
 *creator: Karl
 *
 *describe: 设备备件记录表 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:29:55
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSparepartRecord;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquRepairOrder;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表仓储
    /// </summary>
    public partial class EquSparepartRecordRepository : BaseRepository, IEquSparepartRecordRepository
    {

        public EquSparepartRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquSparepartRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparepartRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparepartRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparepartRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartRecordPagedView>> GetPagedInfoAsync(EquSparepartRecordPagedQuery equSparepartRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("esr.IsDeleted=0");
            sqlBuilder.Where("esr.SiteId=@SiteId");
            sqlBuilder.Select("DISTINCT esr.Code,esr.Name,est.SparePartTypeCode,est.SparePartTypeName,esr.Qty,est.EquipmentId,ee.EquipmentCode,ee.EquipmentName,esr.Position,esr.OperationType,esr.CreatedOn,esr.UpdatedBy,iwc.Code AS WorkCenterCode,esr.WorkCenterCode AS RecordWorkCenterCode,esr.Recipients");
            sqlBuilder.OrderBy("esr.CreatedOn DESC");

            sqlBuilder.LeftJoin(" equ_equipment ee ON ee.Id=esr.EquipmentId");
            sqlBuilder.LeftJoin(" equ_sparepart_type est ON est.Id=esr.SparePartTypeId");
            sqlBuilder.LeftJoin(" proc_resource_equipment_bind preb ON preb.EquipmentId=esr.EquipmentId");
            sqlBuilder.LeftJoin(" inte_work_center_resource_relation iwcrr ON iwcrr.ResourceId=preb.ResourceId");
            sqlBuilder.LeftJoin(" inte_work_center iwc ON iwc.Id=iwcrr.WorkCenterId");

            if (!string.IsNullOrWhiteSpace(equSparepartRecordPagedQuery.Code))
            {
                equSparepartRecordPagedQuery.Code = $"%{equSparepartRecordPagedQuery.Code}%";
                sqlBuilder.Where("esr.Code=@Code");
            }

            if (!string.IsNullOrWhiteSpace(equSparepartRecordPagedQuery.Name))
            {
                equSparepartRecordPagedQuery.Name = $"%{equSparepartRecordPagedQuery.Name}%";
                sqlBuilder.Where("esr.Name=@Name");
            }

            if (!string.IsNullOrWhiteSpace(equSparepartRecordPagedQuery.EquipmentCode))
            {
                equSparepartRecordPagedQuery.EquipmentCode = $"%{equSparepartRecordPagedQuery.EquipmentCode}%";
                sqlBuilder.Where("ee.EquipmentCode=@EquipmentCode");
            }

            if (!string.IsNullOrWhiteSpace(equSparepartRecordPagedQuery.EquipmentName))
            {
                equSparepartRecordPagedQuery.EquipmentName = $"%{equSparepartRecordPagedQuery.EquipmentName}%";
                sqlBuilder.Where("ee.EquipmentName=@EquipmentName");
            }

            if (equSparepartRecordPagedQuery.OperationType.HasValue)
            {
                sqlBuilder.Where("esr.OperationType=@OperationType");
            }

            if (equSparepartRecordPagedQuery.CreatedOn != null && equSparepartRecordPagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equSparepartRecordPagedQuery.CreatedOn[0], EndTime = equSparepartRecordPagedQuery.CreatedOn[1].AddDays(1) });
                sqlBuilder.Where("esr.CreatedOn >= @StartTime AND esr.CreatedOn <= @EndTime");
            }

            var offSet = (equSparepartRecordPagedQuery.PageIndex - 1) * equSparepartRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSparepartRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSparepartRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var equSparepartRecordEntitiesTask = conn.QueryAsync<EquSparepartRecordPagedView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSparepartRecordEntities = await equSparepartRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparepartRecordPagedView>(equSparepartRecordEntities, equSparepartRecordPagedQuery.PageIndex, equSparepartRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSparepartRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartRecordEntity>> GetEquSparepartRecordEntitiesAsync(EquSparepartRecordQuery equSparepartRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparepartRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equSparepartRecordEntities = await conn.QueryAsync<EquSparepartRecordEntity>(template.RawSql, equSparepartRecordQuery);
            return equSparepartRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSparepartRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSparepartRecordEntity equSparepartRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSparepartRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSparepartRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSparepartRecordEntity> equSparepartRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSparepartRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSparepartRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparepartRecordEntity equSparepartRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSparepartRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSparepartRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSparepartRecordEntity> equSparepartRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSparepartRecordEntitys);
        }
        #endregion

    }

    public partial class EquSparepartRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_sparepart_record` esr /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/  LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_sparepart_record` esr /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetEquSparepartRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_sparepart_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_sparepart_record`(  `Id`, `SparepartId`, `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`, `WorkCenterId`, `WorkCenterCode`, `Recipients`) VALUES (   @Id, @SparepartId, @SparePartTypeId, @ProcMaterialId, @Type, @UnitId, @IsKey, @IsStandard, @Status, @BluePrintNo, @Brand, @ManagementMode, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @Code, @Name, @SupplierId, @Manufacturer, @DrawCode, @Specifications, @Position, @IsCritical, @Qty, @OperationType, @OperationQty, @EquipmentId, @WorkCenterId, @WorkCenterCode, @Recipients )  ";
        const string InsertsSql = "INSERT INTO `equ_sparepart_record`(  `Id`, `SparepartId`, `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`, `WorkCenterId`, `WorkCenterCode`, `Recipients`) VALUES (   @Id, @SparepartId, @SparePartTypeId, @ProcMaterialId, @Type, @UnitId, @IsKey, @IsStandard, @Status, @BluePrintNo, @Brand, @ManagementMode, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @Code, @Name, @SupplierId, @Manufacturer, @DrawCode, @Specifications, @Position, @IsCritical, @Qty, @OperationType, @OperationQty, @EquipmentId, @WorkCenterId, @WorkCenterCode, @Recipients )  ";

        const string UpdateSql = "UPDATE `equ_sparepart_record` SET   SparepartId = @SparepartId, SparePartTypeId = @SparePartTypeId, ProcMaterialId = @ProcMaterialId, Type = @Type, UnitId = @UnitId, IsKey = @IsKey, IsStandard = @IsStandard, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, Code = @Code, Name = @Name, SupplierId = @SupplierId, Manufacturer = @Manufacturer, DrawCode = @DrawCode, Specifications = @Specifications, Position = @Position, IsCritical = @IsCritical, Qty = @Qty, OperationType = @OperationType, OperationQty = @OperationQty, EquipmentId = @EquipmentId, WorkCenterId = @WorkCenterId, WorkCenterCode = @WorkCenterCode, Recipients = @Recipients  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_sparepart_record` SET   SparepartId = @SparepartId, SparePartTypeId = @SparePartTypeId, ProcMaterialId = @ProcMaterialId, Type = @Type, UnitId = @UnitId, IsKey = @IsKey, IsStandard = @IsStandard, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, Code = @Code, Name = @Name, SupplierId = @SupplierId, Manufacturer = @Manufacturer, DrawCode = @DrawCode, Specifications = @Specifications, Position = @Position, IsCritical = @IsCritical, Qty = @Qty, OperationType = @OperationType, OperationQty = @OperationQty, EquipmentId = @EquipmentId, WorkCenterId = @WorkCenterId, WorkCenterCode = @WorkCenterCode, Recipients = @Recipients  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_sparepart_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_sparepart_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SparepartId`, `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`, `WorkCenterId`, `WorkCenterCode`, `Recipients`
                            FROM `equ_sparepart_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SparepartId`, `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`, `WorkCenterId`, `WorkCenterCode`, `Recipients`
                            FROM `equ_sparepart_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
