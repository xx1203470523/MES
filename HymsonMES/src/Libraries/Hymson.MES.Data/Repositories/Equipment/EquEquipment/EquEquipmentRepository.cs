using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquEquipmentRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache)
        {
            _connectionOptions = connectionOptions.Value;
            _memoryCache = memoryCache;
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
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentEntity>> GetByGroupIdAsync(long equipmentGroupId)
        {
            var key = $"equ_equipment_{equipmentGroupId}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                return await conn.QueryAsync<EquEquipmentEntity>(GetByGroupIdSql, new { equipmentGroupId });
            });

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
            sqlBuilder.LeftJoin("inte_work_center IWC ON IWC.Id = EE.WorkCenterShopId");
            sqlBuilder.Where("EE.IsDeleted = 0");
            sqlBuilder.Where("EE.SiteId = @SiteId");
            sqlBuilder.OrderBy("EE.UpdatedOn DESC");
            sqlBuilder.Select("EE.*");

            if (pagedQuery.EquipmentType.HasValue)
            {
                sqlBuilder.Where("EE.EquipmentType = @EquipmentType");
            }

            if (pagedQuery.UseStatus.HasValue)
            {
                sqlBuilder.Where("EE.UseStatus = @UseStatus");
            }

            if (pagedQuery.UseDepartment.HasValue)
            {
                sqlBuilder.Where("EE.UseDepartment = @UseDepartment");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where("EE.EquipmentCode LIKE @EquipmentCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentName))
            {
                pagedQuery.EquipmentName = $"%{pagedQuery.EquipmentName}%";
                sqlBuilder.Where("EE.EquipmentName LIKE @EquipmentName");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.WorkCenterShopName))
            {
                pagedQuery.WorkCenterShopName = $"%{pagedQuery.WorkCenterShopName}%";
                sqlBuilder.Where("IWC.Name LIKE @WorkCenterShopName");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Location))
            {
                pagedQuery.Location = $"%{pagedQuery.Location}%";
                sqlBuilder.Where("EE.Location LIKE @Location");
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
        const string DeleteSql = "UPDATE `equ_equipment` SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string GetByCodeSql = "SELECT * FROM equ_equipment WHERE `IsDeleted` = 0 AND SiteId = @Site AND EquipmentCode = @Code LIMIT 1";
        const string GetByIdSql = "SELECT * FROM `equ_equipment` WHERE `Id` = @Id;";
        const string GetByGroupIdSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0 AND EquipmentGroupId = @EquipmentGroupId;";
        const string GetBaseListSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0;";
        const string GetByEquipmentCodeSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0 AND EquipmentCode = @EquipmentCode;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM equ_equipment EE /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_equipment EE /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetEntitiesSqlTemplate = "";
        /// <summary>
        /// 
        /// </summary>
        const string UpdateEquipmentGroupIdSql = "UPDATE `equ_equipment` SET EquipmentGroupId = @EquipmentGroupId WHERE Id = @EquipmentIds ";
        const string ClearEquipmentGroupIdSql = "UPDATE `equ_equipment` SET EquipmentGroupId = 0 WHERE EquipmentGroupId = @equipmentGroupId ";
    }
}
