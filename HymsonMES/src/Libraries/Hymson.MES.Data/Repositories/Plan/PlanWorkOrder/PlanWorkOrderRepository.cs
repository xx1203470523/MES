using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using IdGen;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Plan
{
    /// <summary>
    /// 工单信息表仓储
    /// </summary>
    public partial class PlanWorkOrderRepository : BaseRepository, IPlanWorkOrderRepository
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public PlanWorkOrderRepository(IOptions<ConnectionOptions> connectionOptions,IMemoryCache memoryCache) : base(connectionOptions)
        {
            _memoryCache = memoryCache;
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
        public async Task<PlanWorkOrderEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetByCodeAsync(PlanWorkOrderQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderEntity>(GetByCodeSql, new { OrderCode = query.OrderCode, SiteId = query.SiteId });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            if (!ids.Any()) return new List<PlanWorkOrderEntity>();
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByIdsSql, new { ids });
        }
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetBySiteIdAsync(long siteId)
        {
            var cachedKey = $"{CachedTables.PLAN_WORK_ORDER}&SiteId&{siteId}";
            return await _memoryCache.GetOrCreateLazyAsync(cachedKey, async (cacheEntity) =>
            {
                using var conn = GetMESDbConnection();
                return await conn.QueryAsync<PlanWorkOrderEntity>(GetBySiteIdSql, new { SiteId = siteId });
            });
        }
        /// <summary>
        /// 根据 workOrderId 获取数据
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderRecordEntity> GetByWorkOrderIdAsync(long workOrderId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<PlanWorkOrderRecordEntity>(GetByWorkOrderIdSql, new { workOrderId });
        }
        
        /// <summary>
        /// 根据IDs批量获取数据  含有物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderAboutMaterialInfoView>> GetByIdsAboutMaterialInfoAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderAboutMaterialInfoView>(GetByIdsAboutMaterialInfoSql, new { ids = ids });
        }

        /// <summary>
        /// 根据车间ID获取工单数据
        /// </summary>
        /// <param name="workFarmId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByWorkFarmIdAsync(long workFarmId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByWorkFarmId, new { WorkCenterType = WorkCenterTypeEnum.Farm, workFarmId });
        }

        /// <summary>
        /// 根据产线ID获取工单数据（激活的工单）
        /// </summary>
        /// <param name="workLineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetByWorkLineIdAsync(long workLineId)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderEntity>(GetByWorkLineId, new { WorkCenterType = WorkCenterTypeEnum.Line, workLineId });
        }

        /// <summary>
        /// 获取激活工单数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderView>> GetActivationWorkOrderDataAsync(PlanWorkOrderPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetActivationWorkOrderDataSqlTemplate);

            if (query.WorkCenterIds != null && query.WorkCenterIds.Any())
            {
                sqlBuilder.Where("PWOA.LineId IN @WorkCenterIds");
            }

            if (query.SiteId.HasValue)
            {
                sqlBuilder.Where("PWO.SiteId = @SiteId");
            }

            if (query.ProcessRouteIds != null && query.ProcessRouteIds.Any())
            {
                sqlBuilder.Where("PWO.ProcessRouteId IN @ProcessRouteIds");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            sqlBuilder.OrderBy("PWOA.CreatedOn DESC");

            using var conn = GetMESDbConnection();

            return await conn.QueryAsync<PlanWorkOrderView>(templateData.RawSql, templateData.Parameters);
        }

        /// <summary>
        /// 获取所有工单数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderView>> GetWorkOrderDataAsync(PlanWorkOrderPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetWorkOrderDataSqlTemplate);

            if (query.WorkCenterIds != null && query.WorkCenterIds.Any())
            {
                sqlBuilder.Where("WorkCenterId IN @WorkCenterIds");
            }

            if (query.Status.HasValue)
            {
                sqlBuilder.Where("Status = @Status");
            }

            if (query.NotInIds != null && query.NotInIds.Any())
            {
                sqlBuilder.Where("Id NOT IN @NotInIds");
            }

            if (query.Statuss != null && query.Statuss.Any())
            {
                sqlBuilder.Where("Status IN @Statuss");
            }

            if (query.SiteId.HasValue)
            {
                sqlBuilder.Where("SiteId = @SiteId");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            sqlBuilder.OrderBy("CreatedOn DESC");

            using var conn = GetMESDbConnection();

            return await conn.QueryAsync<PlanWorkOrderView>(templateData.RawSql, templateData.Parameters);
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

            if (pageQuery.CreatedOn != null && pageQuery.CreatedOn.Length > 0 && pageQuery.CreatedOn.Length >= 2)
            {
                sqlBuilder.AddParameters(new { CreatedOnStart = pageQuery.CreatedOn[0], CreatedOnEnd = pageQuery.CreatedOn[1].AddDays(1) });
                sqlBuilder.Where(" wo.CreatedOn >= @CreatedOnStart AND  wo.CreatedOn < @CreatedOnEnd ");

                //sqlBuilder.AddParameters(new { StartId = IdGenProvider.GenerateStartId(pageQuery.CreatedOn[0]), EndId = IdGenProvider.GenerateEndId(pageQuery.CreatedOn[1].AddDays(1),false) });
                //sqlBuilder.Where(" rbr.Id >= @StartId AND  rbr.Id < @EndId");
            }

            if (pageQuery.Statuss != null && pageQuery.Statuss.Any())
            {
                sqlBuilder.Where("wo.Status IN @Statuss");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<PlanWorkOrderListDetailView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<PlanWorkOrderListDetailView>(entities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// ID编码查询
        /// </summary>
        /// <param name="pageQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderListDetailView>> GetPagedInfoAsyncCode(PlanWorkOrderPagedQuery pageQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("wo.IsDeleted = 0");
            sqlBuilder.Where("wo.SiteId = @SiteId");

            if (!string.IsNullOrWhiteSpace(pageQuery.OrderCode))
            {
                sqlBuilder.Where("wo.OrderCode = @OrderCode");
            }

            var offSet = (pageQuery.PageIndex - 1) * pageQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pageQuery.PageSize });
            sqlBuilder.AddParameters(pageQuery);

            using var conn = GetMESDbConnection();
            var entities = await conn.QueryAsync<PlanWorkOrderListDetailView>(templateData.RawSql, templateData.Parameters);
            var totalCount = await conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            return new PagedInfo<PlanWorkOrderListDetailView>(entities, pageQuery.PageIndex, pageQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderEntity>> GetEntitiesAsync(PlanWorkOrderNewQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (!string.IsNullOrWhiteSpace(query.OrderCode))
            {
                query.OrderCode = $"%{query.OrderCode}%";
                sqlBuilder.Where("OrderCode LIKE @OrderCode");
            }
            if (query.Codes != null && query.Codes.Any())
            {
                sqlBuilder.Where("OrderCode IN @Codes");
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderEntity>(template.RawSql, query);
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

            using var conn = GetMESDbConnection();
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
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, planWorkOrderEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, planWorkOrderEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="planWorkOrderEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(PlanWorkOrderEntity planWorkOrderEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, planWorkOrderEntity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="planWorkOrderEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatesAsync(IEnumerable<PlanWorkOrderEntity> planWorkOrderEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, planWorkOrderEntitys);
        }

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyWorkOrderStatusAsync(IEnumerable<UpdateStatusCommand> parms)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateWorkOrderStatusSql, parms);
        }

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyWorkOrderLockedAsync(IEnumerable<UpdateLockedCommand> parms)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateWorkOrderLockedSql, parms);
        }

        /// <summary>
        /// 更新下达数量
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdatePassDownQuantityByWorkOrderIdAsync(UpdatePassDownQuantityCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatePassDownQuantitySql, param);
        }

        /// <summary>
        /// 退还下达数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> RefundPassDownQuantityByWorkOrderIdsAsync(IEnumerable<UpdatePassDownQuantityCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(RefundPassDownQuantitySql, commands);
        }

        /// <summary>
        /// 更新数量（投入数量）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateInputQtyByWorkOrderIdAsync(UpdateQtyByWorkOrderIdCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateInputQtySql, param);
        }

        /// <summary>
        /// 更新数量（投入数量）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateInputQtyByWorkOrderIdsAsync(IEnumerable<UpdateQtyByWorkOrderIdCommand>? commands)
        {
            if (commands == null || !commands.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateInputQtySql, commands);
        }

        /// <summary>
        /// 更新数量（完工数量）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateFinishProductQuantityByWorkOrderIdAsync(UpdateQtyByWorkOrderIdCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateFinishProductQuantitySql, param);
        }

        /// <summary>
        /// 更新数量（完工数量）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateFinishProductQuantityByWorkOrderIdsAsync(IEnumerable<UpdateQtyByWorkOrderIdCommand>? commands)
        {
            if (commands == null || !commands.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateFinishProductQuantitySql, commands);
        }

        #region 工单记录表

        /// <summary>
        /// 新增工单记录表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertPlanWorkOrderRecordAsync(PlanWorkOrderRecordEntity param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertPlanWorkOrderRecordSql, param);
        }

        /// <summary>
        /// 更新生产订单记录的实际开始时间
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(UpdateWorkOrderRealTimeCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRecordRealStartSql, command);
        }

        /// <summary>
        /// 更新生产订单记录的实际开始时间（批量）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePlanWorkOrderRealStartByWorkOrderIdAsync(IEnumerable<UpdateWorkOrderRealTimeCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRecordRealStartSql, commands);
        }

        /// <summary>
        /// 更新生产订单记录的实际结束时间
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(UpdateWorkOrderRealTimeCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRecordRealEndSql, command);
        }

        #endregion

        #region 马威

        /// <summary>
        /// 根据ID获取数据，含有物料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderMavelView> GetByIdMavelAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstAsync<PlanWorkOrderMavelView>(GetByIdMavelSql, new { id = id });
        }

        /// <summary>
        /// 获取马威
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PlanWorkOrderMaterialMavleView>> GetWorkOrderMavelAsync(long siteId)
        {
            string sql = $@"
                select t2.MaterialCode ,t2.MaterialName ,t1.*
                from plan_work_order t1
                inner join proc_material t2 on t1.ProductId = t2.Id and t2.IsDeleted = 0
                where t1.SiteId  = siteId
                and t1.IsDeleted  = 0
            ";

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<PlanWorkOrderMaterialMavleView>(sql);
        }

        #endregion
    }


    /// <summary>
    /// 
    /// </summary>
    public partial class PlanWorkOrderRepository
    {
        const string GetEntitiesSqlTemplate = @"SELECT /**select**/ FROM plan_work_order /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";

        const string GetPagedInfoDataSqlTemplate = @"SELECT 
                          wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`, wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,
                          wor.InputQty, wor.FinishProductQuantity, wor.PassDownQuantity,wor.UnQualifiedQuantity, wor.RealStart, wor.RealEnd,
                          m.MaterialCode, m.MaterialName,m.Version AS MaterialVersion,
                          b.BomCode, b.Version AS BomVersion,
                          pr.`Code` AS ProcessRouteCode, pr.`Name` AS ProcessRouteName, pr.Version AS ProcessRouteVersion,
                          wc.`Code` AS WorkCenterCode, wc.`Name` AS WorkCenterName
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_record wor on wo.Id = wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId = m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId = b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId = pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId = wc.Id
                        /**where**/ Order by wo.CreatedOn DESC LIMIT @Offset, @Rows ";
        const string GetPagedInfoCountSqlTemplate = @"SELECT COUNT(1) 
                         FROM `plan_work_order` wo 
                         LEFT JOIN plan_work_order_record wor on wo.Id = wor.WorkOrderId
                         LEFT JOIN proc_material m on wo.ProductId = m.Id
                         LEFT JOIN proc_bom b on wo.ProductBOMId = b.Id
                         LEFT JOIN proc_process_route pr on wo.ProcessRouteId = pr.Id
                         LEFT JOIN inte_work_center wc on wo.WorkCenterId = wc.Id
                        /**where**/   ";
        const string GetPlanWorkOrderEntitiesSqlTemplate = @"SELECT /**select**/ FROM `plan_work_order` /**where**/  ";

        const string InsertSql = "INSERT INTO `plan_work_order`(  `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, WorkPlanId) VALUES (   @Id, @OrderCode, @ProductId, @WorkCenterType, @WorkCenterId, @ProcessRouteId, @ProductBOMId, @Type, @Qty, @Status, @OverScale, @PlanStartTime, @PlanEndTime, @IsLocked, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @WorkPlanId );  ";
        const string InsertPlanWorkOrderRecordSql = "INSERT INTO `plan_work_order_record`(  `Id`, `RealStart`, `RealEnd`, `InputQty`, `UnqualifiedQuantity`, `FinishProductQuantity`, `PassDownQuantity`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`, `WorkOrderId`, `WorkPlanId`) VALUES (   @Id, @RealStart, @RealEnd, @InputQty, @UnqualifiedQuantity, @FinishProductQuantity, @PassDownQuantity, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId, @WorkOrderId, @WorkPlanId )";

        const string UpdateSql = "UPDATE `plan_work_order` SET  ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `plan_work_order` SET   OrderCode = @OrderCode, ProductId = @ProductId, WorkCenterType = @WorkCenterType, WorkCenterId = @WorkCenterId, ProcessRouteId = @ProcessRouteId, ProductBOMId = @ProductBOMId, Type = @Type, Qty = @Qty, Status = @Status, OverScale = @OverScale, PlanStartTime = @PlanStartTime, PlanEndTime = @PlanEndTime, IsLocked = @IsLocked, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted, SiteId = @SiteId  WHERE Id = @Id ";


