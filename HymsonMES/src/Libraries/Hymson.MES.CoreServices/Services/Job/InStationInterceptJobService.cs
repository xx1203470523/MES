using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Quality;

namespace Hymson.MES.CoreServices.Services.Job
{
    [Job("进站拦截", JobTypeEnum.Standard)]
    public class InStationInterceptJobService : IJobService
    {
        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;
        /// <summary>
        /// 
        /// </summary>
        public InStationInterceptJobService(IManuProductBadRecordRepository manuProductBadRecordRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IMasterDataService masterDataService)
        {
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
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
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, new MultiSFCBo { SFCs = commonBo!.InStationRequestBos!.Select(s => s.SFC), SiteId = commonBo.SiteId });
            var sfcs = sfcProduceEntities!.Where(x => (x.IsRepair ?? TrueOrFalseEnum.No) == TrueOrFalseEnum.No)?.Select(p => p.SFC);
            if (sfcs != null && sfcs.Any())
            {
                var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
                {
                    SiteId = commonBo.SiteId,
                    SFCs = sfcs,
                    Status = ProductBadRecordStatusEnum.Open
                });
                if (manuProductBadRecordEntities != null && manuProductBadRecordEntities.Any())
                {
                    var qualUnqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByIdsAsync(manuProductBadRecordEntities.Select(x => x.UnqualifiedId));
                    if (qualUnqualifiedCodeEntity != null && qualUnqualifiedCodeEntity.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17108)).WithData("SFCs", string.Join(",", manuProductBadRecordEntities.Select(x => x.SFC).Distinct()));
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            return await Task.FromResult<JobResponseBo?>(default);
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }
    }
}
