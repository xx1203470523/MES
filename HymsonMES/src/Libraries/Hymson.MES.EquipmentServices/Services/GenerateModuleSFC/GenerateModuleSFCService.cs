using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.GenerateModuleSFC
{
    /// <summary>
    /// 请求生成模组码-电芯堆叠服务
    /// </summary>
    public class GenerateModuleSFCService : IGenerateModuleSFCService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<GenerateModuleSFCDto> _validationGenerateModuleSFCDtoRules;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationGenerateModuleSFCDtoRules"></param>
        /// <param name="currentEquipment"></param> 
        public GenerateModuleSFCService(IInteCodeRulesRepository inteCodeRulesRepository, IProcMaterialRepository procMaterialRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService, AbstractValidator<GenerateModuleSFCDto> validationGenerateModuleSFCDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationGenerateModuleSFCDtoRules = validationGenerateModuleSFCDtoRules;
            _currentEquipment = currentEquipment;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
        }

        /// <summary>
        /// 请求生成模组码-电芯堆叠
        /// </summary>
        /// <param name="generateModuleSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GenerateModuleSFCModelDto> GenerateModuleSFCAsync(GenerateModuleSFCDto generateModuleSFCDto)
        {
            await _validationGenerateModuleSFCDtoRules.ValidateAndThrowAsync(generateModuleSFCDto);
            var materialEntit = await _procMaterialRepository.GetByCodeAsync(new ProcMaterialQuery { MaterialCode = generateModuleSFCDto.ProductCode, Version = generateModuleSFCDto.Version, SiteId = _currentEquipment.SiteId });
            if (materialEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19118)).WithData("Code", generateModuleSFCDto.ProductCode);
            }
            var inteCodeRulesResult = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery
            {
                ProductId = materialEntit.Id,
                CodeType = CodeRuleCodeTypeEnum.ProcessControlSeqCode,
                SiteId = _currentEquipment.SiteId
            });
            var inteCodeRulesEntit = inteCodeRulesResult.FirstOrDefault();
            if (inteCodeRulesResult == null || inteCodeRulesEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19116)).WithData("Code", generateModuleSFCDto.ProductCode);
            }
            ////生成条码
            var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                CodeRuleId = inteCodeRulesEntit.Id,
                Count = generateModuleSFCDto.Qty,
                SiteId = _currentEquipment.SiteId
            });
            return new GenerateModuleSFCModelDto { SFCs = barcodeList };
        }

    }
}
