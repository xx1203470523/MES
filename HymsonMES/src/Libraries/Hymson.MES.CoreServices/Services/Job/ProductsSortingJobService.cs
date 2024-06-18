using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Data;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// 产品分选
    /// </summary>
    [Job("产品分选", JobTypeEnum.Standard)]
    public class ProductsSortingJobService : IJobService
    {
        /// <summary>
        /// 条码档位表 仓储
        /// </summary>
        private readonly IManuSfcGradeRepository _manuSfcGradeRepository;

        /// <summary>
        /// 条码档位明细表 仓储
        /// </summary>
        private readonly IManuSfcGradeDetailRepository _gradeDetailRepository;

        /// <summary>
        /// 分选规则详情
        /// </summary>
        private readonly IProcSortingRuleDetailRepository _sortingRuleDetailRepository;
        private readonly IProcSortingRuleGradeRepository _sortingRuleGradeRepository;
        private readonly IProcSortingRuleGradeDetailsRepository _ruleGradeDetailsRepository;
        /// <summary>
        ///工序
        /// </summary>
        private readonly IProcProcedureRepository _procedureRepository;

        private readonly IMasterDataService _masterDataService;

        public ProductsSortingJobService(IManuSfcGradeRepository manuSfcGradeRepository,
            IManuSfcGradeDetailRepository gradeDetailRepository,
            IProcSortingRuleDetailRepository sortingRuleDetailRepository,
            IProcSortingRuleGradeRepository sortingRuleGradeRepository,
            IProcSortingRuleGradeDetailsRepository ruleGradeDetailsRepository,
            IProcProcedureRepository procedureRepository,
            IMasterDataService masterDataService)
        {
            _manuSfcGradeRepository = manuSfcGradeRepository;
            _gradeDetailRepository = gradeDetailRepository;
            _sortingRuleDetailRepository = sortingRuleDetailRepository;
            _sortingRuleGradeRepository = sortingRuleGradeRepository;
            _ruleGradeDetailsRepository = ruleGradeDetailsRepository;
            _procedureRepository = procedureRepository;
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
            if (commonBo.Proxy == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }
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
                Status = SysDataStatusEnum.Enable,
                IsDefaultVersion = true,
                MaterialIds = productIds
            };

            //分选规则找不到报错
            var procSortingRules = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetSortingRulesAsync, query);
            if (procSortingRules == null || !procSortingRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11309));
            }

            //获取到条码的参数信息
            var parameterBySfcQuery = new ManuProductParameterBySfcQuery
            {
                SiteId = commonBo.SiteId,
                SFCs = sfcs
            };
            var parameterList = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProductParameterBySfcsAsync, parameterBySfcQuery);
            if (parameterList == null || !parameterList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16364));
            }

            //档位和最终档次信息算不出来报错
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
            if (commonBo.Proxy == null)
            {
                return default;
            }
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
                Status = SysDataStatusEnum.Enable,
                IsDefaultVersion = true,
                MaterialIds = productIds
            };
            var procSortingRules = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetSortingRulesAsync, query);
            if (procSortingRules == null || !procSortingRules.Any())
            {
                return default;
            }

            //分选规则详细
            var sortingRuleIds = procSortingRules.Select(x => x.Id).ToArray();
            var sortingRuleDetailEntities = await _sortingRuleDetailRepository.GetProcSortingRuleDetailEntitiesAsync(new ProcSortingRuleDetailQuery
            {
                SiteId = commonBo.SiteId,
                SortingRuleIds = sortingRuleIds
            });
            if (sortingRuleDetailEntities == null || !sortingRuleDetailEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11309));
            }

            sfcs = sfcProduceEntities.Select(x => x.SFC).ToList();
            //获取到条码的参数信息
            var parameterBySfcQuery = new ManuProductParameterBySfcQuery
            {
                SiteId = commonBo.SiteId,
                SFCs = sfcs
            };
            var parameterList = await commonBo.Proxy.GetDataBaseValueAsync(_masterDataService.GetProductParameterBySfcsAsync, parameterBySfcQuery);
            if (parameterList == null || !parameterList.Any())
            {
                return default;
            }

            var insertGrades = new List<ManuSfcGradeEntity>();
            var gradeDetailEntities = new List<ManuSfcGradeDetailEntity>();
            var sfcGradeDetails = new List<ManuSfcGradeDetailEntity>();
            var ruleDetailIds = new List<long>();
            var gradeId = IdGenProvider.Instance.CreateId();

            var procedureIdList = sortingRuleDetailEntities.Select(x => x.ProcedureId).Distinct().ToList();
            var procProcedures = await _procedureRepository.GetByIdsAsync(procedureIdList);
            foreach (var sfc in sfcs)
            {
                //条码的分选规则
                var productId = sfcProduceEntities.FirstOrDefault(x => x.SFC == sfc)?.ProductId;
                var sortRuleId = procSortingRules.FirstOrDefault(x => x.MaterialId == productId)?.Id ?? 0;
                var sortingRuleDetails = sortingRuleDetailEntities.Where(x => x.SortingRuleId == sortRuleId).ToList();
                if (sortingRuleDetails == null || !sortingRuleDetails.Any())
                {
                    continue;
                }

                var procedureIds = sortingRuleDetails.Select(x => x.ProcedureId).Distinct().ToList();
                foreach (var procedureId in procedureIds)
                {
                    //根据参数筛选过滤拿到最新的参数信息
                    var parameterIds = new List<long> { };
                    var procedureSortingRuleDetails = sortingRuleDetails.Where(X => X.ProcedureId == procedureId).ToList();
                    parameterIds.AddRange(procedureSortingRuleDetails.Select(a => a.ParameterId).Distinct().ToArray());

                    //条码的产品参数
                    //var sfcParameterList = parameterList.Where(x => x.SFC == sfc && parameterIds.Contains(x.ParameterId));
                    var procedureParameterList = parameterList.Where(x => x.SFC == sfc && x.ProcedureId == procedureId && parameterIds.Contains(x.ParameterId));
                    if (procedureParameterList == null || !procedureParameterList.Any())
                    {
                        var code= procProcedures.FirstOrDefault(x=>x.Id== procedureId)?.Code??"";
                        throw new CustomerValidationException(nameof(ErrorCode.MES16372)).WithData("SFC", sfc).WithData("code", code);
                    }

                    //根据参数筛选过滤拿到最新的参数信息
                    var parameterEntities = procedureParameterList.GroupBy(x => x.ParameterId).Select(x => x.OrderByDescending(x => x.CreatedOn).First()).ToList();
                    foreach (var parameterId in parameterIds)
                    {
                        var parameter = parameterEntities.FirstOrDefault(x => x.ParameterId == parameterId);
                        var ruleDetail = GetParameterRating(parameter, sortingRuleDetails);
                        if (ruleDetail == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES16365)).WithData("SFC", sfc);
                        }
                        if (ruleDetail != null && !ruleDetailIds.Contains(ruleDetail.Id))
                        {
                            ruleDetailIds.Add(ruleDetail.Id);
                        }

                        sfcGradeDetails.Add(new ManuSfcGradeDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = commonBo.SiteId,
                            GadeId = gradeId,
                            ProduceId = parameter.ProcedureId,
                            SFC = sfc,
                            Grade = ruleDetail?.Rating ?? "",
                            ParamId = parameter.ParameterId,
                            ParamValue = parameter.ParameterValue,
                            MaxValue = ruleDetail?.MaxValue ?? 0,
                            MinValue = ruleDetail?.MinValue ?? 0,
                            MinContainingType = ruleDetail?.MinContainingType,
                            MaxContainingType = ruleDetail?.MaxContainingType,
                            CreatedBy = commonBo.UserName,
                            UpdatedBy = commonBo.UserName,
                        });
                    }
                }

                string finalGrade = "";
                if (sfcGradeDetails.Any())
                {
                    gradeDetailEntities.AddRange(sfcGradeDetails);
                    //根据组合拿到sfc的最终档次信息,最终档次算不出来报错
                    finalGrade = await GetFinalGrade(sfcGradeDetails, sortingRuleDetails, ruleDetailIds);
                    if (string.IsNullOrWhiteSpace(finalGrade))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16366)).WithData("SFC", sfc);
                    }
                }

                insertGrades.Add(new ManuSfcGradeEntity
                {
                    Id = gradeId,
                    SFC = sfc,
                    Grade = finalGrade,
                    SiteId = commonBo.SiteId,
                    CreatedBy = commonBo.UserName,
                    UpdatedBy = commonBo.UserName
                });
            }

            return new ProductsSortingResponseBo
            {
                InsertGrades = insertGrades,
                GradeDetailEntities = gradeDetailEntities
            };
        }

        /// <summary>
        /// 根据参数和规则获取参数的等级信息
        /// </summary>
        /// <param name="parameter">产品参数</param>
        /// <param name="sortingRuleDetailEntities">分选规则</param>
        /// <returns></returns>
        private ProcSortingRuleDetailEntity GetParameterRating(Core.Domain.Parameter.ManuProductParameterEntity parameter, IEnumerable<ProcSortingRuleDetailEntity> sortingRuleDetailEntities)
        {
            var procSortingRuleDetail = new ProcSortingRuleDetailEntity
            {
                Id = 0,
                Rating = ""
            };

            if (parameter == null || string.IsNullOrWhiteSpace(parameter?.ParameterValue))
            {
                return procSortingRuleDetail;
            }

            var procSortingRules = sortingRuleDetailEntities.Where(x => x.ParameterId == parameter.ParameterId && x.ProcedureId == parameter.ProcedureId).ToList();
            if (procSortingRules == null || !procSortingRules.Any())
            {
                return procSortingRuleDetail;
            }

            var parameterValue = parameter.ParameterValue.ParseToDecimal();
            //先查找固定值的
            var ruleDetail = procSortingRules.FirstOrDefault(x => x.ParameterValue == parameter.ParameterValue.ParseToDecimal());
            if (ruleDetail != null)
            {
                return ruleDetail;
            }

            //再查找范围在这个范围内的,根据类型取<还是小于等于
            if (procSortingRules == null || !procSortingRules.Any())
            {
                return procSortingRuleDetail;
            }

            foreach (var rule in procSortingRules)
            {
                ruleDetail = new ProcSortingRuleDetailEntity();
                if (rule.MinContainingType == ContainingTypeEnum.Lt && rule.MinValue < parameterValue)
                {
                    if (!rule.MaxContainingType.HasValue)
                    {
                        ruleDetail = rule;
                    }
                    else if (rule.MaxContainingType == ContainingTypeEnum.Lt && rule.MaxValue > parameterValue)
                    {
                        ruleDetail = rule;
                    }
                    else if (rule.MaxContainingType == ContainingTypeEnum.LtOrE && rule.MaxValue >= parameterValue)
                    {
                        ruleDetail = rule;
                    }
                }

                if (rule.MinContainingType == ContainingTypeEnum.LtOrE && rule.MinValue <= parameterValue)
                {
                    if (!rule.MaxContainingType.HasValue)
                    {
                        ruleDetail = rule;
                    }
                    else if (rule.MaxContainingType == ContainingTypeEnum.Lt && rule.MaxValue > parameterValue)
                    {
                        ruleDetail = rule;
                    }
                    else if (rule.MaxContainingType == ContainingTypeEnum.LtOrE && rule.MaxValue >= parameterValue)
                    {
                        ruleDetail = rule;
                    }
                }

                if (!rule.MinContainingType.HasValue)
                {
                    if (rule.MaxContainingType == ContainingTypeEnum.Lt && rule.MaxValue > parameterValue)
                    {
                        ruleDetail = rule;
                    }

                    if (rule.MaxContainingType == ContainingTypeEnum.LtOrE && rule.MaxValue >= parameterValue)
                    {
                        ruleDetail = rule;
                    }
                }

                if (ruleDetail != null && ruleDetail.Id > 0)
                {
                    break;
                }
            }

            if (ruleDetail != null)
            {
                return ruleDetail;
            }
            return procSortingRuleDetail;
        }

        /// <summary>
        /// 根据组合拿到电芯最终档位信息
        /// </summary>
        /// <param name="sortingRuleId"></param>
        /// <param name="SfcParamters"></param>
        /// <returns></returns>
        private async Task<string> GetFinalGrade(List<ManuSfcGradeDetailEntity> sfcParamters, List<ProcSortingRuleDetailEntity> sortingRuleDetails, List<long> ruleDetailIds)
        {
            var sortingRuleId = sortingRuleDetails.FirstOrDefault()?.SortingRuleId ?? 0;
            if (sfcParamters == null || !sfcParamters.Any())
            {
                return string.Empty;
            }

            if (sfcParamters.Any(x => string.IsNullOrWhiteSpace(x.Grade)))
            {
                return string.Empty;
            }

            var ruleGradeEntities = await _sortingRuleGradeRepository.GetSortingRuleGradesByIdAsync(sortingRuleId);
            if (ruleGradeEntities == null || !ruleGradeEntities.Any())
            {
                return string.Empty;
            }

            var ruleGradeDetailsEntities = await _ruleGradeDetailsRepository.GetSortingRuleGradeeDetailsByIdAsync(sortingRuleId);
            if (ruleGradeDetailsEntities == null || !ruleGradeDetailsEntities.Any())
            {
                return string.Empty;
            }

            var ruleGradeDetails = ruleGradeDetailsEntities.Where(x => ruleDetailIds.Contains(x.SortingRuleDetailId));
            if (ruleGradeDetails == null || !ruleGradeDetails.Any())
            {
                return string.Empty;
            }

            List<SortingRuleGradeBo> list = new();
            foreach (var item in ruleGradeEntities)
            {
                var gradeDetails = ruleGradeDetailsEntities.Where(x => x.SortingRuleGradeId == item.Id);
                list.Add(new SortingRuleGradeBo
                {
                    Grade = item.Grade,
                    Remark = item.Remark,
                    Ratings = sortingRuleDetails.Where(o => gradeDetails.Select(x => x.SortingRuleDetailId).Contains(o.Id)).Select(k => k.Rating)
                });
            }
            var rating = string.Join(" ", sortingRuleDetails.Where(o => ruleDetailIds.Contains(o.Id)).Select(k => k.Rating).ToArray());
            return list.FirstOrDefault(x => string.Join(" ", x.Ratings) == rating)?.Grade ?? "";
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<JobResponseBo?> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not ProductsSortingResponseBo data)
            {
                return responseBo;
            }

            //入库
            if (data.InsertGrades.Any())
            {
                await _manuSfcGradeRepository.InsertsAsync(data.InsertGrades);
            }
            if (data.GradeDetailEntities.Any())
            {
                await _gradeDetailRepository.InsertsAsync(data.GradeDetailEntities);
            }

            // 面板需要的数据
            List<PanelModuleEnum> panelModules = new() { PanelModuleEnum.ProcSortingRule };
            responseBo.Content = new Dictionary<string, string> { { "PanelModules", panelModules.ToSerialize() } };

            return responseBo;
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