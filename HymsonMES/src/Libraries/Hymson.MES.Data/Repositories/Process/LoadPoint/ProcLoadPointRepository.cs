/*
 *creator: Karl
 *
 *describe: 上料点表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 上料点表仓储
    /// </summary>
    public partial class ProcLoadPointRepository : IProcLoadPointRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcLoadPointRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, new { ids=ids });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcLoadPointEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcLoadPointEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procLoadPointPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointEntity>> GetPagedInfoAsync(ProcLoadPointPagedQuery procLoadPointPagedQuery)
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
           
            var offSet = (procLoadPointPagedQuery.PageIndex - 1) * procLoadPointPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procLoadPointPagedQuery.PageSize });
            sqlBuilder.AddParameters(procLoadPointPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procLoadPointEntitiesTask = conn.QueryAsync<ProcLoadPointEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procLoadPointEntities = await procLoadPointEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcLoadPointEntity>(procLoadPointEntities, procLoadPointPagedQuery.PageIndex, procLoadPointPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procLoadPointQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcLoadPointEntity>> GetProcLoadPointEntitiesAsync(ProcLoadPointQuery procLoadPointQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcLoadPointEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procLoadPointEntities = await conn.QueryAsync<ProcLoadPointEntity>(template.RawSql, procLoadPointQuery);
            return procLoadPointEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcLoadPointEntity procLoadPointEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procLoadPointEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcLoadPointEntity> procLoadPointEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procLoadPointEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcLoadPointEntity procLoadPointEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procLoadPointEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcLoadPointEntity> procLoadPointEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procLoadPointEntitys);
        }

    }

    public partial class ProcLoadPointRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_load_point` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_load_point` /**where**/ ";
        const string GetProcLoadPointEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_load_point` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_load_point`(  `Id`, `SiteCode`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @LoadPoint, @LoadPointName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_load_point`(  `Id`, `SiteCode`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @LoadPoint, @LoadPointName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_load_point` SET   SiteCode = @SiteCode, LoadPoint = @LoadPoint, LoadPointName = @LoadPointName, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_load_point` SET   SiteCode = @SiteCode, LoadPoint = @LoadPoint, LoadPointName = @LoadPointName, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_load_point` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_load_point` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `LoadPoint`, `LoadPointName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_load_point`  WHERE Id IN @ids ";
    }
}
