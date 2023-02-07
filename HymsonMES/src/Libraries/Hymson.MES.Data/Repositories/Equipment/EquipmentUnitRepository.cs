using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.IEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentUnitRepository : IEquipmentUnitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquipmentUnitRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(EquipmentUnitEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, entity);
            entity.Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquipmentUnitEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquipmentUnitEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquipmentUnitEntity>(GetByIdSql, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquipmentUnitEntity>> GetPagedInfoAsync(EquipmentUnitPagedQuery equipmentUnitPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(equipmentUnitPagedQuery.SiteCode))
            {
                sqlBuilder.Where("SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(equipmentUnitPagedQuery.UnitCode))
            {
                sqlBuilder.Where("UnitCode=@UnitCode");
            }
            //if (equipmentUnitPagedQuery.ChangeType.HasValue)
            //{
            //    sqlBuilder.Where("ChangeType=@ChangeType");
            //}
            var offSet = (equipmentUnitPagedQuery.PageIndex - 1) * equipmentUnitPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equipmentUnitPagedQuery.PageSize });
            sqlBuilder.AddParameters(equipmentUnitPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);

            /*
            var whStockChangeRecordEntitiesTask = conn.QueryAsync<EquipmentUnitEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var whStockChangeRecordEntities = await whStockChangeRecordEntitiesTask;
            var totalCount = await totalCountTask;
            */

            var equipmentUnitEntities = await conn.QueryAsync<EquipmentUnitEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquipmentUnitEntity>(equipmentUnitEntities, equipmentUnitPagedQuery.PageIndex, equipmentUnitPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentUnitQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentUnitEntity>> GetEntitiesAsync(EquipmentUnitQuery equipmentUnitQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equipmentUnitEntities = await conn.QueryAsync<EquipmentUnitEntity>(template.RawSql, equipmentUnitQuery);
            return equipmentUnitEntities;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquipmentUnitRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `equ_unit`(`Id`, `SiteCode`, `UnitCode`, `UnitName`, `Type`, `Status`, `Remark`, `CreateBy`, `CreateOn`, `UpdateBy`, `UpdateOn`, `IsDeleted`) VALUES (@Id, @SiteCode, @UnitCode, @UnitName, @Type, @Status, @Remark, @CreateBy, @CreateOn, @UpdateBy, @UpdateOn, @IsDeleted);";
        const string UpdateSql = "";
        const string DeleteSql = "";
        const string GetByIdSql = "";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_unit` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_unit` /**where**/";
        const string GetEntitiesSqlTemplate = "";
    }
}
