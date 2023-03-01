using Dapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentRepository : IEquEquipmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquEquipmentRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量修改设备的设备组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateEquipmentGroupIdAsync(UpdateEquipmentGroupIdCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateEquipmentGroupIdSql, command);
        }

        /// <summary>
        /// 清空设备的设备组
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        public async Task<int> ClearEquipmentGroupIdAsync(long equipmentGroupId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ClearEquipmentGroupIdSql, new { equipmentGroupId });
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
        public async Task<bool> IsExistsAsync(string equipmentCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteScalarAsync(IsExistsSql, new { equipmentCode }) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentEntity>(GetByIdSql, new { id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentEntity>> GetByGroupIdAsync(long equipmentGroupId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<EquEquipmentEntity>(GetByGroupIdSql, new { equipmentGroupId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentView> GetViewByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentView>(GetByIdSql, new { id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public async Task<EquEquipmentEntity> GetByEquipmentCodeAsync(string equipmentCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentEntity>(GetByEquipmentCodeSql, new { equipmentCode });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentEntity>> GetBaseListAsync()
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<EquEquipmentEntity>(GetBaseListSql);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentEntity>> GetEntitiesAsync(EquEquipmentQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equipmentEntities = await conn.QueryAsync<EquEquipmentEntity>(template.RawSql, query);
            return equipmentEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentEntity>> GetPagedListAsync(EquEquipmentPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode) == false)
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where("EquipmentCode LIKE @EquipmentCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentName) == false)
            {
                pagedQuery.EquipmentName = $"%{pagedQuery.EquipmentName}%";
                sqlBuilder.Where("EquipmentName LIKE @EquipmentName");
            }

            if (pagedQuery.EquipmentType > DbDefaultValueConstant.IntDefaultValue)
            {
                sqlBuilder.Where("EquipmentType = @EquipmentType");
            }

            if (pagedQuery.UseStatus > 0)
            {
                sqlBuilder.Where("UseStatus = @UseStatus");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.WorkCenterShopName) == false)
            {
                pagedQuery.WorkCenterShopName = $"%{pagedQuery.WorkCenterShopName}%";
                sqlBuilder.Where("WorkCenterShopName LIKE @WorkCenterShopName");
            }

            if (pagedQuery.UseDepartment > 0)
            {
                sqlBuilder.Where("UseDepartment = @UseDepartment");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.Location) == false)
            {
                pagedQuery.Location = $"%{pagedQuery.Location}%";
                sqlBuilder.Where("Location LIKE @Location");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquEquipmentEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `equ_equipment`(  `Id`, `EquipmentCode`, `EquipmentName`, `EquipmentGroupId`, `EquipmentDesc`, `WorkCenterFactoryId`, `WorkCenterShopId`, `WorkCenterLineId`, `Location`, `EquipmentType`, `UseDepartment`, `EntryDate`, `QualTime`, `ExpireDate`, `Manufacturer`, `Supplier`, `UseStatus`, `Power`, `EnergyLevel`, `Ip`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`, `TakeTime`) VALUES (   @Id, @EquipmentCode, @EquipmentName, @EquipmentGroupId, @EquipmentDesc, @WorkCenterFactoryId, @WorkCenterShopId, @WorkCenterLineId, @Location, @EquipmentType, @UseDepartment, @EntryDate, @QualTime, @ExpireDate, @Manufacturer, @Supplier, @UseStatus, @Power, @EnergyLevel, @Ip, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId, @TakeTime )  ";
        const string UpdateSql = "UPDATE `equ_equipment` SET EquipmentName = @EquipmentName, EquipmentDesc = @EquipmentDesc, WorkCenterFactoryId = @WorkCenterFactoryId, WorkCenterShopId = @WorkCenterShopId, WorkCenterLineId = @WorkCenterLineId, Location = @Location, EquipmentType = @EquipmentType, UseDepartment = @UseDepartment, EntryDate = @EntryDate, QualTime = @QualTime, ExpireDate = @ExpireDate, Manufacturer = @Manufacturer, Supplier = @Supplier, UseStatus = @UseStatus, Power = @Power, EnergyLevel = @EnergyLevel, Ip = @Ip, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, Remark = @Remark, TakeTime = @TakeTime WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_equipment` SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE `Id` = @Ids;";
        const string IsExistsSql = "SELECT Id FROM equ_equipment WHERE `IsDeleted` = 0 AND EquipmentCode = @equipmentCode LIMIT 1";
        const string GetByIdSql = "SELECT * FROM `equ_equipment` WHERE `Id` = @Id;";
        const string GetByGroupIdSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0 AND EquipmentGroupId = @EquipmentGroupId;";
        const string GetBaseListSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0;";
        const string GetByEquipmentCodeSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0 AND EquipmentCode = @EquipmentCode;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_equipment` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment` /**where**/";
        const string GetEntitiesSqlTemplate = "";
        /// <summary>
        /// 
        /// </summary>
        const string UpdateEquipmentGroupIdSql = "UPDATE `equ_equipment` SET EquipmentGroupId = @EquipmentGroupId WHERE Id = @Id ";
        const string ClearEquipmentGroupIdSql = "UPDATE `equ_equipment` SET EquipmentGroupId = 0 WHERE EquipmentGroupId = @equipmentGroupId ";
    }
}
