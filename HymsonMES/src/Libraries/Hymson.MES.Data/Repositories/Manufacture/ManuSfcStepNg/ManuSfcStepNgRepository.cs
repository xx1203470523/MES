/*
 *creator: Karl
 *
 *describe: 条码步骤ng信息记录表 仓储类 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-18 04:12:10
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码步骤ng信息记录表仓储
    /// </summary>
    public partial class ManuSfcStepNgRepository : BaseRepository, IManuSfcStepNgRepository
    {

        public ManuSfcStepNgRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ManuSfcStepNgEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcStepNgEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepNgEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepNgEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据BarCodeStepId批量获取数据
        /// </summary>
        /// <param name="manuSfcStepIdsNgQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepNgEntity>> GetByBarCodeStepIdsAsync(ManuSfcStepIdsNgQuery manuSfcStepIdsNgQuery)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcStepNgEntity>(GetByBarCodeStepIdsSql, manuSfcStepIdsNgQuery);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcStepNgPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepNgEntity>> GetPagedInfoAsync(ManuSfcStepNgPagedQuery manuSfcStepNgPagedQuery)
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

            var offSet = (manuSfcStepNgPagedQuery.PageIndex - 1) * manuSfcStepNgPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcStepNgPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcStepNgPagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcStepNgEntitiesTask = conn.QueryAsync<ManuSfcStepNgEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcStepNgEntities = await manuSfcStepNgEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcStepNgEntity>(manuSfcStepNgEntities, manuSfcStepNgPagedQuery.PageIndex, manuSfcStepNgPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcStepNgEntity>> GetManuSfcStepNgEntitiesAsync(ManuSfcStepNgQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcStepNgEntitiesSqlTemplate);

            if (query.StartDate != null)
            {
                sqlBuilder.Where($"CreatedOn >= '{ query.StartDate?.ToString("yyyy-MM-dd HH:mm:ss") }'");
            }
            if (query.EndDate != null)
            {
                sqlBuilder.Where($"CreatedOn <= @'{query.EndDate?.ToString("yyyy-MM-dd HH:mm:ss")}'");
            }

            sqlBuilder.AddParameters(query);

            sqlBuilder.Select("*");
            using var conn = GetMESDbConnection();
            var manuSfcStepNgEntities = await conn.QueryAsync<ManuSfcStepNgEntity>(template.RawSql, template.Parameters);
            return manuSfcStepNgEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcStepNgEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcStepNgEntity manuSfcStepNgEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcStepNgEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcStepNgEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuSfcStepNgEntity> manuSfcStepNgEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcStepNgEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcStepNgEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcStepNgEntity manuSfcStepNgEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcStepNgEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcStepNgEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuSfcStepNgEntity> manuSfcStepNgEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuSfcStepNgEntitys);
        }
        #endregion

    }

    public partial class ManuSfcStepNgRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_step_ng` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_sfc_step_ng` /**where**/ ";
        const string GetManuSfcStepNgEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc_step_ng` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc_step_ng`(  `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @BarCodeStepId, @UnqualifiedCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc_step_ng`(  `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @BarCodeStepId, @UnqualifiedCode, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `manu_sfc_step_ng` SET   BarCodeStepId = @BarCodeStepId, UnqualifiedCode = @UnqualifiedCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc_step_ng` SET   BarCodeStepId = @BarCodeStepId, UnqualifiedCode = @UnqualifiedCode, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_sfc_step_ng` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc_step_ng` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_step_ng`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_step_ng`  WHERE Id IN @Ids ";

        const string GetByBarCodeStepIdsSql = @"SELECT 
                                          `Id`, `BarCodeStepId`, `UnqualifiedCode`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `manu_sfc_step_ng`  WHERE BarCodeStepId IN @BarCodeStepIds AND SiteId = @SiteId";
        #endregion
    }
}
