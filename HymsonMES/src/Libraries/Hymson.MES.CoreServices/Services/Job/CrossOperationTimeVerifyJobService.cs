using FluentValidation;
using FluentValidation.Results;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 跨工序时间校验
    /// </summary>
    [Job("跨工序时间校验", JobTypeEnum.Standard)]
    public class CrossOperationTimeVerifyJobService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// 仓储接口（跨工序时间管控）
        /// </summary>
        private readonly IProcProcedureTimeControlRepository _procProcedureTimeControlRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="masterDataService"></param>
        /// <param name="procProcedureTimeControlRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        public CrossOperationTimeVerifyJobService(IMasterDataService masterDataService,
            IProcProcedureTimeControlRepository procProcedureTimeControlRepository,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _masterDataService = masterDataService;
            _procProcedureTimeControlRepository = procProcedureTimeControlRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
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
            if (commonBo == null || commonBo.Proxy == null) return default;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16370));
            }

            // 查询当前工序是否存在工序时间管控设置
            var timeControlEntities = await _procProcedureTimeControlRepository.GetEntitiesAsync(new ProcProcedureTimeControlQuery
            {
                ToProcedureId = commonBo.ProcedureId,
                SiteId = commonBo.SiteId
            });
            if (timeControlEntities == null || !timeControlEntities.Any()) return default;

            // 通过产品分组（工序时间管控）
            var timeControlDict = timeControlEntities.ToLookup(t => t.ProductId).ToDictionary(d => d.Key, d => d);

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.InStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            // 通过产品分组（在制条码）
            var sfcProduceDict = sfcProduceEntities.ToLookup(s => s.ProductId).ToDictionary(d => d.Key, d => d);

            // 获取当前时间
            var now = HymsonClock.Now();

            // 检索产品ID相同的工序时间管控设置
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in sfcProduceDict)
            {
                if (!timeControlDict.TryGetValue(item.Key, out var timeControls)) continue;
                if (timeControls == null || !timeControls.Any()) continue;

                // 读取条码步骤（因为步骤表有分表操作，暂时一个一个去查询吧）
                foreach (var sfcProduceEntity in item.Value)
                {
                    var stepEntities = await _manuSfcStepRepository.GetOutStationStepsBySFCAsync(new EntityBySFCQuery
                    {
                        SFC = sfcProduceEntity.SFC,
                        SiteId = commonBo.SiteId
                    });
                    if (stepEntities == null || !stepEntities.Any()) continue;

                    // 存在起始工序对应的步骤（取最新的一条）
                    var lastStepEntity = stepEntities.LastOrDefault(w => timeControls.Any(a => a.FromProcedureId == w.ProcedureId));
                    if (lastStepEntity == null) continue;

                    // 获取起始工序对应的工序时间管控设置
                    var timeControl = timeControls.FirstOrDefault(w => w.FromProcedureId == lastStepEntity.ProcedureId);
                    if (timeControl == null) continue;

                    var validationFailure = new ValidationFailure() { FormattedMessagePlaceholderValues = new() };
                    validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfcProduceEntity.SFC);
                    validationFailure.FormattedMessagePlaceholderValues.Add("SFC", sfcProduceEntity.SFC);

                    // 开始时间不符合跨工序时间校验规则，小于下限值！
                    if (lastStepEntity.CreatedOn.AddMinutes(timeControl.LowerLimitMinute) > now)
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("Value", timeControl.LowerLimitMinute);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16385);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    // 开始时间不符合跨工序时间校验规则，大于上限值！
                    if (lastStepEntity.CreatedOn.AddMinutes(timeControl.UpperLimitMinute) < now)
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("Value", timeControl.UpperLimitMinute);
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16386);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }

            return default;
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
