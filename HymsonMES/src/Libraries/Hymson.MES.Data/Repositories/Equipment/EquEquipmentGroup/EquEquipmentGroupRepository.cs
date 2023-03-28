using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup
{
    /// <summary>
    /// 设备组仓储
    /// </summary>
    public partial class EquEquipmentGroupRepository : IEquEquipmentGroupRepository
    {
        private readonly ConnectionOptions _connectionOptions;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public EquEquipmentGroupRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentGroupEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentGroupEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { IsDeleted = 1, Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentGroupEntity> GetByIdAsync(long id)
        {
            var key = $"equ_equipment_group_{id}";
            return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            {
                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                return await conn.QueryFirstOrDefaultAsync<EquEquipmentGroupEntity>(GetByIdSql, new { IsDeleted = 0, Id = id });
            });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentGroupEntity>> GetPagedListAsync(EquEquipmentGroupPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Select("*");

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupCode) == false)
            {
                pagedQuery.EquipmentGroupCode = $"%{pagedQuery.EquipmentGroupCode}%";
                sqlBuilder.Where("EquipmentGroupCode LIKE @EquipmentGroupCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupName) == false)
            {
                pagedQuery.EquipmentGroupName = $"%{pagedQuery.EquipmentGroupName}%";
                sqlBuilder.Where("EquipmentGroupName LIKE @EquipmentGroupName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquEquipmentGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentGroupEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquEquipmentGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_equipment_group` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_equipment_group` /**innerjoin**/ /**leftjoin**/ /**where**/";

        const string InsertSql = "INSERT INTO `equ_equipment_group`(  `Id`, `EquipmentGroupCode`, `EquipmentGroupName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`) VALUES (   @Id, @EquipmentGroupCode, @EquipmentGroupName, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteId )  ";
        const string UpdateSql = "UPDATE `equ_equipment_group` SET EquipmentGroupName = @EquipmentGroupName, Remark = @Remark WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_equipment_group` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string GetByIdSql = @"SELECT 
                               `Id`, `EquipmentGroupCode`, `EquipmentGroupName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteId`
                            FROM `equ_equipment_group`  WHERE IsDeleted = @IsDeleted AND Id = @Id ";
    }
}
