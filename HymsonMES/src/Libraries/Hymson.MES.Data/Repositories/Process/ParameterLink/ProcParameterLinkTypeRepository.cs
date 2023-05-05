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
    /// 标准参数关联类型表仓储
    /// </summary>
    public partial class ProcParameterLinkTypeRepository : IProcParameterLinkTypeRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
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
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 批量删除（真删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesTrueAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesTrueSql, new { ids = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcParameterLinkTypeEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcParameterLinkTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcParameterLinkTypeEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcParameterLinkTypeEntity>(GetByIdsSql, new { ids = ids });
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
        public async Task<PagedInfo<ProcParameterLinkTypeView>> GetPagedInfoAsync(ProcParameterLinkTypePagedQuery procParameterLinkTypePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where(" g.IsDeleted=0 ");
            //sqlBuilder.Select("*");

            if (procParameterLinkTypePagedQuery.SiteId != 0)
            {
                sqlBuilder.Where(" g.SiteId=@SiteId ");
            }
            if (procParameterLinkTypePagedQuery.ParameterType > 0)
            {
                sqlBuilder.Where(" g.ParameterType=@ParameterType ");
            }
            if (!string.IsNullOrWhiteSpace(procParameterLinkTypePagedQuery.ParameterCode))
            {
                procParameterLinkTypePagedQuery.ParameterCode = $"%{procParameterLinkTypePagedQuery.ParameterCode}%";
                sqlBuilder.Where(" o.ParameterCode like @ParameterCode ");
            }
            if (!string.IsNullOrWhiteSpace(procParameterLinkTypePagedQuery.ParameterName))
            {
                procParameterLinkTypePagedQuery.ParameterName = $"%{procParameterLinkTypePagedQuery.ParameterName}%";
                sqlBuilder.Where(" o.ParameterName like @ParameterName ");
            }

            var offSet = (procParameterLinkTypePagedQuery.PageIndex - 1) * procParameterLinkTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procParameterLinkTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(procParameterLinkTypePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterLinkTypeEntitiesTask = conn.QueryAsync<ProcParameterLinkTypeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procParameterLinkTypeEntities = await procParameterLinkTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcParameterLinkTypeView>(procParameterLinkTypeEntities, procParameterLinkTypePagedQuery.PageIndex, procParameterLinkTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procParameterLinkTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcParameterLinkTypeView>> GetPagedProcParameterLinkTypeByTypeAsync(ProcParameterDetailPagerQuery procParameterDetailPagerQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedProcParameterLinkTypeByTypeSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedProcParameterLinkTypeByTypeCountSqlTemplate);
            sqlBuilder.Where(" g.IsDeleted=0 ");
            //sqlBuilder.Select("*");

            if (procParameterDetailPagerQuery.SiteId != 0)
            {
                sqlBuilder.Where(" g.SiteId=@SiteId ");
            }
            if (!string.IsNullOrWhiteSpace(procParameterDetailPagerQuery.ParameterCode))
            {
                procParameterDetailPagerQuery.ParameterCode = $"%{procParameterDetailPagerQuery.ParameterCode}%";
                sqlBuilder.Where(" g.ParameterCode like @ParameterCode ");
            }
            if (!string.IsNullOrWhiteSpace(procParameterDetailPagerQuery.ParameterName))
            {
                procParameterDetailPagerQuery.ParameterName = $"%{procParameterDetailPagerQuery.ParameterName}%";
                sqlBuilder.Where(" g.ParameterName like @ParameterName ");
            }
            if (procParameterDetailPagerQuery.OperateType == Core.Enums.OperateTypeEnum.Add)
            {
                sqlBuilder.Where(" (o.Id is null or trim(o.Id) = '') ");
            }

            var offSet = (procParameterDetailPagerQuery.PageIndex - 1) * procParameterDetailPagerQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procParameterDetailPagerQuery.PageSize });
            sqlBuilder.AddParameters(procParameterDetailPagerQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procParameterLinkTypeEntitiesTask = conn.QueryAsync<ProcParameterLinkTypeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procParameterLinkTypeEntities = await procParameterLinkTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcParameterLinkTypeView>(procParameterLinkTypeEntities, procParameterDetailPagerQuery.PageIndex, procParameterDetailPagerQuery.PageSize, totalCount);
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
            sqlBuilder.Where(" IsDeleted = 0 ");

            if (procParameterLinkTypeQuery.SiteId != 0) sqlBuilder.Where(" SiteId = @SiteId ");
            if (procParameterLinkTypeQuery.ParameterID != 0) sqlBuilder.Where(" ParameterID = @ParameterID ");
            if (procParameterLinkTypeQuery.ParameterType > 0) sqlBuilder.Where(" ParameterType = @ParameterType ");

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
        public async Task<int> InsertsAsync(IEnumerable<ProcParameterLinkTypeEntity> procParameterLinkTypeEntitys)
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
        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                                g.Id, g.SiteId, g.ParameterType, o.Id as ParameterID,
                                o.ParameterCode, o.ParameterName, o.ParameterUnit,
                                g.Remark, g.CreatedBy, g.CreatedOn, g.UpdatedBy, g.UpdatedOn
                                FROM `proc_parameter_link_type` g 
                                LEFT JOIN proc_parameter o ON g.ParameterID = o.Id 
            /**where**/ ORDER BY g.UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(*) 
                FROM `proc_parameter_link_type` g 
                LEFT JOIN proc_parameter o ON g.ParameterID = o.Id 
                /**where**/ ";

        const string GetPagedProcParameterLinkTypeByTypeSqlTemplate = @"SELECT 
                                o.Id, g.SiteId, g.Id as ParameterID, o.ParameterType, 
                                g.ParameterCode, g.ParameterName, g.Remark, o.CreatedBy,
                                o.CreatedOn, o.UpdatedBy, o.UpdatedOn 
                                FROM `proc_parameter` g 
                                LEFT JOIN proc_parameter_link_type o ON o.ParameterID = g.Id AND o.IsDeleted=0 AND o.ParameterType = @ParameterType 
            /**where**/ ORDER BY g.UpdatedOn DESC LIMIT @Offset,@Rows ";
        const string GetPagedProcParameterLinkTypeByTypeCountSqlTemplate = @"SELECT COUNT(*) 
                        FROM `proc_parameter` g 
                        LEFT JOIN proc_parameter_link_type o ON o.ParameterID = g.Id AND o.IsDeleted=0 AND o.ParameterType = @ParameterType 
                /**where**/ ";
        const string GetProcParameterLinkTypeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_parameter_link_type` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_parameter_link_type`(  `Id`, `SiteId`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ParameterID, @ParameterType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `proc_parameter_link_type`(  `Id`, `SiteId`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @ParameterID, @ParameterType, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `proc_parameter_link_type` SET   SiteId = @SiteId, ParameterID = @ParameterID, ParameterType = @ParameterType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_parameter_link_type` SET   SiteId = @SiteId, ParameterID = @ParameterID, ParameterType = @ParameterType, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_parameter_link_type` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_parameter_link_type` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string DeletesTrueSql = " Delete FROM `proc_parameter_link_type` WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_parameter_link_type`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_parameter_link_type`  WHERE Id IN @ids ";
        const string GetByParameterIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `ParameterID`, `ParameterType`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `proc_parameter_link_type`  WHERE IsDeleted =0 AND ParameterID IN @parameterIds ";
    }
}
