using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.Utils;

namespace Hymson.MES.Services.Validators.WhWarehouseRegion
{
    /// <summary>
    /// 库区验证
    /// </summary>
    internal class WhWarehouseRegionSaveValidator: AbstractValidator<WhWarehouseRegionSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseRegionSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19204));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19205));
            //RuleFor(x => x.WarehouseId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19201));
            RuleFor(x => x.WarehouseCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19201));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10110));
        }
    }

    /// <summary>
    /// 库区验证
    /// </summary>
    internal class WhWarehouseRegionModifyValidator : AbstractValidator<WhWarehouseRegionModifyDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseRegionModifyValidator()
        {
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19205));
            RuleFor(x => x.Id).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19220));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));
            RuleFor(x => x.Name).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10110));
        }
    }
}
