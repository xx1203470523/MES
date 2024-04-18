using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// Marking拦截作业
    /// </summary>
    [Job("Marking拦截", JobTypeEnum.Standard)]
    public class InterceptMarkingJobService : IJobService
    {
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="procProcedureRepository"></param>
        public InterceptMarkingJobService(IManuSfcProduceRepository manuSfcProduceRepository, IManuProductBadRecordRepository manuProductBadRecordRepository, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IProcProcedureRepository procProcedureRepository)
        {
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _procProcedureRepository = procProcedureRepository;
        }

        /// <summary>
        /// 执行前节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult<IEnumerable<JobBo>?>(default);
        }

        /// <summary>
        /// 数据组装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {
            return await Task.FromResult(new EmptyRequestBo { });
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            return await Task.FromResult<JobResponseBo?>(default);
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return;

            // 临时中转变量 commonBo.InStationRequestBos.Select(s => s.SFC)
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            //获取条码生产信息,判断条码是否在制品
            var sfcProduceEntities = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(new ManuSfcProduceQuery { Sfcs= multiSFCBo.SFCs, SiteId= multiSFCBo.SiteId});
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            { 
                return;
            }
            //var procedureIds = sfcProduceEntities.Select(a => a.ProcedureId).Distinct();
            ////判断当前工序是否排队在制品
            //if (!procedureIds.Contains(commonBo.ProcedureId))
            //{
            //    return;
            //}
            else {
                //查询条码和工序是否在需要拦截
                var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery { SFCs = multiSFCBo.SFCs, InterceptOperationId = commonBo.ProcedureId, Status = ProductBadRecordStatusEnum.Open,SiteId= multiSFCBo.SiteId });
                if (manuProductBadRecordEntities == null || !manuProductBadRecordEntities.Any())
                {
                    return;
                }
                else {
                    var unqualifiedIds = manuProductBadRecordEntities.Select(a => a.UnqualifiedId).Distinct();
                    var qualUnqualifiedEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(unqualifiedIds);

                    var procedureEntity = await _procProcedureRepository.GetByIdAsync(commonBo.ProcedureId);

                    throw new CustomerValidationException(nameof(ErrorCode.MES19713)).WithData("sfc", string.Join(",", multiSFCBo.SFCs)).WithData("produceCode",procedureEntity?.Name??"").WithData("unqualifiedCode", string.Join(",", qualUnqualifiedEntities.Select(a=>a.UnqualifiedCodeName).Distinct()));
                }
            }
        }
    }
}
