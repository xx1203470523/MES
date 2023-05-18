using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.MES.EquipmentServices.Dtos.SingleBarCodeLoadingVerification;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationGenerateModuleSFCDtoRules"></param>
        /// <param name="currentEquipment"></param> 
        public GenerateModuleSFCService(IInteCodeRulesRepository inteCodeRulesRepository, IProcMaterialRepository procMaterialRepository, AbstractValidator<GenerateModuleSFCDto> validationGenerateModuleSFCDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationGenerateModuleSFCDtoRules = validationGenerateModuleSFCDtoRules;
            _currentEquipment = currentEquipment;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _procMaterialRepository = procMaterialRepository;
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
                throw new CustomerValidationException(nameof(ErrorCode.MES19114)).WithData("Code", generateModuleSFCDto.ProductCode);
            }
            var inteCodeRulesResult = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery
            {
                ProductId = materialEntit.Id,
                CodeType = CodeRuleCodeTypeEnum.PackagingSeqCode,
            });
            var inteCodeRulesEntit = inteCodeRulesResult.FirstOrDefault();
            if (inteCodeRulesResult == null || inteCodeRulesEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19116)).WithData("Code", generateModuleSFCDto.ProductCode);
            }
            //生成条码
            //var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
            //{
            //    CodeRuleId = inteCodeRulesEntit.Id,
            //    Count = generateModuleSFCDto.Qty
            //});
            //return new GenerateModuleSFCModelDto { SFCs = barcodeList };
            throw new NotImplementedException();
        }
    }
}
