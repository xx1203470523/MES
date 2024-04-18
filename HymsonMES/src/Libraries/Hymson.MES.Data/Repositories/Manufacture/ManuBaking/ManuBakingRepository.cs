/*
 *creator: Karl
 *
 *describe: 烘烤工序 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-08-02 07:32:20
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 烘烤工序仓储
    /// </summary>
    public partial class ManuBakingRepository :BaseRepository, IManuBakingRepository
    {

        public ManuBakingRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ManuBakingEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuBakingEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBakingEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuBakingEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuBakingPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuBakingEntity>> GetPagedInfoAsync(ManuBakingPagedQuery manuBakingPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

           
            var offSet = (manuBakingPagedQuery.PageIndex - 1) * manuBakingPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuBakingPagedQuery.PageSize });
            sqlBuilder.AddParameters(manuBakingPagedQuery);

            using var conn = GetMESDbConnection();
            var manuBakingEntitiesTask = conn.QueryAsync<ManuBakingEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuBakingEntities = await manuBakingEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuBakingEntity>(manuBakingEntities, manuBakingPagedQuery.PageIndex, manuBakingPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuBakingQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuBakingEntity>> GetManuBakingEntitiesAsync(ManuBakingQuery manuBakingQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuBakingEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var manuBakingEntities = await conn.QueryAsync<ManuBakingEntity>(template.RawSql, manuBakingQuery);
            return manuBakingEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuBakingEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuBakingEntity manuBakingEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuBakingEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuBakingEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ManuBakingEntity> manuBakingEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuBakingEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuBakingEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuBakingEntity manuBakingEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuBakingEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuBakingEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ManuBakingEntity> manuBakingEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, manuBakingEntitys);
        }
        #endregion

    }

    public partial class ManuBakingRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_baking` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `manu_baking` /**where**/ ";
        const string GetManuBakingEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_baking` /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_baking`(  `Id`, `SiteId`, `EquipmentId`, `SFC`, `BakingOn`, `BakingEnd`, `BakingPlan`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @SFC, @BakingOn, @BakingEnd, @BakingPlan, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_baking`(  `Id`, `SiteId`, `EquipmentId`, `SFC`, `BakingOn`, `BakingEnd`, `BakingPlan`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @EquipmentId, @SFC, @BakingOn, @BakingEnd, @BakingPlan, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_baking` SET   SiteId = @SiteId, EquipmentId = @EquipmentId, SFC = @SFC, BakingOn = @BakingOn, BakingEnd = @BakingEnd, BakingPlan = @BakingPlan, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_baking` SET   SiteId = @SiteId, EquipmentId = @EquipmentId, SFC = @SFC, BakingOn = @BakingOn, BakingEnd = @BakingEnd, BakingPlan = @BakingPlan, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `manu_baking` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_baking` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
            
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `EquipmentId`, `SFC`, `BakingOn`, `BakingEnd`, `BakingPlan`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_baking`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `EquipmentId`, `SFC`, `BakingOn`, `BakingEnd`, `BakingPlan`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `manu_baking`  WHERE Id IN @Ids ";
        #endregion
    }
}
