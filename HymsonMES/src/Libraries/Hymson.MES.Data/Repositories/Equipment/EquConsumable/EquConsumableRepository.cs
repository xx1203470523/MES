using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Equipment.EquConsumable.Command;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Equipment.EquSparePart
{
    /// <summary>
    /// 仓储（工装注册）
    /// </summary>
    public partial class EquConsumableRepository : IEquConsumableRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数（工装注册）
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquConsumableRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增（工装注册）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquConsumableEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新（工装注册）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquConsumableEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 批量修改备件的备件类型
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateConsumableTypeIdAsync(UpdateConsumableTypeIdCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSparePartTypeIdSql, command);
        }

        /// <summary>
        /// 清空备件的指定备件类型
        /// </summary>
        /// <param name="consumableTypeId"></param>
        /// <returns></returns>
        public async Task<int> ClearConsumableTypeIdAsync(long consumableTypeId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ClearSparePartTypeIdSql, new { consumableTypeId });
        }

        /// <summary>
        /// 删除（工装注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（工装注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = idsArr });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquConsumableEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<EquConsumableEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 分页查询（工装注册）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquConsumableEntity>> GetPagedInfoAsync(EquConsumablePagedQuery pagedQuery)
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

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<EquConsumableEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquConsumableEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquConsumableEntity>> GetEntitiesAsync(EquConsumableQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var equSparePartEntities = await conn.QueryAsync<EquConsumableEntity>(template.RawSql, query);
            return equSparePartEntities;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquConsumableRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_consumable` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `equ_consumable` /**where**/";
        const string GetEquSparePartEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_consumable` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_consumable`(  `Id`, `ConsumableCode`, `ConsumableName`, `ConsumableTypeId`, `ProcMaterialId`, `UnitId`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteCode`) VALUES (   @Id, @ConsumableCode, @ConsumableName, @ConsumableTypeId, @ProcMaterialId, @UnitId, @Status, @BluePrintNo, @Brand, @ManagementMode, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteCode )  ";
        const string UpdateSql = "UPDATE `equ_consumable` SET ConsumableCode = @ConsumableCode, ConsumableName = @ConsumableName, ConsumableTypeId = @ConsumableTypeId, ProcMaterialId = @ProcMaterialId, UnitId = @UnitId, Status = @Status, BluePrintNo = @BluePrintNo, Brand = @Brand, ManagementMode = @ManagementMode, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateSparePartTypeIdSql = "UPDATE `equ_consumable` SET SparePartTypeId = @SparePartTypeId  WHERE Id = @Id ";
        const string ClearSparePartTypeIdSql = "UPDATE `equ_consumable` SET SparePartTypeId = 0 WHERE SparePartTypeId = @SparePartTypeId ";
        const string DeleteSql = "UPDATE `equ_consumable` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `ConsumableCode`, `ConsumableName`, `ConsumableTypeId`, `ProcMaterialId`, `UnitId`, `Status`, `BluePrintNo`, `Brand`, `ManagementMode`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteCode`
                            FROM `equ_consumable`  WHERE Id = @Id ";
    }
}
