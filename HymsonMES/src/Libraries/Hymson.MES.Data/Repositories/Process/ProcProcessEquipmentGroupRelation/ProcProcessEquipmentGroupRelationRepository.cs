using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.ProcProcessEquipmentGroupRelation.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Microsoft.Extensions.Options;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 仓储（设备组关联设备表）
    /// </summary>
    public partial class ProcProcessEquipmentGroupRelationRepository : BaseRepository, IProcProcessEquipmentGroupRelationRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcProcessEquipmentGroupRelationRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProcessEquipmentGroupRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ProcProcessEquipmentGroupRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProcessEquipmentGroupRelationEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ProcProcessEquipmentGroupRelationEntity> entities)
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
        /// 通过ProcEquipmentGroupId删除
        /// </summary>
        /// <param name="equipmentGroupId"></param>
        /// <returns></returns>
        public async Task<int> DeleteByProcEquIdAsync(long equipmentGroupId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByEquIdSql, new { EquipmentGroupId = equipmentGroupId });
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
        public async Task<ProcProcessEquipmentGroupRelationEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProcessEquipmentGroupRelationEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessEquipmentGroupRelationEntity>> GetByGroupIdAsync(ProcProcessEquipmentGroupIdQuery param)
        {
            using var conn = GetMESDbConnection();
            var dto = conn.QueryAsync<ProcProcessEquipmentGroupRelationEntity>(GetByGroupIdSql, param);
            return await dto;
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessEquipmentGroupRelationEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcessEquipmentGroupRelationEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessEquipmentGroupRelationEntity>> GetEntitiesAsync(ProcProcessEquipmentGroupRelationQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProcessEquipmentGroupRelationEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProcessEquipmentGroupRelationEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessEquipmentGroupRelationEntity>> GetPagedInfoAsync(ProcProcessEquipmentGroupRelationPagedQuery pagedQuery)
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
            var entitiesTask = conn.QueryAsync<ProcProcessEquipmentGroupRelationEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProcessEquipmentGroupRelationEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class ProcProcessEquipmentGroupRelationRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_process_equipment_group_relation` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_process_equipment_group_relation` /**where**/ ";
        const string GetProcProcessEquipmentGroupRelationEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_process_equipment_group_relation` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_process_equipment_group_relation`(  `Id`, `EquipmentGroupId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @EquipmentGroupId, @EquipmentId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `proc_process_equipment_group_relation`(  `Id`, `EquipmentGroupId`, `EquipmentId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @EquipmentGroupId, @EquipmentId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `proc_process_equipment_group_relation` SET   EquipmentGroupId = @EquipmentGroupId, EquipmentId = @EquipmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_process_equipment_group_relation` SET   EquipmentGroupId = @EquipmentGroupId, EquipmentId = @EquipmentId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_process_equipment_group_relation` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_process_equipment_group_relation` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE EquipmentGroupId IN @Ids";

        const string DeleteByEquIdSql = "UPDATE `proc_process_equipment_group_relation` SET IsDeleted = Id WHERE EquipmentGroupId = @ProcessEquipmentGroupId ";

        const string GetByIdSql = @"SELECT * FROM `proc_process_equipment_group_relation`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `proc_process_equipment_group_relation`  WHERE Id IN @Ids ";

        const string GetByGroupIdSql = "SELECT EE.Id AS EquipmentId, PPEG.EquipmentGroupId FROM equ_equipment EE " +
            "LEFT JOIN proc_process_equipment_group_relation PPEG ON PPEG.EquipmentId = EE.Id AND PPEG.SiteId = @SiteId AND PPEG.IsDeleted = 0 " +
            "WHERE EE.IsDeleted = 0 AND EE.SiteId = @SiteId " +
            "AND (PPEG.EquipmentGroupId IS NULL OR PPEG.EquipmentGroupId = @ProcessEquipmentGroupId);";

    }
}
