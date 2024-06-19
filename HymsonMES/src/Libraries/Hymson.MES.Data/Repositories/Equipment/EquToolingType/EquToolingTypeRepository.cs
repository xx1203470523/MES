using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Plan.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Equipment
{
    /// <summary>
    /// 仓储（工具类型）
    /// </summary>
    public partial class EquToolingTypeRepository : BaseRepository, IEquToolingTypeGroupRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public EquToolingTypeRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(EquToolingTypeEntity entity)
        {
            try
            {
                using var conn = GetMESDbConnection();
                return await conn.ExecuteAsync(InsertSql, entity);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<EquSparePartsGroupEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(EquSparePartsGroupEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<EquSparePartsGroupEntity> entities)
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
            try
            {
                using var conn = GetMESDbConnection();
                return await conn.ExecuteAsync(DeletesSql, command);
            }
            catch (Exception)
            {

                throw;
            }

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquToolingTypeEntity> GetByIdAsync(long id)
        {
            try
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryFirstOrDefaultAsync<EquToolingTypeEntity>(GetByIdSql, new { Id = id });
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsGroupEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsGroupEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquSparePartsGroupEntity>> GetEntitiesAsync(EquSparePartsGroupQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEquSparePartsGroupEntitiesSqlTemplate);
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquSparePartsGroupEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolingTypeEntity>> GetPagedInfoAsync(EquToolingTypePagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Select("*");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("UpdatedOn DESC");

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

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<EquToolingTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<EquToolingTypeEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public partial class EquToolingTypeRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `equ_tooling_type_manage` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `equ_tooling_type_manage` /**where**/ ";
        const string GetEquSparePartsGroupEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `equ_sparepart_type` /**where**/  ";

        const string InsertSql = "INSERT INTO `equ_tooling_type_manage`(  `Id`, `Code`, `Name`, `Status`, `Calibration`, `LifeSpan`, `Cycle`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Code, @Name, @Calibration, @LifeSpan, @Cycle, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `equ_sparepart_type`(  `Id`, `SiteId`, `Code`, `Name`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @Code, @Name, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `equ_sparepart_type` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `equ_sparepart_type` SET   SiteId = @SiteId, Code = @Code, Name = @Name, Status = @Status, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `equ_sparepart_type` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `equ_tooling_type_manage` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";
        const string GetByCodeSql = "SELECT * FROM `equ_tooling_type_manage` WHERE `IsDeleted` = 0 AND SiteId = @Site AND Code = @Code LIMIT 1";
        const string GetByIdSql = @"SELECT * FROM `equ_tooling_type_manage`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `equ_tooling_type_manage`  WHERE Id IN @Ids ";

    }
}
