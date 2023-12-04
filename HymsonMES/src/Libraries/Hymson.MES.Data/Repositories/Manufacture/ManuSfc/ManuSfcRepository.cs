using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;
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
        public async Task<IEnumerable<ManuSfcEntity>> GetByIdsAsync(IEnumerable<long> ids)
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
            sqlBuilder.OrderBy("MS.CreatedOn DESC");
            sqlBuilder.Select("MS.Id, MS.SFC, MS.IsUsed, MS.CreatedOn, PWO.OrderCode, PM.MaterialCode, PM.MaterialName, PM.BuyType");

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
        /// <param name="manuSfcProducePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceView>> GetManuSfcPagedInfoAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("ms.SiteId = @SiteId");
            sqlBuilder.Where("ms.Status <> 4");
            sqlBuilder.Where("ms.IsDeleted=0");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");

            sqlBuilder.Select(@"msp.ProductBOMId,msp.Id,msp.`Lock`,msp.ProcedureId,ms.Sfc,msp.LockProductionId,CASE ms.Status WHEN  1 THEN msp.Status ELSE 3 END AS  Status,pwo.OrderCode,pp.Code,pp.Name,pm.MaterialCode,pm.MaterialName,pm.Version,pr.ResCode ");

            sqlBuilder.InnerJoin("manu_sfc_info  msi on ms.Id=msi.SfcId AND msi.IsUsed=1 AND msi.IsDeleted=0");
            sqlBuilder.LeftJoin("manu_sfc_produce msp  on msp.SFC =ms.SFC");
            sqlBuilder.LeftJoin("proc_material pm  on msi.ProductId =pm.Id  AND pm.IsDeleted=0");
            sqlBuilder.LeftJoin("plan_work_order pwo on pwo.Id= msi.WorkOrderId AND pwo.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on msp.ProcedureId =pp.Id AND pp.IsDeleted =0");
            sqlBuilder.LeftJoin("proc_resource pr on msp.ResourceId =pr.Id AND pr.IsDeleted =0");

            //状态
            if (manuSfcProducePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.Sfc))
            {
                manuSfcProducePagedQuery.Sfc = $"%{manuSfcProducePagedQuery.Sfc}%";
                sqlBuilder.Where("ms.Sfc like @Sfc");
            }
            if (manuSfcProducePagedQuery.SfcArray != null && manuSfcProducePagedQuery.SfcArray.Length > 0)
            {
                sqlBuilder.Where("ms.Sfc in @SfcArray");
            }
            //工单
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.OrderCode))
            {
                manuSfcProducePagedQuery.OrderCode = $"%{manuSfcProducePagedQuery.OrderCode}%";
                sqlBuilder.Where("pwo.OrderCode like @OrderCode");
            }
            //工序
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.Code))
            {
                manuSfcProducePagedQuery.Code = $"%{manuSfcProducePagedQuery.Code}%";
                sqlBuilder.Where("pp.Code like @Code");
            }
            //资源
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.ResCode))
            {
                manuSfcProducePagedQuery.ResCode = $"%{manuSfcProducePagedQuery.ResCode}%";
                sqlBuilder.Where("pr.ResCode like @ResCode");
            }
            //资源-》资源类型
            if (manuSfcProducePagedQuery.ResourceTypeId.HasValue)
            {
                sqlBuilder.Where("pp.ResourceTypeId=@ResourceTypeId");
            }

            var offSet = (manuSfcProducePagedQuery.PageIndex - 1) * manuSfcProducePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcProducePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcProducePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceView>(manuSfcProduceEntities, manuSfcProducePagedQuery.PageIndex, manuSfcProducePagedQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 分页查询（查询所有条码信息）
        /// 优化
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceSelectView>> GetManuSfcSelectPagedInfoAsync(ManuSfcProduceSelectPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("ms.SiteId = @SiteId");
            sqlBuilder.Where("ms.IsDeleted=0");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");

            sqlBuilder.Select(@"msp.ProductBOMId,msp.IsScrap,msp.Id,msp.`Lock`,msp.ProcedureId,msp.ProcessRouteId,ms.Sfc,msp.LockProductionId,ms.Status 
                                , msi.WorkOrderId, msp.ResourceId, msi.ProductId ");

            sqlBuilder.InnerJoin("manu_sfc_info  msi on ms.Id=msi.SfcId AND msi.IsUsed=1 AND msi.IsDeleted=0");
            sqlBuilder.LeftJoin("manu_sfc_produce msp  on msp.SFC =ms.SFC");


            if (!string.IsNullOrEmpty(query.MaterialVersion) || !string.IsNullOrWhiteSpace(query.MaterialCode)) 
            {
                sqlBuilder.LeftJoin(" proc_material ma  on msi.ProductId =ma.id");
            }

            if (!string.IsNullOrEmpty(query.MaterialCode)) 
            {
                query.MaterialCode = $"%{query.MaterialCode}%";
                sqlBuilder.Where("ma.MaterialCode like  @MaterialCode");
            }
            if (!string.IsNullOrEmpty(query.MaterialVersion))
            {
                query.MaterialVersion = $"%{query.MaterialVersion}%";
                sqlBuilder.Where("ma.Version like @MaterialVersion");
            }

            //sfc条码状态
            if (query.SfcStatus.HasValue)
            {
                sqlBuilder.Where("ms.Status<>@SfcStatus");
            }
            //状态
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (query.Lock.HasValue)
            {
                sqlBuilder.Where("msp.`Lock`=@Lock");
            }
            if (query.NoLock.HasValue && query.NoLock != 1)
            {
                sqlBuilder.Where("(msp.`Lock`!=@NoLock or `Lock`  is null)");
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
            if (query.OrderId.HasValue && query.OrderId > 0)
            {
                sqlBuilder.Where(" msi.WorkOrderId = @OrderId ");
            }
            //工序
            if (query.ProcedureId.HasValue && query.ProcedureId > 0)
            {
                sqlBuilder.Where("  msp.ProcedureId = @ProcedureId ");
            }
            //资源
            if (query.ResourceId.HasValue && query.ResourceId > 0)
            {
                sqlBuilder.Where("  msp.ResourceId = @ResourceId ");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceSelectView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceSelectView>(manuSfcProduceEntities, query.PageIndex, query.PageSize, totalCount);
        }


        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetManuSfcEntitiesAsync(ManuSfcQuery manuSfcQuery)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<ManuSfcEntity>(GetSFCsSql, manuSfcQuery);
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
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcEntity> manuSfcEntitys)
        {
            if (manuSfcEntitys == null || !manuSfcEntitys.Any()) return 0;

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
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcEntity> manuSfcEntitys)
        {
            if (manuSfcEntitys == null || !manuSfcEntitys.Any()) return 0;

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, manuSfcEntitys);
        }

        /// <summary>
        /// 批量更新（带状态检查）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeWithStatusCheckAsync(IEnumerable<ManuSfcEntity>? entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateWithStatusCheckSql, entities);
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
        public async Task<int> MultiUpdateSfcIsUsedAsync(MultiSfcUpdateIsUsedCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(MultiUpdateSfcIsUsedSql, command);
        }

        /// <summary>
        /// 批量更新条码（条码状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> MultiUpdateSfcStatusAsync(MultiSFCUpdateStatusCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(MultiUpdateStatusSql, command);
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
        /// 批量更新进站条码状态和在用状态
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> InStationManuSfcByIdAsync(IEnumerable<InStationManuSfcByIdCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateInStationStatusSql, commands);
        }

        /// <summary>
        /// 批量更新条码（每个条码状态都不一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatusBySfcsAsync(IEnumerable<ManuSfcUpdateStatusCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ManuSfcUpdateStatusCommandSql, commands);
        }

        /// <summary>
        /// 批量更新条码（更具Id 状态更新为一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatuByIdsAsync(ManuSfcUpdateStatusByIdsCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ManuSfcUpdateStatuByIdsSql, command);
        }

        /// <summary>
        /// 批量更新条码状态（根据Id 更新状态 更新状态为不一致）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatuByIdRangeAsync(IEnumerable<ManuSfcUpdateStatusByIdCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ManuSfcUpdateStatuByIdSql, commands);
        }

        /// <summary>
        ///更新条码状态（根据Id 更新状态 更新状态为不一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatuByIdAsync(ManuSfcUpdateStatusByIdCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ManuSfcUpdateStatuByIdSql, command);
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcScrapByIdsAsync(IEnumerable<ScrapManuSfcByIdCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ManuSfcScrapByIdsSql, commands);
        }

        /// <summary>
        ///取消条码报废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcCancellScrapByIdsAsync(IEnumerable<CancelScrapManuSfcByIdCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(ManuSfcCancellScrapByIdsSql, commands);
        }

        /// <summary>
        /// 更新条码数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuSfcQtyByIdRangeAsync(IEnumerable<UpdateManuSfcQtyByIdCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateManuSfcQtyByIdSql, commands);
        }

        /// <summary>
        /// 获取SFC
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetBySFCAsync(GetBySfcQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetBySFCSql, query);
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

        const string GetManuSfcInfoEntitiesSqlTemplate = @"SELECT 
                                            sfc.Id ,sfc.SiteId ,sfc.SFC ,sfc.Qty ,sfc.Status ,info.Id AS SFCInfoId, info.WorkOrderId ,info.ProductId ,info.IsUsed  FROM manu_sfc sfc LEFT JOIN  manu_sfc_info info on sfc.Id =info.SfcId  and info.IsUsed =1
                                            /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Status <> @Status AND SFC IN @Sfcs ";
        const string UpdateWithStatusCheckSql = "UPDATE manu_sfc SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Status <> @Status AND Id = @Id; ";
        const string UpdateManuSfcQtyByIdSql = "UPDATE `manu_sfc` SET  Qty = @Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";

        const string MultiUpdateSfcIsUsedSql = "UPDATE manu_sfc SET IsUsed = @IsUsed, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE SiteId = @SiteId AND IsUsed <> @IsUsed AND SFC IN @SFCs ";
        const string MultiUpdateStatusSql = "UPDATE manu_sfc SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE SiteId = @SiteId AND SFC IN @SFCs; ";
        const string UpdateStatusAndIsUsedSql = "UPDATE `manu_sfc` SET Status = @Status,IsUsed = @IsUsed,  UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs ";
        const string UpdateInStationStatusSql = "UPDATE `manu_sfc` SET Status = @Status,IsUsed = @IsUsed,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string ManuSfcUpdateStatusCommandSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Status= @Status AND SFC =@Sfc AND SiteId = @SiteId";
        const string ManuSfcUpdateStatuByIdsSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE  Id IN @Ids";
        const string ManuSfcUpdateStatuByIdSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE  Id = @Id AND Status=@CurrentStatus";
        const string ManuSfcScrapByIdsSql = "UPDATE `manu_sfc` SET StatusBack=Status,SfcScrapId=@SfcScrapId, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE  Id = @Id AND Status=@CurrentStatus ";
        const string ManuSfcCancellScrapByIdsSql = "UPDATE `manu_sfc` SET Status=StatusBack,SfcScrapId=null, StatusBack = null, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE  Id = @Id AND Status=@CurrentStatus";
        const string DeleteSql = "UPDATE `manu_sfc` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `manu_sfc` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdSql = @"SELECT * FROM `manu_sfc`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_sfc`  WHERE Id IN @Ids ";

        const string GetBySFCSql = @"SELECT * FROM `manu_sfc`  WHERE SFC = @SFC AND SiteId=@SiteId";
        const string GetBySFCsSql = @"SELECT * FROM `manu_sfc`  WHERE SFC IN @SFCs AND IsDeleted=0 ";
        const string GetSFCsSql = @"SELECT * FROM manu_sfc WHERE SiteId = @SiteId AND IsDeleted = 0 AND SFC IN @SFCs; ";
    }
}
