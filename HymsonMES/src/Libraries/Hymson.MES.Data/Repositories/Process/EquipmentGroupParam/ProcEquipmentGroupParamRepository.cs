using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using IdGen;
using Microsoft.Extensions.Options;
using System.Net.NetworkInformation;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组仓储
    /// </summary>
    public partial class ProcEquipmentGroupParamRepository :BaseRepository, IProcEquipmentGroupParamRepository
    {

        public ProcEquipmentGroupParamRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
        {
        }

        #region 方法
        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ProcEquipmentGroupParamEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcEquipmentGroupParamEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcEquipmentGroupParamEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcEquipmentGroupParamEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcEquipmentGroupParamEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEquipmentGroupParamView>> GetPagedInfoAsync(ProcEquipmentGroupParamPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("egp.IsDeleted=0");
            sqlBuilder.Where("egp.SiteId=@SiteId");

            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("egp.Code LIKE @Code");
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                query.Name = $"%{query.Name}%";
                sqlBuilder.Where("egp.Name LIKE @Name");
            }

            if (!string.IsNullOrWhiteSpace(query.MaterialCode))
            {
                query.MaterialCode = $"%{query.MaterialCode}%";
                sqlBuilder.Where("m.MaterialCode LIKE @MaterialCode");
            }

            if (!string.IsNullOrWhiteSpace(query.ProcedureCode))
            {
                query.ProcedureCode = $"%{query.ProcedureCode}%";
                sqlBuilder.Where("p.Code LIKE @ProcedureCode");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var procEquipmentGroupParamEntitiesTask = conn.QueryAsync<ProcEquipmentGroupParamView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procEquipmentGroupParamEntities = await procEquipmentGroupParamEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcEquipmentGroupParamView>(procEquipmentGroupParamEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procEquipmentGroupParamQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetProcEquipmentGroupParamEntitiesAsync(ProcEquipmentGroupParamQuery procEquipmentGroupParamQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcEquipmentGroupParamEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId=@SiteId");
            if (procEquipmentGroupParamQuery.ProductId.HasValue&& procEquipmentGroupParamQuery.ProductId>0) 
            {
                sqlBuilder.Where("ProductId=@ProductId");
            }
            if (procEquipmentGroupParamQuery.ProcedureId.HasValue && procEquipmentGroupParamQuery.ProcedureId > 0)
            {
                sqlBuilder.Where("ProcedureId=@ProcedureId");
            }

            using var conn = GetMESDbConnection();
            var procEquipmentGroupParamEntities = await conn.QueryAsync<ProcEquipmentGroupParamEntity>(template.RawSql, procEquipmentGroupParamQuery);
            return procEquipmentGroupParamEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procEquipmentGroupParamEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcEquipmentGroupParamEntity procEquipmentGroupParamEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procEquipmentGroupParamEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procEquipmentGroupParamEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcEquipmentGroupParamEntity> procEquipmentGroupParamEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, procEquipmentGroupParamEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procEquipmentGroupParamEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcEquipmentGroupParamEntity procEquipmentGroupParamEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procEquipmentGroupParamEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procEquipmentGroupParamEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcEquipmentGroupParamEntity> procEquipmentGroupParamEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, procEquipmentGroupParamEntitys);
        }
        #endregion

        /// <summary>
        /// 根据关联信息（产品，工序，工艺组）获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamEntity>> GetByRelatesInformationAsync(ProcEquipmentGroupParamRelatesInformationQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcEquipmentGroupParamEntity>(GetByRelatesInformationSql, query);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        #region 顷刻

        /// <summary>
        /// 根据设备ID和产品型号查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProcEquipmentGroupParamEquProductView>> QueryByEquProductAsync(ProcEquipmentGroupParamEquProductQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetByEquProductSqlTemplate);
            if (string.IsNullOrEmpty(query.ProductCode) == false)
            {
                sqlBuilder.Where("t2.MaterialCode = @ProductCode");
            }
            sqlBuilder.Where("t1.`Type` = @Type");
            sqlBuilder.Where("t4.Id = @EquipmentId");
            sqlBuilder.Where("t1.Status in ('1', '2')");
            sqlBuilder.Where("t1.IsDeleted = 0");

            using var conn = GetMESDbConnection();
            var list = await conn.QueryAsync<ProcEquipmentGroupParamEquProductView>(template.RawSql, query);
            return list.ToList();
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ProcEquipmentGroupParamRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                                                        egp.* ,m.materialCode as materialCode,m.materialName as materialName, m.Version as  MaterialVersion, 
                                                        p.Code as ProcedureCode , p.Name as ProcedureName 
                                                     FROM `proc_equipment_group_param` egp 
                                                     LEFT JOIN proc_material  m ON  m.id=egp.ProductId
                                                     LEFT JOIN proc_procedure p ON  p.id=egp.ProcedureId
                                                    /**where**/ 
                                                    ORDER BY egp.UpdatedOn DESC
                                                    LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(*) FROM `proc_equipment_group_param` egp   
                                                      LEFT JOIN proc_material  m ON  m.id=egp.ProductId 
                                                      LEFT JOIN proc_procedure p ON  p.id=egp.ProcedureId
                                                     /**where**/ ";
        const string GetProcEquipmentGroupParamEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_equipment_group_param` /**where**/  ";

        const string InsertSql = "INSERT IGNORE `proc_equipment_group_param`(  `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `ProcedureId`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @Code, @Name, @Type, @ProductId, @ProcedureId, @EquipmentGroupId, @Version, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";
        const string InsertsSql = "INSERT IGNORE `proc_equipment_group_param`(  `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `ProcedureId`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @Code, @Name, @Type, @ProductId, @ProcedureId, @EquipmentGroupId, @Version, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";

        const string UpdateSql = "UPDATE `proc_equipment_group_param` SET  Name = @Name, Type = @Type, ProductId = @ProductId, ProcedureId = @ProcedureId, EquipmentGroupId = @EquipmentGroupId, Version = @Version, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_equipment_group_param` SET Name = @Name, Type = @Type, ProductId = @ProductId, ProcedureId = @ProcedureId, EquipmentGroupId = @EquipmentGroupId, Version = @Version, Status = @Status, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_equipment_group_param` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_equipment_group_param` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `ProcedureId`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_equipment_group_param`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `ProcedureId`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_equipment_group_param`  WHERE Id IN @Ids ";
        #endregion

        const string GetByCodeSql = "SELECT * FROM proc_equipment_group_param WHERE IsDeleted = 0 AND SiteId = @Site AND Code = @Code AND Version = @Version LIMIT 1";

        const string GetByRelatesInformationSql = @"SELECT * 
                            FROM `proc_equipment_group_param`  
                            WHERE ProductId = @ProductId 
                            AND ProcedureId = @ProcedureId 
                            AND EquipmentGroupId = @EquipmentGroupId 
                            AND IsDeleted=0 AND SiteId=@SiteId ";

        const string UpdateStatusSql = "UPDATE `proc_equipment_group_param` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        #region 顷刻
        /// <summary>
        /// 根据设备ID和产品型号查询
        /// </summary>
        const string GetByEquProductSqlTemplate = @"
            select t1.Code, t1.Version, t1.ProductId , t2.MaterialCode ,t1.UpdatedOn 
            from proc_equipment_group_param t1
            inner join proc_material t2 on t1.ProductId = t2.Id and t2.IsDeleted = 0
            inner join proc_process_equipment_group_relation t3 on t3.EquipmentGroupId = t1.EquipmentGroupId and t3.IsDeleted = 0
            inner join equ_equipment t4 on t4.Id = t3.EquipmentId and t4.IsDeleted = 0
            /**where**/ 
        ";
        #endregion
    }
}
