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
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表仓储
    /// </summary>
    public partial class EquSparepartRecordRepository :BaseRepository, IEquSparepartRecordRepository
    {

        public EquSparepartRecordRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
            return await conn.QueryFirstOrDefaultAsync<EquSparepartRecordEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparepartRecordEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparepartRecordEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSparepartRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartRecordEntity>> GetPagedInfoAsync(EquSparepartRecordPagedQuery equSparepartRecordPagedQuery)
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
           
            var offSet = (equSparepartRecordPagedQuery.PageIndex - 1) * equSparepartRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSparepartRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSparepartRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var equSparepartRecordEntitiesTask = conn.QueryAsync<EquSparepartRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSparepartRecordEntities = await equSparepartRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparepartRecordEntity>(equSparepartRecordEntities, equSparepartRecordPagedQuery.PageIndex, equSparepartRecordPagedQuery.PageSize, totalCount);
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
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_sparepart_record` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_sparepart_record` /**where**/ ";
        const string GetEquSparepartRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_sparepart_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_sparepart_record`(  `Id`, `SparepartId`,  `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`) VALUES (   @Id, @SparepartId, @SparePartTypeId, @ProcMaterialId, @Type, @UnitId, @IsKey, @IsStandard, @Status, @BluePrintNo, @Brand, @ManagementMode, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @Code, @Name, @SupplierId, @Manufacturer, @DrawCode, @Specifications, @Position, @IsCritical, @Qty, @OperationType, @OperationQty, @EquipmentId )  ";

        const string UpdateSql = "UPDATE `equ_sparepart_record` SET   SparepartId = @SparepartId, SparePartCode = @SparePartCode, SparePartName = @SparePartName, SparePartTypeId = @SparePartTypeId, ProcMaterialId = @ProcMaterialId, Type = @Type, UnitId = @UnitId, IsKey = @IsKey, IsStandard = @IsStandard, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, Code = @Code, Name = @Name, SupplierId = @SupplierId, Manufacturer = @Manufacturer, DrawCode = @DrawCode, Specifications = @Specifications, Position = @Position, IsCritical = @IsCritical, Qty = @Qty, OperationType = @OperationType, OperationQty = @OperationQty, EquipmentId = @EquipmentId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_sparepart_record` SET   SparepartId = @SparepartId, SparePartCode = @SparePartCode, SparePartName = @SparePartName, SparePartTypeId = @SparePartTypeId, ProcMaterialId = @ProcMaterialId, Type = @Type, UnitId = @UnitId, IsKey = @IsKey, IsStandard = @IsStandard, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId, Code = @Code, Name = @Name, SupplierId = @SupplierId, Manufacturer = @Manufacturer, DrawCode = @DrawCode, Specifications = @Specifications, Position = @Position, IsCritical = @IsCritical, Qty = @Qty, OperationType = @OperationType, OperationQty = @OperationQty, EquipmentId = @EquipmentId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_sparepart_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_sparepart_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SparepartId`, `SparePartCode`, `SparePartName`, `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`
                            FROM `equ_sparepart_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SparepartId`, `SparePartCode`, `SparePartName`, `SparePartTypeId`, `ProcMaterialId`, `Type`, `UnitId`, `IsKey`, `IsStandard`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `Code`, `Name`, `SupplierId`, `Manufacturer`, `DrawCode`, `Specifications`, `Position`, `IsCritical`, `Qty`, `OperationType`, `OperationQty`, `EquipmentId`
                            FROM `equ_sparepart_record`  WHERE Id IN @Ids ";
        #endregion
    }
}
