using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
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
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { id = idsArr });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentCode"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(string equipmentCode)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefault(ExistsSql, new { equipmentCode }) != null;
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
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentEntity>> GetByGroupIdAsync(long groupId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<EquEquipmentEntity>(GetByIdSql, new { groupId });
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
            sqlBuilder.Select("*");

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode) == false)
            {
                sqlBuilder.Where("EquipmentCode = @EquipmentCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentName) == false)
            {
                sqlBuilder.Where("EquipmentName = @EquipmentName");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentType) == false)
            {
                sqlBuilder.Where("EquipmentType = @EquipmentType");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.UseStatus) == false)
            {
                sqlBuilder.Where("UseStatus = @UseStatus");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.WorkCenterShopName) == false)
            {
                sqlBuilder.Where("WorkCenterShopName = @WorkCenterShopName");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.UseDepartment) == false)
            {
                sqlBuilder.Where("UseDepartment = @UseDepartment");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.Location) == false)
            {
                sqlBuilder.Where("Location = @Location");
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
        const string InsertSql = "INSERT INTO `equ_equipment`(`Id`, `SiteCode`, `UnitCode`, `UnitName`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteCode, @UnitCode, @UnitName, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `equ_equipment` SET UnitName = @UnitName, Type = @Type, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `equ_equipment` SET `IsDeleted` = 1 WHERE `Id` = @Id;";
        const string ExistsSql = "SELECT Id FROM equ_equipment WHERE `IsDeleted` = 0 AND EquipmentCode = @EquipmentCode LIMIT 1";
        const string GetByIdSql = "SELECT * FROM `equ_equipment` WHERE `Id` = @Id;";
        const string GetBaseListSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0;";
        const string GetByEquipmentCodeSql = "SELECT * FROM `equ_equipment` WHERE `IsDeleted` = 0 AND EquipmentCode = @EquipmentCode;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_equipment` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment` /**where**/";
        const string GetEntitiesSqlTemplate = "";
    }
}
