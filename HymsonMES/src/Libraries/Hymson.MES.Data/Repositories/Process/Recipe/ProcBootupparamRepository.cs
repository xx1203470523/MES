/*
 *creator: Karl
 *
 *describe: 开机参数表 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:22:20
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
    /// 开机参数表仓储
    /// </summary>
    public partial class ProcBootupparamRepository :BaseRepository, IProcBootupparamRepository
    {

        public ProcBootupparamRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBootupparamEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBootupparamEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBootupparamEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryAsync<ProcBootupparamEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBootupparamPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBootupparamEntity>> GetPagedInfoAsync(ProcBootupparamPagedQuery procBootupparamPagedQuery)
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
           
            var offSet = (procBootupparamPagedQuery.PageIndex - 1) * procBootupparamPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBootupparamPagedQuery.PageSize });
            sqlBuilder.AddParameters(procBootupparamPagedQuery);

            using var conn = GetMESParamterDbConnection();
            var procBootupparamEntitiesTask = conn.QueryAsync<ProcBootupparamEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBootupparamEntities = await procBootupparamEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBootupparamEntity>(procBootupparamEntities, procBootupparamPagedQuery.PageIndex, procBootupparamPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBootupparamQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBootupparamEntity>> GetProcBootupparamEntitiesAsync(ProcBootupparamQuery procBootupparamQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBootupparamEntitiesSqlTemplate);
            using var conn = GetMESParamterDbConnection();
            var procBootupparamEntities = await conn.QueryAsync<ProcBootupparamEntity>(template.RawSql, procBootupparamQuery);
            return procBootupparamEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootupparamEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBootupparamEntity procBootupparamEntity)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBootupparamEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBootupparamEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBootupparamEntity> procBootupparamEntitys)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procBootupparamEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBootupparamEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBootupparamEntity procBootupparamEntity)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procBootupparamEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBootupparamEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBootupparamEntity> procBootupparamEntitys)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procBootupparamEntitys);
        }
        #endregion

    }

    public partial class ProcBootupparamRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bootupparam` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_bootupparam` /**where**/ ";
        const string GetProcBootupparamEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_bootupparam` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bootupparam`(  `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RecipeId, @ParamId, @ParamValue, @CenterValue, @MaxValue, @MinValue, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_bootupparam`(  `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @RecipeId, @ParamId, @ParamValue, @CenterValue, @MaxValue, @MinValue, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_bootupparam` SET   SiteId = @SiteId, RecipeId = @RecipeId, ParamId = @ParamId, ParamValue = @ParamValue, CenterValue = @CenterValue, MaxValue = @MaxValue, MinValue = @MinValue, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_bootupparam` SET   SiteId = @SiteId, RecipeId = @RecipeId, ParamId = @ParamId, ParamValue = @ParamValue, CenterValue = @CenterValue, MaxValue = @MaxValue, MinValue = @MinValue, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_bootupparam` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bootupparam` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_bootupparam`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `RecipeId`, `ParamId`, `ParamValue`, `CenterValue`, `MaxValue`, `MinValue`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_bootupparam`  WHERE Id IN @Ids ";
        #endregion
    }
}
