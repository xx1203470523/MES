using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode;
using Org.BouncyCastle.Asn1.Ocsp;

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
        /// 
        /// </summary>
        public InStationInterceptJobService(IManuProductBadRecordRepository manuProductBadRecordRepository, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository)
        {
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
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
            await Task.CompletedTask;
            return null;
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
            var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                SiteId = commonBo.SiteId,
                Sfcs = commonBo.InStationRequestBos?.Select(s => s.SFC),
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
            return null;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            await Task.CompletedTask;
            return null;
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
