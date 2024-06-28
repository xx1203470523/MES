using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcScrap.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 仓储（条码报废表）
    /// </summary>
    public partial class ManuSfcScrapRepository : BaseRepository, IManuSfcScrapRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ManuSfcScrapRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcScrapEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, entity);
        }

        /// <summary>
        /// 新增（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcScrapEntity> entities)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, entities);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcScrapEntity entity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, entity);
        }

        /// <summary>
        /// 取消报废
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcScrapCancelAsync(IEnumerable<ManuSfcScrapCancelCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ScrapCancelSql, commands);
        }

        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcScrapEntity> entities)
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
        public async Task<ManuSfcScrapEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcScrapEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs获取数据（批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcScrapEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcScrapEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 根据步骤Id获取数据（批量）
        /// </summary>
        /// <param name="stepIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcScrapEntity>> GetByStepIdsAsync(IEnumerable<long> stepIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcScrapEntity>(GetByStepIdsSql, new { StepIds = stepIds });
        }

        /// <summary>
        /// 根据步骤Id获取取消数据（批量）
        /// </summary>
        /// <param name="stepIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcScrapEntity>> GetByCancelSfcStepIdsAsync(IEnumerable<long> cancelSfcStepIds)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcScrapEntity>(GetByCancelSfcStepIdsSql, new { CancelSfcStepIds = cancelSfcStepIds });
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcScrapEntity>> GetEntitiesAsync(ManuSfcScrapQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (query.SfcinfoIds != null && query.SfcinfoIds.Any())
            {
                sqlBuilder.Where(" SfcinfoId IN @SfcinfoIds ");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcScrapEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcScrapEntity>> GetPagedListAsync(ManuSfcScrapPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Select("*");
            sqlBuilder.OrderBy("UpdatedOn DESC");
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuSfcScrapEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcScrapEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcScrapRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_scrap /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_sfc_scrap /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM manu_sfc_scrap /**where**/  ";

        const string InsertSql = "INSERT INTO manu_sfc_scrap(  `Id`, `SiteId`, `SFC`, `SfcinfoId`,`SfcStepId`,  `ProcedureId`, `ScrapQty`, `OutFlowProcedureId`, `UnqualifiedId`, `IsCancel`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SFC, @SfcinfoId,@SfcStepId, @ProcedureId, @ScrapQty,@OutFlowProcedureId, @UnqualifiedId, @IsCancel, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string InsertsSql = "INSERT INTO manu_sfc_scrap(  `Id`, `SiteId`, `SFC`, `SfcinfoId`,`SfcStepId`, `ProcedureId`, `ScrapQty`,`OutFlowProcedureId`, `UnqualifiedId`,  `IsCancel`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (  @Id, @SiteId, @SFC, @SfcinfoId,@SfcStepId, @ProcedureId, @ScrapQty,@OutFlowProcedureId, @UnqualifiedId, @IsCancel, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";

        const string UpdateSql = "UPDATE manu_sfc_scrap SET   SiteId = @SiteId, SFC = @SFC, SfcinfoId = @SfcinfoId, ProcedureId = @ProcedureId, ScrapQty = @ScrapQty, IsCancel = @IsCancel, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE manu_sfc_scrap SET   SiteId = @SiteId, SFC = @SFC, SfcinfoId = @SfcinfoId, ProcedureId = @ProcedureId, ScrapQty = @ScrapQty, IsCancel = @IsCancel, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string ScrapCancelSql = "UPDATE manu_sfc_scrap SET IsCancel = @IsCancel, IsCancel = @IsCancel, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string DeleteSql = "UPDATE manu_sfc_scrap SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE manu_sfc_scrap SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM manu_sfc_scrap WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM manu_sfc_scrap WHERE Id IN @Ids ";
        const string GetByStepIdsSql = @"SELECT * FROM manu_sfc_scrap WHERE SfcStepId IN @StepIds ";
        const string GetByCancelSfcStepIdsSql = @"SELECT * FROM manu_sfc_scrap WHERE CancelSfcStepId IN @CancelSfcStepIds ";
    }
}
