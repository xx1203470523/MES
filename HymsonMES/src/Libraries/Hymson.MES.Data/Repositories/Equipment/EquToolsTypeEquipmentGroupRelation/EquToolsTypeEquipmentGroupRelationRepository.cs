using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（工具类型和设备组关系表）
    /// </summary>
    public partial class EquToolsTypeEquipmentGroupRelationRepository : BaseRepository, IEquToolsTypeEquipmentGroupRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolsTypeEquipmentGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolsTypeEquipmentGroupRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquToolsTypeEquipmentGroupRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquToolsTypeEquipmentGroupRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquToolsTypeEquipmentGroupRelationEntity> entities)
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
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByToolTypeIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByToolTypeIdSql, new { ToolTypeId = id });
        }

        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteByToolTypeIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByToolTypeIdsSql, new { ToolTypeIds = ids });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolsTypeEquipmentGroupRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolsTypeEquipmentGroupRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsTypeEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsTypeEquipmentGroupRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsTypeEquipmentGroupRelationEntity>> GetEntitiesAsync(EquToolsTypeEquipmentGroupRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Select("*");

            if (query.ToolTypeId.HasValue)
            {
                sqlBuilder.Where("ToolTypeId=@ToolTypeId");
            }

            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsTypeEquipmentGroupRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolsTypeEquipmentGroupRelationEntity>> GetPagedListAsync(EquToolsTypeEquipmentGroupRelationPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquToolsTypeEquipmentGroupRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolsTypeEquipmentGroupRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquToolsTypeEquipmentGroupRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_tools_type_equipment_group_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_tools_type_equipment_group_relation /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_tools_type_equipment_group_relation /**where**/  ";

        const string InsertSql = "INSERT INTO equ_tools_type_equipment_group_relation(  `ToolTypeId`, `EquipmentGroupId`) VALUES (  @ToolTypeId, @EquipmentGroupId) ";
        const string InsertsSql = "INSERT INTO equ_tools_type_equipment_group_relation(  `ToolTypeId`, `EquipmentGroupId`) VALUES (  @ToolTypeId, @EquipmentGroupId) ";

        const string UpdateSql = "UPDATE equ_tools_type_equipment_group_relation SET   ToolTypeId = @ToolTypeId, EquipmentGroupId = @EquipmentGroupId WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_tools_type_equipment_group_relation SET   ToolTypeId = @ToolTypeId, EquipmentGroupId = @EquipmentGroupId WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_tools_type_equipment_group_relation SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_tools_type_equipment_group_relation SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_tools_type_equipment_group_relation WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_tools_type_equipment_group_relation WHERE Id IN @Ids ";

        const string DeleteByToolTypeIdSql = "delete from `equ_tools_type_equipment_group_relation` WHERE ToolTypeId=@ToolTypeId ";
        const string DeleteByToolTypeIdsSql = "delete from `equ_tools_type_equipment_group_relation` WHERE ToolTypeId in @ToolTypeIds ";
    }
}
