using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（工具类型管理）
    /// </summary>
    public partial class EquToolsTypeRepository : BaseRepository, IEquToolsTypeRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolsTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolsTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquToolsTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquToolsTypeEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquToolsTypeEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, entities);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 软删除（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand command) 
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolsTypeEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolsTypeEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsTypeEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsTypeEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquToolsTypeEntity>> GetEntitiesAsync(EquToolsTypeQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            if (query.Codes!=null&& query.Codes.Any())
            {
                sqlBuilder.Where("Code IN @Codes");
            }
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquToolsTypeEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolsTypeEntity>> GetPagedListAsync(EquToolsTypePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pagedQuery.Code))
            {
                pagedQuery.Code = $"%{pagedQuery.Code}%";
                sqlBuilder.Where("Code LIKE @Code");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.Name))
            {
                pagedQuery.Name = $"%{pagedQuery.Name}%";
                sqlBuilder.Where("Name LIKE @Name");
            }
            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }
            if (pagedQuery.CreatedOn != null && pagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = pagedQuery.CreatedOn[0], CreatedOnEnd = pagedQuery.CreatedOn[1] });
                sqlBuilder.Where("CreatedOn >=@CreatedOnStart AND CreatedOn <@CreatedOnEnd");
            }
            if (pagedQuery.UpdatedOn != null && pagedQuery.UpdatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { StartTime = pagedQuery.UpdatedOn[0], EndTime = pagedQuery.UpdatedOn[1] });
                sqlBuilder.Where("UpdatedOn >= @StartTime AND UpdatedOn < @EndTime");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquToolsTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolsTypeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 根据Code查询对象
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquToolsTypeEntity> GetByCodeAsync(EntityByCodeQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquToolsTypeEntity>(GetByCodeSql, query);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquToolsTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM equ_tools_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM equ_tools_type /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM equ_tools_type /**where**/  ";

        const string InsertSql = "INSERT INTO equ_tools_type(  `Id`, `SiteId`, `Code`, `Name`, `RatedLife`, `RatedLifeUnit`, `IsCalibrated`, `CalibrationCycle`, `CalibrationCycleUnit`, `IsAllEquipmentUsed`, `IsAllMaterialUsed`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @RatedLife, @RatedLifeUnit, @IsCalibrated, @CalibrationCycle, @CalibrationCycleUnit, @IsAllEquipmentUsed, @IsAllMaterialUsed, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO equ_tools_type(  `Id`, `SiteId`, `Code`, `Name`, `RatedLife`, `RatedLifeUnit`, `IsCalibrated`, `CalibrationCycle`, `CalibrationCycleUnit`, `IsAllEquipmentUsed`, `IsAllMaterialUsed`, `Status`, `Remark`, `CreatedOn`, `CreatedBy`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @Code, @Name, @RatedLife, @RatedLifeUnit, @IsCalibrated, @CalibrationCycle, @CalibrationCycleUnit, @IsAllEquipmentUsed, @IsAllMaterialUsed, @Status, @Remark, @CreatedOn, @CreatedBy, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE equ_tools_type SET   SiteId = @SiteId, Code = @Code, Name = @Name, RatedLife = @RatedLife, RatedLifeUnit = @RatedLifeUnit, IsCalibrated = @IsCalibrated, CalibrationCycle = @CalibrationCycle, CalibrationCycleUnit = @CalibrationCycleUnit, IsAllEquipmentUsed = @IsAllEquipmentUsed, IsAllMaterialUsed = @IsAllMaterialUsed, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE equ_tools_type SET   SiteId = @SiteId, Code = @Code, Name = @Name, RatedLife = @RatedLife, RatedLifeUnit = @RatedLifeUnit, IsCalibrated = @IsCalibrated, CalibrationCycle = @CalibrationCycle, CalibrationCycleUnit = @CalibrationCycleUnit, IsAllEquipmentUsed = @IsAllEquipmentUsed, IsAllMaterialUsed = @IsAllMaterialUsed, Status = @Status, Remark = @Remark, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";

        const string DeleteSql = "UPDATE equ_tools_type SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE equ_tools_type SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM equ_tools_type WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM equ_tools_type WHERE Id IN @Ids ";
        const string GetByCodeSql = "SELECT * FROM `equ_tools_type` WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";

    }
}
