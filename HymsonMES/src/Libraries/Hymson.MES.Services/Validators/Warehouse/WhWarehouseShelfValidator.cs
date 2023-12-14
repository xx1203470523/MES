using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.WhWarehouseShelf;

namespace Hymson.MES.Services.Validators.WhWarehouseShelf
{
    /// <summary>
    /// 货架 验证
    /// </summary>
    internal class WhWarehouseShelfSaveValidator: AbstractValidator<WhWarehouseShelfSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseShelfSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19208));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19209));
            RuleFor(x => x.WarehouseCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19210));
            RuleFor(x => x.WarehouseRegionCode).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19211));
        }
    }

    /// <summary>
    /// 货架 验证
    /// </summary>
    internal class WhWarehouseShelfModifyValidator : AbstractValidator<WhWarehouseShelfModifyDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseShelfModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19209));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19210));
            RuleFor(x => x.Row).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19221));
            RuleFor(x => x.Column).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19221));
            RuleFor(x => x.Id).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19220));
        }
    }
}
