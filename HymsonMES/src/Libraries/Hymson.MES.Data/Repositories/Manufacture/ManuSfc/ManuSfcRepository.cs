using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Microsoft.Extensions.Options;
using System.Text;
using ConnectionOptions = Hymson.MES.Data.Options.ConnectionOptions;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码表仓储
    /// </summary>
    public partial class ManuSfcRepository : BaseRepository, IManuSfcRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        /// <param name="memoryCache"></param>
        public ManuSfcRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 方法

        /// <summary>
        /// 批量删除（软删除）
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
        public async Task<ManuSfcEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetByIdsAsync(IEnumerable<long> ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetByIdsSql, new { Ids = ids });
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

            using var conn = GetMESDbConnection();
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
            using var conn = GetMESDbConnection();
            sqlBuilder.AddParameters(query);
            if (!string.IsNullOrEmpty(query.code))
            {
                string procedureIdSql = $"SELECT * FROM `proc_procedure` WHERE proc_procedure.`Code` = '{query.code}' ";
                var procedureId = await conn.QueryAsync<ManuSfcProduceSelectView>(procedureIdSql, templateData.Parameters);
                if (procedureId.Count() > 0)
                {
                    var procedureIdList = procedureId.ToList();
                    //工序
                    int a = 0;

                    StringBuilder pSqlBuilder = new StringBuilder();
                    foreach (var item in procedureIdList)
                    {
                        a++;
                        if (a == 1)
                        {
                            pSqlBuilder.Append($"(msp.ProcedureId = {item.Id} ");
                        }
                        else
                        {
                            pSqlBuilder.Append($"OR msp.ProcedureId = {item.Id} ");
                        }
                    }
                    pSqlBuilder.Append(")");
                    string prSql = pSqlBuilder.ToString();
                    sqlBuilder.Where(prSql);
                }
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

            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcProduceSelectView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcProduceSelectView>(manuSfcProduceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 分页查询（查询所有条码信息-不查询在制表）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcAboutInfoView>> GetManuSfcAboutInfoPagedAsync(ManuSfcAboutInfoPagedQuery query)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);

            sqlBuilder.Where("ms.SiteId = @SiteId");
            sqlBuilder.Where("ms.IsDeleted=0");

            sqlBuilder.Select(@" ms.*, 
                                msi.WorkOrderId,msi.ProductId,
                                pwo.OrderCode as WorkOrderCode, 
                                msi.ProcessRouteId, msi.ProductBomId
            ");

            sqlBuilder.InnerJoin("manu_sfc_info  msi on ms.Id=msi.SfcId AND msi.IsUsed=1 AND msi.IsDeleted=0");
            sqlBuilder.LeftJoin(" plan_work_order pwo on msi.WorkOrderId = pwo.id AND pwo.IsDeleted=0 ");


            if (!string.IsNullOrEmpty(query.Sfc))
            {
                query.Sfc = $"%{query.Sfc}%";
                sqlBuilder.Where("ms.Sfc like  @Sfc");
            }
            if (!string.IsNullOrEmpty(query.SfcHard))
            {
                sqlBuilder.Where("ms.Sfc = @SfcHard ");
            }
            if (query.WorkOrderId.HasValue && query.WorkOrderId > 0)
            {
                sqlBuilder.Where("msi.WorkOrderId = @WorkOrderId");
            }
            if (query.Status.HasValue)
            {
                sqlBuilder.Where("ms.Status = @Status");
            }
            if (query.MaterialId.HasValue && query.MaterialId > 0)
            {
                sqlBuilder.Where("msi.ProductId = @MaterialId");
            }
            if (query.ProcessRouteId.HasValue && query.ProcessRouteId > 0)
            {
                sqlBuilder.Where("msi.ProcessRouteId = @ProcessRouteId");
            }
            if (query.Sfcs != null && query.Sfcs.Any())
            {
                sqlBuilder.Where("ms.Sfc in  @Sfcs");
            }

            var offSet = (query.PageIndex - 1) * query.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = query.PageSize });
            sqlBuilder.AddParameters(query);

            using var conn = GetMESDbConnection();
            var manuSfcProduceEntitiesTask = conn.QueryAsync<ManuSfcAboutInfoView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ManuSfcAboutInfoView>(manuSfcProduceEntities, query.PageIndex, query.PageSize, totalCount);
        }

        /// <summary>
        /// 根据SFC查询条码信息-不查询在制表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSfcAboutInfoView> GetManSfcAboutInfoBySfcAsync(ManuSfcAboutInfoBySfcQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcAboutInfoView>(GetManSfcAboutInfoBySfcSql, query);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetManuSfcEntitiesAsync(EntityBySFCsQuery manuSfcQuery)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetSFCsSql, manuSfcQuery);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetAllManuSfcEntitiesAsync(EntityBySFCsQuery manuSfcQuery)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetAllSFCsSql, manuSfcQuery);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ManuSfcEntity manuSfcEntity)
        {
            using var conn = GetMESDbConnection();
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

            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, manuSfcEntitys);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ManuSfcEntity manuSfcEntity)
        {
            using var conn = GetMESDbConnection();
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

            using var conn = GetMESDbConnection();
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

            using var conn = GetMESDbConnection();
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
            //sqlBuilder.Select("*");
            sqlBuilder.Where("sfc.SiteId=@SiteId ");
            if (param.Sfcs != null && param.Sfcs.Any())
            {
                sqlBuilder.Where("sfc.SFC in @Sfcs");
            }
            if (param.Statuss != null && param.Statuss.Any())
            {
                sqlBuilder.Where("sfc.Status in @Statuss");
            }
            using var conn = GetMESDbConnection();
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
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusSql, command);
        }

        /// <summary>
        /// 批量更新条码（使用状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> MultiUpdateSfcIsUsedAsync(MultiSfcUpdateIsUsedCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(MultiUpdateSfcIsUsedSql, command);
        }

        /// <summary>
        /// 批量更新条码（条码状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> MultiUpdateSfcStatusAsync(MultiSFCUpdateStatusCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(MultiUpdateStatusSql, command);
        }

        /// <summary>
        /// 批量更新条码（条码状态与使用状态）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateSfcStatusAndIsUsedAsync(ManuSfcUpdateStatusAndIsUsedCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusAndIsUsedSql, command);
        }

        /// <summary>
        /// 批量更新进站条码状态和在用状态
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> InStationManuSfcByIdAsync(IEnumerable<InStationManuSfcByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateInStationStatusSql, commands);
        }

        /// <summary>
        /// 批量更新条码（每个条码状态都不一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatusBySfcsAsync(IEnumerable<ManuSfcUpdateStatusCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ManuSfcUpdateStatusCommandSql, commands);
        }

        /// <summary>
        /// 批量更新条码（更具Id 状态更新为一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatuByIdsAsync(ManuSfcUpdateStatusByIdsCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ManuSfcUpdateStatuByIdsSql, command);
        }

        /// <summary>
        /// 批量更新条码状态（根据Id 更新状态 更新状态为不一致）
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatuByIdRangeAsync(IEnumerable<ManuSfcUpdateStatusByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ManuSfcUpdateStatuByIdSql, commands);
        }

        /// <summary>
        ///更新条码状态（根据Id 更新状态 更新状态为不一致）
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcUpdateStatuByIdAsync(ManuSfcUpdateStatusByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ManuSfcUpdateStatuByIdSql, command);
        }

        /// <summary>
        /// 条码报废
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcScrapByIdsAsync(IEnumerable<ScrapManuSfcByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ManuSfcScrapByIdsSql, commands);
        }

        /// <summary>
        ///取消条码报废
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> ManuSfcCancellScrapByIdsAsync(IEnumerable<CancelScrapManuSfcByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(ManuSfcCancellScrapByIdsSql, commands);
        }

        /// <summary>
        /// 更新条码数量
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuSfcQtyByIdRangeAsync(IEnumerable<UpdateManuSfcQtyByIdCommand> commands)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateManuSfcQtyByIdSql, commands);
        }

        /// <summary>
        /// 获取SFC
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuSfcEntity> GetBySFCAsync(EntityBySFCQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<ManuSfcEntity>(GetBySFCSql, query);
        }

        /// <summary>
        /// 获取SFC（批量）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetAllBySFCsAsync(EntityBySFCsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetAllBySFCsql, query);
        }

        /// <summary>
        /// 获取在制SFC（批量）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetProduceBySFCsAsync(EntityBySFCsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetProduceBySFCsql, query);
        }

        /// <summary>
        /// 获取SFC（批量）
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetBySFCsAsync(EntityBySFCsQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetBySFCsNewSql, query);
        }

        /// <summary>
        /// 根据sfc 获取条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuSfcEntity>> GetBySFCsAsync(IEnumerable<string> sfcs)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<ManuSfcEntity>(GetBySFCsSql, new { SFCs = sfcs });
        }

        /// <summary>
        /// 根据SFCs设置条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAndQtyBySfcsAsync(UpdateStatusAndQtyBySfcsCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusAndQtyBySfcsSql, command);
        }

        /// <summary>
        /// 根据Id条码状态与数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateStatusAndQtyByIdAsync(UpdateStatusAndQtyByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateStatusAndQtyByIdSql, command);
        }

        /// <summary>
        /// 根据Id条码数量
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateManuSfcQtyAndCurrentQtyVerifyByIdAsync(UpdateManuSfcQtyAndCurrentQtyVerifyByIdCommand command)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateManuSfcQtyAndCurrentQtyVerifyByIdSql, command);
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
                                            sfc.Id ,sfc.SiteId ,sfc.SFC ,sfc.Qty ,sfc.Status ,
                                            info.Id AS SFCInfoId, info.WorkOrderId ,info.ProductId,info.ProcessRouteId ,info.ProductBOMId  ,info.IsUsed  
                                        FROM manu_sfc sfc 
                                        LEFT JOIN  manu_sfc_info info on sfc.Id =info.SfcId  and info.IsUsed =1
                                            /**where**/  ";

        const string InsertSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `manu_sfc`(  `Id`, `SiteId`, `SFC`, `Qty`, `Status`, IsUsed, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @SFC, @Qty, @Status, @IsUsed, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";

        const string UpdateSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `manu_sfc` SET IsUsed = @IsUsed, SFC = @SFC, Qty = @Qty, Status = @Status, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn, IsDeleted = @IsDeleted WHERE Id = @Id ";
        const string UpdateStatusSql = "UPDATE `manu_sfc` SET Status = @Status, UpdatedBy = @UserId, UpdatedOn = @UpdatedOn  WHERE SiteId = @SiteId AND Status <> @Status AND SFC IN @Sfcs ";
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

        const string GetBySFCSql = @"SELECT * FROM `manu_sfc` WHERE SiteId = @SiteId AND SFC = @SFC";
        const string GetProduceBySFCsql = @"SELECT * FROM `manu_sfc` WHERE SiteId = @SiteId AND SFC IN @SFCs   AND Type=1";
        const string GetAllBySFCsql = @"SELECT * FROM `manu_sfc` WHERE SiteId = @SiteId AND IsDeleted = 0 AND SFC IN @SFCs ";
        const string GetBySFCsNewSql = @"SELECT * FROM `manu_sfc` WHERE SiteId = @SiteId AND SFC IN @SFCs";
        const string GetBySFCsSql = @"SELECT * FROM `manu_sfc`  WHERE SFC IN @SFCs AND IsDeleted=0 ";
        const string GetSFCsSql = @"SELECT * FROM manu_sfc WHERE SiteId = @SiteId AND IsDeleted = 0 AND SFC IN @SFCs; ";
        const string GetAllSFCsSql = @"SELECT * FROM manu_sfc WHERE SiteId = @SiteId AND IsDeleted = 0 AND SFC IN @SFCs ; ";
        const string GetManSfcAboutInfoBySfcSql = @"SELECT ms.*, 
                                msi.WorkOrderId,msi.ProductId,
                                pwo.OrderCode as WorkOrderCode, 
                                msi.ProcessRouteId, msi.ProductBOMId   -- 231219 修改，取 manu_sfc_info 里的才是最新的
                                                    FROM manu_sfc MS 
                                     INNER JOIN manu_sfc_info  msi on ms.Id=msi.SfcId AND msi.IsUsed=1 AND msi.IsDeleted=0
                                     LEFT JOIN  plan_work_order pwo on msi.WorkOrderId = pwo.id AND pwo.IsDeleted=0
                                     WHERE  ms.IsDeleted=0 and ms.SiteId =@SiteId and ms.Sfc=@Sfc
                                                  ";
        const string UpdateStatusAndQtyBySfcsSql = @"UPDATE `manu_sfc` SET Status=@Status, Qty=@Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE  SFC IN @SFCs AND SiteId = @SiteId ";
        const string UpdateStatusAndQtyByIdSql = @"UPDATE `manu_sfc` SET Status=@Status, Qty=@Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE  Id=@Id  AND Status=@CurrentStatus AND Qty=@CurrentQty ";
        const string UpdateManuSfcQtyAndCurrentQtyVerifyByIdSql = @"UPDATE `manu_sfc` SET  Qty=@Qty, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE  Id=@Id  AND Status=@CurrentStatus AND Qty=@CurrentQty ";
    }
}
