using FluentValidation;
using Hymson.MES.EquipmentServices.Request.GenerateModuleSFC;
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
        private readonly AbstractValidator<GenerateModuleSFCRequest> _validationGenerateModuleSFCRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationGenerateModuleSFCRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public GenerateModuleSFCService(AbstractValidator<GenerateModuleSFCRequest> validationGenerateModuleSFCRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationGenerateModuleSFCRequestRules = validationGenerateModuleSFCRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 请求生成模组码-电芯堆叠
        /// </summary>
        /// <param name="generateModuleSFCRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task GenerateModuleSFCAsync(GenerateModuleSFCRequest generateModuleSFCRequest)
        {
            await _validationGenerateModuleSFCRequestRules.ValidateAndThrowAsync(generateModuleSFCRequest);
            throw new NotImplementedException();
        }
    }
}
