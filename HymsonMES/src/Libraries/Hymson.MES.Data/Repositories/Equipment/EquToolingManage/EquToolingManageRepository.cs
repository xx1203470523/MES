using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquToolingManage.Command;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 工具管理表仓储
    /// </summary>
    public partial class EquToolingManageRepository : BaseRepository, IEquToolingManageRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolingManageRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

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
        /// <param name="param"></param>
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
        public async Task<EquToolingManageView> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolingManageView>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolingManageView>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolingManageView>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="procequToolingManagePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolingManageView>> GetPagedInfoAsync(IEquToolingManagePagedQuery equToolingManagePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("a.IsDeleted=0");
            if (string.IsNullOrEmpty(equToolingManagePagedQuery.Sorting))
            {
                sqlBuilder.OrderBy("a.CreatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(equToolingManagePagedQuery.Sorting);
            }

            sqlBuilder.Where("a.SiteId = @SiteId");
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Code))
            {
                equToolingManagePagedQuery.Code = $"%{equToolingManagePagedQuery.Code}%";
                sqlBuilder.Where("a.Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Name))
            {
                equToolingManagePagedQuery.Name = $"%{equToolingManagePagedQuery.Name}%";
                sqlBuilder.Where("a.Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.Status.ToString()))
            {
                sqlBuilder.Where("a.Status = @Status");
            }
            if (!string.IsNullOrWhiteSpace(equToolingManagePagedQuery.ToolsTypeCode))
            {
                equToolingManagePagedQuery.ToolsTypeCode = $"%{equToolingManagePagedQuery.ToolsTypeCode}%";
                sqlBuilder.Where("b.Code like @ToolsTypeCode");
            }
            if (equToolingManagePagedQuery.UpdatedOn != null && equToolingManagePagedQuery.UpdatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = equToolingManagePagedQuery.UpdatedOn[0], EndTime = equToolingManagePagedQuery.UpdatedOn[1].AddDays(1) });
                sqlBuilder.Where("a.UpdatedOn >= @StartTime AND a.UpdatedOn < @EndTime");
            }

            var offSet = (equToolingManagePagedQuery.PageIndex - 1) * equToolingManagePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = equToolingManagePagedQuery.PageSize });
            sqlBuilder.AddParameters(equToolingManagePagedQuery);

            using var conn = GetMESDbConnection();
            var procProcedureEntitiesTask = conn.QueryAsync<EquToolingManageView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProcedureEntities = await procProcedureEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolingManageView>(procProcedureEntities, equToolingManagePagedQuery.PageIndex, equToolingManagePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsEntity>> GetEntitiesAsync(EquToolingManageQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.Codes != null && query.Codes.Any())
            {
                sqlBuilder.Where("Code IN @Codes");
            }
            if (query.ToolTypeIds != null && query.ToolTypeIds.Any())
            {
                sqlBuilder.Where("ToolsId IN @ToolTypeIds");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolsEntity equToolsEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, equToolsEntity);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquSparePartsGroupEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquSparePartsGroupEntity>(GetByCodeSql, query);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquToolsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procLoadPointEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquToolsEntity procLoadPointEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procLoadPointEntity);
        }

        /// <summary>
        ///校准
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> CalibrationAsync(CalibratioCommandCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(CalibrationUpdateSql, command);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procLoadPointEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquToolsEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class EquToolingManageRepository
    {

        const string GetPagedInfoDataSqlTemplate = @"select A.LastVerificationTime, A.Code,A.Name,A.Status,A.RatedLife,A.RatedLifeUnit,A.IsCalibrated,A.CalibrationCycle ,A.CalibrationCycleUnit,A.CurrentUsedLife, A.Remark, A.CreatedOn, A.CreatedBy, A.UpdatedBy, A.UpdatedOn, A.Id, B.Code AS ToolsTypeCode,B.Name AS ToolsTypeName,B.Id AS ToolsId  from equ_tools A JOIN equ_tools_type B ON A.ToolsId = B.Id  /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "select COUNT(*) from equ_tools A  JOIN equ_tools_type B ON A.ToolsId = B.Id /**join**/ /**where**/ ";

        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_tools` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_tools` (`Id`, `SiteId`, `Code`, `Name`, `ToolsId`, `RatedLife`, `RatedLifeUnit`, `CumulativeUsedLife`, `CurrentUsedLife`, `LastVerificationTime`, `IsCalibrated`, `CalibrationCycle`, `CalibrationCycleUnit`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES ( @Id , @SiteId , @Code , @Name , @ToolsId , @RatedLife , @RatedLifeUnit , @CumulativeUsedLife , @CurrentUsedLife , @LastVerificationTime , @IsCalibrated , @CalibrationCycle , @CalibrationCycleUnit , @Status , @Remark , @CreatedOn , @CreatedBy , @UpdatedBy , @UpdatedOn , @IsDeleted )";

        const string InsertsSql = "INSERT INTO `equ_tools` (`Id`, `SiteId`, `Code`, `Name`, `ToolsId`, `RatedLife`, `RatedLifeUnit`, `CumulativeUsedLife`, `CurrentUsedLife`, `LastVerificationTime`, `IsCalibrated`, `CalibrationCycle`, `CalibrationCycleUnit`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES ( @Id , @SiteId , @Code , @Name , @ToolsId , @RatedLife , @RatedLifeUnit , @CumulativeUsedLife , @CurrentUsedLife , @LastVerificationTime , @IsCalibrated , @CalibrationCycle , @CalibrationCycleUnit , @Status , @Remark , @CreatedOn , @CreatedBy , @UpdatedBy , @UpdatedOn , @IsDeleted )";

        const string UpdateSql = "UPDATE `equ_tools` SET NAME = @NAME, STATUS = @STATUS, ToolsId = @ToolsId, RatedLife = @RatedLife, RatedLifeUnit = @RatedLifeUnit, IsCalibrated = @IsCalibrated, CalibrationCycle = @CalibrationCycle, CalibrationCycleUnit = @CalibrationCycleUnit, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id";
        const string UpdatesSql = "UPDATE `equ_tools` SET NAME = @NAME, STATUS = @STATUS, ToolsId = @ToolsId, RatedLife = @RatedLife, RatedLifeUnit = @RatedLifeUnit, IsCalibrated = @IsCalibrated, CalibrationCycle = @CalibrationCycle, CalibrationCycleUnit = @CalibrationCycleUnit, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id";
        const string DeleteSql = "UPDATE `equ_tools` SET IsDeleted = Id WHERE Id = @Id ";
        const string CalibrationUpdateSql = "UPDATE `equ_tools` SET LastVerificationTime = @LastVerificationTime, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id";
        const string DeletesSql = "UPDATE `equ_tools` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids";
        const string GetByIdSql = @"SELECT A.Id, A.Code,A.Name,A.RatedLife,A.RatedLifeUnit,A.CumulativeUsedLife,A.CurrentUsedLife,A.LastVerificationTime,A.IsCalibrated, A.CalibrationCycle,A.CalibrationCycleUnit, A.Status, A.Remark, A.CreatedOn, A.CreatedBy, A.UpdatedBy, A.UpdatedOn, B.Code AS ToolsTypeCode,B.Name AS ToolsTypeName,B.Id AS ToolsId  from equ_tools A JOIN equ_tools_type B ON A.ToolsId = B.Id  WHERE A.Id = @Id ";
        const string GetByIdsSql = @"SELECT  `Id`, `SiteId`, `Code`, `Name`, `ToolsId`, `RatedLife`, `RatedLifeUnit`, `CumulativeUsedLife`, `CurrentUsedLife`, `LastVerificationTime`, `IsCalibrated`, `CalibrationCycle`, `CalibrationCycleUnit`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`
                            FROM `equ_tools`  WHERE Id IN @ids ";

        const string GetByCodeSql = "SELECT * FROM `equ_tools` WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
    }
}
