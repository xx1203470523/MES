/*
 *creator: Karl
 *
 *describe: 设备参数组详情 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 02:08:48
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

using System.Collections;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组详情仓储
    /// </summary>
    public partial class ProcEquipmentGroupParamDetailRepository :BaseRepository, IProcEquipmentGroupParamDetailRepository
    {

        public ProcEquipmentGroupParamDetailRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcEquipmentGroupParamDetailEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcEquipmentGroupParamDetailEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamDetailEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcEquipmentGroupParamDetailEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEquipmentGroupParamDetailEntity>> GetPagedInfoAsync(ProcEquipmentGroupParamDetailPagedQuery procEquipmentGroupParamDetailPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
        
            var offSet = (procEquipmentGroupParamDetailPagedQuery.PageIndex - 1) * procEquipmentGroupParamDetailPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procEquipmentGroupParamDetailPagedQuery.PageSize });
            sqlBuilder.AddParameters(procEquipmentGroupParamDetailPagedQuery);

            using var conn = GetMESDbConnection();
            var procEquipmentGroupParamDetailEntitiesTask = conn.QueryAsync<ProcEquipmentGroupParamDetailEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procEquipmentGroupParamDetailEntities = await procEquipmentGroupParamDetailEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcEquipmentGroupParamDetailEntity>(procEquipmentGroupParamDetailEntities, procEquipmentGroupParamDetailPagedQuery.PageIndex, procEquipmentGroupParamDetailPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamDetailEntity>> GetProcEquipmentGroupParamDetailEntitiesAsync(ProcEquipmentGroupParamDetailQuery procEquipmentGroupParamDetailQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcEquipmentGroupParamDetailEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procEquipmentGroupParamDetailEntities = await conn.QueryAsync<ProcEquipmentGroupParamDetailEntity>(template.RawSql, procEquipmentGroupParamDetailQuery);
            return procEquipmentGroupParamDetailEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcEquipmentGroupParamDetailEntity procEquipmentGroupParamDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procEquipmentGroupParamDetailEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcEquipmentGroupParamDetailEntity> procEquipmentGroupParamDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procEquipmentGroupParamDetailEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcEquipmentGroupParamDetailEntity procEquipmentGroupParamDetailEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procEquipmentGroupParamDetailEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procEquipmentGroupParamDetailEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcEquipmentGroupParamDetailEntity> procEquipmentGroupParamDetailEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procEquipmentGroupParamDetailEntitys);
        }
        #endregion

        /// <summary>
        /// 批量删除 (硬删除) 根据 RecipeId
        /// </summary>
        /// <param name="vehicleTypeIds"></param>
        /// <returns></returns>
        public async Task<int> DeleteTrueByRecipeIdsAsync(long[] recipeIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteTrueByRecipeIdsSql, new { RecipeIds = recipeIds });
        }

        /// <summary>
        /// 根据参数ID查询List
        /// </summary>
        /// <param name="recipeIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamDetailEntity>> GetEntitiesByRecipeIdsAsync(long[] recipeIds) 
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesByRecipeIdsSql);
            using var conn = GetMESDbConnection();
            var entitys = await conn.QueryAsync<ProcEquipmentGroupParamDetailEntity>(template.RawSql, new { RecipeIds = recipeIds });
            return entitys;
        }
    }

    public partial class ProcEquipmentGroupParamDetailRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_equipment_group_param_detail` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_equipment_group_param_detail` /**where**/ ";
        const string GetProcEquipmentGroupParamDetailEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_equipment_group_param_detail` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_equipment_group_param_detail`(  `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `DecimalPlaces`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RecipeId, @ParamId, @ParamValue, @CenterValue, @MaxValue, @MinValue, @DecimalPlaces, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_equipment_group_param_detail`(  `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `DecimalPlaces`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RecipeId, @ParamId, @ParamValue, @CenterValue, @MaxValue, @MinValue, @DecimalPlaces, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_equipment_group_param_detail` SET   SiteId = @SiteId, RecipeId = @RecipeId, ParamId = @ParamId, ParamValue = @ParamValue, CenterValue = @CenterValue, MaxValue = @MaxValue, MinValue = @MinValue, DecimalPlaces = @DecimalPlaces, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_equipment_group_param_detail` SET   SiteId = @SiteId, RecipeId = @RecipeId, ParamId = @ParamId, ParamValue = @ParamValue, CenterValue = @CenterValue, MaxValue = @MaxValue, MinValue = @MinValue, DecimalPlaces = @DecimalPlaces, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_equipment_group_param_detail` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_equipment_group_param_detail` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `DecimalPlaces`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_equipment_group_param_detail`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `DecimalPlaces`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_equipment_group_param_detail`  WHERE Id IN @Ids ";
        #endregion
                
        const string DeleteTrueByRecipeIdsSql = "DELETE From `proc_equipment_group_param_detail` WHERE  RecipeId in @RecipeIds";
        const string GetEntitiesByRecipeIdsSql= @"SELECT *   FROM `proc_equipment_group_param_detail` WHERE  RecipeId in @RecipeIds   ";
    }
}
