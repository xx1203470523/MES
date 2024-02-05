using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.WhWarehouseLocation;

namespace Hymson.MES.Services.Validators.WhWarehouseLocation
{
    /// <summary>
    /// 库位 验证
    /// </summary>
    internal class WhWarehouseLocationSaveValidator: AbstractValidator<WhWarehouseLocationSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseLocationSaveValidator()
        {
            RuleFor(x => x.WarehouseShelfCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19215));
            RuleFor(x => x.WarehouseCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19210));
            RuleFor(x => x.WarehouseRegionCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19211));
            //RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19222));
            RuleFor(x => x.Code).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.WarehouseShelfCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.WarehouseCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.WarehouseRegionCode).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES10109));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));
        }
    }


    internal class WhWarehouseLocationModifyValidator : AbstractValidator<WhWarehouseLocationModifyDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseLocationModifyValidator()
        {
            //RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
            RuleFor(x => x.Id).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19222));
            RuleFor(x => x.Remark).MaximumLength(255).WithErrorCode(nameof(ErrorCode.MES10121));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
        }
    }
}
