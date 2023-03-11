using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
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
            return await conn.QueryFirstOrDefaultAsync<EquFaultPhenomenonView>(GetViewById, new { Id = id });
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
            sqlBuilder.Where("EFP.IsDeleted = 0");
            sqlBuilder.Where("EFP.SiteId = @SiteId");
            sqlBuilder.OrderBy("EFP.UpdatedOn DESC");

            sqlBuilder.Select("EFP.Id, EFP.FaultPhenomenonCode, EFP.FaultPhenomenonName, EFP.EquipmentGroupId, EFP.UseStatus, EFP.CreatedBy, EFP.CreatedOn, EFP.UpdatedBy, EFP.UpdatedOn, EEG.EquipmentGroupName");
            sqlBuilder.LeftJoin("equ_equipment_group EEG ON EFP.EquipmentGroupId = EEG.Id");

            if (string.IsNullOrWhiteSpace(pagedQuery.FaultPhenomenonCode) == false)
            {
                pagedQuery.FaultPhenomenonCode = $"%{pagedQuery.FaultPhenomenonCode}%";
                sqlBuilder.Where("EFP.FaultPhenomenonCode LIKE @FaultPhenomenonCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.FaultPhenomenonName) == false)
            {
                pagedQuery.FaultPhenomenonName = $"%{pagedQuery.FaultPhenomenonName}%";
                sqlBuilder.Where("EFP.FaultPhenomenonName LIKE @FaultPhenomenonName");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupName) == false)
            {
                pagedQuery.EquipmentGroupName = $"%{pagedQuery.EquipmentGroupName}%";
                sqlBuilder.Where("EEG.EquipmentGroupName LIKE @EquipmentGroupName");
            }

            if (pagedQuery.UseStatus.HasValue == true)
            {
                sqlBuilder.Where("EFP.UseStatus = @UseStatus");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquFaultPhenomenonEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<EquFaultPhenomenonEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquFaultPhenomenonRepository
    {
        const string InsertSql = "INSERT INTO `equ_fault_phenomenon`( `Id`, `FaultPhenomenonCode`, `FaultPhenomenonName`, `EquipmentGroupId`, `UseStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @FaultPhenomenonCode, @FaultPhenomenonName, @EquipmentGroupId, @UseStatus, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_fault_phenomenon` SET FaultPhenomenonName = @FaultPhenomenonName, EquipmentGroupId = @EquipmentGroupId, UseStatus = @UseStatus, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, Remark = @Remark WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_fault_phenomenon` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id = @Ids ";
        const string IsExistsSql = "SELECT Id FROM equ_fault_phenomenon WHERE `IsDeleted` = 0 AND FaultPhenomenonCode = @faultPhenomenonCode LIMIT 1";
        const string GetByIdSql = @"SELECT 
                               `Id`, `FaultPhenomenonCode`, `FaultPhenomenonName`, `EquipmentGroupId`, `UseStatus`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`
                            FROM `equ_fault_phenomenon`  WHERE Id = @Id ";
        const string GetViewById = @"SELECT 
                EFP.Id, EFP.FaultPhenomenonCode, EFP.FaultPhenomenonName, EFP.EquipmentGroupId, EFP.UseStatus, EFP.CreatedBy, EFP.CreatedOn, EFP.UpdatedBy, EFP.UpdatedOn, EEG.EquipmentGroupName
                FROM equ_fault_phenomenon EFP LEFT JOIN equ_equipment_group EEG ON EFP.EquipmentGroupId = EEG.Id 
                WHERE EFP.Id = @Id ";

        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_fault_phenomenon EFP /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM equ_fault_phenomenon EFP /**innerjoin**/ /**leftjoin**/ /**where**/ ";

    }
}
