using AutoMapper.Internal;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Sequences;
using Hymson.Utils;
using System.Data;
using System.Text;

namespace Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode
{
    /// <summary>
    /// 生成条码
    /// </summary>
    public class ManuGenerateBarcodeService : IManuGenerateBarcodeService
    {
        private readonly ISequenceService _sequenceService;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;

        /// <summary>
        /// 工作中心，为生成条码的线体部分使用
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        /// <summary>
        /// 条码追溯
        /// </summary>
        private readonly ITracingSourceCoreService _tracingSourceCoreService;
        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteTimeWildcardRepository _inteTimeWildcardRepository;
        private readonly ILocalizationService _localizationService;
        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        public ManuGenerateBarcodeService(
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            ISequenceService sequenceService,
            IInteTimeWildcardRepository inteTimeWildcardRepository, ILocalizationService localizationService, IProcMaterialRepository procMaterialRepository, IInteWorkCenterRepository inteWorkCenterRepository,
            ITracingSourceCoreService tracingSourceCoreService,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _sequenceService = sequenceService;
            _inteTimeWildcardRepository = inteTimeWildcardRepository;
            _localizationService = localizationService;
            _procMaterialRepository = procMaterialRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _tracingSourceCoreService = tracingSourceCoreService;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeBo param)
        {
            var getCodeRulesTask = _inteCodeRulesRepository.GetByIdAsync(param.CodeRuleId);
            var getCodeRulesMakeListTask = _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = param.SiteId,
                CodeRulesId = param.CodeRuleId
            });
            var codeRulesMakeList = await getCodeRulesMakeListTask;
            var codeRule = await getCodeRulesTask;

