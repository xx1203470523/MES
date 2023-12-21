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
        /// <param name="equFaultPhenomenonEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquFaultPhenomenonEntity equFaultPhenomenonEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equFaultPhenomenonEntity);
        }

        /// <summary>
        /// 新增（设备故障现象和原因关系）
        /// </summary>
        /// <param name="equFaultReasonPhenomenonEntities"></param>
        /// <returns></returns>
        public async Task<int> InsertFaultReasonAsync(IEnumerable<EquFaultPhenomenonReasonRelationEntity> equFaultReasonPhenomenonEntities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertFaultReasonPhenomenonRelationSql, equFaultReasonPhenomenonEntities);
        }

        /// <summary>
        /// 更新（设备故障现象）
        /// </summary>
        /// <param name="equFaultPhenomenonEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquFaultPhenomenonEntity equFaultPhenomenonEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equFaultPhenomenonEntity);
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
        /// 删除设备故障原因关系（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquFaultReasonPhenomenonRelationsAsync(DeleteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteEquFaultReasonPhenomenonRelationsSql, command);

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
        public async Task<IEnumerable<EquFaultPhenomenonEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquFaultPhenomenonEntity>(GetByIdsSql, new { ids });
        }

        /// <summary>
        /// 获取已经分配设备故障原因
        /// </summary>
        /// <param name="equFaultPhenomenonQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquFaultPhenomenonReasonRelationEntity>> GetEquFaultReasonListAsync(EquFaultPhenomenonQuery equFaultPhenomenonQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetEquFaultReasonsSql);

            //sqlBuilder.Where("IsDeleted = 0");
            //sqlBuilder.Where("SiteId = @SiteId");

            if (equFaultPhenomenonQuery.Id.HasValue)
            {
                sqlBuilder.Where("FaultPhenomenonId = @Id");
            }

            sqlBuilder.AddParameters(equFaultPhenomenonQuery);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquFaultPhenomenonReasonRelationEntity>(templateData.RawSql, templateData.Parameters);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquFaultPhenomenonRepository
    {
        #region 新增

        const string InsertSql = "INSERT INTO `equ_fault_phenomenon`( `Id`, `Code`, `Name`, `EquipmentGroupId`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @EquipmentGroupId, @Status, @Remark,@CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        // 新增故障现象和原因关系
        const string InsertFaultReasonPhenomenonRelationSql = "INSERT INTO `equ_fault_reason_phenomenon_relation`( `Id`, `FaultReasonId`, `FaultPhenomenonId`,  `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`) VALUES (   @Id, @FaultReasonId, @FaultPhenomenonId,  @CreatedBy, @CreatedOn,@UpdatedBy, @UpdatedOn)";

        #endregion

        #region 修改

        const string UpdateSql = "UPDATE `equ_fault_phenomenon` SET Name = @Name, EquipmentGroupId = @EquipmentGroupId, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, Remark = @Remark WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE equ_fault_phenomenon SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = UpdatedOn WHERE Id = @Id; ";

        const string DeleteSql = "UPDATE `equ_fault_phenomenon` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE IsDeleted = 0 AND Id IN @Ids;";
        const string DeleteEquFaultReasonPhenomenonRelationsSql = "DELETE FROM equ_fault_reason_phenomenon_relation WHERE FaultPhenomenonId in @Ids;";

        #endregion

        #region 查询
        const string GetByIdSql = @"SELECT * FROM `equ_fault_phenomenon`  WHERE Id = @Id ";
        const string GetViewById = @"SELECT 
                EFP.Id, EFP.Code, EFP.Name, EFP.EquipmentGroupId, EFP.Status, EFP.CreatedBy, EFP.CreatedOn, EFP.UpdatedBy, EFP.UpdatedOn, EEG.EquipmentGroupName,EFP.Remark 
                FROM equ_fault_phenomenon EFP LEFT JOIN equ_equipment_group EEG ON EFP.EquipmentGroupId = EEG.Id 
                WHERE EFP.Id = @Id ";
        const string GetByCodeSql = "SELECT * FROM equ_fault_phenomenon WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_fault_phenomenon EFP /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_fault_phenomenon EFP /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string GetByIdsSql = @"SELECT * FROM equ_fault_phenomenon WHERE IsDeleted = 0 AND Id IN @ids ";

        const string GetEquFaultReasonsSql = @"SELECT * FROM equ_fault_reason_phenomenon_relation /**where**/ ";
        #endregion
    }
}
