/*
 *creator: Karl
 *
 *describe: 设备保养计划 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-20 04:05:45
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquMaintenanceTemplate;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquMaintenancePlan
{
    /// <summary>
    /// 设备保养计划仓储
    /// </summary>
    public partial class EquMaintenancePlanRepository : BaseRepository, IEquMaintenancePlanRepository
    {

        public EquMaintenancePlanRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquMaintenancePlanEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenancePlanEntity>(GetByIdSql, new { Id = id });
        }


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        public async Task<EquMaintenancePlanEntity> GetByCodeAsync(EquMaintenancePlanQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquMaintenancePlanEntity>(GetByCodeSql, param);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenancePlanEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquMaintenancePlanEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="EquMaintenancePlanPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenancePlanEntity>> GetPagedInfoAsync(EquMaintenancePlanPagedQuery EquMaintenancePlanPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("CreatedOn DESC");
            if (!string.IsNullOrWhiteSpace(EquMaintenancePlanPagedQuery.Code))
            {
                EquMaintenancePlanPagedQuery.Code = $"%{EquMaintenancePlanPagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(EquMaintenancePlanPagedQuery.Name))
            {
                EquMaintenancePlanPagedQuery.Name = $"%{EquMaintenancePlanPagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }
            if (!string.IsNullOrWhiteSpace(EquMaintenancePlanPagedQuery.Version))
            {
                EquMaintenancePlanPagedQuery.Version = $"%{EquMaintenancePlanPagedQuery.Version}%";
                sqlBuilder.Where("Version LIKE @Version");
            }
            if (EquMaintenancePlanPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }
            if (EquMaintenancePlanPagedQuery.Type.HasValue)
            {
                sqlBuilder.Where("Type=@Type");
            }
            //if (!string.IsNullOrWhiteSpace(EquMaintenancePlanPagedQuery.EquipmentCode))
            //{
            //    sqlBuilder.Where("EquipmentCode=@EquipmentCode");
            //}
            //if (!string.IsNullOrWhiteSpace(EquMaintenancePlanPagedQuery.EquipmentName))
            //{
            //    sqlBuilder.Where("EquipmentName=@EquipmentName");
            //}


            var offSet = (EquMaintenancePlanPagedQuery.PageIndex - 1) * EquMaintenancePlanPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = EquMaintenancePlanPagedQuery.PageSize });
            sqlBuilder.AddParameters(EquMaintenancePlanPagedQuery);

            using var conn = GetMESDbConnection();
            var EquMaintenancePlanEntitiesTask = conn.QueryAsync<EquMaintenancePlanEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var EquMaintenancePlanEntities = await EquMaintenancePlanEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquMaintenancePlanEntity>(EquMaintenancePlanEntities, EquMaintenancePlanPagedQuery.PageIndex, EquMaintenancePlanPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="EquMaintenancePlanQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenancePlanEntity>> GetEquMaintenancePlanEntitiesAsync(EquMaintenancePlanQuery EquMaintenancePlanQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquMaintenancePlanEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var EquMaintenancePlanEntities = await conn.QueryAsync<EquMaintenancePlanEntity>(template.RawSql, EquMaintenancePlanQuery);
            return EquMaintenancePlanEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="EquMaintenancePlanEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquMaintenancePlanEntity EquMaintenancePlanEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, EquMaintenancePlanEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="EquMaintenancePlanEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquMaintenancePlanEntity> EquMaintenancePlanEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, EquMaintenancePlanEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="EquMaintenancePlanEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquMaintenancePlanEntity EquMaintenancePlanEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, EquMaintenancePlanEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="EquMaintenancePlanEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquMaintenancePlanEntity> EquMaintenancePlanEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, EquMaintenancePlanEntitys);
        }
        #endregion

    }

    public partial class EquMaintenancePlanRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_Maintenance_plan` /**innerjoin**/ /**leftjoin**/ /**where**/  /**orderby**/  LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_Maintenance_plan` /**where**/ ";
        const string GetEquMaintenancePlanEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_Maintenance_plan` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_Maintenance_plan`(  `Id`, `Code`, `Name`, `Version`, `Type`, `Status`, `BeginTime`, `EndTime`, `CornExpression`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ExecutorIds`, `LeaderIds`) VALUES (   @Id, @Code, @Name, @Version, @Type, @Status, @BeginTime, @EndTime, @CornExpression, @IsSkipHoliday, @FirstExecuteTime, @Cycle, @CompletionHour, @CompletionMinute, @PreGeneratedMinute, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @ExecutorIds, @LeaderIds )  ";
        const string InsertsSql = "INSERT INTO `equ_Maintenance_plan`(  `Id`, `Code`, `Name`, `Version`, `Type`, `Status`, `BeginTime`, `EndTime`, `CornExpression`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ExecutorIds`, `LeaderIds`) VALUES (   @Id, @Code, @Name, @Version, @Type, @Status, @BeginTime, @EndTime, @CornExpression, @IsSkipHoliday, @FirstExecuteTime, @Cycle, @CompletionHour, @CompletionMinute, @PreGeneratedMinute, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @ExecutorIds, @LeaderIds )  ";

        const string UpdateSql = "UPDATE `equ_Maintenance_plan` SET   Code = @Code, Name = @Name, Version = @Version, Type = @Type, Status = @Status, BeginTime = @BeginTime, EndTime = @EndTime, CornExpression = @CornExpression, IsSkipHoliday = @IsSkipHoliday, FirstExecuteTime = @FirstExecuteTime, Cycle = @Cycle, CompletionHour = @CompletionHour, CompletionMinute = @CompletionMinute, PreGeneratedMinute = @PreGeneratedMinute, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_Maintenance_plan` SET   Code = @Code, Name = @Name, Version = @Version, Type = @Type, Status = @Status, BeginTime = @BeginTime, EndTime = @EndTime, CornExpression = @CornExpression, IsSkipHoliday = @IsSkipHoliday, FirstExecuteTime = @FirstExecuteTime, Cycle = @Cycle, CompletionHour = @CompletionHour, CompletionMinute = @CompletionMinute, PreGeneratedMinute = @PreGeneratedMinute, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_Maintenance_plan` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_Maintenance_plan` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Version`, `Type`, `Status`, `BeginTime`, `EndTime`, `CornExpression`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Version`, `Type`, `Status`, `BeginTime`, `EndTime`, `CornExpression`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan`  WHERE Id IN @Ids ";

        const string GetByCodeSql = @"SELECT  
                               `Id`, `Code`, `Name`, `Version`, `Type`, `Status`, `BeginTime`, `EndTime`, `CornExpression`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `ExecutorIds`, `LeaderIds`
                            FROM `equ_Maintenance_plan`  WHERE Code = @Code AND Version=@Version AND SiteId=@SiteId";
        #endregion
    }
}
