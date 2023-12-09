using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.WhWarehouseRegion;
using Hymson.Utils;

namespace Hymson.MES.Services.Validators.WhWarehouseRegion
{
    /// <summary>
    /// 库区 验证
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
            RuleFor(x => x.WarehouseId).NotEmpty().WithErrorCode(nameof(ErrorCode.MES19201));
        }
    }

}
