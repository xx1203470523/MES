using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateContainerPack.Query;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    ///容器包装面板仓储
    /// </summary>
    public partial class ManuFacePlateContainerPackRepository : BaseRepository, IManuFacePlateContainerPackRepository
    {

        public ManuFacePlateContainerPackRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuFacePlateContainerPackEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateContainerPackEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据FacePlateId获取数据
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateContainerPackEntity> GetByFacePlateIdAsync(long facePlateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateContainerPackEntity>(GetByFacePlateIdSql, new { FacePlateId = facePlateId });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateContainerPackEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateContainerPackEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="ManuFacePlateContainerPackPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateContainerPackEntity>> GetPagedInfoAsync(ManuFacePlateContainerPackPagedQuery manuFacePlateContainerPackPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");

            var offSet = (manuFacePlateContainerPackPagedQuery.PageIndex - 1) * manuFacePlateContainerPackPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuFacePlateContainerPackPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuFacePlateContainerPackPagedQuery);

            using var conn = GetMESDbConnection();
            var manuFacePlateContainerPackEntitiesTask = conn.QueryAsync<ManuFacePlateContainerPackEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuFacePlateContainerPackEntities = await manuFacePlateContainerPackEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFacePlateContainerPackEntity>(manuFacePlateContainerPackEntities, manuFacePlateContainerPackPagedQuery.PageIndex, manuFacePlateContainerPackPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuFacePlateProductionQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateContainerPackEntity>> GetManuFacePlateContainerPackEntitiesAsync(ManuFacePlateContainerPackQuery manuFacePlateContainerPackQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuFacePlateContainerPackEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuFacePlateContainerPackEntities = await conn.QueryAsync<ManuFacePlateContainerPackEntity>(template.RawSql, manuFacePlateContainerPackQuery);
            return manuFacePlateContainerPackEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuFacePlateContainerPackEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuFacePlateContainerPackEntity> manuFacePlateContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuFacePlateContainerPackEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFacePlateContainerPackEntity ManuFacePlateContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, ManuFacePlateContainerPackEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateProductionEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateByFacePlateIdAsync(ManuFacePlateContainerPackEntity manuFacePlateContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateByFacePlateIdSql, manuFacePlateContainerPackEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuFacePlateContainerPackEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuFacePlateContainerPackEntity> manuFacePlateContainerPackEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuFacePlateContainerPackEntity);
        }

     
        #endregion

    }

    public partial class ManuFacePlateContainerPackRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_face_plate_container_pack` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_face_plate_container_pack` /**where**/ ";
        const string GetManuFacePlateContainerPackEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_face_plate_container_pack` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_face_plate_container_pack`( `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`, `ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsAllowDifferentMaterial`, `IsMixedWorkOrder`, `IsAllowQueueProduct`, `IsAllowCompleteProduct`, `IsAllowActiveProduct`, `IsShowMinQty`, `IsShowMaxQty`, `IsShowCurrentQty`, `QualifiedColour`, `ErrorsColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id,@SiteId,@FacePlateId,@ResourceId,@IsResourceEdit,@ProcedureId,@IsProcedureEdit,@ScanJobId,@IsSuccessBeep,@SuccessBeepUrl,@SuccessBeepTime,@IsErrorBeep,@ErrorBeepUrl,@ErrorBeepTime,@IsAllowDifferentMaterial,@IsMixedWorkOrder,@IsAllowQueueProduct,@IsAllowCompleteProduct,@IsAllowActiveProduct,@IsShowMinQty,@IsShowMaxQty,@IsShowCurrentQty,@QualifiedColour,@ErrorsColour,@IsShowLog,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_face_plate_container_pack`( `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`, `ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsAllowDifferentMaterial`, `IsMixedWorkOrder`, `IsAllowQueueProduct`, `IsAllowCompleteProduct`, `IsAllowActiveProduct`, `IsShowMinQty`, `IsShowMaxQty`, `IsShowCurrentQty`, `QualifiedColour`, `ErrorsColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id,@SiteId,@FacePlateId,@ResourceId,@IsResourceEdit,@ProcedureId,@IsProcedureEdit,@ScanJobId,@IsSuccessBeep,@SuccessBeepUrl,@SuccessBeepTime,@IsErrorBeep,@ErrorBeepUrl,@ErrorBeepTime,@IsAllowDifferentMaterial,@IsMixedWorkOrder,@IsAllowQueueProduct,@IsAllowCompleteProduct,@IsAllowActiveProduct,@IsShowMinQty,@IsShowMaxQty,@IsShowCurrentQty,@QualifiedColour,@ErrorsColour,@IsShowLog,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_face_plate_container_pack` SET  SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit,ScanJobId = @ScanJobId,IsSuccessBeep = @IsSuccessBeep, SuccessBeepUrl = @SuccessBeepUrl, SuccessBeepTime = @SuccessBeepTime, IsErrorBeep = @IsErrorBeep, ErrorBeepUrl = @ErrorBeepUrl, ErrorBeepTime = @ErrorBeepTime,IsAllowDifferentMaterial = @IsAllowDifferentMaterial, IsMixedWorkOrder = @IsMixedWorkOrder, IsAllowQueueProduct = @IsAllowQueueProduct, IsAllowCompleteProduct = @IsAllowCompleteProduct, IsAllowActiveProduct = @IsAllowActiveProduct,\r\nIsShowMinQty = @IsShowMinQty,IsShowMaxQty = @IsShowMaxQty,IsShowCurrentQty= @IsShowCurrentQty,QualifiedColour= @QualifiedColour,ErrorsColour = @ErrorsColour,\r\n IsShowLog = @IsShowLog, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_face_plate_container_pack` SET  SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit,ScanJobId = @ScanJobId,IsSuccessBeep = @IsSuccessBeep, SuccessBeepUrl = @SuccessBeepUrl, SuccessBeepTime = @SuccessBeepTime, IsErrorBeep = @IsErrorBeep, ErrorBeepUrl = @ErrorBeepUrl, ErrorBeepTime = @ErrorBeepTime,IsAllowDifferentMaterial = @IsAllowDifferentMaterial, IsMixedWorkOrder = @IsMixedWorkOrder, IsAllowQueueProduct = @IsAllowQueueProduct, IsAllowCompleteProduct = @IsAllowCompleteProduct, IsAllowActiveProduct = @IsAllowActiveProduct,\r\nIsShowMinQty = @IsShowMinQty,IsShowMaxQty = @IsShowMaxQty,IsShowCurrentQty= @IsShowCurrentQty,QualifiedColour= @QualifiedColour,ErrorsColour = @ErrorsColour,\r\n IsShowLog = @IsShowLog, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_face_plate_container_pack` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_face_plate_container_pack` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                              `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`, `ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsAllowDifferentMaterial`, `IsMixedWorkOrder`, `IsAllowQueueProduct`, `IsAllowCompleteProduct`, `IsAllowActiveProduct`, `IsShowMinQty`, `IsShowMaxQty`, `IsShowCurrentQty`, `QualifiedColour`, `ErrorsColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_container_pack`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                         `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`, `ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsAllowDifferentMaterial`, `IsMixedWorkOrder`, `IsAllowQueueProduct`, `IsAllowCompleteProduct`, `IsAllowActiveProduct`, `IsShowMinQty`, `IsShowMaxQty`, `IsShowCurrentQty`, `QualifiedColour`, `ErrorsColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_container_pack`  WHERE Id IN @Ids ";

        const string GetByFacePlateIdSql = @"SELECT 
                              `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`, `ScanJobId`, `IsSuccessBeep`, `SuccessBeepUrl`, `SuccessBeepTime`, `IsErrorBeep`, `ErrorBeepUrl`, `ErrorBeepTime`, `IsAllowDifferentMaterial`, `IsMixedWorkOrder`, `IsAllowQueueProduct`, `IsAllowCompleteProduct`, `IsAllowActiveProduct`, `IsShowMinQty`, `IsShowMaxQty`, `IsShowCurrentQty`, `QualifiedColour`, `ErrorsColour`, `IsShowLog`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_container_pack`  WHERE FacePlateId = @FacePlateId ";

        const string UpdateByFacePlateIdSql = "UPDATE `manu_face_plate_container_pack` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit,ScanJobId = @ScanJobId,IsSuccessBeep = @IsSuccessBeep, SuccessBeepUrl = @SuccessBeepUrl, SuccessBeepTime = @SuccessBeepTime, IsErrorBeep = @IsErrorBeep, ErrorBeepUrl = @ErrorBeepUrl, ErrorBeepTime = @ErrorBeepTime,IsAllowDifferentMaterial = @IsAllowDifferentMaterial, IsMixedWorkOrder = @IsMixedWorkOrder, IsAllowQueueProduct = @IsAllowQueueProduct, IsAllowCompleteProduct = @IsAllowCompleteProduct, IsAllowActiveProduct = @IsAllowActiveProduct,\r\nIsShowMinQty = @IsShowMinQty,IsShowMaxQty = @IsShowMaxQty,IsShowCurrentQty= @IsShowCurrentQty,QualifiedColour= @QualifiedColour,ErrorsColour = @ErrorsColour,\r\n IsShowLog = @IsShowLog, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE FacePlateId = @FacePlateId ";
        #endregion
    }
}
