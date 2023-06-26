using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码表仓储
    /// </summary>
    public partial class ManuSfcRepository : BaseRepository, IManuSfcRepository
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuSfcRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }


        #region 方法
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
            return await conn.ExecuteAsync(DeletesSql, command);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcEntity>(GetByIdsSql, new { Ids = ids });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcPassDownView>> GetPagedListAsync(ManuSfcPassDownPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.LeftJoin("manu_sfc_info MSI ON MSI.SfcId = MS.Id AND MSI.IsUsed = 1");
            sqlBuilder.LeftJoin("plan_work_order PWO ON PWO.Id = MSI.WorkOrderId");
            sqlBuilder.LeftJoin("proc_material PM ON PM.Id = MSI.ProductId");
            sqlBuilder.Where("MS.IsDeleted = 0");
            sqlBuilder.Where("MS.SiteId = @SiteId");
            sqlBuilder.OrderBy("MS.UpdatedOn DESC");
            sqlBuilder.Select("MS.Id, MS.SFC, MS.IsUsed, MS.UpdatedOn, PWO.OrderCode, PM.MaterialCode, PM.MaterialName, PM.BuyType");

            if (pagedQuery.IsUsed.HasValue) sqlBuilder.Where("MS.IsUsed = @IsUsed");

            if (!string.IsNullOrWhiteSpace(pagedQuery.OrderCode))
            {
                pagedQuery.OrderCode = $"%{pagedQuery.OrderCode}%";
                sqlBuilder.Where("PWO.OrderCode LIKE @OrderCode");
            }

            if (!string.IsNullOrWhiteSpace(pagedQuery.SFC))
            {
                pagedQuery.SFC = $"%{pagedQuery.SFC}%";
                sqlBuilder.Where("MS.SFC LIKE @SFC");
            }

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<ManuSfcPassDownView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);

            return new PagedInfo<ManuSfcPassDownView>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceView>> GetManuSfcPagedInfoAsync(ManuSfcProducePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("ms.SiteId = @SiteId");
            sqlBuilder.Where("ms.Status <> 4");
            sqlBuilder.Where("ms.IsDeleted=0");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");

            sqlBuilder.Select(@"msp.ProductBOMId,msp.Id,msp.Lock,msp.ProcedureId,ms.Sfc,msp.LockProductionId,CASE ms.Status WHEN  1 THEN msp.Status ELSE 3 END AS  Status,pwo.OrderCode,pp.Code,pp.Name,pm.MaterialCode,pm.MaterialName,pm.Version,pr.ResCode ");

            sqlBuilder.InnerJoin("manu_sfc_info  msi on ms.Id=msi.SfcId AND msi.IsUsed=1 AND msi.IsDeleted=0");
            sqlBuilder.LeftJoin("manu_sfc_produce msp  on msp.SFC =ms.SFC");
            sqlBuilder.LeftJoin("proc_material pm  on msi.ProductId =pm.Id  AND pm.IsDeleted=0");
            sqlBuilder.LeftJoin("plan_work_order pwo on pwo.Id= msi.WorkOrderId AND pwo.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on msp.ProcedureId =pp.Id AND pp.IsDeleted =0");
            sqlBuilder.LeftJoin("proc_resource pr on msp.ResourceId =pr.Id AND pr.IsDeleted =0");

            //状态
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (query.Lock.HasValue)
            {
                sqlBuilder.Where("msp.Lock=@Lock");
            }
            if (query.NoLock.HasValue)
            {
                if (query.NoLock != 1)
                {
                    sqlBuilder.Where("(msp.Lock!=@NoLock or `Lock`  is null)");
                }
            }
            if (!string.IsNullOrWhiteSpace(query.Sfc))
            {
                query.Sfc = $"%{query.Sfc}%";
                sqlBuilder.Where("ms.Sfc like @Sfc");
            }
            if (query.SfcArray != null && query.SfcArray.Length > 0)
            {
                sqlBuilder.Where("ms.Sfc in @SfcArray");
            }
            //工单
            if (!string.IsNullOrWhiteSpace(query.OrderCode))
            {
                query.OrderCode = $"%{query.OrderCode}%";
                sqlBuilder.Where("pwo.OrderCode like @OrderCode");
            }
            //工序
            if (!string.IsNullOrWhiteSpace(query.Code))
            {
                query.Code = $"%{query.Code}%";
                sqlBuilder.Where("pp.Code like @Code");
            }
            //资源
            if (!string.IsNullOrWhiteSpace(query.ResCode))
            {
                query.ResCode = $"%{query.ResCode}%";
                sqlBuilder.Where("pr.ResCode like @ResCode");
            }
            //资源-》资源类型
            if (query.ResourceTypeId.HasValue)
            {
                sqlBuilder.Where("pp.ResourceTypeId=@ResourceTypeId");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceView>(manuSfcProduceEntities, query.PageIndex, query.PageSize, totalCount);
        }


        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetManuSfcEntitiesAsync(ManuSfcQuery manuSfcQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcEntitiesSqlTemplate);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcEntities = await conn.QueryAsync<ManuSfcEntity>(template.RawSql, manuSfcQuery);
            return manuSfcEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcEntity manuSfcEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, manuSfcEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(List<ManuSfcEntity> manuSfcEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, manuSfcEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcEntity manuSfcEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, manuSfcEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(List<ManuSfcEntity> manuSfcEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, manuSfcEntitys);
        }

        /// <summary>
        /// 查询条码信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcView>> GetManuSfcInfoEntitiesAsync(ManuSfcStatusQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetManuSfcInfoEntitiesSqlTemplate);
            sqlBuilder.Select("*");
            if (param.Sfcs != null && param.Sfcs.Any())
            {
                sqlBuilder.Where("sfc.SFC in @Sfcs");
            }
            if (param.Statuss != null && param.Statuss.Any())
            {
                sqlBuilder.Where("sfc.Status in @Statuss");
            }
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var list = await conn.QueryAsync<ManuSfcView>(template.RawSql, param);
            return list;
        }

        /// <summary>
        /// 批量更新条码状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(ManuSfcUpdateCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 批量更新条码（使用状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateSfcIsUsedAsync(ManuSfcUpdateIsUsedCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSfcIsUsedSql, command);
        }

        /// <summary>
        /// 批量更新条码（条码状态与使用状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateSfcStatusAndIsUsedAsync(ManuSfcUpdateStatusAndIsUsedCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateStatusAndIsUsedSql, command);
        }

        /// <summary>
        /// 获取SFC
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetBySFCAsync(GetBySfcQuery command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetBySFCSql, command);
        }

        /// <summary>
        /// 根据sfc 获取条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetBySFCsAsync(IEnumerable<string> sfcs)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcEntity>(GetBySFCsSql, new { SFCs = sfcs });
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_sfc MS /**innerjoin**/ /**leftjoin**/ /**where**/ ORDER BY MS.CreatedOn DESC  LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM manu_sfc MS /**innerjoin**/ /**leftjoin**/ /**where**/ ";
        const string GetManuSfcEntitiesSqlTemplate = @"SELECT 
                                            /**select**/
                                           FROM `manu_sfc` /**where**/  ";
        const string GetManuSfcInfoEntitiesSqlTemplate = @"SELECT 
                                            sfc.Id ,sfc.SiteId ,sfc.SFC ,sfc.Qty ,sfc.Status ,info.WorkOrderId ,info.ProductId ,info.IsUsed  FROM manu_sfc sfc LEFT JOIN  manu_sfc_info info on sfc.Id =info.SfcId  and info.IsUsed =1
                                            /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Status <> @Status AND SFC IN @Sfcs ";
        const string UpdateSfcIsUsedSql = "UPDATE manu_sfc SET IsUsed = @IsUsed, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn WHERE IsUsed <> @IsUsed AND SFC IN @Sfcs ";

        const string UpdateStatusAndIsUsedSql = "UPDATE `manu_sfc` SET Status = @Status,IsUsed = @IsUsed,  UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs ";

        const string DeleteSql = "UPDATE `manu_sfc` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `manu_sfc`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_sfc`  WHERE Id IN @Ids ";

        const string GetBySFCSql = @"SELECT * FROM `manu_sfc`  WHERE SFC = @SFC AND SiteId=@SiteId";
        const string GetBySFCsSql = @"SELECT * FROM `manu_sfc`  WHERE SFC IN @SFCs AND IsDeleted=0 ";
    }
}
