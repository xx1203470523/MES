/*
 *creator: Karl
 *
 *describe: 在制品维修仓储仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:44:26
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuFacePlateRepair.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 在制品维修仓储
    /// </summary>
    public partial class ManuFacePlateRepairRepository :BaseRepository, IManuFacePlateRepairRepository
    {

        public ManuFacePlateRepairRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuFacePlateRepairEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateRepairEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据FacePlateId获取数据
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateRepairEntity> GetByFacePlateIdAsync(long facePlateId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuFacePlateRepairEntity>(GetByFacePlateIdSql, new { FacePlateId = facePlateId });
        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateRepairEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuFacePlateRepairEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateRepairPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateRepairEntity>> GetPagedInfoAsync(ManuFacePlateRepairPagedQuery manuFacePlateRepairPagedQuery)
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
           
            var offSet = (manuFacePlateRepairPagedQuery.PageIndex - 1) * manuFacePlateRepairPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuFacePlateRepairPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuFacePlateRepairPagedQuery);

            using var conn = GetMESDbConnection();
            var manuFacePlateRepairEntitiesTask = conn.QueryAsync<ManuFacePlateRepairEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuFacePlateRepairEntities = await manuFacePlateRepairEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuFacePlateRepairEntity>(manuFacePlateRepairEntities, manuFacePlateRepairPagedQuery.PageIndex, manuFacePlateRepairPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuFacePlateRepairQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuFacePlateRepairEntity>> GetManuFacePlateRepairEntitiesAsync(ManuFacePlateRepairQuery manuFacePlateRepairQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuFacePlateRepairEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuFacePlateRepairEntities = await conn.QueryAsync<ManuFacePlateRepairEntity>(template.RawSql, manuFacePlateRepairQuery);
            return manuFacePlateRepairEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuFacePlateRepairEntity manuFacePlateRepairEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateRepairEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuFacePlateRepairEntity> manuFacePlateRepairEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuFacePlateRepairEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuFacePlateRepairEntity manuFacePlateRepairEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateRepairEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateByFacePlateRepairIdAsync(ManuFacePlateRepairEntity manuFacePlateRepairEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateByFacePlateIdSql, manuFacePlateRepairEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuFacePlateRepairEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuFacePlateRepairEntity> manuFacePlateRepairEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuFacePlateRepairEntitys);
        }
        #endregion

    }

    public partial class ManuFacePlateRepairRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_face_plate_Repair` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_face_plate_Repair` /**where**/ ";
        const string GetManuFacePlateRepairEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_face_plate_Repair` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_face_plate_Repair`(  `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,  `IsShowProductList`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FacePlateId, @ResourceId, @IsResourceEdit, @ProcedureId, @IsProcedureEdit,@IsShowProductList,  @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_face_plate_Repair`(  `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,  `IsShowProductList`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @FacePlateId, @ResourceId, @IsResourceEdit, @ProcedureId, @IsProcedureEdit,@IsShowProductList,  @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_face_plate_Repair` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit,  IsShowProductList = @IsShowProductList, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_face_plate_Repair` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit,  IsShowProductList = @IsShowProductList, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_face_plate_Repair` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_face_plate_Repair` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,  `IsShowProductList`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_Repair`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,  `IsShowProductList`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_Repair`  WHERE Id IN @Ids ";

        const string GetByFacePlateIdSql = @"SELECT 
                               `Id`, `SiteId`, `FacePlateId`, `ResourceId`, `IsResourceEdit`, `ProcedureId`, `IsProcedureEdit`,  `IsShowProductList`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_face_plate_Repair`  WHERE FacePlateId = @FacePlateId ";

        const string UpdateByFacePlateIdSql = "UPDATE `manu_face_plate_Repair` SET   SiteId = @SiteId, FacePlateId = @FacePlateId, ResourceId = @ResourceId, IsResourceEdit = @IsResourceEdit, ProcedureId = @ProcedureId, IsProcedureEdit = @IsProcedureEdit,IsShowProductList = @IsShowProductList, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE FacePlateId = @FacePlateId ";
        #endregion
    }
}