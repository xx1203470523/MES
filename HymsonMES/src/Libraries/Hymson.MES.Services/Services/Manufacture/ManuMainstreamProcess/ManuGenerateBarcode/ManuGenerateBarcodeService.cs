using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Bos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.Sequences;
using Hymson.Utils;
using System.Data;
using System.Text;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode
{
    /// <summary>
    /// 条码生成
    /// </summary>
    public class ManuGenerateBarcodeService : IManuGenerateBarcodeService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ISequenceService _sequenceService;
        private readonly ICurrentSite _currentSite;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="currentSite"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        public ManuGenerateBarcodeService(ISequenceService sequenceService, ICurrentSite currentSite,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IInteCodeRulesRepository inteCodeRulesRepository)
        {
            _sequenceService = sequenceService;
            _currentSite = currentSite;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
        }


        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeDto param)
        {
            var getCodeRulesTask = _inteCodeRulesRepository.GetByIdAsync(param.CodeRuleId);
            var getCodeRulesMakeListTask = _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CodeRulesId = param.CodeRuleId
            });
            var codeRulesMakeList = await getCodeRulesMakeListTask;
            var codeRule = await getCodeRulesTask;

            return await GenerateBarCodeSerialNumberAsync(new BarCodeSerialNumberBo
            {
                IsTest = param.IsTest,
                IsSimulation = false,
                CodeRulesMakeBos = codeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                }),

                CodeRuleKey = $"{param.CodeRuleId}",
                Count = param.Count,
                Base = codeRule.Base,
                Increment = codeRule.Increment,
                IgnoreChar = codeRule.IgnoreChar,
                OrderLength = codeRule.OrderLength,
                ResetType = codeRule.ResetType,
                StartNumber = codeRule.StartNumber
            });
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleDto param)
        {
            return await GenerateBarCodeSerialNumberAsync(new BarCodeSerialNumberBo
            {
                IsTest = param.IsTest,
                CodeRulesMakeBos = param.CodeRulesMakeList.Select(s => new CodeRulesMakeBo
                {
                    Seq = s.Seq,
                    ValueTakingType = s.ValueTakingType,
                    SegmentedValue = s.SegmentedValue,
                }),

                CodeRuleKey = $"{param.ProductId}",
                Count = param.Count,
                Base = param.Base,
                IgnoreChar = param.IgnoreChar ?? "",
                Increment = param.Increment,
                OrderLength = param.OrderLength,
                ResetType = param.ResetType,
                StartNumber = param.StartNumber
            });
        }


        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarCodeSerialNumberAsync(BarCodeSerialNumberBo bo)
        {
            List<string> list = new();

            List<string> serialStrings = new();
            var serialNumbers = await GenerateBarCodeSerialNumbersWithTryAsync(bo);
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

            StringBuilder stringBuilder = new();
            foreach (var item in serialStrings)
            {
                stringBuilder.Clear();
                bo.CodeRulesMakeBos = bo.CodeRulesMakeBos.OrderBy(x => x.Seq);

                foreach (var rule in bo.CodeRulesMakeBos)
                {
                    if (rule.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                    {
                        stringBuilder.Append(rule.SegmentedValue);
                        continue;
                    }

                    // TODO  暂时使用这种写法  王克明
                    if (rule.SegmentedValue == "%ACTIVITY%")
                    {
                        stringBuilder.Append(item);
                    }
                    else if (rule.SegmentedValue == "%YYMMDD%")
                    {
                        stringBuilder.Append(HymsonClock.Now().ToString("yyMMdd"));
                    }
                    else
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", rule.SegmentedValue);
                    }
                }
                list.Add(stringBuilder.ToString());
            }

            // 如果不是测试，且有模拟验证通过一次，就直接真实生成
            if (bo.IsTest == false && bo.IsSimulation == true)
            {
                bo.IsSimulation = false;
                await GenerateBarCodeSerialNumberAsync(bo);
            }

            return list;
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
            var startNumber = param.StartNumber - 1;

            // 真实生成
            if (param.IsTest = false && param.IsSimulation == false)
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
