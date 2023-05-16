using FluentValidation;
using Hymson.MES.EquipmentServices.Request.SingleBarCodeLoadingVerification;
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
        private readonly AbstractValidator<SingleBarCodeLoadingVerificationRequest> _validationSingleBarCodeLoadingVerificationRequestRules;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationSingleBarCodeLoadingVerificationRequestRules"></param>
        /// <param name="currentEquipment"></param>
        public SingleBarCodeLoadingVerificationService(AbstractValidator<SingleBarCodeLoadingVerificationRequest> validationSingleBarCodeLoadingVerificationRequestRules, ICurrentEquipment currentEquipment)
        {
            _validationSingleBarCodeLoadingVerificationRequestRules = validationSingleBarCodeLoadingVerificationRequestRules;
            _currentEquipment = currentEquipment;
        }

        /// <summary>
        /// 单体条码上料校验
        /// </summary>
        /// <param name="singleBarCodeLoadingVerificationRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SingleBarCodeLoadingVerificationAsync(SingleBarCodeLoadingVerificationRequest singleBarCodeLoadingVerificationRequest)
        {
            await _validationSingleBarCodeLoadingVerificationRequestRules.ValidateAndThrowAsync(singleBarCodeLoadingVerificationRequest);
            throw new NotImplementedException();
        }
    }
}
