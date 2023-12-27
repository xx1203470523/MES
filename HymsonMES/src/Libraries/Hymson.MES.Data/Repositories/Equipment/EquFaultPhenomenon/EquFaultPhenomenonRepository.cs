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
    /// 仓储（设备故障现象）
    /// </summary>
    public partial class EquFaultPhenomenonRepository : BaseRepository, IEquFaultPhenomenonRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquFaultPhenomenonRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增（设备故障现象）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquFaultPhenomenonEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquFaultPhenomenonEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（设备故障现象）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, command);

        }

        /// <summary>
        /// 分页查询（设备故障现象）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultPhenomenonEntity>> GetPagedListAsync(EquFaultPhenomenonPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("EFP.IsDeleted = 0");
            sqlBuilder.Where("EFP.SiteId = @SiteId");
            sqlBuilder.OrderBy("EFP.UpdatedOn DESC");

            sqlBuilder.Select("EFP.*");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("EFP.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("EFP.Name LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentGroupName))
            {
                pagedQuery.EquipmentGroupName = $"%{pagedQuery.EquipmentGroupName}%";
                sqlBuilder.Where("EEG.EquipmentGroupName LIKE @EquipmentGroupName");
            }

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("EFP.Status = @Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<EquFaultPhenomenonEntity>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<EquFaultPhenomenonEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquFaultPhenomenonEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquFaultPhenomenonEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultPhenomenonEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquFaultPhenomenonEntity>(GetByIdsSql, new { ids });
        }



        /// <summary>
        /// 删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteByParentIdAsync(DeleteByParentIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteByParentIdSql, command);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRelationsAsync(IEnumerable<EquFaultPhenomenonReasonRelationEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertRelationsSql, entities);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultPhenomenonReasonRelationEntity>> GetReasonRelationEntitiesAsync(EntityByParentIdQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetRelationEntitiesSqlTemplate);
            sqlBuilder.Where("FaultPhenomenonId = @ParentId");
            sqlBuilder.Select("*");

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquFaultPhenomenonReasonRelationEntity>(template.RawSql, query);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquFaultPhenomenonRepository
    {
        const string InsertSql = "INSERT INTO `equ_fault_phenomenon`( `Id`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (@Id, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateSql = "UPDATE `equ_fault_phenomenon` SET Name = @Name, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE equ_fault_phenomenon SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = UpdatedOn WHERE Id = @Id; ";
        const string DeleteSql = "UPDATE `equ_fault_phenomenon` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
  

        #region 查询
        const string GetByIdSql = @"SELECT * FROM `equ_fault_phenomenon`  WHERE Id = @Id ";
        const string GetByCodeSql = "SELECT * FROM equ_fault_phenomenon WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_fault_phenomenon EFP /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_fault_phenomenon EFP /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetByIdsSql = @"SELECT * FROM equ_fault_phenomenon WHERE IsDeleted = 0 AND Id IN @ids ";
        #endregion



        const string DeleteByParentIdSql = "DELETE FROM equ_fault_phenomenon_reason_relation WHERE FaultPhenomenonId = @ParentId";
        const string InsertRelationsSql = "INSERT INTO equ_fault_phenomenon_reason_relation (Id, FaultPhenomenonId, FaultReasonId) VALUES (@Id, @FaultPhenomenonId, @FaultReasonId) ";
        const string GetRelationEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_fault_phenomenon_reason_relation /**where**/  ";

    }
}
