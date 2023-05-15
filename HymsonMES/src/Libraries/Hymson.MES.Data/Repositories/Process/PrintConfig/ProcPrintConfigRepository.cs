﻿using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProcPrintConfigRepository : IProcPrintConfigRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcPrintConfigRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcPrinterEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcPrinterEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 判断是否存在（编码）
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(string PrintName)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteScalarAsync(IsExistsSql, new { PrintName }) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcPrinterEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcPrinterEntity>(GetByIdSql, new { id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrintName"></param>
        /// <returns></returns>
        public async Task<ProcPrinterEntity> GetByPrintNameAsync(string PrintName)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcPrinterEntity>(GetByPrintNameSql, new { PrintName });
        }

        /// <summary>
        /// 根据IP查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcPrinterEntity> GetByPrintIpAsync(EntityByCodeQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcPrinterEntity>(GetByPrintIpSql, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcPrinterEntity>> GetBaseListAsync()
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ProcPrinterEntity>(GetBaseListSql);
        }

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcPrinterEntity>> GetListAsync(ProcPrinterPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedListSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedListCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(query.PrintName))
            {
                query.PrintName = $"%{query.PrintName}%";
                sqlBuilder.Where("PrintName LIKE @PrintName");
            }
            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceEntitiesTask = conn.QueryAsync<ProcPrinterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceEntities = await procResourceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcPrinterEntity>(procResourceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcPrinterEntity>> GetEntitiesAsync(ProcPrinterQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var Entities = await conn.QueryAsync<ProcPrinterEntity>(template.RawSql, query);
            return Entities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcPrinterEntity>> GetPagedListAsync(ProcPrinterPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");


            if (string.IsNullOrWhiteSpace(pagedQuery.PrintName) == false)
            {
                pagedQuery.PrintName = $"%{pagedQuery.PrintName}%";
                sqlBuilder.Where("PrintName LIKE @PrintName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });

            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<ProcPrinterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<ProcPrinterEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcPrintConfigRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `proc_printer`(  `Id`, `SiteId`, `PrintName`, `PrintIp`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @PrintName, @PrintIp, @Remark,  @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted)  ";
        const string UpdateSql = "UPDATE `proc_printer` SET PrintName = @PrintName, Remark = @Remark,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `proc_printer` SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE `Id` in @Ids;";
        const string IsExistsSql = "SELECT Id FROM proc_printer WHERE `IsDeleted` = 0 AND PrintName = @PrintName LIMIT 1";
        const string GetByIdSql = "SELECT * FROM `proc_printer` WHERE `Id` = @Id;";
        //const string GetByGroupIdSql = "SELECT * FROM `proc_printer` WHERE `IsDeleted` = 0 AND EquipmentGroupId = @EquipmentGroupId;";
        const string GetBaseListSql = "SELECT * FROM `proc_printer` WHERE `IsDeleted` = 0;";
        const string GetByPrintIpSql = "SELECT * FROM proc_printer WHERE SiteId = @Site AND IsDeleted = 0 AND PrintIp = @Code;";
        const string GetByPrintNameSql = "SELECT * FROM `proc_printer` WHERE `IsDeleted` = 0 AND PrintName = @PrintName;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `proc_printer` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_printer` /**where**/";
        const string GetEntitiesSqlTemplate = "";
        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_printer /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM proc_printer /**where**/";

    }
}
