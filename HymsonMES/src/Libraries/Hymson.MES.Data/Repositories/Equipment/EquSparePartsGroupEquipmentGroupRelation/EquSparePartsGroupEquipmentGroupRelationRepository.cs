using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquSparePartsGroupEquipmentGroupRelation.Query.View;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（备件类型和设备关联关系表）
    /// </summary>
    public partial class EquSparePartsGroupEquipmentGroupRelationRepository : BaseRepository, IEquSparePartsGroupEquipmentGroupRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquSparePartsGroupEquipmentGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSparePartsGroupEquipmentGroupRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 获取设备组关联备件维护
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SparePartsEquipmentGroupRelationView>> GetSparePartsEquipmentGroupRelationAsync(long Id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<SparePartsEquipmentGroupRelationView>(GetSparePartsEquipmentGroupRelationSqlTemplate, new { Id = Id });
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquSparePartsGroupEquipmentGroupRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparePartsGroupEquipmentGroupRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquSparePartsGroupEquipmentGroupRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparePartsGroupEquipmentGroupRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparePartsGroupEquipmentGroupRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsGroupEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsGroupEquipmentGroupRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsGroupEquipmentGroupRelationEntity>> GetEntitiesAsync(EquSparePartsGroupEquipmentGroupRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartsGroupEquipmentGroupRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsGroupEquipmentGroupRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartsGroupEquipmentGroupRelationEntity>> GetPagedInfoAsync(EquSparePartsGroupEquipmentGroupRelationPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
           
            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquSparePartsGroupEquipmentGroupRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSparePartsGroupEquipmentGroupRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquSparePartsGroupEquipmentGroupRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spare_parts_group_equipment_group_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spare_parts_group_equipment_group_relation` /**where**/ ";
        const string GetEquSparePartsGroupEquipmentGroupRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spare_parts_group_equipment_group_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spare_parts_group_equipment_group_relation`(  `Id`, `SiteId`, `SparePartsGroupId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) VALUES (   @Id, @SiteId, @SparePartsGroupId, @EquipmentGroupId, @CreatedBy, @CreatedOn )  ";
        const string InsertsSql = "INSERT INTO `equ_spare_parts_group_equipment_group_relation`(  `Id`, `SiteId`, `SparePartsGroupId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) VALUES (   @Id, @SiteId, @SparePartsGroupId, @EquipmentGroupId, @CreatedBy, @CreatedOn )  ";

        const string UpdateSql = "UPDATE `equ_spare_parts_group_equipment_group_relation` SET   SiteId = @SiteId, SparePartsGroupId = @SparePartsGroupId, EquipmentGroupId = @EquipmentGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spare_parts_group_equipment_group_relation` SET   SiteId = @SiteId, SparePartsGroupId = @SparePartsGroupId, EquipmentGroupId = @EquipmentGroupId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_spare_parts_group_equipment_group_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spare_parts_group_equipment_group_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `equ_spare_parts_group_equipment_group_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `equ_spare_parts_group_equipment_group_relation`  WHERE Id IN @Ids ";

        const string GetSparePartsEquipmentGroupRelationSqlTemplate = @"SELECT
	                                                                        ESPGEGR.Id,
	                                                                        ESPGEGR.SparePartsGroupId,
	                                                                        ESPGEGR.EquipmentGroupId,
	                                                                        ESPGEGR.CreatedBy,
	                                                                        ESPGEGR.CreatedOn
                                                                        FROM
	                                                                        equ_spare_parts_group ESPG
                                                                         JOIN equ_spare_parts_group_equipment_group_relation ESPGEGR ON ESPGEGR.SparePartsGroupId = ESPG.Id 
                                                                        WHERE
	                                                                        QUC.Id = @Id 
	                                                                        AND ESPG.IsDeleted = 0";

    }
}
