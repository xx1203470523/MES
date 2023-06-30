using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 出站验证
    /// </summary>
    [Job("出站验证", JobTypeEnum.Standard)]
    public class OutStationVerifyJobService : IJobService
    {
        /// <summary>
        /// 服务接口（生产通用）
        /// </summary>
        private readonly IManuCommonService _manuCommonService;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        public OutStationVerifyJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;

            if ((param is OutStationRequestBo bo) == false) return;

            // 获取生产条码信息
            var sfcProduceEntities = await param.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await param.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.Activity)
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);

            //（前提：这些条码都是同一工单同一工序）
            var firstProduceEntity = sfcProduceEntities.FirstOrDefault();
            if (firstProduceEntity == null) return;

            // 获取生产工单（附带工单状态校验）
            _ = await param.Proxy.GetValueAsync(async parameters =>
            {
                long workOrderId = (long)parameters[0];
                bool isVerifyActivation = parameters.Length <= 1 || (bool)parameters[1];
                return await _masterDataService.GetProduceWorkOrderByIdAsync(workOrderId, isVerifyActivation);
            }, new object[] { firstProduceEntity.WorkOrderId, true });

            // 验证BOM主物料数量
            await _manuCommonService.VerifyBomQtyAsync(new ManuProcedureBomBo
            {
                SiteId = bo.SiteId,
                SFCs = bo.SFCs,
                ProcedureId = bo.ProcedureId,
                BomId = firstProduceEntity.ProductBOMId
            });
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行入库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return 0;
        }

    }
}
