using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
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
        /// 构造函数
        /// </summary>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        public ManuGenerateBarcodeService(
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            ISequenceService sequenceService)
        {
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _sequenceService = sequenceService;
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

            //return await GenerateBarCodeSerialNumberAsync(new BarCodeSerialNumberBo
            //{
            //    IsTest = param.IsTest,
            //    IsSimulation = false,
            //    CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
            //    {
            //        Seq = s.Seq,
            //        ValueTakingType = s.ValueTakingType,
            //        SegmentedValue = s.SegmentedValue,
            //        CustomValue = s.CustomValue,
            //    }),

            //    CodeRuleKey = $"{param.CodeRuleId}",
            //    Count = param.Count,
            //    Base = codeRule.Base,
            //    Increment = codeRule.Increment,
            //    IgnoreChar = codeRule.IgnoreChar,
            //    OrderLength = codeRule.OrderLength,
            //    ResetType = codeRule.ResetType,
            //    StartNumber = codeRule.StartNumber,
            //    CodeMode = codeRule.CodeMode,
            //});

            return (await GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
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
            })).Select(x=>x.BarCode);
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleBo param)
        {
            //return await GenerateBarCodeSerialNumberAsync(new BarCodeSerialNumberBo
            //{
            //    IsTest = param.IsTest,
            //    CodeRulesMakeBos = param.CodeRulesMakeList.Select(s => new CodeRulesMakeBo
            //    {
            //        Seq = s.Seq,
            //        ValueTakingType = s.ValueTakingType,
            //        SegmentedValue = s.SegmentedValue,
            //        CustomValue=s.CustomValue??""
            //    }),

            //    CodeRuleKey = $"{param.ProductId}",
            //    Count = param.Count,
            //    Base = param.Base,
            //    IgnoreChar = param.IgnoreChar ?? "",
            //    Increment = param.Increment,
            //    OrderLength = param.OrderLength,
            //    ResetType = param.ResetType,
            //    StartNumber = param.StartNumber,
            //    CodeMode=param.CodeMode,
            //});

            return (await GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(new BarCodeSerialNumberBo
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
            })).Select(x=>x.BarCode);
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        [Obsolete("这个方法已经过期(单单返回生成的条码，没有标识)，请使用GenerateBarCodeSerialNumberReturnBarCodeInfosAsync方法")]
        public async Task<IEnumerable<string>> GenerateBarCodeSerialNumberAsync(BarCodeSerialNumberBo bo)
        {
            bo.CodeRulesMakeBos = bo.CodeRulesMakeBos.OrderBy(x => x.Seq);

            List<string> list = new();

            List<string> serialStrings = new();
            var copyBo= bo.ToSerialize().ToDeserialize<BarCodeSerialNumberBo>();//复制一份数据，因为下方GenerateBarCodeAboutMoreAsync 方法中有修改值的操作
            //var serialNumbers = await GenerateBarCodeSerialNumbersWithTryAsync(copyBo);
            var serialNumbers = await GenerateBarCodeAboutMoreAsync(copyBo);
            foreach (var item in serialNumbers)
            {
                var str = bo.Base switch
                {
                    10 => $"{item}",
                    16 or 32 => ConvertNumber(item, bo.IgnoreChar, bo.Base),
                    _ => throw new CustomerValidationException(nameof(ErrorCode.MES16202)),
                };

                if (bo.OrderLength > 0 && str.Length > bo.OrderLength)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16201));
                }

                str = str.PadLeft(bo.OrderLength, '0');
                serialStrings.Add(str);
            }

            if (serialStrings == null || serialStrings.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16203));
            }

            #region 组合数据生成条码
            var ruleMakeValues = new string[bo.CodeRulesMakeBos.Count()][];
            var codeRulesMakeBosArray = bo.CodeRulesMakeBos.ToArray();
            //根据编码规则获取到每行数据会生成的值
            //foreach (var item in bo.CodeRulesMakeBos)
            for (int i = 0; i < codeRulesMakeBosArray.Length; i++)
            {
                var item = codeRulesMakeBosArray[i];

                if (item.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                {
                    ruleMakeValues[i] = new string[] { item.SegmentedValue };
                    continue;
                }

                switch (item.SegmentedValue) 
                {
                    case "%ACTIVITY%":
                            ruleMakeValues[i] = serialStrings.ToArray();
                        break;
                    case "%YYMMDD%":
                        ruleMakeValues[i] = new string[] { HymsonClock.Now().ToString("yyMMdd") };
                        break;
                    case "%MULTIPLE_VARIABLE%":
                        //模式是多个时，生成多个条码
                        if (bo.CodeMode == CodeRuleCodeModeEnum.More
                && item.ValueTakingType==CodeValueTakingTypeEnum.VariableValue&& !string.IsNullOrEmpty(item.CustomValue))
                        {
                            ruleMakeValues[i] = item.CustomValue.Split(';') ;//查询出自定义值能转换成几个
                        }
                        break;
                    default:
                        throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue);
                        break;
                }
            }

            //将上方 根据编码规则组成的数组值组成各种情况   ，如获取到[['A1'],['B1','B2'],['C1']]  去生成 A1B1C1、A1B2C1两个条码
            List<string[]> combinations = new List<string[]>();
            GenerateCombinationsHelper(ruleMakeValues, 0, new string[ruleMakeValues.Length], combinations);
            foreach (var combination in combinations)
            {
                list.Add(string.Join("", combination));
            }
            #endregion

            #region //之前的代码
            //StringBuilder stringBuilder = new();
            //foreach (var item in serialStrings)
            //{
            //    stringBuilder.Clear();


            //    foreach (var rule in bo.CodeRulesMakeBos)
            //    {
            //        if (rule.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
            //        {
            //            stringBuilder.Append(rule.SegmentedValue);
            //            continue;
            //        }

            //        // TODO  暂时使用这种写法  王克明
            //        if (rule.SegmentedValue == "%ACTIVITY%")
            //        {
            //            stringBuilder.Append(item);
            //        }
            //        else if (rule.SegmentedValue == "%YYMMDD%")
            //        {
            //            stringBuilder.Append(HymsonClock.Now().ToString("yyMMdd"));
            //        }
            //        else if (rule.SegmentedValue == "%MULTIPLE_VARIABLE%")
            //        {
            //            //这里不做处理，下方有做处理，这里只是跳过下方的错误信息
            //        }
            //        else
            //        {
            //            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", rule.SegmentedValue);
            //        }
            //    }

            //     #region 生成多个
            //    if (bo.CodeMode == CodeRuleCodeModeEnum.More
            //        && bo.CodeRulesMakeBos.Any(x=> x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue == "%MULTIPLE_VARIABLE%" && !string.IsNullOrEmpty(x.CustomValue)))
            //    {
            //        //获取对应的编码规则第一条  只允许出现对应的一次，所以只获取一条
            //        var oneCodeRulesMake = bo.CodeRulesMakeBos.FirstOrDefault(x=> x.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && x.SegmentedValue == "%MULTIPLE_VARIABLE%" && !string.IsNullOrEmpty(x.CustomValue));
            //        var values = oneCodeRulesMake.CustomValue.Split(';');//查询出自定义值能转换成几个
            //        foreach (var v in values)
            //        {
            //            var copyStringBuilder=new StringBuilder(stringBuilder.ToString());
            //            copyStringBuilder.Append(v);
            //            list.Add(copyStringBuilder.ToString());
            //        }
            //    }
            //    #endregion
            //    else
            //    {
            //        list.Add(stringBuilder.ToString());
            //    }

            //}
            #endregion

            // 如果不是测试，且有模拟验证通过一次，就直接真实生成
            if (bo.IsTest == false && bo.IsSimulation == true)
            {
                bo.IsSimulation = false;
                return await GenerateBarCodeSerialNumberAsync(bo);
            }

            return list;
        }

        /// <summary>
        /// 生成 流水号
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BarCodeInfo>> GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(BarCodeSerialNumberBo bo)
        {
            bo.CodeRulesMakeBos = bo.CodeRulesMakeBos.OrderBy(x => x.Seq);

            List<BarCodeInfo> list = new();

            List<string> serialStrings = new();
            var copyBo = bo.ToSerialize().ToDeserialize<BarCodeSerialNumberBo>();//复制一份数据，因为下方GenerateBarCodeAboutMoreAsync 方法中有修改值的操作
            //var serialNumbers = await GenerateBarCodeSerialNumbersWithTryAsync(copyBo);
            var serialNumbers = await GenerateBarCodeAboutMoreAsync(copyBo);
            foreach (var item in serialNumbers)
            {
                var str = bo.Base switch
                {
                    10 => $"{item}",
                    16 or 32 => ConvertNumber(item, bo.IgnoreChar, bo.Base),
                    _ => throw new CustomerValidationException(nameof(ErrorCode.MES16202)),
                };

                if (bo.OrderLength > 0 && str.Length > bo.OrderLength)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16201));
                }

                str = str.PadLeft(bo.OrderLength, '0');
                serialStrings.Add(str);
            }

            if (serialStrings == null || serialStrings.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16203));
            }

            #region 组合数据生成条码
            var codeRulesMakeBosArray = bo.CodeRulesMakeBos.ToArray();
            foreach (var serialStr in serialStrings)
            {
                var ruleMakeValues = new string[bo.CodeRulesMakeBos.Count()][];
                //根据编码规则获取到每行数据会生成的值
                //foreach (var item in bo.CodeRulesMakeBos)
                for (int i = 0; i < codeRulesMakeBosArray.Length; i++)
                {
                    var item = codeRulesMakeBosArray[i];

                    if (item.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                    {
                        ruleMakeValues[i] = new string[] { item.SegmentedValue };
                        continue;
                    }

                    switch (item.SegmentedValue)
                    {
                        case "%ACTIVITY%":
                            ruleMakeValues[i] = new string[] { serialStr };
                            break;
                        case "%YYMMDD%":
                            ruleMakeValues[i] = new string[] { HymsonClock.Now().ToString("yyMMdd") };
                            break;
                        case "%MULTIPLE_VARIABLE%":
                            //模式是多个时，生成多个条码
                            if (bo.CodeMode == CodeRuleCodeModeEnum.More
                    && item.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && !string.IsNullOrEmpty(item.CustomValue))
                            {
                                ruleMakeValues[i] = item.CustomValue.Split(';');//查询出自定义值能转换成几个
                            }
                            break;
                        default:
                            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", item.SegmentedValue);
                            break;
                    }
                }

                //将上方 根据编码规则组成的数组值组成各种情况   ，如获取到[['A1'],['B1','B2'],['C1']]  去生成 A1B1C1、A1B2C1两个条码
                List<string[]> combinations = new List<string[]>();
                GenerateCombinationsHelper(ruleMakeValues, 0, new string[ruleMakeValues.Length], combinations);
                foreach (var combination in combinations)
                {
                    list.Add( new BarCodeInfo 
                    {
                        BarCode= string.Join("", combination),
                        SerialNumber=serialStr
                    });
                }
            }
            #endregion

            // 如果不是测试，且有模拟验证通过一次，就直接真实生成
            if (bo.IsTest == false && bo.IsSimulation == true)
            {
                bo.IsSimulation = false;
               return await GenerateBarCodeSerialNumberReturnBarCodeInfosAsync(bo);
            }

            return list;
        }

        /// <summary>
        ///  获取条码
        ///  添加 针对于编码规则 编码类型为 多个的逻辑
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GenerateBarCodeAboutMoreAsync(BarCodeSerialNumberBo param, int maxLength = 9)
        {
            //处理数量： 如果有这种 %MULTIPLE_VARIABLE% 特殊情况的编码规则，则需要处理，
            //如 该对应的自定义值为 L;R;M  ，前端传参又需要2个条码，则会生成3个条码，但是只走一次序列码
            #region 生成多个
            if (param.CodeMode == CodeRuleCodeModeEnum.More)
            {
                foreach (var rule in param.CodeRulesMakeBos)
                {
                    if (rule.ValueTakingType == CodeValueTakingTypeEnum.VariableValue && rule.SegmentedValue == "%MULTIPLE_VARIABLE%" && !string.IsNullOrEmpty(rule.CustomValue))
                    {
                        var values = rule.CustomValue.Split(';');//查询出自定义值能转换成几个
                        param.Count = (int)Math.Ceiling(param.Count /( values.Length * 1.0));//修改生成的数量

                        break;//只允许出现一次
                    }
                }
            }
            #endregion

            return await GenerateBarCodeSerialNumbersWithTryAsync(param, maxLength);
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
            if (type == 32)
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

        #region 内部方法
        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="param"></param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GenerateBarCodeSerialNumbersWithTryAsync(BarCodeSerialNumberBo param, int maxLength = 9)
        {
            List<int> sequences = new(param.Count);

            // 因为测试提出计数器需要包含起始数字，而计时器是用startNumber往后开始计数的
            var startNumber = param.StartNumber - param.Increment;

            // 真实生成
            if (param.IsTest == false && param.IsSimulation == false)
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
