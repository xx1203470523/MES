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
            //RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19214));
            //RuleFor(x => x.).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19214));
            RuleFor(x => x.WarehouseShelfId).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19215));
        }
    }

}
