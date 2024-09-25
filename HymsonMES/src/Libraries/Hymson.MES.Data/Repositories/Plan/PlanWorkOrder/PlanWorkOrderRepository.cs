using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Microsoft.Extensions.Caching.Memory;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单信息表仓储
    /// </summary>
    public partial class PlanWorkOrderRepository : BaseRepository, IPlanWorkOrderRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ConnectionOptions _connectionOptions;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public PlanWorkOrderRepository(IOptions<ConnectionOptions> connectionOptions, IMemoryCache memoryCache) : base(connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
            _memoryCache = memoryCache;
        }


        /// <summary>
        /// 根据查询条件获取工单产量报表分页数据
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderProductionReportView>> GetPlanWorkOrderProductionReportPageListAsync(PlanWorkOrderProductionReportPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoPlanWorkOrderProductionReportDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoPlanWorkOrderProductionReportCountSqlTemplate);

            sqlBuilder.Where(" pwor.SiteId = @SiteId ");
            sqlBuilder.Where(" pwor.IsDeleted = 0 ");
            sqlBuilder.Where(" pwo.SiteId = @SiteId ");
            sqlBuilder.Where(" pwo.IsDeleted = 0 ");

            if (!string.IsNullOrEmpty(pageQuery.MaterialCode))
            {
                pageQuery.MaterialCode = $"%{pageQuery.MaterialCode}%";
                sqlBuilder.Where(" m.MaterialCode like @MaterialCode ");
            }
            if (!string.IsNullOrEmpty(pageQuery.OrderCode))
            {
                //pageQuery.OrderCode = $"%{pageQuery.OrderCode}%";
                sqlBuilder.Where(" pwo.OrderCode = @OrderCode ");
            }
            if (pageQuery.OrderType.HasValue)
            {
                sqlBuilder.Where(" pwo.Type = @OrderType ");
            }
            if (!string.IsNullOrEmpty(pageQuery.WorkCenterCode))
            {
                sqlBuilder.Where(" iwc.Code = @WorkCenterCode ");
            }
            if (pageQuery.RealEnd != null && pageQuery.RealEnd.Length > 0)
            {
                if (pageQuery.RealEnd.Length >= 2)
                {
                    sqlBuilder.AddParameters(new { RealEndStart = pageQuery.RealEnd[0], RealEndEnd = pageQuery.RealEnd[1].AddDays(1) });
                    sqlBuilder.Where(" pwor.RealEnd >= @RealEndStart AND pwor.RealEnd < @RealEndEnd ");
                }
            }
            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var reportDataTask = conn.QueryAsync<PlanWorkOrderProductionReportView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var reportData = await reportDataTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<PlanWorkOrderProductionReportView>(reportData, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
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
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetByIdAsync(long id)
        {
            //var key = $"plan_work_order&{id}";
            //return await _memoryCache.GetOrCreateLazyAsync(key, async (cacheEntry) =>
            //{
                using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
                return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderEntity>(GetByIdSql, new { Id = id });
            //});
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetByCodeAsync(PlanWorkOrderQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderEntity>(GetByCodeSql, new { OrderCode = query.OrderCode });

        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据 workOrderId 获取数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderRecordEntity> GetByWorkOrderIdAsync(long workOrderId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderRecordEntity>(GetByWorkOrderIdSql, new { workOrderId });
        }

        /// <summary>
        /// 根据工单模糊查询
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>

        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByWorderOrderAsync(PlanWorkOrderQuery query)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            string orderCode = $"{query.OrderCode}%";
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByPreCodeSql, new { OrderCode = orderCode, SiteId = query.SiteId });
        }

        /// <summary>
        /// 根据IDs批量获取数据  含有物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderAboutMaterialInfoView>> GetByIdsAboutMaterialInfoAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderAboutMaterialInfoView>(GetByIdsAboutMaterialInfoSql, new { ids = ids });
        }

        /// <summary>
        /// 根据车间ID获取工单数据
        /// </summary>
        /// <param name="workFarmId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByWorkFarmIdAsync(long workFarmId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByWorkFarmId, new { WorkCenterType = WorkCenterTypeEnum.Farm, workFarmId });
        }

        /// <summary>
        /// 根据产线ID获取工单数据（激活的工单）
        /// </summary>
        /// <param name="workLineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByWorkLineIdAsync(long workLineId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByWorkLineId, new { WorkCenterType = WorkCenterTypeEnum.Line, workLineId });
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoAsync(PlanWorkOrderPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("wo.IsDeleted = 0");
            sqlBuilder.Where("wo.SiteId = @SiteId");

            if (pageQuery.WorkOrderId.HasValue)
            {
                sqlBuilder.Where("wo.Id = @WorkOrderId");
            }
            if (!string.IsNullOrWhiteSpace(pageQuery.OrderCode))
            {
                pageQuery.OrderCode = $"%{pageQuery.OrderCode}%";
                sqlBuilder.Where("wo.OrderCode LIKE @OrderCode");
            }
            if (!string.IsNullOrWhiteSpace(pageQuery.MaterialCode))
            {
                pageQuery.MaterialCode = $"%{pageQuery.MaterialCode}%";
                sqlBuilder.Where("m.MaterialCode LIKE @MaterialCode");
            }
            if (!string.IsNullOrWhiteSpace(pageQuery.MaterialVersion))
            {
                pageQuery.MaterialVersion = $"%{pageQuery.MaterialVersion}%";
                sqlBuilder.Where("m.Version LIKE @MaterialVersion");
            }
            if (!string.IsNullOrWhiteSpace(pageQuery.WorkCenterCode))
            {
                pageQuery.WorkCenterCode = $"%{pageQuery.WorkCenterCode}%";
                sqlBuilder.Where("wc.Code LIKE @WorkCenterCode");
            }

            if (pageQuery.Type.HasValue)
            {
                sqlBuilder.Where("wo.Type = @Type ");
            }

            if (pageQuery.Status.HasValue)
            {
                sqlBuilder.Where("wo.Status = @Status");
            }

            if (pageQuery.PlanStartTime != null && pageQuery.PlanStartTime.Length >= 2)
            {
                sqlBuilder.AddParameters(new { PlanStartTimeStart = pageQuery.PlanStartTime[0], PlanStartTimeEnd = pageQuery.PlanStartTime[1].AddDays(1) });
                sqlBuilder.Where("wo.PlanStartTime >= @PlanStartTimeStart AND wo.PlanStartTime < @PlanStartTimeEnd");
            }

            if (pageQuery.Statuss != null && pageQuery.Statuss.Any())
            {
                sqlBuilder.Where("wo.Status IN @Statuss");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var entities = await conn.QueryAsync<PlanWorkOrderListDetailView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<PlanWorkOrderListDetailView>(entities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="planWorkOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetPlanWorkOrderEntitiesAsync(PlanWorkOrderQuery planWorkOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderEntities = await conn.QueryAsync<PlanWorkOrderEntity>(template.RawSql, planWorkOrderQuery);
            return planWorkOrderEntities;
        }

        /// <summary>
        /// 获取List   equal
        /// </summary>
        /// <param name="planWorkOrderQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetEqualPlanWorkOrderEntitiesAsync(PlanWorkOrderQuery planWorkOrderQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetPlanWorkOrderEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(planWorkOrderQuery.OrderCode))
            {
                sqlBuilder.Where(" OrderCode = @OrderCode ");
            }
            if (planWorkOrderQuery.ProductIds != null && planWorkOrderQuery.ProductIds.Any())
            {
                sqlBuilder.Where(" ProductId in @ProductIds ");
            }
            if (planWorkOrderQuery.StatusList != null && planWorkOrderQuery.StatusList.Any())
            {
                sqlBuilder.Where(" Status in @StatusList ");
            }

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var planWorkOrderEntities = await conn.QueryAsync<PlanWorkOrderEntity>(template.RawSql, planWorkOrderQuery);
            return planWorkOrderEntities;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(PlanWorkOrderEntity planWorkOrderEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, planWorkOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertsSql, planWorkOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderEntity planWorkOrderEntity)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatesSql, planWorkOrderEntitys);
        }

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyWorkOrderStatusAsync(IEnumerable<UpdateStatusCommand> parms)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateWorkOrderStatusSql, parms);
        }

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyWorkOrderLockedAsync(IEnumerable<UpdateLockedCommand> parms)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateWorkOrderLockedSql, parms);
        }

        /// <summary>
        /// 更新下达数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdatePassDownQuantityByWorkOrderId(UpdatePassDownQuantityCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdatePassDownQuantitySql, param);
        }

        /// <summary>
        /// 更新数量（投入数量）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateInputQtyByWorkOrderIdAsync(UpdateQtyCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateInputQtySql, param);
        }

        /// <summary>
        /// 更新数量（投入数量）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateInputQtyByWorkOrderIdAsync(IEnumerable<UpdateQtyCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateInputQtySql, commands);
        }

        /// <summary>
        /// 更新数量（完工数量）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateFinishProductQuantityByWorkOrderIdAsync(UpdateQtyCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateFinishProductQuantitySql, param);
        }

        #region 工单记录表
        /// <summary>
        /// 新增工单记录表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertPlanWorkOrderRecordAsync(PlanWorkOrderRecordEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertPlanWorkOrderRecordSql, param);
        }

        /// <summary>
        /// 更新生产订单记录的实际开始时间
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(UpdateWorkOrderRealTimeCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateRecordRealStartSql, command);
        }

        /// <summary>
        /// 更新生产订单记录的实际开始时间（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(IEnumerable<UpdateWorkOrderRealTimeCommand> commands)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateRecordRealStartSql, commands);
        }

        /// <summary>
        /// 更新生产订单记录的实际结束时间
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(UpdateWorkOrderRealTimeCommand command)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateRecordRealEndSql, command);
        }
        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkOrderRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                          wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`, wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,
                          wor.InputQty, wor.FinishProductQuantity, wor.PassDownQuantity, wor.RealStart, wor.RealEnd,
                          m.MaterialCode, m.MaterialName,m.Version as MaterialVersion,
                          b.BomCode,b.Version as BomVersion,
                          pr.`Code` as ProcessRouteCode ,pr.Version as ProcessRouteVersion,
                          wc.`Code`  as WorkCenterCode,
                          wc.`Name`  as WorkCenterName
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_record wor on wo.Id = wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId = m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId = b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId = pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId = wc.Id
                        /**where**/  Order by wo.CreatedOn desc  LIMIT @Offset,@Rows ";

        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(1) 
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_record wor on wo.Id = wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId = m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId = b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId = pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId = wc.Id
                        /**where**/   ";

        const string GetPlanWorkOrderEntitiesSqlTemplate = @"SELECT
    /**select**/
    FROM `plan_work_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `plan_work_order`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterType, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Qty, @Status, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertsSql = "INSERT INTO `plan_work_order`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterType, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Qty, @Status, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";
        const string InsertPlanWorkOrderRecordSql = "INSERT INTO `plan_work_order_record`(  `Id`, `RealStart`, `RealEnd`, `InputQty`, `UnqualifiedQuantity`, `FinishProductQuantity`, `PassDownQuantity`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `WorkOrderId`) VALUES (   @Id, @RealStart, @RealEnd, @InputQty, @UnqualifiedQuantity, @FinishProductQuantity, @PassDownQuantity, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @WorkOrderId )";
        const string UpdateSql = "UPDATE `plan_work_order` SET  ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `plan_work_order` SET   OrderCode = @OrderCode, ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, Status = @Status, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, IsLocked = @IsLocked, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";

        const string UpdatePassDownQuantitySql = "UPDATE plan_work_order_record SET PassDownQuantity= ifnull(PassDownQuantity,0)+@PassDownQuantity,UpdatedBy=@UserName,UpdatedOn=@UpdateDate WHERE WorkOrderId=@WorkOrderId AND  ifnull(PassDownQuantity,0)<=@PlanQuantity-@PassDownQuantity AND IsDeleted=0";
        const string UpdateInputQtySql = "UPDATE plan_work_order_record SET InputQty = IFNULL(InputQty, 0) + @Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId";
        const string UpdateFinishProductQuantitySql = "UPDATE plan_work_order_record SET FinishProductQuantity = IFNULL(FinishProductQuantity, 0) + @Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId";

        const string DeleteSql = "UPDATE `plan_work_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `plan_work_order`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";
        const string GetByIdSql = @"SELECT * FROM `plan_work_order`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT
      `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId` ,LockedStatus
    FROM `plan_work_order`  WHERE Id IN @ids ";
        const string GetByWorkOrderIdSql = "SELECT * FROM plan_work_order_record WHERE IsDeleted = 0 AND WorkOrderId = @workOrderId ";
        const string GetByCodeSql = @"SELECT * FROM plan_work_order WHERE IsDeleted = 0 AND OrderCode = @OrderCode ";
        const string GetByPreCodeSql = @"SELECT * FROM plan_work_order WHERE IsDeleted = 0 AND OrderCode like @OrderCode ";
        const string GetByIdsAboutMaterialInfoSql = @"SELECT
      wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`, wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,
         m.MaterialCode, m.MaterialName,m.Version as MaterialVersion 
    FROM `plan_work_order`wo 
    LEFT JOIN proc_material m on wo.ProductId=m.Id  
    WHERE wo.Id IN @ids ";

        const string GetByWorkFarmId = "SELECT PWO.* FROM plan_work_order PWO " +
            "LEFT JOIN inte_work_center_relation IWCR ON IWCR.WorkCenterId = PWO.WorkCenterId " +
            "LEFT JOIN inte_work_center IWC ON IWC.Id = IWCR.SubWorkCenterId " +
            "WHERE PWO.IsDeleted = 0 AND PWO.WorkCenterType = @WorkCenterType AND IWCR.SubWorkCenterId = @workFarmId ";
        const string GetByWorkLineId = "SELECT PWO.* FROM plan_work_order_activation PWOA " +
            "LEFT JOIN plan_work_order PWO ON PWO.Id = PWOA.WorkOrderId " +
            "WHERE PWO.IsDeleted = 0 AND PWO.WorkCenterType = @WorkCenterType AND PWOA.LineId = @workLineId ";
        const string UpdateWorkOrderStatusSql = @"UPDATE `plan_work_order` SET Status = @Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id  AND  Status=@BeforeStatus ";
        const string UpdateWorkOrderLockedSql = @"UPDATE `plan_work_order` SET Status = @Status, LockedStatus=@LockedStatus,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateRecordRealStartSql = "UPDATE plan_work_order_record SET RealStart = @UpdatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND RealStart IS NULL AND WorkOrderId IN @WorkOrderIds; ";
        const string UpdateRecordRealEndSql = "UPDATE plan_work_order_record SET RealEnd = @UpdatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE WorkOrderId IN @WorkOrderIds AND IsDeleted = 0 ";

        const string GetPagedInfoPlanWorkOrderProductionReportDataSqlTemplate = @" select pwo.Id,pwo.OrderCode,pwo.Type,pwo.Qty,iwc.Name as WorkCentName,iwc.Code WorkCentCode,
                                m.MaterialCode,m.MaterialName,pwo.PlanStartTime,pwo.PlanEndTime,pwor.RealStart,pwor.RealEnd,
                                pwor.InputQty,pwor.UnqualifiedQuantity,pwor.FinishProductQuantity,pwor.PassDownQuantity,pwo.`Status`,
                                (SELECT count(1) FROM manu_sfc_summary mss where mss.WorkOrderId=pwo.Id AND mss.QualityStatus=0 ) NGQty /*NG数量*/
                                from plan_work_order_record pwor 
                                LEFT JOIN plan_work_order pwo on pwor.WorkOrderId=pwo.Id
                                LEFT JOIN proc_material m on m.Id=pwo.ProductId 
                                LEFT JOIN proc_bom b on b.id=pwo.ProductBOMId 
                                LEFT JOIN inte_work_center iwc on iwc.id=pwo.WorkCenterId  /**where**/  Order by pwo.CreatedOn desc  LIMIT @Offset,@Rows  ";
        const string GetPagedInfoPlanWorkOrderProductionReportCountSqlTemplate = @"select COUNT(1) 
                                from plan_work_order_record pwor 
                                LEFT JOIN plan_work_order pwo on pwor.WorkOrderId=pwo.Id
                                LEFT JOIN proc_material m on m.Id=pwo.ProductId 
                                LEFT JOIN proc_bom b on b.id=pwo.ProductBOMId 
                                LEFT JOIN inte_work_center iwc on iwc.id=pwo.WorkCenterId  /**where**/  ";

    }
}
