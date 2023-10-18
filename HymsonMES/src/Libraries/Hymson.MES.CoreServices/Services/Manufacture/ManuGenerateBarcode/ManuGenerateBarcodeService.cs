﻿using AutoMapper.Internal;
using Hymson.Infrastructure.Exceptions;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.Sequences;
using Hymson.Utils;
using NETCore.Encrypt;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private readonly IInteTimeWildcardRepository _inteTimeWildcardRepository;
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        public ManuGenerateBarcodeService(
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            ISequenceService sequenceService,
            IInteTimeWildcardRepository inteTimeWildcardRepository,ILocalizationService localizationService)
        {
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _sequenceService = sequenceService;
            _inteTimeWildcardRepository = inteTimeWildcardRepository;
            _localizationService = localizationService;
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
                Count = param.Count,
                Base = param.Base,
                IgnoreChar = param.IgnoreChar ?? "",
                Increment = param.Increment,
                OrderLength = param.OrderLength,
                ResetType = param.ResetType,
                StartNumber = param.StartNumber,
                CodeMode = param.CodeMode,
                SiteId=param.SiteId,
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

            var ymdWildcard = "";
            #region 查询年月日的通配
            if (bo.CodeRulesMakeBos.Any(x => x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue == GenerateBarcodeWildcard.YMDWildcard))
            {

                var currentTime = HymsonClock.Now();
                var timeWildcards = await _inteTimeWildcardRepository.GetAllAsync(bo.SiteId);

                var yearWildcard = timeWildcards.FirstOrDefault(x => x.Code == currentTime.Year + "" && x.Type == TimeWildcardTypeEnum.Year);
                if (yearWildcard == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16208)).WithData("code", currentTime.Year + "").WithData("type", _localizationService.GetResource($"{typeof(TimeWildcardTypeEnum).FullName}.{nameof(TimeWildcardTypeEnum.Year)}"));
                }
                else
                    ymdWildcard += yearWildcard.Value;

                var monthWildcard = timeWildcards.FirstOrDefault(x => x.Code == currentTime.Month + "" && x.Type == TimeWildcardTypeEnum.Month);
                if (monthWildcard == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16208)).WithData("code", currentTime.Month + "").WithData("type", _localizationService.GetResource($"{typeof(TimeWildcardTypeEnum).FullName}.{nameof(TimeWildcardTypeEnum.Month)}"));
                }
                else
                    ymdWildcard += monthWildcard.Value;

                var dayWildcard = timeWildcards.FirstOrDefault(x => x.Code == currentTime.Day + "" && x.Type == TimeWildcardTypeEnum.Day);
                if (dayWildcard == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16208)).WithData("code", currentTime.Day + "").WithData("type", _localizationService.GetResource($"{typeof(TimeWildcardTypeEnum).FullName}.{nameof(TimeWildcardTypeEnum.Day)}"));
                }
                else
                    ymdWildcard += dayWildcard.Value;
            }
            #endregion


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
                            rules.Add(new List<string> { HymsonClock.Now().ToString("yyMMdd") });
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
                            rules.Add(new List<string> { ymdWildcard });
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

        #region 内部方法
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
                    var values = codeRulesMakeBo.CustomValue!.Split(';');//查询出自定义值能转换成几个
                    count = (int)Math.Ceiling(param.Count / (values.Length * 1.0));//修改生成的数量
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
            var ignoreCharArray = string.IsNullOrWhiteSpace(ignoreChar) ? new string[0] : ignoreChar.Split(";");
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
