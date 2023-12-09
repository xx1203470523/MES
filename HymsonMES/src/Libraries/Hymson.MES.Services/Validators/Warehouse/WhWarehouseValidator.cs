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
            RuleFor(x => x.Name).MaximumLength(10).WithErrorCode(nameof(ErrorCode.MES19202));
        }
    }

}
