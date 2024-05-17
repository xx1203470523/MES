using FluentValidation;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Services.Dtos.Process;

namespace Hymson.MES.Services.Validators.Process
{
    /// <summary>
    /// 设备参数组 更新 验证
    /// </summary>
    internal class ProcEquipmentGroupParamCreateValidator: AbstractValidator<ProcEquipmentGroupParamCreateDto>
    {
        public ProcEquipmentGroupParamCreateValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));
            RuleFor(x => x.Code).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18704));
            RuleFor(x => x.Code).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18703));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18705));
            RuleFor(x => x.Code).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18706));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18707));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18715));
            RuleFor(x => x.Version).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18717));
            RuleFor(x => x.Version).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18716));

            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(EquipmentGroupParamTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES18710));
            RuleFor(x => x.ProductId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18718));
            RuleFor(x => x.ProcedureId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18719));
            RuleFor(x => x.EquipmentGroupId).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18720));
        }
    }

    /// <summary>
    /// 设备参数组 修改 验证
    /// </summary>
    internal class ProcEquipmentGroupParamModifyValidator : AbstractValidator<ProcEquipmentGroupParamModifyDto>
    {
        public ProcEquipmentGroupParamModifyValidator()
        {
            RuleFor(x => x).NotEmpty().WithErrorCode(nameof(ErrorCode.MES10100));

            RuleFor(x => x.Id).Must(it => it > 0).WithErrorCode(nameof(ErrorCode.MES18712));
            RuleFor(x => x.Name).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18705));
            RuleFor(x => x.Name).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18707));
            RuleFor(x => x.Version).NotEmpty().WithErrorCode(nameof(ErrorCode.MES18715));
            RuleFor(x => x.Version).Must(x => !x.Any(x => Char.IsWhiteSpace(x))).WithErrorCode(nameof(ErrorCode.MES18717));
            RuleFor(x => x.Version).MaximumLength(100).WithErrorCode(nameof(ErrorCode.MES18716));

            RuleFor(x => x.Type).Must(it => Enum.IsDefined(typeof(EquipmentGroupParamTypeEnum), it)).WithErrorCode(nameof(ErrorCode.MES18710));
            RuleFor(x => x.ProductId).Must(it => it>0).WithErrorCode(nameof(ErrorCode.MES18718));
            RuleFor(x => x.ProcedureId).Must(it => it>0).WithErrorCode(nameof(ErrorCode.MES18719));
            RuleFor(x => x.EquipmentGroupId).Must(it => it>0).WithErrorCode(nameof(ErrorCode.MES18720));
        }
    }
}