#if DM
        const string UpdatePassDownQuantitySql = "UPDATE plan_work_order_record SET PassDownQuantity = IFNULL(PassDownQuantity, 0) + CAST(@PassDownQuantity AS DECIMAL), UpdatedBy = @UserName, UpdatedOn = @UpdateDate WHERE WorkOrderId = @WorkOrderId AND IFNULL(PassDownQuantity, 0) <= CAST(@PlanQuantity AS DECIMAL) - CAST(@PassDownQuantity AS DECIMAL) AND IsDeleted = 0";
        const string UpdateFinishProductQuantitySql = "UPDATE plan_work_order_record SET " +
            "FinishProductQuantity = (CASE WHEN FinishProductQuantity IS NULL THEN 0 ELSE FinishProductQuantity END) + CAST(@Qty AS DECIMAL), " +
            "UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId;";
        const string UpdateInputQtySql = "UPDATE plan_work_order_record SET " +
            "InputQty = (CASE WHEN InputQty IS NULL THEN 0 ELSE InputQty END) + CAST(@Qty AS DECIMAL), " +
            "RealStart = (CASE WHEN RealStart IS NULL THEN @UpdatedOn ELSE RealStart END), " +
            "UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId;";
#else
        const string UpdatePassDownQuantitySql = "UPDATE plan_work_order_record SET PassDownQuantity = IFNULL(PassDownQuantity, 0) + @PassDownQuantity, UpdatedBy = @UserName, UpdatedOn = @UpdateDate WHERE WorkOrderId = @WorkOrderId AND IFNULL(PassDownQuantity, 0) <= @PlanQuantity - @PassDownQuantity AND IsDeleted = 0; ";
        const string UpdateFinishProductQuantitySql = "UPDATE plan_work_order_record SET " +
            "FinishProductQuantity = @Qty, " +
            "UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId;";
        const string UpdateInputQtySql = "UPDATE plan_work_order_record SET " +
            "InputQty = (CASE WHEN InputQty IS NULL THEN 0 ELSE InputQty END) + @Qty, " +
            "RealStart = (CASE WHEN RealStart IS NULL THEN @UpdatedOn ELSE RealStart END), " +
            "UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE IsDeleted = 0 AND WorkOrderId = @WorkOrderId;";
