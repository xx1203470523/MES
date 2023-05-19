using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentLinkApi
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentLinkApiRepository : IEquEquipmentLinkApiRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquEquipmentLinkApiRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentLinkApiEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquEquipmentLinkApiEntity> entitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entitys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentLinkApiEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquEquipmentLinkApiEntity> entitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entitys);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(IEnumerable<long> idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(SoftDeleteSql, new { Id = idsArr });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long equipmentId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteByEquipmentIdSql, new { EquipmentId=equipmentId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentIds"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] equipmentIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesByEquipmentIdSql, new { EquipmentIds= equipmentIds });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentLinkApiEntity>(GetByIdSql, new { id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <param name="apiType"></param>
        /// <returns></returns>
        public async Task<EquEquipmentLinkApiEntity> GetByEquipmentIdAsync(long equipmentId, string apiType)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentLinkApiEntity>(GetByEquipmentIdSql, new { equipmentId, apiType });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentLinkApiEntity>> GetListAsync(long equipmentId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<EquEquipmentLinkApiEntity>(GetByEquipmentSql, new { equipmentId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentLinkApiEntity>> GetEntitiesAsync(EquEquipmentLinkApiQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equipmentUnitEntities = await conn.QueryAsync<EquEquipmentLinkApiEntity>(template.RawSql, query);
            return equipmentUnitEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentLinkApiEntity>> GetPagedListAsync(EquEquipmentLinkApiPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquEquipmentLinkApiEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentLinkApiEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentLinkApiRepository
    {
        /// <summary>
        /// 
        /// </summary>
        const string InsertSql = "INSERT INTO `equ_equipment_link_api`(  `Id`, `EquipmentId`, `ApiUrl`, `ApiType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @EquipmentId, @ApiUrl, @ApiType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_equipment_link_api` SET   EquipmentId = @EquipmentId, ApiUrl = @ApiUrl, ApiType = @ApiType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string SoftDeleteSql = "UPDATE `equ_equipment_link_api` SET `IsDeleted` = 1 WHERE `Id` = @Id;";
        const string DeleteByEquipmentIdSql = "DELETE FROM equ_equipment_link_api WHERE EquipmentId=@EquipmentId ;";
        const string DeletesByEquipmentIdSql = "DELETE FROM equ_equipment_link_api WHERE EquipmentId in @EquipmentIds ;";
        const string GetByIdSql = "SELECT * FROM `equ_equipment_link_api` WHERE `Id` = @Id;";
        const string GetByEquipmentIdSql = "SELECT * FROM `equ_equipment_link_api` WHERE `EquipmentId` = @EquipmentId;";
        const string GetByEquipmentSql = "SELECT * FROM `equ_equipment_link_api` WHERE `EquipmentId` = @EquipmentId;";
        const string GetPagedInfoDataSqlTemplate = "SELECT /**select**/ FROM `equ_equipment_link_api` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_link_api` /**innerjoin**/ /**leftjoin**/ /**where**/";
        const string GetEntitiesSqlTemplate = "";
    }
}
