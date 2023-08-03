/*
 *creator: Karl
 *
 *describe: 设备参数组 仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

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
        /// <param name="procEquipmentGroupParamPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEquipmentGroupParamEntity>> GetPagedInfoAsync(ProcEquipmentGroupParamPagedQuery procEquipmentGroupParamPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (procEquipmentGroupParamPagedQuery.PageIndex - 1) * procEquipmentGroupParamPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procEquipmentGroupParamPagedQuery.PageSize });
            sqlBuilder.AddParameters(procEquipmentGroupParamPagedQuery);

            using var conn = GetMESDbConnection();
            var procEquipmentGroupParamEntitiesTask = conn.QueryAsync<ProcEquipmentGroupParamEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procEquipmentGroupParamEntities = await procEquipmentGroupParamEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcEquipmentGroupParamEntity>(procEquipmentGroupParamEntities, procEquipmentGroupParamPagedQuery.PageIndex, procEquipmentGroupParamPagedQuery.PageSize, totalCount);
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

    }

    public partial class ProcEquipmentGroupParamRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_equipment_group_param` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_equipment_group_param` /**where**/ ";
        const string GetProcEquipmentGroupParamEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_equipment_group_param` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_equipment_group_param`(  `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `Procedure`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @Code, @Name, @Type, @ProductId, @Procedure, @EquipmentGroupId, @Version, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";
        const string InsertsSql = "INSERT INTO `proc_equipment_group_param`(  `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `Procedure`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`) VALUES (   @Id, @SiteId, @Code, @Name, @Type, @ProductId, @Procedure, @EquipmentGroupId, @Version, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted, @IsUsed )  ";

        const string UpdateSql = "UPDATE `proc_equipment_group_param` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Type = @Type, ProductId = @ProductId, Procedure = @Procedure, EquipmentGroupId = @EquipmentGroupId, Version = @Version, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, IsUsed = @IsUsed  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `proc_equipment_group_param` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Type = @Type, ProductId = @ProductId, Procedure = @Procedure, EquipmentGroupId = @EquipmentGroupId, Version = @Version, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, IsUsed = @IsUsed  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_equipment_group_param` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_equipment_group_param` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `Procedure`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_equipment_group_param`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Code`, `Name`, `Type`, `ProductId`, `Procedure`, `EquipmentGroupId`, `Version`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `IsUsed`
                            FROM `proc_equipment_group_param`  WHERE Id IN @Ids ";
        #endregion
    }
}
