using Hymson.Localization.Services;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.ManuCommon;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 不良录入
    /// </summary>
    [Job("不良录入", JobTypeEnum.Standard)]
    public class BadRecordJobService : IJobService
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
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuCommonService"></param>
        /// <param name="masterDataService"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="localizationService"></param>
        public BadRecordJobService(IManuCommonService manuCommonService,
            IMasterDataService masterDataService,
            IManuSfcProduceRepository manuSfcProduceRepository,
            ILocalizationService localizationService)
        {
            _manuCommonService = manuCommonService;
            _masterDataService = masterDataService;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<BadRecordRequestBo>();
            if (bo == null) return;

            // 获取生产条码信息
            var sfcProduceEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceEntitiesBySFCsWithCheckAsync, bo);
            if (sfcProduceEntities == null || sfcProduceEntities.Any() == false) return;

            var sfcProduceBusinessEntities = await bo.Proxy.GetValueAsync(_masterDataService.GetProduceBusinessEntitiesBySFCsAsync, bo);

            // 合法性校验
            sfcProduceEntities.VerifySFCStatus(SfcProduceStatusEnum.Activity, _localizationService.GetResource($"{typeof(SfcProduceStatusEnum).FullName}.{nameof(SfcProduceStatusEnum.Activity)}"))
                              .VerifyProcedure(bo.ProcedureId)
                              .VerifyResource(bo.ResourceId);
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<InStationRequestBo>();
            if (bo == null) return null;
            return await _masterDataService.GetJobRalationJobByProcedureIdOrResourceId(new Bos.Common.MasterData.JobRelationBo
            {
                ProcedureId = bo.ProcedureId,
                ResourceId = bo.ResourceId,
            });
        }


        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            var bo = param.ToBo<BadRecordRequestBo>();
            if (bo == null) return default;

            // 待执行的命令
            BadRecordResponseBo responseBo = new();

            // 获取维修业务
            var sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessEntitiesBySFCAsync(new SfcListProduceBusinessQuery
            {
                SiteId = bo.SiteId,
                Sfcs = bo.SFCs,
                BusinessType = ManuSfcProduceBusinessType.Repair
            });

            responseBo.IsShow = sfcProduceBusinessEntities.Any() == false;
            return responseBo;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not BadRecordResponseBo data) return responseBo;

            responseBo.Content.Add("IsShow", $"{data.IsShow}");
            return await Task.FromResult(responseBo);
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
