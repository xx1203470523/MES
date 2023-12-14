using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.WhWareHouse;

namespace Hymson.MES.Services.Validators.WhWareHouse
{
    /// <summary>
    /// 仓库 验证
    /// </summary>
    internal class WhWarehouseSaveValidator: AbstractValidator<WhWarehouseSaveDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseSaveValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19201));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19202));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
        }
    }

    internal class WhWarehouseModifyValidator : AbstractValidator<WhWarehouseModifyDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public WhWarehouseModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19202));
            RuleFor(x => x.Status).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19219));
            RuleFor(x => x.Id).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19220));
        }
    }

}
