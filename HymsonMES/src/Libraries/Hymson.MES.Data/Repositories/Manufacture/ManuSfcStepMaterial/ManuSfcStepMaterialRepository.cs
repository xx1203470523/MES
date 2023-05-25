/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
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
    /// 出站绑定的物料批次条码仓储
    /// </summary>
    public partial class ManuSfcStepMaterialRepository :BaseRepository, IManuSfcStepMaterialRepository
    {

        public ManuSfcStepMaterialRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuSfcStepMaterialEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcStepMaterialEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepMaterialEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepMaterialEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepMaterialPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepMaterialEntity>> GetPagedInfoAsync(ManuSfcStepMaterialPagedQuery manuSfcStepMaterialPagedQuery)
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
           
            var offSet = (manuSfcStepMaterialPagedQuery.PageIndex - 1) * manuSfcStepMaterialPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcStepMaterialPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcStepMaterialPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcStepMaterialEntitiesTask = conn.QueryAsync<ManuSfcStepMaterialEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcStepMaterialEntities = await manuSfcStepMaterialEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcStepMaterialEntity>(manuSfcStepMaterialEntities, manuSfcStepMaterialPagedQuery.PageIndex, manuSfcStepMaterialPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcStepMaterialQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepMaterialEntity>> GetManuSfcStepMaterialEntitiesAsync(ManuSfcStepMaterialQuery manuSfcStepMaterialQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcStepMaterialEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuSfcStepMaterialEntities = await conn.QueryAsync<ManuSfcStepMaterialEntity>(template.RawSql, manuSfcStepMaterialQuery);
            return manuSfcStepMaterialEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcStepMaterialEntity manuSfcStepMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcStepMaterialEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcStepMaterialEntity> manuSfcStepMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcStepMaterialEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepMaterialEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcStepMaterialEntity manuSfcStepMaterialEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcStepMaterialEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcStepMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcStepMaterialEntity> manuSfcStepMaterialEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcStepMaterialEntitys);
        }
        #endregion

    }

    public partial class ManuSfcStepMaterialRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step_material` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_step_material` /**where**/ ";
        const string GetManuSfcStepMaterialEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_step_material` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_step_material`(  `Id`, `SiteId`, `StepId`, `SFC`, `MaterialBarcode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @StepId, @SFC, @MaterialBarcode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_step_material`(  `Id`, `SiteId`, `StepId`, `SFC`, `MaterialBarcode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @StepId, @SFC, @MaterialBarcode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc_step_material` SET   SiteId = @SiteId, StepId = @StepId, SFC = @SFC, MaterialBarcode = @MaterialBarcode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_step_material` SET   SiteId = @SiteId, StepId = @StepId, SFC = @SFC, MaterialBarcode = @MaterialBarcode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_step_material` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_step_material` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `StepId`, `SFC`, `MaterialBarcode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_step_material`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `StepId`, `SFC`, `MaterialBarcode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_sfc_step_material`  WHERE Id IN @Ids ";
        #endregion
    }
}
