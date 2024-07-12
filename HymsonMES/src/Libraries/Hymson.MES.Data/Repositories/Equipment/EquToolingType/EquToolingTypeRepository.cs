using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（工具类型）
    /// </summary>
    public partial class EquToolingTypeRepository : BaseRepository, IEquToolingTypeGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolingTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolingTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquToolingTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquToolingTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquToolingTypeEntity> entities)
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
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsyncRelation(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            //删除关联物料
            await conn.ExecuteAsync(DeleteMaterialSql, command);
            //删除关联设备组
            return await conn.ExecuteAsync(DeleteEquipmentGroupSql, command);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquToolingTypeEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolingTypeEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolingTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolingTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolingTypeEntity>> GetByIdsAsync(IEnumerable<long>  ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolingTypeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolingTypeEntity>> GetEntitiesAsync(EquToolingTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartsGroupEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.Codes!=null&& query.Codes.Any())
            {
                sqlBuilder.Where("Code IN @Codes");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolingTypeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolingTypeEntity>> GetPagedInfoAsync(EquToolingTypePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }
            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (pagedQuery.UpdatedOn != null && pagedQuery.UpdatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = pagedQuery.UpdatedOn[0], EndTime = pagedQuery.UpdatedOn[1].AddDays(1) });
                sqlBuilder.Where("UpdatedOn >= @StartTime AND UpdatedOn < @EndTime");
            }
            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquToolingTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolingTypeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquToolingTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_tooling_type_manage` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_tooling_type_manage` /**where**/ ";
        const string GetEquSparePartsGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_tooling_type_manage` /**where**/  ";
            
        const string InsertSql = "INSERT INTO `equ_tooling_type_manage`(  `Id`, `Code`, `Name`, `Status`, `Calibration`, `LifeSpan`, `Cycle`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Calibration, @LifeSpan, @Cycle,  @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `equ_tooling_type_manage`(  `Id`, `Code`, `Name`, `Status`, `Calibration`, `LifeSpan`, `Cycle`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Calibration, @LifeSpan, @Cycle,  @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        //更新equ_tools_type_equipment_group_relation 工具类型和设备组关系表
        const string UpdateEquipmentSql = "UPDATE `equ_tools_type_equipment_group_relation` SET   ToolTypeId = @ToolTypeId  WHERE Id = @Id ";
        //更新equ_tools_type_material_relation 工具类型和物料关系表     
        const string UpdateMaterialSql = "UPDATE `equ_sparepart_type` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        //更新equ_tooling_type_manage 工具类型管理
        const string UpdateSql = "UPDATE `equ_tooling_type_manage` SET   Code = @Code, Name = @Name, Status = @Status, Calibration = @Calibration, LifeSpan = @LifeSpan, Cycle = @Cycle, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_tooling_type_manage` SET   Code = @Code, Name = @Name, Status = @Status, Calibration = @Calibration, LifeSpan = @LifeSpan, Cycle = @Cycle, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_tooling_type_manage` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string DeletesSql = "UPDATE `equ_tooling_type_manage` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string DeleteEquipmentGroupSql = "DELETE FROM equ_tools_type_equipment_group_relation WHERE ToolTypeId IN @Ids";
        const string DeleteMaterialSql = "DELETE FROM equ_tools_type_material_relation WHERE ToolTypeId IN @Ids";

        const string GetByCodeSql = "SELECT * FROM `equ_tooling_type_manage` WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetByIdSql = @"SELECT * FROM `equ_tooling_type_manage`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `equ_tooling_type_manage`  WHERE Id IN @Ids ";

    }
}
