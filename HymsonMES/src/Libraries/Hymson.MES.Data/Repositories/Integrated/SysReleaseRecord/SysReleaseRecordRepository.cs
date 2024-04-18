/*
 *creator: Karl
 *
 *describe: 发布记录表 仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */

using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;


namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 发布记录表仓储
    /// </summary>
    public partial class SysReleaseRecordRepository : BaseRepository, ISysReleaseRecordRepository
    {

        public SysReleaseRecordRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
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
        public async Task<SysReleaseRecordEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<SysReleaseRecordEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据Version获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SysReleaseRecordEntity> GetByVersionAsync(SysReleaseRecordPagedQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<SysReleaseRecordEntity>(GetBVersionSql, param);
        }


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysReleaseRecordEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<SysReleaseRecordEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sysReleaseRecordPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<SysReleaseRecordEntity>> GetPagedInfoAsync(SysReleaseRecordPagedQuery sysReleaseRecordPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy(" CreatedOn DESC");

            if (!string.IsNullOrWhiteSpace(sysReleaseRecordPagedQuery.Version))
            {
                sqlBuilder.Where("Version=@Version");
            }
            if (sysReleaseRecordPagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status=@Status");
            }
            if (sysReleaseRecordPagedQuery.EnvironmentType.HasValue)
            {
                sqlBuilder.Where("EnvironmentType=@EnvironmentType");
            }
            if (sysReleaseRecordPagedQuery.RealTime != null && sysReleaseRecordPagedQuery.RealTime.Length > 0 && sysReleaseRecordPagedQuery.RealTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { RealTimeOnStart = sysReleaseRecordPagedQuery.RealTime[0], RealTimeOnEnd = sysReleaseRecordPagedQuery.RealTime[1].AddDays(1) });
                sqlBuilder.Where(" RealTime >= @RealTimeOnStart AND RealTime < @RealTimeOnEnd ");
            }
            var offSet = (sysReleaseRecordPagedQuery.PageIndex - 1) * sysReleaseRecordPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = sysReleaseRecordPagedQuery.PageSize });
            sqlBuilder.AddParameters(sysReleaseRecordPagedQuery);

            using var conn = GetMESDbConnection();
            var sysReleaseRecordEntitiesTask = conn.QueryAsync<SysReleaseRecordEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var sysReleaseRecordEntities = await sysReleaseRecordEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<SysReleaseRecordEntity>(sysReleaseRecordEntities, sysReleaseRecordPagedQuery.PageIndex, sysReleaseRecordPagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="sysReleaseRecordQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysReleaseRecordEntity>> GetSysReleaseRecordEntitiesAsync(SysReleaseRecordQuery sysReleaseRecordQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetSysReleaseRecordEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            var sysReleaseRecordEntities = await conn.QueryAsync<SysReleaseRecordEntity>(template.RawSql, sysReleaseRecordQuery);
            return sysReleaseRecordEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sysReleaseRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(SysReleaseRecordEntity sysReleaseRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, sysReleaseRecordEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="sysReleaseRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<SysReleaseRecordEntity> sysReleaseRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, sysReleaseRecordEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysReleaseRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(SysReleaseRecordEntity sysReleaseRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, sysReleaseRecordEntity);
        }

        /// <summary>
        /// 更新 状态
        /// </summary>
        /// <param name="sysReleaseRecordEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(SysReleaseRecordEntity sysReleaseRecordEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, sysReleaseRecordEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="sysReleaseRecordEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<SysReleaseRecordEntity> sysReleaseRecordEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, sysReleaseRecordEntitys);
        }
        #endregion

    }

    public partial class SysReleaseRecordRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `sys_release_record` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `sys_release_record` /**where**/ ";
        const string GetSysReleaseRecordEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `sys_release_record` /**where**/  ";

        const string InsertSql = "INSERT INTO `sys_release_record`(  `Id`, `SiteId`, `Version`, `PlanTime`, `RealTime`, `Status`, `Remark`, `Content`, `EnvironmentType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Version, @PlanTime, @RealTime, @Status, @Remark, @Content, @EnvironmentType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `sys_release_record`(  `Id`, `SiteId`, `Version`, `PlanTime`, `RealTime`, `Status`, `Remark`, `Content`, `EnvironmentType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Version, @PlanTime, @RealTime, @Status, @Remark, @Content, @EnvironmentType, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `sys_release_record` SET   SiteId = @SiteId, Version = @Version, PlanTime = @PlanTime, RealTime = @RealTime, Status = @Status, Remark = @Remark, Content = @Content, EnvironmentType = @EnvironmentType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `sys_release_record` SET   RealTime = @RealTime, Status = @Status,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `sys_release_record` SET   SiteId = @SiteId, Version = @Version, PlanTime = @PlanTime, RealTime = @RealTime, Status = @Status, Remark = @Remark, Content = @Content, EnvironmentType = @EnvironmentType, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `sys_release_record` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `sys_release_record` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `SiteId`, `Version`, `PlanTime`, `RealTime`, `Status`, `Remark`, `Content`, `EnvironmentType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `sys_release_record`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `SiteId`, `Version`, `PlanTime`, `RealTime`, `Status`, `Remark`, `Content`, `EnvironmentType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `sys_release_record`  WHERE Id IN @Ids ";

        const string GetBVersionSql = @"SELECT  
                               `Id`, `SiteId`, `Version`, `PlanTime`, `RealTime`, `Status`, `Remark`, `Content`, `EnvironmentType`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `sys_release_record`  WHERE Version = @Version AND EnvironmentType=@EnvironmentType AND IsDeleted=0";
        #endregion
    }
}
