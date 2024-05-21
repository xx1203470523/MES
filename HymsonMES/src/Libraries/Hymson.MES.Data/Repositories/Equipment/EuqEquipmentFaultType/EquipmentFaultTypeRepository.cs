using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 设备故障类型仓储
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public partial class EquipmentFaultTypeRepository : BaseRepository, IEquipmentFaultTypeRepository
    {
        public EquipmentFaultTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 设备故障类型
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentFaultTypeEntity>> GetPagedInfoAsync(EquipmentFaultTypePagedQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (string.IsNullOrEmpty(param.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(param.Sorting);
            }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                param.Code = $"%{param.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                param.Name = $"%{param.Name}%";
                sqlBuilder.Where("Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(param.Status.ToString()))
            {
                //param.Status = $"%{param.Status}%";
                sqlBuilder.Where("Status like @Status");
            }

            var offSet = (param.PageIndex - 1) * param.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = param.PageSize });
            sqlBuilder.AddParameters(param);

            using var conn = GetMESDbConnection();
            var qualUnqualifiedGroupEntitiesTask = conn.QueryAsync<EquEquipmentFaultTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualUnqualifiedGroupEntities = await qualUnqualifiedGroupEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquEquipmentFaultTypeEntity>(qualUnqualifiedGroupEntities, param.PageIndex, param.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentFaultTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentFaultTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcPrintSetupEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcPrintSetupEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 新增（设备故障类型表）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquEquipmentFaultTypeEntity param)
        {
            try
            {
                using var conn = GetMESDbConnection();
                return await conn.ExecuteAsync(InsertSql, param);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquEquipmentFaultTypeEntity param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, param);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquEquipmentFaultTypeEntity> GetByCodeAsync(QualUnqualifiedGroupByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentFaultTypeEntity>(GetByCodeSql, param);
        }
        #endregion

        #region 设备故障类型关联故障现象
        /// <summary>
        /// 插入设备故障类型与现象关系
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertQualUnqualifiedCodeGroupRelationRangAsync(List<EquipmentFaultTypesPhenomenonRelation> param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertQualUnqualifiedCodeGroupRelationSql, param);
        }

        /// <summary>
        /// 删除设备故障类型关联现象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DelteQualUnqualifiedCodeGroupRelationSql, new { UnqualifiedGroupId = id });
        }
        #endregion

        #region 设备故障类型关联设备组
        /// <summary>
        /// 插入设备故障类型关联设备组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertQualUnqualifiedGroupProcedureRelationRangAsync(List<EQualUnqualifiedGroupProcedureRelation> param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertQualUnqualifiedGroupProcedureRelationSql, param);
        }

        /// <summary>
        /// 删除设备故障类型关联设备组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(RealDelteQualUnqualifiedGroupProcedureRelationSql, new { UnqualifiedGroupId = id });
        }
        #endregion    

        /// <summary>
        /// 获取设备故障类型关联（现象表）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentFaultPhenomenonRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquipmentFaultPhenomenonRelationView>(GetQualUnqualifiedCodeGroupRelationSqlTemplate, new { Id = id });
        }

        /// <summary>
        /// 获取设备故障类型关联（设备组关系表）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquipmentFaultEquipmentGroupRelationView>> GetQualUnqualifiedCodeProcedureRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquipmentFaultEquipmentGroupRelationView>(GetQualUnqualifiedCodeProcedureRelationSqlTemplate, new { Id = id });
        }
    }

    public partial class EquipmentFaultTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_fault_type` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_fault_type` /**where**/ ";
        const string GetByIdSql = @"SELECT 
                               Id, SiteId, Code, Name, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted, Status
                                FROM `equ_fault_type`  WHERE Id = @Id  AND IsDeleted=0 ";
        const string GetByCodeSql = @"SELECT 
                              Id, SiteId, Code, Name, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                                FROM `equ_fault_type`  WHERE Code = @Code AND SiteId = @Site  AND IsDeleted = 0";
        const string GetQualUnqualifiedCodeProcedureRelationSqlTemplate = @"SELECT  EEG.Id,EFTEGR.FaultTypeId, EFTEGR.EquipmentGroupId, EFTEGR.CreatedBy, EFTEGR.CreatedOn, EEG.EquipmentGroupCode AS ProcedureCode, EEG.EquipmentGroupName AS UnqualifiedCodeName
                                                                            FROM equ_fault_type_equipment_group_relation EFTEGR LEFT JOIN equ_equipment_group EEG ON EFTEGR.EquipmentGroupId = EEG.Id  AND EEG.IsDeleted = 0 WHERE EFTEGR.FaultTypeId = @Id";

        const string GetQualUnqualifiedCodeGroupRelationSqlTemplate = @"SELECT  EFP.Id, EFP.`Code`, EFP.`Name`, EFP.CreatedBy, EFP.CreatedOn, EFP.UpdatedBy, EFP.UpdatedOn
                                                                        FROM equ_fault_type_phenomenon_relation EFTPR    LEFT JOIN   equ_fault_phenomenon EFP on EFP.Id = EFTPR. FaultPhenomenonId AND EFP.IsDeleted = 0 
                                                                        WHERE EFTPR.FaultTypeId=@Id";
        const string InsertSql = "INSERT INTO `equ_fault_type`(  `Id`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )    ";
        const string InsertQualUnqualifiedCodeGroupRelationSql = @"INSERT INTO `equ_fault_type_phenomenon_relation`(`FaultTypeId`, `FaultPhenomenonId`, `CreatedBy`, `CreatedOn`) 
                                                                   VALUES (@FaultTypeId, @FaultPhenomenonId, @CreatedBy, @CreatedOn)";
        const string InsertQualUnqualifiedGroupProcedureRelationSql = @"INSERT INTO `equ_fault_type_equipment_group_relation`(`FaultTypeId`, `EquipmentGroupId`, `CreatedBy`, `CreatedOn`) 
                                                                        VALUES(@FaultTypeId, @EquipmentGroupId, @CreatedBy, @CreatedOn) ";
        const string UpdateSql = "UPDATE `equ_fault_type`  SET     Status = @Status, Name = @Name, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_fault_type` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids AND IsDeleted=0 ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_fault_type`  WHERE Id IN @ids ";
        const string DelteQualUnqualifiedCodeGroupRelationSql = "DELETE  FROM equ_fault_type_phenomenon_relation WHERE  FaultTypeId = @UnqualifiedGroupId ";
        const string RealDelteQualUnqualifiedGroupProcedureRelationSql = "DELETE  FROM equ_fault_type_equipment_group_relation WHERE  FaultTypeId = @UnqualifiedGroupId ";
    }
}
