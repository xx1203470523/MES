/*
 *creator: Karl
 *
 *describe: 开机配方表 仓储类 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-05 04:11:36
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
    /// 开机配方表仓储
    /// </summary>
    public partial class ProcBootuprecipeRepository :BaseRepository, IProcBootuprecipeRepository
    {

        public ProcBootuprecipeRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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

        public async Task<ProcBootuprecipeEntity> GetByCodeAsync(string code)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBootuprecipeEntity>(GetByCodeSql, new { Code = code });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBootuprecipeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcBootuprecipeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBootuprecipeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.QueryAsync<ProcBootuprecipeEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procBootuprecipePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBootuprecipeEntity>> GetPagedInfoAsync(ProcBootuprecipePagedQuery procBootuprecipePagedQuery)
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
           
            var offSet = (procBootuprecipePagedQuery.PageIndex - 1) * procBootuprecipePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procBootuprecipePagedQuery.PageSize });
            sqlBuilder.AddParameters(procBootuprecipePagedQuery);

            using var conn = GetMESParamterDbConnection();
            var procBootuprecipeEntitiesTask = conn.QueryAsync<ProcBootuprecipeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procBootuprecipeEntities = await procBootuprecipeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcBootuprecipeEntity>(procBootuprecipeEntities, procBootuprecipePagedQuery.PageIndex, procBootuprecipePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procBootuprecipeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcBootuprecipeEntity>> GetProcBootuprecipeEntitiesAsync(ProcBootuprecipeQuery procBootuprecipeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcBootuprecipeEntitiesSqlTemplate);
            using var conn = GetMESParamterDbConnection();
            var procBootuprecipeEntities = await conn.QueryAsync<ProcBootuprecipeEntity>(template.RawSql, procBootuprecipeQuery);
            return procBootuprecipeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procBootuprecipeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcBootuprecipeEntity procBootuprecipeEntity)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(InsertSql, procBootuprecipeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procBootuprecipeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcBootuprecipeEntity> procBootuprecipeEntitys)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procBootuprecipeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procBootuprecipeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcBootuprecipeEntity procBootuprecipeEntity)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procBootuprecipeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procBootuprecipeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcBootuprecipeEntity> procBootuprecipeEntitys)
        {
            using var conn = GetMESParamterDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procBootuprecipeEntitys);
        }
        #endregion

    }

    public partial class ProcBootuprecipeRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_bootuprecipe` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_bootuprecipe` /**where**/ ";
        const string GetProcBootuprecipeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_bootuprecipe` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_bootuprecipe`(  `Id`, `SiteId`, `ProductId`, `EquipmentGroupId`, `Code`, `Name`, `Version`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @ProductId, @EquipmentGroupId, @Code, @Name, @Version, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";
        const string InsertsSql = "INSERT INTO `proc_bootuprecipe`(  `Id`, `SiteId`, `ProductId`, `EquipmentGroupId`, `Code`, `Name`, `Version`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @ProductId, @EquipmentGroupId, @Code, @Name, @Version, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";

        const string UpdateSql = "UPDATE `proc_bootuprecipe` SET   SiteId = @SiteId, ProductId = @ProductId, EquipmentGroupId = @EquipmentGroupId, Code = @Code, Name = @Name, Version = @Version, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, IsUsed = @IsUsed  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_bootuprecipe` SET   SiteId = @SiteId, ProductId = @ProductId, EquipmentGroupId = @EquipmentGroupId, Code = @Code, Name = @Name, Version = @Version, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, IsUsed = @IsUsed  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_bootuprecipe` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_bootuprecipe` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ProductId`, `EquipmentGroupId`, `Code`, `Name`, `Version`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_bootuprecipe`  WHERE Id = @Id ";
        const string GetByCodeSql = @"SELECT 
                               `Id`, `SiteId`, `ProductId`, `EquipmentGroupId`, `Code`, `Name`, `Version`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_bootuprecipe`  WHERE Code = @Code ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ProductId`, `EquipmentGroupId`, `Code`, `Name`, `Version`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_bootuprecipe`  WHERE Id IN @Ids ";
        #endregion
    }
}
