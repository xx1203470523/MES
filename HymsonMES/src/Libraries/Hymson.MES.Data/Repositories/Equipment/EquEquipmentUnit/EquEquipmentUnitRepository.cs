using Dapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentUnitRepository : IEquEquipmentUnitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquEquipmentUnitRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentUnitEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentUnitEntity entity)
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentUnitEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentUnitEntity>(GetByIdSql, new { IsDeleted = 0, id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentUnitEntity>> GetPagedListAsync(EquEquipmentUnitPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (pagedQuery.Type > DbDefaultValueConstant.IntDefaultValue)
            {
                sqlBuilder.Where("Type = @Type");
            }

            if (pagedQuery.Status > DbDefaultValueConstant.IntDefaultValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.UnitCode) == false)
            {
                pagedQuery.UnitCode = $"%{pagedQuery.UnitCode}%";
                sqlBuilder.Where("UnitCode LIKE @UnitCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.UnitName) == false)
            {
                pagedQuery.UnitName = $"%{pagedQuery.UnitName}%";
                sqlBuilder.Where("UnitName LIKE @UnitName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquEquipmentUnitEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentUnitEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentUnitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `equ_unit`(`Id`, `SiteId`, `UnitCode`, `UnitName`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteId, @UnitCode, @UnitName, @Type, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `equ_unit` SET UnitName = @UnitName, Type = @Type, Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `equ_unit` SET `IsDeleted` = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE `Id` = @Ids;";
        const string GetByIdSql = "SELECT `Id`, `SiteId`, `UnitCode`, `UnitName`, `Type`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn` FROM `equ_unit` WHERE `IsDeleted` = @IsDeleted AND `Id` = @id;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_unit` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_unit` /**where**/";
        const string GetEntitiesSqlTemplate = "";
    }
}
