using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcStep.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.Utils.Tools;
using Hymson.WaterMark;

namespace Hymson.MES.BackgroundServices.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkOrderStatisticService : IWorkOrderStatisticService
    {
        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 统计服务
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waterMarkService"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public WorkOrderStatisticService(IWaterMarkService waterMarkService,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _waterMarkService = waterMarkService;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 执行统计
        /// </summary>
        /// <param name="limitCount"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(int limitCount = 1000)
        {
            var waterMarkId = await _waterMarkService.GetWaterMarkAsync(BusinessKey.WorkOrderStatistic);

            // 获取步骤表数据
            var manuSfcStepList = await _manuSfcStepRepository.GetListByStartWaterMarkIdAsync(new ManuSfcStepStatisticQuery
            {
                StartWaterMarkId = waterMarkId,
                Rows = limitCount
            });
            if (manuSfcStepList == null || !manuSfcStepList.Any()) return;

            var user = $"{BusinessKey.WorkOrderStatistic}作业";
            List<UpdateQtyByWorkOrderIdCommand> updateInputQtyCommands = new();
            List<UpdateQtyByWorkOrderIdCommand> updateFinishQtyCommands = new();

            // 取得集合里面条码对应的表名
            var tableNameList = _manuSfcStepRepository.GetTableNames(manuSfcStepList);

            // 通过站点分组（如果是单站点项目，只会一条记录）
            var manuSfcStepForSiteIdDic = manuSfcStepList.ToLookup(x => x.SiteId).ToDictionary(d => d.Key, d => d);

            // 查询步骤时需要站点，因为相同的条码不一定是同一站点
            List<Task<IEnumerable<ManuSfcStepEntity>>> tasks = new();
            foreach (var siteId in manuSfcStepForSiteIdDic)
            {
                foreach (var table in tableNameList)
                {
                    tasks.Add(_manuSfcStepRepository.GetInOutStationStepsBySFCsAsync(table.Key, new EntityBySFCsQuery
                    {
                        SiteId = siteId.Key,
                        SFCs = table.Value.Where(w => w.SiteId == siteId.Key).Select(s => s.SFC)
                    }));
                }
            }

            // 取得所有的条码步骤（条码是在水位处取得）
            var allStepArray = await Task.WhenAll(tasks);
            var allStepEntities = allStepArray.SelectMany(s => s);

            // 通过工单对条码进行分组
            var manuSfcStepForWorkOrderDic = allStepEntities.ToLookup(x => x.WorkOrderId).ToDictionary(d => d.Key, d => d);
            foreach (var item in manuSfcStepForWorkOrderDic)
            {
                // 投入数量（数量那里分组，只是为了去掉重复进站）
                updateInputQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = item.Value.Where(w => w.CurrentStatus == SfcStatusEnum.lineUp).DistinctBy(d => d.SFCInfoId).Count(),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Min(w => w.CreatedOn)
                });

                // 产出数量（数量那里分组，只是为了去掉重复出站）
                updateFinishQtyCommands.Add(new UpdateQtyByWorkOrderIdCommand
                {
                    WorkOrderId = item.Key,
                    Qty = item.Value.Where(w => w.AfterOperationStatus == SfcStatusEnum.Complete).DistinctBy(d => d.SFCInfoId).Count(),
                    UpdatedBy = user,
                    UpdatedOn = item.Value.Max(w => w.CreatedOn)
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();

            // 更新工单的投入数量
            await _planWorkOrderRepository.UpdateInputQtyByWorkOrderIdsAsync(updateInputQtyCommands);

            // 更新工单的产出数量
            await _planWorkOrderRepository.UpdateFinishProductQuantityByWorkOrderIdsAsync(updateFinishQtyCommands);

            // 更新水位
            await _waterMarkService.RecordWaterMarkAsync(BusinessKey.WorkOrderStatistic, manuSfcStepList.Max(x => x.Id));
            trans.Complete();

        }

    }
}
