using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup.Query;
using Microsoft.Extensions.Options;
using static Dapper.SqlMapper;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipmentGroup
{
    /// <summary>
    /// 设备组仓储
    /// </summary>
    public partial class EquEquipmentGroupRepository : BaseRepository, IEquEquipmentGroupRepository
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public EquEquipmentGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { IsDeleted = 1, Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentGroupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentGroupEntity>(GetByIdSql, new { IsDeleted = 0, Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentGroupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentGroupEntity>(GetByIdsSql, new { ids = ids });
        }


        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentGroupEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentGroupEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentGroupEntity>> GetByCodeOrNameAsync(EntityByCodeQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = " ";
            }
            if (string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = " ";
            }
            query.Code = $"%{query.Code}%";
            query.Name = $"%{query.Name}%";
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentGroupEntity>(GetByCodeOrNameSql, query);
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

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupCode))
            {
                pagedQuery.EquipmentGroupCode = $"%{pagedQuery.EquipmentGroupCode}%";
                sqlBuilder.Where("EquipmentGroupCode LIKE @EquipmentGroupCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupName))
            {
                pagedQuery.EquipmentGroupName = $"%{pagedQuery.EquipmentGroupName}%";
                sqlBuilder.Where("EquipmentGroupName LIKE @EquipmentGroupName");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<EquEquipmentGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<EquEquipmentGroupEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="equipmentGroupQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentGroupEntity>> GetEntitiesAsync(EquEquipmentGroupQuery equipmentGroupQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (equipmentGroupQuery.EquipmentGroupCodes != null && equipmentGroupQuery.EquipmentGroupCodes.Any())
            {
                sqlBuilder.Where(" EquipmentGroupCode in @EquipmentGroupCodes ");
            }
            using var conn = GetMESDbConnection();
            var equipmentEntities = await conn.QueryAsync<EquEquipmentGroupEntity>(template.RawSql, equipmentGroupQuery);
            return equipmentEntities;
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
        const string GetByIdsSql = @"SELECT * FROM `equ_equipment_group`  WHERE Id IN @ids and IsDeleted=0  ";
        const string GetByCodeSql = "SELECT * FROM equ_equipment_group WHERE `IsDeleted` = 0 AND SiteId = @Site AND EquipmentGroupCode = @Code LIMIT 1";
        const string GetByCodeOrNameSql = "SELECT * FROM equ_equipment_group WHERE `IsDeleted` = 0 AND SiteId = @Site AND (EquipmentGroupCode LIKE @Code OR EquipmentGroupName LIKE @Name)";
        const string GetEntitiesSqlTemplate = "SELECT * FROM `equ_equipment_group` /**where**/ ";
    }
}
