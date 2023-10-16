/*
 *creator: Karl
 *
 *describe: 生产过站面板 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 生产过站面板仓储
    /// </summary>
    public partial class ManuFacePlateProductionRepository : BaseRepository, IManuFacePlateProductionRepository
    {

        public ManuFacePlateProductionRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuFacePlateProductionEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateProductionEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据FacePlateId获取数据
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateProductionEntity> GetByFacePlateIdAsync(long facePlateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateProductionEntity>(GetByFacePlateIdSql, new { FacePlateId = facePlateId });
        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateProductionEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateProductionEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateProductionPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateProductionEntity>> GetPagedInfoAsync(ManuFacePlateProductionPagedQuery manuFacePlateProductionPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
           
            var offSet = (manuFacePlateProductionPagedQuery.PageIndex - 1) * manuFacePlateProductionPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuFacePlateProductionPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuFacePlateProductionPagedQuery);

            using var conn = GetMESDbConnection();
            var manuFacePlateProductionEntitiesTask = conn.QueryAsync<ManuFacePlateProductionEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuFacePlateProductionEntities = await manuFacePlateProductionEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFacePlateProductionEntity>(manuFacePlateProductionEntities, manuFacePlateProductionPagedQuery.PageIndex, manuFacePlateProductionPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuFacePlateProductionQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateProductionEntity>> GetManuFacePlateProductionEntitiesAsync(ManuFacePlateProductionQuery manuFacePlateProductionQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuFacePlateProductionEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuFacePlateProductionEntities = await conn.QueryAsync<ManuFacePlateProductionEntity>(template.RawSql, manuFacePlateProductionQuery);
            return manuFacePlateProductionEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFacePlateProductionEntity manuFacePlateProductionEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuFacePlateProductionEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateProductionEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuFacePlateProductionEntity> manuFacePlateProductionEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuFacePlateProductionEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFacePlateProductionEntity manuFacePlateProductionEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuFacePlateProductionEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateByFacePlateIdAsync(ManuFacePlateProductionEntity manuFacePlateProductionEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateByFacePlateIdSql, manuFacePlateProductionEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuFacePlateProductionEntity> manuFacePlateProductionEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuFacePlateProductionEntitys);
        }
        #endregion

    }

    public partial class ManuFacePlateProductionRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_face_plate_production` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_face_plate_production` /**where**/ ";
        const string GetManuFacePlateProductionEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_face_plate_production` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_face_plate_production`(  `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,`ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsShowBindWorkOrder`, `IsShowQualifiedQty`, `QualifiedColour`, `IsShowUnqualifiedQty`, `UnqualifiedColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, BarcodeType) VALUES (   @Id, @SiteId, @FacePlateId, @ResourceId, @IsResourceEdit, @ProcedureId, @IsProcedureEdit, @ScanJobId, @IsSuccessBeep, @SuccessBeepUrl, @SuccessBeepTime, @IsErrorBeep, @ErrorBeepUrl, @ErrorBeepTime, @IsShowBindWorkOrder, @IsShowQualifiedQty, @QualifiedColour, @IsShowUnqualifiedQty, @UnqualifiedColour, @IsShowLog, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @BarcodeType )  ";
        const string InsertsSql = "INSERT INTO `manu_face_plate_production`(  `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,`ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsShowBindWorkOrder`, `IsShowQualifiedQty`, `QualifiedColour`, `IsShowUnqualifiedQty`, `UnqualifiedColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, BarcodeType) VALUES (   @Id, @SiteId, @FacePlateId, @ResourceId, @IsResourceEdit, @ProcedureId, @IsProcedureEdit, @ScanJobId, @IsSuccessBeep, @SuccessBeepUrl, @SuccessBeepTime, @IsErrorBeep, @ErrorBeepUrl, @ErrorBeepTime, @IsShowBindWorkOrder, @IsShowQualifiedQty, @QualifiedColour, @IsShowUnqualifiedQty, @UnqualifiedColour, @IsShowLog, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @BarcodeType )  ";

        const string UpdateSql = "UPDATE `manu_face_plate_production` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit,ScanJobId = @ScanJobId, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit, IsSuccessBeep = @IsSuccessBeep, SuccessBeepUrl = @SuccessBeepUrl, SuccessBeepTime = @SuccessBeepTime, IsErrorBeep = @IsErrorBeep, ErrorBeepUrl = @ErrorBeepUrl, ErrorBeepTime = @ErrorBeepTime, IsShowBindWorkOrder = @IsShowBindWorkOrder, IsShowQualifiedQty = @IsShowQualifiedQty, QualifiedColour = @QualifiedColour, IsShowUnqualifiedQty = @IsShowUnqualifiedQty, UnqualifiedColour = @UnqualifiedColour, IsShowLog = @IsShowLog, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, BarcodeType=@BarcodeType  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_face_plate_production` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit,ScanJobId = @ScanJobId, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit, IsSuccessBeep = @IsSuccessBeep, SuccessBeepUrl = @SuccessBeepUrl, SuccessBeepTime = @SuccessBeepTime, IsErrorBeep = @IsErrorBeep, ErrorBeepUrl = @ErrorBeepUrl, ErrorBeepTime = @ErrorBeepTime, IsShowBindWorkOrder = @IsShowBindWorkOrder, IsShowQualifiedQty = @IsShowQualifiedQty, QualifiedColour = @QualifiedColour, IsShowUnqualifiedQty = @IsShowUnqualifiedQty, UnqualifiedColour = @UnqualifiedColour, IsShowLog = @IsShowLog, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted , BarcodeType=@BarcodeType  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_face_plate_production` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_face_plate_production` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`,`ScanJobId`, `ProcedureId`, `IsProcedureEdit`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsShowBindWorkOrder`, `IsShowQualifiedQty`, `QualifiedColour`, `IsShowUnqualifiedQty`, `UnqualifiedColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,BarcodeType
                            FROM `manu_face_plate_production`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,`ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsShowBindWorkOrder`, `IsShowQualifiedQty`, `QualifiedColour`, `IsShowUnqualifiedQty`, `UnqualifiedColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,BarcodeType
                            FROM `manu_face_plate_production`  WHERE Id IN @Ids ";

        const string GetByFacePlateIdSql = @"SELECT 
                               `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,`ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsShowBindWorkOrder`, `IsShowQualifiedQty`, `QualifiedColour`, `IsShowUnqualifiedQty`, `UnqualifiedColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`,BarcodeType
                            FROM `manu_face_plate_production`  WHERE FacePlateId = @FacePlateId ";

        const string UpdateByFacePlateIdSql = "UPDATE `manu_face_plate_production` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ScanJobId = @ScanJobId, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit, IsSuccessBeep = @IsSuccessBeep, SuccessBeepUrl = @SuccessBeepUrl, SuccessBeepTime = @SuccessBeepTime, IsErrorBeep = @IsErrorBeep, ErrorBeepUrl = @ErrorBeepUrl, ErrorBeepTime = @ErrorBeepTime, IsShowBindWorkOrder = @IsShowBindWorkOrder, IsShowQualifiedQty = @IsShowQualifiedQty, QualifiedColour = @QualifiedColour, IsShowUnqualifiedQty = @IsShowUnqualifiedQty, UnqualifiedColour = @UnqualifiedColour, IsShowLog = @IsShowLog, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted,BarcodeType=@BarcodeType WHERE FacePlateId = @FacePlateId ";
        #endregion
    }
}
