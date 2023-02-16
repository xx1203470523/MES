/*
 *creator: Karl
 *
 *describe: 标准参数关联类型表 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-13 05:06:17
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
    /// 标准参数关联类型表仓储
    /// </summary>
    public partial class ProcParameterLinkTypeRepository : IProcParameterLinkTypeRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public ProcParameterLinkTypeRepository(IOptions<ConnectionOptions> connectionOptions)
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
        public async Task<ProcParameterLinkTypeEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcParameterLinkTypeEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterLinkTypeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcParameterLinkTypeEntity>(GetByIdsSql, new { ids = ids});
        }

        /// <summary>
        /// 根据ParameterIDs批量获取数据
        /// </summary>
        /// <param name="parameterIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterLinkTypeEntity>> GetByParameterIdsAsync(long[] parameterIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcParameterLinkTypeEntity>(GetByParameterIdsSql, new { parameterIds = parameterIds });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procParameterLinkTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterLinkTypeEntity>> GetPagedInfoAsync(ProcParameterLinkTypePagedQuery procParameterLinkTypePagedQuery)
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
           
            var offSet = (procParameterLinkTypePagedQuery.PageIndex - 1) * procParameterLinkTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procParameterLinkTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(procParameterLinkTypePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterLinkTypeEntitiesTask = conn.QueryAsync<ProcParameterLinkTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procParameterLinkTypeEntities = await procParameterLinkTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcParameterLinkTypeEntity>(procParameterLinkTypeEntities, procParameterLinkTypePagedQuery.PageIndex, procParameterLinkTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procParameterLinkTypeQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterLinkTypeEntity>> GetProcParameterLinkTypeEntitiesAsync(ProcParameterLinkTypeQuery procParameterLinkTypeQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcParameterLinkTypeEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            if (!string.IsNullOrEmpty(procParameterLinkTypeQuery.SiteCode)) 
            {
                sqlBuilder.Where(" SiteCode = @SiteCode ");
            }
            if (procParameterLinkTypeQuery.ParameterID!=0) 
            {
                sqlBuilder.Where(" ParameterID = @ParameterID ");
            }
            if (procParameterLinkTypeQuery.ParameterType > 0)
            {
                sqlBuilder.Where(" ParameterType = @ParameterType ");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterLinkTypeEntities = await conn.QueryAsync<ProcParameterLinkTypeEntity>(template.RawSql, procParameterLinkTypeQuery);
            return procParameterLinkTypeEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procParameterLinkTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcParameterLinkTypeEntity procParameterLinkTypeEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, procParameterLinkTypeEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procParameterLinkTypeEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcParameterLinkTypeEntity> procParameterLinkTypeEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, procParameterLinkTypeEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procParameterLinkTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcParameterLinkTypeEntity procParameterLinkTypeEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, procParameterLinkTypeEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procParameterLinkTypeEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcParameterLinkTypeEntity> procParameterLinkTypeEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, procParameterLinkTypeEntitys);
        }

    }

    public partial class ProcParameterLinkTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_parameter_link_type` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `proc_parameter_link_type` /**where**/ ";
        const string GetProcParameterLinkTypeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_parameter_link_type` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_parameter_link_type`(  `Id`, `SiteCode`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ParameterID, @ParameterType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_parameter_link_type`(  `Id`, `SiteCode`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteCode, @ParameterID, @ParameterType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_parameter_link_type` SET   SiteCode = @SiteCode, ParameterID = @ParameterID, ParameterType = @ParameterType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_parameter_link_type` SET   SiteCode = @SiteCode, ParameterID = @ParameterID, ParameterType = @ParameterType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_parameter_link_type` SET IsDeleted = '1' WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_parameter_link_type` SET IsDeleted = '1' WHERE Id in @ids";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteCode`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_parameter_link_type`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_parameter_link_type`  WHERE Id IN @ids ";
        const string GetByParameterIdsSql = @"SELECT 
                                          `Id`, `SiteCode`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_parameter_link_type`  WHERE IsDeleted =0 AND ParameterID IN @parameterIds ";
    }
}
