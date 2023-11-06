/*
 *creator: Karl
 *
 *describe: ESOP 仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// ESOP仓储
    /// </summary>
    public partial class ProcEsopRepository : BaseRepository, IProcEsopRepository
    {

        public ProcEsopRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<ProcEsopEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcEsopEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEsopEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcEsopEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procEsopPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEsopView>> GetPagedInfoAsync(ProcEsopPagedQuery procEsopPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("esop.SiteId = @SiteId");
            sqlBuilder.Where("esop.IsDeleted=0");

            sqlBuilder.Select("esop.Id,pm.MaterialCode,pm.Version,pp.`Code` ProcedureCode,pp.`Name` ProcedureName,esop.`Status`,esop.UpdatedBy,esop.UpdatedOn");
            sqlBuilder.LeftJoin("proc_material pm on esop.MaterialId=pm.Id and pm.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on esop.ProcedureId=pp.Id and pp.IsDeleted=0");

            if (string.IsNullOrEmpty(procEsopPagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("esop.UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(procEsopPagedQuery.Sorting);
            }

            //状态
            if (procEsopPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("esop.Status=@Status");
            }
            //物料编码
            if (!string.IsNullOrWhiteSpace(procEsopPagedQuery.MaterialCode))
            {
                procEsopPagedQuery.MaterialCode = $"%{procEsopPagedQuery.MaterialCode}%";
                sqlBuilder.Where("pm.MaterialCode like @MaterialCode");
            }
            //物料名称
            if (!string.IsNullOrWhiteSpace(procEsopPagedQuery.MaterialName))
            {
                procEsopPagedQuery.MaterialName = $"%{procEsopPagedQuery.MaterialName}%";
                sqlBuilder.Where("pm.MaterialName like @MaterialName");
            }
            //工序编码
            if (!string.IsNullOrWhiteSpace(procEsopPagedQuery.ProcedureCode))
            {
                procEsopPagedQuery.ProcedureCode = $"%{procEsopPagedQuery.ProcedureCode}%";
                sqlBuilder.Where("pp.Code like @ProcedureCode");
            }
            //工序名称
            if (!string.IsNullOrWhiteSpace(procEsopPagedQuery.ProcedureName))
            {
                procEsopPagedQuery.ProcedureName = $"%{procEsopPagedQuery.ProcedureName}%";
                sqlBuilder.Where("pp.Name like @ProcedureName");
            }

            var offSet = (procEsopPagedQuery.PageIndex - 1) * procEsopPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procEsopPagedQuery.PageSize });
            sqlBuilder.AddParameters(procEsopPagedQuery);

            using var conn = GetMESDbConnection();
            var procEsopEntitiesTask = conn.QueryAsync<ProcEsopView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procEsopEntities = await procEsopEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcEsopView>(procEsopEntities, procEsopPagedQuery.PageIndex, procEsopPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procEsopQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEsopEntity>> GetProcEsopEntitiesAsync(ProcEsopQuery procEsopQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcEsopEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var procEsopEntities = await conn.QueryAsync<ProcEsopEntity>(template.RawSql, procEsopQuery);
            return procEsopEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEsopEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcEsopEntity procEsopEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procEsopEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEsopEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcEsopEntity> procEsopEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procEsopEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEsopEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcEsopEntity procEsopEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procEsopEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procEsopEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcEsopEntity> procEsopEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procEsopEntitys);
        }
        #endregion

    }

    public partial class ProcEsopRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_esop` esop /**innerjoin**/ /**leftjoin**/ /**where**/  /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_esop esop ` /**where**/ ";
        const string GetProcEsopEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_esop` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_esop`(  `Id`, `SiteId`, `MaterialId`, `ProcedureId`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @MaterialId, @ProcedureId, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_esop`(  `Id`, `SiteId`, `MaterialId`, `ProcedureId`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @MaterialId, @ProcedureId, @Status, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `proc_esop` SET   SiteId = @SiteId, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_esop` SET   SiteId = @SiteId, MaterialId = @MaterialId, ProcedureId = @ProcedureId, Status = @Status, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_esop` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_esop` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `MaterialId`, `ProcedureId`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_esop`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `MaterialId`, `ProcedureId`, `Status`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_esop`  WHERE Id IN @Ids ";
        #endregion
    }
}
