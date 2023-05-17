using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
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
        private readonly ISequenceService _sequenceService;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        public ManuGenerateBarcodeService(ISequenceService sequenceService, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository, IInteCodeRulesRepository inteCodeRulesRepository, ICurrentSite currentSite)
        {
            _sequenceService = sequenceService;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeSerialNumberAsync(BarcodeSerialNumberDto param)
        {
            List<int> serialList = new List<int>();
            if (param.Count == 1)
            {
                var seq = await _sequenceService.GetSerialNumberAsync(param.ResetType, param.CodeRuleKey, param.StartNumber, param.Increment, 9);
                serialList.Add(seq);
            }
            else
            {
                serialList = (await _sequenceService.GetSerialNumbersAsync(param.ResetType, param.CodeRuleKey, param.Count, param.StartNumber, param.Increment, 9)).ToList();
            }

            List<string> list = new List<string>();
            foreach (var item in serialList)
            {
                //var number = item * param.Increment + param.StartNumber;
                var str = string.Empty;
                switch (param.Base)
                {
                    case 10:
                        str = item.ToString();
                        break;
                    case 16:
                    case 32:
                        str = ConvertNumber(item, param.IgnoreChar, param.Base);
                        break;
                    default:
                        throw new CustomerValidationException(nameof(ErrorCode.MES16202));
                }
                if (param.OrderLength > 0 && str.Length > param.OrderLength)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16201));
                }
                str = str.PadLeft(param.OrderLength, '0');
                list.Add(str);
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
            List<string> list = new List<string>();
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
            StringBuilder stringBuilder = new StringBuilder();
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
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException">未找到生成规则</exception>
        public async Task<IEnumerable<string>> GenerateBarcodeListByIdAsync(GenerateBarcodeDto param)
        {
            var getCodeRulesTask = _inteCodeRulesRepository.GetByIdAsync(param.CodeRuleId);
            var getCodeRulesMakeListTask = _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery { SiteId = _currentSite.SiteId ?? 0, CodeRulesId = param.CodeRuleId });
            var codeRulesMakeList = await getCodeRulesMakeListTask;
            var codeRule = await getCodeRulesTask;
            var barcodeSerialNumberList = await GenerateBarcodeSerialNumberAsync(new BarcodeSerialNumberDto
            {
                CodeRuleKey = param.IsTest ? param.CodeRuleId.ToString() + "Test" : param.CodeRuleId.ToString(),
                Count = param.Count,
                Base = codeRule.Base,
                Increment = codeRule.Increment,
                IgnoreChar = codeRule.IgnoreChar,
                OrderLength = codeRule.OrderLength,
                ResetType = codeRule.ResetType,
                StartNumber = codeRule.StartNumber
            });

            if (barcodeSerialNumberList == null || !barcodeSerialNumberList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16203));
            }
            List<string> list = new List<string>();
            foreach (var item in barcodeSerialNumberList)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var rule in codeRulesMakeList.OrderBy(x => x.Seq))
                {
                    if (rule.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                    {
                        stringBuilder.Append(rule.SegmentedValue);
                    }
                    else
                    {
                        // TODO  暂时使用这种写法  王克明
                        if (rule.SegmentedValue == "%ACTIVITY%")
                        {
                            stringBuilder.Append(item);
                        }
                        else if (rule.SegmentedValue == "%YYMMDD%")
                        {
                            stringBuilder.Append(HymsonClock.Now().ToString("yyMMdd"));
                        }
                        else if (rule.SegmentedValue == "%YY%")
                        {
                            stringBuilder.Append(GetYY(HymsonClock.Now().Year));
                        }
                        else if (rule.SegmentedValue == "%MM%")
                        {
                            stringBuilder.Append(GetMonth(HymsonClock.Now().Month));
                        }
                        else if (rule.SegmentedValue == "%DD%")
                        {
                            stringBuilder.Append(GetDay(HymsonClock.Now().Day));
                        }
                        else
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", rule.SegmentedValue);
                        }
                    }
                }
                list.Add(stringBuilder.ToString());
            }
            return list;
        }

        /// <summary>
        /// 条码生成
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException">未找到生成规则</exception>
        public async Task<IEnumerable<string>> GenerateBarcodeListAsync(CodeRuleDto param)
        {

            var barcodeSerialNumberList = await GenerateBarcodeSerialNumberAsync(new BarcodeSerialNumberDto
            {
                CodeRuleKey = param.IsTest ? param.ProductId.ToString() + "Test" : param.ProductId.ToString(),
                Count = param.Count,
                Base = param.Base,
                IgnoreChar = param.IgnoreChar ?? "",
                Increment = param.Increment,
                OrderLength = param.OrderLength,
                ResetType = param.ResetType,
                StartNumber = param.StartNumber
            });

            if (barcodeSerialNumberList == null || !barcodeSerialNumberList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16203));
            }
            List<string> list = new List<string>();
            foreach (var item in barcodeSerialNumberList)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var rule in param.CodeRulesMakeList.OrderBy(x => x.Seq))
                {
                    if (rule.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                    {
                        stringBuilder.Append(rule.SegmentedValue);
                    }
                    else
                    {
                        // TODO  暂时使用这种写法  王克明
                        if (rule.SegmentedValue == "%ACTIVITY%")
                        {
                            stringBuilder.Append(item);
                        }
                        else if (rule.SegmentedValue == "%YYMMDD%")
                        {
                            stringBuilder.Append(HymsonClock.Now().ToString("yyMMdd"));
                        }
                        else if (rule.SegmentedValue == "%YY%")
                        {
                            stringBuilder.Append(GetYY(HymsonClock.Now().Year));
                        }
                        else if (rule.SegmentedValue == "%MM%")
                        {
                            stringBuilder.Append(GetMonth(HymsonClock.Now().Month));
                        }
                        else if (rule.SegmentedValue == "%DD%")
                        {
                            stringBuilder.Append(GetDay(HymsonClock.Now().Day));
                        }
                        else
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES16205)).WithData("value", rule.SegmentedValue);
                        }
                    }
                }
                list.Add(stringBuilder.ToString());
            }
            return list;
        }

        /// <summary>
        /// 获取转换年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private string GetYY(int year)
        {
            var dic = new Dictionary<string, int>
            {
                { "A", 2021 },
                { "B", 2022 },
                { "C", 2023 },
                { "D", 2024 },
                { "E", 2025 },
                { "F", 2026 },
                { "G", 2027 },
                { "H", 2028 },
                { "I", 2029 }
            };
            return dic.Where(it => it.Value == year).FirstOrDefault().Key;
        }
        /// <summary>
        /// 获取转换月
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        private string GetMonth(int month)
        {
            if (month < 10)
            {
                return month.ToString();
            }
            var dic = new Dictionary<string, int>
            {
                { "A", 10 },
                { "B", 11 },
                { "C", 12 },
            };
            return dic.Where(it => it.Value == month).FirstOrDefault().Key;

        }

        /// <summary>
        /// 获取转换日
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private string GetDay(int day)
        {
            if (day < 10)
            {
                return day.ToString();
            }
            var dic = new Dictionary<string, int>
            {
                { "A", 10 },
                { "B", 11 },
                { "C", 12 },
                { "D", 13 },
                { "E", 14 },
                { "F", 15 },
                { "G", 16 },
                { "H", 17 },
                { "J", 18 },
                { "K", 19 },
                { "L", 20 },
                { "M", 21 },
                { "N", 22 },
                { "O", 23 },
                { "P", 24 },
                { "Q", 25 },
                { "R", 26 },
                { "S", 27 },
                { "T", 28 },
                { "U", 29 },
                { "V", 30 },
                { "W", 31 },
            };
            return dic.Where(it => it.Value == day).FirstOrDefault().Key;
        }
    }
}
