using FluentValidation;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
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
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manuSfcRepository"></param>
        /// <param name="validationSingleBarCodeLoadingVerificationDtoRules"></param>
        /// <param name="currentEquipment"></param>
        public SingleBarCodeLoadingVerificationService(IManuSfcRepository manuSfcRepository, AbstractValidator<SingleBarCodeLoadingVerificationDto> validationSingleBarCodeLoadingVerificationDtoRules, ICurrentEquipment currentEquipment)
        {
            _validationSingleBarCodeLoadingVerificationDtoRules = validationSingleBarCodeLoadingVerificationDtoRules;
            _currentEquipment = currentEquipment;

            _manuSfcRepository = manuSfcRepository;
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
            var manuSfcEntit = await _manuSfcRepository.GetBySFCAsync(new GetBySFCQuery { SFC = singleBarCodeLoadingVerificationDto.SFC, SiteId = _currentEquipment.SiteId });
            if (manuSfcEntit == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19113)).WithData("SFC", singleBarCodeLoadingVerificationDto.SFC);
            }
        }
    }
}
