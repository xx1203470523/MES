using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Quality;

namespace Hymson.MES.Services.Validators.Quality
{
    /// <summary>
    /// OQC检验单 验证
    /// </summary>
    internal class QualOqcOrderSaveValidator : AbstractValidator<QualOqcOrderSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public QualOqcOrderSaveValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.ShipmentDetailIds).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10111));
        }
    }

}
