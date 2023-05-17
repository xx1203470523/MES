using FluentValidation;
using Hymson.MES.EquipmentServices.Dtos.SingleBarCodeLoadingVerification;
using Hymson.Web.Framework.WorkContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.SingleBarCodeLoadingVerification
{
    /// <summary>
    /// 单体条码上料校验服务
    /// </summary>
    public class SingleBarCodeLoadingVerificationService : ISingleBarCodeLoadingVerificationService
    {
        private readonly ICurrentEquipment _currentEquipment;
        private readonly AbstractValidator<SingleBarCodeLoadingVerificationDto> _validationSingleBarCodeLoadingVerificationDtoRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationSingleBarCodeLoadingVerificationDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public SingleBarCodeLoadingVerificationService(AbstractValidator<SingleBarCodeLoadingVerificationDto> validationSingleBarCodeLoadingVerificationDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationSingleBarCodeLoadingVerificationDtoRules = validationSingleBarCodeLoadingVerificationDtoRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 单体条码上料校验
        /// </summary>
        /// <param name="singleBarCodeLoadingVerificationDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SingleBarCodeLoadingVerificationAsync(SingleBarCodeLoadingVerificationDto singleBarCodeLoadingVerificationDto)
        {
            await _validationSingleBarCodeLoadingVerificationDtoRules.ValidateAndThrowAsync(singleBarCodeLoadingVerificationDto);
            throw new NotImplementedException();
        }
    }
}
