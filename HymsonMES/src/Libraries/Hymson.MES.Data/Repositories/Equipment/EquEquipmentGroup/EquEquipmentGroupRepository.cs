using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup
{
    /// <summary>
    /// 设备组仓储
    /// </summary>
    public partial class EquEquipmentGroupRepository : IEquEquipmentGroupRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public EquEquipmentGroupRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equEquipmentGroupEntity"></param>
        /// <returns></returns>
        public async Task InsertAsync(EquEquipmentGroupEntity equEquipmentGroupEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, equEquipmentGroupEntity);
            equEquipmentGroupEntity.Id = id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equEquipmentGroupEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentGroupEntity equEquipmentGroupEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, equEquipmentGroupEntity);
        }
        
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { IsDeleted = 1, Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> SoftDeleteAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { IsDeleted = 1, id = idsArr });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentGroupEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentGroupEntity>(GetByIdSql, new { IsDeleted = 0, Id = id });
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
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupCode) == false)
            {
                sqlBuilder.Where("EquipmentGroupCode = @EquipmentGroupCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupName) == false)
            {
                sqlBuilder.Where("EquipmentGroupName = @EquipmentGroupName");
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

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equEquipmentGroupQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentGroupEntity>> GetEntitiesAsync(EquEquipmentGroupQuery equEquipmentGroupQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquEquipmentGroupEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equEquipmentGroupEntities = await conn.QueryAsync<EquEquipmentGroupEntity>(template.RawSql, equEquipmentGroupQuery);
            return equEquipmentGroupEntities;
        }

    }

    public partial class EquEquipmentGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_equipment_group` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `equ_equipment_group` /**where**/";
        const string GetEquEquipmentGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_equipment_group` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_equipment_group`(  `Id`, `EquipmentGroupCode`, `EquipmentGroupName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`) VALUES (   @Id, @EquipmentGroupCode, @EquipmentGroupName, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_equipment_group` SET   EquipmentGroupCode = @EquipmentGroupCode, EquipmentGroupName = @EquipmentGroupName, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_equipment_group` SET IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `EquipmentGroupCode`, `EquipmentGroupName`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`
                            FROM `equ_equipment_group`  WHERE IsDeleted = @IsDeleted AND Id = @Id ";
    }
}
