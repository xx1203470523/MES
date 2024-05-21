using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Validators.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    internal class ManusBarcodeSplitAdjustValidator : AbstractValidator<ManuBarCodeSplitRequestDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public ManusBarcodeSplitAdjustValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12800));
            RuleFor(x => x.SFC).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12801));
            RuleFor(x => x.Qty).Must(x=>x>0).WithErrorCode(nameof(ErrorCode.MES12828));
        }
    }
}
