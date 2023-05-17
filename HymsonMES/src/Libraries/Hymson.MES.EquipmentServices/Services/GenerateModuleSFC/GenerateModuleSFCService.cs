using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.GenerateModuleSFC;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationGenerateModuleSFCDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public GenerateModuleSFCService(AbstractValidator<GenerateModuleSFCDto> validationGenerateModuleSFCDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationGenerateModuleSFCDtoRules = validationGenerateModuleSFCDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 请求生成模组码-电芯堆叠
        /// </summary>
        /// <param name="generateModuleSFCDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task GenerateModuleSFCAsync(GenerateModuleSFCDto generateModuleSFCDto)
        {
            await _validationGenerateModuleSFCDtoRules.ValidateAndThrowAsync(generateModuleSFCDto);
            throw new NotImplementedException();
        }
    }
}
