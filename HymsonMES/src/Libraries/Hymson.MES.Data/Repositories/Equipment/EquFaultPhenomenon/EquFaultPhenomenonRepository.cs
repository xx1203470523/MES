using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon
{
    /// <summary>
    /// 仓储（设备故障现象）
    /// </summary>
    public partial class EquFaultPhenomenonRepository : IEquFaultPhenomenonRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquFaultPhenomenonRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增（设备故障现象）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquFaultPhenomenonEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquFaultPhenomenonEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（设备故障现象）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, idsArr);

        }

        /// <summary>
        /// 判断是否存在（编码）
        /// </summary>
        /// <param name="faultPhenomenonCode"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(string faultPhenomenonCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteScalarAsync(IsExistsSql, new { faultPhenomenonCode }) != null;
        }

        /// <summary>
        /// 根据ID获取数据（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquFaultPhenomenonEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonView> GetViewByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquFaultPhenomenonView>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 分页查询（设备故障现象）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultPhenomenonEntity>> GetPagedInfoAsync(EquFaultPhenomenonPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            //sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equFaultPhenomenonEntitiesTask = conn.QueryAsync<EquFaultPhenomenonEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equFaultPhenomenonEntities = await equFaultPhenomenonEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquFaultPhenomenonEntity>(equFaultPhenomenonEntities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List（设备故障现象）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultPhenomenonEntity>> GetEquFaultPhenomenonEntitiesAsync(EquFaultPhenomenonQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquFaultPhenomenonEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equFaultPhenomenonEntities = await conn.QueryAsync<EquFaultPhenomenonEntity>(template.RawSql, query);
            return equFaultPhenomenonEntities;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquFaultPhenomenonRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_fault_phenomenon` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `equ_fault_phenomenon` /**where**/";
        const string GetEquFaultPhenomenonEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_fault_phenomenon` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_fault_phenomenon`(  `Id`, `FaultPhenomenonCode`, `FaultPhenomenonName`, `EquipmentGroupId`, `UseStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @FaultPhenomenonCode, @FaultPhenomenonName, @EquipmentGroupId, @UseStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_fault_phenomenon` SET   FaultPhenomenonCode = @FaultPhenomenonCode, FaultPhenomenonName = @FaultPhenomenonName, EquipmentGroupId = @EquipmentGroupId, UseStatus = @UseStatus, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_fault_phenomenon` SET IsDeleted = '1' WHERE Id = @Id ";
        const string IsExistsSql = "SELECT Id FROM equ_fault_phenomenon WHERE `IsDeleted` = 0 AND FaultPhenomenonCode = @faultPhenomenonCode LIMIT 1";
        const string GetByIdSql = @"SELECT 
                               `Id`, `FaultPhenomenonCode`, `FaultPhenomenonName`, `EquipmentGroupId`, `UseStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`
                            FROM `equ_fault_phenomenon`  WHERE Id = @Id ";
    }
}
