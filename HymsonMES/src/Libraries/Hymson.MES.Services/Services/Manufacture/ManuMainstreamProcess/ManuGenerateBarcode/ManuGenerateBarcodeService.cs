using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.Sequences;
using Hymson.Utils;
using IdGen;
using System.Collections.Generic;
using System.Text;

namespace Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode
{
    /// <summary>
    /// 条码生成
    /// </summary>
    public class ManuGenerateBarcodeService
    {
        private readonly ISequenceService _sequenceService;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sequenceService"></param>
        /// <param name="inteCodeRulesMakeRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        public ManuGenerateBarcodeService(ISequenceService sequenceService, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository, IInteCodeRulesRepository inteCodeRulesRepository)
        {
            _sequenceService = sequenceService;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
        }

        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeSerialNumber(BarcodeSerialNumberDto param)
        {
            List<string> list = new List<string>();
            var serialList = await _sequenceService.GetSerialNumbersAsync(param.ResetType, param.Id.ToString(), param.Count, 0, 9999);
            foreach (var item in serialList)
            {
                var number = item * param.Increment + param.StartNumber;
                var str = string.Empty;
                switch (param.Base)
                {
                    case 10:
                        str = number.ToString();
                        break;
                    case 16:
                        str = ConvertNumber(number, param.IgnoreChar, 16);
                        break;
                    case 32:
                        str = ConvertNumber(number, param.IgnoreChar, 32);
                        break;
                    default:
                        throw new BusinessException(nameof(ErrorCode.MES16201));
                }
                if (str.Length > param.OrderLength)
                {
                    throw new BusinessException(nameof(ErrorCode.MES16201));
                }
                str = str.PadLeft(param.OrderLength, '0');
                list.Add(str);
            }
            return list;
        }

        /// <summary>
        /// 流水转换
        /// </summary>
        /// <param name="number"></param>
        /// <param name="ignoreChar"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string ConvertNumber(int number, string ignoreChar, int type)
        {
            List<string> list = new List<string>();
            if (type == 16)
            {
                list = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            }
            if (type == 32)
            {
                list = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" ,
                    "G", "H", "I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y"};
            }
            var ignoreCharArray = ignoreChar.Split(";");
            list.RemoveAll(match => ignoreCharArray.Contains(match));
            if (!list.Any()) { }
            int remainder = number % list.Count;
            int quotient = number / list.Count;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in quotient.ToString())
            {
                stringBuilder.Append(list[item]);
            }
            stringBuilder.Append(list[remainder]);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GenerateBarcodeList(long id, int count)
        {
            var getCodeRulesTask = _inteCodeRulesRepository.GetByIdAsync(id);
            var getCodeRulesMakeListTask = _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery { CodeRulesId = id });
            var codeRulesMakeList = await getCodeRulesMakeListTask;
            var codeRule = await getCodeRulesTask;
            var barcodeSerialNumberList = GenerateBarcodeSerialNumber(new BarcodeSerialNumberDto
            {
                Id = id,
                Count = count,
                Base = codeRule.Base,
                Increment = codeRule.Increment,
                OrderLength = codeRule.OrderLength,
                ResetType = codeRule.ResetType,
                StartNumber = codeRule.StartNumber
            }).ToString();

            if (barcodeSerialNumberList.Any())
            {
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
                            stringBuilder.Append(HymsonClock.Now().ToString("yyMMdd"));
                        }
                        else if (rule.SegmentedValue == "%YYMMDD%")
                        {
                            stringBuilder.Append(item);
                        }
                    }
                }
                list.Add(stringBuilder.ToString());
            }
            return list;
        }
    }
}
