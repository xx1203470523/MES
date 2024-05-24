using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Quality.Query;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 仓储（车间物料不良记录）
    /// </summary>
    public partial class QualMaterialUnqualifiedDataRepository : BaseRepository, IQualMaterialUnqualifiedDataRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public QualMaterialUnqualifiedDataRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualMaterialUnqualifiedDataEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<QualMaterialUnqualifiedDataEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualMaterialUnqualifiedDataEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<QualMaterialUnqualifiedDataEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entities);
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
        public async Task<QualMaterialUnqualifiedDataEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualMaterialUnqualifiedDataEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualMaterialUnqualifiedDataEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualMaterialUnqualifiedDataEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualMaterialUnqualifiedDataEntity>> GetEntitiesAsync(QualMaterialUnqualifiedDataQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            if (query.MaterialInventoryId.HasValue)
            {
                sqlBuilder.Where("MaterialInventoryId=@MaterialInventoryId");
            }
            if (query.MaterialInventoryId.HasValue)
            {
                sqlBuilder.Where("UnqualifiedStatus=@UnqualifiedStatus");
            }
            sqlBuilder.AddParameters(query);


            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualMaterialUnqualifiedDataEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualMaterialUnqualifiedDataView>> GetPagedListAsync(QualMaterialUnqualifiedDataPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Select("wmi.MaterialBarCode,wmi.QuantityResidue,wmi.MaterialId,pm.MaterialCode,pm.Version,pm.MaterialName,mud.UnqualifiedStatus ,mud.CreatedOn,mud.DisposalResult,mud.DisposalTime,mud.Id ");
            
            sqlBuilder.Where("mud.IsDeleted=0");
            sqlBuilder.Where("mud.SiteId=@SiteId");

            sqlBuilder.LeftJoin("wh_material_inventory wmi on wmi.Id=mud.MaterialInventoryId");
            sqlBuilder.LeftJoin("proc_material pm on pm.Id=wmi.MaterialId");
            //sqlBuilder.LeftJoin("qual_material_unqualified_data_detail mudd on mudd.MaterialUnqualifiedDataId=mud.Id");
            //sqlBuilder.LeftJoin("qual_unqualified_group qug on qug.Id=mudd.UnqualifiedGroupId");
            //sqlBuilder.LeftJoin("qual_unqualified_code quc on quc.Id=mudd.UnqualifiedCodeId");

            sqlBuilder.OrderBy("mud.Id DESC");

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialBarCode))
            {
                pagedQuery.MaterialBarCode = $"%{pagedQuery.MaterialBarCode}%";
                sqlBuilder.Where("wmi.MaterialBarCode LIKE @MaterialBarCode");
            }
            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where("pm.MaterialCode LIKE @MaterialCode");
            }
            //if (!string.IsNullOrWhiteSpace(pagedQuery.UnqualifiedGroup))
            //{
            //    pagedQuery.UnqualifiedGroup = $"%{pagedQuery.UnqualifiedGroup}%";
            //    sqlBuilder.Where("qug.UnqualifiedGroup LIKE @UnqualifiedGroup");
            //}
            //if (!string.IsNullOrWhiteSpace(pagedQuery.UnqualifiedCode))
            //{
            //    pagedQuery.UnqualifiedCode = $"%{pagedQuery.UnqualifiedCode}%";
            //    sqlBuilder.Where("quc.UnqualifiedCode LIKE @UnqualifiedCode");
            //}

            if (pagedQuery.UnqualifiedStatus.HasValue)
            {
                sqlBuilder.Where("mud.UnqualifiedStatus = @UnqualifiedStatus");
            }

            if (pagedQuery.DisposalResult.HasValue)
            {
                sqlBuilder.Where("mud.DisposalResult = @DisposalResult");
            }
            if (pagedQuery.CreatedOn != null && pagedQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = pagedQuery.CreatedOn[0], CreatedOnEnd = pagedQuery.CreatedOn[1] });
                sqlBuilder.Where(" mud.CreatedOn >= @CreatedOnStart AND mud.CreatedOn < @CreatedOnEnd ");
            }
            if (pagedQuery.DisposalTime != null && pagedQuery.DisposalTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { DisposalTimeStart = pagedQuery.DisposalTime[0], DisposalTimeEnd = pagedQuery.DisposalTime[1] });
                sqlBuilder.Where(" mud.DisposalTime>=@DisposalTimeStart AND mud.DisposalTime <@DisposalTimeEnd ");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<QualMaterialUnqualifiedDataView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualMaterialUnqualifiedDataView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class QualMaterialUnqualifiedDataRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM qual_material_unqualified_data mud /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM qual_material_unqualified_data mud /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM qual_material_unqualified_data /**where**/  ";

        const string InsertSql = "INSERT INTO qual_material_unqualified_data(  `Id`, `MaterialInventoryId`, `UnqualifiedStatus`, `UnqualifiedRemark`, `DisposalResult`, `DisposalTime`, `DisposalRemark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @MaterialInventoryId, @UnqualifiedStatus, @UnqualifiedRemark, @DisposalResult, @DisposalTime, @DisposalRemark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";
        const string InsertsSql = "INSERT INTO qual_material_unqualified_data(  `Id`, `MaterialInventoryId`, `UnqualifiedStatus`, `UnqualifiedRemark`, `DisposalResult`, `DisposalTime`, `DisposalRemark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (  @Id, @MaterialInventoryId, @UnqualifiedStatus, @UnqualifiedRemark, @DisposalResult, @DisposalTime, @DisposalRemark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId) ";

        const string UpdateSql = "UPDATE qual_material_unqualified_data SET   MaterialInventoryId = @MaterialInventoryId, UnqualifiedStatus = @UnqualifiedStatus, UnqualifiedRemark = @UnqualifiedRemark, DisposalResult = @DisposalResult, DisposalTime = @DisposalTime, DisposalRemark = @DisposalRemark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE qual_material_unqualified_data SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE qual_material_unqualified_data SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM qual_material_unqualified_data WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM qual_material_unqualified_data WHERE Id IN @Ids ";

    }
}
