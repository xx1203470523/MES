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
    public partial class InteCalendarDateDetailRepository : IInteCalendarDateDetailRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteCalendarDateDetailRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(InteCalendarDateDetailEntity entity)
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
        public async Task<int> InsertRangeAsync(IEnumerable<InteCalendarDateDetailEntity> entitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entitys);
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCalendarDateDetailEntity entity)
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
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteByCalendarIdsAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { calendarId = idsArr });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCalendarDateDetailEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteCalendarDateDetailEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteCalendarDateDetailEntity>> GetEntitiesAsync(long calendarId)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetInteCalendarEntitiesSqlTemplate);
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteCalendarDateDetailEntity>(template.RawSql, new { calendarId });
            return entities;
        }
    }

    public partial class InteCalendarDateDetailRepository
    {
        const string GetInteCalendarEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `inte_calendar` /**innerjoin**/ /**leftjoin**/ /**where**/  ";

        const string InsertSql = "INSERT INTO `inte_calendar`(  `Id`, `CalendarName`, `CalendarType`, `EquOrLineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`, `UseStatus`) VALUES (   @Id, @CalendarName, @CalendarType, @EquOrLineId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode, @UseStatus )  ";
        const string UpdateSql = "UPDATE `inte_calendar` SET   CalendarName = @CalendarName, CalendarType = @CalendarType, EquOrLineId = @EquOrLineId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode, UseStatus = @UseStatus  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_calendar` SET IsDeleted = '1' WHERE Id = @Id ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `CalendarName`, `CalendarType`, `EquOrLineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`, `UseStatus`
                            FROM `inte_calendar`  WHERE Id = @Id ";
    }
}
