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
    /// 标准参数表仓储
    /// </summary>
    public partial class ProcParameterRepository : IProcParameterRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcParameterRepository(IOptions<ConnectionOptions> connectionOptions)
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
        /// 分页查询
        /// </summary>
        /// <param name="procParameterPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterEntity>> GetPagedInfoAsync(ProcParameterPagedQuery procParameterPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            //if (procParameterPagedQuery.SiteId != 0)
            //{
            sqlBuilder.Where(" SiteId = @SiteId ");
            //}
            if (!string.IsNullOrWhiteSpace(procParameterPagedQuery.ParameterCode))
            {
                procParameterPagedQuery.ParameterCode = $"%{procParameterPagedQuery.ParameterCode}%";
                sqlBuilder.Where(" ParameterCode like @ParameterCode ");
            }
            if (!string.IsNullOrWhiteSpace(procParameterPagedQuery.ParameterName))
            {
                procParameterPagedQuery.ParameterName = $"%{procParameterPagedQuery.ParameterName}%";
                sqlBuilder.Where(" ParameterName like @ParameterName ");
            }
            if (!string.IsNullOrWhiteSpace(procParameterPagedQuery.Remark))
            {
                procParameterPagedQuery.Remark = $"%{procParameterPagedQuery.Remark}%";
                sqlBuilder.Where(" Remark like @Remark ");
            }

            var offSet = (procParameterPagedQuery.PageIndex - 1) * procParameterPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procParameterPagedQuery.PageSize });
            sqlBuilder.AddParameters(procParameterPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterEntitiesTask = conn.QueryAsync<ProcParameterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procParameterEntities = await procParameterEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcParameterEntity>(procParameterEntities, procParameterPagedQuery.PageIndex, procParameterPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procParameterQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterEntity>> GetProcParameterEntitiesAsync(ProcParameterQuery procParameterQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcParameterEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (procParameterQuery.SiteId != 0)
            //{
            sqlBuilder.Where(" SiteId=@SiteId ");
            //}
            if (!string.IsNullOrWhiteSpace(procParameterQuery.ParameterCode))
            {
                //procParameterQuery.ParameterCode = $"%{procParameterQuery.ParameterCode}%";
                sqlBuilder.Where(" ParameterCode = @ParameterCode ");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterEntities = await conn.QueryAsync<ProcParameterEntity>(template.RawSql, procParameterQuery);
            return procParameterEntities;
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
        const string GetProcParameterEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_parameter` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_parameter`(  `Id`, `SiteId`, `ParameterCode`, `ParameterName`, `ParameterUnit`, DataType, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ParameterCode, @ParameterName, @ParameterUnit, @DataType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_parameter` SET ParameterUnit = @ParameterUnit, DataType = @DataType, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_parameter` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_parameter` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `proc_parameter` WHERE Id = @Id ";

    }

}
