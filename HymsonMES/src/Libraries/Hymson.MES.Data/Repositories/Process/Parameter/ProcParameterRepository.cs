using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.Parameter.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 标准参数表仓储
    /// </summary>
    public partial class ProcParameterRepository : IProcParameterRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ProcParameterRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache)
        {
            _connectionOptions = connectionOptions.Value;
            _memoryCache = memoryCache;
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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcParameterEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcParameterEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcParameterEntity>(GetByIdsSql, new { ids });
        }

        /// <summary>
        /// 更具编码获取参数信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterEntity>> GetByCodesAsync(ProcParametersByCodeQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcParameterEntity>(GetByCodesSql, param);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterEntity>> GetPagedListAsync(ProcParameterPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            sqlBuilder.Where(" SiteId = @SiteId ");

            if (!string.IsNullOrEmpty(pagedQuery.ParameterUnit))
            {
                pagedQuery.ParameterUnit = $"%{pagedQuery.ParameterUnit}%";
                sqlBuilder.Where("ParameterUnit LIKE @ParameterUnit");
            }

            if (pagedQuery.DataType.HasValue)
            {
                sqlBuilder.Where("DataType = @DataType");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ParameterCode))
            {
                pagedQuery.ParameterCode = $"%{pagedQuery.ParameterCode}%";
                sqlBuilder.Where(" ParameterCode LIKE @ParameterCode ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ParameterName))
            {
                pagedQuery.ParameterName = $"%{pagedQuery.ParameterName}%";
                sqlBuilder.Where(" ParameterName LIKE @ParameterName ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Remark))
            {
                pagedQuery.Remark = $"%{pagedQuery.Remark}%";
                sqlBuilder.Where(" Remark LIKE @Remark ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterEntitiesTask = conn.QueryAsync<ProcParameterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procParameterEntities = await procParameterEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcParameterEntity>(procParameterEntities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procParameterQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterEntity>> GetProcParameterEntitiesAsync(ProcParameterQuery procParameterQuery)
        {
            var key = $"proc_parameter&{procParameterQuery.SiteId}&{procParameterQuery.ParameterCode}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                var sqlBuilder = new SqlBuilder();
                var template = sqlBuilder.AddTemplate(GetProcParameterEntitiesSqlTemplate);
                sqlBuilder.Where("IsDeleted = 0");
                sqlBuilder.Where("SiteId = @SiteId");
                sqlBuilder.Select("*");

                if (!string.IsNullOrWhiteSpace(procParameterQuery.ParameterCode))
                {
                    sqlBuilder.Where("ParameterCode = @ParameterCode");
                }

                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                var procParameterEntities = await conn.QueryAsync<ProcParameterEntity>(template.RawSql, procParameterQuery);
                return procParameterEntities;
            });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcParameterEntity procParameterEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procParameterEntity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procParameterEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcParameterEntity procParameterEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procParameterEntity);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcParameterRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_parameter` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_parameter` /**where**/ ";
        const string GetProcParameterEntitiesSqlTemplate = @"SELECT /**select**/ FROM `proc_parameter` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_parameter`(  `Id`, `SiteId`, `ParameterCode`, `ParameterName`, `ParameterUnit`, DataType, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ParameterCode, @ParameterName, @ParameterUnit, @DataType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_parameter` SET ParameterUnit = @ParameterUnit, DataType = @DataType, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_parameter` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_parameter` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `proc_parameter` WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_parameter` WHERE Id IN @ids AND IsDeleted=0 ";
        const string GetByCodesSql = @"SELECT * FROM `proc_parameter` WHERE ParameterCode IN @Codes AND SiteId= @SiteId  AND IsDeleted=0 ";
    }

}
