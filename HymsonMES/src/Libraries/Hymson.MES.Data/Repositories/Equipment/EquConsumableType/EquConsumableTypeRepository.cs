using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquConsumableType.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquConsumableType
{
    /// <summary>
    /// 仓储（工装类型）
    /// </summary>
    public partial class EquConsumableTypeRepository : IEquConsumableTypeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数（工装类型）
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquConsumableTypeRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增（工装类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquConsumableTypeEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新（工装类型）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquConsumableTypeEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（工装类型）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = idsArr });
        }

        /// <summary>
        /// 根据ID获取数据（工装类型）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquConsumableTypeEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquConsumableTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 分页查询（工装类型）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquConsumableTypeEntity>> GetPagedInfoAsync(EquConsumableTypePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");

            if (string.IsNullOrWhiteSpace(pagedQuery.SiteCode) == false)
            {
                sqlBuilder.Where("SiteCode = @SiteCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.ConsumableTypeCode) == false)
            {
                pagedQuery.ConsumableTypeCode = $"%{pagedQuery.ConsumableTypeCode}%";
                sqlBuilder.Where("ConsumableTypeCode LIKE @ConsumableTypeCode");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.ConsumableTypeName) == false)
            {
                pagedQuery.ConsumableTypeName = $"%{pagedQuery.ConsumableTypeName}%";
                sqlBuilder.Where("ConsumableTypeName LIKE @ConsumableTypeName");
            }

            if (pagedQuery.Status > 0)
            {
                sqlBuilder.Where("Status = @Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquConsumableTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<EquConsumableTypeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquConsumableTypeEntity>> GetEquSparePartTypeEntitiesAsync(EquConsumableTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartTypeEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<EquConsumableTypeEntity>(template.RawSql, query);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquConsumableTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_consumable_type` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `equ_consumable_type` /**where**/";
        const string GetEquSparePartTypeEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_consumable_type` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_consumable_type`(  `Id`, `SparePartTypeCode`, `SparePartTypeName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteCode`) VALUES (   @Id, @SparePartTypeCode, @SparePartTypeName, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_consumable_type` SET  SparePartTypeName = @SparePartTypeName, Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `equ_consumable_type` SET IsDeleted = 1 WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `SparePartTypeCode`, `SparePartTypeName`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteCode`
                            FROM `equ_consumable_type` WHERE Id = @Id ";
    }
}
