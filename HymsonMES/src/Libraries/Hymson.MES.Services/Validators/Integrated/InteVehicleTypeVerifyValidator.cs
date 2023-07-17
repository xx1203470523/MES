/*
 *creator: Karl
 *
 *describe: 载具类型验证    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-13 03:15:22
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Validators.Integrated
{
    /// <summary>
    /// 载具类型验证 更新 验证
    /// </summary>
    internal class InteVehicleTypeVerifyCreateValidator: AbstractValidator<InteVehicleTypeVerifyCreateDto>
    {
        public InteVehicleTypeVerifyCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Type).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18513));
            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(VehicleTypeVerifyTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES18514));
            RuleFor(x => x.VerifyId).Must(it => it>0).WithErrorCode(nameof(ErrorCode.MES18515));

        }
    }

}