            var barcodes = await GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = param.IsTest,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue,
                }),

                CodeRuleKey = $"{param.CodeRuleId}",
                Count = param.Count,
                Base = codeRule.Base,
                Increment = codeRule.Increment,
                IgnoreChar = codeRule.IgnoreChar,
                OrderLength = codeRule.OrderLength,
                ResetType = codeRule.ResetType,
                StartNumber = codeRule.StartNumber,
                CodeMode = codeRule.CodeMode,
                SiteId = param.SiteId,
            });
            var list = new List<string>();
            foreach (var barcode in barcodes)
            {
                list.AddRange(barcode.BarCodes);
            }
            return list;
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleBo param)
        {
            var barcodes = await GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
            {
                IsTest = param.IsTest,
                CodeRulesMakeBos = param.CodeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                    CustomValue = s.CustomValue ?? ""
                }),

                CodeRuleKey = $"{param.ProductId}",
                ProductId = param.ProductId,
                Count = param.Count,
                Base = param.Base,
                IgnoreChar = param.IgnoreChar ?? "",
                Increment = param.Increment,
                OrderLength = param.OrderLength,
                ResetType = param.ResetType,
                StartNumber = param.StartNumber,
                CodeMode = param.CodeMode,
                CodeType = param.CodeType,
                SiteId = param.SiteId,
            });
            var list = new List<string>();
            foreach (var barcode in barcodes)
            {
                list.AddRange(barcode.BarCodes);
            }
            return list;
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BarCodeInfo>> GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(BarCodeSerialNumberBo bo)
        {
            bo.CodeRulesMakeBos = bo.CodeRulesMakeBos.OrderBy(x => x.Seq);

            List<BarCodeInfo> list = new();

            var serialStrings = await GetSerialNumbersAsync(bo);

            if (serialStrings == null || !serialStrings.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16203));
            }
            var now = HymsonClock.Now();
            #region 组合数据生成条码
            foreach (var serialStr in serialStrings)
            {
                var rules = new List<List<string>>();
                foreach (var item in bo.CodeRulesMakeBos)
                {
                    if (item.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                    {
                        rules.Add(new List<string> { item.SegmentedValue! });
                        continue;
                    }

                    switch (item.SegmentedValue)
                    {
                        case GenerateBarcodeWildcard.Activity:
                            rules.Add(new List<string> { serialStr });
                            break;
                        case GenerateBarcodeWildcard.Yymmdd:
                            rules.Add(new List<string> { now.ToString("yyMMdd") });
                            break;
                        case GenerateBarcodeWildcard.MultipleVariable:
                            //模式是多个时，生成多个条码
                            if (bo.CodeMode == CodeRuleCodeModeEnum.More)
                            {
                                if (string.IsNullOrEmpty(item.CustomValue))
                                {
                                    throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue);
                                }
                                var customValueArray = item.CustomValue.Split(';');
                                rules.Add(customValueArray.ToList());
                            }
                            break;
                        case GenerateBarcodeWildcard.YMDWildcard:
                            var year = await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Year);
                            var month = await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Month);
                            var day = await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Day);
                            rules.Add(new List<string> { $"{year}{month}{day}" });
                            break;
                        case GenerateBarcodeWildcard.SingleYearMapping:

                            rules.Add(new List<string> { await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Year) });
                            break;

                        case GenerateBarcodeWildcard.SingleMonthMapping:
                            rules.Add(new List<string> { await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Month) });
                            break;
                        case GenerateBarcodeWildcard.SingleDayMapping:
                            rules.Add(new List<string> { await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Day) });
                            break;
                        case GenerateBarcodeWildcard.SingleYearDirect:

                            rules.Add(new List<string> { now.ToString("yy") });
                            break;

                        case GenerateBarcodeWildcard.SingleMonthDirect:
                            rules.Add(new List<string> { now.ToString("MM") });
                            break;
                        case GenerateBarcodeWildcard.SingleDayDirect:
                            rules.Add(new List<string> { now.ToString("dd") });
                            break;
                        case GenerateBarcodeWildcard.BatterySpecifications:
                            rules.Add(new List<string> { await GenerateBatterySpecificationsAsync(bo) });
                            break;
                        case GenerateBarcodeWildcard.LINETYPE:
                            rules.Add(new List<string> { await GenerateLineAsync(bo) });
                            break;
                        case GenerateBarcodeWildcard.AnodeMain:
                            rules.Add(new List<string> { await GenerateMaterialTypeAsync(bo, GenerateBarcodeWildcard.AnodeMain) });
                            break;
                        case GenerateBarcodeWildcard.CathodeMain:
                            rules.Add(new List<string> { await GenerateMaterialTypeAsync(bo, GenerateBarcodeWildcard.CathodeMain) });
                            break;
                        case GenerateBarcodeWildcard.Diaphragm:
                            rules.Add(new List<string> { await GenerateMaterialTypeAsync(bo, GenerateBarcodeWildcard.Diaphragm) });
                            break;
                        case GenerateBarcodeWildcard.PositivePlate:
                            rules.Add(new List<string> { await GenerateMaterialTypeAsync(bo, GenerateBarcodeWildcard.PositivePlate) });
                            break;
                        case GenerateBarcodeWildcard.ElectrodeState:
                            rules.Add(new List<string> { await GenerateElectrodeStateAsync(bo) });
                            break;
                        default:
                            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue!);
                    }
                }

                var combinations = GenerateCombination1s(rules);
                var barCodes = new List<string>();
                foreach (var combination in combinations)
                {
                    barCodes.Add(string.Join("", combination));
                }
                list.Add(new BarCodeInfo
                {
                    SerialNumber = serialStr,
                    BarCodes = barCodes
                });
            }

            #endregion

            return list;
        }

        ///// <summary>
        ///// 生成流水号
        ///// </summary>
        ///// <returns></returns>
        //public async Task<IEnumerable<BarCodeInfo>> GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(BarCodeSerialNumberBo bo, BarCodeExtendBo? barCodeExtendBo = null)
        //{
        //    bo.CodeRulesMakeBos = bo.CodeRulesMakeBos.OrderBy(x => x.Seq);

        //    List<BarCodeInfo> list = new();

        //    var serialStrings = await GetSerialNumbersAsync(bo);

        //    if (serialStrings == null || !serialStrings.Any())
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES16203));
        //    }
        //    var now = HymsonClock.Now();

        //    #region 组合数据生成条码
        //    foreach (var serialStr in serialStrings)
        //    {
        //        var rules = new List<List<string>>();
        //        foreach (var item in bo.CodeRulesMakeBos)
        //        {
        //            if (item.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
        //            {
        //                rules.Add(new List<string> { item.SegmentedValue! });
        //                continue;
        //            }

        //            switch (item.SegmentedValue)
        //            {
        //                case GenerateBarcodeWildcard.Activity:
        //                    rules.Add(new List<string> { serialStr });
        //                    break;
        //                case GenerateBarcodeWildcard.Yymmdd:
        //                    rules.Add(new List<string> { now.ToString("yyMMdd") });
        //                    break;
        //                case GenerateBarcodeWildcard.MultipleVariable:
        //                    //模式是多个时，生成多个条码
        //                    if (bo.CodeMode == CodeRuleCodeModeEnum.More)
        //                    {
        //                        if (string.IsNullOrEmpty(item.CustomValue))
        //                        {
        //                            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue);
        //                        }
        //                        var customValueArray = item.CustomValue.Split(';');
        //                        rules.Add(customValueArray.ToList());
        //                    }
        //                    break;
        //                case GenerateBarcodeWildcard.YMDWildcard:
        //                    var year = await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Year);
        //                    var month = await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Month);
        //                    var day = await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Day);
        //                    rules.Add(new List<string> { $"{year}{month}{day}" });
        //                    break;
        //                case GenerateBarcodeWildcard.SingleYearMapping:

        //                    rules.Add(new List<string> { await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Year) });
        //                    break;

        //                case GenerateBarcodeWildcard.SingleMonthMapping:
        //                    rules.Add(new List<string> { await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Month) });
        //                    break;
        //                case GenerateBarcodeWildcard.SingleDayMapping:
        //                    rules.Add(new List<string> { await GenerateSingleDateAsync(bo, TimeWildcardTypeEnum.Day) });
        //                    break;
        //                case GenerateBarcodeWildcard.SingleYearDirect:

        //                    rules.Add(new List<string> { now.ToString("yy") });
        //                    break;

        //                case GenerateBarcodeWildcard.SingleMonthDirect:
        //                    rules.Add(new List<string> { now.ToString("MM") });
        //                    break;
        //                case GenerateBarcodeWildcard.SingleDayDirect:
        //                    rules.Add(new List<string> { now.ToString("dd") });
        //                    break;
        //                case GenerateBarcodeWildcard.BatterySpecifications:
        //                    rules.Add(new List<string> { await GenerateBatterySpecificationsAsync(bo) });
        //                    break;

        //                case GenerateBarcodeWildcard.LINETYPE:
        //                    if (barCodeExtendBo == null)
        //                    {
        //                        throw new CustomerValidationException(nameof(ErrorCode.MES16211));
        //                    }
        //                    if (string.IsNullOrEmpty(item.CustomValue))
        //                    {
        //                        throw new CustomerValidationException(nameof(ErrorCode.MES16205));
        //                    }
        //                    else
        //                    {
        //                        var customValues = item.CustomValue.Split(";").ToList<string>();
        //                        var barCodeExtendValue = customValues.FirstOrDefault(x => x.Contains(barCodeExtendBo?.LineCode ?? ""));
        //                        if (string.IsNullOrEmpty(barCodeExtendValue))
        //                        {
        //                            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue);
        //                        }
        //                        else
        //                        {
        //                            var barCodeExtendValueArray = barCodeExtendValue.Split(":");
        //                            if (barCodeExtendValueArray.Length != 2)
        //                            {
        //                                throw new CustomerValidationException(nameof(ErrorCode.MES16210)).WithData("Value", barCodeExtendValue);
        //                            }
        //                            else
        //                            {
        //                                rules.Add(new List<string> { barCodeExtendValueArray[1] });
        //                            }
        //                        }
        //                    }
        //                    break;
        //                default:
        //                    throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue!);
        //            }
        //        }

        //        var combinations = GenerateCombination1s(rules);
        //        var barCodes = new List<string>();
        //        foreach (var combination in combinations)
        //        {
        //            barCodes.Add(string.Join("", combination));
        //        }
        //        list.Add(new BarCodeInfo
        //        {
        //            SerialNumber = serialStr,
        //            BarCodes = barCodes
        //        });
        //    }

        //    #endregion

        //    return list;
        //}

        #region 内部方法

        /// <summary>
        /// 生成条码物料类型相关数据
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="wildcard">通配符</param>
        /// <returns></returns>
        private async Task<string> GenerateMaterialTypeAsync(BarCodeSerialNumberBo bo, string wildcard)
        {

            if (bo.IsTest)
            {
                ///因为是动态生成，在前端测试时用ZZ占位
                return "ZZ";
            }
            else
            {

                string productModel = await PrepareProductModelAsync(bo, wildcard);

                return productModel;
            }
        }
        /// <summary>
        /// 获取产品型号
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="wildcard"></param>
        /// <returns></returns>
        private async Task<string> PrepareProductModelAsync(BarCodeSerialNumberBo bo, string wildcard)
        {
            if (bo.Sfcs == null || !bo.Sfcs.Any())
            {
                return string.Empty;
            }
            string productModel = "";
            bool isFound = false;
            foreach (var sfc in bo.Sfcs)
            {
                var manuSFCNodeSourceEntities = await _tracingSourceCoreService.OriginalSourceAsync(new Data.Repositories.Common.Query.EntityBySFCQuery
                {
                    SFC = sfc,
                    SiteId = bo.SiteId,
                });
                //关联物料信息，查询是什么类型的物料
                var productIds = manuSFCNodeSourceEntities.Select(x => x.ProductId);
                var procMaterialEntities = await _procMaterialRepository.GetByIdsAsync(productIds);
                foreach (var procMaterialEntity in procMaterialEntities)
                {
                    var  materialTypeEnum= SelectMaterialType(wildcard);
                    if (materialTypeEnum==null)
                    {
                        throw new Exception("找不到对应的物料类型");
                    }
                    if (procMaterialEntity.MaterialType == materialTypeEnum)
                    {
                        productModel = procMaterialEntity.ProductModel ?? string.Empty;
                        isFound = true;
                        break;
                    }
                }
                if (isFound) break;

            }

            return productModel;
        }
        /// <summary>
        /// 通配符映射物料类型
        /// </summary>
        /// <param name="wildcard"></param>
        /// <returns></returns>
        private static Core.Enums.Process.MaterialTypeEnum? SelectMaterialType(string wildcard)
        {
            switch (wildcard)
            {
                case GenerateBarcodeWildcard.AnodeMain: return Core.Enums.Process.MaterialTypeEnum.AnodeMain;
                case GenerateBarcodeWildcard.CathodeMain: return Core.Enums.Process.MaterialTypeEnum.CathodeMain;
                case GenerateBarcodeWildcard.Diaphragm: return Core.Enums.Process.MaterialTypeEnum.Diaphragm;
                case GenerateBarcodeWildcard.PositivePlate: return Core.Enums.Process.MaterialTypeEnum.PositivePlate;
            }
            return null;
        }

        /// <summary>
        /// 生成条码线体相关数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        private async Task<string> GenerateLineAsync(BarCodeSerialNumberBo bo)
        {

            if (bo.IsTest)
            {
                ///因为是动态生成，在前端测试时用XX占位
                return "XX";
            }
            else
            {
                if (!bo.InteWorkCenterId.HasValue)
                {
                    return string.Empty;
                }
                var inteWorkCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(bo.InteWorkCenterId.Value);
                if (inteWorkCenterEntity == null)
                    return string.Empty;
                return inteWorkCenterEntity.LineCoding ?? string.Empty;
            }
        }

        /// <summary>
        /// 极片状态 通过工单类型映射相关数据
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        private async Task<string> GenerateElectrodeStateAsync(BarCodeSerialNumberBo bo)
        {

            if (bo.IsTest)
            {
                ///因为是动态生成，在前端测试时用YY占位
                return "YY";
            }
            else
            {
                if (!bo.WorkOrderId.HasValue)
                {
                    return string.Empty;
                }
                var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(bo.WorkOrderId.Value);
                if (planWorkOrderEntity == null)
                    return string.Empty;
                return MappingElectrodeState(planWorkOrderEntity.Type);
            }
        }
        /// <summary>
        /// 通过工单类型映射出极片状态
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string MappingElectrodeState(PlanWorkOrderTypeEnum type)
        {
            return type switch
            {
                PlanWorkOrderTypeEnum.Production => "C",
                PlanWorkOrderTypeEnum.TrialProduction => "G",
                PlanWorkOrderTypeEnum.Rework => "R",
                PlanWorkOrderTypeEnum.Experiment => "D",
                _ => string.Empty,
            };
        }
        /// <summary>
        /// 获取产品型号
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        private async Task<string> GenerateBatterySpecificationsAsync(BarCodeSerialNumberBo bo)
        {
            //if (bo.CodeType != CodeRuleCodeTypeEnum.ProcessControlSeqCode)
            //    return string.Empty;
            var procMaterialEntity = await _procMaterialRepository.GetByIdAsync(bo.ProductId);
            if (procMaterialEntity == null)
                return string.Empty;
            return procMaterialEntity.ProductModel ?? string.Empty;
        }
        /// <summary>
        /// 生成单个的年 ，月，日 映射字符 为组合提供自由度
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="timeWildcardTypeEnum"></param>
        /// <returns></returns>
        private async Task<string> GenerateSingleDateAsync(BarCodeSerialNumberBo bo, TimeWildcardTypeEnum timeWildcardTypeEnum)
        {
            var result = string.Empty;
            #region 查询年月日的通配
            var currentTime = HymsonClock.Now();
            var timeWildcards = await _inteTimeWildcardRepository.GetAllAsync(bo.SiteId);
            if (timeWildcardTypeEnum == TimeWildcardTypeEnum.Year)
            {
                var yearWildcard = timeWildcards.FirstOrDefault(x => x.Code == currentTime.Year + "" && x.Type == timeWildcardTypeEnum);
                if (yearWildcard == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16208)).WithData("code", currentTime.Year + "").WithData("type", _localizationService.GetResource($"{typeof(TimeWildcardTypeEnum).FullName}.{nameof(TimeWildcardTypeEnum.Year)}"));
                }
                else
                    result += yearWildcard.Value;
                return result;
            }
            if (timeWildcardTypeEnum == TimeWildcardTypeEnum.Month)
            {
                var monthWildcard = timeWildcards.FirstOrDefault(x => x.Code == currentTime.Month + "" && x.Type == timeWildcardTypeEnum);
                if (monthWildcard == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16208)).WithData("code", currentTime.Month + "").WithData("type", _localizationService.GetResource($"{typeof(TimeWildcardTypeEnum).FullName}.{nameof(TimeWildcardTypeEnum.Month)}"));
                }
                else
                    result += monthWildcard.Value;
                return result;
            }
            if (timeWildcardTypeEnum == TimeWildcardTypeEnum.Day)
            {
                var dayWildcard = timeWildcards.FirstOrDefault(x => x.Code == currentTime.Day + "" && x.Type == timeWildcardTypeEnum);
                if (dayWildcard == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16208)).WithData("code", currentTime.Day + "").WithData("type", _localizationService.GetResource($"{typeof(TimeWildcardTypeEnum).FullName}.{nameof(TimeWildcardTypeEnum.Day)}"));
                }
                else
                    result += dayWildcard.Value;
                return result;
            }


            #endregion
            return result;
        }

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<string>> GetSerialNumbersAsync(BarCodeSerialNumberBo param, int maxLength = 9)
        {
            List<string> serialStrings = new();
            var count = param.Count;
            if (param.CodeMode == CodeRuleCodeModeEnum.More)
            {
                var codeRulesMakeBo = param.CodeRulesMakeBos.FirstOrDefault(x =>
                x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue
                && x.SegmentedValue == GenerateBarcodeWildcard.MultipleVariable
                && !string.IsNullOrEmpty(x.CustomValue));

                if (codeRulesMakeBo != null)
                {
                    //var values = codeRulesMakeBo.CustomValue!.Split(';');//查询出自定义值能转换成几个
                    //count = (int)Math.Ceiling(param.Count / (values.Length * 1.0));//修改生成的数量
                }
            }

            var serialNumbers = await GenerateBarCodeSerialNumbersWithTryAsync(new SerialNumberBo
            {
                CodeRuleKey = param.CodeRuleKey,
                IsTest = param.IsTest,
                IsSimulation = param.IsSimulation,
                Increment = param.Increment,
                Count = count,
                ResetType = param.ResetType,
                StartNumber = param.StartNumber,
            }, maxLength);

            foreach (var item in serialNumbers)
            {
                var str = param.Base switch
                {
                    10 => $"{item}",
                    16 or 32 => ConvertNumber(item, param.IgnoreChar, param.Base),
                    _ => throw new CustomerValidationException(nameof(ErrorCode.MES16202)),
                };

                if (param.OrderLength > 0 && str.Length > param.OrderLength)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16201));
                }

                str = str.PadLeft(param.OrderLength, '0');
                serialStrings.Add(str);
            }

            return serialStrings;
        }

        /// <summary>
        /// 回溯算法
        /// 递归出数组所有的路线
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="current"></param>
        /// <param name="combinations"></param>
        private void GenerateCombinationsHelper(string[][] array, int index, string[] current, List<string[]> combinations)
        {
            if (index == array.Length)
            {
                combinations.Add((string[])current.Clone());
                return;
            }

            foreach (var item in array[index])
            {
                current[index] = item;
                GenerateCombinationsHelper(array, index + 1, current, combinations);
            }
        }

        /// <summary>
        /// 回溯算法
        /// </summary>
        private void GenerateCombinations(IEnumerable<IEnumerable<string>> dataList, List<List<string>> combinations, int index = 0)
        {
            if (dataList == null || dataList.Any())
            {
                return;
            }

            var currentItem = dataList.ElementAt(index);

            var list = new List<List<string>>();

            foreach (var str in currentItem)
            {
                foreach (var item in combinations)
                {
                    item.Add(str);
                    list.Add(new List<string>(item)); // 注意要创建一个新的组合列表
                    item.RemoveAt(item.Count - 1); // 恢复组合列表
                }
            }
            GenerateCombinations(dataList, list, index++);
        }

        /// <summary>
        /// 回溯算法
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<string>> GenerateCombination1s(IEnumerable<IEnumerable<string>> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                yield return Enumerable.Empty<string>();
                yield break;
            }

            var currentItem = dataList.First();

            foreach (var str in currentItem)
            {
                var remainingCombinations = GenerateCombination1s(dataList.Skip(1));
                foreach (var combination in remainingCombinations)
                {
                    yield return new List<string> { str }.Concat(combination);
                }
            }
        }

        /// <summary>
        /// 流水转换
        /// </summary>
        /// <param name="number">转换流水号</param>
        /// <param name="ignoreChar">忽略字符</param>
        /// <param name="type">进制类型 16|32</param>
        /// <returns></returns>
        private string ConvertNumber(int number, string ignoreChar, int type)
        {
            List<string> list = new();
            if (type != 16 && type != 32)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16206));
            }
            if (type == 16)
            {
                list = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            }
            else if (type == 32)
            {
                list = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F",
                    "G", "H","J","K","L","M","N","P","Q","R","T","U","V","W","X","Y"};
            }
            var ignoreCharArray = string.IsNullOrWhiteSpace(ignoreChar) ? Array.Empty<string>() : ignoreChar.Split(";");
            list.RemoveAll(match => ignoreCharArray.Contains(match));
            if (list == null || !list.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16204)).WithData("base", type);
            }
            return Convert(number, list);
        }

        /// <summary>
        /// 进制转换
        /// </summary>
        /// <param name="number">转换数字</param>
        /// <param name="list">转换进制字符</param>
        /// <returns></returns>
        private string Convert(int number, List<string> list)
        {
            StringBuilder stringBuilder = new();
            int remainder = number % list.Count;
            stringBuilder.Insert(0, list[remainder]);
            int quotient = number / list.Count;
            if (quotient >= list.Count)
            {
                stringBuilder.Insert(0, Convert(quotient, list));
            }
            else
            {
                if (quotient != 0)
                {
                    stringBuilder.Insert(0, list[quotient]);
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GenerateBarCodeSerialNumbersWithTryAsync(SerialNumberBo param, int maxLength = 9)
        {
            List<int> sequences = new(param.Count);

            // 因为测试提出计数器需要包含起始数字，而计时器是用startNumber往后开始计数的
            var startNumber = param.StartNumber - param.Increment;

            // 真实生成
            if (!param.IsTest && !param.IsSimulation)
            {
                var serialNumbers = await _sequenceService.GetSerialNumbersAsync(param.ResetType, param.CodeRuleKey, param.Count, startNumber, param.Increment, maxLength)
                    ?? throw new CustomerValidationException(nameof(ErrorCode.MES16200));

                sequences.AddRange(serialNumbers);
                return sequences;
            }

            // 测试生成/模拟生成
            var count = param.Count;
            var currentValue = await _sequenceService.GetCurrentValueAsync(param.ResetType, param.CodeRuleKey, startNumber);
            do
            {
                currentValue += param.Increment;
                sequences.Add(currentValue);
                count--;
            } while (count > 0);

            if (currentValue >= GetMaxValue(maxLength))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16207)).WithData("BarCode", currentValue);
            }

            return sequences;
        }

        /// <summary>
        /// 获取十进制位数最大值
        /// </summary>
        /// <param name="numberDigits">十进制位数</param>
        /// <returns></returns>
        private static int GetMaxValue(int numberDigits)
        {
            return numberDigits switch
            {
                1 => 10,
                2 => 100,
                3 => 1000,
                4 => 10000,
                5 => 100000,
                6 => 1000000,
                7 => 10000000,
                8 => 100000000,
                9 => 1000000000,
                _ => 1000000000,
            };
        }
        #endregion
    }
}
