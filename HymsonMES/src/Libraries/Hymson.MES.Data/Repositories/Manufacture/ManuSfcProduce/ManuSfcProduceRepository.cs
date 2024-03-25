using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除）仓储
    /// </summary>
    public partial class ManuSfcProduceRepository : BaseRepository, IManuSfcProduceRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuSfcProduceRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcProducePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceView>> GetPagedInfoAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("msp.SiteId = @SiteId");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");

            sqlBuilder.Select("msp.IsScrap,msp.ProductBOMId,msp.Id,msp.ProcessRouteId,msp.ProcedureId,msp.Sfc,msp.Status,msp.ProductId,pwo.OrderCode,pp.Code,pp.Name,pm.MaterialCode,pm.MaterialName,pm.Version,pr.ResCode ");

            sqlBuilder.LeftJoin("proc_material pm  on msp.ProductId =pm.Id  and pm.IsDeleted=0");
            sqlBuilder.LeftJoin("plan_work_order pwo on msp.WorkOrderId =pwo.Id  and pwo.IsDeleted=0");
            sqlBuilder.LeftJoin("proc_procedure pp on msp.ProcedureId =pp.Id and pp.IsDeleted =0");
            sqlBuilder.LeftJoin("proc_resource pr on msp.ResourceId =pr.Id and pr.IsDeleted =0");

            //状态
            if (manuSfcProducePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (manuSfcProducePagedQuery.IsScrap.HasValue)
            {
                sqlBuilder.Where("msp.IsScrap=@IsScrap");
            }
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.Sfc))
            {
                manuSfcProducePagedQuery.Sfc = $"%{manuSfcProducePagedQuery.Sfc}%";
                sqlBuilder.Where("msp.Sfc like @Sfc");
            }
            if (manuSfcProducePagedQuery.SfcArray != null && manuSfcProducePagedQuery.SfcArray.Length > 0)
            {
                sqlBuilder.Where("msp.Sfc in @SfcArray");
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
            //产品编码
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.MaterialCode))
            {
                manuSfcProducePagedQuery.MaterialCode = $"%{manuSfcProducePagedQuery.MaterialCode}%";
                sqlBuilder.Where("pm.MaterialCode like @MaterialCode");
            }
            //产品版本
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.Version))
            {
                manuSfcProducePagedQuery.Version = $"%{manuSfcProducePagedQuery.Version}%";
                sqlBuilder.Where("pm.Version like @Version");
            }

            var offSet = (manuSfcProducePagedQuery.PageIndex - 1) * manuSfcProducePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcProducePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcProducePagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceView>(manuSfcProduceEntities, manuSfcProducePagedQuery.PageIndex, manuSfcProducePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcProducePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceEntity>> GetPagedListAsync(ManuSfcProducePagedQuery manuSfcProducePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("msp.SiteId = @SiteId");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");
            sqlBuilder.Select("msp.*");

            //状态
            if (manuSfcProducePagedQuery.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            if (manuSfcProducePagedQuery.IsScrap.HasValue)
            {
                sqlBuilder.Where("msp.IsScrap=@IsScrap");
            }
            if (!string.IsNullOrWhiteSpace(manuSfcProducePagedQuery.Sfc))
            {
                manuSfcProducePagedQuery.Sfc = $"%{manuSfcProducePagedQuery.Sfc}%";
                sqlBuilder.Where("msp.Sfc like @Sfc");
            }
            if (manuSfcProducePagedQuery.SfcArray != null && manuSfcProducePagedQuery.SfcArray.Length > 0)
            {
                sqlBuilder.Where("msp.Sfc in @SfcArray");
            }
            //工单
            if (manuSfcProducePagedQuery.OrderId.HasValue && manuSfcProducePagedQuery.OrderId > 0)
            {
                sqlBuilder.Where(" msp.WorkOrderId = @OrderId ");
            }
            //工序
            if (manuSfcProducePagedQuery.ProcedureId.HasValue && manuSfcProducePagedQuery.ProcedureId > 0)
            {
                sqlBuilder.Where("  msp.ProcedureId = @ProcedureId ");
            }
            //资源
            if (manuSfcProducePagedQuery.ResourceId.HasValue && manuSfcProducePagedQuery.ResourceId > 0)
            {
                sqlBuilder.Where("  msp.ResourceId = @ResourceId ");
            }
            //物料
            if (manuSfcProducePagedQuery.ProductId.HasValue && manuSfcProducePagedQuery.ProductId > 0)
            {
                sqlBuilder.Where("  msp.ProductId = @ProductId ");
            }

            var offSet = (manuSfcProducePagedQuery.PageIndex - 1) * manuSfcProducePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = manuSfcProducePagedQuery.PageSize });
            sqlBuilder.AddParameters(manuSfcProducePagedQuery);

            using var conn = GetMESDbConnection();
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceEntity>(manuSfcProduceEntities, manuSfcProducePagedQuery.PageIndex, manuSfcProducePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询（在制）
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceEntity>> GetPagedListAsync(ManuSfcProduceNewPagedQuery pagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetProducePagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetProducePagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.OrderBy("Id DESC");
            sqlBuilder.Select("Id, SFCId, SFC, ProductId, WorkOrderId, CreatedOn");

            if (pagedQuery.IsUsed.HasValue) sqlBuilder.Where("CreatedOn = UpdatedOn");
            if (pagedQuery.WorkOrderIds != null) sqlBuilder.Where("WorkOrderId IN @WorkOrderIds");
            if (!string.IsNullOrWhiteSpace(pagedQuery.SFC)) sqlBuilder.Where("SFC = @SFC");

            var offSet = (pagedQuery.PageIndex - 1) * pagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = pagedQuery.PageSize });
            sqlBuilder.AddParameters(pagedQuery);

            using var conn = GetMESDbConnection();
            var entitiesTask = conn.QueryAsync<ManuSfcProduceEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var entities = await entitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceEntity>(entities, pagedQuery.PageIndex, pagedQuery.PageSize, totalCount);
        }


        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetManuSfcProduceEntitiesAsync(ManuSfcProduceQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);

            sqlBuilder.Where("SiteId = @SiteId");

            if (query.Sfcs != null && query.Sfcs.Any())
            {
                sqlBuilder.Where("Sfc in @Sfcs");
            }

            if (query.ResourceId.HasValue)
            {
                sqlBuilder.Where("ResourceId = @ResourceId");
            }
            if (query.ProcedureId.HasValue)
            {
                sqlBuilder.Where("ProcedureId = @ProcedureId");
            }

            using var conn = GetMESDbConnection();
            var manuSfcProduceEntities = await conn.QueryAsync<ManuSfcProduceEntity>(template.RawSql, query);
            return manuSfcProduceEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceInfoView>> GetManuSfcProduceInfoEntitiesAsync(ManuSfcProduceQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesInfoSqlTemplate);

            sqlBuilder.Select("msp.*,msi.Id as SfcInfoId");
            sqlBuilder.LeftJoin("manu_sfc mf  on mf.SFC =msp.sfc  and mf.IsDeleted=0");
            sqlBuilder.LeftJoin("manu_sfc_info msi on msi.SfcId =mf.Id  and msi.IsDeleted=0 and msi.WorkOrderId =MSP.WorkOrderId ");

            sqlBuilder.Where("msp.SiteId = @SiteId");
            if (query.Sfcs != null && query.Sfcs.Any())
            {
                sqlBuilder.Where("msp.Sfc in @Sfcs");
            }

            using var conn = GetMESDbConnection();
            var manuSfcProduceEntities = await conn.QueryAsync<ManuSfcProduceInfoView>(template.RawSql, query);
            return manuSfcProduceEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcProduceEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceEntity> GetBySFCAsync(ManuSfcProduceBySfcQuery sfcQuery)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceEntity>(GetBySFCSql, sfcQuery);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcProduceEntity manuSfcProduceEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcProduceEntity);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys)
        {
            if (manuSfcProduceEntitys == null || !manuSfcProduceEntitys.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, manuSfcProduceEntitys);
        }

        /// <summary>
        /// 质量锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> QualityLockAsync(QualityLockCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQualityLockSql, command);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcProduceEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcProduceEntity manuSfcProduceEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcProduceEntity);
        }

        /// <summary>
        /// 批量更新（带状态检查）
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeWithStatusCheckAsync(IEnumerable<ManuSfcProduceEntity>? entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateWithStatusCheckSql, entities);
        }

        /// <summary>
        /// 批量更新（带状态检查）
        /// </summary>
        /// <param name="multiUpdateStatusCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateProduceInStationSFCAsync(IEnumerable<UpdateProduceInStationSFCCommand> multiUpdateProduceInStationSFCCommands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateProduceInStationSFCSql, multiUpdateProduceInStationSFCCommands);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="manuSfcProduceEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangeAsync(IEnumerable<ManuSfcProduceEntity> manuSfcProduceEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, manuSfcProduceEntitys);
        }

        /// <summary>
        /// 批量更新数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyRangeAsync(IEnumerable<UpdateSfcProcedureQtyByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQtySql, commands);
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
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangeAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteRangeSql, new { ids });
        }

        /// <summary>
        /// 删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicalAsync(DeletePhysicalBySfcCommand sfcCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletePhysicalSql, sfcCommand);
        }

        /// <summary>
        /// 批量删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicalRangeAsync(DeletePhysicalBySfcsCommand sfcsCommand)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletePhysicalRangeSql, sfcsCommand);
        }

        /// <summary>
        /// 批量删除（物理删除）条码信息
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicalRangeByIdsSqlAsync(PhysicalDeleteSFCProduceByIdsCommand idsCommand)
        {
            if (idsCommand == null || idsCommand.Ids == null || !idsCommand.Ids.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletePhysicalRangeByIdsSql, idsCommand);
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicalByIdSqlAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletePhysicalRangeByIdSql, new { Id = id });
        }

        /// <summary>
        /// 批量更新条码IsScrap
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateIsScrapAsync(UpdateIsScrapCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateIsScrapSql, command);
        }

        /// <summary>
        /// 批量更新条码工艺路线和工序信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateRouteAsync(ManuSfcUpdateRouteCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRouteSql, command);
        }

        /// <summary>
        /// 根据清空复投次数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> CleanRepeatedCountById(CleanRepeatedCountCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(CleanRepeatedCountSql, command);
        }

        /// <summary>
        /// 批量更新条码工艺路线和工序信息并清空复投次数（条码独立更新）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateRouteByIdRangeAsync(IEnumerable<ManuSfcUpdateRouteByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRouteBIdSql, commands);
        }

        /// <summary>
        /// 更新条码工艺路线和工序信息并清空复投次数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateRouteByIdAsync(ManuSfcUpdateRouteByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateRouteBIdSql, command);
        }

        /// <summary>
        /// 更新条码Status
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAsync(UpdateStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 批量更新条码Status
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusByIdRangeAsync(IEnumerable<UpdateManuSfcProduceStatusByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusByIdSql, commands);
        }

        /// <summary>
        /// 更新条码Status
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusByIdAsync(UpdateManuSfcProduceStatusByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusByIdSql, command);
        }

        /// <summary>
        /// 更新资源，工艺路线，工序，维修状态
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateProcedureIdAsync(UpdateProcedureCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateProcedureIdSql, command);
        }

        /// <summary>
        /// 根据SFC批量更新工序与状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param 
        /// <returns></returns>
        public async Task<int> UpdateProcedureAndStatusRangeAsync(UpdateProcedureAndStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateProcedureAndStatusSql, command);
        }

        /// <summary>
        /// 根据SFCs批量更新资源
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param 
        /// <returns></returns>
        public async Task<int> UpdateProcedureAndResourceRangeAsync(UpdateProcedureAndResourceCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateProcedureAndResourceSql, command);
        }

        /// <summary>
        /// 锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> LockedSfcProcedureAsync(LockedProcedureCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(LockSfcProcedureSql, command);
        }

        /// <summary>
        /// 解除锁定
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UnLockedSfcProcedureAsync(UnLockedProcedureCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UnLockSfcProcedureSql, command);
        }


        #region 在制品业务
        /// <summary>
        /// 新增在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcProduceBusinessAsync(ManuSfcProduceBusinessEntity manuSfcProduceBusinessEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSfcProduceBusinessSql, manuSfcProduceBusinessEntity);
        }

        /// <summary>
        /// 批量新增在制品业务
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task<int> InsertSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> entities)
        {
            if (entities == null || !entities.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSfcProduceBusinessSql, entities);
        }

        /// <summary>
        /// 插入或者更新
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertOrUpdateSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertOrUpdateSfcProduceBusinessSql, manuSfcProduceBusinessEntitys);
        }

        /// <summary>
        /// 更新在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdatetSfcProduceBusinessAsync(ManuSfcProduceBusinessEntity manuSfcProduceBusinessEntity)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSfcProduceBusinessSql, manuSfcProduceBusinessEntity);
        }

        /// <summary>
        /// 批量更新在制品业务
        /// </summary>
        /// <param name="manuSfcProduceBusinessEntitys"></param>
        /// <returns></returns>
        public async Task<int> UpdatestSfcProduceBusinessRangeAsync(IEnumerable<ManuSfcProduceBusinessEntity> manuSfcProduceBusinessEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSfcProduceBusinessSql, manuSfcProduceBusinessEntitys);
        }

        /// <summary>
        /// 根据ID获取在制品业务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceBusinessEntity> GetSfcProduceBusinessBySFCIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceBusinessEntity>(GetSfcProduceBusinessBySFCIdSql, new { SfcInfoId = id });
        }

        /// <summary>
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceBusinessEntity> GetSfcProduceBusinessBySFCAsync(SfcProduceBusinessQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProduceBusinessEntity>(GetSfcProduceBusinessBySFCSql, query);
        }

        /// <summary>
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetSfcProduceBusinessEntitiesBySFCAsync(SfcListProduceBusinessQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcProduceBusinessEntity>(GetEntitiesSql, query);
        }

        /// <summary>
        /// 根据SFC获取在制品业务
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceBusinessView>> GetSfcProduceBusinessListBySFCAsync(SfcListProduceBusinessQuery sfc)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcProduceBusinessView>(GetSfcProduceBusinessBySFCsSql, sfc);
        }

        /// <summary>
        /// 根据IDs批量获取在制品业务
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceBusinessEntity>> GetSfcProduceBusinessBySFCIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcProduceBusinessEntity>(GetSfcProduceBusinessBySFCIdsSql, new { SfcInfoIds = ids });
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinessBySfcInfoIdAsync(DeleteSfcProduceBusinesssBySfcInfoIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSfcProduceBusinessBySfcInfoIdSql, command);
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinessByIdAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSfcProduceBusinessByIdSql, new { Ids = ids });
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinessBySfcInfoIdsAsync(DeleteSFCProduceBusinesssByIdsCommand command)
        {
            if (command == null || command.SfcInfoIds == null || !command.SfcInfoIds.Any()) return 0;

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSfcProduceBusinessBySfcInfoIdsSql, command);
        }

        /// <summary>
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> DeleteSfcProduceBusinesssAsync(DeleteSfcProduceBusinesssCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(RealDeletesSfcProduceBusinessSql, command);
        }
        #endregion

        /// <summary>
        /// 根据SFCs 获取数据
        /// </summary>
        /// <param name="sfcsQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetListBySfcsAsync(ManuSfcProduceBySfcsQuery sfcsQuery)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcProduceEntity>(GetListBySfcsSql, sfcsQuery);
        }

        /// <summary>
        /// 根据工序ID,状态获取在制品数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcProduceEntity>> GetActivityListByProcedureIdStatusAsync(ManuSfcProduceByProcedureIdStatusQuery query)
        {
            using var conn = GetMESDbConnection();
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetListByProcedureIdAndResIdStatusSql);
            sqlBuilder.Where("p.SiteId = @SiteId");
            sqlBuilder.Where("p.ProcedureId = @ProcedureId");
            if (query.ResourceId.HasValue)
            {
                sqlBuilder.Where(" p.ResourceId = @ResourceId");
            }
            sqlBuilder.Where("p.Status = @Status");

            if (!string.IsNullOrEmpty(query.MaterialVersion) || !string.IsNullOrWhiteSpace(query.MaterialCode))
            {
                sqlBuilder.LeftJoin(" proc_material ma  on p.ProductId =ma.id");
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

            return await conn.QueryAsync<ManuSfcProduceEntity>(template.RawSql, query);
        }

        /// <summary>
        /// 分页查询（查询所有在制条码信息，加入载具）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceVehicleView>> GetManuSfcPageListAsync(ManuSfcProduceVehiclePagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("msp.SiteId = @SiteId");
            sqlBuilder.OrderBy("msp.UpdatedOn DESC");

            sqlBuilder.Select(@"msp.Id,msp.SFC,msp.WorkOrderId,msp.ProcedureId,msp.ResourceId,msp.Status,msp.ProductId,msp.ProcessRouteId,msp.ProductBOMId,vfs.VehicleId");

            sqlBuilder.LeftJoin("inte_vehicle_freight_stack vfs on msp.SFC =vfs.BarCode");

            //单条码
            if (!string.IsNullOrWhiteSpace(query.Sfc))
            {
                query.Sfc = $"%{query.Sfc}%";
                sqlBuilder.Where("msp.Sfc like @Sfc");
            }

            //多条码
            if (query.SfcArray != null && query.SfcArray.Length > 0)
            {
                sqlBuilder.Where("msp.Sfc in @SfcArray");
            }

            //状态
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("msp.Status=@Status");
            }
            //是否报废
            if (query.IsScrap.HasValue)
            {
                sqlBuilder.Where("msp.IsScrap=@IsScrap");
            }

            //产品
            if (query.ProductId.HasValue && query.ProductId > 0)
            {
                sqlBuilder.Where("  msp.ProductId = @ProductId ");
            }
            //工单
            if (query.WorkOrderId.HasValue && query.WorkOrderId > 0)
            {
                sqlBuilder.Where(" msp.WorkOrderId = @WorkOrderId ");
            }
            //工艺路线
            if (query.ProcessRouteId.HasValue && query.ProcessRouteId > 0)
            {
                sqlBuilder.Where("  msp.ProcessRouteId = @ProcessRouteId ");
            }
            //Bom
            if (query.ProductBOMId.HasValue && query.ProductBOMId > 0)
            {
                sqlBuilder.Where("  msp.ProductBOMId = @ProductBOMId ");
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
            //载具
            if (query.VehicleId.HasValue && query.VehicleId > 0)
            {
                sqlBuilder.Where("  vfs.VehicleId = @VehicleId ");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceVehicleView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceVehicleView>(manuSfcProduceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 根据条码更改条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAndQtyBySfcsAsync(UpdateStatusAndQtyBySfcsCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusAndQtyBySfcsSql, command);
        }

        /// <summary>
        /// 根据id更改数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyByIdAsync(UpdateManuSfcProduceQtyByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQtyByIdSql, command);
        }

        #region 顷刻

        /// <summary>
        /// 根据条码更改条码状态
        /// 用于设备接口产出米数上报
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateQtyBySfcAsync(UpdateQtyBySfcCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateQtyBySfcsSql, command);
        }

        /// <summary>
        /// 获取设备最近条码
        /// </summary>
        /// <returns></returns>
        public async Task<ManuSfcProductMaterialView> GetEquipmentNewestSfc(ManuSfcEquipmentNewestQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcProductMaterialView>(GetEquipmentNewestSfcsql, query);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ManuSfcProduceRepository
    {
        const string GetProducePagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM manu_sfc_produce /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset, @Rows ";
        const string GetProducePagedInfoCountSqlTemplate = @"SELECT COUNT(1) FROM manu_sfc_produce /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ ";

        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `manu_sfc_produce`  msp /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(1) FROM `manu_sfc_produce`  msp  /**innerjoin**/ /**leftjoin**/  /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT * FROM `manu_sfc_produce` /**where**/  ";
        const string GetEntitiesInfoSqlTemplate = @"SELECT  /**select**/ FROM `manu_sfc_produce` msp /**innerjoin**/ /**leftjoin**/  /**where**/   ";

        const string InsertSql = "INSERT INTO `manu_sfc_produce`(  `Id`, `SFC`,`SFCId`, `ProductId`, `WorkOrderId`, `BarCodeInfoId`, `ProcessRouteId`, `WorkCenterId`, `ProductBOMId`, `Qty`, `EquipmentId`, `ResourceId`, `ProcedureId`, `Status`, `Lock`, `LockProductionId`, `IsSuspicious`, `RepeatedCount`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `SiteId`, `IsScrap`) VALUES (   @Id, @SFC,@SFCId, @ProductId, @WorkOrderId, @BarCodeInfoId, @ProcessRouteId, @WorkCenterId, @ProductBOMId, @Qty, @EquipmentId, @ResourceId, @ProcedureId, @Status, @Lock, @LockProductionId, @IsSuspicious, @RepeatedCount, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @SiteId, @IsScrap )  ";
        const string InsertSfcProduceBusinessSql = "INSERT INTO `manu_sfc_produce_business`(  `Id`, `SiteId`, `SfcProduceId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcProduceId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string UpdateSql = "UPDATE `manu_sfc_produce` SET Sfc = @Sfc, ProductId = @ProductId, WorkOrderId = @WorkOrderId, BarCodeInfoId = @BarCodeInfoId, ProcessRouteId = @ProcessRouteId, WorkCenterId = @WorkCenterId, ProductBOMId = @ProductBOMId, EquipmentId = @EquipmentId, ResourceId = @ResourceId, ProcedureId = @ProcedureId, Status = @Status, `Lock` = @Lock, LockProductionId = @LockProductionId, IsSuspicious = @IsSuspicious, RepeatedCount = @RepeatedCount, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string UpdateWithStatusCheckSql = "UPDATE manu_sfc_produce SET Status = @Status, ResourceId = @ResourceId, ProcessRouteId = @ProcessRouteId, ProcedureId = @ProcedureId, EquipmentId = @EquipmentId, RepeatedCount = @RepeatedCount, IsScrap = @IsScrap, IsRepair = @IsRepair, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Status <> @Status AND Id = @Id; ";

        const string UpdateProduceInStationSFCSql = "UPDATE manu_sfc_produce SET Status = @Status, ResourceId = @ResourceId, ProcedureId = @ProcedureId, EquipmentId = @EquipmentId, RepeatedCount = @RepeatedCount, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id AND (Status = @CurrentStatus OR CreatedOn = UpdatedOn); ";
        const string UpdateSfcProduceBusinessSql = "UPDATE `manu_sfc_produce_business` SET BusinessType = @BusinessType, BusinessContent = @BusinessContent, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  WHERE Id = @Id ";
        const string UpdateQtySql = "UPDATE `manu_sfc_produce` SET    Qty = @Qty,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id ";
        const string DeleteSql = "delete from manu_sfc_produce where Id = @Id  ";
        const string DeleteRangeSql = "UPDATE `manu_sfc_produce` SET IsDeleted = Id ,UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids";
        const string GetByIdSql = @"SELECT * FROM `manu_sfc_produce`  WHERE Id = @Id ";
        const string GetByIdsSql = @"SELECT * FROM `manu_sfc_produce`  WHERE Id IN @ids ";
        const string GetSfcProduceBusinessBySFCIdSql = "SELECT * FROM manu_sfc_produce_business WHERE IsDeleted = 0 AND SfcProduceId = @SfcInfoId ";
        const string GetSfcProduceBusinessBySFCSql = @" SELECT SPB.* FROM manu_sfc_produce_business SPB  
                                          LEFT JOIN manu_sfc_produce sfc on sfc.Id =SPB.SfcProduceId 
                            WHERE SPB.IsDeleted = 0 AND SPB.BusinessType = @BusinessType AND SPB.SiteId=@SiteId AND SFC.SFC = @Sfc ";
        const string GetEntitiesSql = @" SELECT SPB.* FROM manu_sfc_produce_business SPB  
                                          LEFT JOIN manu_sfc_produce sfc on sfc.Id =SPB.SfcProduceId 
                            WHERE SPB.IsDeleted = 0 AND SPB.BusinessType = @BusinessType AND SPB.SiteId=@SiteId AND SFC.SFC IN @Sfcs ";
        const string GetSfcProduceBusinessBySFCsSql = @"SELECT SFC.Sfc,SPB.* FROM manu_sfc_produce_business SPB  
                                        LEFT JOIN manu_sfc_produce SFC on sfc.Id =SPB.SfcProduceId 
                            WHERE SPB.IsDeleted = 0 AND SPB.BusinessType = @BusinessType AND SPB.SiteId=@SiteId AND SFC.SFC IN @Sfcs ";
        const string GetSfcProduceBusinessBySFCIdsSql = "SELECT * FROM manu_sfc_produce_business WHERE SfcProduceId IN @SfcInfoIds  AND IsDeleted=0";
        const string GetBySFCSql = @"SELECT * FROM manu_sfc_produce WHERE SFC = @Sfc and SiteId=@SiteId ";
        const string DeletePhysicalSql = "DELETE FROM manu_sfc_produce WHERE SFC = @Sfc and SiteId=@SiteId ";
        const string DeletePhysicalRangeSql = "DELETE FROM manu_sfc_produce WHERE SiteId = @SiteId AND SFC IN @Sfcs ";
        const string DeletePhysicalRangeByIdsSql = "DELETE FROM manu_sfc_produce WHERE SiteId = @SiteId AND Id IN @Ids ";
        const string DeletePhysicalRangeByIdSql = "DELETE FROM manu_sfc_produce WHERE Id = @Id ";
        const string DeleteSfcProduceBusinessBySfcInfoIdSql = "DELETE FROM manu_sfc_produce_business WHERE SiteId = @SiteId AND SfcProduceId = @SfcInfoId";
        const string DeleteSfcProduceBusinessByIdSql = "DELETE FROM manu_sfc_produce_business WHERE Id in @Ids";
        const string DeleteSfcProduceBusinessBySfcInfoIdsSql = "DELETE FROM manu_sfc_produce_business WHERE SiteId = @SiteId AND SfcProduceId IN @SfcInfoIds";
        const string RealDeletesSfcProduceBusinessSql = "DELETE FROM manu_sfc_produce_business WHERE SfcProduceId IN @SfcInfoIds AND BusinessType=@BusinessType";
        const string InsertOrUpdateSfcProduceBusinessSql = @"INSERT INTO `manu_sfc_produce_business`(  `Id`, `SiteId`, `SfcProduceId`, `BusinessType`, `BusinessContent`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SfcProduceId, @BusinessType, @BusinessContent, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted ) ON DUPLICATE KEY UPDATE
                                                             BusinessContent = @BusinessContent,  UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted  ";
        //质量锁定sql
        const string UpdateQualityLockSql = "update  manu_sfc_produce set `Lock`=@Lock,LockProductionId=@LockProductionId,UpdatedBy = @UserId, UpdatedOn = @UpdatedOn where SFC in  @Sfcs  and SiteId=@SiteId ";
        const string UpdateIsScrapSql = "UPDATE `manu_sfc_produce` SET IsScrap = @IsScrap, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs and SiteId=@SiteId and IsScrap =@CurrentIsScrap  ";

        //在制维修 
        const string UpdateStatusSql = "UPDATE `manu_sfc_produce` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateStatusByIdSql = "UPDATE `manu_sfc_produce` SET Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id AND Status=@CurrentStatus ";
        const string UpdateProcedureIdSql = "UPDATE `manu_sfc_produce` SET ResourceId = @ResourceId, ProcessRouteId = @ProcessRouteId, ProcedureId = @ProcedureId, Status = @Status, IsRepair = @IsRepair, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdateProcedureAndResourceSql = "UPDATE `manu_sfc_produce` SET   ProcedureId = @ProcedureId,ResourceId=@ResourceId,UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs and SiteId=@SiteId ";

        //在制品步骤控制 
        const string UpdateProcedureAndStatusSql = "UPDATE `manu_sfc_produce` SET ProcedureId = @ProcedureId, ResourceId=@ResourceId,Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SFC in @Sfcs AND SiteId=@SiteId ";
        //不良录入修改工艺路线和工序信息

        const string CleanRepeatedCountSql = "UPDATE `manu_sfc_produce` SET RepeatedCount = 0,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id =@Id ";
        const string UpdateRouteSql = "UPDATE `manu_sfc_produce` SET ProcessRouteId = @ProcessRouteId, ProcedureId=@ProcedureId,Status=@Status,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id in @Ids ";
        const string UpdateRouteBIdSql = "UPDATE `manu_sfc_produce` SET ProcessRouteId = @ProcessRouteId, ProcedureId=@ProcedureId,Status=@Status,IsRepair=@IsRepair,RepeatedCount=0, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id= @Id ";
        const string LockSfcProcedureSql = "UPDATE `manu_sfc_produce`  SET  BeforeLockedStatus=Status,Status = @Status,UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE  SFC in  @Sfcs and SiteId=@SiteId ";
        const string UnLockSfcProcedureSql = "UPDATE `manu_sfc_produce`  SET Status = BeforeLockedStatus, BeforeLockedStatus=null,UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE  SFC in  @Sfcs and SiteId=@SiteId ";

        const string GetListBySfcsSql = @"SELECT * FROM manu_sfc_produce WHERE SFC in @Sfcs and SiteId=@SiteId ";

        const string GetListByProcedureIdAndResIdStatusSql = @"SELECT p.* FROM manu_sfc_produce p /**innerjoin**/ /**leftjoin**/ /**where**/ ";

        const string UpdateStatusAndQtyBySfcsSql = @"UPDATE `manu_sfc_produce` SET Status = @Status, Qty = @Qty ,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE SiteId = @SiteId AND SFC IN @SFCs ";
        const string UpdateQtyByIdSql = @"UPDATE `manu_sfc_produce` SET  Qty = @Qty ,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id =  @Id  ";

        #region 顷刻

        /// <summary>
        /// 更新条码数量
        /// </summary>
        const string UpdateQtyBySfcsSql = @"UPDATE manu_sfc_produce SET Qty = @Qty ,UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE SiteId = @SiteId AND SFC = @SFC ";

        /// <summary>
        /// 获取设备最新条码
        /// </summary>
        const string GetEquipmentNewestSfcsql = @"
            select t2.MaterialCode , t1.* 
            from manu_sfc_produce t1
            inner join proc_material t2 on t1.ProductId = t2.Id and t2.IsDeleted = 0
            where t1.EquipmentId = @EquipmentId
            and t1.IsDeleted = 0
            order by t1.CreatedOn desc 
            limit 0,1
        ";

        #endregion
    }
}
