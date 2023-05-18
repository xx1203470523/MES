using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices.Dtos.SingleBarCodeLoadingVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Validators.SingleBarCodeLoadingVerification
{
    /// <summary>
    ///单体条码上料校验
    /// </summary>
    internal class SingleBarCodeLoadingVerificationValidator : AbstractValidator<SingleBarCodeLoadingVerificationDto>
    {
        public SingleBarCodeLoadingVerificationValidator()
        {
            //RuleFor(x => x.EquipmentCode).NotEmpty().WithErrorCode(ErrorCode.MES19001);
            //RuleFor(x => x.ResourceCode).NotEmpty().WithErrorCode(ErrorCode.MES19002);
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(ErrorCode.MES19003);
        }
    }
}
