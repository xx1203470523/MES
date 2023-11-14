using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common.MasterData;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.NewJob
{
    /// <summary>
    /// 产品分选
    /// </summary>
    [Job("产品分选", JobTypeEnum.Standard)]
    public class ProductsSortingJobService : IJobService
    {
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 分选规则
        /// </summary>
        private readonly IProcSortingRuleRepository _sortingRuleRepository;
        private readonly IProcSortingRuleDetailRepository _sortingRuleDetailRepository;
        private readonly IProcSortingRuleGradeRepository _sortingRuleGradeRepository;
        private readonly IProcSortingRuleGradeDetailsRepository _ruleGradeDetailsRepository;

        /// <summary>
        /// 条码档位表 仓储
        /// </summary>
        private readonly IManuSfcGradeRepository _manuSfcGradeRepository;

        /// <summary>
        /// 条码档位明细表 仓储
        /// </summary>
        private readonly IManuSfcGradeDetailRepository _gradeDetailRepository;

        private readonly IMasterDataService _masterDataService;

        public ProductsSortingJobService(IManuSfcRepository manuSfcRepository,
            IProcSortingRuleRepository sortingRuleRepository,
            IManuSfcGradeRepository manuSfcGradeRepository,
            IManuSfcGradeDetailRepository gradeDetailRepository,
            IMasterDataService masterDataService)
        {
            _manuSfcRepository = manuSfcRepository;
            _sortingRuleRepository = sortingRuleRepository;
            _manuSfcGradeRepository = manuSfcGradeRepository;
            _gradeDetailRepository = gradeDetailRepository;
            _masterDataService = masterDataService;
        }

        /// <summary>
        /// 参数校验
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            if (param is not JobRequestBo commonBo) return;
            if (commonBo == null) return;
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return;

            var sfcs = commonBo.InStationRequestBos.Select(s => s.SFC);
            // 验证DTO
            if (!sfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = sfcs };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            //判断条码的分选规则信息
            var productIds = sfcProduceEntities.Select(x => x.ProductId).Distinct().ToArray();
            //根据物料找到分选规则
            var query = new ProcSortingRuleQuery
            {
                SiteId = commonBo.SiteId,
                Status = Core.Enums.SysDataStatusEnum.Enable,
                IsDefaultVersion= true,
                MaterialIds=productIds
            };
            var procSortingRules = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetSortingRulesAsync, query);
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
            if (commonBo.InStationRequestBos == null || !commonBo.InStationRequestBos.Any()) return default;

            var sfcs = commonBo.InStationRequestBos.Select(s => s.SFC);
            // 验证DTO
            if (!sfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = sfcs };
            // 获取条码信息
            var sfcProduceEntities = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                return default;
            }

            //判断条码的分选规则信息
            var productIds = sfcProduceEntities.Select(x => x.ProductId).Distinct().ToArray();
            //根据物料找到分选规则
            var query = new ProcSortingRuleQuery
            {
                SiteId = commonBo.SiteId,
                Status = Core.Enums.SysDataStatusEnum.Enable,
                IsDefaultVersion = true,
                MaterialIds = productIds
            };
            var procSortingRules = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetSortingRulesAsync, query);

            //为了不报错
            await Task.CompletedTask;
            return default;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            //为了不报错
            await Task.CompletedTask;
            return null;
        }

        /// <summary>
        /// 执行后节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            await Task.CompletedTask;
            return null;
        }
    }
}