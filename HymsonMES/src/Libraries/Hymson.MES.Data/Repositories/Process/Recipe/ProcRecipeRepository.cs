/*
 *creator: Karl
 *
 *describe: 配方表 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-04 03:02:39
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 配方表仓储
    /// </summary>
    public partial class ProcRecipeRepository :BaseRepository, IProcRecipeRepository
    {

        public ProcRecipeRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcRecipeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcRecipeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcRecipeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcRecipeEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procRecipePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcRecipeEntity>> GetPagedInfoAsync(ProcRecipePagedQuery procRecipePagedQuery)
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
           
            var offSet = (procRecipePagedQuery.PageIndex - 1) * procRecipePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procRecipePagedQuery.PageSize });
            sqlBuilder.AddParameters(procRecipePagedQuery);

            using var conn = GetMESDbConnection();
            var procRecipeEntitiesTask = conn.QueryAsync<ProcRecipeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procRecipeEntities = await procRecipeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcRecipeEntity>(procRecipeEntities, procRecipePagedQuery.PageIndex, procRecipePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procRecipeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcRecipeEntity>> GetProcRecipeEntitiesAsync(ProcRecipeQuery procRecipeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcRecipeEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procRecipeEntities = await conn.QueryAsync<ProcRecipeEntity>(template.RawSql, procRecipeQuery);
            return procRecipeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procRecipeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcRecipeEntity procRecipeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procRecipeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procRecipeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcRecipeEntity> procRecipeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procRecipeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procRecipeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcRecipeEntity procRecipeEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procRecipeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procRecipeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcRecipeEntity> procRecipeEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procRecipeEntitys);
        }
        #endregion

    }

    public partial class ProcRecipeRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_recipe` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_recipe` /**where**/ ";
        const string GetProcRecipeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_recipe` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_recipe`(  `Id`, `SiteId`, `ProcedureId`, `ProductId`, `RecipeCode`, `Version`, `RecipeName`, `Remark`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @ProcedureId, @ProductId, @RecipeCode, @Version, @RecipeName, @Remark, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";
        const string InsertsSql = "INSERT INTO `proc_recipe`(  `Id`, `SiteId`, `ProcedureId`, `ProductId`, `RecipeCode`, `Version`, `RecipeName`, `Remark`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @ProcedureId, @ProductId, @RecipeCode, @Version, @RecipeName, @Remark, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";

        const string UpdateSql = "UPDATE `proc_recipe` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ProductId = @ProductId, RecipeCode = @RecipeCode, Version = @Version, RecipeName = @RecipeName, Remark = @Remark, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, IsUsed = @IsUsed  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_recipe` SET   SiteId = @SiteId, ProcedureId = @ProcedureId, ProductId = @ProductId, RecipeCode = @RecipeCode, Version = @Version, RecipeName = @RecipeName, Remark = @Remark, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, IsUsed = @IsUsed  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_recipe` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_recipe` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProcedureId`, `ProductId`, `RecipeCode`, `Version`, `RecipeName`, `Remark`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_recipe`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProcedureId`, `ProductId`, `RecipeCode`, `Version`, `RecipeName`, `Remark`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_recipe`  WHERE Id IN @Ids ";
        #endregion
    }
}
