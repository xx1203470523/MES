using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（设备点检保养项目）
    /// </summary>
    public partial class EquOperationPermissionsRepository : BaseRepository, IEquOperationPermissionsRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquOperationPermissionsRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquOperationPermissionsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquOperationPermissionsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquOperationPermissionsEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquOperationPermissionsEntity> entities)
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
        public async Task<EquOperationPermissionsEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquOperationPermissionsEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquOperationPermissionsEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquOperationPermissionsEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquOperationPermissionsEntity>> GetEntitiesAsync(EquOperationPermissionsQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Where("EquipmentId=@EquipmentId ");
            sqlBuilder.Where("Type=@Type ");
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquOperationPermissionsEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquOperationPermissionsEntity>> GetPagedListAsync(EquOperationPermissionsPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("eop.*");
            sqlBuilder.Select("ee.EquipmentCode");
            sqlBuilder.Select("ee.EquipmentName");
            sqlBuilder.Select("ee.Location");
            sqlBuilder.Select("eg.EquipmentGroupCode");
            sqlBuilder.Select("eg.EquipmentGroupName");
            sqlBuilder.Where("eop.IsDeleted = 0");
            sqlBuilder.Where("eop.SiteId = @SiteId");
            sqlBuilder.OrderBy("eop.UpdatedOn DESC, eop.Id DESC");
            sqlBuilder.LeftJoin("equ_equipment ee on ee.Id=eop.EquipmentId");
            sqlBuilder.LeftJoin("equ_equipment_group eg on eg.Id=ee.EquipmentGroupId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                pagedQuery.EquipmentCode = $"%{pagedQuery.EquipmentCode}%";
                sqlBuilder.Where(" ee.EquipmentCode like @EquipmentCode ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentName))
            {
                pagedQuery.EquipmentName = $"%{pagedQuery.EquipmentName}%";
                sqlBuilder.Where(" ee.EquipmentName like @EquipmentName ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupCode))
            {
                pagedQuery.EquipmentGroupCode = $"%{pagedQuery.EquipmentGroupCode}%";
                sqlBuilder.Where("eg.EquipmentGroupCode LIKE @EquipmentGroupCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupName))
            {
                pagedQuery.EquipmentGroupName = $"%{pagedQuery.EquipmentGroupName}%";
                sqlBuilder.Where("eg.EquipmentGroupName LIKE @EquipmentGroupName");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.ExecutorIds))
            {
                pagedQuery.ExecutorIds = $"%{pagedQuery.ExecutorIds}%";
                sqlBuilder.Where(" eop.ExecutorIds like @ExecutorIds ");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.LeaderIds))
            {
                pagedQuery.LeaderIds = $"%{pagedQuery.LeaderIds}%";
                sqlBuilder.Where(" eop.LeaderIds like @LeaderIds ");
            }
            if (pagedQuery.Status!=null)
            {
                sqlBuilder.Where(" eop.Status like @Status ");
            }
            if (pagedQuery.Type!=null)
            {
                sqlBuilder.Where(" eop.Type like @Type ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquOperationPermissionsEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquOperationPermissionsEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }


    /// <summary>
    /// 设备维保权限
    /// </summary>
    public partial class EquOperationPermissionsRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_operation_permissions eop /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_operation_permissions eop /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_operation_permissions /**where**/  ";

        const string InsertSql = "INSERT INTO equ_operation_permissions(`Id`,`EquipmentId`,`Status`,`Type`,`ExecutorIds`,`LeaderIds`,`Remark`,`CreatedOn`,`CreatedBy`,`UpdatedBy`,`UpdatedOn`,`SiteId`,`IsDeleted`) VALUES (@Id,@EquipmentId,@Status,@Type,@ExecutorIds,@LeaderIds,@Remark,@CreatedOn,@CreatedBy,@UpdatedBy,@UpdatedOn,@SiteId,@IsDeleted) ";
        const string UpdatesSql = "UPDATE equ_operation_permissions SET ExecutorIds=@ExecutorIds,LeaderIds=@LeaderIds,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id=@Id ";

        const string DeleteSql = "UPDATE equ_operation_permissions SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_operation_permissions SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT eop.*,ee.EquipmentCode,ee.EquipmentName,ee.Location FROM equ_operation_permissions eop LEFT JOIN equ_equipment ee on ee.Id=eop.EquipmentId WHERE eop.Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_operation_permissions WHERE Id IN @Ids ";

    }
}
