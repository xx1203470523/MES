/*
 *creator: Karl
 *
 *describe: 设备点检计划 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-16 02:14:30
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSpotcheckPlan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.EquSpotcheckPlan
{
    /// <summary>
    /// 设备点检计划仓储
    /// </summary>
    public partial class EquSpotcheckPlanRepository : BaseRepository, IEquSpotcheckPlanRepository
    {

        public EquSpotcheckPlanRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<EquSpotcheckPlanEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSpotcheckPlanEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckPlanEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSpotcheckPlanEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckPlanPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckPlanEntity>> GetPagedInfoAsync(EquSpotcheckPlanPagedQuery equSpotcheckPlanPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId=@SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(equSpotcheckPlanPagedQuery.Code))
            {
                sqlBuilder.Where("Code=@Code");
            }
            if (!string.IsNullOrWhiteSpace(equSpotcheckPlanPagedQuery.Name))
            {
                sqlBuilder.Where("Name=@Name");
            }
            if (!string.IsNullOrWhiteSpace(equSpotcheckPlanPagedQuery.Version))
            {
                sqlBuilder.Where("Version=@Version");
            }
            if (equSpotcheckPlanPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }
            //if (!string.IsNullOrWhiteSpace(equSpotcheckPlanPagedQuery.EquipmentCode))
            //{
            //    sqlBuilder.Where("EquipmentCode=@EquipmentCode");
            //}
            //if (!string.IsNullOrWhiteSpace(equSpotcheckPlanPagedQuery.EquipmentName))
            //{
            //    sqlBuilder.Where("EquipmentName=@EquipmentName");
            //}

            var offSet = (equSpotcheckPlanPagedQuery.PageIndex - 1) * equSpotcheckPlanPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equSpotcheckPlanPagedQuery.PageSize });
            sqlBuilder.AddParameters(equSpotcheckPlanPagedQuery);

            using var conn = GetMESDbConnection();
            var equSpotcheckPlanEntitiesTask = conn.QueryAsync<EquSpotcheckPlanEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var equSpotcheckPlanEntities = await equSpotcheckPlanEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquSpotcheckPlanEntity>(equSpotcheckPlanEntities, equSpotcheckPlanPagedQuery.PageIndex, equSpotcheckPlanPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="equSpotcheckPlanQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSpotcheckPlanEntity>> GetEquSpotcheckPlanEntitiesAsync(EquSpotcheckPlanQuery equSpotcheckPlanQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSpotcheckPlanEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var equSpotcheckPlanEntities = await conn.QueryAsync<EquSpotcheckPlanEntity>(template.RawSql, equSpotcheckPlanQuery);
            return equSpotcheckPlanEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckPlanEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquSpotcheckPlanEntity equSpotcheckPlanEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equSpotcheckPlanEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckPlanEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<EquSpotcheckPlanEntity> equSpotcheckPlanEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, equSpotcheckPlanEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckPlanEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSpotcheckPlanEntity equSpotcheckPlanEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, equSpotcheckPlanEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="equSpotcheckPlanEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<EquSpotcheckPlanEntity> equSpotcheckPlanEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, equSpotcheckPlanEntitys);
        }
        #endregion

    }

    public partial class EquSpotcheckPlanRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_spotcheck_plan` /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_spotcheck_plan` /**where**/ ";
        const string GetEquSpotcheckPlanEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_spotcheck_plan` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_spotcheck_plan`(  `Id`, `Code`, `Name`, `Version`, `ExecutorIds`, `LeaderIds`, `Type`, `Status`, `BeginTime`, `EndTime`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Version, @ExecutorIds, @LeaderIds, @Type, @Status, @BeginTime, @EndTime, @IsSkipHoliday, @FirstExecuteTime, @Cycle, @CompletionHour, @CompletionMinute, @PreGeneratedMinute, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `equ_spotcheck_plan`(  `Id`, `Code`, `Name`, `Version`, `ExecutorIds`, `LeaderIds`, `Type`, `Status`, `BeginTime`, `EndTime`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Version, @ExecutorIds, @LeaderIds, @Type, @Status, @BeginTime, @EndTime, @IsSkipHoliday, @FirstExecuteTime, @Cycle, @CompletionHour, @CompletionMinute, @PreGeneratedMinute, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string UpdateSql = "UPDATE `equ_spotcheck_plan` SET   Code = @Code, Name = @Name, Version = @Version, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds, Type = @Type, Status = @Status, BeginTime = @BeginTime, EndTime = @EndTime, IsSkipHoliday = @IsSkipHoliday, FirstExecuteTime = @FirstExecuteTime, Cycle = @Cycle, CompletionHour = @CompletionHour, CompletionMinute = @CompletionMinute, PreGeneratedMinute = @PreGeneratedMinute, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_spotcheck_plan` SET   Code = @Code, Name = @Name, Version = @Version, ExecutorIds = @ExecutorIds, LeaderIds = @LeaderIds, Type = @Type, Status = @Status, BeginTime = @BeginTime, EndTime = @EndTime, IsSkipHoliday = @IsSkipHoliday, FirstExecuteTime = @FirstExecuteTime, Cycle = @Cycle, CompletionHour = @CompletionHour, CompletionMinute = @CompletionMinute, PreGeneratedMinute = @PreGeneratedMinute, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_spotcheck_plan` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_spotcheck_plan` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `Code`, `Name`, `Version`, `ExecutorIds`, `LeaderIds`, `Type`, `Status`, `BeginTime`, `EndTime`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_spotcheck_plan`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `Code`, `Name`, `Version`, `ExecutorIds`, `LeaderIds`, `Type`, `Status`, `BeginTime`, `EndTime`, `IsSkipHoliday`, `FirstExecuteTime`, `Cycle`, `CompletionHour`, `CompletionMinute`, `PreGeneratedMinute`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `equ_spotcheck_plan`  WHERE Id IN @Ids ";
        #endregion
    }
}
