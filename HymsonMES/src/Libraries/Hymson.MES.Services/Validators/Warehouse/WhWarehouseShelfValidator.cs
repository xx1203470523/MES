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
            RuleFor(x => x.WarehouseId).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19210));
            RuleFor(x => x.WarehouseRegionId).NotEmpty().NotEqual(0).WithErrorCode(nameof(ErrorCode.MES19211));
        }
    }
}