#endif

        const string RefundPassDownQuantitySql = "UPDATE plan_work_order_record SET PassDownQuantity = IFNULL(PassDownQuantity, 0) - @PassDownQuantity, UpdatedBy = @UserName, UpdatedOn = @UpdateDate WHERE WorkOrderId = @WorkOrderId AND IsDeleted = 0;";

        const string DeleteSql = "UPDATE `plan_work_order` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `plan_work_order`  SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn  WHERE Id in @ids ";

        const string GetByIdSql = @"SELECT * FROM `plan_work_order`  WHERE Id = @Id ";
        const string GetBySiteIdSql = @"SELECT * FROM `plan_work_order` WHERE SiteId=@SiteId AND IsDeleted = 0";
        const string GetByIdsSql = @"SELECT
      `Id`, `OrderCode`, `ProductId`, `WorkCenterType`, `WorkCenterId`, `ProcessRouteId`, `ProductBOMId`, `Type`, `Qty`, `Status`, `OverScale`, `PlanStartTime`, `PlanEndTime`, `IsLocked`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId` ,LockedStatus
    FROM `plan_work_order`  WHERE Id IN @ids ";
        const string GetByWorkOrderIdSql = "SELECT * FROM plan_work_order_record WHERE IsDeleted = 0 AND WorkOrderId = @workOrderId ";
        const string GetByCodeSql = @"SELECT * FROM plan_work_order WHERE IsDeleted = 0 AND OrderCode = @OrderCode and SiteId=@SiteId ";
        const string GetByIdsAboutMaterialInfoSql = @"SELECT
      wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`, wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,
         m.MaterialCode, m.MaterialName,m.Version as MaterialVersion 
    FROM `plan_work_order`wo 
    LEFT JOIN proc_material m on wo.ProductId=m.Id  
    WHERE wo.Id IN @ids ";
        const string GetByIdMavelSql = @"
        SELECT
              wo.`Id`, wo.`OrderCode`, wo.`ProductId`, wo.`WorkCenterType`, wo.`WorkCenterId`, wo.`ProcessRouteId`, wo.`ProductBOMId`, 
              wo.`Type`, wo.`Qty`, wo.`Status`, wo.`OverScale`, wo.`PlanStartTime`, wo.`PlanEndTime`, wo.`IsLocked`, wo.`Remark`, wo.`CreatedBy`,
              wo.`CreatedOn`, wo.`UpdatedBy`, wo.`UpdatedOn`, wo.`IsDeleted`, wo.`SiteId`,
              m.MaterialCode, m.MaterialName,m.Version as MaterialVersion ,pwp.WorkPlanCode  OrderNo
            FROM `plan_work_order`wo 
            LEFT JOIN proc_material m on wo.ProductId=m.Id and m.IsDeleted = 0
            left join plan_work_plan pwp on pwp.Id = wo.WorkPlanId and pwp.IsDeleted = 0
            WHERE wo.Id = @id;
        ";

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

        const string GetActivationWorkOrderDataSqlTemplate = "SELECT PWO.*,PWOA.LineId,PWOR.PassDownQuantity FROM plan_work_order_activation PWOA LEFT JOIN plan_work_order PWO ON PWO.Id = PWOA.WorkOrderId LEFT JOIN plan_work_order_record PWOR ON PWO.ID = PWOR.WorkOrderId /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
        const string GetWorkOrderDataSqlTemplate = "SELECT * FROM plan_work_order /**where**/ /**orderby**/ LIMIT @Offset,@Rows";
    }
}
