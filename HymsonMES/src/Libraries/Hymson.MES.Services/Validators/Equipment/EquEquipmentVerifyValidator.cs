/*
 *creator: Karl
 *
 *describe: 设备验证    验证规则 | 代码由框架生成  
 *builder:  Karl
 *build datetime: 2023-07-28 09:02:39
 */
using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Services.Dtos.Equipment;

namespace Hymson.MES.Services.Validators.Equipment
{
    /// <summary>
    /// 设备验证 更新 验证
    /// </summary>
    internal class EquEquipmentVerifyCreateValidator: AbstractValidator<EquEquipmentVerifyCreateDto>
    {
        public EquEquipmentVerifyCreateValidator()
        {
            RuleFor(x => x.Account).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12610));
            RuleFor(x => x.Account).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES12612));
            RuleFor(x => x.Account).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12613));
            RuleFor(x => x.Password).NotEmpty().WithErrorCode(nameof(ErrorCode.MES12611));
            RuleFor(x => x.Password).MaximumLength(50).WithErrorCode(nameof(ErrorCode.MES12614));
        }
    }
}
