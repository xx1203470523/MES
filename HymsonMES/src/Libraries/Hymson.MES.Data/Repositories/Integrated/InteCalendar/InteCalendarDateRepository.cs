using Dapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Hymson.MES.Data.Repositories.Integrated.InteCalendar
{
    /// <summary>
    /// 日历维护仓储
    /// </summary>
    public partial class InteCalendarDateRepository : IInteCalendarDateRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteCalendarDateRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(InteCalendarDateEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var id = await conn.ExecuteScalarAsync<long>(InsertSql, entity);
            entity.Id = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<InteCalendarDateEntity> entitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCalendarDateEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public async Task<int> DeleteByCalendarIdAsync(long calendarId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { calendarId });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="calendarIdsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteByCalendarIdsAsync(long[] calendarIdsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { calendarId = calendarIdsArr });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCalendarDateEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteCalendarDateEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCalendarDateEntity>> GetEntitiesAsync(long calendarId)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCalendarEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteCalendarDateEntity>(template.RawSql, new { calendarId });
            return entities;
        }

        /*
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="inteCalendarPagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCalendarDateEntity>> GetPagedInfoAsync(InteCalendarPagedQuery inteCalendarPagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            //sqlBuilder.Select("*");

            //if (!string.IsNullOrWhiteSpace(procMaterialPagedQuery.SiteCode))
            //{
            //    sqlBuilder.Where("SiteCode=@SiteCode");
            //}
           
            var offSet = (inteCalendarPagedQuery.PageIndex - 1) * inteCalendarPagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = inteCalendarPagedQuery.PageSize });
            sqlBuilder.AddParameters(inteCalendarPagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteCalendarEntitiesTask = conn.QueryAsync<InteCalendarDateEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteCalendarEntities = await inteCalendarEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteCalendarDateEntity>(inteCalendarEntities, inteCalendarPagedQuery.PageIndex, inteCalendarPagedQuery.PageSize, totalCount);
        }


        */

    }

    public partial class InteCalendarDateRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_calendar` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `inte_calendar` /**where**/";
        const string GetInteCalendarEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_calendar` /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_calendar`(  `Id`, `CalendarName`, `CalendarType`, `EquOrLineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`, `UseStatus`) VALUES (   @Id, @CalendarName, @CalendarType, @EquOrLineId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode, @UseStatus )  ";
        const string UpdateSql = "UPDATE `inte_calendar` SET   CalendarName = @CalendarName, CalendarType = @CalendarType, EquOrLineId = @EquOrLineId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode, UseStatus = @UseStatus  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_calendar` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `CalendarName`, `CalendarType`, `EquOrLineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`, `UseStatus`
                            FROM `inte_calendar`  WHERE Id = @Id ";
    }
}
