using Dapper;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipmentUnit.Query;
using Hymson.MES.Data.Repositories.Integrated.InteCalendar.Query;
using Hymson.MES.Data.Repositories.Integrated.InteClass.Query;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated.InteCalendar
{
    /// <summary>
    /// 日历维护仓储
    /// </summary>
    public partial class InteCalendarRepository : IInteCalendarRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteCalendarRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteCalendarEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteCalendarEntity entity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, command);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equOrLineId"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(long equOrLineId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteScalarAsync(ExistsSql, new
            {
                UseStatus = (int)CalendarUseStatusEnum.Enable,
                equOrLineId
            }) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equOrLineId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(long equOrLineId, long id)
        {
            // w.Id != modifyDto.Id
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteScalarAsync(ExistsSql, new
            {
                UseStatus = (int)CalendarUseStatusEnum.Enable,
                equOrLineId,
                id
            }) != null;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCalendarEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteCalendarEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCalendarView>> GetPagedListAsync(InteCalendarPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IC.IsDeleted = 0");
            sqlBuilder.OrderBy("IC.UpdatedOn DESC");
            sqlBuilder.Select("IC.Id, IC.CalendarName, IC.CalendarType, IC.Remark, IC.UseStatus, IC.CreatedBy, IC.CreatedOn, IC.UpdatedBy, IC.UpdatedOn");
            sqlBuilder.Select("IWC.`Code`, IWC.`Name`, EE.EquipmentCode, EE.EquipmentName");
            sqlBuilder.LeftJoin("inte_work_center IWC ON IC.EquOrLineId = IWC.Id");
            sqlBuilder.LeftJoin("equ_equipment EE ON IC.EquOrLineId = EE.Id");

            /*
            if (string.IsNullOrWhiteSpace(pagedQuery.SiteCode) == false)
            {
                sqlBuilder.Where("IC.SiteCode = @SiteCode");
            }
            */

            if (string.IsNullOrWhiteSpace(pagedQuery.CalendarName) == false)
            {
                pagedQuery.CalendarName = $"%{pagedQuery.CalendarName}%";
                sqlBuilder.Where("IC.CalendarName LIKE @CalendarName");
            }

            if (pagedQuery.CalendarType > DbDefaultValueConstant.IntDefaultValue)
            {
                sqlBuilder.Where("IC.CalendarType = @CalendarType");
            }

            if (pagedQuery.UseStatus > DbDefaultValueConstant.IntDefaultValue)
            {
                sqlBuilder.Where("IC.UseStatus = @UseStatus");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.Code) == false)
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("IWC.`Code` LIKE @Code");
                sqlBuilder.Where("EE.EquipmentCode LIKE @Code");
            }

            if (string.IsNullOrWhiteSpace(pagedQuery.Name) == false)
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("IWC.`Name` LIKE @Name");
                sqlBuilder.Where("EE.EquipmentName LIKE @Name");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<InteCalendarView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<InteCalendarView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteCalendarRepository
    {
        const string InsertSql = "INSERT INTO `inte_calendar`(  `Id`, `CalendarName`, `CalendarType`, `EquOrLineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`, `UseStatus`) VALUES (   @Id, @CalendarName, @CalendarType, @EquOrLineId, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @Remark, @SiteCode, @UseStatus )  ";
        const string UpdateSql = "UPDATE `inte_calendar` SET   CalendarName = @CalendarName, CalendarType = @CalendarType, EquOrLineId = @EquOrLineId, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, Remark = @Remark, SiteCode = @SiteCode, UseStatus = @UseStatus  WHERE Id = @Id ";
        const string DeleteSql = "UPDATE `inte_calendar` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id = @Id ";
        const string ExistsSql = "SELECT 1 FROM `inte_calendar` WHERE EquOrLineId = @EquOrLineId AND UseStatus = @UseStatus AND IsDeleted = 0 ";
        const string GetByIdSql = @"SELECT 
                               `Id`, `CalendarName`, `CalendarType`, `EquOrLineId`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `Remark`, `SiteCode`, `UseStatus`
                            FROM `inte_calendar`  WHERE Id = @Id ";

        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM inte_calendar IC /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM inte_calendar IC /**leftjoin**/ /**where**/";

    }
}
