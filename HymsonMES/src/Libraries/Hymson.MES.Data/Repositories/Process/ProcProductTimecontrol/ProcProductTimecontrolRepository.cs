using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 产品工序时间仓储
    /// </summary>
    public partial class ProcProductTimecontrolRepository :BaseRepository, IProcProductTimecontrolRepository
    {

        public ProcProductTimecontrolRepository(IOptions<ConnectionOptions> connectionOptions): base(connectionOptions)
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
        public async Task<ProcProductTimecontrolEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ProcProductTimecontrolEntity>(GetByIdSql, new { Id=id});
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductTimecontrolEntity>> GetByIdsAsync(long[] ids) 
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ProcProductTimecontrolEntity>(GetByIdsSql, new { Ids = ids});
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProductTimecontrolView>> GetPagedInfoAsync(ProcProductTimecontrolPagedQuery pagedQuery)
       {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("pt.IsDeleted=0");
            sqlBuilder.Where("pt.SiteId = @SiteId");
            sqlBuilder.OrderBy("pt.UpdatedOn DESC");

            sqlBuilder.Select("pt.*,pm.MaterialCode,pm.MaterialName,pm.Version,pp.`Code` ProcedureCode,pp.`Name` ProcedureName");

            sqlBuilder.LeftJoin("proc_material pm on pt.ProductId=pm.Id and pm.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on pt.ProcedureId=pp.Id and pp.IsDeleted=0");

            if (!string.IsNullOrWhiteSpace(pagedQuery.MaterialCode))
            {
                pagedQuery.MaterialCode = $"%{pagedQuery.MaterialCode}%";
                sqlBuilder.Where("pm.MaterialCode like @MaterialCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.ProcedureCode))
            {
                pagedQuery.ProcedureCode = $"%{pagedQuery.ProcedureCode}%";
                sqlBuilder.Where("pp.Code like @ProcedureCode");
            }

            if (pagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("pt.Status =@Status");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var procProductTimecontrolEntitiesTask = conn.QueryAsync<ProcProductTimecontrolView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procProductTimecontrolEntities = await procProductTimecontrolEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcProductTimecontrolView>(procProductTimecontrolEntities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="procProductTimecontrolQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductTimecontrolEntity>> GetProcProductTimecontrolEntitiesAsync(ProcProductTimecontrolQuery procProductTimecontrolQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetProcProductTimecontrolEntitiesSqlTemplate);

            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (procProductTimecontrolQuery.ProductId.HasValue)
            {
                sqlBuilder.Where(" ProductId=@ProductId ");
            }
            if (procProductTimecontrolQuery.ProcedureId.HasValue)
            {
                sqlBuilder.Where(" ProcedureId=@ProcedureId ");
            }
            if (procProductTimecontrolQuery.Status.HasValue)
            {
                sqlBuilder.Where(" Status=@Status ");
            }

            using var conn = GetMESDbConnection();
            var procProductTimecontrolEntities = await conn.QueryAsync<ProcProductTimecontrolEntity>(template.RawSql, procProductTimecontrolQuery);
            return procProductTimecontrolEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="procProductTimecontrolEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcProductTimecontrolEntity procProductTimecontrolEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procProductTimecontrolEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="procProductTimecontrolEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<ProcProductTimecontrolEntity> procProductTimecontrolEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, procProductTimecontrolEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procProductTimecontrolEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcProductTimecontrolEntity procProductTimecontrolEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procProductTimecontrolEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="procProductTimecontrolEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(List<ProcProductTimecontrolEntity> procProductTimecontrolEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, procProductTimecontrolEntitys);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="procMaterialEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ChangeStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }
        #endregion

    }

    public partial class ProcProductTimecontrolRepository
    {
        #region 
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `proc_product_timecontrol` pt  /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `proc_product_timecontrol` pt  /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetProcProductTimecontrolEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `proc_product_timecontrol` /**where**/  ";

        const string InsertSql = "INSERT INTO `proc_product_timecontrol`(  `Id`, `ProductId`, `ProcedureId`, `CacheTime`, `WarningTime`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @ProductId, @ProcedureId, @CacheTime, @WarningTime, @Status, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string UpdateSql = "UPDATE `proc_product_timecontrol` SET   CacheTime = @CacheTime, WarningTime = @WarningTime, Status = @Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `proc_product_timecontrol` SET Status= @Status, UpdatedBy=@UpdatedBy, UpdatedOn=@UpdatedOn  WHERE Id = @Id ";

        const string DeleteSql = "UPDATE `proc_product_timecontrol` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `proc_product_timecontrol` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT 
                               `Id`, `ProductId`, `ProcedureId`, `CacheTime`, `WarningTime`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `proc_product_timecontrol`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT 
                                          `Id`, `ProductId`, `ProcedureId`, `CacheTime`, `WarningTime`, `Status`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`
                            FROM `proc_product_timecontrol`  WHERE Id IN @Ids ";
        #endregion
    }
}
